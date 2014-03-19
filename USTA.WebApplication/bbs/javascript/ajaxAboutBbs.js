

//添加新主题
function addNewTopic(forumId,tag) {
    var btn = $("#btnAddTopic");

    var forumId = forumId;
    var topicTitle = $("#ctl00_ContentPlaceHolder1_txtTilte").val();
    //var topicContent = tinyMCE.get("ctl00_ContentPlaceHolder1_txtContent").getContent();
    var topicContent = FCKeditorAPI.GetInstance('ctl00_ContentPlaceHolder1_FCKeditor1').GetXHTML();
    if ($.trim(topicTitle).length == 0) {
        alert('请输入标题！');
        location.href = '#reply';
        return;
    }
    if ($.trim(topicContent).length == 0) {
        alert('请输入内容！');
        return;
    }
    var attachmentIds = $("#hidAttachmentId").val();
    
    btn.attr('disabled', true);
    btn.val('添加主题中...请稍候');
    $.ajax({
        //传输方式
        type: 'post',
        //wcf地址，根据实际情况进行修改
        url: '/wcf/WebServicesAboutBbs.svc/AddTopicByForumId',
        //超时设置
        timeout: 20000,
        //数据传输类型
        contentType: 'text/json',
        //data: '{"id":"' + id + '","ip":"' + ip + '","categoryId":"' + categoryId + '"}',
        data: '{"forumId":"' + escape(forumId) + '","topicTitle":"' + escape(topicTitle) + '","topicContent":"' + escape(topicContent) + '","attachmentIds":"' + escape(attachmentIds) + '"}',
        //投票成功处理函数
        success: function(msg) {
//            var a = eval('(' + msg + ')');
//            alert(a);
        alert('添加主题成功！');
        window.parent.location.href = 'BBSTopicList.aspx?forumId=' + forumId + "&tag=" + tag;
            btn.val('添加主题');
            btn.attr('disabled', false);
        },
        //投票异常处理函数
        error: function(msg) {
            alert("对不起，添加主题失败，服务器超时，请重新添加主题或使用“意见反馈”功能发送错误反馈，我们将会尽快解决，谢谢！");
            //重置按钮
            btn.attr('disabled', false);
            btn.val('重新添加主题');
        }
    });
}



//回复主题
function addNewPost(tag, topicId) {
    var btn = $("#btnAddPost");

    //    var topicContent = tinyMCE.get("ctl00_ContentPlaceHolder1_txtContent").getContent();
    var topicContent = FCKeditorAPI.GetInstance('ctl00_ContentPlaceHolder1_FCKeditor1').GetXHTML();
    //alert(topicContent);
    if ($.trim(topicContent).length == 0) {
        alert('请输入内容！');
        location.href = '#reply';
        return;
    }
    var attachmentIds = $("#hidAttachmentId").val();

    btn.attr('disabled', true);
    btn.val('添加回复中...请稍候');
    $.ajax({
        //传输方式
        type: 'post',
        //wcf地址，根据实际情况进行修改
        url: '/wcf/WebServicesAboutBbs.svc/AddPostByTopicId',
        //超时设置
        timeout: 20000,
        //数据传输类型
        contentType: 'text/json',
        //data: '{"id":"' + id + '","ip":"' + ip + '","categoryId":"' + categoryId + '"}',
        data: '{"topicId":"' + escape(topicId) + '","topicContent":"' + escape(topicContent) + '","attachmentIds":"' + escape(attachmentIds) + '"}',
        //投票成功处理函数
        success: function(msg) {
            //            var a = eval('(' + msg + ')');
            //            alert(a);
            alert('添加回复成功！感谢您的回复！');
            window.location.href = "BBSViewTopic.aspx?tag=" + tag + "&topicId=" + topicId;
            btn.val('添加回复');
            btn.attr('disabled', false);
        },
        //投票异常处理函数
        error: function(msg) {
        alert("对不起，添加回复失败，服务器超时，请重新添加回复或使用“意见反馈”功能发送错误反馈，我们将会尽快解决，谢谢！");
            //重置按钮
            btn.attr('disabled', false);
            btn.val('重新添加回复');
        }
    });
}