using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using Koowoo.Data.Mapping;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;
using Koowoo.Domain;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace Koowoo.Data
{


    public class KoowooContext : DbContext, IDbContext
    {
        public KoowooContext()
            : base("name=SqlServiceEntities")
        {

        }

        public KoowooContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除一对多的级联删除关系
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //移除表名复数形式
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            //   modelBuilder.Configurations.Add(new ConfigMap());
            //   modelBuilder.Configurations.Add(new DictMap());
            ////   modelBuilder.Configurations.Add(new DictTypeMap());
            //   modelBuilder.Configurations.Add(new UserTokenMap());
            //   modelBuilder.Configurations.Add(new RoleMap());
            //   modelBuilder.Configurations.Add(new MenuMap());
            //   modelBuilder.Configurations.Add(new UserMap());
            //   //modelBuilder.Configurations.Add(new UserRoleMap());
            //   //modelBuilder.Configurations.Add(new RoleMenuMap());
            //   modelBuilder.Configurations.Add(new AreaMap());


            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(KoowooEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

        //public virtual DbSet<UserTokenEntity> sysUserToken { get; set; }
        //public virtual DbSet<UserEntity> sysUser { get; set; }
        //public virtual DbSet<MenuEntity> sysMenu { get; set; }
        //public virtual DbSet<RoleEntity> sysRole { get; set; }
        //public virtual DbSet<DictEntity> sysDict { get; set; }
        //public virtual DbSet<ConfigEntity> sysConfig { get; set; }
        //public virtual DbSet<AreaEntity> bizArea { get; set; }
        //public virtual DbSet<UserRoleEntity> userRole { get; set; }
        //public virtual DbSet<RoleMenuEntity> roleMenu { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }


        public IEnumerable<DbEntityEntry> CurrentEntries()
        {
            return ChangeTracker.Entries();
        }
        //public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        //{
        //    if (parameters != null && parameters.Length > 0)
        //    {
        //        for (int i = 0; i <= parameters.Length - 1; i++)
        //        {
        //            var p = parameters[i] as DbParameter;
        //            if (p == null)
        //                throw new Exception("Not support parameter type");

        //            commandText += i == 0 ? " " : ", ";

        //            commandText += "@" + p.ParameterName;
        //            if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
        //            {
        //                //output parameter
        //                commandText += " output";
        //            }
        //        }
        //    }

        //    var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

        //    //performance hack applied as described here - http://www.nopcommerce.com/boards/t/25483/fix-very-important-speed-improvement.aspx
        //    bool acd = this.Configuration.AutoDetectChangesEnabled;
        //    try
        //    {
        //        this.Configuration.AutoDetectChangesEnabled = false;

        //        for (int i = 0; i < result.Count; i++)
        //            result[i] = AttachEntityToContext(result[i]);
        //    }
        //    finally
        //    {
        //        this.Configuration.AutoDetectChangesEnabled = acd;
        //    }
        //    return result;
        //}

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        //protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        //{
        //    //little hack here until Entity Framework really supports stored procedures
        //    //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
        //    var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
        //    if (alreadyAttached == null)
        //    {
        //        //attach new entity
        //        Set<TEntity>().Attach(entity);
        //        return entity;
        //    }

        //    //entity is already loaded
        //    return alreadyAttached;
        //}

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }
    }
}
