using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.Collections.Generic;

namespace IPM.Business
{
    public interface ICatalogQuestionBusiness : IBusinessBase<CatalogQuestion>
    {
        
    }
    public class CatalogQuestionBusiness : BusinessBase<CatalogQuestion>, ICatalogQuestionBusiness
    {
        private readonly ICatalogQuestionRepository _catalogQuestionRepository;
        private readonly IUnitOfWork _unitOfWork;

   
        public CatalogQuestionBusiness(ICatalogQuestionRepository catalogQuestionRepository, IUnitOfWork unitOfWork) : base(catalogQuestionRepository, unitOfWork)
        {
            this._catalogQuestionRepository = catalogQuestionRepository;
            this._unitOfWork = unitOfWork;
        }

    }
}
