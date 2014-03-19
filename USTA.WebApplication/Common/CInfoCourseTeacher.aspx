<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_CInfoCourseTeacher"
    MasterPageFile="~/MasterPage/CourseInfoForCommon.master" Codebehind="CInfoCourseTeacher.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForCommon.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="CouserInfoContent">
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
                    <table class="mytable mytableWidth"cellspacing="0">
                        <tr>
                            <td class="row">
                                <%#Eval("teacherResume")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <ItemStyle CssClass="row" />
                <FooterTemplate>
                    <%=(this.dlstCourseTeacher.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
    </div>
</asp:Content>