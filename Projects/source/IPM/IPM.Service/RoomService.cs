using IPM.Business;
using IPM.Model.Models;
using System.Collections.Generic;

namespace IPM.Service
{
    public interface IRoomService
    {
        IEnumerable<Room> GetAll();

        Room GetSingleById(int id);

        void Save();
    }

    public class RoomService : IRoomService
    {
        private IRoomBusiness _roomBusiness;

        public RoomService(IRoomBusiness roomBusiness)
        {
            _roomBusiness = roomBusiness;
        }

        /// <summary>
        /// Get all data room
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Room> GetAll()
        {
            return _roomBusiness.GetAll();
        }

        /// <summary>
        /// Get room flow RoomID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Room GetSingleById(int id)
        {
            return _roomBusiness.GetSingleById(id);
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save()
        {
            _roomBusiness.Save();
        }
    }
}