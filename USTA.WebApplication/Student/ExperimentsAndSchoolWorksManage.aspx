<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" Inherits="Student_ExperimentsAndSchoolWorksManage" Codebehind="ExperimentsAndSchoolWorksManage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <form id="form1" runat="server">
 <div id="container-1" style="padding-top: 40px;">
            <ul class="ui-tabs-nav">
                <li id="liFragment1" runat="server"><a href="?fragment=1"><span>我的作业管理</span></a></li>
                <li id="liFragment2" runat="server"><a href="?fragment=2"><span>我的实验管理</span></a></li>
                <li id="liFragment3" class="ui-tabs-hide" runat="server"><a href="?fragment=3"><span>暂时不用</span></a></li>
            </ul>
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <asp:DropDownList ID="ddltcourseSchoolwork" runat="server" ClientIDMode="Static" onchange="location.href='/Student/ExperimentsAndSchoolWorksManage.aspx?fragment=1&courseNoTermTagClassID='+$('#ddltcourseSchoolwork').val();"></asp:DropDownList>
             <br /><br />
            <asp:DataList ID="dlstMySchoolworks" runat="server" Width="100%" >
        <HeaderTemplate>
                            <table class="datagrid2">
                                <tr>
                                    <th width="5%">
                                        序号
                                    </th>
                                    <th width="22%">
                                        标题
                                    </th>
                                    <th width="15%">
                                        提交内容
                                    </th>
                                    <th width="10%">
                                        是否批阅
                                    </th>
                                    <th width="8%">是否过期</th>
                                     <th width="5%">得分</th> 
                                     <th width="15%">
                                        修改后的作业
                                    </th>
                                     <th width="20%">评语</th>  
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table class="datagrid2">
                                <tr>
                                    <td width="5%"><%#Container.ItemIndex+1 %>
                                    </td>
                                    <td width="22%"><a href="CInfoSchoolwork.aspx?courseNo=<%#Eval("courseNo").ToString().Trim()%>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim())%>&termtag=<%#Eval("termTag")%>&schoolworkNotifyId=<%#Eval("schoolWorkNotifyId") %>">
                                        <%#Eval("schoolWorkNotifyTitle")%></a>
                                    </td>
                                    <td width="15%" style="padding:10px;"><%#(Eval("isOnline").ToString().Trim()=="True")?((Eval("isSubmit").ToString().Trim()=="True")?"":"未提交"):"书面提交" %>
                                       
                                       <%#GetURL(Eval("attachmentId").ToString())%>
                                 
                                    </td>
                                    <td width="10%">
                                        <%#isCheck(Eval("isCheck").ToString())%>
                                    </td>
                                    <td width="8%">
                                        <%#DeadLine(Convert.ToDateTime(Eval("deadline").ToString()))%>
                                    </td>
                                    <td width="5%">&nbsp;<%#Eval("score")%></td>
                                    <td width="15%" style="padding:10px;">&nbsp;<%#GetURL(Eval("returnAttachmentId").ToString())%></td>
                                    <td width="20%" style="padding:10px;">&nbsp;<%#Eval("remark")%></td>
                                 </tr>
                             </table>
                         </ItemTemplate><FooterTemplate><%=(this.dlstMySchoolworks.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" />
        
        </asp:DataList>
            </div>
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <asp:DropDownList ID="ddltExperimentCourse" runat="server" ClientIDMode="Static" onchange="location.href='/Student/ExperimentsAndSchoolWorksManage.aspx?fragment=2&courseNoTermTagClassID='+$('#ddltExperimentCourse').val();"></asp:DropDownList>
                      <br /><br />
            <asp:DataList ID="dlstMyExperiments" runat="server"  Width="100%" >
        <HeaderTemplate>
                            <table class="datagrid2">
                                <tr>
                                    <th width="5%">
                                        序号
                                    </th>
                                    <th width="22%">
                                        标题
                                    </th>
                                    <th width="15%">
                                        提交内容
                                    </th>
                                    <th width="10%">
                                        是否批阅
                                    </th>
                                    <th width="8%">是否过期</th>
                                        <th width="5%">得分</th> 
                                        <th width="15%">修改后的作业</th>
                                        <th width="20%">评语</th> 
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table class="datagrid2">
                                <tr>
                                    <td width="5%"><%#Container.ItemIndex+1 %>
                                    </td>
                                    <td width="22%"><a href="CInfoExperiment.aspx?courseNo=<%#Eval("courseNo").ToString().Trim() %>&classID=<%#Server.UrlEncode(Eval("classID").ToString().Trim())%>&termtag=<%#Eval("termTag")%>&experimentResourceId=<%#Eval("experimentResourceId")%>">
                                        <%#Eval("experimentResourceTitle")%></a>
                                    </td>
                                    <td width="15%" style="padding:10px;">
                                        <%#(Eval("isSubmit").ToString().Trim()=="True")?"":"未提交"%> <%#GetURL(Eval("attachmentId").ToString())%>
                                    </td>
                                    <td width="10%">
                                        <%#isCheck(Eval("isCheck").ToString())%>
                                    </td>
                                    <td width="8%">
                                        <%#DeadLine(Convert.ToDateTime(Eval("deadLine").ToString()))%>
                                    </td>
                                    <td width="5%">&nbsp;<%#Eval("score") %></td> 
                                     <td width="15%" style="padding:10px;">&nbsp;<%#GetURL(Eval("returnAttachmentId").ToString())%></td> 
                                    <td width="20%" style="padding:10px;">&nbsp;<%#Eval("remark")%></td> 
                                 </tr>
                             </table>
                         </ItemTemplate><FooterTemplate><%=(this.dlstMyExperiments.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" />
       </asp:DataList>
             
            </div>
            
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            
            </div>
    </div>
    </form>
</asp:Content>

