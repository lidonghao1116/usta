<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_CInfoUploadScore" 
    MasterPageFile="~/MasterPage/CourseInfoForTeacher.master" Codebehind="CInfoUploadScore.aspx.cs" %>

<%@ MasterType  VirtualPath="~/MasterPage/CourseInfoForTeacher.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
         <a href="/Teacher/UploadScore.ashx?courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>">点击导出到Excel</a>
     </div>
</asp:Content>
