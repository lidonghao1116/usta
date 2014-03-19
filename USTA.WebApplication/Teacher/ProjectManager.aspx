<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="ProjectManager.aspx.cs" Inherits="USTA.WebApplication.Teacher.ProjectManager" %>
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
            <li id="liFragment2" runat="server" visible="false"><a href="#"><span>项目详情</span></a></li>
            <li id="liFragment3" runat="server"  ><a href="?fragment=3"><span>类目管理</span></a></li>
            <li id="liFragment4" runat="server"><a href="?fragment=4"><span>报销项管理</span></a></li>
            <li id="liFragment5" runat="server"><a href="?fragment=5"><span>报销记录</span></a></li>
            <li id="liFragment6" runat="server"><a href="?fragment=6"><span>报销规则管理</span></a></li>
        </ul>
        
          <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <div>
            项目名称：<asp:TextBox ID="tb_ProjectName_ProjectFrag" Width="160px" runat="server"></asp:TextBox>&nbsp;
            负责人：<asp:TextBox ID="tb_UserName_ProjectFrag" Width="100px" runat="server"></asp:TextBox>&nbsp;
            项目分类：<asp:DropDownList ID="SearchProject_RootCategory" runat="server" OnSelectedIndexChanged="SearchProject_RootCategory_Changed" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="SearchProject_SubCategory" runat="server" OnSelectedIndexChanged="SearchProject_SubCategory_Changed" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="SearchProject_ThirdCategory" runat="server" OnSelectedIndexChanged="SearchProject_ThirdCategory_Changed" AutoPostBack="true"></asp:DropDownList>
            &nbsp;&nbsp;<asp:Button ID="SearchProject" Text="查找" OnClick="SearchProject_Click" Width="60px" runat="server"/>
            <asp:Button ID="ExportProject" Text="导出" OnClientClick="javascript:doExportProject();" Width="60px" runat="server" />
            &nbsp;&nbsp;<a href="AddProject.aspx?keepThis=true&TB_iframe=true&height=300&width=500" title="添加新项目" class="thickbox" >添加新项目</a>
            </div>
            <div>
                <asp:DataList ID="ProjectDataList" runat="server" DataKeyField="id" Width="100%">
                    <HeaderTemplate>
                        <table class="datagrid2">
                            <tr>
                                <th width="5%">编号</th>
                                <th width="30%">项目名称</th>
                                <th width="15%">负责人</th>
                                <th width="10%">分类</th>
                                <th width="40%">操作</th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="datagrid2">
                            <tr>
                                <td width="5%"><%#Container.ItemIndex + 1 %></td>
                                <td width="30%"><%#Eval("name") %></td>
                                <td width="15%"><%#Eval("userName") %></td>
                                <td width="10%"><%#Eval("category.name") %></td>
                                <td width="40%">
                                    <a href="AddReimRule.aspx?fragment=5&projectId=<%#Eval("id") %>&keepThis=true&TB_iframe=true&height=300&width=500" class="thickbox">添加报销规则</a>     
                                    <a href="AddReimEntry.aspx?projectId=<%#Eval("id") %>&keepThis=true&TB_iframe=true&height=300&width=500" title="添加项目报销" class="thickbox" >添加项目报销</a>
                                    <a href="ViewProject.aspx?projectId=<%#Eval("id") %>&keepThis=true&TB_iframe=true&height=300&width=500" title="查看项目详情" class="thickbox" >查看项目详情</a>
                                    <a href="ProjectManager.aspx?fragment=5&projectId=<%#Eval("id") %>">查看报销记录</a>                    
                                      <a href="AddProject.aspx?op=edit&pid=<%#Eval("id") %>&keepThis=true&TB_iframe=true&height=300&width=500" title="编辑项目" class="thickbox" >编辑</a>                      
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
          </div>
          <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
          <div>
            <asp:DropDownList ID="ProjectCategoryDdList" runat="server" OnSelectedIndexChanged="ProjectCategoryDdList_Changed" AutoPostBack="true">
                <asp:ListItem Value="0">所有类目</asp:ListItem>
                <asp:ListItem Value="1">一级类目</asp:ListItem>
                <asp:ListItem Value="2">二级类目</asp:ListItem>
                <asp:ListItem Value="3">三级类目</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="RootProjectCategory" runat="server" OnSelectedIndexChanged="RootCategory_Changed" AutoPostBack="true"></asp:DropDownList>
            <asp:DropDownList ID="SubProjectCategory" runat="server" OnSelectedIndexChanged="SubCategory_Changed" AutoPostBack="true"></asp:DropDownList>
            &nbsp;&nbsp;<a href="AddNewProjectCategory.aspx?keepThis=true&TB_iframe=true&height=300&width=500" title="添加新类目" class="thickbox" >添加新类目</a>
          </div>
          <div>
              <asp:DataList ID="dlstCategory" runat="server" Width="100%">
              <HeaderTemplate>
                 <table class="datagrid2">
                    <tr>
                        <th width="5%">编号</th>
                        <th width="20%">类目名称</th>
                        <th width="55%">描述</th>
                        <th width="20%">操作</th>
                    </tr>
                </table>
              </HeaderTemplate>
              <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="5%"><%#Container.ItemIndex + 1 %></td>
                            <td width="20%"><%#Eval("name") %></td>
                            <td width="55%"><%#Eval("memo") %></td>
                            <td width="20%">
                                <a href="AddNewProjectCategory.aspx?cid=<%#Eval("id") %>&op=edit&keepThis=true&TB_iframe=true&height=300&width=500" title="编辑类目" class="thickbox" >编辑类目</a>
                            </td>
                         </tr>
                     </table>
              </ItemTemplate>
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="ProjectCategoryPager" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="ProjectCategoryPager_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
          </div>
          </div>
          <div class="ui-tabs-panel ui-tabs-hide" id="divFragment4" runat="server">
          <div>
            <a href="AddNewReim.aspx?keepThis=true&TB_iframe=true&height=300&width=500" title="添加报销项" class="thickbox" >添加报销项</a>
          </div>
          <div>
            <asp:DataList ID="ReimDataList" Width="100%" runat="server">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="10%">
                                编号
                            </th>
                            <th width="60%">
                                报销项
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
                            <td width="10%">
                                <%#Container.ItemIndex + 1 %>
                            </td>
                            <td width="60%">
                                <%#Eval("name") %>
                            </td>
                            <td width="30%">
                                <a href="ViewReimDetail.aspx?reimId=<%#Eval("id") %>&keepThis=true&TB_iframe=true&height=300&width=500" title="查看详情" class="thickbox" >查看详情</a>
                                <a href="AddNewReim.aspx?op=edit&reimId=<%#Eval("id") %>&keepThis=true&TB_iframe=true&height=300&width=500" title="编辑" class="thickbox">编辑</a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                
                </FooterTemplate>
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="ReimPager" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="ReimPager_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
          </div>
          </div>
          <div class="ui-tabs-panel ui-tabs-hide" id="divFragment5" runat="server">
          <div>
            项目名称：<asp:TextBox ID="tb_projectName" Width="200" runat="server"></asp:TextBox>
            &nbsp;&nbsp;<asp:Button ID="btn_QueryReimEntry" OnClick="QueryReimEntry_Click" Text="查找" Width="60px" runat="server"/>
            <asp:Button ID="btn_ExportReimEntry" Text="导出" Width="60px" runat="server" OnClientClick="javascript:doExportReimEntry();"/>
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
          <div>
            当前查询项的报销总金额为：<asp:Literal ID="literal_CurrentReimValue" runat="server"></asp:Literal>
          </div>
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
          <div class="ui-tabs-panel ui-tabs-hide" id="divFragment6" runat="server">
              <div>
                项目名称：<asp:TextBox ID="tb_projectName_Rule" Width="200" runat="server"></asp:TextBox>
                &nbsp;&nbsp;<asp:Button ID="btn_QueryProjectRule" OnClick="QueryProjectRule_Click" Text="查找" Width="60px" runat="server"/>
              </div>
              <div>
                <asp:DataList ID="ddlProjectRuleList" Width="100%" runat="server" DataKeyField="ruleId" OnItemCommand="ProjectRuleCommand_Click">
                    <HeaderTemplate>
                        <table class="datagrid2">
                            <tr>
                                <th width="5%">编号</th>
                                <th width="30%">项目名称</th>
                                <th width="20%">报销项</th>
                                <th width="15%">单次最大报销金额</th>
                                <th width="15%">最大报销金额</th>
                                <th width="15%">操作</th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="datagrid2">
                            <tr>
                                <td width="5%"><%#Container.ItemIndex + 1 %></td>
                                <td width="30%"><%#Eval("project.name") %></td>
                                <td width="20%"><%#Eval("reim.name") %></td>
                                <td width="15%"><%#Eval("reimValue") %></td>
                                <td width="15%"><%#Eval("maxReimValue") %></td>
                                <td width="15%">
                                    <asp:LinkButton CommandName="deleteRule" runat="server">删除</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
                <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="ProjectRulePager" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="ProjectRulePager_PageChanged"
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
<script type="text/javascript">
    function doExportProject() {
        var href = "/Teacher/OutPutProjectExcel.ashx?pname=" + $("#ctl00_ContentPlaceHolder1_tb_ProjectName_ProjectFrag").val()
            + "&uname=" + $("#ctl00_ContentPlaceHolder1_tb_UserName_ProjectFrag").val()
            + "&rc=" + $("#ctl00_ContentPlaceHolder1_SearchProject_RootCategory").val()
            + "&sc=" + $("#ctl00_ContentPlaceHolder1_SearchProject_SubCategory").val()
            + "&tc=" + $("#ctl00_ContentPlaceHolder1_SearchProject_ThirdCategory").val();
        window.open(href);
    }

    function doExportReimEntry() {
        var href = "/Teacher/OutPutProjectReimExcel.ashx?pname=" + $("#ctl00_ContentPlaceHolder1_tb_projectName").val();

        window.open(href);
     }
</script>
</asp:Content>
