<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" Inherits="Administrator_ViewChooseCourse" Codebehind="ViewChooseCourse.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="form1" runat="server">
学生：
 
    <asp:Label ID="lblstudentName" runat="server" Text="Label"></asp:Label>
&nbsp;&nbsp; <a href="AddCourseforStudent.aspx?keepThis=true&studentNo=<%=studentNo%>&TB_iframe=true&height=350&width=600" title="添加课程" class="thickbox"> 添加课程</a><br />
    所选课程：<br />
    <asp:DataList ID="dlstcourses" runat="server" Width="40%">
    <HeaderTemplate>
                            <table class="datagrid2">
                                <tr>
                                    <th width="70%">
                                        课程名称
                                    </th>                                 
                                        <th width="30%">
                                            操作
                                        </th>
                                </tr>
                            </table>
                        </HeaderTemplate>
    <ItemTemplate>
    <table class="datagrid2"><tr><td width="70%"><%#Eval("courseName")%></td><td width="30%"><a href="ViewChooseCourse.aspx?del=true&studentNo=<%=studentNo %>&courseNo=<%#Eval("courseNo")%>">删除</a></td></tr>
    </table>
    </ItemTemplate><FooterTemplate><%=(this.dlstcourses.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" />
    </asp:DataList>
    </form>
</asp:Content>

