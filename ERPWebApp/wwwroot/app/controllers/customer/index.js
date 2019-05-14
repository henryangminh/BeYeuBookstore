var customerController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });


        $('#dtBegin').on('change', function () {
            loadData();
        });

        $('#dtEnd').on('change', function () {
            loadData();
        });


        $('#selStatus').on('change', function () {
            loadData();
        });


        $('#btnCreate').on('click', function () {
            resetForm();
            $('#modal-add-edit').modal('show');

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

        });


        //Delete
        $('body').on('click', '.btn-delete', function (e) {
            if (confirm("Bạn chắc chắn muốn xóa?")) {
                var that = $(this).data('id');
                $.ajax({
                    type: 'POST',
                    url: '/Customer/Delete',
                    data: { id: that },
                    dataType: 'json',
                    beforeSend: function () {
                        general.startLoading();
                    },
                    success: function (response) {

                        general.notify('Xóa thành công!', 'success');
                        loadData();
                        general.stopLoading();

                    },
                    error: function (status) {
                        general.notify('Có lỗi trong khi xóa !', 'error');
                        general.stopLoading();
                    },
                });
            }
            else {
                alert("Bạn vừa bấm hủy");
            }

        });
        //Validate
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtFullName:
                {
                    required: true
                },
            
                seldetailStatus: {
                    required: true
                },
                txtPhoneNumber: {
                    required: true,
                    number: true,
                },
                txtAddress: {
                    required: true
                }
            }
        });
        //Save 

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var keyId = $('#txtGuidId').val();
                var fullName = $('#txtFullName').val();
                var phoneNumber = $('#txtPhoneNumber').val();
                var address = $('#txtAddress').val();
                var gender = $('#selGender option:selected').val();
                var status = $('#seldetailStatus option:selected').val();

                $.ajax({
                    type: 'POST',
                    url: '/Customer/SaveEntity',
                    data: {
                        Id: keyId,
                        FullName: fullName,
                        PhoneNumber: phoneNumber,
                        Address: address,
                        Gender: gender,
                        Status: status,
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
        $('#txtName').val('')


    }

    function loadDetail(that) {

        $.ajax({
            type: "GET",
            url: "/Customer/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                console.log("loaddetail", response);
                var data = response;

                $('#txtId').val(data.KeyId);
                $('#txtGuidId').val(data.UserBy.Id);
                $('#txtUserName').val(data.UserBy.UserName);
                $('#dtDateCreated').val(moment(data.DateCreated).format("DD/MM/YYYY"));
                $('#dtDateModified').val(moment(data.DateModified).format("DD/MM/YYYY"));
                $('#txtFullName').val(data.UserBy.FullName);
                $('#selGender').val(data.UserBy.Gender);
                $('#seldetailStatus').val(data.UserBy.Status);
                $('#txtPhoneNumber').val(data.UserBy.PhoneNumber);
                $('#txtAddress').val(data.UserBy.Address);
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
            status: $('#selStatus option:selected').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/Customer/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log("data", response);
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
                    FullName: item.UserBy.FullName,
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




