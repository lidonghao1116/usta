<%@ Page Language="C#" AutoEventWireup="true" Inherits="Student_CousrInfo_2_CouserInfo" 
    MasterPageFile="~/MasterPage/CourseInfoForStudent.master" Codebehind="CInfoCourseInfo.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForStudent.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
    <div class="ui-tabs-panel">
        <asp:DataList ID="dlstCourseInfos" runat="server">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <th scope="col" width="5%">
                                学时
                            </th>
                            <th scope="col" width="5%">
                                学分
                            </th>
                            <th scope="col" width="10%">
                                类型
                            </th>
                            <th scope="col" width="25%">
                                先修课程
                            </th>
                            <th scope="col" width="25%">
                                参考书
                            </th>
                            <th scope="col" width="10%">
                                课程（教师）主页
                            </th>
                            <th scope="col" width="10%">
                                考试方式
                            </th>
                            <th scope="col" width="10%">
                                答疑安排
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td class="row" width="5%">
                                <%#Eval("period")%>&nbsp;
                            </td>
                            <td class="row" width="5%">
                                <%#Eval("credit")%>&nbsp;
                            </td>
                            <td class="row" width="10%">
                                <%#Eval("courseSpeciality")%>&nbsp;
                            </td>
                            <td class="row" width="25%">
                                <%#Eval("preCourse")%>&nbsp;
                            </td>
                            <td class="row" width="25%">
                                <%#Eval("refferenceBooks")%>&nbsp;
                            </td>
                            <td class="row" width="10%">
                                <%#Eval("homePage")%>&nbsp;
                            </td>
                            <td class="row" width="10%">
                                <%#Eval("examineMethod")%>&nbsp;
                            </td>
                            <td class="row" width="10%">
                                <%#Eval("courseAnswer")%>&nbsp;
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstCourseInfos.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
    </div>
</asp:Content>
