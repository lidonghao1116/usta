<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_CInfoCourseIntro" 
   MasterPageFile="~/MasterPage/CourseInfoForCommon.master" Codebehind="CInfoCourseIntro.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForCommon.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="CouserInfoContent">
    <div class="ui-tabs-panel">
        <asp:DataList ID="dlstCourseIntro" runat="server">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <th scope="col">
                                课程简介
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td class="row">
                                <%#Eval("courseIntroduction")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <ItemStyle CssClass="row" />
                <FooterTemplate>
                    <%=(this.dlstCourseIntro.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
    </div>
</asp:Content>