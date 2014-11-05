<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_NotifyInfoManage"
    MasterPageFile="~/MasterPage/FrameManage.master" CodeBehind="NotifyInfoManage.aspx.cs" %>

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
        });

        function checkNotifyInfo() {
            if ($('#ddlNotifyTypeChild option').length == 0) {
                alert('请选择“' + $('#ddlNotifyType option:selected').text() + "”下的二级分类\n若无二级分类，请在文章类别管理页面添加相应地的二级分类");
                return false;
            }

            if (checkKindValue('Textarea1') && $.trim($('#txtTitle').val()).length > 0) {
                delBeforeUnloadEvent();
                return true;
            }
            else {
                initBeforeUnloadEvent('温馨提示：当前页面数据可能未保存哟~（此为提示，并不代表您真正未保存数据），确定离开吗？');
                return true;
            }
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1" runat="server">
    <div id="container-1" style="padding-top: 40px;">
        <ul class="ui-tabs-nav">
            <li id="liFragment1" runat="server"><a href="?fragment=1"><span>文章管理</span></a></li>
            <li id="liFragment2" runat="server"><a href="?fragment=2"><span>添加文章</span></a></li>
            <li id="liFragment3" runat="server"><a href="?fragment=3"><span>文章类别管理</span></a></li>
        </ul>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment1" runat="server">
            <table align="center" width="100%" class="tableAddStyle">
                <tr>
                    <td width="40%">
                        请选择文章一级分类：
                        <asp:DropDownList ID="ddlNotifyTypeManage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNotifyTypeManage_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        请选择文章二级分类：
                        <asp:DropDownList ID="ddlNotifyTypeManageChild" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNotifyTypeManageChild_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:DataList Width="100%" ID="dlNotify" runat="server" DataKeyField="adminNotifyInfoId"
                OnItemCommand="dlNotify_ItemCommand" CellPadding="0">
                <HeaderTemplate>
                    <table width="100%" class="datagrid2">
                        <tr>
                            <th width="40%">
                                标题
                            </th>
                            <th width="20%">
                                发布时间
                            </th>
                            <th width="10%">
                                浏览次数
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
                            <td width="40%">
                                <%#Eval("notifyTitle")%>
                            </td>
                            <td width="20%">
                                <%#Eval("updateTime")%>
                            </td>
                            <td width="10%">
                                <%#Eval("scanCount")%>
                            </td>
                            <td width="30%">
                                <a href="EditNotifyInfo.aspx?adminNotifyInfoId=<%#Eval("adminNotifyInfoId")%>&page=<%=pageIndex %>&keepThis=true&TB_iframe=true&height=550&width=900"
                                    title="修改通知" class="thickbox">修改</a>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="delete" OnClientClick="return deleteTip();">删除</asp:LinkButton>
                                <asp:LinkButton ID="LinkButton4" runat="server" CommandName="editIstop">                                
                                <%#(Convert.ToInt32(Eval("isTop").ToString().Trim()) > 0 ? "取消置顶" : "置顶")%>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <FooterTemplate>
                    <%=(this.dlNotify.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
            </asp:DataList>
            <webdiyer:AspNetPager NumericButtonCount="5" CurrentPageButtonStyle="color:#FFF;" UrlPaging ="true" ID="AspNetPager1" runat="server"
                FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" 
                PrevPageText="上一页" LayoutType="Table" AlwaysShow="true" CustomInfoHTML="当前第%CurrentPageIndex%页&amp;nbsp;总页数%PageCount%&amp;nbsp;共%RecordCount%条记录&nbsp;"
                HorizontalAlign="Left" InvalidPageIndexErrorMessage="请输入要跳转的整数页号" ShowCustomInfoSection="Left"
                SubmitButtonText="点击跳转" TextBeforePageIndexBox="请输入要跳转的页号：" CssClass="pager"
                CustomInfoSectionWidth="15%" CustomInfoTextAlign="Right" 
                TextAfterPageIndexBox="&amp;nbsp;" ShowBoxThreshold="1">
            </webdiyer:AspNetPager>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment2" runat="server">
            <table align="center" width="100%" class="tableAddStyleNone">
            <tr>
                    <td width="130px" class="border">
                        请选择文章一级分类：
                    </td>
                    <td width="900px" class="border">
                        <asp:DropDownList ID="ddlNotifyType" runat="server" ClientIDMode="Static" AutoPostBack="True" OnSelectedIndexChanged="ddlNotifyType_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr><tr>
                    <td width="130px" class="border">
                        请选择文章二级分类：
                    </td>
                    <td width="900px" class="border">
                        <asp:DropDownList ID="ddlNotifyTypeChild" runat="server"  ClientIDMode="Static"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="border">
                        标题：
                    </td>
                    <td class="border">
                        <asp:TextBox ID="txtTitle" runat="server" Width="300px" ClientIDMode="Static"></asp:TextBox>
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
                    <td colspan="2" class="border">
                        附件：
                        <!--upload start-->
                        <input id="Button3" type="button" value="添加一个附件" onclick="addIframe(<%=fileFolderType %>);" />&nbsp;&nbsp;<b>上传文件大小不超过</b><%=ConfigurationManager.AppSettings["uploadFileLimit"] %>
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
                    <td colspan="2" class="border">
                        <asp:Button ID="btnAdd" runat="server" Text="提交" OnClick="btnAdd_Click" OnClientClick="return checkNotifyInfo();" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="ui-tabs-panel ui-tabs-hide" id="divFragment3" runat="server">
            <div>
                                <a href="AddFirstNotifyType.aspx?keepThis=true&TB_iframe=true&height=200&width=900"
                                    title="添加一级分类" class="thickbox">添加一级分类</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="AddSecondNotifyType.aspx?keepThis=true&TB_iframe=true&height=200&width=900"
                                    title="添加二级分类" class="thickbox">添加二级分类</a>
            </div>
            <br />
            <div>
               <asp:DataList ID="dlstNotifyTypeParent" runat="server" align="center" Width="100%" CellPadding="0"
                OnItemDataBound="dlstNotifyTypeParent_ItemDataBound" RepeatDirection="Horizontal" RepeatColumns="1">
                <ItemTemplate>
                                
                                 <table width="100%" cellpadding="0" cellspacing="0"><tr><td style="border-bottom: 1px solid #056FAE; height: 25px;font-weight:bold;font-size:13px;width:24px;" valign="middle">
                                 <img src="../images/news.gif" width="17" height="17" /></td><td style="border-bottom: 1px solid #056FAE; height: 25px;font-weight:bold;font-size:13px;" valign="middle"><%#Eval("notifyTypeName").ToString().Trim()%>&nbsp;&nbsp;（
                                    <a href="EditFirstNotifyType.aspx?notifyTypeId=<%#Eval("notifyTypeId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=200&width=900" class="thickbox" title="编辑一级分类">
                                        编辑</a>&nbsp;&nbsp; <a href="?del=true&fragment=3&notifyTypeId=<%#Eval("notifyTypeId").ToString().Trim()%>"
                                            onclick="return deleteTip();">删除</a>）&nbsp;&nbsp;全部二级分类</td></tr><tr><td colspan="2"><asp:DataList ID="dlstNotifyType" runat="server" Width="100%">
                    <HeaderTemplate>
                        <table class="datagrid2">
                            <tr>
                                <th width="60%">
                                    类别名称
                                </th>
                                <th width="20%">
                                    显示顺序
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
                                <td width="60%">
                                    <%#Eval("notifyTypeName").ToString().Trim()%>
                                </td>
                                <td width="20%">
                                    <%#Eval("sequence").ToString().Trim()%>
                                </td>
                                <td width="20%">
                                    <a href="EditSecondNotifyType.aspx?notifyTypeId=<%#Eval("notifyTypeId").ToString().Trim()%>&keepThis=true&TB_iframe=true&height=200&width=900" class="thickbox" title="编辑二级分类">
                                        编辑</a>&nbsp;&nbsp; <a href="?del=true&fragment=3&notifyTypeId=<%#Eval("notifyTypeId").ToString().Trim()%>"
                                            onclick="return deleteTip();">删除</a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList></td></tr></table> </ItemTemplate>
                <ItemStyle Width="50%" VerticalAlign="Top" />
                <FooterTemplate>
                    <%=(this.dlstNotifyTypeParent.Items.Count == 0 ? "未找到数据" : null)%></FooterTemplate>
                <FooterStyle CssClass="datalistNoLine" />
                <HeaderTemplate>
                </HeaderTemplate>
            </asp:DataList>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
