using System.Text;
using System.Threading.Tasks;
using Koowoo.Pojo.System;
using Koowoo.Domain.System;
using Koowoo.Core;
using Koowoo.Core.Pager;
using System.Linq;
using System;
using Koowoo.Pojo;
using System.Collections.Generic;
using Koowoo.Data;
using Koowoo.Core.Extentions;
using Koowoo.Data.Interface;
using Koowoo.Pojo.Request;

namespace Koowoo.Services.System
{
    public interface IRoleService
    {
        TableData GetList(QueryListReq req);
        void Create(RoleDto user);
        void Update(RoleDto user);
        void UpdateAuthen(RoleDto role);
        RoleDto GetById(int userId);
        void Delete(string ids);
    }

    public class RoleService : IRoleService, IDependency
    {
        private readonly IRepository<RoleEntity> _roleRopsitory;
        private readonly IRepository<MenuEntity> _menuRopsitory;

        public RoleService(IRepository<RoleEntity> roleRopsitory, IRepository<MenuEntity> menuRopsitory)
        {
            _roleRopsitory = roleRopsitory;
            _menuRopsitory = menuRopsitory;
        }

        public TableData GetList(QueryListReq req)
        {
            var query = _roleRopsitory.Table;
            query = query.Where(o => !o.Deleted);

            if (!string.IsNullOrEmpty(req.keyword))
            {
                query = query.Where(o => o.RoleName.Contains(req.keyword));
            }

            query = query.OrderBy(o => o.RoleID);
            var pagedList = query.ToPagedList(req.page, req.pageSize);
            return new TableData
            {
                currPage = req.page,
                pageSize = req.pageSize,
                pageTotal = pagedList.TotalPageCount,
                totalCount = pagedList.TotalItemCount,
                list = pagedList.MapToList<RoleDto>()
            };
        }

        public RoleDto GetById(int roleId)
        {
            var role = _roleRopsitory.GetById(roleId);
            if (role != null)
            {
                var roleDto = role.MapTo<RoleDto>();
                if (role.Menus != null)
                {
                    // var menus = role.Menus.Select(o => o.MenuID.ToString());
                    roleDto.MenuIds = string.Join(",", role.Menus.Select(o => o.MenuID));
                }
                // roleDto.Menus = role.Menus.MapToList<MenuDto>();

                return roleDto;
            }
            else
                return null;
        }

        public void Create(RoleDto role)
        {
            var entity = role.MapTo<RoleEntity>();
            entity.CreateTime = DateTime.Now;


            //var menuIdList = !string.IsNullOrEmpty(role.MenuIds) ? role.MenuIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            //menuIdList.ForEach(o =>
            //{
            //    var menu = _menuRopsitory.GetById(int.Parse(o));
            //    entity.Menus.Add(menu);
            //});
            _roleRopsitory.Insert(entity);
        }

        public void Update(RoleDto role)
        {
            var entity = _roleRopsitory.GetById(role.RoleID);
            entity.RoleName = role.RoleName;
            entity.Description = role.Description;
            entity.Status = role.Status;

            //if (entity.Menus != null)
            //{
            //    entity.Menus.Clear();
            //}

            //var menuIdList = !string.IsNullOrEmpty(role.MenuIds) ? role.MenuIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            //menuIdList.ForEach(o =>
            //{
            //    var menu = _menuRopsitory.GetById(int.Parse(o));
            //    entity.Menus.Add(menu);
            //});


            _roleRopsitory.Update(entity);
        }

        public void UpdateAuthen(RoleDto role)
        {
            var entity = _roleRopsitory.GetById(role.RoleID);

            if (entity.Menus != null)
            {
                entity.Menus.Clear();
            }

            var menuIdList = !string.IsNullOrEmpty(role.MenuIds) ? role.MenuIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            menuIdList.ForEach(o =>
            {
                var menu = _menuRopsitory.GetById(int.Parse(o));
                entity.Menus.Add(menu);
            });

            _roleRopsitory.Update(entity);
        }

        public void Delete(string ids)
        {
            var idList1 = ids.Trim(',').Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
            var entities = idList1.Select(id => _roleRopsitory.GetById(id));
            entities.ToList().ForEach(item =>
            {
                if (item.Menus != null)
                {
                    item.Menus.Clear();
                }
                _roleRopsitory.Delete(item);
            });
        }
    }
}
