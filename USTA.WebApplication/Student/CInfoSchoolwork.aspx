<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CourseInfoForStudent.master" AutoEventWireup="true" Inherits="Student_CInfoSchoolwork" Codebehind="CInfoSchoolwork.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForStudent.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CouserInfoHeat" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CouserInfoContent" Runat="Server">
<div class="ui-tabs-panel">
<div id="divOfSchoolwork" runat="server">
                <asp:DataList ID="dlstSchoolWork" runat="server" Width="100%">
                    <ItemTemplate>
                        <table width="100%" class="tableEditStyle">
                            <tr>
                                <td colspan="2">
                                    <h3>
                                        <%#Eval("schoolWorkNotifyTitle").ToString().Trim()%>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td width="10%">
                                    内容：
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%#Eval("schoolWorkNotifyContent").ToString().Trim()%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    发布时间：
                                </td>
                                <td>
                                    <%#Eval("updateTime").ToString().Trim()%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    截止时间：
                                </td>
                                <td>
                                    <%#Eval("deadline").ToString().Trim()%>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-bottom:none;">
                                    附件：
                                </td>
                                <td style="border-bottom:none;">
                                    <%#GetURL(Eval("attachmentIds").ToString().Trim())%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
                <span id="isSchoolcommitspan" runat="server"></span><br />
                <asp:Button ID="btnSchoolworkCommit" Text="提交" ClientIDMode="Static" runat="server" OnClientClick="if($.trim($('#hidAttachmentId').val()).length == 0){alert('请上传作业文件！');return false;}else{delBeforeUnloadEvent();}" OnClick="btnSchoolworkCommit_Click" />
                </div>
                <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                <asp:HiddenField ID="hidOriginalAttachmentId" ClientIDMode="Static" runat="server" Value="" />
</div>
</asp:Content>

