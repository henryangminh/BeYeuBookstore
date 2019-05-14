var webmasterController = function () {
    this.initialize = function () {
        loadData();
        loadWebmasterPosition();
        registerEvents();
    }
    function registerEvents() {
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $('#selWebmasterPosition').on('change', function () {
            loadData();
        });

        $('#selStatus').on('change', function () {
            loadData();
        });

        $('#dtBegin').on('change', function () {
            loadData();
        });

        $('#dtEnd').on('change', function () {
            loadData();
        });

        //$('#modal-add-edit').on('hide', function () {
        //    resetForm();
        //});

        $('#btnCreate').on('click', function () {
            resetForm();
            $('#frmMaintainance').trigger('reset');

        });

        $('#txtKeyword').on('keyup', function (e) {
            if (e.keyCode === 13) {
                loadData();
            }
        });

        $('#btnCancel').on('click', function () {

            $('#frmMaintainance').trigger('reset');
        });
        //Reset Form

        //Edit

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadDetail(that);

            formEdittitle.style.display = "block";
            formEditMastertitle.style.display = "none";
            $('#formEditMaster').addClass('hidden');
            $('#formEdit').removeClass('hidden');
            //document.getElementById("btnSave").style.display = "block";
        });


        //Delete
        $('body').on('click', '.btn-delete', function (e) {
            var that = $(this).data('id');
            $.ajax({
                type: 'POST',
                url: '/WebMaster/Delete',
                data: { id: that },
                dataType: 'json',
                beforeSend: function () {
                    general.startLoad();
                },
                success: function (response) {

                    general.notify('Xóa thành công!', 'success');
                    loadData();
                    general.stopLoad();

                },
                error: function (status) {
                    general.notify('Có lỗi trong khi xóa !', 'error');
                    general.stopLoad();
                },
            });

        });
        //Validate
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                selRequestTo:
                {
                    required: true
                },
                seldetailTimeKeepingType:
                {
                    required: true
                },
                txtFromdate: {
                    required: true
                },
                txtTodate: {
                    required: true
                },
                txtReason: {
                    required: true
                }
            }
        });
        //Save 

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var keyId;
                if ($('#txtId').val() == "") {
                    keyId = 0;
                }
                else {
                    keyId = parseInt($('#txtId').val());
                }
              
                $.ajax({
                    type: 'POST',
                    url: '/AllowedVacation/SaveEntity',
                    data: {
                        KeyId: keyId,
                       
                    },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (response) {

                        $('#modal-add-edit').modal('hide');
                        general.notify('Ghi thành công!', 'success');
                        resetForm();
                        $('#frmMaintainance').trigger('reset');
                        general.stopLoad();
                        loadData();
                    },
                    error: function (err) {
                        general.notify('Có lỗi trong khi ghi !', 'error');
                        general.stopLoad();

                    },
                });
                return false;
            }
        });




    }

    function resetForm() {
        $('#txtId').val('');
        $('#txtEmployeeKeyId').val('')
        $('#txtFromdate').val('');
        $('#txtTodate').val('');
        $('#txtNOdate').val('');
        $('#txtPercentSalary').val('');
        $('#txtReason').val('');
        $('#txtNote').val('');
        $('#dtDateModified').val('');
        $('#chkCommitBackToWork').prop('checked', false);
        $('#txtStatus').empty();



    }

    function loadDetail(that) {

        var _Year = new Date();
        var Year = _Year.getFullYear();
        $.ajax({
            type: "GET",
            url: "/AllowedVacation/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                console.log("loaddetail", response);
                var data = response;

                switch (data.StatusFK) {
                    case general.formStatus.Typing:
                        $('#btnRequest').removeClass('hidden');
                        $('#btnApprove').removeClass('hidden');
                        $('#btnDeny').removeClass('hidden');
                        break;
                    case general.formStatus.RequestConfirmation:
                        $('#btnRequest').addClass('hidden');
                        $('#btnApprove').removeClass('hidden');
                        $('#btnDeny').removeClass('hidden');
                        break;
                    case general.formStatus.Approved:
                    case general.formStatus.Denied:

                        $('#btnRequest').addClass('hidden');
                        $('#btnApprove').addClass('hidden');
                        $('#btnDeny').addClass('hidden');
                        break;
                }

                $('#chkCommitBackToWork').prop('checked', true);
                $.ajax({
                    type: "GET",
                    url: "/AllowedVacation/GetPermitDays",
                    data: { id: data.EmployeeFK, year: Year },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (response) {
                        console.log("loadPermitDays", response);
                        var _data = response;
                        $('#txtNODaysRemain').val(_data.NODaysRemain);
                        $('#txtPermitDaysId').val(_data.KeyId);
                        general.stopLoad();

                    },
                    error: function (status) {
                        general.notify('Có lỗi xảy ra', 'error');
                        general.stopLoad();
                    }
                });
                var _statusName = '';
                switch (data.StatusFK) {
                    case general.formStatus.Typing:
                        _statusName = 'Đang nhập';
                        break;
                    case general.formStatus.RequestConfirmation:
                        _statusName = 'Yêu cầu xác nhận';
                        break;
                    case general.formStatus.Approved:
                        _statusName = 'Đã xác nhận';
                        break;
                    case general.formStatus.Denied:
                        _statusName = 'Không xác nhận';
                        break;
                }
                $('#txtStatus').val(_statusName);
                $('#modal-add-edit').modal('show');
                general.stopLoad();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoad();
            }
        });


    }
}
function loadData(isPageChanged) {

    var template = $('#table-template').html();
    var render = "";

    $.ajax({
        type: 'GET',
        data: {
            fromdate: $('#dtBegin').val(),
            todate: $('#dtEnd').val(),
            keyword: $('#txtKeyword').val(),
            type: $('#selWebmasterPosition option:selected').val(),
            status: $('#selStatus option:selected').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/WebMaster/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log("dataWM", response);
            var order = 1;
            $.each(response.Results, function (i, item) {
                var _color = '';
                var _statusName = '';
                switch (item.UserBy.Status) {
                    case general.status.Active:
                        _color = 'green';
                        _statusName = 'Kích hoạt';
                        break;
                    case general.status.InActive:
                        _color = 'red'
                        _statusName = 'Khóa';
                        break;
                }
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                    Fullname: item.UserBy.FullName,
                    WebmasterType: item.WebMasterTypeFKNavigation.WebMasterTypeName,
                    UserName: item.UserBy.UserName,
                    Status: '<span class="badge bg-' + _color + '">' + _statusName + '</span>',
              

                });
                order++;

            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            general.stopLoad();
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            general.stopLoad();
        }
    });
}

function wrapPaging(recordCount, callBack, changePageSize) {
    var totalsize = Math.ceil(recordCount / general.configs.pageSize);
    //Unbind pagination if it existed or click change pagesize
    if ($('#paginationUL a').length === 0 || changePageSize === true) {
        $('#paginationUL').empty();
        $('#paginationUL').removeData("twbs-pagination");
        $('#paginationUL').unbind("page");
    }
    //Bind Pagination Event
    if (totalsize > 0)
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

function loadWebmasterPosition() {
    $.ajax({
        type: 'GET',
        url: '/WebMaster/GetAllWebMasterPosition',

        dataType: "json",
        beforeSend: function () {
            general.startLoad();
        },

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selWebmasterPosition').append("<option value='" + item.KeyId + "'>" + item.WebMasterTypeName + "</option>");
            });
            general.stopLoad();
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load loại sách !', 'error');
            general.stopLoad();

        },
    });

}

