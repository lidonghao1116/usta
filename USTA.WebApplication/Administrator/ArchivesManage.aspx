<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="ArchivesManage.aspx.cs" Inherits="USTA.WebApplication.Administrator.ArchivesManage" %>



<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
    <script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
    <!--Validate-->
        <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>

    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();
        });

        function compareTime() {
            var startTime = Date.parse($('#startTime').val().replace(/-/g, "/"));
            var endTime = Date.parse($('#endTime').val().replace(/-/g, "/"));
            if (startTime > endTime) {
                alert("开始时间不能晚于截止时间，请修改:)");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>结课资料管理</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>结课资料规则管理</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>结课资料上传开始通知时间设置</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
         请选择学期：<asp:DropDownList ID="ddlTermTags" runat="server" ClientIDMode="Static" onchange="location.href='/Administrator/ArchivesManage.aspx?fragment=1&termTag='+$('#ddlTermTags').val() + '&locale='+$('#ddlPlace').val()+'&keyword='+$.trim($('#txtKeyword').val());">
                <asp:ListItem Text="在所有学期中查找" Value="all"></asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;请选择开课地点：<asp:DropDownList ID="ddlPlace" runat="server" ClientIDMode="Static" onchange="location.href='/Administrator/ArchivesManage.aspx?fragment=1&termTag='+$('#ddlTermTags').val() + '&locale='+$('#ddlPlace').val()+'&keyword='+$.trim($('#txtKeyword').val());">
                <asp:ListItem Text="合肥" Value="合肥"></asp:ListItem>
                <asp:ListItem Text="苏州" Value="苏州"></asp:ListItem>
            </asp:DropDownList>
                &nbsp;&nbsp;&nbsp; 关键字：<asp:TextBox ID="txtKeyword" runat="server" ClientIDMode="Static"></asp:TextBox>
                <input type="button" value="搜索" onclick="location.href='/Administrator/ArchivesManage.aspx?fragment=1&termTag='+$('#ddlTermTags').val() + '&locale='+$('#ddlPlace').val()+'&keyword='+$.trim($('#txtKeyword').val());" /><br /><br />
<asp:DataList ID="dlstCourses" runat="server"  RepeatDirection="Horizontal" RepeatColumns="1" Width="100%" CssClass="multiRecordsDataList" EnableViewState="false">
            <ItemTemplate>
            <a name="archive_<%#Container.ItemIndex%>"></a>
                <img src="../images/BULLET.GIF" align="middle" />
                        <a href="ViewArchives.aspx?keepThis=true&courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim()) %>&termTag=<%#Eval("termTag").ToString().Trim() %>&TB_iframe=true&height=500&width=800" title="查看结课资料归档" class="thickbox"> <%#Eval("courseName").ToString().Trim()%>(<%#Eval("classID").ToString().Trim() %>)</a>&nbsp;&nbsp;<a href="AddArchivesForTeacherOrAssistant.aspx?keepThis=true&courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim()) %>&termTag=<%#Eval("termTag").ToString().Trim() %>&keyword=<%=keyword %>&locale=<%=Server.UrlEncode(locale) %>&anchor=<%#Container.ItemIndex%>&teacherType=教师&TB_iframe=true&height=500&width=800" title="为<%#Eval("courseName").ToString().Trim()%>(<%#Eval("classID").ToString().Trim() %>)教师添加结课资料" class="thickbox">为本门课程教师添加结课资料</a>&nbsp;&nbsp;<a href="AddArchivesForTeacherOrAssistant.aspx?keepThis=true&courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim()) %>&termTag=<%#Eval("termTag").ToString().Trim() %>&keyword=<%=keyword %>&locale=<%=Server.UrlEncode(locale) %>&anchor=<%#Container.ItemIndex%>&teacherType=助教&TB_iframe=true&height=500&width=800" title="为<%#Eval("courseName").ToString().Trim()%>(<%#Eval("classID").ToString().Trim() %>)助教添加结课资料" class="thickbox">为本门课程助教添加结课资料</a><br /><%#this.GetArchivesList("教师", Eval("courseNo").ToString().Trim(), Eval("classID").ToString().Trim(),Eval("termTag").ToString().Trim())%> <%#this.GetArchivesList("助教", Eval("courseNo").ToString().Trim(), Eval("classID").ToString().Trim(),Eval("termTag").ToString().Trim())%> <div style="height:25px;"></div>
                  
            </ItemTemplate><FooterTemplate><%=(this.dlstCourses.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" BorderWidth="0" />
        </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" UrlPaging ="true" ID="AspNetPager1" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager1_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        <div><a href="AddArchivesItem.aspx?keepThis=true&TB_iframe=true&height=300&width=900" title="添加结课资料规则" class="thickbox">添加结课资料规则</a></div>
        <asp:DataList ID="dlstArchivesItems" DataKeyField="archiveItemId" runat="server" Width="100%" OnItemCommand="dlstArchivesItems_ItemCommand">
                    <HeaderTemplate>
                        <table class="datagrid2">
                            <tr>
                                <th width="5%">
                                    序号
                                </th>
                                <th width="20%">
                                    规则名称
                                </th>
                                <th width="20%">
                                    教师类型
                                </th>
                                <th width="15%">
                                    序号
                                </th>
                                <th width="20%">
                                    备注
                                </th>
                                <th width="20%">
                                    相关操作
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="datagrid2">
                            <tr>
                                <td width="5%">
                                    <%#Container.ItemIndex+1%>
                                </td>
                                <td width="20%">
                                    <%#Eval("archiveItemName").ToString().Trim()%>
                                </td>
                                <td width="20%">
                                    <%#Eval("teacherType").ToString().Trim()%>
                                </td>
                                <td width="15%">
                                    <%#USTA.Common.CommonUtility.ChangeTermToString(Eval("termTag").ToString().Trim())%>
                                </td>
                                <td width="20%">
                                    <%#Eval("remark").ToString().Trim()%>
                                </td>
                                <td width="20%">
                                    <a href="EditArchivesItem.aspx?archiveItemId=<%#Eval("archiveItemId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=300&width=900" class="thickbox" title="修改结课资料规则">
                                        编辑</a>&nbsp;&nbsp; 
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="delete" OnClientClick="return deleteTip();">删除</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.dlstArchivesItems.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" />
                </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">

        <div class="ViewTips" style="margin-left:auto;margin-right:auto;float:none;"><div class="TipsTopic">温馨提示：此功能用于在设定的时间区间内提醒教师和助教上传结课资料</div></div><br /><br />
             结课资料上传通知开始时间：<input type="text" id="startTime" runat="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})" clientidmode="Static" /><br />
            <br />
             结课资料上传通知结束时间：<input type="text" id="endTime" runat="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})" clientidmode="Static" /><br />
            <br />
            <asp:Button ID="Button3" runat="server" Text="提交" OnClick="Button3_Click" OnClientClick="compareTime();" />
        </div>
        </div>
    </form>
</asp:Content>
