
function initKindEditor(K, id, width, height) {
    window.editor = [];
    if ($('#' + id).length == 1) {
        window.editor.push(K.create('#' + id, {
            width: width, //编辑器的宽度
            height: height, //编辑器的高度
            uploadJson: '/Ashx/upload_json.ashx',
            allowFlashUpload: false,
            allowMediaUpload: false,
            allowFileManager: false
        }));
    }
}