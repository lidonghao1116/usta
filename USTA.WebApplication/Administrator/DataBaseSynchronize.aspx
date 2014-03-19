<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" Inherits="Administrator_DataBaseSynchronize" Codebehind="DataBaseSynchronize.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="form1" runat="server">
        <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>数据同步</span></a></li>
            <li id="liFragment2" class="ui-tabs-hide" runat="server"><a href="?fragment=2"><span>
                暂时不使用</span></a></li>
            <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="?fragment=3"><span>
                暂时不使用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
        <table><tr><td>请选择要同步的数据内容：</td></tr>
    <tr><td>
        <asp:CheckBox ID="chkMajorTypeSchoolClass" runat="server" Text="专业与班级数据"  /> <br />

        <asp:CheckBox ID="chkCourse" runat="server" Text="课程数据" /><br />

        <asp:CheckBox ID="chkTeacherCoursePlan" runat="server" Text="教师数据及任课数据" /><br />

        <asp:CheckBox ID="chkStudentElectiveStudent" runat="server" Text="学生及选课数据(包括学生作业与实验数据)" />

    </td></tr>
    <tr><td>
        <asp:Literal ID="ltlResult" runat="server"></asp:Literal>
        <br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="同步数据" />
    </td></tr>
    </table>
        <br />
        </div>

        
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>

        
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
        </div>
    </form>
</asp:Content>

