var allowedvacationController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        loadTimeKeepingType();
        loadAllowedVacationStatus();
        loadEmployee();
        var formEditMaster = document.getElementById("formEditMaster");
        var formEdit = document.getElementById("formEdit");
        var formEditMastertitle = document.getElementById("formEditMastertitle");
        var formEdittitle = document.getElementById("formEdittitle");
        var formStatus = document.getElementById("formStatus");

        $('#btnCreate').on('click', function () {
            resetForm();
            $('#frmMaintainance').trigger('reset');

            var _Year = new Date();
            var Year = _Year.getFullYear();
            $.ajax({
                type: 'GET',
                url: '/AllowedVacation/GetInfo',

                dataType: "json",

                success: function (response) {
                    console.log("loadInfo", response);
                    var data = response;
                    $('#modal-add-edit').modal('show');
                    //$('#').val(new LocalDate());
                    $("#dtDateCreated").val(moment().format('DD/MM/YYYY'));
                    $('#txtEmployeeKeyId').val(data.KeyId);
                    $('#txtEmplyeeFullName').val(data.UserBy.FullName);
                    $('#txtDepartment').val(data.DepartmentFkNavigation.DepartmentName);
                    $('#txtPosition').val(data.PositionFkNavigation.PositionName);
                    $.ajax({
                        type: "GET",
                        url: "/AllowedVacation/GetPermitDays",
                        data: { id: data.KeyId, year: Year },
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
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi load !', 'error');

                },
            });

            formEdittitle.style.display = "block";
            formEditMastertitle.style.display = "none";
            $('#formStatus').addClass('hidden');
            $('#formEditMaster').addClass('hidden');
            $('#formEdit').removeClass('hidden');
            
        });

        $('#btnSearch').on('click', function () {
            loadData(true);
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
            $('#formStatus').addClass('hidden');
            $('#formEditMaster').addClass('hidden');
            $('#formEdit').removeClass('hidden');
            //document.getElementById("btnSave").style.display = "block";
            


        });
        $('body').on('click', '.btn-editmaster', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadDetail(that);
            formEdittitle.style.display = "none";
            formEditMastertitle.style.display = "block";
            
            $('#formStatus').removeClass('hidden');
            $('#formEditMaster').removeClass('hidden');
            $('#formEdit').addClass('hidden');
            });

        $('body').on('click', '.btn-delete', function (e) {
            var that = $(this).data('id');
            $.ajax({
                type: 'POST',
                url: '/AllowedVacation/Delete',
                data: { id: that },
                dataType: 'json',
                success: function (response) {
                    general.notify('Xóa thành công!', 'success');
                    loadData();

                },
                error: function (status) {
                    general.notify('Có lỗi trong khi xóa !', 'error');

                },
            });

        });

        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                seldetailTimeKeepingType: {
                    required: true
                },
               
                txtFromdate: {
                    required: true
                },
                txtTodate: {
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
                if (moment(toDate).isAfter(fromDate) == false) {
                    general.notify('Ngày kết thúc phải lớn hơn ngày bắt đầu!', 'error');
                    return false;
                }
                var nODate = parseInt((Date.parse($('#txtTodate').val()) - Date.parse($('#txtFromdate').val())) / 1000 / 60 / 60 / 24);
                if (nODate > parseInt($('#txtNODaysRemain').val()) && timeKeepingTypeFK=='15') {
                    general.notify('Số ngày phép năm không đủ hoặc đã hết!', 'error');
                    return false;
                }
                var percentSalary = $('#txtPercentSalary').val();
                var reason = $('#txtReason').val();
                var note = $('#txtNote').val();
                var handOverTo = $('#selHandoverTo option:selected').val();
                //var permitDaysID = $('#txtPermitDaysId').val();
                var commitBackToWork;
                if ($('#chkCommitBackToWork').is(":checked")) {
                    commitBackToWork = true;
                }
                else {
                    commitBackToWork = false;
                }
                if ($('#txtStatusFK').val() == "") {
                    statusFK = "1"
                }
                else {
                    statusFK = $('#txtStatusFK').val();
                }
                $.ajax({
                    type: 'POST',
                    url: '/AllowedVacation/SaveEntity',
                    data: {
                        KeyId: keyId,
                        //EmployeeFK: employeeFK,
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
                        //permitId: permitDaysID,
                    },
                    dataType: "json",

                    success: function (response) {

                        $('#modal-add-edit').modal('hide');
                        general.notify('Ghi thành công!', 'success');
                        resetForm();
                        $('#frmMaintainance').trigger('reset');
                        loadData();
                    },
                    error: function (err) {
                        general.notify('Có lỗi trong khi ghi !', 'error');

                    },
                });
                return false;
            }
        });

        $('#btnRequest').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            $.ajax({
                type: 'POST',
                url: '/AllowedVacation/UpdateStatus',
                data: {
                    id: keyId,
                    status: "2"
                },
                dataType: "json",

                success: function (response) {

                    $('#modal-add-edit').modal('hide');
                    general.notify('Yêu cầu thành công!', 'success');
                    resetForm();
                    $('#frmMaintainance').trigger('reset');
                    loadData();
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi ghi !', 'error');

                },
            });
        });

        $('#seldetailTimeKeepingType').on('change', function (e) {
            e.preventDefault();
            var Id = parseInt($('#seldetailTimeKeepingType').val());
            $.ajax({
                type: 'GET',
                url: '/AllowedVacation/GetPaidRate',
                data: {
                    id: Id,
                    
                },
                dataType: "json",

                success: function (response) {
                    console.log("loadPaidRate", response)
                    var data = response;
                    $('#txtPercentSalary').val(data.GetPaidRate);
                    loadData();
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi load !', 'error');

                },
            });
        });

        $('#btnApprove').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            $.ajax({
                type: 'POST',
                url: '/AllowedVacation/UpdateStatus',
                data: {
                    id: keyId,
                    status: "3"
                },
                dataType: "json",

                success: function (response) {

                    $('#modal-add-edit').modal('hide');
                    general.notify('Xác nhận thành công!', 'success');
                    resetForm();
                    $('#frmMaintainance').trigger('reset');
                    loadData();
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi ghi !', 'error');

                },
            });
        });

        $('#btnDeny').on('click', function (e) {
            e.preventDefault();
            var keyId = parseInt($('#txtId').val());
            $.ajax({
                type: 'POST',
                url: '/AllowedVacation/UpdateStatus',
                data: {
                    id: keyId,
                    status: "4"
                },
                dataType: "json",

                success: function (response) {

                    $('#modal-add-edit').modal('hide');
                    general.notify('Không xác nhận thành công!', 'success');
                    resetForm();
                    $('#frmMaintainance').trigger('reset');
                    loadData();
                },
                error: function (err) {
                    general.notify('Có lỗi trong khi ghi !', 'error');

                },
            });
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
        resetForm();
        var btnRequest = document.getElementById("btnRequest");
        var btnApprove = document.getElementById("btnApprove");
        var btnDeny = document.getElementById("btnDeny");
        var btnSave = document.getElementById("btnSave");
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
                $('#modal-add-edit').modal('show');
                $('#txtId').val(data.KeyId);
                $('#txtEmployeeKeyId').val(data.EmployeeFK);
                //EmployeeId = parseInt(data.EmployeeFK);
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
                $('#txtStatusFK').val(data.StatusFK);
                switch (data.StatusFK) {
                    case 1:
                        $('#btnRequest').removeClass('hidden');
                        $('#btnApprove').removeClass('hidden');
                        $('#btnDeny').removeClass('hidden');
                        break;
                    case 2:
                        $('#btnRequest').addClass('hidden');
                        $('#btnApprove').removeClass('hidden');
                        $('#btnDeny').removeClass('hidden');
                        break;
                    case 3:
                    case 4:

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
                    case 1:
                        _statusName = 'Đang nhập';
                        break;
                    case 2:
                        _statusName = 'Yêu cầu xác nhận';
                        break;
                    case 3:
                        _statusName = 'Đã xác nhận';
                        break;
                    case 4:
                        _statusName = 'Không xác nhận';
                        break;
                }
                $('#txtStatus').val(_statusName);
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
        var linebreak = '<br>';
        var template = $('#table-template').html();
        var render = "";

        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txtKeyword').val(),
                page: general.configs.pageIndex,
                pageSize: general.configs.pageSize
            },
            url: '/AllowedVacation/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                console.log("data", response);

                $.each(response.Results, function (i, item) {
                    
                    var _color = '';
                    switch (item.StatusFK) {
                        case 1:
                            _color = 'black'
                            break;
                        case 2:
                            _color = 'blue'
                            break;
                        case 3:
                            _color = 'green'
                            break;
                        case 4:
                            _color = 'red'
                            break;
                    }
                    var prop = '';
                    if (item.StatusFK == 2 || item.StatusFK == 3 || item.StatusFK == 4)
                    {

                        prop = 'disabled';

                    }
                    render += Mustache.render(template, {
                        KeyId: item.KeyId,
                        FullName: item.EmployeeFKNavigation.UserBy.FullName,
                        Department: item.EmployeeFKNavigation.DepartmentFkNavigation.DepartmentName,
                        TimeKeepingType: item.TimekeepingTypeFKNavigation.TimekeepingTypeName,
                        NODate: item.NODate,
                        Reason: item.Reason,
                        FromDateToDate: 'Từ ngày: ' + moment(item.FromDate).format("DD/MM/YYYY") + '<br>Đến ngày: ' + moment(item.ToDate).format("DD/MM/YYYY"),
                        Status: '<span class="badge bg-' + _color + '">' + item.AllowedVacationStatusFKNavigation.HPStatusName + '</span>',
                        Prop: prop,
                    });
                    

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

    function loadTimeKeepingType() {
        $.ajax({
            type: 'GET',
            url: '/AllowedVacation/GetAllTimeKeepingType',

            dataType: "json",

            success: function (response) {

                $.each(response, function (i, item) {
                    $('#selTimeKeepingType').append("<option value='" + item.KeyId + "'>" + item.TimekeepingTypeName + "</option>");
                    $('#seldetailTimeKeepingType').append("<option value='" + item.KeyId + "'>" + item.TimekeepingTypeName + "</option>");


                });
            },
            error: function (err) {
                general.notify('Có lỗi trong khi load loại nghỉ !', 'error');

            },
        });

    }
    function loadAllowedVacationStatus() {
        $.ajax({
            type: 'GET',
            url: '/AllowedVacation/GetAllAllowedVacationStatus',

            dataType: "json",

            success: function (response) {

                $.each(response, function (i, item) {
                    $('#selAllowedVacationStatus').append("<option value='" + item.KeyId + "'>" + item.HPStatusName + "</option>");

                });
            },
            error: function (err) {
                general.notify('Có lỗi trong khi load trạng thái !', 'error');

            },
        });

    }
    function loadEmployee() {
        $.ajax({
            type: 'get',
            url: '/AllowedVacation/GetAllEmployee',

            datatype: "json",

            success: function (response) {

                console.log("HandOverEmployee:", response);
                $.each(response, function (i, item) {
                    $('#selHandoverTo').append("<option value='" + item.Id + "'>" + item.Name + "</option>");

                });
            },
            error: function (err) {
                general.notify('Có lỗi trong khi load trạng thái !', 'error');

            },
        });
    }

//function loadEmployee() {
//    $.ajax({
//        type: 'GET',
    
//        url: '/addressbook/GetListFullName',
//        dataType: 'json',
//        success: function (response) {
//            console.log(response);
//            if (response != null && response.length > 0)
//                callback(response);
//            else {
//                general.notify('Hết danh sách nhân viên cần tạo mới. Vui lòng thêm ở danh bạ trước!', 'error');
//                return false;
//            }
//            return true;

//        },
//        error: function (status) {
//            console.log(status);
//            general.notify('Không thể load dữ liệu', 'error');
//        }
//    });
