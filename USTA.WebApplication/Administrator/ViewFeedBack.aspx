<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_ViewFeedBack"
    MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="ViewFeedBack.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>查看反馈</span></a></li>
            <li id="liFragment2" class="ui-tabs-hide" runat="server"><a href="?fragment=2"><span>
                暂时不使用</span></a></li>
            <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="?fragment=3"><span>
                暂时不使用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <asp:Label ID="lblCount" runat="server" Text="Label"></asp:Label>
            <asp:DataList ID="dlFeedBack" runat="server" DataKeyField="feedBackId"
                        Width="100%" >
                        <HeaderTemplate>
                            <table class="datagrid2">
                                <tr>
                                    <th width="30%">
                                    反馈标题
                                       
                                    </th>
                                    <th width="20%">
                                        反馈时间
                                    </th>
                                    <th width="20%">
                                         反馈者姓名及联系方式</th>
                                    <th width="10%">
                                        反馈类型
                                    </th>
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
                            <td width="30%">
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
                                    <%#USTA.Common.CommonUtility.ReturnFeedBackTypeByVal(int.Parse(Eval("type").ToString()))%>
                                </td>
                            <td width="10%">
                            
                            <%#(Eval("isRead").ToString().Trim()=="True")? "已读":"<b>未读</b>"%>
                            </td>
                            <td width="10%">
                            <a href="ViewFeedinfo.aspx?feedBackId=<%#Eval("feedBackId")%>&keepThis=true&TB_iframe=true&height=300&width=500"" title="反馈查看" class="thickbox">查看详细信息</a>

                           
                            </td>
                        </tr>
                    </table>
                </ItemTemplate><FooterTemplate><%=(this.dlFeedBack.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
             <input id="dzxBtnSelectAll" name="dzxBtnSelectAll" type="button" value="全选" onclick="selectAll();" />
             <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click" />
             
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" UrlPaging="true" ID="AspNetPager1" runat="server" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager1_PageChanged"
                 PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
    </div>
    </form>
</asp:Content>
