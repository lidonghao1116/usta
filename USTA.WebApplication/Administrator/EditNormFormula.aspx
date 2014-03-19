<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditNormFormula.aspx.cs" Inherits="USTA.WebApplication.Administrator.EditNormFormula" %>

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
            $(".value_a").attr('disabled', 'true');
            $(".value_a").attr("style", "background:Gray;");
        });
        function append(id, text,type) {
            var fvalue = $("#formula").val();
            var tvalue = $("#TextNormFormula").val();
            $("#formula").val(fvalue + id);
            $("#TextNormFormula").val(tvalue + text);
            $("#formula1").html(tvalue + text);
            if (type == 1) {
                $(".value_a").attr('disabled', 'true');
                $(".value_a").attr("style", "background:Gray;");
                $(".value_ba").removeAttr('disabled');
                $(".value_ba").removeAttr('style');
                $(".value_bb").attr('disabled', 'true');
                $(".value_bb").attr("style", "background:Gray;");
                $(".value_c").removeAttr('disabled');
                $(".value_c").removeAttr('style');

            }
            if (type == 2) {
                $(".value_a").removeAttr('disabled');
                $(".value_a").removeAttr('style');
                $(".value_ba").attr('disabled', 'true');
                $(".value_ba").attr("style", "background:Gray;");
                $(".value_bb").attr('disabled', 'true');
                $(".value_bb").attr("style", "background:Gray;");
                $(".value_c").removeAttr('disabled');
                $(".value_c").removeAttr('style');

            }
            if (type == 3) {
                $(".value_a").removeAttr('disabled');
                $(".value_a").removeAttr('style');
                $(".value_ba").attr('disabled', 'true');
                $(".value_ba").attr("style", "background:Gray;");
                $(".value_bb").removeAttr('disabled');
                $(".value_bb").removeAttr('style');
                $(".value_c").attr('disabled', 'true');
                $(".value_c").attr("style", "background:Gray;");

            }
        }
        function removeApp() {
            $("#formula").val("");
            $("#TextNormFormula").val("");
            $("#formula1").html("");
            $(".value_a").attr('disabled', 'true');
            $(".value_a").attr("style", "background:Gray;");
            $(".value_ba").removeAttr('disabled');
            $(".value_ba").removeAttr('style');
            $(".value_bb").removeAttr('disabled');
            $(".value_bb").removeAttr('style');
            $(".value_c").removeAttr('disabled');
            $(".value_c").removeAttr('style');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:HiddenField ID="formula" runat="server" />
     <asp:HiddenField ID="TextNormFormula" runat="server" />
    <table class="tableAddStyleNone" width="100%">
    <tr><td class="border">计算符号：</td><td class="border"><input id="add" onclick="append('+','+',1)" class="value_a" type="button" value="+"/>
    <input id="minus" onclick="append('-','-',1)" class="value_a" type="button" value="-"/>
    <input id="mul" onclick="append('*','*',1)" class="value_a" type="button" value="*"/>
    <input id="divide" onclick="append('\/','\/',1)" class="value_a" type="button" value="/"/>
    <input id="leftK" onclick="append('(','(',2)" class="value_ba" type="button" value="("/>
    <input id="rightK" onclick="append(')',')',2)" class="value_bb" type="button" value=")"/>
    &nbsp;&nbsp;&nbsp; <input id="delete" onclick="removeApp()" type="button" value="重新设置"/>
</td></tr>
<tr><td class="border">计算项：</td><td class="border">
<%if (this.normIds == "-1")
  { %>
   <input class="value_c" onclick="append('B','理论课时',3)" type="button" value="理论课时"/>
   <input class="value_c" onclick="append('C','实验课时',3)" type="button" value="实验课时"/>
  <%}
  else if (this.normIds == "0")
  { %>
  <input class="value_c" onclick="append('A','硕士教学',3)" type="button" value="硕士教学"/>
  <%} %>
<asp:DataList ID="dlstChildNorm" runat="server"  RepeatDirection="Horizontal" RepeatColumns="4">
    <ItemTemplate>
      <input id="<%#Eval("normId") %>" class="value_c" onclick="append('<%#Eval("normId") %>','<%#Eval("name") %>',3)" type="button" value="<%#Eval("name") %>"/>
    </ItemTemplate>
    </asp:DataList></td></tr>
    <tr><td class="border">计算公式</td><td class="border">
    <span id="formula1" style="font-size:12px;"></span>
    </td></tr>
    <tr><td class="border" colspan="2"> <asp:Button ID="btnCommit" runat="server" onclick="btnCommit_Click" Text="提交" /></td></tr>
    </table>
    </div>
    </form>
</body>
</html>
