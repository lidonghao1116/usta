<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/ViewSchoolworkNotify.master" AutoEventWireup="true" Inherits="Teacher_VSCheckSchoolworks" Codebehind="VSCheckSchoolworks.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/ViewSchoolworkNotify.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CouserInfoHead" Runat="Server">
   <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    
    <style type="text/css">
        body
        {
            background: #FFFFFF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CouserInfoContent" Runat="Server">
 
    <div class="ui-tabs-panel" runat="server">
        <table width="80%" style="margin-left:10%" class="tableEditStyle">
            <tr>
                <td width="20%">
                    学号：
                </td>
                <td>
                    <asp:Label ID="lblNo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    姓名：
                </td>
                <td>
                    <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    评语：
                </td>
                <td>
                    <asp:TextBox ID="txtRemark" runat="server" Width="330px" Height="77px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    得分：
                </td>
                <td>
                    <asp:TextBox ID="txtScore" runat="server" Font-Size="9pt" Width="123px" CssClass="required number"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    
                    <%=attachmentIds==0?string.Empty:"<br />"+this.GetURL(attachmentIds.ToString())+"<br />"%>
                    
                    <!--upload start-->
                    <input id="Button3" type="button" value="回传已经批阅的作业" onclick="addIframe(<%=fileFolderType %>);" />（可以不添加）&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>
                    <div id="iframes">
                    </div>
                    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                    <!--upload end-->
                </td>
            </tr>
            <tr>
                <td style="border-bottom: 0px;">
                </td>
                <td style="border-bottom: 0px;">
                    <asp:Button ID="btnUpdate" runat="server" Text="提交" Width="50px" OnClick="btnUpdate_Click" />
                </td>
            </tr>
        </table>
    </div>
 
</asp:Content>

