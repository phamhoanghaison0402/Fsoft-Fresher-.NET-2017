namespace IPM.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private IPMDbContext dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public IPMDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}