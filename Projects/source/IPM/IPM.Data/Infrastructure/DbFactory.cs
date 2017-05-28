using System;

namespace IPM.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        /// <summary>
        /// Initial
        /// </summary>
        /// <returns></returns>
        IPMDbContext Init();
    }

    /// <summary>
    /// DbFactory
    /// </summary>
    public class DbFactory : Disposable, IDbFactory
    {
        private IPMDbContext dbContext;

        /// <summary>
        /// Init
        /// </summary>
        /// <returns></returns>
        public IPMDbContext Init()
        {
            return dbContext ?? (dbContext = new IPMDbContext());
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}