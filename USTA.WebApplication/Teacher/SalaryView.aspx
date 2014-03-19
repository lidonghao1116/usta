<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="SalaryView.aspx.cs" Inherits="USTA.WebApplication.Teacher.SalaryView" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();
        });

        function addTip() {
            return confirm("确定添加？");
        }

        function deleteTip() {
            return confirm("确定删除？");
        }

    </script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "100%", "200px");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>薪酬预算查询</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>已发放薪酬查询</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            
            <div>
                <div>
                    教师：<asp:Literal ID="TeacherSalary_Name" runat="server"></asp:Literal>
                    学期：<asp:DropDownList ID="TeacherSalary_TermTag" runat="server" AutoPostBack="true" OnSelectedIndexChanged="TeacherSalary_TermTagChanged">
                        </asp:DropDownList>
                    <asp:Button ID="btn_TeacherSalary_Query" runat="server" Text="查找" OnClick="TeacherSalaryQuery_Click"/>
                </div>
                <div>
 <asp:DataList Width="100%" ID="TeacherSalaryList" runat="server" DataKeyField="teacherSalaryId" OnItemCommand="TeacherSalaryManage_ItemCommand" OnItemDataBound="TeacherSalaryManage_ItemBound" CellPadding="0">
                <HeaderTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <th width="10%">
                                教师
                            </th>
                            <th width="10%">
                                教师类型
                            </th>
                            <th width="10%">
                                薪酬预算
                            </th>
                            <th width="10%">
                                学期
                            </th>
                            <th width="20%">
                                添加时间
                            </th>
                            <th width="10%">
                                状态
                            </th>
                            <th width="30%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <td width="10%">
                                <%#Eval("teacher.teacherName")%>
                            </td>
                            <td width="10%">
                            <asp:Literal ID="TeacherSalary_TeacherType_Literal" runat="server"></asp:Literal>
                            </td>
                            <td width="10%">
                                <%#Eval("totalTeachCost") %>
                            </td>
                            <td width="10%">
                                <%#Eval("termTag") %>
                            </td>
                            <td width="20%">
                                <%#Eval("createdTime") %>
                            </td>
                            <td width="10%">
                                <%#bool.Parse(Eval("isConfirm").ToString().Trim()) ? "已确认" : "未确认" %>
                            </td>
                            <td width="30%">
                            <a href="/Teacher/ViewTeacherSalarySummaryDetail.aspx?keepThis=true&teacherSalaryId=<%#Eval("teacherSalaryId").ToString().Trim()%>&TB_iframe=true&height=300&width=500" class="thickbox">查看详情</a>
                                <asp:LinkButton ID="LinkButton_TeacherSalary_Confirm" runat="server" CommandName="confirm" Visible="false">确认</asp:LinkButton>
                                <asp:HyperLink ID="TeacherSalary_feedback_Link" runat="server" class="thickbox" Visible="false" >反馈信息</asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.TeacherSalaryList.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
                    <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="TeacherSalaryPager" runat="server"
                    FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="TeacherSalaryPager_PageChanged"
                    PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                    HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                    SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                    CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                    TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <div>
            教师：<asp:Literal ID="SalaryEntry_TeacherName" runat="server"></asp:Literal>
                学期：<asp:DropDownList ID="SalaryQuery_TermTag" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SalaryQuery_TermTagChanged">
                      </asp:DropDownList>
                月份：<input type="text" id="SalaryQuery_SalaryMonth" runat ="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM',alwaysUseStartDate:true})"/>
                <asp:Button ID="btn_SalaryQuery" Text="查找" runat="server" OnClick="SalaryQuery_Click"/>
            </div>
            <div>
                <asp:DataList Width="100%" ID="SalaryEntryList" runat="server" DataKeyField="salaryEntryId" OnItemDataBound="SalaryEntryManage_ItemBound">
                    <HeaderTemplate>
                        <table width="100%" class="datagrid2">
                            <tr>
                                <th width="10%">
                                    教师
                                </th>
                                <th width="10%">
                                    学年
                                </th>
                                <th width="10%">
                                    月份
                                </th>
                                <th width="10%">
                                    含税薪酬
                                </th>
                                <th width="10%">
                                    不含税薪酬
                                </th>
                                <th width="10%">
                                    税后总薪酬
                                </th>
                                <th width="15%">
                                    创建时间
                                </th>
                                <th width="10%">
                                    状态
                                </th>
                                <th width="15%">
                                    操作
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width="100%" class="datagrid2">
                            <tr>
                                <td width="10%">
                                    <%#Eval("teacher.teacherName") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("termTag") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("salaryMonth") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("teacherCostWithTax") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("teacherCostWithoutTax") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("teacherTotalCost") %>
                                </td>
                                <td width="15%">
                                    <%#Eval("createdTime") %>
                                </td>
                                <td width="10%">
                                    <asp:Literal ID="literal_SalaryEntryStatus" runat="server"></asp:Literal>
                                </td>
                                <td width="15%">
                                    <a href="/Common/ViewTeacherSalaryDetail.aspx?keepThis=true&salaryEntryId=<%#Eval("salaryEntryId").ToString().Trim()%>&TB_iframe=true&height=300&width=500" class="thickbox">查看详情</a>
                                    <asp:HyperLink ID="SalaryEntry_feedback_Link" runat="server" NavigateUrl='<%#"/Teacher/TeacherSalaryQA.aspx?keepThis=true&salaryId=" + Eval("salaryEntryId") + "&salaryType=2&TB_iframe=true&height=300&width=500"%>' class="thickbox" Visible="false" >反馈信息</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.SalaryEntryList.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" />
                </asp:DataList>
                    <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="SalaryEntryPager" runat="server"
                    FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="SalaryEntryPager_PageChanged"
                    PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                    HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                    SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                    CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                    TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
                    </webdiyer:AspNetPager>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
