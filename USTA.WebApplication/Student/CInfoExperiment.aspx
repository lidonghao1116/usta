<%@ Page Language="C#" AutoEventWireup="true" Inherits="Student_CousrInfo_9_Experiment"
    MasterPageFile="~/MasterPage/CourseInfoForStudent.master" Codebehind="CInfoExperiment.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/CourseInfoForStudent.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="headcontent" ContentPlaceHolderID="CouserInfoHeat" runat="server">
</asp:Content>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
    
    <div class="ui-tabs-panel">
            <div id="divOfExperiment" runat="server">
                <asp:DataList ID="dlstExperiment" runat="server" Width="100%">
                    <ItemTemplate>
                        <table width="100%" class="tableEditStyle">
                            <tr>
                                <td colspan="2">
                                    <h3>
                                        <%#Eval("experimentResourceTitle").ToString().Trim()%>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    内容
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%#Eval("experimentResourceContent").ToString().Trim()%>
                                </td>
                            </tr>
                            <tr>
                                <td width="10%">
                                    附件
                                </td>
                                <td>
                                    <%#GetURL(Eval("attachementIds").ToString().Trim())%>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td width="10%">
                                    上传时间
                                </td>
                                <td>
                                    <%#Eval("updateTime").ToString().Trim()%>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-bottom:none;">
                                    截止时间
                                </td>
                                <td style="border-bottom:none;">
                                    <%#Eval("deadLine").ToString().Trim()%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
                <span id="experimentspan" runat="server"></span><br />
                <asp:Button ID="btnExperiment" ClientIDMode="Static" runat="server"  OnClientClick="if($.trim($('#hidAttachmentId').val()).length == 0){alert('请上传实验文件！');return false;}else{delBeforeUnloadEvent();}"  OnClick="btnExperiment_Click" Text="提交" />
            </div>
    </div>
     <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                <asp:HiddenField ID="hidOriginalAttachmentId" ClientIDMode="Static" runat="server" Value="" />
</asp:Content>