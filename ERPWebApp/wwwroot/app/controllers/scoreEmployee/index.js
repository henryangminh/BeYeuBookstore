var scoreEmployeeController = function () {
    this.initialize = function () {
        loadDepartment();
        loadEmployee();
        loadEmployeeType();
        loadTimeKeepingType(function (res) {
            loadData(false);
        });
        
        registerEvents();
    }
    var timeKeepingType = [];
    var gIdList = [];
    function registerEvents() {
        $('.datepicker').datepicker({
            format: "dd/mm/yyyy",
            todayBtn: "linked",
            clearBtn: true,
            language: "vi",
            todayHighlight: true
        });
        $('#btnSwitchNewMonth').on('click', function () {
            $.ajax({
                type: 'POST',
                url: '/ScoreEmployee/SwitchToNewMonth',
                success: function (res) {
                    var template = $('#table-template').html();
                    var render = '';
                    
                    res.forEach(function (item) {
                        render += Mustache.render(template, {
                            Id: item.EmployeeFK,
                            FullName: item.EmployeeFKNavigation.UserBy.FullName,
                            Date: general.dateFormatJson(item.Date, true),
                            DayOfWeek: GetDayInWeek(item.Date),
                            TimeKeepingType: getTimeKeepingTypeOptions(item.TimeKeepingTypeFK)
                        });
                    });
                    $('#tbl-content').html(render);
                },
                error: function () {
                    general.notify("Có lỗi khi chuyển sang tháng mới", "error");
                }
            });
        });

        $('body').on('focusout', '#txtHoursIn', function () {
            var hoursin = $(this).val();
            var hoursout = $(this).closest('tr').find('#txtHoursOut').val();
            var t = $(this).parent('#hoursin');
            if (hoursout != "") {
                GetHours(t,hoursin, hoursout);
            }
           
        });

        $('body').on('focusout', '#txtHoursOut', function () {
            var hoursout = $(this).val();
            var hoursin = $(this).closest('tr').find('#txtHoursIn').val();
            var t = $(this).parent('#hoursout');
            if (hoursin != "") {
                GetHours(t, hoursin, hoursout);
            }

        });

        $('#btnSave').on('click', function () {
            getValue(function (data) {
                $.ajax({
                    type: 'POST',
                    url: '/ScoreEmployee/UpdateScoreEmployee',
                    data: { list: data },
                    success: function () {
                        general.stopLoad();
                        general.notify('Ghi thành công!', 'success');
                        
                    },
                    error: function () {
                        general.stopLoad();
                        general.notify('Có lỗi khi ghi!', 'error');
                    }
                })
            });
            
        });

        $('#dtFromDate').on('change', function () {
            loadData(true);
        });
        $('#dtToDate').on('change', function () {
            loadData(true);
        });
        $('#selDepartmentFK').on('change', function () {
            loadData(true);
        });
        $('#selEmployeeTypeFK').on('change', function () {
            loadData(true);
        });
        $('#selEmployeeFK').on('change', function () {
            loadData(true);
        });
        $('#txtKeyword').on('keyup', function (e) {
            if (e.which == 13)
                loadData(true);
        });

        $('#btnSearch').on('click', function () {

        }); loadData(true);

        $('#btnImport').on('click', function () {
            $('#modal-import-excel').modal('show');
        });

        $('#btnImportExcel').on('click', function () {
            general.startLoad();
            //var file = $('#fileInputExcel')[0].files[0];
            //console.log(file);
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            $.ajax({
                url: 'ScoreEmployee/ImportExcel',
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    console.log(data);
                    if (data.length > 0) {
                        s
                        var s = '';
                        data.forEach(function (item, i) {
                            s += item + ", ";
                        })
                        alert("Có một vài tên bị lỗi. Vui lòng kiểm tra lại: " + s)
                    }
                    $('#modal-import-excel').modal('hide');
                    loadData(false);
                    stopLoad();
                }
            });
            return false;
        });
        $('#btnUndo').on('click', function () {
            loadData(false);
        });
        $('#btnAuto').on('click', function () {
            $('#tbl-content tr').each(function () {
                var dayOfWeek = $(this).find('#dayOfWeeks').text();
                if (dayOfWeek == "CN") {
                    $(this).find('#txtHoursIn').val("");
                    $(this).find('#txtHoursOut').val("");
                    $(this).find('#txtNOWorkHours').val(0);
                    $(this).find('#txtWork').val(0);
                    $(this).find('#txtOverTime').val(0);
                    $(this).find('#txtWorkOT').val(0);
                    $(this).find('.selTimeKeepingType').val(general.timeKeepingType.SundayOff);
                }
                else {
                    $(this).find('#txtHoursIn').val("08:00");
                    $(this).find('#txtHoursOut').val("17:30");
                    $(this).find('#txtNOWorkHours').val(8);
                    $(this).find('#txtWork').val(1);
                    $(this).find('#txtOverTime').val(0);
                    $(this).find('#txtWorkOT').val(0);
                    $(this).find('.selTimeKeepingType').val(general.timeKeepingType.WorkDay);
                }
            });
        });

        $('#btnSalary').on('click', function () {
            $.ajax({
                type: "POST",
                url: "ScoreEmployee/CalculatingSalary",
                data: {
                    FromDate: general.dateFormatJson($('#dtFromDate').datepicker('getDate'), false),
                    ToDate: general.dateFormatJson($('#dtToDate').datepicker('getDate'), false),
                    Employee: general.toInt($('#selEmployeeFK').find('option:selected').val()),
                    KeyWord: $('#txtKeyword').val()
                },
                beforeSend: function () {
                    general.startLoad();
                },
                success: function (response) {
                    window.location.href = response;
                    general.stopLoad();
                },
                error: function () {
                    general.notify('Quá trình bị lỗi', 'error');
                    general.stopLoad();
                }
            });
        });
    }
    function loadData(filter) {
        $.ajax({
            type: 'GET',
            url: '/ScoreEmployee/GetListScoreEmployee',
            data: {
                Filter: filter,
                FromDate: general.dateFormatJson($('#dtFromDate').datepicker('getDate'), false),
                ToDate: general.dateFormatJson($('#dtToDate').datepicker('getDate'),false),
                Department: general.toInt($('#selDepartmentFK').find('option:selected').val()),
                Type: general.toInt($('#selEmployeeTypeFK').find('option:selected').val()),
                Employee: general.toInt($('#selEmployeeFK').find('option:selected').val()),
                KeyWord: $('#txtKeyword').val()
            },
            success: function (res) {
                console.log(res);
                var template = $('#table-template').html();
                var render = '';  
                res.forEach(function (item) {
                    render += Mustache.render(template, {
                        KeyId: item.KeyId,
                        Id: item.EmployeeFK,
                        FullName: item.EmployeeFKNavigation.UserBy.FullName,
                        Date: general.dateFormatJson(item.Date, true),
                        DayOfWeek: GetDayInWeek(item.Date),
                        HoursIn: item.HoursIn,
                        HoursOut: item.HoursOut,
                        NOWorkHours: item.NOWorkHours,
                        Work: item.Work,
                        Overtime: item.OverTime,
                        WorkOT: item.WorkOT,
                        TimeKeepingType: getTimeKeepingTypeOptions(item.TimeKeepingTypeFK),
                        Note: item.Note
                    });
                });
                $('#tbl-content').html(render);
            },
            error: function () {
                general.notify("Có lỗi khi load dữ liệu", "error");
            }
        });
    }
    function getValue(callback) {
        general.startLoad();
        var scoreEmployeeArr = [];
        var i = 0;
        $('#tbl-content tr').each(function () {
            var keyId = $(this).data('keyid');
            var hoursin = $(this).find('#txtHoursIn').val();
            var hoursout = $(this).find('#txtHoursOut').val();
            var noWorkHours = $(this).find('#txtNOWorkHours').val();
            var work = $(this).find('#txtWork').val();
            var overtime = $(this).find('#txtOverTime').val();
            var workOT = $(this).find('#txtWorkOT').val();
            var timeKeepingTypeFK = $(this).find('.selTimeKeepingType').find('option:selected').val();
            var note = $(this).find('#txtNote').val();
            var obj = {
                KeyId: keyId,
                HoursIn: hoursin,
                HoursOut: hoursout,
                NOWorkHours: noWorkHours,
                Work: work,
                OverTime: overtime,
                WorkOT: workOT,
                TimeKeepingTypeFK: timeKeepingTypeFK,
                Note: note,
            };
            scoreEmployeeArr.push(obj);
            i++;
        });
        if (i == $('#tbl-content tr').length)
            callback(scoreEmployeeArr);
    }
    function getTimeKeepingTypeOptions(selectedId) {
        var attributes = "<select class='border-none selTimeKeepingType' style='max-width:150px;padding:0px!important'>";
        attributes += '<option value="" selected="select">' + "" + '</option>';
        $.each(timeKeepingType, function (i, item) {

            if (selectedId === item.KeyId)
                attributes += '<option value="' + item.KeyId + '" selected="select">' + item.TimekeepingTypeName + '</option>';
            else
                attributes += '<option value="' + item.KeyId + '">' + item.TimekeepingTypeName + '</option>';
        });
        attributes += "</select>";
        return attributes;
    }
    function loadTimeKeepingType(callback) {
        $.ajax({
            type: 'GET',
            url: '/TimeKeepingType/GetAll',
            success: function (res) {
                timeKeepingType = res;
                callback(res);
            },
            error: function () {

            }
        });
    }
    function GetDayInWeek(_date) {
        var date = new Date(_date);
        var t = date.getDay();
        var rs = '';
        switch (t) {
            case 0: rs = 'CN'; break;
            case 1: rs = 'Hai'; break;
            case 2: rs = 'Ba'; break;
            case 3: rs = 'Tư'; break;
            case 4: rs = 'Năm'; break;
            case 5: rs = 'Sáu'; break;
            case 6: rs = 'Bảy'; break;
        }
        return rs;
    }
    function GetHours(temp,hoursin, hoursout) {
        var s = hoursin.split(":");
        var e = hoursout.split(":");
        var min = e[1] - s[1];
        var hour_carry = 0;
        if (min < 0) {
            min += 60;
            hour_carry += 1;
        }
        hour = e[0] - s[0] - hour_carry;
        min = (min / 60);
        var a = hour + min;
        var hours = a - 1.5;
        temp.siblings('#noWorkHours').children('#txtNOWorkHours').val(hours);
        temp.siblings('#work').children('#txtWork').val(hours / 8);
        var ot = hours - 8;
        if (ot > 0) {
            temp.siblings('#overtime').children('#txtOverTime').val(ot);
            temp.siblings('#workOT').children('#txtWorkOT').val(ot / 8);
        }
        else {
            temp.siblings('#overtime').children('#txtOverTime').val(0);
            temp.siblings('#workOT').children('#txtWorkOT').val(0);
        }
    }
    function loadDepartment() {
        $('#selDepartmentFK option').remove();
        $.ajax({
            type: 'GET',
            url: '/employee/GetAllDepartment',
            success: function (response) {
                console.log(response);
                $('#selDepartmentFK').append("<option value=''>Chọn phòng ban</option>");
                $.each(response, function (i, item) {
                    $('#selDepartmentFK').append("<option value='" + item.KeyId + "'>" + item.DepartmentName + "</option>");
                });
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    function loadEmployee() {
        $('#selEmployeeFK option').remove();
        $.ajax({
            type: 'GET',
            url: '/Employee/GetListEmployee',
            success: function (response) {
                console.log(response);
                $('#selEmployeeFK').append("<option value=''>Chọn nhân viên</option>");
                $.each(response, function (i, item) {
                    $('#selEmployeeFK').append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                });
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }

    function loadEmployeeType() {
        $('#selEmployeeTypeFK option').remove();
        $.ajax({
            type: 'GET',
            url: '/EmployeeType/GetAll',
            success: function (response) {
                console.log(response);
                $('#selEmployeeTypeFK').append("<option value=''>Chọn loại nhân viên</option>");
                $.each(response, function (i, item) {
                    $('#selEmployeeTypeFK').append("<option value='" + item.KeyId + "'>" + item.Name + "</option>");
                });
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
}