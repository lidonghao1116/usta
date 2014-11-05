<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master"
    AutoEventWireup="true" Inherits="Student_EditStudentinfo" Codebehind="EditStudentinfo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>我的联系信息</span></a></li>
            <li id="liFragment2" class="ui-tabs-hide" runat="server"><a href="?fragment=2"><span>
                暂时不使用</span></a></li>
            <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="?fragment=3"><span>
                暂时不使用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
                <table>
                <tr>
                        <td colspan="2">
                            温馨提示：若您需要修改个人信息，请到信息化平台修改。
                        </td></tr>
                    <tr>
                        <td>
                            Email：
                        </td>
                        <td>
                            <asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>手机号：
                        </td>
                        <td><asp:Literal ID="ltlPhone" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr><td>备注</td><td>
                    <asp:Literal ID="ltlRemark"  runat="server"></asp:Literal></td></tr>
                </table>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
    </div>
    </form>
</asp:Content>
