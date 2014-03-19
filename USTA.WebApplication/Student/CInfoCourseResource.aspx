<%@ Page Language="C#" AutoEventWireup="true" Inherits="Student_CousrInfo_5_CourseResource"    
    MasterPageFile="~/MasterPage/CourseInfoForStudent.master" Codebehind="CInfoCourseResource.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForStudent.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
    <div class="ui-tabs-panel">
         <asp:DataList ID="dlstCourseResource" runat="server">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <th scope="col" width="20%">
                                标题
                            </th>
                            <th scope="col" width="80%">
                                内容
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td width="20%">
                                <%#Eval("courseResourceTitle")%>
                            </td>
                            <td width="80%">
                                <%#GetURL(Eval("attachmentIds").ToString()) %>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstCourseResource.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
    </div>
</asp:Content>