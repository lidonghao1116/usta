using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

using USTA.Model;
using USTA.Dal;
using USTA.Common;
using USTA.Bll;
using System.Transactions;

namespace USTA.WebApplication.Administrator
{
    public partial class EnrollManage : System.Web.UI.Page
    {
        public string pageIndex = HttpContext.Current.Request["page"] == null ? string.Empty : HttpContext.Current.Request["page"];

        public string gameCategoryId = HttpContext.Current.Request["gameCategoryId"] == null ? string.Empty : HttpContext.Current.Request["gameCategoryId"];

        public string gameTypeId = HttpContext.Current.Request["gameTypeId"] == null ? string.Empty : HttpContext.Current.Request["gameTypeId"];

        public int _gameCategoryId = 0;

        public int _gameTypeId = 0;

        public int _pageIndex = 1;

        //控制Tab的显示
        string fragmentFlag = "1";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CommonUtility.SafeCheckByParams<String>(pageIndex, ref _pageIndex))
            {
                Javascript.GoHistory(-1, "参数有误：（", Page);
                return;
            }

            if (Request["fragment"] != null)
            {
                fragmentFlag = Request["fragment"];
            }

            CommonUtility.ShowLiControl(fragmentFlag, liFragment1, liFragment2, liFragment3, divFragment1, divFragment2, divFragment3);

            if (fragmentFlag.Equals("1"))
            {
                if (!IsPostBack)
                {
                    DataBindGameCategoryAndType(ddlEnrollGameCategory, ddlEnrollGameType);
                }
            }

            if (fragmentFlag.Equals("2"))
            {
                if (!IsPostBack)
                {
                    DataBindGameCategoryAndType(ddlGameCategory, ddlGameType);
                    DataListBindEnrollList();

                    ddlGameCategory.Attributes.Add("onchange", "location.href='/Administrator/EnrollManage.aspx?fragment=2&page=" + pageIndex + "&gameCategoryId='+$('#ddlGameCategory').val()+'&gameTypeId='+$('#ddlGameType').val();");
                    ddlGameType.Attributes.Add("onchange", "location.href='/Administrator/EnrollManage.aspx?fragment=2&page=" + pageIndex + "&gameCategoryId='+$('#ddlGameCategory').val()+'&gameTypeId='+$('#ddlGameType').val();");
                }
            }


            if (fragmentFlag.Equals("3"))
            {
                if (!IsPostBack)
                {
                    DataBindGameCategoryAndType(ddlDrawGameCategory, ddlDrawGameType);
                    DataListBindGroupNum();
                }
            }
        }

        //第1个标签；开始
        protected void ddlEnrollGameCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();

            while (ddlEnrollGameType.Items.Count > 0)
            {
                ddlEnrollGameType.Items.RemoveAt(0);
            }
            DalOperationAboutGameType dalGameType = new DalOperationAboutGameType();

            DataTable dtGameType = dalGameType.GetListByGameCategoryIdAndSex(int.Parse(ddlEnrollGameCategory.SelectedValue), user.Sex == "男" ? "1" : "2").Tables[0];

            for (int i = 0; i < dtGameType.Rows.Count; i++)
            {
                ddlEnrollGameType.Items.Add(new ListItem(dtGameType.Rows[i]["gameTypeTitle"].ToString().Trim(), dtGameType.Rows[i]["gameTypeId"].ToString().Trim()));
            }

            //检查是否有活动届次和活动类型数据

            if (!(ddlEnrollGameCategory.Items.Count > 0 && ddlEnrollGameType.Items.Count > 0))
            {
                Javascript.AlertAndRedirect("当前暂无需要报名的活动届次信息：（", "/Administrator/EnrollManage.aspx?fragment=2", Page);
                return;
            }
        }

        //绑定组别
        protected void DataListBindGroupNum()
        {
            //检查是否有活动届次和活动类型数据

            if (!(ddlDrawGameCategory.Items.Count > 0 && ddlDrawGameType.Items.Count > 0))
            {
                Javascript.GoHistory(-1, "当前暂无活动届次和活动类型信息：（", Page);
                return;
            }

            DalOperationAboutGameDrawList dal = new DalOperationAboutGameDrawList();
            DataTable dt = dal.GetGroupNumList(int.Parse(ddlDrawGameCategory.SelectedValue), int.Parse(ddlDrawGameType.SelectedValue)).Tables[0];

            this.dlstGroupNum.DataSource = dt;
            this.dlstGroupNum.DataBind();
        }

        //依据组别绑定组号信息
        protected void dlstGroupNum_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataList dataList = (DataList)e.Item.FindControl("dlstGroupIndex");
                DataRowView rowv = (DataRowView)e.Item.DataItem;
                string groupNum = rowv["groupNum"].ToString().Trim();
                int gameCategoryId = int.Parse(rowv["gameCategoryId"].ToString().Trim());
                int gameTypeId = int.Parse(rowv["gameTypeId"].ToString().Trim());
                DalOperationAboutGameDrawList dal = new DalOperationAboutGameDrawList();
                DataSet ds = dal.GetGroupIndexList(groupNum, gameCategoryId, gameTypeId);
                dataList.DataSource = ds.Tables[0].DefaultView;
                dataList.DataBind();
            }
        }


        //第1个标签；开始
        protected void ddlDrawGameCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (ddlDrawGameType.Items.Count > 0)
            {
                ddlDrawGameType.Items.RemoveAt(0);
            }

            DalOperationAboutGameType dal = new DalOperationAboutGameType();

            DataTable dt = dal.GetGameTypeByGameCategoryId(int.Parse(ddlDrawGameCategory.SelectedValue)).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlDrawGameType.Items.Add(new ListItem(dt.Rows[i]["gameTypeTitle"].ToString().Trim(), dt.Rows[i]["gameTypeId"].ToString().Trim()));
            }

            DataListBindGroupNum();
        }


        //第1个标签；开始
        protected void ddlDrawGameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataListBindGroupNum();
        }


        public void DataBindGameCategoryAndType(DropDownList ddlGameCategory, DropDownList ddlGameType)
        {
            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();

            DalOperationAboutGameCategory dalGameCategory = new DalOperationAboutGameCategory();

            DataTable dtGameCategory = (ddlGameCategory.ID == "ddlEnrollGameCategory" ? dalGameCategory.GetGameCategoryIng(DateTime.Now).Tables[0] : dalGameCategory.GetList().Tables[0]);

            for (int i = 0; i < dtGameCategory.Rows.Count; i++)
            {
                ddlGameCategory.Items.Add(new ListItem(dtGameCategory.Rows[i]["gameTitle"].ToString().Trim(), dtGameCategory.Rows[i]["gameCategoryId"].ToString().Trim()));
            }

            //检查是否有活动届次数据

            if (ddlGameCategory.Items.Count == 0)
            {
                Javascript.AlertAndRedirect("当前暂无需要报名的活动届次信息：（", "/Administrator/EnrollManage.aspx?fragment=2", Page);
                return;
            }

            if (CommonUtility.SafeCheckByParams<String>(gameCategoryId, ref _gameCategoryId))
            {
                for (int i = 0; i < ddlGameCategory.Items.Count; i++)
                {
                    if (ddlGameCategory.Items[i].Value == gameCategoryId)
                    {
                        ddlGameCategory.SelectedIndex = i;
                        break;
                    }
                }
            }

            DalOperationAboutGameType dalGameType = new DalOperationAboutGameType();

            DataTable dtGameType = (ddlGameType.ID == "ddlEnrollGameType" || ddlGameType.ID == "ddlGameType" ? dalGameType.GetListByGameCategoryIdAndSex(int.Parse(ddlGameCategory.SelectedValue), user.Sex == "男" ? "1" : "2").Tables[0] : dalGameType.GetGameTypeByGameCategoryId(int.Parse(ddlGameCategory.SelectedValue)).Tables[0]);

            for (int i = 0; i < dtGameType.Rows.Count; i++)
            {
                ddlGameType.Items.Add(new ListItem(dtGameType.Rows[i]["gameTypeTitle"].ToString().Trim(), dtGameType.Rows[i]["gameTypeId"].ToString().Trim()));
            }

            if (CommonUtility.SafeCheckByParams<String>(gameTypeId, ref _gameCategoryId))
            {
                for (int i = 0; i < ddlGameType.Items.Count; i++)
                {
                    if (ddlGameType.Items[i].Value == gameTypeId)
                    {
                        ddlGameType.SelectedIndex = i;
                        break;
                    }
                }
            }

            //检查是否有活动届次和活动类型数据

            if (!(ddlGameCategory.Items.Count > 0 && ddlGameType.Items.Count > 0))
            {
                Javascript.AlertAndRedirect("当前暂无需要报名的活动届次信息：（", "/Administrator/EnrollManage.aspx?fragment=2", Page);
                return;
            }

        }

        //绑定信息
        public void DataListBindEnrollList()
        {
            //检查是否有活动届次和活动类型数据

            if (!(ddlGameCategory.Items.Count > 0 && ddlGameType.Items.Count > 0))
            {
                Javascript.GoHistory(-1, "当前暂无活动届次和活动类型信息：（", Page);
                return;
            }

            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutGameEnrollList doan = new DalOperationAboutGameEnrollList();
            DataTable dt = doan.GetListByTeacherNo_GameCategoryId_GameTypeId(user.userNo, int.Parse(ddlGameCategory.SelectedValue), int.Parse(ddlGameType.SelectedValue)).Tables[0];

            this.AspNetPager1.RecordCount = dt.Rows.Count;
            AspNetPager1.PageSize = CommonUtility.pageSize;

            PagedDataSource pds = new PagedDataSource();    //定义一个PagedDataSource类来执行分页功      
            pds.DataSource = dt.DefaultView;
            pds.AllowPaging = true;
            if (_pageIndex == 0)
            {
                _pageIndex = 1;
            }

            pds.CurrentPageIndex = _pageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;

            this.dlEnroll.DataSource = pds;
            this.dlEnroll.DataBind();

            if (pds.Count == 0)
            {
                this.dlEnroll.ShowFooter = true;
            }
            else
            {
                this.dlEnroll.ShowFooter = false;
            }
        }


        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            if (fragmentFlag == "2")
            {
                DataListBindEnrollList();
            }
        }

        protected void dlEnroll_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbtnDraw = (LinkButton)e.Item.FindControl("lbtnDraw");
                Literal ltlNoDraw = (Literal)e.Item.FindControl("ltlNoDraw");
                Literal ltlDraw = (Literal)e.Item.FindControl("ltlDraw");
                Literal ltlCloseDraw = (Literal)e.Item.FindControl("ltlCloseDraw");

                DataRowView rowv = (DataRowView)e.Item.DataItem;
                string gameDrawListId = rowv["gameDrawListId"].ToString().Trim();
                string isOpenDraw = rowv["isOpenDraw"].ToString().Trim();

                if (string.IsNullOrEmpty(isOpenDraw))
                {
                    ltlCloseDraw.Visible = true;
                }
                else if (isOpenDraw == "0")
                {
                    ltlCloseDraw.Visible = true;
                }

                if (string.IsNullOrEmpty(gameDrawListId))
                {
                    ltlNoDraw.Visible = true;
                    if (isOpenDraw == "1")
                    {
                        lbtnDraw.Visible = true;
                    }
                }
                else
                {
                    ltlDraw.Visible = true;
                }
            }
        }

        protected void dlEnroll_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();

                DalOperationAboutGameDrawList dal = new DalOperationAboutGameDrawList();
                if (e.CommandName == "draw")
                {
                    int gameCategoryId = int.Parse(e.CommandArgument.ToString().Split(",".ToCharArray())[0]);
                    int gameTypeId = int.Parse(e.CommandArgument.ToString().Split(",".ToCharArray())[1]);

                    DalOperationAboutGameCategory dalgc = new DalOperationAboutGameCategory();
                    DataTable _dt = dalgc.CheckIsOpenDrawByGameCategoryId(gameCategoryId).Tables[0];
                    //首先判断当前届次和活动类型是否已经开放抽签
                    if (_dt.Rows.Count == 0)
                    {
                        Javascript.GoHistory(-1, "参数错误：（", Page);
                        return;
                    }

                    if (_dt.Rows[0]["isOpenDraw"].ToString().Trim() != "1")
                    {
                        Javascript.GoHistory(-1, "当前活动届次和活动类型暂未开放抽签：（", Page);
                        return;
                    }

                    //其次判断是否已经抽过签
                    if (dal.Exists(user.userNo, gameCategoryId, gameTypeId) > 0)
                    {
                        Javascript.GoHistory(-1, "当前活动届次和活动类型已经抽过签，请勿重复抽签哟：）", Page);
                        return;
                    }
                    //使用事务进行抽签控制
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            DalOperationAboutGameType dalgt = new DalOperationAboutGameType();
                            //首先获取组容量
                            int _groupCapability = int.Parse(dalgt.GetGroupCapabilityByGameTypeId(gameCategoryId, gameTypeId).Tables[0].Rows[0]["groupCapability"].ToString().Trim());
                            //其次获取总报名人数
                            DalOperationAboutGameEnrollList dalel = new DalOperationAboutGameEnrollList();
                            int _enrollListCount = int.Parse(dalel.GetEnrollListCountByGameCategoryIdAndGameTypeId(gameCategoryId, gameTypeId).Tables[0].Rows[0][0].ToString().Trim());

                            //获取分组数
                            int groupMod = _enrollListCount % _groupCapability;

                            int groupCount = 0;

                            if (groupMod == 0)
                            {
                                groupCount = _enrollListCount / _groupCapability;
                            }
                            else
                            {
                                groupCount = (_enrollListCount / _groupCapability) + 1;
                            }

                            //先判断需要几个字母
                            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                            //所需要使用的字母个数
                            string usedCharacters = characters.Substring(0, groupCount);
                            //Response.Write(usedCharacters.Length);

                            List<string> listGroupNumAndIndex = new List<string>();

                            List<string> listNotUsedGroupNumAndIndex = new List<string>();

                            for (int i = 0; i < usedCharacters.Length; i++)
                            {
                                for (int j = 0; j < _groupCapability; j++)
                                {
                                    //最后一组编号需要连续，因此需要进行特殊处理
                                    if (groupMod != 0)
                                    {
                                        //访问到最后一组
                                        if (i == usedCharacters.Length - 1)
                                        {
                                            if (groupMod >= j + 1)
                                            {
                                                //Response.Write(usedCharacters.Substring(i, 1) + (j + 1).ToString() + "ss<br/>");
                                                listGroupNumAndIndex.Add(usedCharacters.Substring(i, 1) + (j + 1).ToString());
                                            }
                                        }
                                        else
                                        {
                                            listGroupNumAndIndex.Add(usedCharacters.Substring(i, 1) + (j + 1).ToString());
                                        }
                                    }
                                    else
                                    {
                                        //Response.Write(usedCharacters.Substring(i, 1) + (j + 1).ToString() + "aa<br/>");
                                        listGroupNumAndIndex.Add(usedCharacters.Substring(i, 1) + (j + 1).ToString());
                                    }
                                }
                            }
                            //Response.Write(listGroupNumAndIndex.Count + "<br/>");

                            //获取已经使用的编号列表
                            DataTable _dtGroupNumAndIndex = dal.GetGroupNumAndIndexByGameCategoryIdAndGameTypeId(gameCategoryId, gameTypeId).Tables[0];

                            List<string> listGroupNumAndIndexCopy = new List<string>();

                            foreach (string _item in listGroupNumAndIndex)
                            {
                                //Response.Write(_item + "<br/>");
                                listGroupNumAndIndexCopy.Add(_item);
                            }

                            foreach (string _item in listGroupNumAndIndex)
                            {
                                for (int j = 0; j < _dtGroupNumAndIndex.Rows.Count; j++)
                                {
                                    if (_dtGroupNumAndIndex.Rows[j]["groupNumAndIndex"].ToString().Trim() == _item)
                                    {
                                        //Response.Write(_item + "<br/>");
                                        //移除已使用的编号
                                        listGroupNumAndIndexCopy.Remove(_item);
                                    }
                                }
                            }

                            //Response.Write(listGroupNumAndIndexCopy.Count + "<br/>");

                            //为使效果更随机，再次对编号数组使用随机方法打乱
                            while (listGroupNumAndIndexCopy.Count > 0)
                            {
                                Random random = new Random();
                                int _index = int.Parse(random.NextDouble().ToString().Substring(2, 4)) % listGroupNumAndIndexCopy.Count;

                                listNotUsedGroupNumAndIndex.Insert(0, listGroupNumAndIndexCopy[_index]);

                                listGroupNumAndIndexCopy.RemoveAt(_index);
                            }

                            //Response.Write(groupMod + "groupMod<br/>");

                            //Response.Write(groupCount + "groupCount<br/>");

                            //Response.Write(listNotUsedGroupNumAndIndex.Count + "listNotUsedGroupNumAndIndex<br/>");

                            Random random1 = new Random();


                            int _index1 = int.Parse(random1.NextDouble().ToString().Substring(2, 4)) % listNotUsedGroupNumAndIndex.Count;
                            //Response.Write(_index1 + "<br/>");
                            //Response.End();

                            //得到随机取到的编号值
                            string _result = string.Empty;

                            _result = listNotUsedGroupNumAndIndex[_index1];

                            if (string.IsNullOrEmpty(_result))
                            {
                                Javascript.GoHistory(-1, "抽签失败：（，请重试", Page);
                                return;
                            }

                            dal.Add(new GameDrawList { gameCategoryId = gameCategoryId, gameTypeId = gameTypeId, groupIndex = int.Parse(_result.Substring(1, 1)), groupNum = _result.Substring(0, 1), updateTime = DateTime.Now, teacherNo = user.userNo });
                            scope.Complete();
                            Javascript.AlertAndRedirect("抽签成功！", "/Administrator/EnrollManage.aspx?fragment=2&page=" + pageIndex + "&gameCategoryId=" + gameCategoryId + "&gameTypeId=" + gameTypeId, Page);
                        }
                        catch (System.Exception ex)
                        {
                            MongoDBLog.LogRecord(ex);
                            Javascript.GoHistory(-1, "抽签失败：（，请重试", Page);
                        }
                    }
                }
            }
        }

        protected int GenerateRandom(int end)
        {
            Random rdm = new Random(9832745);
            return rdm.Next(end);
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <returns></returns>
        protected string GetTableTd()
        {
            return string.Empty;
        }

        protected void btnEnroll_Click(object sender, EventArgs e)
        {
            UserCookiesInfo user = BllOperationAboutUser.GetUserCookiesInfo();
            DalOperationAboutGameEnrollList dal = new DalOperationAboutGameEnrollList();
            DateTime _now = DateTime.Now;

            DalOperationAboutGameCategory dalgc = new DalOperationAboutGameCategory();

            //首先检查报名时间是否已经截止
            if (dalgc.CheckGameCategoryIsOverTimeByGameCategoryId(int.Parse(ddlEnrollGameCategory.SelectedValue), _now).Tables[0].Rows.Count == 0)
            {
                Javascript.GoHistory(-1, "当前所选活动届次报名已经截止：（", Page);
                return;
            }

            if (dal.Exists(user.userNo, int.Parse(ddlEnrollGameCategory.SelectedValue), int.Parse(ddlEnrollGameType.SelectedValue)) > 0)
            {
                Javascript.AlertAndRedirect("当前所选活动届次和活动类型已经报名，点击确定查看报名信息", "/Administrator/EnrollManage.aspx?fragment=2", Page);
                return;
            }

            try
            {
                dal.Add(new GameEnrollList { gameCategoryId = int.Parse(ddlEnrollGameCategory.SelectedValue), gameTypeId = int.Parse(ddlEnrollGameType.SelectedValue), teacherNo = user.userNo, updateTime = _now });
                Javascript.AlertAndRedirect("报名成功：）", "/Administrator/EnrollManage.aspx?fragment=2", Page);
            }
            catch (System.Exception ex)
            {
                MongoDBLog.LogRecord(ex);
                Javascript.GoHistory(-1, "报名失败：）", Page);
            }
        }
    }
}