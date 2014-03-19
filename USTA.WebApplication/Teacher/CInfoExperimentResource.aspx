<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_CInfoExperimentResource" 
    MasterPageFile="~/MasterPage/CourseInfoForTeacher.master" Codebehind="CInfoExperimentResource.aspx.cs" %>

<%@ MasterType  VirtualPath="~/MasterPage/CourseInfoForTeacher.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
         <a href="AddExperimentResource.aspx?keepThis=true&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID) %>&termtag=<%=Master.termtag %>&fragment=6&TB_iframe=true&height=500&width=900"
                title="布置课程实验" class="thickbox">布置课程实验</a>
            <asp:DataList ID="dlstExperimentResource" runat="server" Width="60%">
                <HeaderTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <caption>
                        </caption>
                        <tr>
                            <th scope="col" width="50%">
                                实验名称
                            </th>
                            <th scope="col" width="50%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="mytable mytableWidth" cellspacing="0">
                        <tr>
                            <td class="row" width="50%">
                                <a href="VExpRExperimentReq.aspx?experimentResourceId=<%#Eval("experimentResourceId") %>&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>">
                                    <%#Eval("experimentResourceTitle")%></a> <%#(isNew(Eval("updateTime").ToString()))?"<img src='../images/new.gif' align='middle' style='margin-bottom:5px;'/>":"" %>
                            </td>
                            <td class="row" width="50%">
                                <a href="EditExperimentResource.aspx?keepThis=true&experimentResourceId=<%#Eval("experimentResourceId")%>&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>&TB_iframe=true&height=500&width=900"
                                    title="修改实验" class="thickbox">[编辑]</a>
                                
                                <a href="CInfoExperimentResource.aspx?experimentResourceId=<%#Eval("experimentResourceId")%>&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>&op=delete"
                                    onclick="return deleteTip();">[删除]</a>
                                   <a href="VExpRExperimentReq.aspx?experimentResourceId=<%#Eval("experimentResourceId") %>&courseNo=<%=Master.courseNo.ToString().Trim()%>&classID=<%=Server.UrlEncode(Master.classID).ToString().Trim() %>&termtag=<%=Master.termtag.ToString().Trim() %>">
                                   [进入查看]</a> 
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstExperimentResource.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
     </div>
</asp:Content>
