using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// Interface of Catalogrepository.
    /// </summary>
    public interface ICatalogRepository : IRepository<Catalog>
    {
        /// <summary>
        ///  Declare function GetAll catalog by guideline id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<int> GetAll(int id);
        /// <summary>
        /// Declare function GetQuestion by catalog id.
        /// </summary>
        /// <param name="idcatalog"></param>
        /// <returns></returns>
        IEnumerable<CatalogQuestion> GetQuestion(int idcatalog);
        /// <summary>
        /// Declare function AvailableQuestion.
        /// </summary>
        /// <param name="idcatalog"></param>
        /// <returns></returns>
        IEnumerable<Question> AvailableQuestion(int idcatalog);
    }

    /// <summary>
    /// Declare class CatalogRepository implement RepositoryBase, ICatalogRepository.
    /// </summary>
    public class CatalogRepository : RepositoryBase<Catalog>, ICatalogRepository
    {
        /// <summary>
        /// Constructor CatalogRepository.
        /// </summary>
        /// <param name="dbFactory"></param>
        public CatalogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }


        /// <summary>
        /// Implement function GetAll Catalog of this Guideline via guideline id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetAll(int id)
        {
            var idlist = (from gc in DbContext.GuidelineCatalogs
                where gc.GuidelineID == id
                select gc.CatalogID).ToList();

            return idlist;
        }

        /// <summary>
        /// Implement function get GetQuestion by catalog id.
        /// </summary>
        /// <param name="idcatalog"></param>
        /// <returns></returns>
        public IEnumerable<CatalogQuestion> GetQuestion(int idcatalog)
        {
            var query = (from catalogQuestion in DbContext.CatalogQuestions
                where catalogQuestion.CatalogID == idcatalog
                select catalogQuestion).OrderBy(x => x.ID);

            return query;
        }

        /// <summary>
        /// Implement function get AvailableQuestion. 
        /// </summary>
        /// <param name="idcatalog"></param>
        /// <returns></returns>
        public IEnumerable<Question> AvailableQuestion(int idCatalog)
        {
            var queryquestion = from    question           in      DbContext.Questions
                                join    catalogQuestion    in      DbContext.CatalogQuestions 
                                on      question.ID        equals  catalogQuestion.QuestionID
                                where   catalogQuestion.CatalogID == idCatalog
                                select  question;
            var listQuestion = (from    question            in      DbContext.Questions
                                where   !(queryquestion.Any(x => x.ID == question.ID))
                                select  question).OrderBy(x => x.ID);

            return listQuestion;
        }

    }
}