<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_CInfoCourseInfos"
    MasterPageFile="~/MasterPage/CourseInfoForTeacher.master" Codebehind="CInfoCourseInfos.aspx.cs" %>

<%@ MasterType  VirtualPath="~/MasterPage/CourseInfoForTeacher.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
         <asp:DataList ID="dlstCourseInfos" runat="server">
                <HeaderTemplate>
                   课程信息&nbsp;<a href="EditCourseinfoTip.aspx?keepThis=true&courseNo=<%=Master.courseNo%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>&TB_iframe=true&height=600&width=650"
                                    title="修改课程基本信息" class="thickbox">编辑</a> 
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
                    <table class="datagrid2" cellspacing="0">
                        <tr>
                            <td class="row" width="5%">
                                <%#Eval("period").ToString().Trim()%>
                            </td>
                            <td class="row" width="5%">
                                <%#Eval("credit").ToString().Trim()%>
                            </td>
                            <td class="row" width="10%">
                                <%#Eval("courseSpeciality").ToString().Trim()%>
                            </td>
                            <td class="row" width="25%">
                                <%#Eval("preCourse".ToString().Trim())%>
                            </td>
                            <td class="row" width="25%">
                                <%#Eval("refferenceBooks").ToString().Trim()%>
                            </td>
                            <td class="row" width="10%">
                            <a href="<%#Eval("homePage").ToString().Trim().IndexOf("http://")>0?Eval("homePage").ToString().Trim():("http://"+Eval("homePage").ToString().Trim())%>" target="_blank"><%#Eval("homePage")%></a>    
                            </td>
                            <td class="row" width="10%">
                                <%#Eval("examineMethod").ToString().Trim()%>
                            </td>
                            <td class="row" width="10%">
                                <%#Eval("courseAnswer").ToString().Trim()%>
                            </td>
                        </tr>
                    </table>
                   
                </ItemTemplate>
                <FooterTemplate><%=(this.dlstCourseInfos.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" BorderWidth="0" />
            </asp:DataList>
           
     </div>
</asp:Content>
