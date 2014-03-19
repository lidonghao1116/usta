<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="USTA.WebApplication.Common.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>温馨提示</title>
<link href="/css/error.css?ver=20110703" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
<div id="errorbox1">
<div class="errortitle">温馨提示</div>
<div class="errorcontent">
<p class="red">非常抱歉，您刚才的操作没有成功。<br />我们已经收到了这个错误信息，会尽快处理。请您先选择以下链接进行其他的操作。 <br /></p>
<p><a href="<%=errorUri %>">返回出错页面</a> | <a href="/Common/NotifyList.aspx">返回系统首页</a></p>
<p>&nbsp;</p>
<p style="height: 120px"></p></div></div>
    </form>
</body>
</html>
