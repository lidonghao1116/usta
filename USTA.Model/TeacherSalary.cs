using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// 教师每个学期开始时为其设置的教学薪酬总值
    /// </summary>
    public class TeacherSalary
    {
        #region 构造方法
        public TeacherSalary() 
        { }
        #endregion

        #region
        /// <summary>
        /// teacherSalaryId字段，用于标识主键
        /// </summary>
        public int teacherSalaryId
        {
            set;
            get;
        }
        /// <summary>
        /// TeacherSalary关联的教师信息
        /// </summary>
        public TeachersList teacher
        {
            set;
            get;
        }

        /// <summary>
        /// TeacherSalary关联的课程信息
        /// </summary>
        public Courses course
        {
            set;
            get;
        }
        /// <summary>
        /// 在该学期老师与课程的关系：教师或助教
        /// </summary>
        public int atCourseType
        {
            set;
            get;
        }

        /// <summary>
        /// 教师类型：院内教师/助教, 院外教师， 院外助教
        /// </summary>
        public int teacherType
        {
            set;
            get;
        }
        
        /// <summary>
        /// 当前学期，教师的理论总课时
        /// </summary>
        public int teachPeriod
        {
            set;
            get;
        }
        /// <summary>
        /// 当前学期，课程的实验课时
        /// </summary>
        public int experPeriod
        {
            set;
            get;
        }
       
        /// <summary>
        /// 当前学期，一位老师薪酬的预算总额
        /// </summary>
        public float totalTeachCost
        {
            set;
            get;
        }
        /// <summary>
        /// 学期标识
        /// </summary>
        public string termTag
        {
            set;
            get;
        }

        /// <summary>
        /// 是否已被教师确认
        /// </summary>
        public bool isConfirm
        {
            set;
            get;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string memo
        {
            set;
            get;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdTime
        {
            set;
            get;
        }

        private List<SalaryItemElement> salaryInItemElements;
        private string salaryInItemValueList;
        #endregion

        /// <summary>
        /// 设置salaryInItemElements的值，每次重设置此值是，需要执行
        /// ReBuildSalaryInItems方法重要计算salaryInItemList和salaryInItemValue;
        /// </summary>
        /// <param name="salaryInItemElements"></param>
        public void SetSalaryInItemElements(List<SalaryItemElement> salaryInItemElements)
        {
            this.salaryInItemElements = salaryInItemElements;
            this.salaryInItemValueList = ReBuildSalaryInItems();
        }

        /// <summary>
        /// 当设置后salaryInItemElements后，需要根据最新的salaryInItemElements的值
        /// 重新为salaryInItemList和salaryInItemValue设置新值
        /// </summary>
        private string ReBuildSalaryInItems()
        {
            return ReBuildSalaryItemsCommon(this.salaryInItemElements);
        }
        /// <summary>
        /// 返回salaryInItemElements;
        /// </summary>
        /// <returns></returns>
        public List<SalaryItemElement> GetSalaryInItemElements()
        {
            return this.salaryInItemElements;
        }

        private string ReBuildSalaryItemsCommon(List<SalaryItemElement> salaryItemElements)
        {
            string salaryItemValueList = "";
            if (salaryItemElements != null && salaryItemElements.Count > 0)
            {

                foreach (SalaryItemElement element in salaryItemElements)
                {
                    string salaryValue = element.salaryItemId + ":" + element.salaryStandard + "," + element.times + "," + element.adjustFactor + ";";
                    salaryItemValueList += salaryValue;
                }

                salaryItemValueList = salaryItemValueList.Substring(0, salaryItemValueList.Length - 1);
            }

            return salaryItemValueList;

        }
      
        /// <summary>
        /// 设置salaryInItemValueList，并重新计算SalaryInItemElements信息
        /// </summary>
        /// <param name="salaryInItemValueList"></param>
        public void SetSalaryInItemValueList(string salaryInItemValueList, bool isRebuildElemetns)
        {
            this.salaryInItemValueList = salaryInItemValueList;
            if (isRebuildElemetns)
            {
                ReBuildSalaryInItemElements();
            }
        }

        private void ReBuildSalaryInItemElements()
        {
            this.salaryInItemElements = ReBuildSalaryItemElementCommon(this.salaryInItemValueList);
        }
        /// <summary>
        /// 返回salaryInItemValueList信息
        /// </summary>
        /// <returns></returns>
        public string GetSalaryInItemValueList()
        {
            return this.salaryInItemValueList;
        }

        private List<SalaryItemElement> ReBuildSalaryItemElementCommon(string salaryItemValueList)
        {
            List<SalaryItemElement> salaryItemElements = null;

            if (salaryItemValueList != null && salaryItemValueList.Trim().Length > 0)
            {
                string[] salaryItemValues = salaryItemValueList.Split(';');
                if (salaryItemValues.Length > 0)
                {
                    salaryItemElements = new List<SalaryItemElement>();
                    int length = salaryItemValues.Length;
                    for (int i = 0; i < length; i++)
                    {
                        SalaryItemElement element = new SalaryItemElement();
                        string[] itemValue = salaryItemValues[i].Split(':');
                        element.salaryItemId = itemValue[0];
                        string[] itemValueElements = itemValue[1].Split(',');
                        element.salaryStandard = float.Parse(itemValueElements[0]);
                        element.times = float.Parse(itemValueElements[1]);
                        element.itemCost = float.Parse(itemValueElements[2]);
                        element.MonthNum = int.Parse(itemValueElements[3]);
                        element.hasTax = int.Parse(itemValueElements[4]) == 1;
                        salaryItemElements.Add(element);
                    }
                }
            }
            return salaryItemElements;
        }
    }
}
