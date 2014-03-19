<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="ShowUSTALogs.aspx.cs" Inherits="USTA.WebApplication.Administrator.ShowUSTALogs" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="form1" runat="server">
<div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>系统日志查看</span></a></li>
            <li id="liFragment2" class="ui-tabs-hide" runat="server"><a href="?fragment=2"><span>
                暂时不使用</span></a></li>
            <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="?fragment=3"><span>
                暂时不使用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
        <br />
        <br />
        请输入要查看的日志日期，格式如：2011-03-20   <input type="text" id="txtDateTime" runat="server" onclick="WdatePicker({startDate:'%y-%M-01',dateFmt:'yyyy-MM-dd',alwaysUseStartDate:true})"
                class="required" clientidmode="Static" /> 
        <input id="Button1" type="button" value="确定" onclick="location.href='ShowUSTALogs.aspx?datetime='+$('#txtDateTime').val();" /><asp:DataList ID="DataListUSTALogs" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" Width="100%">
                <ItemTemplate>
                <table class="datagrid2" style="border-top:1px #CAE8EA solid;">
                                <tr>
                                <td>出错时间：</td>
                                    <td width="80%">
                                        <%#Eval("errorTime")%>
                                    </td>
                                    </tr>
                                <tr>
                                <td>出错页面地址：</td>
                                    <td width="80%">
                                        <%#Eval("errorUrl")%>
                                    </td>
                                    </tr>
                               <tr>
                                <td>访问者IP为：</td>
                                    <td width="80%">
                                        <%#Eval("accessIP")%>
                                    </td>
                                    </tr>
                               <tr>
                                <td>异常码为：</td>
                                    <td width="80%">
                                        <%#Eval("errorCode")%>
                                    </td>
                                    </tr>
                               <tr>
                                <td>出错对象为：</td>
                                    <td width="80%">
                                        <%#Eval("errorObject")%>
                                    </td>
                                </tr>
                               <tr>
                                <td>堆栈信息为：</td>
                                    <td width="80%">
                                        <%#Eval("errorStackTrace")%>
                                    </td>
                                </tr>
                               <tr>
                                <td>出错函数为：</td>
                                    <td width="80%">
                                        <%#Eval("errorMethod")%>
                                    </td>
                                </tr>
                               <tr>
                                <td>出错信息为：</td>
                                    <td width="80%">
                                        <%#Eval("errorMessage")%>
                                    </td>
                                </tr>
                               <tr>
                                <td>参考帮助链接为：</td>
                                    <td width="80%">
                                        <%#Eval("errorHelpLink")%>
                                    </td>
                                    </tr>
                            </table>
                            <br />
                </ItemTemplate>
                <FooterTemplate><%=(this.DataListUSTALogs.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
            </asp:DataList> <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager2" runat="server" FirstPageText="首页" LastPageText="尾页" PrevPageText="上一页" NextPageText="下一页" 
                         UrlPaging="true" EnableUrlRewriting="true" UrlRewritePattern="ShowUSTALogs.aspx?datetime={0}&page=%CurrentPageIndex%"
                         LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;" HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
                    </webdiyer:AspNetPager>
        </div>
        
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
        <br />
    </div>
    </form>
</asp:Content>
