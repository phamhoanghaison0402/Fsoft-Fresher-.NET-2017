using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.Collections.Generic;

namespace IPM.Business
{
    /// <summary>
    /// interface of GuidelineCatalogBusiness
    /// </summary>
    public interface IGuidelineCatalogBusiness : IBusinessBase<GuidelineCatalog>
    {
    }

    /// <summary>
    /// class GuidelineCatalogBusiness implement BusinessBase, IGuidelineCatalogBusiness
    /// </summary>
    public class GuidelineCatalogBusiness : BusinessBase<GuidelineCatalog>, IGuidelineCatalogBusiness
    {
        private IGuidelineCatalogRepository _guidelineCatalogRepository;
        private IUnitOfWork _unitOfWork;
        public GuidelineCatalogBusiness(IGuidelineCatalogRepository guidelineCatalogRepository, IUnitOfWork unitOfWork)
            : base(guidelineCatalogRepository, unitOfWork)
        {
            this._guidelineCatalogRepository = guidelineCatalogRepository;
            this._unitOfWork = unitOfWork;
        }
    }
}