<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_VSWorkNUnlineWork" 
     MasterPageFile="~/MasterPage/ViewSchoolworkNotify.master" Codebehind="VSWorkNPaperWork.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/ViewSchoolworkNotify.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="content1" ContentPlaceHolderID="CouserInfoContent" runat="server">
     <div class="ui-tabs-panel">
         <asp:DataList ID="dlstpaperwork" runat="server" Width="90%">
            <HeaderTemplate>
               <table class="datagrid2"> 
                  <tr>
                    <th width="20%">学生编号</th>
                    <th width="20%">学生姓名</th>
                    <th width="20%">操作</th>
                    <th width="20%">评语</th>
                    <th width="20%">得分</th>
                 </tr>
                 </table> 
               </HeaderTemplate>
               <ItemTemplate>
               <table  class="datagrid2">
                    <tr >
                    <td width="20%"><%#Eval("studentNo") %></td>
                    <td width="20%"><%#Eval("studentName") %></td>
                    <td  width="20%"><%#((bool.Parse(Eval("isCheck").ToString())) ? "<a href='VSCheckSchoolworks.aspx?isOnline=0&courseNo=" + Master.courseNo + "&termTag=" + Master.termtag + "&classID=" + Server.UrlEncode(Master.classID) + "&schoolworkNotifyId=" + Master.schoolworkNotifyId + "&schoolWorkId=" + Eval("schoolWorkId").ToString().Trim() + "' title='" + "批改课程作业'>" + "重新批阅" + @"</a>" : "<a href='VSCheckSchoolworks.aspx?isOnline=0&courseNo=" + Master.courseNo + "&termTag=" + Master.termtag + "&classID=" + Server.UrlEncode(Master.classID) + "&schoolworkNotifyId=" + Master.schoolworkNotifyId + "&schoolWorkId=" + Eval("schoolWorkId").ToString().Trim() + "' title='" + "批改课程作业'>" + "批阅" + @"</a>")%></td>        
                   <td width="20%"> <%#Eval("remark")%></td>                
                   <td width="20%"> <%#Eval("score") %></td>
                   </tr>                   
               </table>
               </ItemTemplate>
        </asp:DataList>
        <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager3" runat="server" UrlPaging="true" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager3_PageChanged"
                         PrevPageText="上一页"
                         LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;" HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
        </webdiyer:AspNetPager>
     </div>
</asp:Content>