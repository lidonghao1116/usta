using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class SalaryQA
    {

        public SalaryQA() { 
        
        }

        public int salaryQaId
        {
            set;
            get;
        }

        public TeachersList teacher
        {
            set;
            get;
        }

        public int salaryId
        {
            set;
            get;
        }

        public string qaContent
        {
            set;
            get;
        }

        public int salaryType
        {
            set;
            get;
        }

        public DateTime createdTime
        {
            set;
            get;
        }
    }
}
