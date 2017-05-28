using IPM.Business;
using IPM.Model.Models;
using System.Collections.Generic;
using System;
using log4net;

namespace IPM.Service
{
    public interface IPositionService : IServiceBase<Position>
    {
        /// <summary>
        /// Check: Code is exist
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
    /// class inhenritance position service interface
    /// </summary>
    public class PositionService : ServiceBase<Position>, IPositionService
    {
        private IPositionBusiness _positionBusiness;
        public PositionService(IPositionBusiness positionBusiness) : base (positionBusiness)
        {
            this._positionBusiness = positionBusiness;
        }

        /// <summary>
        /// check code position not same
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CheckCode(string code)
        {
            return _positionBusiness.CheckCode(code);  
        }

        /// <summary>
        /// check name emplement interface
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckName(string name)
        {
            return _positionBusiness.CheckName(name);
        }

        /// <summary>
        /// validate code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ValidateCode(string code)
        {
            return _positionBusiness.ValidateCode(code);
        }

        /// <summary>
        /// validate name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ValidateName(string name)
        {
            return _positionBusiness.ValidateName(name);
        }
    }
}
