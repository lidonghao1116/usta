<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_ViewHistoryData" MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="ViewHistoryData.aspx.cs" %>


<%@ Register assembly="AspNetPager" namespace="Wuqi.Webdiyer" tagprefix="webdiyer" %>
        
        <asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="form1" runat="server">
    
        <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>查看历史课程</span></a></li>
            <li id="liFragment2" class="ui-tabs-hide" runat="server"><a href="?fragment=2"><span>
                历史课程</span></a></li>
            <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="?fragment=3"><span>
                暂时不使用</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
        <asp:DataList ID="dlTermCourse" runat="server" DataKeyField="termTag" Width="100%" CssClass="multiRecordsDataList">
                    <ItemTemplate><img src="../images/BULLET.GIF" align="middle" /><a href="/Common/ViewHistoryData.aspx?fragment=2&termTag=<%#Eval("termTag").ToString()%>"><%#USTA.Common.CommonUtility.ChangeTermToString(Eval("termTag").ToString())%></a>
                    </ItemTemplate><FooterTemplate><%=(this.dlTermCourse.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" BorderWidth="0" />
                    </asp:DataList>
        </div>
        
        
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        <asp:DataList ID="DataList1" runat="server" Width="100%" CssClass="multiRecordsDataList" RepeatDirection="Horizontal" RepeatColumns="4">
                    <ItemTemplate>  
                      <img src="../images/BULLET.GIF" align="middle" /><a href="/Common/CInfoCourseIntro.aspx?courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("ClassID").ToString().Trim()) %>&termtag=<%#Eval("termTag") %>"><%#Eval("courseName").ToString().Trim()%>(<%#Eval("ClassID").ToString().Trim()%>)</a>
                     </ItemTemplate><FooterTemplate><%=(this.DataList1.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" BorderWidth="0" />
                    </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
        </div>
    
    </form>
</asp:Content>
