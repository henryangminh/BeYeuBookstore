var UserController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    var gid = '';
    var gAvatarImage = '';
    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vn',
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPassword: {
                    equalTo: "#txtPassword"
                },
                txtEmail: {
                    required: true,
                    email: true
                }
            }
        });

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData(true);
            }
        });
        $("#btn-search").on('click', function () {
            loadData(true);
        });
        $("#ddl-show-page").on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            initRoleList();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/User/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoad();
                },
                success: function (response) {
                    var data = response;
                    gid= data.Id;
                    $('#txtFullName').val(data.FullName);
                    $('#txtUserName').val(data.UserName);
                    $('#txtEmail').val(data.Email);
                    $('#txtPhoneNumber').val(data.PhoneNumber);
                    $('#ckStatus').prop('checked', data.Status === 1);
                    gAvatarImage = data.Avatar;
                    $('#lblAvatar').text('');
                    initRoleList(data.Roles);

                    disableFieldEdit(true);
                    $('#modal-add-edit').modal('show');
                    general.stopLoad();

                },
                error: function () {
                    general.notify('Có lỗi xảy ra', 'error');
                    general.stopLoad();
                }
            });
        });




        $('#btnSelectAvatar').on('click', function () {
            $('#fileInputAvatar').click();
        });

        $("#fileInputAvatar").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            if (files.length == 1) {
                for (var i = 0; i < files.length; i++) {
                    data.append(files[i].name, files[i]);
                }
                $.ajax({
                    type: "POST",
                    url: "/Upload/UploadImage",
                    contentType: false,
                    processData: false,
                    data: data,
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (path) {
                        gAvatarImage = path;
                        $('#lblAvatar').text(files[0].name);
                        general.stopLoad();
                    },
                    error: function () {
                        general.notify('Lỗi khi upload ảnh !', 'error');
                        general.stopLoad();
                    }
                });
            }
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var fullName = $('#txtFullName').val();
                var userName = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                var email = $('#txtEmail').val();
                var phoneNumber = $('#txtPhoneNumber').val();
                var roles = [];
                $.each($('input[name="ckRoles"]'), function (i, item) {
                    if ($(item).prop('checked') === true)
                        roles.push($(item).prop('value'));
                });
                var status = $('#ckStatus').prop('checked') === true ? 1 : 0;

                $.ajax({
                    type: "POST",
                    url: "/User/SaveEntity",
                    data: {
                        Id: gid,
                        FullName: fullName,
                        UserName: userName,
                        Password: password,
                        Email: email,
                        PhoneNumber: phoneNumber,
                        Avatar: gAvatarImage,
                        Status: status,
                        Roles: roles
                    },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (res) {
                        if (res) {
                            general.notify('Lưu thành công', 'success');
                            $('#modal-add-edit').modal('hide');
                            resetFormMaintainance();
                            loadData(true);
                        }
                        else {
                            general.notify('Ghi lỗi', 'error');
                        }
                        general.stopLoad();
                    },
                    error: function () {
                        general.notify('Ghi lỗi', 'error');
                        general.stopLoad();
                    }
                });
            }
            return false;
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            general.confirm('Chắc chắn xóa ?', function () {
                $.ajax({
                    type: "POST",
                    url: "User/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function () {
                        general.notify('Delete successful', 'success');
                        general.stopLoad();
                        loadData();
                    },
                    error: function () {
                        general.notify('Has an error', 'error');
                        general.stopLoad();
                    }
                });
            });
        });

    };


    function disableFieldEdit(disabled) {
        $('#txtUserName').prop('disabled', disabled);
        $('#txtPassword').prop('disabled', disabled);
        $('#txtConfirmPassword').prop('disabled', disabled);

    }
    function resetFormMaintainance() {
        disableFieldEdit(false);
        gid = '';

        initRoleList();
        $('#txtFullName').val('');
        $('#txtUserName').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('input[name="ckRoles"]').removeAttr('checked');
        $('#txtEmail').val('');
        $('#txtPhoneNumber').val('');
        $('#txtAvatar').val('');
        $('#ckStatus').prop('checked', true);

    }

    function initRoleList(selectedRoles) {
        $.ajax({
            url: "Role/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                var template = $('#role-template').html();
                var data = response;
                var render = '';
                $.each(data, function (i, item) {
                    var checked = '';
                    if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1)
                        checked = 'checked';
                    render += Mustache.render(template,
                        {
                            Name: item.Name,
                            Description: item.Description,
                            Checked: checked
                        });
                });
                $('#list-roles').html(render);
            }
        });
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "User/GetAllPaging",
            data: {
                categoryId: $('#ddl-category-search').val(),
                keyword: $('#txt-search-keyword').val(),
                page: general.configs.pageIndex,
                pageSize: general.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                console.log("user", response);
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            FullName: item.FullName,
                            Id: item.Id,
                            Email: item.Email,
                            UserType: item.UserTypeName,
                            Avatar: item.Avatar === null ? '<img src="/images/user/user.png" width=25 />' : '<img src="' + item.Avatar + '" width=25 />',
                            DateModify: general.dateTimeFormatJson(item.DateModified),
                            Status: general.getStatus(item.Status)
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render !== undefined) {
                        $('#tbl-content').html(render);

                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);


                }
                else {
                    $('#tbl-content').html('');
                }
                general.stopLoad();
            },
            error: function (status) {
                console.log(status);
                general.stopLoad();
            }
        });
    };

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / general.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                general.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}