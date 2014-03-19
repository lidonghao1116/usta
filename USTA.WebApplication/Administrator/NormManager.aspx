<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="NormManager.aspx.cs" Inherits="USTA.WebApplication.Administrator.NormManager" %>
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
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>指标管理</span></a></li>
            <li id="liFragment3" runat="server"  visible="false"><a href="#"><span>教师工作量详情</span></a></li>
            <li id="liFragment4" runat="server"><a href="?fragment=4"><span>导出教师工作量excel</span></a></li>
            <li id="liFragment5" runat="server"  visible="false"><a href="?fragment=5"><span>工作量模板</span></a></li>
        </ul>
        
          <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
          <asp:Label ID="lblKeyword" runat="server" Text="关键字(用户ID或者名字)：" Width="150px"></asp:Label>
            <asp:TextBox ID="txtKeyword" ClientIDMode="static" runat="server"></asp:TextBox>
            &nbsp;
            教师类型： <asp:DropDownList ID="ddltTeacherType" runat="server" OnSelectedIndexChanged="ddltTeacherType_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="" Text="所有"></asp:ListItem>
            <asp:ListItem Value="国内" Text="国内"></asp:ListItem>
            <asp:ListItem Value="校内" Text="校内"></asp:ListItem>
            <asp:ListItem Value="本院" Text="本院"></asp:ListItem>
            <asp:ListItem Value="其他" Text="其他"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnQuery" runat="server" Text="模糊查询"
                OnClick="btnQuery_Click" />

                <a href="ViewNormAuth.aspx?keepThis=true&>&TB_iframe=true&height=300&width=500" title="查看有查看权限的用户" class="thickbox">查看有查看权限的用户</a>
            <br />
            <br />
            <asp:DataList ID="dlSearchTeacher" runat="server" DataKeyField="teacherNo"
              OnItemDataBound="dlSearchTeacher_ItemDataBound" OnItemCommand="dlSearchTeacher_ItemCommand" Width="100%">
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
                                
                               
                             
                                <a href="NormManager.aspx?fragment=3&teacherNo=<%#Eval("teacherNo") %>">
                                    进入工作量管理</a>&nbsp;&nbsp;&nbsp;&nbsp;
                               
                                <asp:LinkButton ID="LBtnRemoveAuth" runat="server" CommandName="removeAuth" OnClientClick="return deleteTip('')">删除查看权限</asp:LinkButton>

                                <asp:LinkButton ID="LBtnAddAuth" runat="server" CommandName="addAuth"   OnClientClick="return addTip('');" >添加查看权限</asp:LinkButton>

                                
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
          <script type="text/javascript">
              function addTip(s) {
                  return confirm("确定添加"+s+"?");
              }
              function deleteTip(s) {
                  return confirm("确定删除"+s+"?");
              }
          </script>
          <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
         
          
            请选择学年：<asp:DropDownList ID="ddltTermYear" runat="server" AutoPostBack="true"  onselectedindexchanged="ddltTermYear_SelectedIndexChanged"></asp:DropDownList><br /><br />
          
            <table class="datagrid2">
                <tr><th width="15%">指标 &nbsp;<a href="/Administrator/AddNorm.aspx?year=<%=this.term%>&keepThis=true&TB_iframe=true&height=200&width=500" class="thickbox"  title="添加指标">[添加一级指标]</a></th><th width="35%">子指标</th><th width="25%">计算规则</th><th width="25%">操作</th></tr>
                <tr><td  width="15%">硕士教学</td><td width="35%">
                <table class="normManage"   Width="100%"><tr><td>理论课时</td></tr>
                <tr><td>实验课时</td></tr></table>
                <asp:DataList ID="shuoshiddlt" runat="server" CellPadding="0" Width="100%" Style="margin-bottom: 10px;" RepeatDirection="Vertical" CssClass="normManage">
                <ItemTemplate>
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        	<tr>
                        		<td width="40%" style="border-bottom:0px;"><span title='<%#Eval("comment").ToString().Trim()%>'> <%#Eval("name").ToString().Trim()%></span></td><td style="border-bottom:0px;"><a href='/Administrator/EditNorm.aspx?year=<%=this.term%>&normId=<%#Eval("normId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=200&width=400' class="thickbox"  title="编辑指标">编辑</a>
                                        <a href='/Administrator/NormManager.aspx?deleteNormId=<%#Eval("normId").ToString().Trim()%>&fragment=2&term=<%=this.term%>' onclick="return confirm('确认删除？');">删除</a> </td>
                        	</tr>
                            </table>
                </ItemTemplate>
                </asp:DataList>
                </td>
                <td  width="25%"><%=this.getFormulaShow(-1)%></td>
                <td width="25%">
                                
                       <a href="/Administrator/AddNorm.aspx?year=<%=this.term%>&parentId=-1&keepThis=true&TB_iframe=true&height=200&width=500" class="thickbox"  title="添加指标">[添加子指标]</a>
                               <a href="/Administrator/EditNormFormula.aspx?normId=-1&termYear=<%=this.ddltTermYear.SelectedValue%>&keepThis=true&TB_iframe=true&height=200&width=500" class="thickbox"  title="修改规则">[更改规则]</a> 
                            
                </td>
                </tr>
              </table>

            <asp:DataList ID="dlstFirstNorm" runat="server" align="center" Width="100%" CellPadding="0"
                OnItemDataBound="dlstFirstNorm_ItemDataBound" RepeatDirection="Vertical" >
               <%-- <HeaderTemplate>
                  <table class="datagrid2">
                <tr><th width="15%">指标 &nbsp;<a href="/Administrator/AddNorm.aspx?keepThis=true&TB_iframe=true&height=200&width=500" class="thickbox"  title="添加指标">[添加一级指标]</a></th><th width="35%">子指标</th><th width="25%">计算规则</th><th width="25%">操作</th></tr>
                 </table>
                </HeaderTemplate>--%>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="15%" >
                               <span title=" <%#Eval("comment").ToString().Trim()%>"> <%#Eval("name").ToString().Trim()%></span>  
                               &nbsp; &nbsp;
                               </td>
                               <td width="35%">

                                <asp:DataList ID="dlstSecondNorm" runat="server"  CellPadding="0" Width="100%" Style="margin-bottom: 10px;" RepeatDirection="Vertical" CssClass="normManage">
                        <ItemTemplate>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        	<tr>
                        		<td width="40%" style="border-bottom:0px;"><span title='<%#Eval("comment").ToString().Trim()%>'> <%#Eval("name").ToString().Trim()%></span></td><td style="border-bottom:0px;"><a href='/Administrator/EditNorm.aspx?year=<%=this.term%>&normId=<%#Eval("normId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=200&width=400' class="thickbox"  title="编辑指标">编辑</a>
                                        <a href='/Administrator/NormManager.aspx?deleteNormId=<%#Eval("normId").ToString().Trim()%>&fragment=2&term=<%=this.term%>' onclick="return confirm('确认删除？');">删除</a> </td>
                        	</tr>
                        </table>
                                      
                                   
                               
                        </ItemTemplate>
                    </asp:DataList>

                               </td>
                               <td width="25%" ><%#this.getFormulaShow(Convert.ToInt32(Eval("normId").ToString().Trim()))%>
                                 
                            </td>
                            
                                <td width="25%" >

                                <a href="/Administrator/EditNorm.aspx?year=<%=this.term%>&normId=<%#Eval("normId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=200&width=500" class="thickbox"  title="编辑指标">[编辑]</a>
                                 <a href="/Administrator/NormManager.aspx?deleteNormId=<%#Eval("normId").ToString().Trim()%>&fragment=2&term=<%=this.term%>" onclick="return confirm('确认删除？');">[删除]</a> 
                       <a href="/Administrator/AddNorm.aspx?year=<%=this.term%>&parentId=<%#Eval("normId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=200&width=500" class="thickbox"  title="添加指标">[添加子指标]</a>
                               <a href="/Administrator/EditNormFormula.aspx?normId=<%#Eval("normId").ToString().Trim()%>&termYear=<%=this.ddltTermYear.SelectedValue%>&keepThis=true&TB_iframe=true&height=200&width=500" class="thickbox"  title="修改规则">[更改规则]</a> 
                            </td>
                        </tr>
                  </table>
                   
                   
                    
                </ItemTemplate>
                <ItemStyle VerticalAlign="Top" />
                <FooterTemplate>
                   </FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList><br />
            <table width="100%" class="datagrid2">
            <tr><th>总规则&nbsp;&nbsp; <a href='/Administrator/EditNormFormula.aspx?normId=0&termYear=<%=this.ddltTermYear.SelectedValue%>&keepThis=true&TB_iframe=true&height=200&width=500' class="thickbox"  title="修改规则"">[更改]</a></th></tr>
            <tr><td>&nbsp;<%=this.getFormulaShow(0) %></td></tr>
          </table>
        </div>
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
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
                <tr><th width="15%">指标</th><th width="15%">子指标</th><th width="15%">子指标值</th><th width="15%">指标值</th><th width="20%">规则</th><th width="20%">操作</th></tr>
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
                            <td width="20%"><%#this.getFormulaShow( Convert.ToInt32( Eval("normId")) )%></td><td width="20%"> <a href="/Administrator/EditNormValue.aspx?normId=<%#Eval("normId")%>&term=<%=term %>&teacherNo=<%=teacherNo %>&keepThis=true&TB_iframe=true&height=400&width=500" class="thickbox"  title="修改值">[编辑值]</a>
                                    </td>
                        </tr>
                    </table>
                     
                </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                
                <FooterTemplate>
                    <%=(this.dlstDetailFirstNorm.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <table class="datagrid2"><tr><th>汇总</th></tr><tr><td>合计工作量：<%=this.GetNormValue(0)%> &nbsp;&nbsp;&nbsp;&nbsp;总规则：&nbsp;<%=this.getFormulaShow(0) %></td></tr></table>
            <div>
            <table> <tr><td>是否已经确认：</td><td><asp:Label ID="confirmStatus" runat="server"></asp:Label>&nbsp;&nbsp;<asp:Label ID="labelComment" runat="server"></asp:Label></td></tr></table>
            
            <asp:DataList ID="dlstConfirm" runat="server" Width=60%>
            
            <HeaderTemplate>
            <%if (this.dlstConfirm.Items.Count > 0)
              { %>
            <table class="datagrid2">
                <tr><th width="25%">时间</th><th width="25%">问题</th><th width="25%">回答</th><th width="25%">操作</th></tr>
                 </table>
            <%} %>
            </HeaderTemplate>
          
            <ItemTemplate>
            <table class="datagrid2"><tr><td width="25%"><%#Eval("createTime") %></td><td width="25%"><%#Eval("question") %></td><td width="25%"><%#Eval("answer") %></td>
            <td><%#(Eval("type").ToString().Trim()) != "0" ? "<a href=\"/Administrator/AnswerNormConfirm.aspx?id=" + Eval("id") + "&keepThis=true&TB_iframe=true&height=250&width=450\" class=\"thickbox\"  title=\"回答\">[回答]</a>" : ""%></td></tr></table>
            </ItemTemplate>
            </asp:DataList>
            </div>
            
            </div>
         <div class="ui-tabs-panel ui-tabs-hide" id="divFragment4" runat="server">
               <table class="tableAddStyle" width="100%">
                <tr><td width="20%">请选择学年：</td><td><asp:DropDownList ID="dlstTerms" runat="server"></asp:DropDownList></td></tr>
                <tr><td>教师名称（不填默认导出全部）：</td><td><asp:TextBox ID="txtTeacherName" runat="server"></asp:TextBox></td></tr>
                <tr><td>教师类型：</td><td><asp:DropDownList ID="ddltTeacherTypeExcel" runat="server" >
            <asp:ListItem Value="" Text="请选择"></asp:ListItem>
            <asp:ListItem Value="国内" Text="国内"></asp:ListItem>
            <asp:ListItem Value="校内" Text="校内"></asp:ListItem>
            <asp:ListItem Value="本院" Text="本院"></asp:ListItem>
            <asp:ListItem Value="其他" Text="其他"></asp:ListItem>
            </asp:DropDownList></td></tr>
                <tr><td colspan="2"><a href="javascript:doExcel();">导出excel</a></td></tr>
               </table>
         </div>
         <div class="ui-tabs-panel ui-tabs-hide" id="divFragment5" runat="server">
         
              <table>
            <tr><td width="20%">请选择学年：</td><td><asp:DropDownList ID="templateTermyear" runat="server" AutoPostBack="True" 
                onselectedindexchanged="templateTermyear_SelectedIndexChanged">
            </asp:DropDownList></td></tr>
           
            </table>
             <asp:DataList ID="dlstTempF" runat="server" align="center" Width="100%" CellPadding="0"
                OnItemDataBound="dlstTempF_ItemDataBound" RepeatDirection="Vertical" >
                <HeaderTemplate>
                  <table class="datagrid2">
                <tr><th width="15%">指标</th><th width="15%">子指标</th><th width="15%">子指标值</th><th width="15%">指标值</th><th width="20%">规则</th><th width="20%">操作</th></tr>
                 </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table  class="datagrid2">
                        <tr>
                            <td width="15%" >
                                <span title=" <%#Eval("comment").ToString().Trim()%>"> <%#Eval("name").ToString().Trim()%></span>&nbsp;
                               
                            </td>
                            <td  width="30%" colspan="2">
                             <asp:DataList ID="dlstTempS" runat="server"  CellPadding="0"  Width="100%" Style="margin-bottom: 10px;" RepeatDirection="Vertical" CssClass="normManage">
                        <ItemTemplate>
                   
                                        <td width="50%"><span title=" <%#Eval("comment").ToString().Trim()%>"> <%#Eval("name").ToString().Trim()%></span></td>
                                         <td width="50%"><%#this.GetNormValue(Convert.ToInt32(Eval("normId").ToString().Trim()))%></td>
                                         
                                    
                        </ItemTemplate>
                    </asp:DataList>
                            </td>
                            <td width="15%">
                                <%#this.GetNormValue(Convert.ToInt32(Eval("normId").ToString().Trim()))%>
                            </td>
                            <td width="20%"><%#this.getFormulaShow(Convert.ToInt32(Eval("normId").ToString().Trim()))%></td><td width="20%">
                             <a href="/Administrator/EditNormValue.aspx?normId=<%#Eval("normId").ToString().Trim()%>&term=<%=term %>&teacherNo=<%=teacherNo %>&keepThis=true&TB_iframe=true&height=200&width=400" class="thickbox"  title="修改值">[编辑值]</a>
                            </td>
                        </tr>
                    </table>
                     
                </ItemTemplate>
                <ItemStyle VerticalAlign="Top" />
                <FooterTemplate>
                    <%=(this.dlstTempF.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
            </asp:DataList><br />    
               <table class="datagrid2"><tr><th>汇总信息</th></tr><tr><td>合计工作量：<%=this.GetNormValue(0)%> &nbsp;&nbsp;&nbsp;&nbsp;总规则：&nbsp;<%=this.getFormulaShow(0) %></td></tr></table>
           <br />    
           <asp:Button ID="btnTemplate" runat="server" Text="应用到所有老师" OnClick="btnTemplate_Click" OnClientClick="return sure();" />

         </div>
         </div>
        </form>
        <script type="text/javascript">
            function sure() {
                return window.confirm("执行后该学年的所有老师的工作量将全被覆盖，确认修改？");
            }
            function doExcel() {
                location.href = "/Administrator/OutPutNormExcel.ashx?term=" + $("#ctl00_ContentPlaceHolder1_dlstTerms").val() + "&searchKey=" + $("#ctl00_ContentPlaceHolder1_txtTeacherName").val() + "&teacherType=" + $("#ctl00_ContentPlaceHolder1_ddltTeacherTypeExcel").val();
            }
        </script>
</asp:Content>
