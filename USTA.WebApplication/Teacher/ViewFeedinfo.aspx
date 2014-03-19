<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewFeedinfo.aspx.cs" Inherits="USTA.WebApplication.Teacher.ViewFeedinfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
    <title></title>
      <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
     <table class="tableAddStyle" align="center" width="100%">
       
     <tr>
         <td width="20%" > 标题：</td>
         <td  > <%=feedback.feedBackTitle %>                      
      </td>
    </tr>
    <tr>
         <td > 内容：</td>
         <td > <%=feedback.feedBackContent %>  </td>
    </tr>
    <tr>   
            <% 
               if (feedback.feedBackContactTo!=null)
             {
            %>
             <td style="border-bottom:0px;">  联系方式： </td>
            <td style="border-bottom:0px;"> <%=feedback.feedBackContactTo %> </td>
          <%
            }      
         %>  
         
      </tr>
      <tr><td>添加回复：</td><td><asp:TextBox ID="txtbackinfo" runat="server"  Width="90%" Height="100px"
              TextMode="MultiLine"></asp:TextBox></td></tr>
    </table>
   
    <asp:Button ID="btnCommit" runat="server" Text="确定" onclick="btnCommit_Click" />
    </div>
    </form>
</body>
</html>

