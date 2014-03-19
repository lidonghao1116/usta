using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    public class Norm
    {
        public Norm()
        {
        }

        public int normId
        {
            set;
            get;
        }
        public String name
        {
            set;
            get;
        }
        public int parentId
        {
            set;
            get;
        }
        public String comment
        {
            set;
            get;
        }
        public int type
        {
            set;
            get;
        }
        public string year
        {
            set;
            get;

        }
        public List<Norm> children
        {
            set;
            get;
        }
    }
}
