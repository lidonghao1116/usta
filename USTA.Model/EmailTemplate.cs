using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USTA.Model
{
    /// <summary>
    /// 邮件模板实体类
    /// </summary>
    public class EmailTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public EmailTemplate()
        {

        }

        /// <summary>
        /// 模板类型，与EnumCollections下的EmailType相关联
        /// </summary>
        public int type
        {
            set;
            get;
        }

        /// <summary>
        /// 邮件模板标题
        /// </summary>
        public string title
        {
            set;
            get;
        }

        /// <summary>
        /// 邮件是否可修改
        /// </summary>
        public int isModify
        {
            set;
            get;
        }

        /// <summary>
        /// 邮件模板内容
        /// </summary>
        public string content
        {
            set;
            get;
        }
    }
}