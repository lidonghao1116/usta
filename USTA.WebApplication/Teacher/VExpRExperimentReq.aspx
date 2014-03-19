<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_VExpRExperimentReq" 
   MasterPageFile="~/MasterPage/ViewExperimentResources.master" Codebehind="VExpRExperimentReq.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/ViewExperimentResources.master" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
         <asp:DataList ID="dlstExperimentResource" runat="server" Width="100%">
            <ItemTemplate>
                <table class="datagrid2" width="100%">
                    <tr>
                        <th width="10%" style="text-align:center">
                            标题</th>
                        <th>
                            <%#Eval("experimentResourceTitle")%></th>
                    </tr>
                    <tr>
                        <td style="text-align:center">
                            内容</td>
                        <td>
                                <%#Eval("experimentResourceContent")%></td>
                    </tr>      
                    <tr>
                        <td style="text-align:center">
                            附件</td>
                        <td>&nbsp;<%#GetURL(Eval("attachementIds").ToString())%>
                                </td>
                    </tr>
                    <tr>
                        <td style="text-align:center">
                            上传时间</td>
                        <td><%#Eval("updateTime")%>
                                </td>
                    </tr>
                    <tr>
                        <td style="text-align:center">
                            截止时间</td>
                        <td>
                                <%#Eval("deadLine")%></td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>  
     </div>
</asp:Content>