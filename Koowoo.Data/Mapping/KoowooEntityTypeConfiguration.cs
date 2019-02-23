using System.Data.Entity.ModelConfiguration;

namespace Koowoo.Data.Mapping
{
    public abstract class KoowooEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected KoowooEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {
            
        }
    }
}