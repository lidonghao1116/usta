<%@ Page Language="C#" AutoEventWireup="true" Inherits="Teacher_EditCourseinfoTip" Codebehind="EditCourseinfoTip.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="masterHead">
    <title></title>
        
        <!--Validate-->
 <script type="text/javascript" src="../javascript/jquery-1.4.min.js"></script>
    <script type="text/javascript" src="../javascript/validate/jquery.validate.js"></script>

    <script type="text/javascript" src="../javascript/validate/cmxforms.js"></script>
    
        <script src="../tinymce/tiny_mce_src.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        tinyMCE.init({
            mode: "exact",
            elements: "text_ReferenceBooks,txtAnswer,tex_preCourses",
            editor_selector: "mceEditor",
            theme: "advanced",
            width: "500px",
            height: "100px",
            plugins: "safari,pagebreak,style,advhr,advimage,advlink,iespell,insertdatetime,preview,searchreplace,contextmenu,paste,directionality,noneditable,visualchars,nonbreaking,xhtmlxtras,inlinepopups",
            // Theme options
            theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist,|,outdent,indent,blockquote,|,forecolor,backcolor,|,link,unlink,anchor,cleanup,|,syntaxhl,preview",
            theme_advanced_buttons2: "",
            theme_advanced_buttons3: "",
            theme_advanced_buttons4: "",
            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: true,
            remove_linebreaks: false,
            extended_valid_elements: "textarea[cols|rows|disabled|name|readonly|class]",
            content_css: "tinymce/themes/simple/skins/o2k7/content.css",
            language: "zh"
        });
    </script>
</head>
<body>
   <form id="form1" runat="server">
    <div>
    <table width="100%" class="tableAddStyle">
        <tr><td width="60px">先修课程：</td><td><asp:TextBox ID="tex_preCourses" runat="server" 
                Height="100px" TextMode="MultiLine" Width="500px"></asp:TextBox></td></tr>
        <tr><td>参考书：</td><td><asp:TextBox ID="text_ReferenceBooks" runat="server" 
                Height="100px" TextMode="MultiLine" Width="500px"></asp:TextBox></td></tr>
        <tr><td>课程（老师）主页：</td><td><asp:TextBox ID="txtcourseurl" runat="server" 
                Height="25px" Width="500px"></asp:TextBox></td></tr>
        <tr><td>考试方式：</td><td><asp:TextBox ID="txtExamtype" runat="server" Width="500px"></asp:TextBox></td></tr>
        <tr><td>答疑安排：</td><td><asp:TextBox ID="txtAnswer" runat="server" Height="100px" 
                TextMode="MultiLine" Width="500px"></asp:TextBox></td></tr>
        <tr><td style="border-bottom:0px;"></td><td style="border-bottom:0px;"><asp:Button ID="EditCourses" runat="server" onclick="EditCourses_Click"  Text="提交" /></td></tr>
       </table>
       
        <asp:HiddenField ID="hidencourseNo" runat="server" />
    
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#form1").validate();
         uploadInfo.iframeCount=<%=iframeCount %>;
        });
    </script>
    </form>
</body>
</html>
