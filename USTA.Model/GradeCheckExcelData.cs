using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class GradeCheckExcelData
    {
        /// <summary>
        /// 
        /// </summary>
        public GradeCheckExcelData()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public List<StudentsGradeCheckConfirm> listStudentsGradeCheckConfirm
        {
            set;
            get;
        }


        /// <summary>
        /// 
        /// </summary>
        public List<StudentsGradeCheckDetail> listStudentsGradeCheckDetail
        {
            set;
            get;
        }

    }
}
