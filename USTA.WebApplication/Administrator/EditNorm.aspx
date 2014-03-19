<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditNorm.aspx.cs" Inherits="USTA.WebApplication.Administrator.EditNorm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
        <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate();
        });
    </script>
</head>
<body>
   <form id="form1" runat="server">
    <div>
    <asp:HiddenField runat="server" ID="hidNormId" />
     <asp:HiddenField runat="server" ID="hidParentId" />
    <table width="100%" class="tableAddStyleNone">
    <%if (this.parentName != null)
      { %>
      <tr><td class="border">父指标</td><td class="border"><%=this.parentName%></td></tr>
    <% }%>
    <tr><td class="border">指标类型</td><td class="border">
           <asp:DropDownList ID="ddltNormType" runat="server">
            <asp:ListItem Value="0" Text="参与计算类型"></asp:ListItem>
             <asp:ListItem Value="1" Text="展示类型，不参与计算类型"></asp:ListItem>
           </asp:DropDownList>
            </td></tr>
        <tr><td class="border">指标名称</td><td class="border">
            <asp:TextBox ID="TextNormName" runat="server" CssClass="required" ></asp:TextBox>
            </td></tr>
         <tr><td class="border">备注</td><td class="border">
             <asp:TextBox ID="TextComment" runat="server" TextMode="MultiLine" ></asp:TextBox>
            </td></tr>
            <tr><td></td><td>
                <asp:Button ID="btnSubmit" runat="server" Text="提交" onclick="btnSubmit_Click" /> </td></tr>
    </table>
    </div>
    </form>
</body>
</html>
