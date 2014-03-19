<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_FeedBack" MasterPageFile="~/MasterPage/FrameManage.master"
    CodeBehind="FeedBack.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>意见反馈</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>查看意见反馈回复</span></a></li>
            <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="#"><span>暂时不使用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <table align="center" width="100%" class="tableAddStyleNone">
            <tr>
                    <td width="200px" class="border">
                        反馈类型：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddltType" runat="server">
                            <asp:ListItem Value="1" Text="系统反馈"></asp:ListItem>
                            <asp:ListItem Value="2" Text="四六级报名反馈"></asp:ListItem>
                            <asp:ListItem Value="3" Text="成绩审核反馈"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        反馈标题：
                    </td>
                    <td class="border">
                        <asp:TextBox ID="txtTitle" runat="server" Width="339px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">                        
                        反馈内容：
                    </td>
                    <td class="border">
                            <asp:TextBox ID="txtContent" runat="server" Height="200px" TextMode="MultiLine" Width="600px"></asp:TextBox>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        联系方式(电话,E-mail或者匿名)：
                    </td>
                    <td class="border"><asp:TextBox ID="txtContact" runat="server" Width="339px"></asp:TextBox>
                    </td>
                </tr>
                <tr><td width="200px" class="border">    
                    </td>
                    <td class="border">
                        <asp:Button ID="btnConfirm" runat="server" Text="提交反馈信息" OnClick="btnConfirm_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <div align="center" class="feedstable">
                <asp:DataList runat="server" ID="dlstfeeds" Width="100%">
                    <HeaderTemplate>
                        <table class="datagrid2" width="100%" style="margin-top: 30px;">
                            <tr>
                                <th width="25%">
                                    标题
                                </th>
                                <th width="25%">
                                    时间
                                </th>
                                <th width="15%">
                                    状态
                                </th>
                                <th width="20%">
                                    反馈类型
                                </th>
                                <th width="15%">
                                    操作
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="datagrid2" width="100%">
                            <tr>
                                <td width="25%">
                                    <%#(Eval("isRead").ToString().ToLower().Equals("false"))?"<b>"+Eval("feedBackTitle")+"</b>":Eval("feedBackTitle")%>
                                </td>
                                <td width="25%">
                                    <%#(Eval("isRead").ToString().ToLower().Equals("false")) ? "<b>" + Eval("updateTime") + "</b>" : Eval("updateTime")%>
                                </td>
                                <td width="15%">
                                    <%#(Eval("isRead").ToString().ToLower().Equals("false")) ? "<b>未读</b>" : "已读"%>
                                </td>
                                <td width="20%">
                                    <%#USTA.Common.CommonUtility.ReturnFeedBackTypeByVal(int.Parse(Eval("type").ToString()))%>
                                </td>
                                <td width="15%">
                                    <a href="Viewfeedbackinfo.aspx?feedbackId=<%#Eval("feedBackId")%>&keepThis=true&TB_iframe=true&height=300&width=500"
                                        title="查看反馈意见" class="thickbox">查看</a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.dlstfeeds.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" />
                </asp:DataList>
            </div>
            <p>
                &nbsp;</p>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
    </div>
    </form>
</asp:Content>
