<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BBSindex.master" AutoEventWireup="true" EnableViewState="false" Inherits="bbs_BBSUserinfo" Codebehind="BBSUserinfo.aspx.cs" %>
<%@ MasterType  VirtualPath="~/MasterPage/BBSindex.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BBSContent" Runat="Server">
<div class="ui-tabs-panel">
<table width="100%"><tr><td width="40%">
        更改头像：<div id="head"><asp:Literal ID="ltavatar" runat="server"></asp:Literal></div>
        
       <a href="AvatarsSelect.aspx?keepThis=true&TB_iframe=true&height=400&width=700"
                                            title="选择系统自定义头像" class="thickbox">选择系统自定义头像</a>&nbsp;&nbsp;或&nbsp;&nbsp;<a href="UploadAvatar.aspx?keepThis=true&TB_iframe=true&height=200&width=600"
                                            title="上传自定义头像" class="thickbox">上传自定义头像</a>
        </td><td>&nbsp;
            
            </td></tr></table>
            </div>
</asp:Content>

