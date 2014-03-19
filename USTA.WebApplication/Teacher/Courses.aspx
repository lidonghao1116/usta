<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_Courses"
    MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="Courses.aspx.cs" %>

<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>我的课程列表</span></a></li>
            <li id="liFragment2" class="ui-tabs-hide" runat="server"><a href="?fragment=2"><span>
                暂时不使用</span></a></li>
            <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="?fragment=3"><span>
                暂时不使用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">

        <asp:DropDownList ID="ddltTerms" runat="server" AutoPostBack="true"
             OnSelectedIndexChanged="ddltTerms_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <br />
        
            <asp:DataList ID="dlstCourses" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" Width="100%" CssClass="multiRecordsDataList">
                <ItemTemplate>
                                 <img src="../images/BULLET.GIF" align="middle" /><a href="CInfoCourseIntro.aspx?courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("ClassID").ToString().Trim())%>&termtag=<%#Eval("termTag")%>&teacherType=<%=Server.UrlEncode("教师") %>">
                                    <%#Eval("courseName").ToString().Trim() %>(<%#Eval("ClassID") %>)</a>
                </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.dlstCourses.Items.Count == 0 ? "未找到以教师身份担任的课程数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            
            <asp:DataList ID="dlstAssistantCourses" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" Width="100%" CssClass="multiRecordsDataList">
                <ItemTemplate>
                                 <img src="../images/BULLET.GIF" align="middle" /><a href="CInfoCourseIntro.aspx?courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("ClassID").ToString().Trim()) %>&termtag=<%#Eval("termTag").ToString().Trim()%>&teacherType=<%=Server.UrlEncode("助教") %>">
                                    <%#Eval("courseName").ToString().Trim()%>(<%#Eval("ClassID").ToString().Trim()%>)</a>(助教)
                </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.dlstAssistantCourses.Items.Count == 0 ? "未找到以助教身份担任的课程数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
    </div>
    </form>
</asp:Content>
