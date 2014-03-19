<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/FrameManage.master"
    AutoEventWireup="true" Inherits="Administrator_EmailManage" CodeBehind="EmailManage.aspx.cs" %>

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
    <script charset="utf-8" type="text/javascript" src="/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" type="text/javascript" src="/javascript/kindeditorInit.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            initKindEditor(K, 'Textarea1', "100%", "200px");
            initKindEditor(K, 'Textarea2', "100%", "200px");
            initKindEditor(K, 'txtEmailTemplateContent', "100%", "200px"); 
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>邮件管理</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>少量发送邮件</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>批量发送邮件</span></a></li>
            <li id="liFragment4" runat="server"><a href="?fragment=4"><span>邮箱设置</span></a></li>
            <li id="liFragment5" runat="server"><a href="?fragment=5"><span>邮件模板设置</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <p>
                <asp:Button ID="btnDeleteEmailList" Text="删除" runat="server" OnClick="btnDeleteEmailList_Click"
                    OnClientClick="return deleteTip();" />&nbsp;<asp:Literal ID="ltlSelectAllEmail" runat="server"></asp:Literal>&nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlEmailType" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlEmailType_SelectedIndexChanged">
                    <asp:ListItem Value="-1" Text="所有邮件"></asp:ListItem>
                    </asp:DropDownList>
                <asp:DataList ID="dlSendingEmailList" runat="server" DataKeyField="sendingEmailListId"
                    Width="100%">
                    <HeaderTemplate>
                        <table class="datagrid2">
                            <tr>
                                <th width="40%">
                                    邮件标题
                                </th>
                                <th width="20%">
                                    接收者
                                </th>
                                <th width="20%">
                                    邮件类型
                                </th>
                                <th width="20%">
                                    发送状态
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table class="datagrid2">
                            <tr>
                                <td width="40%">
                                    <asp:HiddenField ID="mailId" runat="server" Value='<%#Eval("sendingEmailListId") %>' />
                                    <asp:CheckBox ID="ChkBox" ToolTip="选择/不选" runat="server" />
                                    <%#Eval("emailTitle")%>
                                </td>
                                <td width="20%">
                                    <%#Eval("userName")%>
                                </td>
                                <td width="20%">
                                    <%#USTA.Common.CommonUtility.ReturnEmailTypeByVal(int.Parse(Eval("sendType").ToString().Trim()))%>
                                </td>
                                <td width="20%">
                                    <%#(Eval("isSendSuccess").ToString() == "True" ? "发送成功":"待发送")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%=(this.dlSendingEmailList.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                    <FooterStyle CssClass="datalistNoLine" />
                </asp:DataList>
                <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" ID="AspNetPager1" runat="server"
                    FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" OnPageChanged="AspNetPager1_PageChanged"
                    PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                    HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                    SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                    CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" TextAfterPageIndexBox="&amp;nbsp;"
                    ShowBoxThreshold="1">
                </webdiyer:AspNetPager>
            </p>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <table width="100%" class="tableAddStyle">
                <tr>
                    <td colspan="2" style="border-bottom: 0px;">
                        用户类型：
                        <asp:DropDownList ID="ddlUserType" runat="server">
                            <asp:ListItem>学生</asp:ListItem>
                            <asp:ListItem>教师</asp:ListItem>
                            <asp:ListItem>助教</asp:ListItem>
                        </asp:DropDownList>
                        关键字(用户ID或者名字)：<asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                        <asp:Button ID="btnQuery" runat="server" Text="模糊查询" OnClick="btnQuery_Click" OnClientClick="$('#txtTitle').removeAttr('class');" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="border-bottom: 0px;">
                        <p>
                            <asp:DataList ID="dlSearchUser" runat="server" DataKeyField="userNo" Width="100%"
                                RepeatDirection="Horizontal" RepeatColumns="6">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUserName" Text='<%#Eval("userName")%>' runat="server" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtEmail" Text='<%#Eval("emailAddress")%>' runat="server" Visible="false"></asp:TextBox>
                                    <asp:CheckBox ID="ChkBox" ToolTip="选择/不选" runat="server" />
                                    &nbsp;
                                    <%#Eval("userNo")%>
                                    &nbsp;
                                    <%#Eval("userName")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%=(this.dlSearchUser.Items.Count == 0 ? "未找到数据" : "<font color='red'>选择需要发送邮件的用户</font>")%></FooterTemplate>
                                <FooterStyle CssClass="datalistNoLine" />
                            </asp:DataList>
                            <asp:Literal ID="ltlSelectAllSearchUser" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
            </table>
            <div id="maiContent" runat="server" visible="false">
                <table align="center" width="100%" class="tableAddStyleNone">
                    <tr>
                        <td width="5%" class="border">
                            标题：
                        </td>
                        <td class="border">
                            <asp:TextBox ID="txtTitle" runat="server" Width="300px" ClientIDMode="Static" CssClass="required"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            内容：
                            <textarea id="Textarea1" cols="100" rows="8" runat="server" clientidmode="Static"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="border">
                            附件：
                        </td>
                        <td class="border">
                            <!--upload start-->
                            <span id="upload1" runat="server"></span>
                            <%--<input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />
    <div id="iframes"></div>--%>
                            <!--upload end-->
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="border">
                            <asp:Button ID="btnSendMails" runat="server" Text="发送" OnClick="btnSendMails_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            <table width="100%" class="tableAddStyle">
                <tr>
                    <td colspan="2">
                        选择用户组类型：
                        <asp:DropDownList ID="ddlUserGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlUserGroup_SelectedIndexChanged">
                            <asp:ListItem Value="3">学生</asp:ListItem>
                            <asp:ListItem Value="1">教师</asp:ListItem>
                            <asp:ListItem Value="2">助教</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlCourses" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCourses_SelectedIndexChanged">
                            <asp:ListItem>----请选择课程----</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <p>
                            <asp:DataList ID="dlSearchUserGroup" runat="server" DataKeyField="userNo" Width="100%"
                                RepeatDirection="Horizontal" RepeatColumns="6">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUserName" Text='<%#Eval("userName")%>' runat="server" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtEmail" Text='<%#Eval("emailAddress")%>' runat="server" Visible="false"></asp:TextBox>
                                    <asp:CheckBox ID="ChkBox" ToolTip="选择/不选" runat="server" />
                                    &nbsp;
                                    <%#Eval("userNo")%>
                                    &nbsp;
                                    <%#Eval("userName")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%=(this.dlSearchUserGroup.Items.Count == 0 ? "未找到数据" : "<font color='red'>选择请确认要发送邮件的用户</font>")%></FooterTemplate>
                                <FooterStyle CssClass="datalistNoLine" />
                            </asp:DataList>
                            <asp:Literal ID="ltlSelectAllUser" runat="server"></asp:Literal>
                        </p>
                    </td>
                </tr>
                <div id="EmailCounts" runat="server" visible="false">
                    <table align="center" width="100%" class="tableAddStyleNone">
                        <tr>
                            <td class="border">
                                标题：
                            </td>
                            <td class="border">
                                <asp:TextBox ID="txtEmailTitle" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                内容：
                                <textarea id="Textarea2" cols="100" rows="8" runat="server" clientidmode="Static"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="border">
                                附件：
                                <!--upload start-->
                                <span id="upload2" runat="server"></span>
                                <%--<input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />
    <div id="iframes"></div>
    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />
                                --%>
                                <!--upload end-->
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="border">
                                <asp:Button ID="btnSendMail" runat="server" Text="发送" OnClick="btnSendMail_Click"
                                    OnClientClick="return checkFckValue('ctl00_ContentPlaceHolder1_FCKeditor2');" />
                            </td>
                        </tr>
                    </table>
                </div>
            </table>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment4" runat="server">
            <table width="100%" class="tableEditStyle beautyTableStyle">
                <tr>
                    <td width="15%">
                        邮箱地址：
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="email"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        密 码：
                    </td>
                    <td>
                        <label>
                            <input type="checkbox" name="" onchange="if(this.checked){$('#txtpasswd').show();}else{$('#txtpasswd').hide();}" />&nbsp;如果需要修改密码请勾选此多选框（默认不修改）</label><br />
                        &nbsp;&nbsp;<asp:TextBox ID="txtpasswd" runat="server" ClientIDMode="static" Style="display: none;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        邮件服务器地址：
                    </td>
                    <td>
                        <asp:TextBox ID="txtMailServer" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        服务器端口：
                    </td>
                    <td>
                        <asp:TextBox ID="txtMailServerPort" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="border-bottom: 0px;">
                        <asp:Button ID="btnMailCommit" runat="server" Text="修改" OnClick="btnMailCommit_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hidAttachmentId" ClientIDMode="Static" runat="server" Value="" />

    
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment5" runat="server">
        请选择邮件模板类型：<asp:DropDownList ID="ddlEmailTemplateType" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlEmailTemplateType_SelectedIndexChanged">
                    </asp:DropDownList>
                    <div runat="server" id="divEmailTemplate" visible="false">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tableAddStyle">
                    <tr><td colspan="2"><font color="red">温馨提示：所有的邮件的称呼已经设置好，只需要设置邮件正文内容即可</font></td></tr>
                    	<tr>
                    		<td width="15%">邮件标题：</td><td><asp:TextBox ID="txtEmailTemplateTitle" runat="server" Width="500px"></asp:TextBox></td>
                    	</tr>
                        <tr>
                    		<td width="15%">邮件内容：</td><td><asp:TextBox ID="txtEmailTemplateContent" runat="server" ClientIDMode="Static"></asp:TextBox></td>
                    	</tr>
                        <tr>
                    		<td width="15%"></td><td><asp:Button ID="btnEmailTemplate" runat="server" Text="修改" OnClick="btnEmailTemplate_Click" /></td>
                    	</tr>
                    </table>
                    </div>
        </div>
    </form>
</asp:Content>
