using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class NormConfirm
    {
        public NormConfirm()
        {
        }
        public int id
        {
            set;
            get;
        }
        public string term
        {
            set;
            get;
        }
        public string teacherNo
        {
            set;
            get;
        }
        public string question
        {
            set;
            get;
        }
        public string answer
        {
            set;
            get;
        }
        public string value
        {
            set;
            get;
        }
        public int type
        {
            set;
            get;
        }
        public int isDelete
        {
            set;
            get;
        }
        public DateTime createTime
        {
            set;
            get;
        }

    }
}
