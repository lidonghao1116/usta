<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master" AutoEventWireup="true" CodeBehind="ExamSeatArrange.aspx.cs" Inherits="USTA.WebApplication.Administrator.ExamSeatArrange" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
<div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>考试座位安排</span></a></li>
            <li id="liFragment2" runat="server" visible="false"><a href="?fragment=2"><span>暂不使用</span></a></li>
            <li id="liFragment3" runat="server" visible="false"><a href="?fragment=3"><span>暂不使用</span></a></li>
            </ul>

            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
                        &nbsp;&nbsp;请选择培养地：<asp:DropDownList ID="ddlGradeCheckLocale" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlGradeCheckLocale_SelectedIndexChanged">
                            <asp:ListItem Text="合肥" Value="合肥"></asp:ListItem>
                            <asp:ListItem Text="苏州" Value="苏州"></asp:ListItem>
                        </asp:DropDownList>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlCourses" runat="server" ClientIDMode="Static">
                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;请输入行数：<asp:TextBox ID="txtRows" runat="server" ClientIDMode="Static" CssClass="required number" Width="50px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnExamSeatArrange" runat="server" Text="计算列数并随机生成考试座位" OnClick="btnExamSeatArrange_Click" />&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" title="导出当前所选课程座位排列Excel表" onclick="this.href='OutputExamSeatArrange.ashx?rows='+$('#txtRows').val()+'&courseName='+encodeURIComponent(document.getElementById('ddlCourses').options[document.getElementById('ddlCourses').selectedIndex].text)"
                target="_blank" runat="server" visible="false" id="outputExcel">导出当前所选课程座位排列Excel表</a><br /><br />

                <asp:PlaceHolder ID="phExamSeats" runat="server"></asp:PlaceHolder>
                        </div>

            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server"></div>

            <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server"></div>
            </div>
            </form>
</asp:Content>
