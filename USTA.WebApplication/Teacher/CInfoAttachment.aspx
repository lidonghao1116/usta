<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_CInfoAttachment"
    MasterPageFile="~/MasterPage/CourseInfoForTeacher.master" Codebehind="CInfoAttachment.aspx.cs" %>

<%@ MasterType  VirtualPath="~/MasterPage/CourseInfoForTeacher.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
     <div id="archivesItemsUploadFiles">
            <!--upload start-->
            <asp:PlaceHolder ID="phUpload" runat="server"></asp:PlaceHolder>
            <!--upload end-->
                <br />
            <asp:Button ID="btnUpload" runat="server" Text="提交" OnClick="btnUpload_Click" OnClientClick="if(!checkIsUpload()){alert('无法提交，可能的原因为：\n您未上传任何文件。\n请上传文件后再提交:)');return false;}"  />
     </div></div>
</asp:Content>
