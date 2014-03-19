<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_ViewCourses"
    MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="ViewCourses.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>课程关注信息查看</span></a></li>
            <li id="liFragment2" class="ui-tabs-hide" runat="server"><a href="?fragment=2"><span>
                暂时不使用</span></a></li>
            <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="?fragment=3"><span>
                暂时不使用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <asp:DropDownList ID="ddltTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltTerms_SelectedIndexChanged" Width="200px" >
            </asp:DropDownList>
            <br />
            <br />
            <asp:DataList ID="dlCourse" runat="server" DataKeyField="courseNo" RepeatDirection="Horizontal" RepeatColumns="4" Width="100%" CssClass="multiRecordsDataList">
                <ItemTemplate>
                   <img src="../images/BULLET.GIF" align="middle" />
                            <a href="ViewCourseAttention.aspx?keepThis=true&courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim()) %>&termTag=<%#Eval("termTag").ToString().Trim() %>&TB_iframe=true&height=300&width=500" title="课程关注查看" class="thickbox"> <%#Eval("courseName").ToString().Trim()%>(<%#Eval("classID").ToString().Trim() %>)</a>
                             
                </ItemTemplate><FooterTemplate><%=(this.dlCourse.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" BorderWidth="0" />
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
    </div>
    </form>
</asp:Content>
