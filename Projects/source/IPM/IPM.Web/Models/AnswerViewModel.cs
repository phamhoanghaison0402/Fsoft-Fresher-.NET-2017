using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using IPM.Model.Models;
using IPM.Service;

namespace IPM.Web.Models
{
    public class AnswerQuestionViewModel
    {
        public int ID { get; set; }

        public string CandidateAnswer { get; set; }

        public string Comment { get; set; }

        public Question Question { get; set; }

        public bool Active { get; set; }
    }

    public class InterviewAnswerViewModel
    {
        public int ID { get; set; }

        public int Mark { get; set; }

        public Catalog Catalog { get; set; }

        public  ICollection<AnswerQuestionViewModel> ListAnswerQuestions { get; set; }

        public bool Active { get; set; }

        public int RowSpan { get; set; }
    }

    public class InterviewViewModel
    {
        public int ID { get; set; }

        public DateTime StartTime { get; set; }

        public bool? Result { get; set; }

        public string Record { get; set; }

        public RoundProcess RoundProcess { get; set; }

        public User Interviewer { get; set; }

        public User InterviewAdmin { get; set; }

        public ICollection<InterviewAnswerViewModel> ListInterviewAnswers { get; set; }
    }

    public class InterviewListViewModel
    {
        public Candidate Candidate { get; set; }

        public ICollection<InterviewViewModel> Interviews {get; set; }

    }

}