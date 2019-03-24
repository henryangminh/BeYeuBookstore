var feedbackController = function () {
    this.initialize = function () {
        registerEvents();
        registerControls();
    }
    function registerEvents() {
        $('#btnSave').on('click', function () {
            var data = {
                ErrorContent: CKEDITOR.instances.txtContent.getData(),
                FormName: $('#txtformName').val(),
                FullName: $('#user').text(),
                level: $('#selLevel option:selected').text(),
                type: $('#selType option:selected').text(),
            };
            $.ajax({
                type: 'POST',
                url: '/FeedBack/SaveEntity',
                data: data,
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (res) {
                    general.stopLoad();
                    general.notify('Gửi phản hồi thành công!', 'success');
                    reset();
                },
                error: function (err) {
                    general.stopLoad();
                    general.notify('Có lỗi trong khi gửi phản hồi');
                }
            });
        });
        $('#btnCanel').on('click', function () {
            reset();
        });
    }
    function registerControls() {
        CKEDITOR.replace('txtContent', {});
    }
    function reset() {
        $('txtformName').val('');
        CKEDITOR.instances.txtContent.setData('');
        $('#selLevel').val('');
        $('#selType').val('');
    }
}