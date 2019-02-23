using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Koowoo.Data;
using Koowoo.Data.Interface;
using Koowoo.Core;
using System;
using System.Linq;

namespace Koowoo.Services
{
    public static class AutofacExt
    {
        private static IContainer _container;

        public static void InitAutofac()
        {
            var builder = new ContainerBuilder();

            //注册数据库基础操作和工作单元
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).PropertiesAutowired();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.Register<IDbContext>(c => new KoowooContext()).InstancePerLifetimeScope();

            Type baseType = typeof(IDependency);
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().ToArray();//获取已加载到此应用程序域的执行上下文中的程序集。
            Type[] dependencyTypes = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && p != baseType).ToArray();//得到接口和实现类
                                                                                     //   RegisterDependencyTypes(dependencyTypes);//第一步：注册类型

            builder.RegisterTypes(dependencyTypes)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();//PropertiesAutowired注册为属性注入类型，所有实现IDependency的注册为InstancePerLifetimeScope生命周期



            // 注册controller，使用属性注入
            builder.RegisterControllers(Assembly.GetCallingAssembly()).PropertiesAutowired();

            //注册所有的ApiControllers
            builder.RegisterApiControllers(Assembly.GetCallingAssembly()).PropertiesAutowired();

            builder.RegisterModelBinders(Assembly.GetCallingAssembly());
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            //builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // 注册所有的Attribute
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            _container = builder.Build();

            //Set the MVC DependencyResolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));

            //Set the WebApi DependencyResolver
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)_container);

            ContainerManager.SetContainer(_container);
        }


    }
}