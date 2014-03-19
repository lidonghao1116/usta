<%@ Page Language="C#" AutoEventWireup="true" Inherits="Student_CousrInfo_8_CourseTeacher"
    MasterPageFile="~/MasterPage/CourseInfoForStudent.master" Codebehind="CInfoCourseTeacher.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForStudent.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
    <div class="ui-tabs-panel">
         <asp:DataList ID="dlstCourseTeacher" runat="server">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <th scope="col">
                                教师简介
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td>
                                <%#Eval("teacherResume")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstCourseTeacher.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
    </div>
</asp:Content>
