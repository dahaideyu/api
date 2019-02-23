using Koowoo.Core;
using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq.Expressions;

namespace Koowoo.Data
{
    /// <summary>
    /// 数据库初始化
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<KoowooContext>
    {
        private readonly DateTime now = new DateTime(2018, 9, 7, 23, 22, 21);

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;//启用自动迁移
            AutomaticMigrationDataLossAllowed = false;//是否允许接受数据丢失的情况，false=不允许，会抛异常；true=允许，有可能数据会丢失
        }

        protected override void Seed(KoowooContext context)
        {
           // #region
           // var role = new RoleEntity { FId = Guid.NewGuid().ToString(), FName = "超级管理员", FRemark = "超级管理员", FSort = 1, FIsSystem = true, FEnable = true };

           // var roles = new List<RoleEntity> {
           //    role
           // };
           // #endregion

           // #region
           // var user = new UserEntity
           // {
           //     FId = Guid.NewGuid().ToString(),
           //     FUserName = "admin",
           //     FOrginzId = "",
           //     FEPhone = "13000000000",
           //     FEPhoto = "/Images/account.png",
           //     FEName = "超级管理员",
           //     FPassWord = Utils.MD5("admin123456"),
           //     FType = 1,
           //     FEStatus = "正常",
           //     FCreationTime = DateTime.Now,
           //     FEnable = true
           // };
           //// user.Roles.Add(role);

           // var users = new List<UserEntity>
           // {
           //     user
           // };
           // #endregion

            
           // #region 菜单
           // var system = new MenuEntity
           // {
           //     FId = Guid.NewGuid().ToString(),
           //     FName = "系统设置",
           //     FUrl = "#",
           //     FType = 1,
           //     FArea = "",
           //     FController = "",
           //     FAction = "fa-cog",
           //     FEnable = true,
           //     FIsManage = true,
           //     FSort = 1
           // };//1
           // var log = new MenuEntity
           // {
           //     FId = Guid.NewGuid().ToString(),
           //     FName = "日志查看",
           //     FUrl = "#",
           //     FType = 1,
           //     FArea = "Sys",
           //     FController = "User",
           //     FAction = "Index",
           //     FEnable = true,
           //     FIsManage = true,
           //     FSort = 2
           // };//2

           // var menuMgr = new MenuEntity
           // {
           //     FParentId = system.FId,
           //     FId = Guid.NewGuid().ToString(),
           //     FName = "菜单管理",
           //     FUrl = "/Sys/Menu/Index",
           //     FType = 2,
           //     FArea = "Sys",
           //     FController = "Menu",
           //     FAction = "Index",
           //     FIcon = "fa-cubes",
           //     FEnable = true,
           //     FIsManage = true,
           //     FSort = 1
           // };//3
           // var roleMgr = new MenuEntity
           // {
           //     FParentId = system.FId,
           //     FId = Guid.NewGuid().ToString(),
           //     FName = "角色管理",
           //     FUrl = "/Sys/Role/Index",
           //     FType = 2,
           //     FArea = "Sys",
           //     FController = "Role",
           //     FAction = "Index",
           //     FIcon = "fa-key",
           //     FEnable = true,
           //     FIsManage = true,
           //     FSort = 2
           // };//4
           // var userMgr = new MenuEntity
           // {
           //     FParentId = system.FId,
           //     FId = Guid.NewGuid().ToString(),
           //     FName = "用户管理",
           //     FUrl = "/Sys/User/Index",
           //     FType = 2,
           //     FArea = "Sys",
           //     FController = "User",
           //     FAction = "Index",
           //     FIcon = "fa-users",
           //     FEnable = true,
           //     FIsManage = true,
           //     FSort = 3
           // };//5

           // var orgMgr = new MenuEntity
           // {
           //     FParentId = system.FId,
           //     FId = Guid.NewGuid().ToString(),
           //     FName = "组织架构管理",
           //     FUrl = "/Sys/Organiz/Index",
           //     FType = 2,
           //     FArea = "Sys",
           //     FController = "Organiz",
           //     FAction = "Index",
           //     FIcon = "fa-building",
           //     FEnable = true,
           //     FIsManage = true,
           //     FSort = 4
           // };//6

           // var areaMgr = new MenuEntity
           // {
           //     FParentId = system.FId,
           //     FId = Guid.NewGuid().ToString(),
           //     FName = "区域管理",
           //     FUrl = "/Sys/Area/Index",
           //     FType = 2,
           //     FArea = "Sys",
           //     FController = "Area",
           //     FAction = "Index",
           //     FIcon = "fa-building",
           //     FEnable = true,
           //     FIsManage = true,
           //     FSort = 5
           // };//7

           // var dictMgr = new MenuEntity
           // {
           //     FParentId = system.FId,
           //     FId = Guid.NewGuid().ToString(),
           //     FName = "字典管理",
           //     FUrl = "/Sys/Dict/Index",
           //     FType = 2,
           //     FArea = "Sys",
           //     FController = "Dict",
           //     FAction = "Index",
           //     FIcon = "fa-book",
           //     FEnable = true,
           //     FIsManage = true,
           //     FSort = 6
           // };//8


           // //菜单
           // var menus = new List<MenuEntity>
           // {
           //     system,
           //     log,
           //     menuMgr,
           //     roleMgr,
           //     userMgr,
           //     orgMgr,
           //     areaMgr,
           //     dictMgr,
           //     new MenuEntity
           //     {
           //         FParentId = log.FId,
           //         FId = Guid.NewGuid().ToString(),
           //         FName = "操作日志",
           //         FUrl = "/Sys/Log/Index",
           //         FType = 2,
           //         FArea = "Sys",
           //         FController = "Log",
           //         FAction = "Index",
           //         FEnable = true,
           //            FIsManage = true,
           //         FSort = 1
           //     }
           // };
           // var menuBtns = GetMenuButtons(menuMgr.FId, "Menu");//13
           // var rolwBtns = GetMenuButtons(roleMgr.FId, "Role");//16
           // var userBtns = GetMenuButtons(userMgr.FId, "User");//19
           // var organizBtns = GetMenuButtons(orgMgr.FId, "Organiz");//22
           // var areaBtns = GetMenuButtons(areaMgr.FId, "Area");//19
           // var dictBtns = GetMenuButtons(dictMgr.FId, "Dict");//19

           // menus.AddRange(menuBtns);//23
           // menus.AddRange(rolwBtns);//26
           // menus.AddRange(userBtns);//29

           // menus.AddRange(organizBtns);//29
           // menus.AddRange(areaBtns);//29
           // menus.AddRange(dictBtns);//29

           // #endregion

           // AddOrUpdate(context, m => m.FUserName, users.ToArray());
           // AddOrUpdate(context, m => m.FName, roles.ToArray());          
        }


        #region Private

        /// <summary>
        /// 获取菜单的基础按钮
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        //List<MenuEntity> GetMenuButtons(string parentId, string controllerName)
        //{
            //string area = "Sys";
            //return new List<MenuEntity>
            //{
            //    new MenuEntity
            //    {
            //        FParentId = parentId,
            //        FId = Guid.NewGuid().ToString(),
            //        FName = "添加",
            //        FUrl = string.Format("/{0}/{1}/Add",area,controllerName),
            //        FArea = area,
            //        FController = controllerName,
            //        FAction = "Add",
            //        FIcon="fa-plus",
            //        FEnable = true,
            //           FIsManage = true,
            //        FType = 3,
            //        FSort = 1
            //    },
            //    new MenuEntity
            //    {
            //        FParentId = parentId,
            //        FId = Guid.NewGuid().ToString(),
            //        FName = "修改",
            //        FUrl = string.Format("/{0}/{1}/Edit",area,controllerName),
            //        FArea = area,
            //        FController = controllerName,
            //        FAction = "Edit",
            //        FEnable = true,
            //           FIsManage = true,
            //        FIcon="fa-edit",
            //        FType = 3,
            //        FSort = 2
            //    },
            //    new MenuEntity
            //    {
            //        FParentId = parentId,
            //        FId = Guid.NewGuid().ToString(),
            //        FName = "删除",
            //        FUrl = string.Format("/{0}/{1}/Delete",area,controllerName),
            //        FArea = area,
            //        FController = controllerName,
            //        FAction = "Delete",
            //        FIcon="fa-trash",
            //        FEnable = true,
            //           FIsManage = true,
            //        FType = 3,
            //        FSort = 3
            //    }
            //};
        //}

        /// <summary>
        /// 添加更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="exp"></param>
        /// <param name="param"></param>
        void AddOrUpdate<T>(DbContext context, Expression<Func<T, object>> exp, T[] param) where T : class
        {
            DbSet<T> set = context.Set<T>();
            set.AddOrUpdate(exp, param);
        }
        #endregion


    }
}
