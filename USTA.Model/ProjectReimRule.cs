using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class ProjectReimRule
    {
        public int ruleId { 
            set;
            get;
        }

        public Project project
        {
            set;
            get;
        }

        public Reim reim
        {
            set;
            get;
        }

        public float reimValue
        {
            set;
            get;
        }

        public float maxReimValue
        {
            set;
            get;
        }

    }
}
