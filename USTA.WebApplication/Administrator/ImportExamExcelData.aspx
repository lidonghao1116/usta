<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_ImportExamExcelData" MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="ImportExamExcelData.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
            <ul class="ui-tabs-nav">
                <li id="liFragment1" runat="server"><a href="?fragment=1"><span>考试安排信息管理</span></a></li>
                <li id="liFragment2" runat="server"><a href="?fragment=2"><span>导入考试安排Excel数据</span></a></li>
                <li id="liFragment3" runat="server" style="display:none;"><a href="?fragment=3"><span>搜索考试安排信息</span></a></li>
            </ul>
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
          
               <asp:DataList ID="dlCourse" runat="server" DataKeyField="examArrangeListId" onitemcommand="dlCourse_ItemCommand" 
                      Width="100%"   >
               <HeaderTemplate>
               <table class="datagrid2"> 
                  <tr>
                          <th width="16%">课程名称</th>
                          <th width="16%">考试时间</th>
                          <th width="16%">考试地点</th>
                          <th width="16%">备注</th>
                          <th width="16%">任课教师</th>
                        
                          <th width="20%">操作</th> 
                         
                 </tr>
                 </table> 
               </HeaderTemplate>
                    <ItemTemplate>                  
                    <table class="datagrid2">                      
                     <tr>
                          <td width="16%"><%#Eval("courseName")%></td>
                          <td width="16%"><%#Eval("examArrangeTime")%></td>
                          <td width="16%"><%#Eval("examArrageAddress")%></td>
                          <td width="16%"><%#Eval("remark")%></td>
                          <td width="16%"><%#Eval("teacherName")%></td>
                         
                          <td width="20%"><asp:LinkButton ID="LinkButton1" runat="server" CommandName="delete">删除</asp:LinkButton></td>   
                                                
                     </tr>
                    </table>
                    </ItemTemplate><FooterTemplate><%=(this.dlCourse.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" />
                    </asp:DataList>
           </div>
           
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
             <asp:FileUpload ID="fileUploadUrl" runat="server" />
              <asp:Button ID="Button1" runat="server" Text="导入考试安排Excel数据" onclick="Button1_Click" />
            </div>
            
            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            
            </div>
      </div>
    </form>
</asp:Content>
