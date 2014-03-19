<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="NormView.aspx.cs" Inherits="USTA.WebApplication.Teacher.NormView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
 <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <form id="form1" runat="server">
 <div style="margin-left:15px;margin-right:15px;">
  <table><tr>
            <td>&nbsp;<b>学年：</b></td><td><asp:DropDownList ID="ddltNormTerm" runat="server" AutoPostBack="True" 
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
            


 <div style="margin-left:30px;margin-right:30px;margin-top:30px;">
<%-- <div style="margin-bottom:10px;">
学年：<asp:DropDownList ID="ddltNormTerm" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddltNormTerm_SelectedIndexChanged">
            </asp:DropDownList></div>
<asp:DataList ID="dlstDetailFirstNorm" runat="server" align="center" Width="100%" CellPadding="0"
                OnItemDataBound="dlstDetailFirstNorm_ItemDataBound" RepeatDirection="Vertical" >
                <HeaderTemplate>
                  <table class="datagrid2">
                <tr><th width="15%">指标</th><th width="15%">子指标</th><th width="15%">子指标值</th><th width="25%">指标值</th><th width="25%">规则</th><th width="25%"></th></tr>
                 </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table  class="datagrid2">
                        <tr>
                            <td width="15%" >
                                <span title=" <%#Eval("comment")%>"> <%#Eval("name")%></span>&nbsp;
                               
                            </td>
                            <td width="30%">
                             <asp:DataList ID="dlstDetailSecondNorm" runat="server"  CellPadding="0"  Width="80%" Style="margin-bottom: 10px;text-align:left" RepeatDirection="Vertical">
                        <ItemTemplate>
                   
                                       <span title=" <%#Eval("comment")%>"> <%#Eval("name")%></span>
                                         <%#this.GetNormValue(Convert.ToInt32(Eval("normId")))%>
                                         
                                    
                        </ItemTemplate>
                        </asp:DataList>
                        </td><td width="15%" >
                                <%#this.GetNormValue(Convert.ToInt32(Eval("normId")))%>
                            </td>
                            <td width="20%"><%#this.getFormulaShow( Convert.ToInt32( Eval("normId")) )%></td><td width="20%"></td>
                        </tr>
                    </table>
                    
                </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                <FooterTemplate>
                   
                <FooterStyle CssClass="datalistNoLine" />
                </FooterTemplate>
            </asp:DataList>--%>
           <%-- <table class="datagrid2"><tr><td>合计工作量：<%=this.GetNormValue(0)%> &nbsp;&nbsp;&nbsp;&nbsp;总规则：&nbsp;<%=this.getFormulaShow(0) %></td></tr></table>
           --%>
            <%if (this._IsConfirm)
              { %>
            <%}
              else
              { %>
              没问题，点击
               <asp:Button ID="btnConfirm" runat="server" Text="确认" 
        onclick="btnConfirm_Click" />
                     &nbsp;   &nbsp;   &nbsp;  有疑问，点击             
               <a href="/Teacher/AddNormConfirm.aspx?value=<%=this.GetNormValue(0)%>&term=<%=term %>&teacherNo=<%=teacherNo %>&keepThis=true&TB_iframe=true&height=250&width=450" class="thickbox"  title="提问">我有疑问</a>
            <%} %>
            </div>
            </div>
            </form>
             <div style="margin-left:30px;margin-right:30px;margin-top:30px;">
            <asp:DataList ID="dlstConfirm" runat="server" Width=60%>
            
            <HeaderTemplate>
            <%if (this.dlstConfirm.Items.Count > 0)
              { %>
            <table class="datagrid2">
                <tr><th width="25%">时间</th><th width="25%">问题</th><th width="25%">回答</th><th width="25%"></th></tr>
                 </table>
            <%} %>
            </HeaderTemplate>
          
            <ItemTemplate>
            <table class="datagrid2"><tr><td width="25%"><%#Eval("createTime") %></td><td width="25%"><%#Eval("question") %></td><td width="25%"><%#Eval("answer") %></td><td></td></tr></table>
            </ItemTemplate>
            </asp:DataList>
            </div>
            
</asp:Content>
