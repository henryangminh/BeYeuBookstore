var bookController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        loadBookCategory() 
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $('#selBookCategory').on('change', function () {
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
                url: '/Book/Delete',
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
        $('#txtTodate').on('change', function (e) {
            $('#txtNOdate').val(countDateWithoutSunday($('#txtFromdate').val(), $('#txtTodate').val()));
        });
        $('#txtFromdate').on('change', function (e) {
            $('#txtNOdate').val(countDateWithoutSunday($('#txtFromdate').val(), $('#txtTodate').val()));
        });
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
                var statusFK;
                var timeKeepingTypeFK = $('#seldetailTimeKeepingType option:selected').val();
                if (timeKeepingTypeFK == '0') {
                    general.notify('Vui lòng chọn loại nghỉ !', 'error');
                    return false;
                }
                var fromDate = $('#txtFromdate').val();
                if (fromDate == '') {
                    general.notify('Vui lòng điền ngày bắt đầu !', 'error');
                    return false;
                }
                var toDate = $('#txtTodate').val();
                if (toDate == '') {
                    general.notify('Vui lòng điền ngày kết thúc !', 'error');
                    return false;
                }
                if (moment(toDate, "DD/MM/YYYY") < moment(fromDate, "DD/MM/YYYY")) {
                    general.notify('Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu!', 'error');
                    return false;
                }


                var nODate = $('#txtNOdate').val();
                if (nODate > parseInt($('#txtNODaysRemain').val()) && timeKeepingTypeFK == '15') {
                    general.notify('Số ngày phép năm không đủ hoặc đã hết!', 'error');
                    return false;
                }
                var percentSalary = $('#txtPercentSalary').val();
                var reason = $('#txtReason').val();
                var note = $('#txtNote').val();
                var handOverTo = $('#selHandoverTo option:selected').val();
                var requestTo = $('#selRequestTo option:selected').val();
                //var permitDaysID = $('#txtPermitDaysId').val();
                var commitBackToWork;
                if ($('#chkCommitBackToWork').is(":checked")) {
                    commitBackToWork = true;
                }
                else {
                    commitBackToWork = false;
                }
                if ($('#txtStatusFK').val() == "") {
                    statusFK = general.formStatus.Typing
                }
                else {
                    statusFK = $('#txtStatusFK').val();
                }
                $.ajax({
                    type: 'POST',
                    url: '/AllowedVacation/SaveEntity',
                    data: {
                        KeyId: keyId,
                        TimeKeepingTypeFK: timeKeepingTypeFK,
                        FromDate: fromDate,
                        ToDate: toDate,
                        NODate: nODate,
                        PercentSalary: percentSalary,
                        Reason: reason,
                        Note: note,
                        StatusFK: statusFK,
                        HandoverToFK: handOverTo,
                        CommitBackToWork: commitBackToWork,
                        RequestToFK: requestTo,
                    },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoading();
                    },
                    success: function (response) {

                        $('#modal-add-edit').modal('hide');
                        general.notify('Ghi thành công!', 'success');
                        resetForm();
                        $('#frmMaintainance').trigger('reset');
                        general.stopLoading();
                        loadData();
                    },
                    error: function (err) {
                        general.notify('Có lỗi trong khi ghi !', 'error');
                        general.stopLoading();

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
        $('#selRequestTo')
            .find('option')
            .remove()
            .end()
            .append('<option value="">Chọn người yêu cầu xác nhận</option>')
            ;
        loadRequestTo();


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
                general.startLoading();
            },
            success: function (response) {
                console.log("loaddetail", response);
                var data = response;

                $('#txtId').val(data.KeyId);
                $('#txtEmployeeKeyId').val(data.EmployeeFK);
                $('#txtEmplyeeFullName').val(data.EmployeeFKNavigation.UserBy.FullName);
                $('#txtDepartment').val(data.EmployeeFKNavigation.DepartmentFkNavigation.DepartmentName);
                $('#seldetailTimeKeepingType').val(data.TimeKeepingTypeFK);
                $('#txtPosition').val(data.EmployeeFKNavigation.PositionFkNavigation.PositionName);
                var _FromDate = data.FromDate.split("T");
                $('#txtFromdate').val(_FromDate[0]);
                var _ToDate = data.ToDate.split("T");
                $('#txtTodate').val(_ToDate[0]);
                $('#txtNOdate').val(data.NODate);
                $('#txtPercentSalary').val(data.PercentSalary);
                $('#txtReason').val(data.Reason);
                $('#txtNote').val(data.Note);
                $('#dtDateCreated').val(moment(data.DateCreated).format("DD/MM/YYYY"));
                $('#dtDateModified').val(moment(data.DateModified).format("DD/MM/YYYY"));
                $('#selHandoverTo').val(data.HandoverToFK);
                //loadRequestToById(data.RequestToFK);
                //$('#txtRequestToId').val(data.RequestToFK);
                $('#txtStatusFK').val(data.StatusFK);
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
                        general.startLoading();
                    },
                    success: function (response) {
                        console.log("loadPermitDays", response);
                        var _data = response;
                        $('#txtNODaysRemain').val(_data.NODaysRemain);
                        $('#txtPermitDaysId').val(_data.KeyId);
                        general.stopLoading();

                    },
                    error: function (status) {
                        general.notify('Có lỗi xảy ra', 'error');
                        general.stopLoading();
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
                general.stopLoading();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
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
            bookcategoryid: $('#selBookCategory').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/Book/GetAllPaging',
        dataType: 'json',
        success: function (response) {
            console.log("data", response);
            var order = 1;
            $.each(response.Results, function (i, item) {
                
                render += Mustache.render(template, {
                    
                    KeyId: item.KeyId,
                    Merchant: item.MerchantFKNavigation.MerchantCompanyName,
                    BookTitle: item.BookTitle,
                    Author: item.Author,
                    BookType: item.BookCategoryFKNavigation.BookCategoryName,
                    UnitPrice: item.UnitPrice,
                    Qty: item.Quantity,
                    Description: item.Description,
                    
                });
                order++;

            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
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

function loadBookCategory() {
    $.ajax({
        type: 'GET',
        url: '/Book/GetAllBookCategory',

        dataType: "json",

        success: function (response) {

            $.each(response, function (i, item) {
                $('#selBookCategory').append("<option value='" + item.KeyId + "'>" + item.BookCategoryName + "</option>");


            });
        },
        error: function (err) {
            general.notify('Có lỗi trong khi load loại sách !', 'error');

        },
    });

}

