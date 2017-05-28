using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IPM.Business
{
    public interface IPositionBusiness : IBusinessBase<Position>
    {
        /// <summary>
        /// Check: Code is existed
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckCode(string code);

        /// <summary>
        /// check name is existed
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool CheckName(string name);

        /// <summary>
        /// validate code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool ValidateCode(string code);
        
        /// <summary>
        /// Validate name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ValidateName(string name);
    }

    /// <summary>
    /// class inhenritance interface position business
    /// </summary>
    public class PositionBusiness : BusinessBase<Position>, IPositionBusiness
    {
        private IPositionRepository _positionRepository;
        private IUnitOfWork _unitOfWork;
        public PositionBusiness(IPositionRepository positionRepository, IUnitOfWork unitOfWork) 
            : base (positionRepository, unitOfWork)
        {
            this._positionRepository = positionRepository;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// delete position
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       override public Position Delete(int id)
        {
            var positionDb = _positionRepository.GetSingleById(id);

            positionDb.Active = false;
            _positionRepository.Update(positionDb);

            return positionDb;
        }

        /// <summary>
        /// get all position  in present
        /// </summary>
        /// <returns></returns>
        override public IEnumerable<Position> GetAll()
        {          
            return _positionRepository.Get(x => x.Active == true);
        }

        /// <summary>
        /// Check: code is exist
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CheckCode(string code)
        {
            return _positionRepository.CheckCode(code);
        }

        /// <summary>
        /// method check name is existed
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckName(string name)
        {
            return _positionRepository.CheckName(name);
        }

        /// <summary>
        /// validate code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ValidateCode(string code)
        {
            Regex rgx = new Regex(@"^[A-Z0-9]{1,20}$");
            return rgx.IsMatch(code);
        }
        
        /// <summary>
        /// validate name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ValidateName(string name)
        {
            Regex rgx = new Regex(@"^([A-Za-zAÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶEÉÈẺẼẸÊẾỀỂỄỆIÍÌỈĨỊOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢUÚÙỦŨỤƯỨỪỬỮỰYÝỲỶỸỴĐaáàảãạâấầẩẫậăắằẳẵặeéèẻẽẹêếềểễệiíìỉĩịoóòỏõọôốồổỗộơớờởỡợuúùủũụưứừửữựyýỳỷỹỵđ]{1})+([A-Za-z0-9 AÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶEÉÈẺẼẸÊẾỀỂỄỆIÍÌỈĨỊOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢUÚÙỦŨỤƯỨỪỬỮỰYÝỲỶỸỴĐaáàảãạâấầẩẫậăắằẳẵặeéèẻẽẹêếềểễệiíìỉĩịoóòỏõọôốồổỗộơớờởỡợuúùủũụưứừửữựyýỳỷỹỵđ]){0,49}$");
            return rgx.IsMatch(name);
        }
    }
}
