<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnglishExamManage.aspx.cs" 
    MasterPageFile="~/MasterPage/FrameManage.master" Inherits="USTA.WebApplication.Teacher.EnglishExamManage" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />

    

    <!--Validate-->

    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>

    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>四六级报名信息管理</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>批量更新报名相关状态信息</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>导出四六级报名信息Excel表</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <p>
                班级：<asp:DropDownList ID="ddlSerachSchoolClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSerachSchoolClass_SelectedIndexChanged">
                <asp:ListItem Text="担任班主任的所有班级" Value="all"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp; 关键字(姓名或学号)：<asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                <asp:Button ID="btnCommit" runat="server" Text="搜索" OnClick="btnCommit_Click" />
            </p>
            <br />
            <asp:DataList ID="dlstEnglishExamSignUpInfo" runat="server" DataKeyField="englishExamId" Width="100%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="10%">
                                所属考试
                            </th>
                            <th width="8%">
                                学号
                            </th>
                            <th width="6%">
                                姓名
                            </th>
                            <th width="6%">
                                考试类型
                            </th>
                            <th width="6%">
                                考试地点
                            </th>
                            <th width="8%">
                                报名状态
                            </th>
                            <th width="8%">
                                缴费状态
                            </th>
                            <th width="8%">
                                准考证状态
                            </th>
                            <th width="8%">
                                成绩单状态
                            </th>
                            <th width="6%">
                                分数
                            </th>
                            <th width="8%">
                                报名时间
                            </th>
                            <th width="8%">
                                截止时间
                            </th>
                            <th width="10%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="10%">
                                <a href="/Common/ViewEnglishExamNotify.aspx?englishExamNotifyInfoId=<%#Eval("englishExamNotifyId")%>&keepThis=true&TB_iframe=true&height=450&width=900"
                                    title="<%#Eval("englishExamNotifyTitle")%>" class="thickbox"><%#Eval("englishExamNotifyTitle")%>(点击查看)</a>
                            </td>
                            <td width="8%">
                               <%#Eval("studentNo").ToString().Trim()%>
                            </td>
                            <td width="6%">
                               <%#Eval("studentName").ToString().Trim()%>
                            </td>
                            <td width="6%"><%#Eval("examType").ToString().Trim()%>
                            </td>
                            <td width="6%"><%#Eval("examPlace").ToString().Trim()%>
                            </td>
                            <td width="8%"><%#(Eval("englishExamSignUpConfirm").ToString().Trim() == "1" ? "已确认" : "未确认")%>
                            </td>
                            <td width="8%"><%#(Eval("isPaid").ToString().Trim()=="1"?"已缴费":"未缴费")%><%#(Eval("isPaidRemark").ToString().Trim().Length == 0 ? string.Empty : "(" + Eval("isPaidRemark").ToString().Trim() + ")")%>
                            </td>
                            <td width="8%"><%#Eval("examCertificateState").ToString().Trim()%><%#(Eval("examCertificateRemark").ToString().Trim().Length == 0 ? string.Empty : "(" + Eval("examCertificateRemark").ToString().Trim() + ")")%>
                            </td>
                            <td width="8%"><%#Eval("gradeCertificateState").ToString().Trim()%>
                            </td>
                            <td width="6%"><%#Eval("grade").ToString().Trim()%>
                            </td>
                            <td width="8%"><%#Eval("updateTime").ToString().Trim()%>
                            </td>
                            <td width="8%"><%#Eval("deadLineTime").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                    >><a href="EditEnglishExamSignUpInfo.aspx?englishExamId=<%#(Eval("englishExamId").ToString().Trim())%>&englishExamNotifyId=<%#(Eval("englishExamNotifyId").ToString().Trim())%>&studentNo=<%#Eval("studentNo").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=420&width=800"
                                        title="查看、确认或修改报名信息" class="thickbox">查看、确认或修改报名信息</a><br />
                                    >><a href="EditEnglishExamSignUpInfoState.aspx?englishExamId=<%#(Eval("englishExamId").ToString().Trim())%>&englishExamNotifyId=<%#(Eval("englishExamNotifyId").ToString().Trim())%>&studentNo=<%#Eval("studentNo").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=470&width=800"
                                        title="修改报名相关状态信息" class="thickbox">修改报名相关状态信息</a></td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstEnglishExamSignUpInfo.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager2" runat="server" UrlPaging="true" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager2_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server"><table class="datagrid2">
                        <tr>
                            <th colspan="2" width="10%">
                                批量操作
                            </th>
                        </tr>
                        <tr>
                            <td width="20%">
                              请选择所属考试通知：
                            </td>
                            <td> <asp:DropDownList ID="ddlEnglishExamNotify" runat="server">
                        </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                              请选择班级：
                            </td>
                            <td><asp:DropDownList ID="ddlSchoolClass" runat="server">
                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                              请选择缴费状态：
                            </td>
                            <td><asp:DropDownList ID="ddlIspaid" runat="server">
                            <asp:ListItem Text="已缴费" Value="1"></asp:ListItem>
                            <asp:ListItem Text="未缴费" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="Button3" runat="server" Text="更改当前班级所有报名学生缴费状态" OnClick="btnCommitIspaid_Click" OnClientClick="return confirm('确认更改当前班级所有报名学生缴费状态吗？');" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                              请选择准考证状态：
                            </td>
                            <td><asp:DropDownList ID="ddlExamCertificate" runat="server">
                            <asp:ListItem Text="暂无" Value="暂无"></asp:ListItem>
                            <asp:ListItem Text="待领" Value="待领"></asp:ListItem>
                            <asp:ListItem Text="代领" Value="代领"></asp:ListItem>
                            <asp:ListItem Text="已领" Value="已领"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="Button2" runat="server" Text="更改当前班级所有报名学生准考证状态" OnClick="btnCommitExamCertificate_Click" OnClientClick="return confirm('确认更改当前班级所有报名学生准考证状态吗？');" />
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                              请选择成绩单状态：
                            </td>
                            <td><asp:DropDownList ID="ddlGradeCertificate" runat="server">
                            <asp:ListItem Text="暂无" Value="暂无"></asp:ListItem>
                            <asp:ListItem Text="待领" Value="待领"></asp:ListItem>
                            <asp:ListItem Text="代领" Value="代领"></asp:ListItem>
                            <asp:ListItem Text="已领" Value="已领"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="Button1" runat="server" Text="更改当前班级所有报名学生成绩单状态" OnClick="btnCommitGradeCertificate_Click" OnClientClick="return confirm('确认更改当前班级所有报名学生成绩单状态吗？');" />
                            </td>
                        </tr>
                    </table>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server"><table class="datagrid2">
                        <tr>
                            <th colspan="2" width="10%">
                                导出Excel
                            </th>
                        </tr>
                        <tr>
                            <td width="10%">
                              请选择所属考试通知：
                            </td>
                            <td width="10%"> <asp:DropDownList ID="ddlEnglishExamNotifyExcel" runat="server" ClientIDMode="Static">
                        </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="10%">
                              请选择要导出的班级：
                            </td>
                            <td width="10%"><asp:DropDownList ID="ddlSchoolClassExcel" runat="server" ClientIDMode="Static">
                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="10%">
                            </td>
                            <td width="10%">
                <a href="#" title="导出所选班级四六级报名信息Excel表" onclick="this.href='OutputEnglishExamSignUpExcel.ashx?englishExamNotifyId='+$('#ddlEnglishExamNotifyExcel').val()+'&schoolClassId='+$('#ddlSchoolClassExcel').val();" target="_blank">导出所选班级四六级报名信息Excel表</a>
                            </td>
                        </tr>
                    </table>
        </div>
    </form>
</asp:Content>
