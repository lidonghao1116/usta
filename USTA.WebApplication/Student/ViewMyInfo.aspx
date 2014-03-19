<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" Inherits="Student_ViewStudentInfo" Codebehind="ViewMyInfo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div align="center"> 
<fieldset style="width:60%; margin:auto;"><legend>我的联系方式</legend>
手机号:<span id="spanPhone" runat="server"></span><br />
Email:<span id="spanEmail" runat="server"></span><br />
</fieldset>
</div>
</asp:Content>

