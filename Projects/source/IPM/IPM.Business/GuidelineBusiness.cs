using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.Collections.Generic;

namespace IPM.Business
{
    /// <summary>
    /// Interface of Guideline
    /// </summary>
    public interface IGuidelineBusiness : IBusinessBase<Guideline>
    {
    

        bool ChangeActive(int id);

        IEnumerable<GuidelineCatalog> GetCatalog(int idguideline);

        IEnumerable<Catalog> AvailableCatalog(int idguideline);
    }

    /// <summary>
    ///Class of Guideline implement BusinessBase and IGuidelineBusiness
    /// </summary>
    public class GuidelineBusiness : BusinessBase<Guideline>, IGuidelineBusiness
    {
        private readonly IGuidelineRepository _guidelineRepository;
        private readonly IUnitOfWork _unitOfWork;

     
        public GuidelineBusiness(IGuidelineRepository guidelineRepository, IUnitOfWork unitOfWork) : base(guidelineRepository, unitOfWork)
        {
            this._guidelineRepository = guidelineRepository;
            this._unitOfWork = unitOfWork;
        }

        public override IEnumerable<Guideline> GetAll()
        {
            return _guidelineRepository.Get(x => x.Active == true);
        }

        public bool ChangeActive(int id)
        {
            var guideline = _guidelineRepository.GetSingleById(id);
            guideline.Active = false;
            _guidelineRepository.Update(guideline);
            return true;
        }

        public IEnumerable<GuidelineCatalog> GetCatalog(int idguideline)
        {
            return _guidelineRepository.GetCatalog(idguideline);
        }

        public IEnumerable<Catalog> AvailableCatalog(int idguideline)
        {
            return _guidelineRepository.AvailableCatalog(idguideline);
        }
    }
}