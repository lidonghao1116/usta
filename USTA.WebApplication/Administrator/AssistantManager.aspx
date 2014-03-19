<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Administrator_AssistantManager" MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="AssistantManager.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />

    

    <!--Validate-->

    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>

    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#aspnetForm").validate();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"  visible="false"><a href="?fragment=1"><span>Excel导入助教信息</span></a></li>
            <li id="liFragment2" runat="server"  visible="false"><a href="?fragment=2"><span>添加助教信息</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>搜索助教信息</span></a></li>
            <li id="liFragment4" runat="server" visible="false"><a href="#"><span>助教</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        </div>
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
                <table style="margin-bottom:10px">
                    <tr>
                        <td>
                            <asp:Label ID="lblKeyword" runat="server" Text="关键字(用户ID或者名字)：" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnQuery" runat="server" Text="模糊查询" OnClick="btnQuery_Click" />
                        </td>
                    </tr>
                </table>
                <asp:DataList ID="dlSearchAssistant" runat="server" DataKeyField="assistantNo" OnItemCommand="dlSearchAssistant_ItemCommand"
                    Width="100%">
                    <HeaderTemplate>
                        <table class="datagrid2">
                            <tr>
                                <th width="30%">
                                    助教编号
                                </th>
                                <th width="15%">
                                    助教名称
                                </th>
                                <th width="15%">
                                    邮件地址
                                </th>
                                <th width="40%">
                                    操作
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="datagrid2">
                            <tr>
                                <td width="30%">
                                    <%#Eval("assistantNo")%>
                                </td>
                                <td width="15%">
                                    <%#Eval("assistantName")%>
                                </td>
                                <td width="15%">
                                    <%#Eval("emailAddress")%>
                                </td>
                                <td width="40%">
                                    <a href="ViewAssistantInfo.aspx?keepThis=true&&assistantNo=<%#Eval("assistantNo").ToString().Trim()%>&TB_iframe=true&height=300&width=500"
                                        title="查看助教详细信息" class="thickbox">查看详细信息</a>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="cancelAssistant" OnClientClick="return deleteTip();">取消助教身份</asp:LinkButton>
                                    <a href="AssistantManager.aspx?fragment=4&assistantNo=<%#Eval("assistantNo").ToString().Trim()%>">进入任课管理</a>
                                </td>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.dlSearchAssistant.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" />
                </asp:DataList>
                <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager2" runat="server" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager2_PageChanged"
                    PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
                </webdiyer:AspNetPager>
          
            </div>
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment4" runat="server">
                教师：<asp:Label ID="lblassistamtName" runat="server" Text="Label"></asp:Label><a href="AddCourseforAssitant.aspx?keepThis=true&assistantNo=<%=assistantNo%>&TB_iframe=true&height=350&width=600"
                    title="添加课程" class="thickbox"> 添加课程</a><br />
                <asp:DataList ID="dlstcoursesOfAssistamt" runat="server" Width="40%">
                    <HeaderTemplate>
                        <table class="datagrid2">
                            <tr>
                                <th width="70%">
                                    课程名称
                                </th>
                                <th width="30%">
                                    操作
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="datagrid2">
                            <tr>
                                <td width="70%">
                                    <%#Eval("courseName").ToString().Trim()%>(<%#Eval("ClassID").ToString().Trim()%>)
                                </td>
                                <td width="30%">
                                    <a href="AssistantManager.aspx?del=true&coursesTeachersCorrelationId=<%#Eval("coursesTeachersCorrelationId").ToString().Trim()%>&assistantNo=<%#Eval("assistantNo").ToString().Trim()%>&courseNo=<%#Eval("courseNo").ToString().Trim()%>&fragment=4">
                                        删除</a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </div>
    </div>
    </form>
</asp:Content>
