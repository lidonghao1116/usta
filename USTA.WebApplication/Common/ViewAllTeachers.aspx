<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master"
    AutoEventWireup="true" Inherits="Common_ViewAllTeachers" Codebehind="ViewAllTeachers.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />

    
    
    <script type="text/javascript">
        $(window).keydown(function(event) {
            switch (event.keyCode) {
                case 13:
                    enterKey(event, 'ctl00_ContentPlaceHolder1_btnSubmit');
                    break;
                default:
                    break;
            }
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
     
    <br />
    <br />
    <div align="center">
        请输入教师的姓名关键字：
        <asp:TextBox ID="txtSearchstring" runat="server" AutoPostBack="true"></asp:TextBox>
        <asp:Button ID="btnSubmit" runat="server" Text="搜索" OnClick="btnSubmit_Click" />
    <br />
    <br />
    </div>
    
    <fieldset style="width:70%;margin-left:auto;margin-right:auto;" id="fieldsetTeacher" runat="server"><legend id="legendTeacher" runat="server">所有教师</legend>
     <br />
        <asp:DataList ID="dlstTeacher" runat="server" CssClass="multiRecordsDataList"
            RepeatDirection="Horizontal" RepeatColumns="6" Width="95%" style="margin-left:auto;margin-right:auto;">
            <ItemTemplate>
                <img src="../images/BULLET.GIF" align="middle" /><a href="<%#(Eval("usertye").ToString()=="1")?"ViewTeacherInfo.aspx?email="+Eval("email").ToString().Trim():"ViewAssistantInfo.aspx?email="+Eval("email").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=300&width=500"
                    title="查看教师基本信息" class="thickbox"><%#Eval("username") %></a>
            </ItemTemplate>
            <FooterTemplate>
                <%=dlstTeacher.Items.Count==0?"未搜索到相关数据": null%></FooterTemplate>
            <FooterStyle BorderWidth="0" />
        </asp:DataList>
    </fieldset>
    <br />
    <br />
    </form>
</asp:Content>
