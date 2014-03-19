<%@ Page Language="C#" AutoEventWireup="true" Inherits="CourseManager"
    MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="CourseManager.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />

    

    <!--Validate-->

    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>

    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"  visible="false"><a href="?fragment=1"><span>Excel导入课程信息</span></a></li>
            <li id="liFragment2" runat="server"  visible="false"><a href="?fragment=2"><span>添加课程信息</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>搜索课程信息</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            <p>
                学期：<asp:DropDownList ID="ddlSerachTermTags" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSerachTermTags_SelectedIndexChanged">
                <asp:ListItem Text="在所有学期中查找" Value="all"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp; 关键字：<asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                <asp:Button ID="btnCommit" runat="server" Text="搜索" OnClick="btnCommit_Click" />
            </p>
            <br />
            <asp:DataList ID="dlSearchCourse" runat="server" DataKeyField="courseNo" Width="100%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="10%">
                                课程编号
                            </th>
                            <th width="20%">
                                课程名称
                            </th>
                            <th width="5%">
                                学时
                            </th>
                            <th width="10%">
                                实验课时
                            </th>
                            <th width="5%">
                                学分
                            </th>
                            <th width="40%">
                                课程属性
                            </th>
                            <th width="10%">
                                学期标识
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="10%">
                                <%#Eval("courseNo").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <a href="/Common/CInfoCourseIntro.aspx?courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim())%>&termtag=<%#Eval("termTag").ToString().Trim()%>">
                                    <%#Eval("courseName").ToString().Trim()%>(<%#Eval("classID").ToString().Trim()%>)</a>
                            </td>
                            <td width="5%">
                                <%#Eval("period").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <%#Eval("TestHours").ToString().Trim()%>
                            </td>
                            <td width="5%">
                                <%#Eval("credit").ToString().Trim()%>
                            </td>
                            <td width="40%">
                                <%#Eval("courseSpeciality").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <%#Eval("termTag").ToString().Trim()%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlSearchCourse.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager2"  runat="server" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager2_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
    </form>
</asp:Content>
