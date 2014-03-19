using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace USTA.Controls
{
    /**/
    /// <summary>
    /// 自定义Repeater　支持EmptyDataTemplate
    /// 作者:cantops
    /// </summary>
    public class Repeater : System.Web.UI.WebControls.Repeater
    {
        /// <summary>
        /// 模板参数
        /// </summary>
        private ITemplate emptyDataTemplate;

        [PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(TemplateControl))]
        public ITemplate EmptyDataTemplate
        {
            get { return emptyDataTemplate; }
            set { emptyDataTemplate = value; }
        }

        /// <summary>
        /// 重写DataBinding事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnDataBinding(EventArgs e)
        {

            base.OnDataBinding(e);
            if (emptyDataTemplate != null)
            {
                if (this.Items.Count == 0)
                {
                    EmptyDataTemplate.InstantiateIn(this);
                }
            }
        }

    }


}
