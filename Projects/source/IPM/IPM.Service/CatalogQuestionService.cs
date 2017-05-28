using IPM.Business;
using IPM.Model.Models;
using System.Collections.Generic;
using System;


namespace IPM.Service
{
    public interface ICatalogQuestionService : IServiceBase<CatalogQuestion>
    {
       
    }
    public class CatalogQuestionService : ServiceBase<CatalogQuestion>, ICatalogQuestionService
    {
        private readonly ICatalogQuestionBusiness _catalogQuestionBusiness;

     
        public CatalogQuestionService(ICatalogQuestionBusiness catalogQuestionBusiness) :base(catalogQuestionBusiness)
        {
            this._catalogQuestionBusiness = catalogQuestionBusiness;
        }

    }
}
