<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_CInfoCourseResource" 
    MasterPageFile="~/MasterPage/CourseInfoForCommon.master" Codebehind="CInfoCourseResource.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForCommon.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="CouserInfoContent">
    <div class="ui-tabs-panel">
         <asp:DataList ID="dlstCourseResource" runat="server">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <caption>
                        </caption>
                        <tr>
                            <th scope="col" width="20%">
                                资源标题
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
                                    <%#Eval("courseResourceTitle")%><%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
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
