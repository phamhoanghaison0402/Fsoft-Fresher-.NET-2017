using IPM.Model.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace IPM.Data
{
    public class IPMDbContext : DbContext
    {
        public IPMDbContext() : base("IPMConnection")
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }
        public virtual DbSet<AnswerQuestion> AnswerQuestions { set; get; }

        public virtual DbSet<Candidate> Candidates { set; get; }

        public virtual DbSet<CandidateCertificate> CandidateCertificates { set; get; }

        public virtual DbSet<CandidateSkill> CandidateSkills { set; get; }

        public virtual DbSet<Catalog> Catalogs { set; get; }

        public virtual DbSet<CatalogQuestion> CatalogQuestions { set; get; }

        public virtual DbSet<Document> Documents { set; get; }

        public virtual DbSet<Certificate> Certificates { set; get; }

        public virtual DbSet<Guideline> Guidelines { set; get; }

        public virtual DbSet<GuidelineCatalog> GuidelineCatalogs { set; get; }

        public virtual DbSet<Interview> Interviews { set; get; }

        public virtual DbSet<InterviewProcess> InterviewProcesses { set; get; }

        public virtual DbSet<InterviewAnswer> InterviewAnswers { set; get; }

        public virtual DbSet<InterviewRound> InterviewRounds { set; get; }

        public virtual DbSet<MeetingRequest> MeetingRequests { set; get; }

        public virtual DbSet<Position> Positions { set; get; }

        public virtual DbSet<Question> Questions { set; get; }

        public virtual DbSet<Role> Roles { set; get; }

        public virtual DbSet<Room> Rooms { set; get; }

        public virtual DbSet<RoundProcess> RoundProcesses { set; get; }

        public virtual DbSet<Skill> Skills { set; get; }

        public virtual DbSet<SystemParameter> SystemParameters { set; get; }

        public virtual DbSet<User> Users { set; get; }

        public virtual DbSet<UserRole> UserRoles { set; get; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            builder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}