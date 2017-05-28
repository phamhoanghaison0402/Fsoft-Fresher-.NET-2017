using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IPM.Business
{
    public interface IRoomBusiness
    {
        IEnumerable<Room> GetAll();

        Room GetSingleById(int id);

        void Save();
    }

    public class RoomBusiness : IRoomBusiness
    {
        private readonly ILog log = LogManager.GetLogger("RoomBusiness.cs");
        private IRoomRepository _roomRepository;
        private IUnitOfWork _unitOfWork;

        public RoomBusiness(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
        {
            this._roomRepository = roomRepository;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void Save()
        {
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Get list room
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Room> GetAll()
        { 
            var list = _roomRepository.GetAll();
            if (list == null)
            {
                log.Warn("Not found data for room");
            }
            else
            {
                log.Info("List room has data");
            }
            return list;
        }

        /// <summary>
        /// Get room folow RoomID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Room GetSingleById(int id)
        {
            var room = _roomRepository.GetSingleById(id);

            if (room == null)
            {
                log.Warn("Not found data for room");
            }
            else
            {
                log.Info("Room has been found");
            }
            return room;
        }
    }
}
