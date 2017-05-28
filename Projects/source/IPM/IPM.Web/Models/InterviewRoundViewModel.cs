using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPM.Web.Models
{

    /// <summary>
    /// View model for InterviewRound
    /// </summary>
    public class InterviewRoundViewModel
    {

        public int ID
        {
            get;

            set;
        }


        public string RoundName
        {
            get;

            set;
        }


        public string Description
        {
            get;

            set;
        }


        public int GuidelineID
        {
            get;

            set;
        }


        public bool Active
        {
            get;

            set;
        }

    }
}