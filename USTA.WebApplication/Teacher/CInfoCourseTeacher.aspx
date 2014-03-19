<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_CInfoCourseTeacher" 
    MasterPageFile="~/MasterPage/CourseInfoForTeacher.master" Codebehind="CInfoCourseTeacher.aspx.cs" %>

<%@ MasterType  VirtualPath="~/MasterPage/CourseInfoForTeacher.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
         <asp:DataList ID="dlstCourseTeacher" runat="server">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <caption>
                        </caption>
                        <tr>
                            <th scope="col" width="100%">
                                教师简介<%=editTeacherAtag %>
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td class="row">
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
