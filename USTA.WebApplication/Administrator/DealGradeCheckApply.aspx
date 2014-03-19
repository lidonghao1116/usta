<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealGradeCheckApply.aspx.cs"
    Inherits="USTA.WebApplication.Administrator.DealGradeCheckApply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table width="100%" class="tableAddStyleNone">
     <tr>
        <td colspan="2" class="border"><asp:Literal ClientIDMode="Static" runat="server" ID="ltlTermTag"></asp:Literal></td>
     </tr>
      <tr>
        <td colspan="2" class="border"> <asp:Literal ClientIDMode="Static" runat="server" ID="ltlCourseName"></asp:Literal></td>
      </tr>
      <tr><td colspan="2"class="border"><asp:Literal ClientIDMode="Static" runat="server" ID="ltlUpdateTime"></asp:Literal></td></tr>
      <tr><td class="border" width="10%"> 方式：</td><td class="border"> <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlGradeCheckApplyType">
        </asp:DropDownList></td></tr>
      <tr><td class="border"  width="10%"> 审核意见：</td><td class="border"><asp:TextBox TextMode="MultiLine" ID="txtapplyChecKSuggestion" runat="server"
            Rows="5" Width="80%"></asp:TextBox></td></tr>
       <tr><td colspan="2" class="border"> <asp:Button ID="btnAccord" runat="server" Text="符合" OnClick="btnAccord_Click" OnClientClick="" />
        <asp:Button ID="btnNotAccord" runat="server" Text="不符合" OnClick="btnNotAccord_Click"
            OnClientClick="" /></td></tr>
       
    </div>
    </form>
</body>
</html>
