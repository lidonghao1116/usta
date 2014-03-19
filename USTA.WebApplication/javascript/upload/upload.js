
var uploadInfo = { iframeCount: "0", iframesFlag: "0" };

function addIframe(fileFolderType) {
    //判断是否超过附件限制
    if (uploadInfo.iframeCount >= 100) {
        alert("最多允许上传100个附件！当前的附件数已经超过100个！");
        return;
    }

    //判断1-100中哪个iframe未存在
    for (i = 1; i <= 100; i++) {
        if ($('#frUpload' + i).attr("src") == undefined) {
            uploadInfo.iframesFlag = i;
            break;
        }
    }

    var iframesId = "iframes";
    var hiddenId = "hidAttachmentId";
    var containerId = '';

    uploadInfo.iframeCount++;

    if (arguments.length == 3) {
        iframesId += arguments[1];
        hiddenId += arguments[2];
        containerId = arguments[1];
    }


    //添加附件
    $('#iframes' + containerId).append("<div style=\"margin-top:5px;\" class=\"uploadStyle\" id=\"iframe" + uploadInfo.iframesFlag + "\"><table><tr><td valign=\"middle\"><iframe id=\"frUpload" +
            uploadInfo.iframesFlag + "\"  scrolling=\"no\" src=\"../Common/Upload.aspx?frId=" + uploadInfo.iframesFlag + "&fileFolderType=" + fileFolderType + "&iframeId=" + uploadInfo.iframesFlag + "&hiddenId=" + containerId
             + "\" height=\"26px\" style=\"width: 250px\" frameborder=\"0\"></iframe></td><td valign=\"top\" align='left'><div id=\"upLoading" + uploadInfo.iframesFlag +
            "\"></div><div id=\"upSuccess" + uploadInfo.iframesFlag + "\"></div><div id=\"upError" + uploadInfo.iframesFlag + "\"></div><span><a style='cursor:pointer;text-decoration:underline;' id=\"upDeleteIframe" + uploadInfo.iframesFlag + "\" onclick=\"deleteIframeById(" + uploadInfo.iframesFlag + ",'" + containerId + "');\">删除</a></span></td></tr></table></div>");
}

function uploadLoading(frId, fileFolderType) {
    $('#frUpload' + frId).css("display", "none");
    $('#upSuccess' + frId).css("display", "none");
    $('#upLoading' + frId).css("display", "block");
    $('#upError' + frId).css("display", "none");

    $('#upLoading' + frId).html("<img src='../javascript/loadingAnimation.gif' align='absmiddle' title='上传中，请稍候...' alt='上传中，请稍候...' />上传中，请稍候...");
    setTimeout("funcTimeOut(" + frId + "," + fileFolderType + ", '上传文件失败，请检查文件大小后重新上传！')", 5000);
}

//超时函数
function funcTimeOut(frId, fileFolderType, errorMessage) {
    if ($('#upSuccess' + frId).css("display") == "none") {
        if ($('#frUpload' + frId).contents().find('#hidFrId').length == 0) {
            uploadError(frId, fileFolderType, errorMessage);
        }
        else {
            setTimeout("funcTimeOut(" + frId + "," + fileFolderType + ", '上传文件失败，请检查文件大小后重新上传！')", 5000);
        }
    }
}

function uploadSuccess(frId, attachmentId, fileName, fileFolderType, hiddenId) {
    var _hiddenId = hiddenId;
    var hiddenId = '#hidAttachmentId' + _hiddenId;

    if ($.trim($(hiddenId).val()).length > 0) {
        $(hiddenId).val($(hiddenId).val() + ',' + attachmentId);
    }
    else {
        $(hiddenId).val(attachmentId);
    }
    $('#upSuccess' + frId).css("display", "block");
    $('#upLoading' + frId).css("display", "none");
    $('#frUpload' + frId).css("display", "none");
    $('#upError' + frId).css("display", "none");
    $('#upDeleteIframe' + frId).css("display", "none");

    $('#upSuccess' + frId).html("<img src='../images/note_ok.gif' align='absmiddle' />" + fileName + "上传成功！<a style='cursor:pointer;text-decoration:underline;' onclick=\"deleteAttachment(" + attachmentId + "," + frId + ",'" + _hiddenId + "');\">[删除]</a>");
}

function uploadError(frId, fileFolderType, errorMessage) {
    $('#upSuccess' + frId).css("display", "none");
    $('#upLoading' + frId).css("display", "none");
    $('#frUpload' + frId).css("display", "block").attr("src", "../Common/Upload.aspx?frId=" + frId + "&fileFolderType=" + fileFolderType + "&iframeId=" + frId + "&hiddenId=" + frId);
    $('#upError' + frId).css("display", "block").html("<img src='../images/note_error.gif' align='absmiddle' />" + errorMessage);
}

function deleteIframeById(iframeId,containerId) {
    $("#iframe" + iframeId).remove();
    uploadInfo.iframeCount -= 1;
}

function deleteAttachment(attachmentId, frId, hiddenId) {
    uploadInfo.iframeCount--;

    if ($.trim($('#hidAttachmentId' + hiddenId).val()).length > 0) {

        var arrayTemp = $('#hidAttachmentId' + hiddenId).val().split(",");

        var attachmentIds = [];

        for (i = 0; i < arrayTemp.length; i++) {
            if (arrayTemp[i] != attachmentId) {
                attachmentIds.push(arrayTemp[i]);
            }
        }

        attachmentIds = attachmentIds.join(',');

        $('#hidAttachmentId' + hiddenId).val(attachmentIds);
    }

    //删除相应的iframe
    $('#iframe' + frId).remove();
    $('#tableIframes' + frId).remove();
}