<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_ViewStudentInfo" Codebehind="ViewStudentInfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
    <title></title>
     <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table width="100%" class="tableAddStyle beautyTableStyle" align="center">    
      <tr>
        <td><asp:Label ID="lblId" runat="server" Font-Size="9pt" Text="学生编号：" Width="80px"></asp:Label></td>
        <td><asp:Label ID="lblNo" runat="server" Font-Size="9pt"></asp:Label></td>
      </tr>
      <tr>
        <td><asp:Label ID="lblNames" runat="server" Font-Size="9pt" Text="学生姓名：" Width="73px"></asp:Label></td>
        <td><asp:Label ID="lblName" runat="server" Font-Size="9pt"></asp:Label></td>
      </tr>
      <tr>
        <td><asp:Label ID="lblSpecialitys" runat="server" Font-Size="9pt" Text="学生专业：" Width="76px"></asp:Label></td>
        <td><asp:Label ID="lblSpeciality" runat="server" Font-Size="9pt"></asp:Label></td>
      </tr>
       <tr>
        <td><asp:Label ID="lblClasses" runat="server" Font-Size="9pt" Text="班级：" Width="78px"></asp:Label></td>
        <td><asp:Label ID="lblClass" runat="server" Font-Size="9pt"></asp:Label></td>
      </tr>
       <tr>
        <td><asp:Label ID="lblMobileNos" runat="server" Font-Size="9pt" Text="手机号：" Width="78px"></asp:Label></td>
        <td><asp:Label ID="lblMobileNo" runat="server" Font-Size="9pt"></asp:Label></td>
      </tr>
      <tr>
        <td><asp:Label ID="lblEmails" runat="server" Font-Size="9pt" Text="E-mail：" Width="78px"></asp:Label></td>
        <td><asp:Label ID="lblEmail" runat="server" Font-Size="9pt"></asp:Label></td>
      </tr>
      <tr>
        <td style="border-bottom:0px;"><asp:Label ID="lblRemarks" runat="server" Font-Size="9pt" Text="备注：" Width="75px"></asp:Label></td>
        <td style="border-bottom:0px;"><asp:Label ID="lblRemark" runat="server" Font-Size="9pt"  Width="155px"></asp:Label></td>
      </tr>
      
     </table>
    </div>
    </form>
</body>
</html>
