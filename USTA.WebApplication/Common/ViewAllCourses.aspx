<%@ Page Language="C#" AutoEventWireup="true" Inherits="Common_ViewAllCourses" MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="ViewAllCourses.aspx.cs" %>

<%@ Register assembly="AspNetPager" namespace="Wuqi.Webdiyer" tagprefix="webdiyer" %>
        
        <asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="form1" runat="server">
    <div>
     <table>
      <tr>
        <td><%=tag%> 课程</td> 
      </tr>
      <tr>
       <td colspan ="2" style="height: 35px">                 
                    <asp:DataList ID="dlTermCourse" runat="server" DataKeyField="courseNo" onitemcommand="dlTermCourse_ItemCommand" 
                       >
                    <ItemTemplate>
                    
                    <table>                      
                     <tr>
                                           
                     <td><asp:LinkButton ID="LinkButton2" runat="server" CommandName="view"> <%#Eval("courseName")%></asp:LinkButton></td>
                                                 
                     </tr>
                    </table>
                    </ItemTemplate><FooterTemplate><%=(this.dlTermCourse.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate><FooterStyle CssClass="datalistNoLine" />
                    </asp:DataList>
      </tr>
           
     </table>
    </div>
    </form>
</asp:Content>
