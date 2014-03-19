<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_StudentManager"
    MasterPageFile="~/MasterPage/FrameManage.master" CodeBehind="StudentManager.aspx.cs" %>

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

        function outputWord() {
            if ($('#ddlCourses').val() == 'all') {
                alert("请选择课程：）");
                return false;
            }

            $('#hrefOutputWord').attr('href', 'OutputGradeCheckApplyWord.aspx?termTagCourseNoClassID=' + $('#ddlCourses').val() + '&courseName=' + document.getElementById('ddlCourses').options[document.getElementById('ddlCourses').selectedIndex].text + '&termTag=' + $('#ddlTermTags').val());
            return true;
        }


        function checkNotifyInfo() {
            var startTime = Date.parse($('#startTime').val().replace(/-/g, "/"));
            var endTime = Date.parse($('#endTime').val().replace(/-/g, "/"));
            if (startTime > endTime) {
                alert("开始时间不能晚于截止时间，请修改:)");
                return false;
            }

            if (checkKindValue('txtNotifyContent') && $.trim($('#txtNotifyTitle').val()).length > 0) {
                delBeforeUnloadEvent();
                return true;
            }
            else {
                initBeforeUnloadEvent('温馨提示：当前页面数据可能未保存哟~（此为提示，并不代表您真正未保存数据），确定离开吗？');
            }
            return false;
        }
    </script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'txtNotifyContent', "100%", "200px");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>搜索学生信息</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>重修重考申请管理</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>重修重考原因管理</span></a></li>
            <li id="liFragment4" runat="server" visible="false"><a href="?fragment=4"><span>修改选课</span></a></li>
            <li id="liFragment5" runat="server"><a href="?fragment=5"><span>成绩审核规则管理</span></a></li>
            <li id="liFragment6" runat="server" visible="false"><a href="?fragment=6"><span>成绩审核Excel导入</span></a></li>
            <li id="liFragment7" runat="server" visible="false"><a href="?fragment=7" visible="false">
                <span>学生成绩审核管理</span></a></li>
            <li id="liFragment8" runat="server" visible="false"><a href="?fragment=8"><span>暂不使用</span></a></li>
            <li id="liFragment9" runat="server"><a href="?fragment=9"><span>重修重考通知及开放时间管理</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <table style="margin-bottom: 10px">
                <tr>
                    <td>
        <div style="margin-bottom: 10px;">
                        请选择学年：<asp:DropDownList ID="ddlSearchYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearchYear_SelectedIndexChanged" ClientIDMode="static">
                        </asp:DropDownList>
                        &nbsp;&nbsp;专业：<asp:DropDownList ID="ddlSearchMajor" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlSearchMajor_SelectedIndexChanged">
                            <asp:ListItem Text="在所有专业中查找" Value="all"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;班级：<asp:DropDownList ID="ddlSearchSchoolClass" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlSearchSchoolClass_SelectedIndexChanged">
                            <asp:ListItem Text="在所有班级中查找" Value="all"></asp:ListItem>
                        </asp:DropDownList>&nbsp;&nbsp;
                        请选择是否符合学位申请筛选条件：<asp:DropDownList ID="ddlGradeCheckDegree" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlGradeCheckDegree_SelectedIndexChanged">
                            <asp:ListItem Text="全部" Value="all"></asp:ListItem>
                            <asp:ListItem Text="未处理" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="不符合" Value="0"></asp:ListItem>
                            <asp:ListItem Text="符合" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;请选择培养地：<asp:DropDownList ID="ddlGradeCheckLocale" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlGradeCheckLocale_SelectedIndexChanged" ClientIDMode="Static">
                            <asp:ListItem Text="全部" Value="all"></asp:ListItem>
                            <asp:ListItem Text="合肥" Value="合肥"></asp:ListItem>
                            <asp:ListItem Text="苏州" Value="苏州"></asp:ListItem>
                        </asp:DropDownList>&nbsp;&nbsp;</div>
        <div style="margin-top:10px;margin-bottom: 10px;">
                        <asp:Label ID="lblKeyword" runat="server" Font-Size="9pt" Text="关键字(学生学号或姓名)："></asp:Label>&nbsp;
                        <asp:TextBox ID="txtKeyword" runat="server" ClientIDMode="Static"></asp:TextBox>&nbsp;&nbsp;
                        <asp:Button ID="btnQuery" runat="server" Text="模糊查询" OnClick="btnQuery_Click" />
            &nbsp;&nbsp;&nbsp;<a href="#" title="导出当前学年及培养地所有学生成绩审核信息Excel表" onclick="this.href='OutputGradeCheckExcel.ashx?year='+$('#ddlSearchYear').val()+'&locale='+$('#ddlGradeCheckLocale').val();"
                target="_blank">导出当前学年及培养地所有学生成绩审核信息Excel表</a></div>
                    </td>
                </tr>
            </table>
            <asp:DataList ID="dlSearchStudent" runat="server" DataKeyField="studentNo" Width="100%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="6%">
                                学生编号
                            </th>
                            <th width="6%">
                                学生名称
                            </th>
                                <th width="12%">
                                    班级</th>
                            <th width="6%">
                                培养地
                                </th>
                            <th width="10%">
                                是否符合学位申请要求
                                </th>
                                <%=tableHeader%>
                                    <th width="5%">
                                        操作
                                    </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="6%">
                                <%#Eval("studentNo").ToString().Trim()%>
                            </td>
                            <td width="6%">
                                <%#Eval("studentName").ToString().Trim()%>
                            </td>
                            <td width="12%">
                                <%#Eval("SchoolClassName").ToString().Trim()%>
                            </td>
                            <td width="6%">
                                <%#Eval("locale").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <%#string.IsNullOrEmpty(Eval("isAccord").ToString().Trim()) ? "未处理" : (Eval("isAccord").ToString().Trim()=="1"?"符合":"不符合")%>
                            </td>
                            <%#this.GetTableTd(Eval("studentNo").ToString().Trim(), !string.IsNullOrEmpty(Eval("updateTime").ToString().Trim()) ? DateTime.Parse(Eval("updateTime").ToString().Trim()) : DateTime.Now)%>
                            <td width="5%"><a href="StudentManager.aspx?fragment=7&studentNo=<%#Eval("studentNo").ToString().Trim()%>">
                                        编辑</a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlSearchStudent.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
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
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
        <div style="margin-bottom: 10px;">
            请选择学期：<asp:DropDownList ID="ddlTermTags" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTermTags_SelectedIndexChanged" ClientIDMode="Static">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;请选择课程：<asp:DropDownList ID="ddlCourses" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlCourses_SelectedIndexChanged" ClientIDMode="Static">
            </asp:DropDownList>
                        &nbsp;&nbsp;请选择审核结果：<asp:DropDownList ID="ddlApplyResult" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlApplyResult_SelectedIndexChanged">
                            <asp:ListItem Text="全部" Value="all"></asp:ListItem>
                            <asp:ListItem Text="不符合" Value="不符合"></asp:ListItem>
                            <asp:ListItem Text="符合" Value="符合"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;请选择培养地：<asp:DropDownList ID="ddlApplyLocale" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlApplyLocale_SelectedIndexChanged">
                            <asp:ListItem Text="全部" Value="all"></asp:ListItem>
                            <asp:ListItem Text="合肥" Value="合肥"></asp:ListItem>
                            <asp:ListItem Text="苏州" Value="苏州"></asp:ListItem>
                        </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;</div>
            <div style="margin-bottom: 10px;">
            请输入要查找的课程名称关键字：<asp:TextBox ID="txtCourseName" runat="server" ClientIDMode="Static"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button5" runat="server" Text="模糊查询" OnClick="btnQueryCourses_Click" /><%--&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" title="导出当前所选课程所有重修重考信息Excel表" onclick="this.href='OutputGradeCheckApplyExcel.ashx?termTagCourseNoClassID='+($('#ddlCourses').val()=='all'?'all_'+$('#ddlTermTags').val():$('#ddlCourses').val())+'&courseName='+document.getElementById('ddlCourses').options[document.getElementById('ddlCourses').selectedIndex].text+'&termTag='+$('#ddlTermTags').val();"
                target="_blank">导出当前所选课程所有重修重考信息Excel表</a>--%>&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0);" title="导出当前所选课程所有审核通过的重修重考信息Word文档" onclick="return outputWord();" id="hrefOutputWord"
                target="_blank">导出当前所选课程所有审核通过的重修重考信息Word文档</a></div><asp:DataList
                ID="dlstGradeCheckApply" runat="server" DataKeyField="studentNo" Width="100%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="8%">
                                学号
                            </th>
                            <th width="8%">
                                学生姓名
                            </th>
                            <th width="10%">
                                班级
                            </th>
                            <th width="20%">
                                重修重考课程
                            </th>
                            <th width="10%">
                                重修重考原因
                            </th>
                            <th width="10%">
                                申请时间
                            </th>
                            <th width="8%">
                                审核结果
                            </th>
                            <th width="6%">
                                培养地
                            </th>
                            <th width="6%">
                                申请类型
                            </th>
                            <th width="14%">
                                操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="8%">
                                <%#Eval("studentNo").ToString().Trim()%>
                            </td>
                            <td width="8%">
                                <%#Eval("studentName").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <%#Eval("SchoolClassName").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <%#Eval("courseName").ToString().Trim() + "(" + Eval("classID").ToString().Trim() + ")"%>
                            </td>
                            <td width="10%">
                                <%#Eval("applyReason").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <%#Eval("updateTime").ToString().Trim()%>
                            </td>
                            <td width="8%">
                                <%#(string.IsNullOrEmpty(Eval("applyResult").ToString().Trim())? "未处理" :Eval("applyResult").ToString().Trim())%>
                            </td>
                            <td width="6%">
                                <%#Eval("locale").ToString().Trim()%>
                            </td>
                            <td width="6%">
                                <%#Eval("gradeCheckApplyType").ToString().Trim()%>
                            </td>
                            <td width="14%">
                                <a href="DealGradeCheckApply.aspx?keepThis=true&studentNo=<%#Eval("studentNo").ToString().Trim()%>&gradeCheckApplyId=<%#Eval("gradeCheckApplyId").ToString().Trim()%>&TB_iframe=true&height=300&width=800"
                                    title="查看并处理申请信息" class="thickbox">查看并处理申请信息</a>&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstGradeCheckApply.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" UrlPaging="true" ID="AspNetPager1"
                runat="server" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager1_PageChanged"
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;"
                ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            <div>
                <a href="AddGradeCheckApplyReason.aspx?keepThis=true&TB_iframe=true&height=300&width=900"
                    title="添加重修重考原因" class="thickbox">添加重修重考原因</a></div>
            <asp:DataList ID="dlstGradeCheckApplyReason" DataKeyField="gradeCheckApplyReasonId" runat="server" Width="100%"
                OnItemCommand="dlstGradeCheckApplyReason_ItemCommand">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="40%">
                                名称
                            </th>
                            <th width="40%">
                                备注
                            </th>
                            <th width="20%">
                                相关操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="40%">
                                <%#Eval("gradeCheckApplyReasonTitle").ToString().Trim()%>
                            </td>
                            <td width="40%">
                                <%#Eval("gradeCheckApplyReasonRemark").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <a href="EditGradeCheckApplyReason.aspx?gradeCheckApplyReasonId=<%#Eval("gradeCheckApplyReasonId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=300&width=900"
                                    class="thickbox" title="修改重修重考原因">编辑</a>&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="delete" OnClientClick="return deleteTip();">删除</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstGradeCheckApplyReason.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>

        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment4" runat="server">
            学生：
            <asp:Label ID="lblstudentName" runat="server" Text="Label"></asp:Label>
            <br />
            <span style="color: Red; width: 500px;">温馨提示：由于选课数据已经完全与信息系统一致同步，因此教学系统不再提供选课删除功能，<br />
                因为这样的操作会影响数据一致性，若要为学生添加或者删除课程，请在信息系统中进行，<br />
                然后再使用教学系统中的：系统功能-数据同步-学生选课数据(包括学生作业与实验数据)，进行同步即可。</span> &nbsp;&nbsp;
            <br />
            <br />
            <asp:DataList ID="dlstcourses" runat="server" Width="40%">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="100%">
                                当前学期已选课程名称
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="100%">
                                <%#Eval("courseName")%>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstcourses.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment5" runat="server">
            <div style="margin-bottom: 10px">请选择学期：<asp:DropDownList ID="ddlTermYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTermYear_SelectedIndexChanged" ClientIDMode="Static">
        <asp:ListItem Value="all" Text="全部"></asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                <a href="AddGradeCheckItem.aspx?keepThis=true&TB_iframe=true&height=300&width=900"
                    title="添加单项成绩审核规则" class="thickbox">添加单项成绩审核规则</a></div>
            <asp:DataList ID="dlstGradeCheck" DataKeyField="gradeCheckId" runat="server" Width="100%"
                OnItemCommand="dlstGradeCheck_ItemCommand">
                <HeaderTemplate>
                    <table class="datagrid2">
                        <tr>
                            <th width="40%">
                                名称
                            </th>
                            <th width="20%">
                                默认值
                            </th>
                            <th width="20%">
                                所属学年
                            </th>
                            <th width="10%">
                                显示顺序
                            </th>
                            <th width="10%">
                                相关操作
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="datagrid2">
                        <tr>
                            <td width="40%">
                                <%#Eval("gradeCheckItemName").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <%#Eval("gradeCheckItemDefaultValue").ToString().Trim()%>
                            </td>
                            <td width="20%">
                                <%#(Eval("termYear").ToString().Trim().Length == 0 ? "暂无" : "20" + Eval("termYear").ToString().Trim() + "学年")%>
                            </td>
                            <td width="10%">
                                <%#Eval("displayOrder").ToString().Trim()%>
                            </td>
                            <td width="10%">
                                <a href="EditGradeCheckItem.aspx?gradeCheckId=<%#Eval("gradeCheckId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=300&width=900"
                                    class="thickbox" title="修改单项成绩审核数据">编辑</a>&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="delete" OnClientClick="return deleteTip();">删除</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlstGradeCheck.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment6" runat="server">  <div class="ViewTips" style="margin-left:auto;margin-right:auto;float:none;margin-bottom:30px;"><div class="TipsTopic" style="height:30px;text-align:left;text-indent:40px;">成绩审核Excel导入格式要求：</div><div class="TipsTopic" style="height:30px;text-align:left;text-indent:40px;">1. 上传文件类型必须为Excel文件；</div><div class="TipsTopic" style="height:30px;text-align:left;text-indent:40px;">2. Excel文件第一行必须为列名，并且前七列的顺序必须为：序号、学号、姓名、性别、班级、年级、班主任，</div><div class="TipsTopic" style="height:30px;text-align:left;text-indent:40px;">最后两列的列名必须为“是否符合学位申请条件”、“不及格科目”；</div><div class="TipsTopic" style="height:30px;text-align:left;text-indent:40px;">3. Excel中的所有成绩审核规则相关的列名必须与教学辅助系统中“成绩审核规则数据”中对应学年的规则名称完全一致，</div><div class="TipsTopic" style="height:30px;text-align:left;text-indent:40px;">不能有换行、空格、特殊字符等；</div><div class="TipsTopic" style="height:30px;text-align:left;text-indent:40px;">4. 具体学生记录中如果有数字类型值，必须为英文输入法状态下的数字。</div></div>
            <table class="tableAddStyle">
                <tr>
                    <td width="20%">
                        请选择学年：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTermYearImportExcelData" runat="server" ClientIDMode="static">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    请选择要导入的成绩审核Excel表：
                    </td>
                    <td>
        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td><asp:Button ID="btnImportGradeCheckExcelData" runat="server" Text="导入Excel数据" 
             OnClick="btnImportGradeCheckExcelData_Click" OnClientClick="return confirm('确认导入当前所选学年的成绩审核Excel数据吗？\n此操作将会替换系统中当前所选学年的原有成绩审核数据');" />
                    </td>
                </tr>
            </table>
        
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment7" runat="server">
            <asp:DataList ID="dlstStudentSchoolClassName" runat="server" align="center" Width="100%"
                CellPadding="0" RepeatDirection="Horizontal" RepeatColumns="2">
                <ItemTemplate>
                    <table style="border-bottom: 1px solid #056FAE; height: 25px;" width="100%">
                        <tr>
                            <td style="height: 25px; width: 30%;">
                                <%#Eval("SchoolClassName").ToString().Trim()%>&nbsp;&nbsp;学号：<%=this.studentNo %>&nbsp;&nbsp;<%#Eval("studentName").ToString().Trim() %>
                            </td>
                            <td>
                                <a href="AddGradeCheckDetail.aspx?studentNo=<%=this.studentNo%>&keepThis=true&TB_iframe=true&height=400&width=800"
                                    class="thickbox" title="添加一次成绩审核记录">添加一次成绩审核记录</a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                <FooterTemplate>
                    <%=(this.dlstStudentSchoolClassName.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                <HeaderTemplate>
                </HeaderTemplate>
            </asp:DataList>
            <asp:DataList ID="dlstStudentGradeCheck" runat="server" align="center" Width="100%"
                CellPadding="0" OnItemDataBound="dlstStudentGradeCheck_ItemDataBound" RepeatDirection="Horizontal"
                RepeatColumns="1">
                <ItemTemplate>
                    <table style="border-bottom: 1px solid #056FAE; height: 25px;" width="100%">
                        <tr>
                            <td style="height: 25px; width: 50%;">
                                成绩审核时间：<%#Eval("updateTime").ToString().Trim()%>
                            </td>
                            <td>
                                <a href="EditGradeCheckDetail.aspx?studentNo=<%=this.studentNo%>&updateTime=<%#Eval("updateTime").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=400&width=800"
                                    class="thickbox" title="编辑">编辑</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="?fragment=7&action=delete&studentNo=<%=this.studentNo%>&updateTime=<%#Eval("updateTime").ToString().Trim()%>"
                                        onclick="return deleteTip();" title="删除">删除</a>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px;" colspan="2">
                                <%--是否符合学位申请条件：<%#(this.GetStudentGradeCheckConfirm(DateTime.Parse(Eval("updateTime").ToString().Trim())) == "1" ? "符合" : "<font color=\"red\">不符合</font>")%>&nbsp;&nbsp;不及格科目（备注）：<%#this.GetStudentGradeCheckConfirmAboutRemark(DateTime.Parse(Eval("updateTime").ToString().Trim()))%>&nbsp;&nbsp;--%><%#this.GetGradeCheckApplyInfo(int.Parse(Eval("gradeCheckApplyId").ToString().Trim()))%>
                            </td>
                        </tr>
                    </table>
                    <asp:DataList ID="dlstStudentGradeCheckItem" runat="server" CellPadding="0" Width="100%"
                        Style="margin-bottom: 10px;">
                        <ItemTemplate>
                            <table style="border-bottom: 1px dashed #CCCCCC;" width="90%">
                                <tr>
                                    <td width="70%" style="height: 25px;">
                                        <img src="../images/BULLET.GIF" align="absmiddle" />
                                        <%#this.GetGradeCheckDetailByGradeCheckId(int.Parse(Eval("gradeCheckId").ToString().Trim()))%><%#Eval("gradeCheckDetailValue").ToString().Trim()%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <FooterTemplate>
                            未找到数据</FooterTemplate>
                        <FooterStyle CssClass="datalistNoLine" />
                    </asp:DataList>
                </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                <FooterTemplate>
                    <%=(this.dlstStudentGradeCheck.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                <HeaderTemplate>
                </HeaderTemplate>
            </asp:DataList>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment8" runat="server">
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment9" runat="server">
            <table class="tableAddStyle" width="100%">
            <tr>
                    <td width="15%">重修重考开始办理时间：
                    </td>
                    <td>
                       <input type="text" id="startTime" runat="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})"
                class="required" clientidmode="Static" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">重修重考截止办理时间：
                    </td>
                    <td>
                        <input type="text" id="endTime" runat="server" onclick="WdatePicker({startDate:'%y-%M-01 00:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})"
                class="required" clientidmode="Static" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">通知标题：
                    </td>
                    <td>
                        <asp:TextBox ID="txtNotifyTitle" runat="server" Width="600px" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        通知内容：
                    </td>
                    <td>
                        <asp:TextBox ID="txtNotifyContent" runat="server" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        附件：
                    </td>
                    <td>
                        <span id="spanAttachment" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                    </td>
                    <td>
                        <!--upload start-->
                        <input id="Button4" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>
                        <div id="iframes">
                            <%-- <div id="iframes1">
                                <div id="upLoading1">
                                </div>
                                <div id="upSuccess1">
                                </div>
                                <div id="upError1">
                                </div>
                                <iframe id="frUpload1" scrolling="no" src="../Common/Upload.aspx?frId=1&fileFolderType=<%=fileFolderType %>"
                                    height="30px" style="width: 799px" frameborder="0"></iframe>
                            </div>--%>
                        </div>
                        <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                        <!--upload end-->
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                    </td>
                    <td>
                        <asp:Button ID="Button2" runat="server" Text="提交" OnClick="Button2_Click" OnClientClick="checkNotifyInfo();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</asp:Content>
