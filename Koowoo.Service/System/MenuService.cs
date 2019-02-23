using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koowoo.Pojo.System;
using Koowoo.Domain.System;
using Koowoo.Core;
using Koowoo.Core.Extentions;
using Koowoo.Data.Interface;
using Koowoo.Pojo;

namespace Koowoo.Services.System
{
    public interface IMenuService
    {       
        void Create(MenuDto user);
        void Update(MenuDto user);
        MenuDto GetById(int userId);
        ResponseModel Delete(int menuId);
        List<MenuDto> GetAllMenuList(bool showHide = true);
        IList<MenuListDto> GetSelectList();
    }

    public class MenuService : IMenuService, IDependency
    {
        private readonly IRepository<MenuEntity> _menuRepository;

        public MenuService(IRepository<MenuEntity> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public MenuDto GetById(int menuId)
        {
            var entity = _menuRepository.GetById(menuId);
            if (entity != null)
            {
                var model = entity.ToModel();
                var parentMenu = _menuRepository.GetById(model.ParentID);
                model.ParentName = parentMenu?.MenuName ?? string.Empty;
                return model;
            }
            else
                return null;
        }

        public void Create(MenuDto model)
        {
            var entity = model.ToEntity();
            entity.CreateTime = DateTime.Now;
            _menuRepository.Insert(entity);
        }

        public void Update(MenuDto model)
        {
            var entity = _menuRepository.GetById(model.MenuID);
            entity = model.ToEntity(entity);
            _menuRepository.Update(entity);
        }

        public ResponseModel Delete(int menuId)
        {
            var parentList = _menuRepository.Table.Where(a => a.ParentID == menuId).ToList();
            if (parentList != null && parentList.Count > 0)
            {
                return new ResponseModel() { code = 1, msg = "请删除下级菜单后在删除" };
            }
            _menuRepository.Delete(p => p.MenuID == menuId);
            return new ResponseModel() { code = 0, msg = "success" };
        }

        public List<MenuDto> GetAllMenuList(bool showHide =true)
        {
          var query = _menuRepository.TableNoTracking ;
            query = query.Where(a => !a.Deleted);
            if (!showHide)
            {
                query = query.Where(a => a.Active == true);
            }         
            return query.MapToList<MenuDto>();
        }

        public IList<MenuListDto> GetSelectList()
        {
            List<MenuDto> myMenus = new List<MenuDto>();
            myMenus = _menuRepository.TableNoTracking.Where(a => !a.Deleted && a.MenuType!=2).ToList().MapToList<MenuDto>();

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
                foreach (var p in rolePermissions.Where(t => t.ParentID == parentId)
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
