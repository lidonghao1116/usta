using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// 实体类：SalaryEntry，对应教师的一条薪酬记录
    /// </summary>
    public class SalaryEntry
    {
        #region
        public SalaryEntry()
        { }
        #endregion

        #region
        public int salaryEntryId
        {
            set;
            get;
        }

        public TeachersList teacher
        {
            set;
            get;
        }

        public Courses course
        {
            set;
            get;
        }

        public int teacherType
        {
            set;
            get;
        }

        public string atCourseType
        {
            set;
            get;
        }

        public float salaryInAdjustFactor
        {
            set;
            get;
        }

        public float salaryOutAdjustFactor
        {
            set;
            get;
        }

        public int teachPeriod
        {
            set;
            get;
        }

        public int teachAssiPeriod
        {
            set;
            get;
        }

        public string termTag
        {
            set;
            get;
        }

        public string salaryMonth
        {
            set;
            get;
        }
        /// <summary>
        /// 教师薪酬中不含税部分
        /// </summary>
        public float teacherCostWithoutTax
        {
            set;
            get;
        }

        /// <summary>
        /// 教师薪酬中含税部分
        /// </summary>
        public float teacherCostWithTax
        {
            set;
            get;
        }

        /// <summary>
        /// 教师薪酬中扣除税后的总和
        /// </summary>
        public float teacherTotalCost
        {

            set;
            get;
        }

        public int salaryEntryStatus
        {
            set;
            get;
        }

        public string memo
        {
            set;
            get;
        }

        public DateTime createdTime
        {
            set;
            get;
        }

        private List<SalaryItemElement> salaryInItemElements;
        private List<SalaryItemElement> salaryOutItemElements;

        private string salaryInItemValueList;
        private string salaryOutItemValueList;
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

        /// <summary>
        /// 设置salaryOutItemElements，并重新计算salaryOutItemValueList信息
        /// </summary>
        /// <param name="salaryOutItemElements"></param>
        public void SetSalaryOutItemElements(List<SalaryItemElement> salaryOutItemElements) 
        {
            this.salaryOutItemElements = salaryOutItemElements;
            this.salaryOutItemValueList = ReBuildSalaryOutItems();
        }

        private string ReBuildSalaryOutItems()
        {
            return ReBuildSalaryItemsCommon(this.salaryOutItemElements);
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
        /// 返回salaryOutItemElements信息
        /// </summary>
        /// <returns></returns>
        public List<SalaryItemElement> GetSalaryOutItemElements()
        {
            return this.salaryOutItemElements;
        }
        /// <summary>
        /// 设置salaryInItemValueList，并重新计算SalaryInItemElements信息
        /// </summary>
        /// <param name="salaryInItemValueList"></param>
        public void SetSalaryInItemValueList(string salaryInItemValueList, bool isRebuildElemetns) 
        {
            this.salaryInItemValueList = salaryInItemValueList;
            if (isRebuildElemetns) {
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
        /// <summary>
        /// 设置salaryOutItemValueList，并重新计算SalaryOutItemElements信息
        /// </summary>
        /// <param name="salaryOutItemValueList"></param>
        public void SetSalaryOutItemValueList(string salaryOutItemValueList, bool isRebuildElements)
        {
            this.salaryOutItemValueList = salaryOutItemValueList;
            if (isRebuildElements) {
                ReBuildSalaryOutItemElements();
            }
        }

        private void ReBuildSalaryOutItemElements()
        {
            this.salaryOutItemElements = ReBuildSalaryItemElementCommon(this.salaryOutItemValueList);
        }
        /// <summary>
        /// 返回salaryOutItemValueList信息
        /// </summary>
        /// <returns></returns>
        public string GetSalaryOutItemValueList()
        {
            return this.salaryOutItemValueList;
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
                        element.adjustFactor = float.Parse(itemValueElements[2]);
                        element.itemCost = element.salaryStandard * element.times * element.adjustFactor;
                        element.itemCost = float.Parse(string.Format("{0:0.00}", element.itemCost));

                        salaryItemElements.Add(element);
                    }
                }
            }
            return salaryItemElements;
        }
    }
}
