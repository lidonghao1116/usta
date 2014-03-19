<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_VSWorkNWorkReq" 
    MasterPageFile="~/MasterPage/ViewSchoolworkNotify.master" Codebehind="VSWorkNWorkReq.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/ViewSchoolworkNotify.master" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
        <asp:DataList ID="dlstSchoolworkNotify" runat ="server" Width="100%" CellPadding="0" CellSpacing="0">
            <ItemTemplate>
                <table class="datagrid2" width="100%">
                    <tr>
                        <th width="10%" style="text-align:center" >标题：</th>
                        <th><%#Eval("schoolWorkNotifyTitle") %></th>
                    </tr>
                    <tr>
                        <td style="text-align:center">内容：</td>
                        <td><%#Eval("schoolWorkNotifyContent") %></td>
                    </tr>
                    <tr>
                        <td style="text-align:center">发布时间：</td>
                        <td><%#Eval("updateTime") %></td>
                    </tr>
                    <tr>
                        <td style="text-align:center">截止时间：</td>
                        <td><%#Eval("deadline") %></td>
                    </tr>
                    <tr>
                        <td style="text-align:center">提交方式：</td>
                        <td><%#(Eval("isOnline").ToString()=="True")?"在线提交":"书面提交" %></td>
                    </tr>
                    <tr>
                         <td style="text-align:center">附件：</td><td><%#GetURL(Eval("attachmentIds").ToString())%></td>
                    </tr>
                </table>
            </ItemTemplate>
       </asp:DataList>
    </div>
</asp:Content>