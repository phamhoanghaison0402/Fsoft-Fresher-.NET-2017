using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Data.Infrastructure;
using IPM.Model.Models;

namespace IPM.Data.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        
    }
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
