<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="NormManager.aspx.cs" Inherits="USTA.WebApplication.Teacher.NormManager" %>
 <%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
 <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />

<script type="text/javascript">
    function deleteTip() {
        return confirm("确定删除？");
    }
</script>
<style type="text/css">
.td-class
{
    text-align:center;
    border-bottom: 1px solid #056FAE;
    border-left: 1px solid #056FAE;
    
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server">
   <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>教师</span></a></li>
            <li id="liFragment2" runat="server"  visible="false"><a href="?fragment=2"><span>教师工作量详情</span></a></li>
        
        </ul>
        
           <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
          <asp:Label ID="lblKeyword" runat="server" Text="关键字(用户ID或者名字)：" Width="150px"></asp:Label>
            <asp:TextBox ID="txtKeyword" ClientIDMode="static" runat="server"></asp:TextBox>
            &nbsp;
            教师类型： <asp:DropDownList ID="ddltTeacherType" runat="server" OnSelectedIndexChanged="ddltTeacherType_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="" Text="请选择"></asp:ListItem>
            <asp:ListItem Value="国内" Text="国内"></asp:ListItem>
            <asp:ListItem Value="校内" Text="校内"></asp:ListItem>
            <asp:ListItem Value="本院" Text="本院"></asp:ListItem>
            <asp:ListItem Value="其他" Text="其他"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnQuery" runat="server" Text="模糊查询"
                OnClick="btnQuery_Click" />

            <asp:DataList ID="dlSearchTeacher" runat="server" DataKeyField="teacherNo"
               Width="100%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="100px">
                                教师姓名
                            </th>
                            <th width="200px">
                                邮件地址
                            </th>
                            <th width="100px">
                                办公室地址
                            </th>
                            <th width="100px">
                                是否为助教
                            </th>
                            <th width="100px">
                                教师类型
                            </th>
                            <th>
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="100px">
                                <%#Eval("teacherName").ToString().Trim()%>
                            </td>
                            <td width="200px">
                                <%#Eval("emailAddress").ToString().Trim()%>
                            </td>
                            <td width="100px">
                                <%#Eval("officeAddress").ToString().Trim()%>
                            </td>
                            <td width="100px">
                                <%#(Eval("IsAssistant").ToString().Trim() == "1" ? "是" :"否")%>
                            </td>
                            <td width="100px">
                                <%#Eval("TeacherType").ToString().Trim()%>
                            </td>
                            <td>
                    
                             
                                <a href="NormManager.aspx?fragment=2&teacherNo=<%#Eval("teacherNo") %>">
                                    进入工作量管理</a>&nbsp;&nbsp;&nbsp;&nbsp;
                               

                                
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlSearchTeacher.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" UrlPaging="true" ID="AspNetPager2" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager2_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
          </div>
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
             <table><tr><td><b>教师：</b></td><td><asp:Label ID ="labelTeacherName" runat="server" />
            <asp:HiddenField ID="hiddenteacherNo" runat="server" />
            </td><td>&nbsp;<b>学年：</b></td><td><asp:DropDownList ID="ddltNormTerm" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddltNormTerm_SelectedIndexChanged">
            </asp:DropDownList></td></tr>
           
            </table>
              <div><h3>硕士教学： </h3> 
              工作量：
              <%=this.GetNormValue(-1)%> &nbsp;
              规则：<%=this.getFormulaShow(-1)%></div>
            <asp:Label ID="shuoshijx" runat="server"></asp:Label>

            <br />
            <h3>其他工作量： </h3> 
             <asp:DataList ID="dlstDetailFirstNorm" runat="server" align="center" Width="100%" CellPadding="0"
                OnItemDataBound="dlstDetailFirstNorm_ItemDataBound" RepeatDirection="Vertical" >
                <HeaderTemplate>
                  <table class="datagrid2">
                <tr><th width="15%">指标</th><th width="15%">子指标</th><th width="15%">子指标值</th><th width="15%">指标值</th><th width="20%">规则</th></tr>
                 </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table  class="datagrid2">
                        <tr>
                            <td width="15%" >
                                <span title=" <%#Eval("comment")%>"> <%#Eval("name")%></span>&nbsp;
                            </td>
                            <td width="30%" colspan="2">
                             <asp:DataList ID="dlstDetailSecondNorm" runat="server"  CellPadding="0"  Width="100%" Style="margin-bottom: 10px;" RepeatDirection="Vertical" CssClass="normManage">
                                <ItemTemplate>
                                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                	<tr>
                                		<td width="50%" style="border-bottom:0px;"><span title=" <%#Eval("comment")%>"> <%#Eval("name")%></span></td>
                                		<td style="border-bottom:0px;"><%#this.GetNormValue(Convert.ToInt32(Eval("normId")))%></td>
                                	</tr>
                                </table>
                                </ItemTemplate>
                            </asp:DataList>
                            </td><td width="15%">
                                <%#this.GetNormValue(Convert.ToInt32(Eval("normId")))%>
                            </td>
                            <td width="20%"><%#this.getFormulaShow( Convert.ToInt32( Eval("normId")) )%></td>
                        </tr>
                    </table>
                     
                </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                
                <FooterTemplate>
                    <%=(this.dlstDetailFirstNorm.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <table class="datagrid2"><tr><th>汇总</th></tr><tr><td>合计工作量：<%=this.GetNormValue(0)%> &nbsp;&nbsp;&nbsp;&nbsp;总规则：&nbsp;<%=this.getFormulaShow(0) %></td></tr></table>
            
         </div>
        </form>
</asp:Content>
