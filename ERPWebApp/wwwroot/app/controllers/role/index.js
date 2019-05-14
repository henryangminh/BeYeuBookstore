var RoleController = function () {
    var self = this;
    var gid;
    this.initialize = function () {
        loadData();
        loadFunctionList();
        registerEvents();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtName: { required: true }
            }
        });

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which == 13) {
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
            $('#modal-add-edit').modal('show');

        });

        //Grant permission
        $('body').on('click', '.btn-grant', function () {
            gid = $(this).data('id');
            fillPermission(gid);
            $('#modal-grantpermission').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Role/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoad();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtName').val(data.Name);
                    $('#txtDescription').val(data.Description);
                    $('#modal-add-edit').modal('show');
                    general.stopLoad();

                },
                error: function (status) {
                    general.notify('Có lỗi xảy ra', 'error');
                    general.stopLoad();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var name = $('#txtName').val();
                var description = $('#txtDescription').val();

                $.ajax({
                    type: "POST",
                    url: "/Role/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                    },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (response) {
                        if (response) {
                            general.notify('Lưu thành công', 'success');
                            $('#modal-add-edit').modal('hide');
                            resetFormMaintainance();
                            loadData(true);
                            general.stopLoad();
                        }
                        else {
                            general.notify('Tên nhóm quyền đã tồn tại !', 'error');
                        }
                        general.stopLoad();
                    },
                    error: function () {
                        general.notify('Ghi lỗi, có thể do kết nối hoặc mô tả quá dài !', 'error');
                        general.stopLoad();
                    }
                });
                return false;
            }

        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            general.confirm('Bạn có chắc chắn xóa ?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Role/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (response) {
                        general.notify('Xóa thành công !', 'success');
                        general.stopLoad();
                        loadData();
                    },
                    error: function (status) {
                        general.notify('Lỗi xóa ! Vui lòng kiểm tra lại !', 'error');
                        general.stopLoad();
                    }
                });
            });
        });

        $("#btnSavePermission").off('click').on('click', function () {
            var listPermmission = [];
            $.each($('#tblFunction tbody tr'), function (i, item) {
                listPermmission.push({
                    RoleId:gid,
                    FunctionId: $(item).data('keyid'),
                    CanRead: $(item).find('.ckView').first().prop('checked'),
                    CanCreate: $(item).find('.ckAdd').first().prop('checked'),
                    CanUpdate: $(item).find('.ckEdit').first().prop('checked'),
                    CanDelete: $(item).find('.ckDelete').first().prop('checked'),
                    CanConfirm: $(item).find('.ckConfirm').first().prop('checked'),
                });
            });
            $.ajax({
                type: "POST",
                url: "/Role/SavePermission",
                data: {
                    listPermmission: listPermmission,
                    roleId:gid
                },
                beforeSend: function () {
                    general.startLoad();
                },
                success: function (response) {
                    general.notify('Lưu thành công !', 'success');
                    $('#modal-grantpermission').modal('hide');
                    general.stopLoad();
                },
                error: function () {
                    general.notify('Lưu bị lỗi', 'error');
                    general.stopLoad();
                }
            });
        });
    };

    function loadFunctionList(callback) {
        var strUrl = "/Function/GetAll";
        return $.ajax({
            type: "GET",
            url: strUrl,
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                var template = $('#result-data-function').html();
                var render = "";
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Name: item.Name,
                        treegridparent: item.ParentId != null ? "treegrid-parent-" + item.ParentId : "",
                        Id: item.KeyId,
                        AllowCreate: item.AllowCreate ? "checked" : "",
                        AllowEdit: item.AllowEdit ? "checked" : "",
                        AllowView: item.AllowView ? "checked" : "",
                        AllowDelete: item.AllowDelete ? "checked" : "",
                        AllowConfirm: item.AllowConfirm ? "checked" : "",
                        Status: general.getStatus(item.Status),
                    });
                });
                // gán vào html
                if (render != "") {
                    $('#lst-data-function').html(render);
                }

                // tạo các event
                $('.tree').treegrid();
                // ---chọn check all thì check hết con--------------
                $('#ckCheckAllView').on('click', function () {
                    $('.ckView').prop('checked', $(this).is(':checked'));
                });

                $('#ckCheckAllCreate').on('click', function () {
                    $('.ckAdd').prop('checked', $(this).is(':checked'));
                });
                $('#ckCheckAllEdit').on('click', function () {
                    $('.ckEdit').prop('checked', $(this).is(':checked'));
                });
                $('#ckCheckAllDelete').on('click', function () {
                    $('.ckDelete').prop('checked', $(this).is(':checked'));
                });
                $('#ckCheckAllConfirm').on('click', function () {
                    $('.ckConfirm').prop('checked', $(this).is(':checked'));
                });
                 // ---End check all thì check hết con--------------

                // --- check con thì check parent của nó luôn và ngược lại check cha thì check hết con cỏ nó---(khó)
                // --- check con thì bỏ or check for checkAll
                $('.ckView').on('click', function () {
                    var value = $(this).is(':checked');

                    // check con
                    var classParent = $(this).val();
                    checkChilren(classParent, value,0);

                    // check cha
                    ////kiểm tra this có chứa class dạng treegrid-parent-x không rồi lấy ra x
                    //var t = $(this).closest('tr').attr('class');
                    //var index = t.indexOf('treegrid-parent-');
                    //if (index >= 0) {
                    //    var j = index + 16;
                    //    var str = t.substring(j, t.length); //good
                    //    if (str != '') {
                    //        checkParent(str, value,0)
                    //    }
                    //}
                    //checkAll
                    if ($('.ckView:checked').length == response.length) {
                        $('#ckCheckAllView').prop('checked', true);
                    } else {
                        $('#ckCheckAllView').prop('checked', false);
                    }
                });
                $('.ckAdd').on('click', function () {
                    // check con
                    var classParent = $(this).val();
                    var value = $(this).is(':checked');
                    checkChilren(classParent, value, 1);

                    if ($('.ckAdd:checked').length == response.length) {
                        $('#ckCheckAllCreate').prop('checked', true);
                    } else {
                        $('#ckCheckAllCreate').prop('checked', false);
                    }
                });
                $('.ckEdit').on('click', function () {
                    // check con
                    var classParent = $(this).val();
                    var value = $(this).is(':checked');
                    checkChilren(classParent, value, 2);

                    if ($('.ckEdit:checked').length == response.length) {
                        $('#ckCheckAllEdit').prop('checked', true);
                    } else {
                        $('#ckCheckAllEdit').prop('checked', false);
                    }
                });
                $('.ckDelete').on('click', function () {
                    // check con
                    var classParent = $(this).val();
                    var value = $(this).is(':checked');
                    checkChilren(classParent, value, 3);

                    if ($('.ckDelete:checked').length == response.length) {
                        $('#ckCheckAllDelete').prop('checked', true);
                    } else {
                        $('#ckCheckAllDelete').prop('checked', false);
                    }
                });
                $('.ckConfirm').on('click', function () {
                    // check con
                    var classParent = $(this).val();
                    var value = $(this).is(':checked');
                    checkChilren(classParent, value, 4);

                    if ($('.ckConfirm:checked').length == response.length) {
                        $('#ckCheckAllConfirm').prop('checked', true);
                    } else {
                        $('#ckCheckAllConfirm').prop('checked', false);
                    }
                });

                  // ---End check con thì bỏ or check for checkAll


                if (callback != undefined) {
                    callback();
                }
                general.stopLoading();
            },
            error: function (status) {
                console.log(status);
                general.stopLoad();
            }
        });
    }

    function fillPermission(roleId) {
        var strUrl = "/Role/ListAllFunction";
        return $.ajax({
            type: "POST",
            url: strUrl,
            data: {
                roleId: roleId
            },
            dataType: "json",
            beforeSend: function () {
                general.stopLoad();
            },
            success: function (response) {
                if (response.length > 0)
                    loadPermission(response, function () {
                        // check all nếu là chọn hết con
                        if ($('.ckView:checked').length == $('#tblFunction tbody tr .ckView').length) {
                            $('#ckCheckAllView').prop('checked', true);
                        } else {
                            $('#ckCheckAllView').prop('checked', false);
                        }

                        if ($('.ckAdd:checked').length == $('#tblFunction tbody tr .ckAdd').length) {
                            $('#ckCheckAllCreate').prop('checked', true);
                        } else {
                            $('#ckCheckAllCreate').prop('checked', false);
                        }

                        if ($('.ckEdit:checked').length == $('#tblFunction tbody tr .ckEdit').length) {
                            $('#ckCheckAllEdit').prop('checked', true);
                        } else {
                            $('#ckCheckAllEdit').prop('checked', false);
                        }

                        if ($('.ckDelete:checked').length == $('#tblFunction tbody tr .ckDelete').length) {
                            $('#ckCheckAllDelete').prop('checked', true);
                        } else {
                            $('#ckCheckAllDelete').prop('checked', false);
                        }

                        if ($('.ckConfirm:checked').length == $('#tblFunction tbody tr .ckConfirm').length) {
                            $('#ckCheckAllConfirm').prop('checked', true);
                        } else {
                            $('#ckCheckAllConfirm').prop('checked', false);
                        }
                    });
                else
                    resetPermission();
                general.stopLoad();
            },
            error: function (status) {
                console.log(status);
                general.stopLoad();
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidId').val('');
        $('#txtName').val('');
        $('#txtDescription').val('');
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/Role/GetAllPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: general.configs.pageIndex,
                pageSize: general.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Name: item.Name,
                            Id: item.Id,
                            Description: item.Description
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render != undefined) {
                        $('#tbl-content').html(render);

                    }
                    general.stopLoad();
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);


                }
                else {
                    $('#tbl-content').html('');
                }
                general.stopLoading();
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
    function loadPermission(litsPermission, callback) {
        // duyệt row table
        $.each($('#tblFunction tbody tr'), function (i, item) {
            // duyệt list permission
            $.each(litsPermission, function (j, Permission) {
                if (Permission.FunctionId == $(item).data('keyid')) {
                    $(item).find('.ckView').first().prop('checked', Permission.CanRead);
                    $(item).find('.ckAdd').first().prop('checked', Permission.CanCreate);
                    $(item).find('.ckEdit').first().prop('checked', Permission.CanUpdate);
                    $(item).find('.ckDelete').first().prop('checked', Permission.CanDelete);
                    $(item).find('.ckConfirm').first().prop('checked', Permission.CanConfirm);
                }
            });
        });
        if (callback != undefined)
            callback();
    }
    function resetPermission() {
        // reset check All
        $('#ckCheckAllView').prop('checked', false);
        $('#ckCheckAllCreate').prop('checked', false);
        $('#ckCheckAllEdit').prop('checked', false);
        $('#ckCheckAllDelete').prop('checked', false);
        $('#ckCheckAllConfirm').prop('checked', false);

        // reset bảng
        $.each($('#tblFunction tbody tr'), function (i, item) {
            $(item).find('.ckView').first().prop('checked', false);
            $(item).find('.ckAdd').first().prop('checked', false);
            $(item).find('.ckEdit').first().prop('checked', false);
            $(item).find('.ckDelete').first().prop('checked', false);
            $(item).find('.ckConfirm').first().prop('checked', false);
        });
    }
    function checkChilren(id,Ischeck,option) {
        if (id != '') {
            var classParent = '.treegrid-parent-' + id;
            var listItem = $('#lst-data-function').find(classParent);
            if (listItem.length > 0) {
                for (var i = 0; i < listItem.length; i++) {
                    item = listItem[i];
                    switch (option) {
                        case 0:
                            $(item).find('.ckView').first().prop('checked', Ischeck);
                            break;
                        case 1:
                            $(item).find('.ckAdd').first().prop('checked', Ischeck);
                            break;
                        case 2:
                            $(item).find('.ckEdit').first().prop('checked', Ischeck);
                            break;
                        case 3:
                            $(item).find('.ckDelete').first().prop('checked', Ischeck);
                            break;
                        case 4:
                            $(item).find('.ckConfirm').first().prop('checked', Ischeck);
                            break;
                    }
                    var idParent = $(item).data('keyid');
                    checkChilren(idParent, Ischeck, option); // làm biếng
                }
            }
        }
    }

    // đang còn sai
    function checkParent(id, Ischeck, option) {
        if (id != '') {
            var idParent = '.treegrid-' + id;
            var Parent = $('#lst-data-function ' + idParent); // bản chất nó chỉ có 1 thằng thôi
            if (Parent.length > 0) {
                item = Parent.first();

                switch (option) {
                    case 0:
                        $(Parent).find('.ckView').first().prop('checked', Ischeck);
                        break;
                    case 1:
                        $(Parent).find('.ckAdd').first().prop('checked', Ischeck);
                        break;
                    case 2:
                        $(Parent).find('.ckEdit').first().prop('checked', Ischeck);
                        break;
                    case 3:
                        $(Parent).find('.ckDelete').first().prop('checked', Ischeck);
                        break;
                    case 4:
                        $(Parent).find('.ckConfirm').first().prop('checked', Ischeck);
                        break;
                }
                var t = Parent.attr('class');
                var index = t.indexOf('treegrid-parent-');
                if (index >= 0) {
                    var j = index + 16;
                    var str = t.substring(j, t.length); //good
                    if (str != '') {
                        checkParent(str, value, option)
                    }
                }
            }
        }
    }
}