<%@ Page Language="C#" AutoEventWireup="true" Inherits="Administrator_BaseConfig" MasterPageFile="~/MasterPage/FrameManage.master" Codebehind="BaseConfig.aspx.cs" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form id="form1" runat="server">
    <br />
    <fieldset style="width:60%; margin:auto;"><legend>系统设置</legend><table width="60%" align="center">
        <tr>
            <td>
    系统名称：<asp:TextBox ID="txtSystemName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
    系统版本：<asp:TextBox ID="txtSystemVersion" runat="server" 
        ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
    系统版权：</td>
        </tr>
        <tr>
            <td>
    <asp:TextBox ID="txtSystemCopyright" runat="server" Height="253px" TextMode="MultiLine" 
        Width="438px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
    文件服务器地址：</td>
        </tr>
        <tr>
            <td>
    <asp:TextBox ID="txtServerAddress" runat="server" Width="497px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
    <asp:Button ID="btnUpdate" runat="server" Text="确定" onclick="btnUpdate_Click" />
            </td>
        </tr>
    </table>
    <br /></fieldset><br />
</form>

</asp:Content>
