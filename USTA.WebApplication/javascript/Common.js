
function displaySubMenu(li) {
    var subMenu = li.getElementsByTagName("ul")[0];
    subMenu.style.display = "block";
}
function hideSubMenu(li) {
    var subMenu = li.getElementsByTagName("ul")[0];
    subMenu.style.display = "none";
}


//验证设置的时间必须要大于当前时间
function checkDateTime(setTimeString) {
    var nowTime = new Date().getDate();
    var checkTime = new Date(setTimeString);

    return checkTime > nowTime;
}

//检测kind编辑器的内容是否为空，参数值为kind编辑器的ID
function checkKindValue(editorId) {
    var editors = window.editor;
    for (var i = 0; i < editors.length; i++) {
        editors[i].sync(editorId);
    }
    if ($('#' + editorId).length > 0 && $.trim($('#' + editorId).val()).length == 0) {
        alert("请输入内容！");
        return false;
    }
    return true;
}

//检测TinyMCE编辑器的内容是否为空
function checkTinyMceValue(mceId) {
    if (tinyMCE.get(mceId).getContent() != null && tinyMCE.get(mceId).getContent().length == 0) {
        alert("请输入内容！");
        return false;
    }
}

//遍历文本控件改变背景颜色,　并且统一设置按钮的样式
function changeTextControlBgColor() {
    $(":text").css({ background: "#F4F5F6", border: "1px solid #0278B4" });
    $(":text").css("height", "20px");
    $(":password").css({ background: "#F4F5F6", border: "1px solid #0278B4" });
    $("textarea").css({ background: "#F4F5F6", border: "1px solid #0278B4" });
    $("select").css({ border: "1px solid #0278B4" });
    $(":submit").addClass("commonBtn");
    $(":button").addClass("commonBtn").css({ cursor: "pointer" });
}

//删除提示函数
function deleteTip() {
    return confirm("确认删除吗？一旦删除不可恢复，请确认！");
}

//Tips提示折叠函数
function tipsFold(infoId, id) {
    if ($('#' + id).css("display") == "none") {
        $('#' + id).css("display", "block");
        $('#' + infoId).text("[收起提示]");
    } else {
        $('#' + id).css("display", "none");
        $('#' + infoId).text("[查看提示]");
    }
}

//全选函数及反选函数
function selectAll() {
    if ($('#dzxBtnSelectAll').val() == "全选") {
        $('#dzxBtnSelectAll').val("取消全选");
        $(":checkbox").attr("checked", true);
    }
    else {
        $('#dzxBtnSelectAll').val("全选");
        $(":checkbox").attr("checked", false);
    }
}

//当全选框为空时，隐藏全选按钮和批量下载按钮
function hideSelectAllButton() {
    if ($(":checkbox").length == 0 && $('#dzxBtnSelectAll').length == 1) {
        $('#dzxBtnSelectAll').hide();
        $('#batchMultiFilesDownload').hide();
    }
}

//触发搜索按钮事件
function enterKey(ent, btnId) {
    ent = (ent) ? ent : (window.event) ? window.event : "";
    if (ent.keyCode == 13) {
        var btn = document.getElementById(btnId);
        if (btn != ent.srcElement) {
            btn.focus();
            btn.click();
        }
    }
}




//检测给定的数字是否在0--100的范围内的浮点数
function checkValueIsFloat(checkValue) {
    if (parseFloat(checkValue)) {
        alert("true");
    }
}

//跨域获取教学周信息
function getTimeStamp() {

    $.ajax({
        //传输方式
        type: 'get',
        //wcf地址，根据实际情况进行修改
        url: '/WebServices/WebServicesAboutGetTimeStamp.asmx/GetTimeStamp',
        //设置缓存
        cache: true,
        //超时设置
        timeout: 20000,
        //数据传输类型
        contentType: 'text/html',
        dataType: 'xml',


        success: function (msg) {
            $("#getTimeStamp").html("当前教学周信息：" + $(msg).find("string").text());
        },
        //异常处理函数
        error: function (msg) {
            $("#getTimeStamp").html("对不起，获取教学周信息失败，请直接访问<a href='" + $(msg).find("string").text() + "' target='_blank'>" + $(msg).find("string").text() + "</a>查看教学周信息");
        }
    });
}

//获取批量下载的URL并使用弹出窗口发送文件
function getBatchMultiFilesDownload() {
    //下载附件ID集合
    var attachmentIds = "";

    //获取当前所有选择的多选框
    $(":checkbox").each(function () {
        if ((typeof $(this) != "undefined") && $(this).attr("checked") == true) {
            attachmentIds += $(this).val() + ",";
        }
    });

    //判断是否选中至少一个以上多选框
    if (attachmentIds.length == 0) {
        alert('请至少选择一个文件进行批量下载！');
        return;
    }
    attachmentIds = attachmentIds.substring(0, attachmentIds.length - 1);

    $("#batchMultiFilesDownload").attr("href", '/Teacher/Ashx/BatchDownloadFiles.ashx?attachmentIds=' + attachmentIds);
}


window.onunload = function () { clearHiddenValue(); $(":checkbox").attr("checked", false); };
function clearHiddenValue() {
    if ($('#hidAttachmentId').length != 0) {
        $('#hidAttachmentId').val("");
    }

    if ($('#hidAttachmentId1').length != 0) {
        $('#hidAttachmentId1').val("");
    }
}

window.onerror = function () {
    return true;
}


$(document).ready(function () {
    changeTextControlBgColor();

    hideSelectAllButton();

    $(window).scroll(backTop);
});

function backTop() {
    var _top =$('#back-top');
    var _scrollTop = $(window).scrollTop();
    var _winWidth = $(window).width();
    if (_scrollTop > 0) {
        if (_winWidth > 1024) {
            _top.css({ left: $('.navigation').position().left+1024+'px', top: ($(window).height() + _scrollTop - 50)+'px' });
        } else {
            _top.css({ right: '10px', top: ($(window).height() + _scrollTop - 50)+'px' });
        }
        _top.show();
    } else {
        _top.hide();
    }
}

function initBeforeUnloadEvent(tips) {
    window.onbeforeunload = function () {
        return tips;
    };
}

function delBeforeUnloadEvent() {
    window.onbeforeunload = null;
};

//为iframe绑定onbeforeunload事件
function initIframeBeforeUnloadEvent(iframeId, tips) {
    document.getElementById(iframeId).onbeforeunload = function () {
        return tips;
    };
};

function delIframeBeforeUnloadEvent(iframeId) {
    document.getElementById(iframeId).onbeforeunload = null;
};



function checkIsUpload() {
    var isUpload = false;

    $('#archivesItemsUploadFiles').find("input:hidden").each(function () {
        if ($.trim($(this).val()).length > 0) {
            isUpload = true;
        }
    });
    return isUpload;
}