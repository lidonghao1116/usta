<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_VExpRExperimentUnSubed" 
    MasterPageFile="~/MasterPage/ViewExperimentResources.master" Codebehind="VExpRExperimentUnSubed.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/ViewExperimentResources.master" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
             <div>
        姓名：<asp:TextBox ID="txtNameSearch" runat="server"></asp:TextBox>&nbsp;<asp:Button 
                ID="btnSubmitSearch" runat="server" Text="提交" onclick="btnSubmitSearch_Click" />
        </div>
         <asp:DataList ID="dlNoExp" runat="server" DataKeyField="studentNo" Width="100%">
             <HeaderTemplate>
               <table class="datagrid2"> 
                  <tr>
                    <th width="40%">学生编号</th>
                    <th width="40%">学生姓名</th>
                 </tr>
                 </table> 
               </HeaderTemplate>
            <ItemTemplate>
                <table  class="datagrid2">
                    <tr >
                    <td width="40%"><%#Eval("studentNo") %></td>
                    <td width="40%"><%#Eval("studentName") %></td>                   
                   </tr>                    
               </table>
            </ItemTemplate><FooterTemplate><%=(this.dlNoExp.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" />          
        </asp:DataList> 
         <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager1" runat="server" UrlPaging="true" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager1_PageChanged"
                         PrevPageText="上一页"
                         LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;" HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
         </webdiyer:aspnetpager>
     </div>
</asp:Content>
