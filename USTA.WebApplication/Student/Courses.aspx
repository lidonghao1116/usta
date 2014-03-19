<%@ Page Language="C#" AutoEventWireup="true" Inherits="Student_Courses"
    MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="Courses.aspx.cs" %>

<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>我的课程</span></a></li>
            <li id="liFragment2" class="ui-tabs-hide" runat="server"><a href="?fragment=2"><span>
                暂时不用</span></a></li>
            <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="?fragment=3"><span>
                暂时不用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <asp:DropDownList ID="ddltTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltTerms_SelectedIndexChanged">
            </asp:DropDownList>
                <asp:DataList ID="dlstCourses" runat="server" CssClass="multiRecordsDataList" RepeatDirection="Horizontal" RepeatColumns="4" Width="80%">
            <ItemTemplate>
                <img src="../images/BULLET.GIF" align="middle" /><a href="CInfoCourseIntro.aspx?courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim())%>&termtag=<%#Eval("termTag") %>">
                                        <%#Eval("courseName") %>(<%#Eval("ClassID") %>)</a>
                    </ItemTemplate><FooterTemplate><%=(this.dlstCourses.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" BorderWidth="0" />
                </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
    </div>
    </form>
</asp:Content>
