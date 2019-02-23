using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koowoo.Domain.System;
using Koowoo.Data;
using Koowoo.Pojo;
using Koowoo.Pojo.System;
using System.Web.Security;
using System.Web;
using Koowoo.Core;
using Koowoo.Core.Pager;
using Koowoo.Core.Extentions;
using Koowoo.Data.Interface;
using Koowoo.Pojo.Request;
using Koowoo.Domain;

namespace Koowoo.Services.System
{
    public interface IUserService {
        IList<MenuListDto> GetMyMenus(int userId);
        TableData GetList(QueryListReq req);
        ResponseModel Create(UserDto user);
        void Update(UserDto user);
        UserDto GetById(int userId);
        void Delete(string ids);
        ResponseModel<UserDto> Login(string username, string userpass);
        void SignIn(UserDto user);
        void Logout();
        List<MenuDto> GetMenusByUserId(int userId);
        ResponseModel ChangePwd(int userId, string oldPassword, string newPassword);
        IList<string> GetMyPermissions(int userId);
        ResponseModel ResetPwd(int userId, string newPassword);
    }

    public class UserService : IUserService, IDependency
    {
        private readonly IRepository<MenuEntity> _menuRepository;
        private readonly IRepository<RoleEntity> _roleRepository;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<AreaEntity> _areaRepository;

        public UserService(IRepository<UserEntity> userRepository,IRepository<MenuEntity> menuRepository,
            IRepository<RoleEntity> roleRepository,IRepository<AreaEntity> areaRepository)
        {
            _userRepository = userRepository;
            _menuRepository = menuRepository;
            _roleRepository = roleRepository;
            _areaRepository = areaRepository;
        }

        public TableData GetList(QueryListReq req)
        {
            var query = _userRepository.Table;
            query = query.Where(o => !o.Deleted);

            if (!string.IsNullOrEmpty(req.keyword))
            {
                query = query.Where(o => o.UserName.Contains(req.keyword));
            }

            query = query.OrderByDescending(o => o.UserID);
            var pagedList = query.ToPagedList(req.page,req.pageSize);

            var list = new List<UserDto>();

            foreach (var user in pagedList.ToList())
            {
                UserDto userDto = user.MapTo<UserDto>();
                if (user.Roles != null)
                {
                    userDto.RoleIds = string.Join(",", user.Roles.Select(o => o.RoleID));
                    userDto.RoleNames = string.Join(",", user.Roles.Select(o => o.RoleName));
                }
                else
                {
                    userDto.RoleIds = "";
                    userDto.RoleNames = "";
                }
                userDto.Password = "";
                var commuity = _areaRepository.GetById(user.CommunityUUID);
                userDto.CommunityName = commuity!=null?commuity.ChineseName:"";
                list.Add(userDto);
            }


            return new TableData
            {
                currPage = req.page,
                pageSize = req.pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = list
            };
        }

        public UserDto GetById(int userId)
        {
            var entity = _userRepository.GetById(userId);
            if (entity != null)
            {
                var userDto = entity.ToModel();
                if (entity.Roles != null)
                {
                    userDto.RoleIds = string.Join(",", entity.Roles.Select(o => o.RoleID));
                    userDto.RoleNames = string.Join(",", entity.Roles.Select(o => o.RoleName));
                }
                else
                {
                    userDto.RoleIds = "";
                    userDto.RoleNames = "";
                }
                userDto.Password = "";
                var commuity = _areaRepository.GetById(entity.CommunityUUID);
                userDto.CommunityName = commuity != null ? commuity.ChineseName : "";

                return userDto;
            }
            else
                return null;
        }

        public ResponseModel Create(UserDto user)
        {
            var result = new ResponseModel();
            var existUser = _userRepository.Table.Where(a => a.UserName == user.UserName).FirstOrDefault();
            if (existUser != null)
            {
                result.code = 1;
                result.msg = "用户名已存在";
                return result;
            }
            var entity = user.MapTo<UserEntity>();
            entity.Salt = Utils.MakeRandomString(6);
            entity.Password = Utils.MD5(user.Password + entity.Salt);
            entity.CreateTime = DateTime.Now;
            entity.Deleted = false;
      
            var roleIdList = !string.IsNullOrEmpty(user.RoleIds) ? user.RoleIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            roleIdList.ForEach(o =>
            {
                var role = _roleRepository.GetById(int.Parse(o));
                entity.Roles.Add(role);
            });

            _userRepository.Insert(entity);
            return result;         
        }

        public void Update(UserDto user)
        {
            var entity = _userRepository.GetById(user.UserID);           
            entity.Email = user.Email;
            entity.Remark = user.Remark;
            entity.Mobile = user.Mobile;
            entity.Status = user.Status;
            entity.CommunityUUID = user.CommunityUUID;
            if(!user.Password.IsBlank())
                entity.Password = Utils.MD5(user.Password + entity.Salt);

            if (entity.Roles != null)
            {
                entity.Roles.Clear();
            }
        
            var roleIdList = !string.IsNullOrEmpty(user.RoleIds) ? user.RoleIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            roleIdList.ForEach(o =>
            {
                var role = _roleRepository.GetById(int.Parse(o));
                entity.Roles.Add(role);
            });
            _userRepository.Update(entity);
        }

        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();

            var entities = idList1.Select(id => _userRepository.GetById(id));

            entities.ToList().ForEach(item =>
            {
                if (item.Roles != null)
                {
                    item.Roles.Clear();
                }
                _userRepository.Delete(item);
            });

                 
        }


        public ResponseModel<UserDto> Login(string username, string userpass)
        {
            var res = new ResponseModel<UserDto>();
            try
            {
                res.code = 1;
                var user = _userRepository.TableNoTracking.Where(u => u.UserName == username && !u.Deleted).FirstOrDefault();
                if (user == null)
                {
                    res.msg = "用户不存在";
                }
                else
                {
                    string password = Utils.MD5(userpass + user.Salt);
                    if (user.Password != password)
                        res.msg = "登录密码错误";
                    else if (user.Deleted)
                        res.msg = "用户已被删除";
                    else if (user.Status == 0)
                        res.msg = "账号未被激活";
                    else if (user.Status == 2)
                        res.msg = "账号被禁用";
                    else
                    {
                        res.code = 0;
                        res.msg = "登录成功";
                        res.data = user.MapTo<UserDto>();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                Log.Error(null, ex.Message);
                res.code = 9999;
                res.msg = "系统异常";
                return res;
            }
        }

        public void SignIn(UserDto user)
        {
            //写入注册信息
            DateTime expiration = DateTime.Now.AddDays(7);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2,
                user.UserName,
                DateTime.Now,
                expiration,
                true,
                user.UserID.ToString(),
                FormsAuthentication.FormsCookiePath);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Expires = expiration
            };

#if !DEBUG
                cookie.Domain = FormsAuthentication.CookieDomain;
#endif
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public ResponseModel ChangePwd(int userId, string oldPassword, string newPassword)
        {
            var res = new ResponseModel();

            var model = _userRepository.GetById(userId);
            if (model != null)
            {
                if (model.Password != Utils.MD5(oldPassword + model.Salt))
                {
                    res.code = 1;
                    res.msg = "原密码错误";
                }
                else
                {
                    string password = Utils.MD5(newPassword + model.Salt);
                    model.Password = password;
                    _userRepository.Update(model);
                    res.code = 0;
                    res.msg = "修改密码成功";
                }
            }
            return res;
        }


        public ResponseModel ResetPwd(int userId, string newPassword)
        {
            var res = new ResponseModel();
            var model = _userRepository.GetById(userId);
            if (model != null)
            {
                string password = Utils.MD5(newPassword + model.Salt);
                model.Password = password;
                _userRepository.Update(model);
            }

            res.code = 0;
            res.msg = "修改密码成功";
            return res;
        }

        public IList<string> GetMyPermissions(int userId)
        {
            List<MenuDto> myMenus = new List<MenuDto>();
            if (userId == 1)
            {
                myMenus = _menuRepository.Table.Where(a=>!a.Deleted).ToList().MapToList<MenuDto>();                
            }
            else
            {
                myMenus = GetMenusByUserId(userId);
            }

            if (myMenus == null)
                return null;
            else
                return myMenus.Where(o => o.Perms != null).Select(a => a.Perms).ToList();
        }

        public List<MenuDto> GetMenusByUserId(int userId)
        {
            UserEntity sysUser = _userRepository.GetById(userId);
            var menuList = sysUser.Roles.SelectMany(o => o.Menus).ToList();
            return menuList.MapToList<MenuDto>();
        }

        public IList<MenuListDto> GetMyMenus(int userId)
        {
            List<MenuDto> myMenus = new List<MenuDto>();
            if (userId == 1) //超级管理员
                myMenus = _menuRepository.Table.Where(a=>!a.Deleted).ToList().MapToList<MenuDto>();
            else
                myMenus = GetMenusByUserId(userId);

            var menuList = SortMenuForTree(0, myMenus);
            return menuList;
        }

        /// <summary>
        /// 菜单节点
        /// </summary>
        /// <param name="parentId">父节点</param>
        /// <returns></returns>
        private List<MenuListDto> SortMenuForTree(int parentId, IList<MenuDto> rolePermissions)
        {
            var model = new List<MenuListDto>();
            if (rolePermissions != null)
            {
                foreach (var p in rolePermissions.Where(t => t.ParentID == parentId && t.MenuType != 2)
               .OrderBy(t => t.OrderNum))
                {
                    var menu = new MenuListDto
                    {
                        MenuID = p.MenuID,
                        MenuName = p.MenuName,
                        MenuUrl = p.MenuUrl,
                        MenuIcon = p.MenuIcon
                    };
                    menu.Children.AddRange(SortMenuForTree(p.MenuID, rolePermissions));
                    model.Add(menu);
                }

            }
            return model;
        }
    }
}
