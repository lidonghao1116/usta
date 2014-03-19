<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GradeCheckApply.aspx.cs"
    Inherits="USTA.WebApplication.Student.GradeCheckApply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="/css/main.css" />
</head>
<body>
    <form id="form1" runat="server">
    <table class="tableAddStyle" width="100%">
        <tr>
            <td>
                重修重考的学期：<asp:DropDownList ID="ddlTermTags" runat="server">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp; 关键字：<asp:TextBox ID="txtGradeCheckCourses" runat="server"></asp:TextBox><asp:Button
                    ID="btnCommit" runat="server" Text="搜索" OnClick="btnCommit_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataList ID="dlstcourses" runat="server" RepeatDirection="Horizontal" RepeatColumns="1"
                    Width="100%" CssClass="multiRecordsDataList" OnItemDataBound="dlstcourses_ItemDataBound">
                    <ItemTemplate>
                        <label>
                            <asp:CheckBox ID="select" runat="server" />
                            <span id="courseName" runat="server">
                                <%#Eval("courseName").ToString().Trim()%>(<%#Eval("classID").ToString().Trim() %>)</span></label>
                        请选择方式：<asp:DropDownList ID="ddlGradeCheckApplyType" runat="server">
                            <asp:ListItem Value="重修">重修</asp:ListItem>
                            <asp:ListItem Value="重考">重考</asp:ListItem>
                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;请选择原因：
                <asp:DropDownList ID="ddlApplyReason" runat="server">
                </asp:DropDownList>
                        <span id="courseNo" runat="server" visible="false">
                            <%#Eval("courseNo").ToString().Trim()%></span> <span id="classID" runat="server"
                                visible="false">
                                <%#Eval("classID").ToString().Trim()%></span> <span id="termTag" runat="server" visible="false">
                                    <%#Eval("termTag").ToString().Trim()%></span>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.dlstcourses.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" BorderWidth="0" />
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button3" runat="server" Text="提交" OnClick="Button3_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
