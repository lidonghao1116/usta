<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master"
    AutoEventWireup="true" CodeBehind="EnglishExamManage.aspx.cs" Inherits="USTA.WebApplication.Administrator.EnglishExamManage" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link type="text/css" rel="Stylesheet" href="../javascript/tab/ui.css" />
    <link type="text/css" rel="Stylesheet" href="../javascript/thickbox.css" />
    <script type="text/javascript" src="../javascript/ui/My97DatePicker/WdatePicker.js"></script>
    <!--Validate-->
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>
    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#aspnetForm").validate();
        });

        function outputExcel() {
            if ($('#ddlSchoolClassExcel option').length == 0) {
                alert('未找到对应的班级，无法导出，请检查筛选条件！');
                return false;
            };
            var _val = $('#ddlSchoolClassExcel').val();
            if ($.trim(_val) == "all") {
                $('#hrefExcel').attr("href", 'OutputAllEnglishExamSignUpExcel.ashx?englishExamNotifyId=' + $('#ddlEnglishExamNotifyExcel').val() + '&schoolClassId=' + $('#ddlSchoolClassExcel').val() + '&locale=' + $('#ddlLocaleExcel').val());
            } else {
                $('#hrefExcel').attr("href", '/Teacher/OutputEnglishExamSignUpExcel.ashx?useSchoolClassId=true&englishExamNotifyId=' + $('#ddlEnglishExamNotifyExcel').val() + '&schoolClassId=' + $('#ddlSchoolClassExcel').val());
            }
        }

        $(document).ready(function () {
            onLocaleExcelChange();
            $('#ddlSchoolClassExcel').change(function () {
                onLocaleExcelChange();
            });
        });

        function onLocaleExcelChange() {
            var _val = $('#ddlSchoolClassExcel').val();
            if ($.trim(_val) == "all") {
                $('#localeExcel').show();
            } else {
                $('#localeExcel').hide();
            }
        }

    </script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "100%", "200px");
        });

        window.schoolClassName = eval(<%=schoolClassName %>);

        function filter() 
        {
            //清除下拉框值
            $('#ddlSchoolClassExcel').html("");

            var _temp = "<option value=\"all\">全部班级</option>";
            
            for(var item in window.schoolClassName)
            {
                if(item.indexOf($.trim($('#filterKeyword').val()))!=-1)
                {
                    _temp+="<option value=\""+window.schoolClassName[item]+"\">"+item+"</option>";
                }
             }
            $('#ddlSchoolClassExcel').html(_temp);
        }

        //验证
        function checkInputValue(){
            var isInput = checkKindValue('Textarea1');
            if(isInput && $.trim($('#txtTitle').val()).length > 0 && $.trim($('#datepicker').val()).length > 0){
                delBeforeUnloadEvent();
            }
            else{
                initBeforeUnloadEvent('温馨提示：当前页面数据可能未保存哟~（此为提示，并不代表您真正未保存数据），确定离开吗？');
            }
        };

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>四六级考试通知管理</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>添加四六级考试安排</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>四六级考试报名信息管理</span></a></li>
            <li id="liFragment4" runat="server"><a href="?fragment=4"><span>导出四六级报名信息Excel表</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">请选择培养地：<asp:DropDownList ID="ddlEnglishExamNotifyLocale" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlEnglishExamNotifyLocale_SelectedIndexChanged"
                            ClientIDMode="Static">
                            <asp:ListItem Text="合肥" Value="合肥"></asp:ListItem>
                            <asp:ListItem Text="苏州" Value="苏州"></asp:ListItem>
                        </asp:DropDownList>
            <br />
            <asp:DataList Width="100%" ID="dlstEnglishExamNotify" runat="server" DataKeyField="englishExamNotifyId"
                OnItemCommand="dlstEnglishExamNotify_ItemCommand" CellPadding="0">
                <HeaderTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <th width="30%">
                                标题
                            </th>
                            <th width="15%">
                                更新时间
                            </th>
                            <th width="20%">
                                截止日期
                            </th>
                            <th width="10%">
                                浏览次数
                            </th>
                            <th width="15%">
                                通知所面向的培养地
                            </th>
                            <th width="10%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <td width="30%">
                                <a href="/Common/ViewEnglishExamNotify.aspx?englishExamNotifyInfoId=<%#Eval("englishExamNotifyId")%>&keepThis=true&TB_iframe=true&height=500&width=900"
                                    title="<%#Eval("englishExamNotifyTitle").ToString().Trim()%>" class="thickbox"><%#Eval("englishExamNotifyTitle").ToString().Trim()%>(点击查看)</a>
                               
                            </td>
                            <td width="15%">
                                <%#Eval("updateTime").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <%#(Eval("deadLineTime").ToString().Trim())%><%#(Convert.ToDateTime(Eval("deadLineTime").ToString().Trim()) < DateTime.Now ? "(报名已结束)" : "<font color=\"red\">(报名进行中)</font>")%>
                            </td>
                            <td width="10%">
                                <%#Eval("hits").ToString().Trim()%>
                            </td>
                            <td width="15%">
                                <%#Eval("locale").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <a href="EditEnglishExamNotifyInfo.aspx?englishExamNotifyInfoId=<%#Eval("englishExamNotifyId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=500&width=900"
                                    title="修改通知" class="thickbox">修改</a>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="delete" OnClientClick="return deleteTip();">删除</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstEnglishExamNotify.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager1" runat="server" UrlPaging="true"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager1_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;"
                ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <table align="center" width="100%" class="tableAddStyleNone"><tr>
                    <td width="100px" class="border">
                        请选择通知所面向的培养地：
                    </td>
                    <td class="border">
                        <asp:DropDownList ID="ddlLocale" runat="server">
                    <asp:ListItem Text="合肥" Value="合肥"></asp:ListItem>
                    <asp:ListItem Text="苏州" Value="苏州"></asp:ListItem>
                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="100px" class="border">
                        标题：
                    </td>
                    <td class="border">
                        <asp:TextBox ID="txtTitle" runat="server" Width="300px" clientidmode="Static"></asp:TextBox>
                    </td>
                </tr> 
                <tr>
                    <td colspan="2">
                        内容：
                        <div>
                            <textarea id="Textarea1" cols="100" rows="8" runat="server" clientidmode="Static"></textarea></div>
                    </td>
                </tr>
                <tr>
                    <td class="border">
                        截止时间：
                    </td>
                    <td class="border">
                        <input type="text" id="datepicker" runat="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})" clientidmode="Static"
                            class="required" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="border">
                        附件：
                        <!--upload start-->
                        <input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>
                        <div id="iframes">
                        </div>
                        <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                        <!--upload end-->
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="border">
                        <asp:Button ID="btnAdd" runat="server" Text="提交" OnClick="btnAdd_Click" OnClientClick="checkInputValue();" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            <p>
                班级：<asp:DropDownList ID="ddlSerachSchoolClass" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlSerachSchoolClass_SelectedIndexChanged"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                请选择培养地：<asp:DropDownList ID="ddlSearchLocale" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlSearchLocale_SelectedIndexChanged"
                            ClientIDMode="Static">
                            <asp:ListItem Text="合肥" Value="合肥"></asp:ListItem>
                            <asp:ListItem Text="苏州" Value="苏州"></asp:ListItem>
                        </asp:DropDownList>
                &nbsp;&nbsp;&nbsp; 关键字(姓名或学号)：<asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                <asp:Button ID="btnCommit" runat="server" Text="搜索" OnClick="btnCommit_Click" />
            </p>
            <br />
            <asp:DataList ID="dlstEnglishExamSignUpInfo" runat="server" DataKeyField="englishExamId"
                Width="100%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="10%">
                                所属考试
                            </th>
                            <th width="10%">
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
                            <th width="12%">
                                报名时间
                            </th>
                            <th width="12%">
                                截止时间
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="10%">
                                <a href="/Common/ViewEnglishExamNotify.aspx?englishExamNotifyInfoId=<%#Eval("englishExamNotifyId")%>&keepThis=true&TB_iframe=true&height=500&width=900"
                                    title="<%#Eval("englishExamNotifyTitle")%>" class="thickbox"><%#Eval("englishExamNotifyTitle")%>(点击查看)</a>
                            </td>
                            <td width="10%">
                                <%#Eval("studentNo").ToString().Trim()%>
                            </td>
                            <td width="6%">
                                <%#Eval("studentName").ToString().Trim()%>
                            </td>
                            <td width="6%">
                                <%#Eval("examType").ToString().Trim()%>
                            </td>
                            <td width="6%">
                                <%#Eval("examPlace").ToString().Trim()%>
                            </td>
                            <td width="8%">
                                <%#(Eval("englishExamSignUpConfirm").ToString().Trim() == "1" ? "已确认" : "未确认")%>
                            </td>
                            <td width="8%">
                                <%#(Eval("isPaid").ToString().Trim()=="1"?"已缴费":"未缴费")%><%#(Eval("isPaidRemark").ToString().Trim().Length == 0 ? string.Empty : "(" + Eval("isPaidRemark").ToString().Trim() + ")")%>
                            </td>
                            <td width="8%">
                                <%#Eval("examCertificateState").ToString().Trim()%><%#(Eval("examCertificateRemark").ToString().Trim().Length == 0 ? string.Empty : "(" + Eval("examCertificateRemark").ToString().Trim() + ")")%>
                            </td>
                            <td width="8%">
                                <%#Eval("gradeCertificateState").ToString().Trim()%>
                            </td>
                            <td width="6%">
                                <%#Eval("grade").ToString().Trim()%>
                            </td>
                            <td width="12%">
                                <%#Eval("updateTime").ToString().Trim()%>
                            </td>
                            <td width="12%">
                                <%#Eval("deadLineTime").ToString().Trim()%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstEnglishExamSignUpInfo.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager2" runat="server" 
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager2_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;"
                ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
    </div>
    <div class="ui-tabs-panel ui-tabs-hide" id="divFragment4" runat="server">
        <table class="datagrid2">
            <tr>
                <th colspan="2" width="10%">
                    导出Excel
                </th>
            </tr>
            <tr>
                <td width="30%">
                    请选择所属考试通知：
                </td>
                <td>
                    <asp:DropDownList ID="ddlEnglishExamNotifyExcel" runat="server" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="30%">
                    请输入筛选关键字：
                </td>
                <td>
                    <input type="text" id="filterKeyword" />&nbsp;&nbsp;<input type="button" value="筛选"
                        onclick="filter()" />&nbsp;&nbsp;<asp:DropDownList ID="ddlSchoolClassExcel" runat="server"
                            ClientIDMode="Static">
                        </asp:DropDownList><span id="localeExcel" style="display:none;">&nbsp;&nbsp;&nbsp;&nbsp;请选择：<asp:DropDownList ID="ddlLocaleExcel" runat="server"
                            ClientIDMode="Static">
                            <asp:ListItem Text="合肥" Value="合肥"></asp:ListItem>
                            <asp:ListItem Text="苏州" Value="苏州"></asp:ListItem>
                        </asp:DropDownList></span>
                </td>
            </tr>
            <tr>
                <td width="30%">
                </td>
                <td>
                    <a href="javascript:void(0);" id="hrefExcel" title="导出Excel表" onclick="outputExcel();"
                        target="_blank">导出Excel表</a>
                </td>
            </tr>
        </table>
    </div>
    </form>
</asp:Content>
