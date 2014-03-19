<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="MyFeedBacks.aspx.cs" Inherits="USTA.WebApplication.Teacher.MyFeedBacks" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server">
  <asp:Label ID="lblCount" runat="server" Text="Label"></asp:Label>
<asp:DataList ID="dlstCETFeedback" runat="server"  Width="100%">
<HeaderTemplate>
<table class="datagrid2">
                                <tr>
                                    <th width="40%">
                                    反馈标题
                                       
                                    </th>
                                    <th width="20%">
                                        反馈时间
                                    </th>
                                    <th width="20%">
                                         反馈者姓名及联系方式</td>
                                    <th width="10%">
                                        状态
                                    </th>
                                        <th width="10%">
                                            操作
                                        </th>
                                </tr>
                            </table>
                            </HeaderTemplate>
<ItemTemplate>
    <table class="datagrid2">
                        <tr>
                            <td width="40%">
                               <asp:Label ID="lblfeedBackId" runat="server"  Text='<%#Eval("feedBackId")%>'  Visible=false></asp:Label>
                                <asp:CheckBox ID="ChkBox" ToolTip="选择/不选"  runat="server" />
                                &nbsp; &nbsp;  <img src="../images/BULLET.GIF" align="absmiddle" /> 
                                <%#(Eval("isRead").ToString().Trim() == "True") ? Eval("feedBackTitle").ToString().Trim() : "<b>" + Eval("feedBackTitle").ToString().Trim() + "</b>"%>
                            </td>
                            <td width="20%">
                             <%#(Eval("isRead").ToString().Trim() == "True") ? Eval("updateTime").ToString().Trim() : "<b>" + Eval("updateTime").ToString().Trim() + "</b>"%>                                
                            </td>
                            <td width="20%">
                            <%#Eval("feedBackContactTo").ToString().Trim() %>                           
                            </td>
                            <td width="10%">
                            
                            <%#(Eval("isRead").ToString().Trim()=="True")? "已读":"<b>未读</b>"%>
                            </td>
                            <td width="10%">
                            <a href="ViewFeedinfo.aspx?page=<%=pageIndex %>&feedBackId=<%#Eval("feedBackId")%>&keepThis=true&TB_iframe=true&height=300&width=500"" title="反馈查看" class="thickbox">查看详细信息</a>

                           
                            </td>
                        </tr>
                    </table>
</ItemTemplate>
<FooterTemplate><%=(this.dlstCETFeedback.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" />
</asp:DataList>
<input id="dzxBtnSelectAll" name="dzxBtnSelectAll" type="button" value="全选" onclick="selectAll();" />
             <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click" />
             
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager1" runat="server" UrlPaging="true" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager1_PageChanged"
                 PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
            </form>
</asp:Content>
