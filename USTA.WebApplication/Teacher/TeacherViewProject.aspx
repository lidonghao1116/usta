<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="TeacherViewProject.aspx.cs" Inherits="USTA.WebApplication.Teacher.TeacherViewProject" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
 <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server">
<div id="container-1" style="padding-top: 40px;">

        <ul class="ui-tabs-nav">
            
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>项目列表</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>报销记录</span></a></li>
        </ul>
        
          <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <div>
            项目名称：<asp:TextBox ID="tb_ProjectName_ProjectFrag" Width="160px" runat="server"></asp:TextBox>&nbsp;
            &nbsp;&nbsp;<asp:Button ID="SearchProject" Text="查找" OnClick="SearchProject_Click" Width="60px" runat="server"/>            
            </div>
            <div>
                <asp:DataList ID="ProjectDataList" runat="server" DataKeyField="id" Width="100%">
                    <HeaderTemplate>
                        <table class="datagrid2">
                            <tr>
                                <th width="5%">编号</th>
                                <th width="45%">项目名称</th>
                                <th width="10%">分类</th>
                                <th width="40%">操作</th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="datagrid2">
                            <tr>
                                <td width="5%"><%#Container.ItemIndex + 1 %></td>
                                <td width="45%"><%#Eval("name") %></td>
                                <td width="10%"><%#Eval("category.name") %></td>
                                <td width="40%">
                                    <a href="ViewProject.aspx?projectId=<%#Eval("id") %>&keepThis=true&TB_iframe=true&height=300&width=500" title="查看项目详情" class="thickbox" >查看项目详情</a>
                                    <a href="TeacherViewProject.aspx?fragment=2&projectId=<%#Eval("id") %>">查看报销记录</a>                                             
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
                <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="ProjectDataList_Pager" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="ProjectDataListPager_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
            </div>
          </div>
          <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
          <div>
            项目名称：<asp:TextBox ID="tb_projectName" Width="200" runat="server"></asp:TextBox>
            &nbsp;&nbsp;<asp:Button ID="btn_QueryReimEntry" OnClick="QueryReimEntry_Click" Text="查找" Width="60px" runat="server"/>
          </div>
          <div>
          <asp:DataList ID="ReimEntryDataList" Width="100%" runat="server">
            <HeaderTemplate>
                <table class="datagrid2">
                    <tr>
                        <th width="5%">编号</th>
                        <th width="40%">项目名称</th>
                        <th width="20%">报销项名称</th>
                        <th width="10%">报销金额</th>
                        <th width="20%">操作</th>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table class="datagrid2">
                    <tr>
                        <td width="5%"><%#Container.ItemIndex + 1 %></td>
                        <td width="40%"><%#Eval("project.name") %></td>
                        <td width="20%"><%#Eval("reim.name") %></td>
                        <td width="10%"><%#Eval("value") %></td>
                        <td width="20%">
                            <a href="ViewReimEntry.aspx?keepThis=true&projectId=<%#Eval("project.id") %>&reimId=<%#Eval("reim.id") %>&TB_iframe=true&height=300&width=600" title="查看详情" class="thickbox" >查看详情</a>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterTemplate></FooterTemplate>
          </asp:DataList>
          <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="ReimEntryPager" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="ReimEntryPager_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>  
          </div>
          </div>
</div>
</form>
</asp:Content>

