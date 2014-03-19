<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="SalaryManage.aspx.cs" Inherits="USTA.WebApplication.Teacher.SalaryManage" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();
        });

        function addTip() {
            return confirm("确定添加？");
        }

        function deleteTip() {
            return confirm("确定删除？");
        }

    </script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "100%", "200px");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server" visible="false"><a href="?fragment=1"><span>教师薪酬发放</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>薪酬项管理</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>薪酬预算查询与导出</span></a></li>
            <li id="liFragment4" runat="server" visible="false"><a href="?fragment=4"><span>添加酬金</span></a></li>
            <li id="liFragment5" runat="server"><a href="?fragment=5"><span>已发放薪酬查询与导出</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <asp:Label ID="lblKeyword" runat="server" Text="关键字(用户ID或者名字)：" Width="150px"></asp:Label>
            <asp:TextBox ID="txtKeyword" ClientIDMode="static" runat="server"></asp:TextBox>
            <asp:Label ID="lblTermTag" runat="server" Text="学期：" Width="40px"></asp:Label>
            <asp:DropDownList ID="searchTeacherTermTag" runat="server" AutoPostBack="true" OnSelectedIndexChanged="seacherTeacher_SelectChanged"></asp:DropDownList>
            <asp:Button ID="btnQuery" runat="server" Text="模糊查询"
                OnClick="SearchTeacher_Click" />
            <br />
            <br />
            <asp:DataList ID="dlSearchTeacher" runat="server" DataKeyField="teacherNo"
                Width="100%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="20%">
                                教师名称
                            </th>
                            <th width="80%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="20%">
                                <%#Eval("teacherName")%>
                            </td>
                            <td width="80%">
                                <a href="ViewTeacherInfo.aspx?keepThis=true&teacherNo=<%#Eval("teacherNo").ToString().Trim()%>&TB_iframe=true&height=300&width=500"
                                    title="查看教师详细信息" class="thickbox">查看详细信息</a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlSearchTeacher.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="TeacherListPager" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="TeacherSearch_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <div>
                <div>薪酬项列表&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                
                <div>
 <asp:DataList Width="100%" ID="SalaryItemList" runat="server" DataKeyField="salaryItemId" CellPadding="0">
                <HeaderTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <th width="10%">
                                薪酬项
                            </th>
                            <th width="10%">
                                计量单位
                            </th>
                            <th width="10%">
                                适用对象
                            </th>
                            <th width="5%">
                                状态
                            </th>
                            <th width="10%">
                                是否含税
                            </th>
                            <th width="10%">
                                是否默认选中
                            </th>
                            <th width="30%">
                                薪酬项描述
                            </th>
                            <th width="15%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <td width="10%">
                                <%#Eval("salaryItemName")%>
                            </td>
                            <td width="10%">
                                <%#Eval("salaryItemUnit") %>
                            </td>
                            <td width="10%">
                                <asp:Literal ID="SalaryItem_Literal" runat="server"></asp:Literal>
                            </td>
                            <td width="5%">
                                <%#Eval("salaryItemStatus").ToString().Trim() == "1" ? "已展示" : "已隐藏" %>
                            </td>
                            <td width="10%">
                                <%#bool.Parse(Eval("hasTax").ToString().Trim()) ? "含税" : "不含税" %>
                            </td>
                            <td width="10%">
                                <%#bool.Parse(Eval("isDefaultChecked").ToString().Trim()) ? "默认选中" : "默认不选中" %>
                            </td>
                            <td width="30%">
                                <%#Eval("salaryItemDesc")%>
                            </td>
                            <td width="15%">
                                </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.SalaryItemList.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
                    <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="SalaryItemPager" runat="server"
                    FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="SalaryItemPager_PageChanged"
                    PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                    HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                    SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                    CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                    TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            
            <div>
                <div>
                    教师：<asp:TextBox ID="TeacherSalary_TeacherName" runat="server" Width="100"></asp:TextBox>
                    类型：<asp:DropDownList ID="TeacherSalary_TeacherType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="TeacherSalary_TermTagChanged"></asp:DropDownList>
                    <asp:HiddenField ID="TeacherSalary_TeacherID" runat="server"/>
                    学期：<asp:DropDownList ID="TeacherSalary_TermTag" runat="server" AutoPostBack="true" OnSelectedIndexChanged="TeacherSalary_TermTagChanged">
                        </asp:DropDownList>
                        状态：<asp:DropDownList ID="TeacherSalary_Status" runat="server" AutoPostBack="true" OnSelectedIndexChanged="TeacherSalary_TermTagChanged">
                        <asp:ListItem Value="0">所有状态</asp:ListItem>
                        <asp:ListItem Value="1">未确认</asp:ListItem>
                        <asp:ListItem Value="2">已确认</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btn_TeacherSalary_Query" runat="server" Text="查找" OnClick="TeacherSalaryQuery_Click"/>
                    <button onclick="javascript:doExcel4TeacherSalary();">导出</button>
                </div>
                <div>
 <asp:DataList Width="100%" ID="TeacherSalaryList" runat="server" DataKeyField="teacherSalaryId" CellPadding="0">
                <HeaderTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <th width="10%">
                                教师
                            </th>
                            <th width="10%">
                                教师类型
                            </th>
                            <th width="10%">
                                薪酬预算
                            </th>
                            <th width="10%">
                                学期
                            </th>
                            <th width="20%">
                                添加时间
                            </th>
                            <th width="10%">
                                状态
                            </th>
                            <th width="30%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table width="100%" class="datagrid2">
                         <tr>
                            <td width="10%">
                                <%#Eval("teacher.teacherName")%>
                            </td>
                            <td width="10%">
                                <asp:Literal ID="TeacherSalary_TeacherType_Literal" runat="server"></asp:Literal>
                            </td>
                            <td width="10%">
                                <%#Eval("totalTeachCost") %>
                            </td>
                            <td width="10%">
                                <%#Eval("termTag") %>
                            </td>
                            <td width="20%">
                                <%#Eval("createdTime") %>
                            </td>
                            <td width="10%">
                                <%#bool.Parse(Eval("isConfirm").ToString().Trim()) ? "已确认" : "未确认" %>
                            </td>
                            <td width="30%">
                            <a href="/Teacher/ViewTeacherSalarySummaryDetail.aspx?keepThis=true&teacherSalaryId=<%#Eval("teacherSalaryId").ToString().Trim()%>&TB_iframe=true&height=400&width=500" class="thickbox">查看详情</a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.TeacherSalaryList.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <div>
                当前查询预算总金额：<asp:Literal ID="literal_SelectedTeacherSummay" runat="server"></asp:Literal>(元)                &nbsp;&nbsp;

                预算总金额：<asp:Literal ID="literal_TotalTeacherSummay" runat="server"></asp:Literal>(元)
                
            </div>
                    <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="TeacherSalaryPager" runat="server"
                    FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="TeacherSalaryPager_PageChanged"
                    PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                    HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                    SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                    CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                    TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
                    </webdiyer:AspNetPager>
                </div>
            </div>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment4" runat="server">
            <div>
               
            </div>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment5" runat="server">
            <div>
                教师：<asp:TextBox ID="SalaryQuery_Name" Width="100" runat="server"></asp:TextBox>
                类型：<asp:DropDownList ID="SalaryQuery_TeacherType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SalaryQuery_TermTagChanged"></asp:DropDownList>
                学期：<asp:DropDownList ID="SalaryQuery_TermTag" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SalaryQuery_TermTagChanged">
                      </asp:DropDownList>
                月份：<input type="text" id="SalaryQuery_SalaryMonth" runat ="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM',alwaysUseStartDate:true})"/>
                状态：<asp:DropDownList ID="SalaryQuery_SalaryEntryStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SalaryQuery_TermTagChanged">
                    <asp:ListItem Value="0">-所有状态-</asp:ListItem>
                    <asp:ListItem Value="1">未发放</asp:ListItem>
                    <asp:ListItem Value="4">已发放</asp:ListItem>
                    <asp:ListItem Value="2">未确认</asp:ListItem>
                    <asp:ListItem Value="3">已确认</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btn_SalaryQuery" Text="查找" runat="server" OnClick="SalaryQuery_Click"/>
                <asp:Button ID="btn_SalaryExport" Text="导出" runat="server" OnClientClick="javascript:doExcel4SalaryEntry();" />
            </div>
            <div>
                <asp:DataList Width="100%" ID="SalaryEntryList" runat="server" DataKeyField="salaryEntryId" OnItemDataBound="SalaryEntryManage_ItemBound">
                    <HeaderTemplate>
                        <table width="100%" class="datagrid2">
                            <tr>
                                <th width="10%">
                                    教师
                                </th>
                                <th width="10%">
                                    学年
                                </th>
                                <th width="10%">
                                    月份
                                </th>
                                <th width="10%">
                                    含税薪酬
                                </th>
                                <th width="10%">
                                    不含税薪酬
                                </th>
                                <th width="10%">
                                    税后总薪酬
                                </th>
                                <th width="15%">
                                    创建时间
                                </th>
                                <th width="10%">
                                    状态
                                </th>
                                <th width="15%">
                                    操作
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width="100%" class="datagrid2">
                            <tr>
                                <td width="10%">
                                    <%#Eval("teacher.teacherName") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("termTag") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("salaryMonth") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("teacherCostWithTax") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("teacherCostWithoutTax") %>
                                </td>
                                <td width="10%">
                                    <%#Eval("teacherTotalCost") %>
                                </td>
                                <td width="15%">
                                    <%#Eval("createdTime") %>
                                </td>
                                <td width="10%">
                                    <asp:Literal ID="literal_SalaryEntryStatus" runat="server"></asp:Literal>
                                </td>
                                <td width="15%">
                                    <a href="/Common/ViewTeacherSalaryDetail.aspx?keepThis=true&salaryEntryId=<%#Eval("salaryEntryId").ToString().Trim()%>&TB_iframe=true&height=400&width=600" class="thickbox">查看详情</a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.SalaryEntryList.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" />
                </asp:DataList>
                <div>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <td colspan="5"><span style="font-weight: bold">薪酬汇总</span></td>
                        </tr>
                        <tr>
                            <td width="30%">当前查询薪酬汇总</td>
                            <td width="10%">
                                <asp:Literal ID="literal_SelectedSalaryWithTax" runat="server"></asp:Literal>
                            </td>
                            <td width="10%">
                                <asp:Literal ID="literal_SelectedSalaryWithoutTax" runat="server"></asp:Literal>
                            </td>
                            <td width="10%">
                                <asp:Literal ID="literal_SelectedTotalSalary" runat="server"></asp:Literal>
                            </td>
                            <td width="40%"></td>
                        </tr>
                        <tr>
                            <td>所有薪酬汇总</td>
                            <td>
                                <asp:Literal ID="literal_AllSalaryWithTax" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="literal_AllSalaryWithoutTax" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="literal_AllTotalSalary" runat="server"></asp:Literal>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                    <webdiyer:AspNetPager NumericButtonCount="5" UrlPaging="true" CurrentPageButtonStyle="color:#FFF;" ID="SalaryEntryPager" runat="server"
                    FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="SalaryEntryPager_PageChanged"
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
        function doExcel4TeacherSalary() {
            var href = "/Teacher/OutPutTeacherSalaryExcel.ashx?tname=" + $("#ctl00_ContentPlaceHolder1_TeacherSalary_TeacherName").val() + "&termTag=" + $("#ctl00_ContentPlaceHolder1_TeacherSalary_TermTag").val() + "&teacherType=" + $("#ctl00_ContentPlaceHolder1_TeacherSalary_TeacherType").val() + "&status=" +
                    $("#ctl00_ContentPlaceHolder1_TeacherSalary_Status").val();
            window.open(href);
        }

        function doExcel4SalaryEntry() {

            var href = "/Teacher/OutPutSalaryEntryExcel.ashx?tname=" + $("#ctl00_ContentPlaceHolder1_SalaryQuery_Name").val() + "&termTag=" + $("#ctl00_ContentPlaceHolder1_SalaryQuery_TermTag").val() + "&month=" + $("#ctl00_ContentPlaceHolder1_SalaryQuery_SalaryMonth").val() + "&teacherType=" +
                    $("#ctl00_ContentPlaceHolder1_SalaryQuery_TeacherType").val() + "&status=" + $("#ctl00_ContentPlaceHolder1_SalaryQuery_SalaryEntryStatus").val();

            window.open(href);
        }
        </script>
</asp:Content>


