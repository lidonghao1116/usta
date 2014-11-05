<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="EnglishExamSignUp.aspx.cs" Inherits="USTA.WebApplication.Student.EnglishExamSignUp" %>


<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>我要报名</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>查看报名信息</span></a></li>
            <li id="liFragment3" runat="server"><a href="../Common/FeedBack.aspx"><span>疑问反馈</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <table align="center" width="100%" class="tableAddStyleNone" runat="server" id="tbEnglishtSignUp"><tr>
                    <td width="200px" class="border">                        
                        当前考试类型：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddlEnglishExamNotify" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="border">
                        以下为您的报考基本信息，请确认（标记为<span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>的信息不可修改，若有问题，请和学工部老师联系修改）：
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">
                        姓名：
                    </td>
                    <td class="border">
                        <asp:Literal ID="ltlName" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr>
                    <td width="200px" class="border">                        
                        性别：
                    </td>
                    <td class="border">
                        <asp:Literal ID="ltlSex" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        学号：
                    </td>
                    <td class="border"><asp:Literal ID="ltlStudentNo" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        证件类型：
                    </td>
                    <td class="border"><asp:Literal ID="ltlCardType" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        证件号码：
                    </td>
                    <td class="border"><asp:Literal ID="ltlCardNum" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        入学年份：
                    </td>
                    <td class="border"><asp:Literal ID="ltlMatriculationDate" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        专业：
                    </td>
                    <td class="border"><asp:Literal ID="ltlMajor" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        班级：
                    </td>
                    <td class="border"><asp:Literal ID="ltlSchoolClass" runat="server"></asp:Literal><span style="color:Red; font-size:13px; font-weight:bolder;"> * </span>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        考试类型：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddlEnglishExamType" runat="server">
                        <asp:ListItem Text="四级" Value="四级"></asp:ListItem>
                        <asp:ListItem Text="六级" Value="六级"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr><td width="200px" class="border">                        
                        考试地点：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddlEnglishExamPlace" runat="server">
                        <asp:ListItem Text="苏州" Value="苏州"></asp:ListItem>
                        <asp:ListItem Text="合肥" Value="合肥"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr><td width="200px" class="border">    
                    </td>
                    <td class="border">
                        <asp:Button ID="btnConfirm" runat="server" Text="已经确认报考信息并报名" OnClick="btnConfirm_Click" OnClientClick="if(!confirm('是否确认报考信息（包括基本信息、考试类型、考试地点等）并报名？')){return false;}" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <div align="center" class="feedstable">
                <asp:DataList runat="server" ID="dlstEnglishExamSignUp" Width="100%">
                    <HeaderTemplate>
                        <table class="datagrid2" width="100%" style="margin-top: 30px;">
                            <tr>
                                <th width="25%">
                                    所属考试
                                </th>
                                <th width="15%">
                                    报名时间
                                </th>
                                <th width="10%">
                                    报名状态
                                </th>
                                <th width="25%">
                                    缴费状态
                                </th>
                                <th width="25%">
                                    操作
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="datagrid2" width="100%">
                            <tr>
                                <td width="25%">
                                     <a href="/Common/ViewEnglishExamNotify.aspx?englishExamNotifyInfoId=<%#Eval("englishExamNotifyId")%>&keepThis=true&TB_iframe=true&height=500&width=900"
                                    title="<%#Eval("englishExamNotifyTitle")%>" class="thickbox"><%#Eval("englishExamNotifyTitle")%>(点击查看)</a>
                                </td>
                                <td width="15%">
                                    <%#(Eval("updateTime").ToString().Trim())%>
                                </td>
                                <td width="10%">
                                    <%#(Eval("englishExamSignUpConfirm").ToString().Trim() == "1" ? "已确认" : "未确认")%>
                                </td>
                                <td width="25%">
                                    <%#(Eval("isPaid").ToString().Trim()=="1"?"已缴费":"未缴费（说明：此状态并不说明 您未缴费，由于是人工审核缴费，因此有一定的延时）")%>
                                </td>
                                <td width="25%">
                                    <a href="EditEnglishExamSignUpInfo.aspx?englishExamId=<%#(Eval("englishExamId").ToString().Trim())%>&englishExamNotifyId=<%#(Eval("englishExamNotifyId").ToString().Trim())%>&keepThis=true&TB_iframe=true&height=430&width=800"
                                        title="查看并修改报名信息" class="thickbox">查看并修改报名信息</a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.dlstEnglishExamSignUp.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" />
                </asp:DataList>
            </div>
            <p>
                &nbsp;</p>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
        </div>
    </div>
    </form>
</asp:Content>
