<%@ Page Language="C#" AutoEventWireup="true" Inherits="USTA.WebApplication.Student.MyGradeCheckManage"
    MasterPageFile="~/MasterPage/FrameManage.master" CodeBehind="StudentManager.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
    <!--Validate-->
    <%--    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>

    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>我的成绩审核管理</span></a></li>
            <li id="liFragment2" runat="server" visible="false"><a href="?fragment=2"><span>暂未使用</span></a></li>
            <li id="liFragment3" runat="server" visible="false"><a href="?fragment=3"><span>暂未使用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
        <div style="text-align:center;">
            <asp:HyperLink ID="notifyTitle" runat="server"></asp:HyperLink>
            </div>
            <asp:DataList ID="dlstStudentSchoolClassName" runat="server" align="center" Width="100%"
                CellPadding="0" RepeatDirection="Horizontal" RepeatColumns="2">
                <ItemTemplate>
                    <table style="border-bottom: 1px solid #056FAE; height: 25px;" width="100%">
                        <tr>
                            <td style="height: 25px;">
                                <%#Eval("SchoolClassName").ToString().Trim()%>&nbsp;&nbsp;学号：<%=this.studentNo %>&nbsp;&nbsp;<%#Eval("studentName").ToString().Trim() %>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                <FooterTemplate>
                    <%=(this.dlstStudentSchoolClassName.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                <HeaderTemplate>
                </HeaderTemplate>
            </asp:DataList>
            <a href="GradeCheckApply.aspx?keepThis=true&amp;TB_iframe=true&amp;height=500&amp;width=800" title="申请办理重修重考" class="thickbox">申请办理重修重考</a>
            <asp:DataList ID="dlstStudentGradeCheck" runat="server" align="center" Width="100%"
                CellPadding="0" OnItemDataBound="dlstStudentGradeCheck_ItemDataBound">
                <ItemTemplate>
                    <table style="border-bottom: 1px solid #056FAE; height: 25px;" width="100%">
                        <tr>
                            <td style="height: 25px;">
                                申请时间：<%#Eval("updateTime").ToString().Trim()%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--是否符合学位申请条件：<%#(this.GetStudentGradeCheckConfirm(DateTime.Parse(Eval("updateTime").ToString().Trim())) == "1" ? "符合" : "<font color=\"red\">不符合</font>")%>&nbsp;&nbsp;不及格科目（备注）：<%#this.GetStudentGradeCheckConfirmAboutRemark(DateTime.Parse(Eval("updateTime").ToString().Trim()))%>&nbsp;&nbsp;--%><%#this.GetGradeCheckApplyInfo(int.Parse(Eval("gradeCheckApplyId").ToString().Trim()))%>
                            </td>
                        </tr>
                    </table>
                   <%--<asp:DataList ID="dlstStudentGradeCheckItem" runat="server" CellPadding="0" Width="100%"
                        Style="margin-bottom: 10px;">
                        <ItemTemplate>
                            <table style="border-bottom: 1px dashed #CCCCCC;" width="100%">
                                <tr>
                                    <td width="70%" style="height: 25px;">
                                        <img src="../images/BULLET.GIF" align="absmiddle" />
                                        <%#this.GetGradeCheckDetailByGradeCheckId(int.Parse(Eval("gradeCheckId").ToString().Trim()))%><%#Eval("gradeCheckDetailValue").ToString().Trim()%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <FooterTemplate>
                            未找到数据</FooterTemplate>
                        <FooterStyle CssClass="datalistNoLine" />
                    </asp:DataList>--%>
                </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                <FooterTemplate>
                    <%=(this.dlstStudentGradeCheck.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                <HeaderTemplate>
                </HeaderTemplate>
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
    </div>
    </form>
</asp:Content>
