using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class NewOld
    {
        public NewOld(string oldNo,string newNo,string classId,string term)
        {
            this.oldcourseNo = oldNo;
            this.newcourseNo = newNo;
            this.classId = classId;
            this.termtag = term;
        }
        public string oldcourseNo
        {
            set;
            get;
        }
        public string newcourseNo
        {
            set;
            get;
        }
        public string classId
        {
            set;
            get;
        }
        public string termtag
        {
            set;
            get;
        }
    }
}
