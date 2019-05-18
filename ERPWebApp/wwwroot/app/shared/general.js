var general = {
    configs: {
        pageSize: 10,
        pageIndex: 1
    },
    systemParameterGroup: {
        InfoCompany : 1,
        Salary : 2,
        Deduction : 3,
        ForDeduction : 4,
    },
    systemParaCode:{
        HolidaysInYear : "S0007",
        CompanyEntryTime : "S0008",
        CompanyTimeOut : "S0009",
        TimeStartLunch : "S0010",
        TimeEndLunch : "S0011",
        Offset : "S0012",
        CoefficientOfNight : "S0013",
        CoefficientsOvertimeHolidays : "S0027",
        CoefficientOfSundayShift : "S0014",
        CorporateSocialInsurance : "S0015",
        EmployeeSocialInsurance : "S0016",
        CorporateMedicalInsurance : "S0017",
        EmployeeMedicalInsurance : "S0018",
        CorparateUnemploymentInsurance : "S0019",
        EmployeeUnemploymentInsurance : "S0020",
        CorparateUnionFee : "S0021",
        EmployeeUnionFee : "S0022",
        TotalSalaryFund : "S0023",
    },
    addressBookType: {
        max_length: 5,
        Employee : "P",
        Vendor : "S",
        CustomerC : "C",
    },
    code: {
        Invoice: "IV",
        CashReceipt: "CR",
        SalesOrder:"SO"
    },
    productType: {
        Material: 1,
        Labor: 2,
        Equipment:3,
        FinishedProducts:4
    },
    bomId: {
        Header: 'TD',
        Note: 'NOTE',
        TTA: 'TTA',
        TTB: 'TTB',
        TTC:'TTC'
    },
    unit: {
        Percent: 5,
        Header: 79,
        Note:83
    },
    unitName: {
        Tam : "Tấm"
    },
    projectStatus: {
        create: 1,
        buildingBoq1: 2,
        buildedBoq1: 3,
        RequesteCreateBoq2: 4,
        buildingBoq2: 5,
        RequesteConfirmBoq2:6,
        buildedBoq2 : 7,
        building : 8,
        finish : 9,
        cancel : 10
    },
    poStatus: {
        Draft: 1,
        Confirmed: 2,
        Completed: 3,
        Invoiced: 4,
        Returned: 5,
        Cancelled:6
    },
    soStatus: { // boqstatus
        create: 1,
        RequestedConfirmBoq1: 2,
        NoConfirmBoq1: 3,
        ConfirmedBoq1: 4,
        CompletedBoq1: 5,
        FinishBOQ1:6,
        RequesteCreateBoq2: 7,
        buildingBoq2: 8,
        RequesteConfirmBoq2: 9,
        buildedBoq2: 10,
        Building: 11,
        Settlement: 12,
        Finish: 13,
        Cancelled: 14
    },
    soStatusWood: {
        create: 1,
        Confirmed: 2,
        UnConfirmed: 3
    },
    acceptanceStatus: {
        draft: 1,
        Confirmed: 2,
        Invoiced: 3,
        Returned: 4,
        Requested:5
    },
    accountDefault: {
        AccountForContract : "ADFC",
        TamUng : "TU",
        PhaiThuNoiBo : "NB",
        ThueGTGT : "GTGT",
        PhaiThuKhac : "TK"
    },
    status: {
        Active: 1,
        InActive:0
    },

    censorStatus: {
        Uncensored: 0,
        ContentCensored: 1,
        Unqualified: 2,
    },
    
    contractStatus: {
        Requesting: 0,
        Unqualified: 1,
        Success: 2,
        DepositePaid: 3,
        AccountingCensored: 4,
    },

    merchantScale: {
        Large:0,
        Medium:1,
        Small:2,
    },
    salaryTableStatus: {
        create: 1,
        requireconfirm: 2,
        confirm: 3
    },
    roleName: {
        BoardOfDirectors:"BoardOfDirectors"
    },

    deliStatus: {
        UnConfirm: 1,
        Confirm: 2,
        Packaged: 3,
        OnDelivery: 4,
        Success: 5,
        Fail: 6,
    }, 
    userType: {
        Merchant: 1,
        Advertiser: 2,
        Customer: 3,
        Webmaster: 4,
        
    },
    maxSizeAllowed: {
        BookImg: 2097152, //2Mb
        AdContentImg: 3145728, //3Mb

    },
    notify: function (message, type) {
        $.notify(message, {
            // whether to hide the notification on click
            clickToHide: true,
            // whether to auto-hide the notification
            autoHide: true,
            // if autoHide, hide after milliseconds
            autoHideDelay: 5000,
            // show the arrow pointing at the element
            arrowShow: true,
            // arrow size in pixels
            arrowSize: 5,
            // position defines the notification position though uses the defaults below
            position: 'bottom right',
            // default positions
            elementPosition: 'top right',
            globalPosition: 'top right',
            // default style
            style: 'bootstrap',
            // default class (string or [string])
            className: type,
            // show animation
            showAnimation: 'slideDown',
            // show animation duration
            showDuration: 400,
            // hide animation
            hideAnimation: 'slideUp',
            // hide animation duration
            hideDuration: 200,
            // padding between element and notification
            gap: 2
        });
    },
    confirm: function (message, okCallback) {
        bootbox.confirm({
            message: message,
            buttons: {
                confirm: {
                    label: 'Đồng ý',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Hủy',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result === true) {
                    okCallback();
                }
            }
        });
    },
    dateFormatJson: function (datetime,isDMY=true) {
        if (datetime == null || datetime == '')
            return '';
        var newdate = new Date(datetime);
        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();
        var hh = newdate.getHours();
        var mm = newdate.getMinutes();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        if (isDMY)
            return day + "/" + month + "/" + year;
        else
            return year + "-" + month + "-" + day;
    },
    dateTimeFormatJson: function (datetime) {
        if (datetime == null || datetime == '')
            return '';
        var newdate = new Date(datetime);
        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();
        var hh = newdate.getHours();
        var mm = newdate.getMinutes();
        var ss = newdate.getSeconds();
        if (month < 10)
            month = "0" + month;
        if (day < 10)
            day = "0" + day;
        if (hh < 10)
            hh = "0" + hh;
        if (mm < 10)
            mm = "0" + mm;
        if (ss < 10)
            ss = "0" + ss;
        return day + "/" + month + "/" + year + " " + hh + ":" + mm + ":" + ss;
    },
    getDatePicker: function (control) {
        return this.dateFormatJson(control.datepicker('getDate'), false);
    },
    setDatePicker: function (control, date) {
        if (date != null) {
            var temp = new Date(date);
            control.datepicker('setDate', temp);;
        }
        else {
            // set rổng ? có nên ko 
            control.datepicker('clearDates');
        }
    },
    resetDatePicker: function (control) {
        control.datepicker('clearDates');
    },
    addDays: function (date, days) {
        var result = new Date(date);
        result.setDate(result.getDate() + days);
        return result;
    },
    startLoading: function () {
        if ($('.dv-loading').length > 0)
            $('.dv-loading').removeClass('hide');
    },
    stopLoading: function () {
        if ($('.dv-loading').length > 0)
            $('.dv-loading')
                .addClass('hide');
    },
    startLoad: function () {
        $('#preloader').show();
        $('body').css({ 'opacity': 0.5 });
    },
    stopLoad: function () {
        $('#preloader').hide();
        $('body').css({ 'opacity': 1 });
    },
    getCheck: function (checked) {
        if (checked == true)
            return '<input type="checkbox"checked onclick="return false;">';
        else
            return '<input type="checkbox" onclick="return false;">';
    },
    getStatus: function (status) {
        if (status == 1)
            return '<span class="badge bg-green">Kích hoạt</span>';
        else
            return '<span class="badge bg-red">Khoá</span>';
    },
    getGender: function (gender) {
        if (gender == null || '')
            return '';
        else if (gender == 1)
            return 'Nữ';
        else
            return 'Nam';
    },
    formatNumber: function (number, precision) {
        if (!isFinite(number)) {
            return number.toString();
        }

        var a = number.toFixed(precision).split('.');
        a[0] = a[0].replace(/\d(?=(\d{3})+$)/g, '$&,');
        return a.join('.');
    },
    unflattern: function (arr) {
        var map = {};
        var roots = [];
        for (var i = 0; i < arr.length; i += 1) {
            var node = arr[i];
            node.children = [];
            map[node.id] = i; // use map to look-up the parents
            if (node.parentId !== null) {
                arr[map[node.parentId]].children.push(node);
            } else {
                roots.push(node);
            }
        }
        return roots;
    },
    getCode: function (leter, s, max) {
        var l = s.length;
        var i = 0;
        var t = leter;
        if (l < max) {
            while (i < (max - l)) {
                t = t + "0";
                i++;
            }
            t = t + s;
        }
        else
            t = t + s;
        return t;
    },
    commaSeparateNumber: function (val) {
        if (val != null && val !="") {
            while (/(\d+)(\d{3})/.test(val.toString())) {
                val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
            }
            return val;
        }
        return 0;
    },
    ignoreCommaNumber: function (val) {
        if (val != null && val !="") {
            var t = val.replace(/,/g, "");
            return parseInt(t);
        }
        return 0;
    },
    toFloat: function (val) {
        if (val != null && val != "" && val != undefined) {
            var t = val.toString().replace(/,/g, "");
            var rs = parseFloat(t);
          
            if (!isNaN(rs))
                return +rs.toFixed(10);
            else
                console.log('float: ' + val);
        }
        return 0;
    },
    toInt: function (val) {
        if (val != null && val != "" && val != undefined) {
            var t = val.toString().replace(/,/g, "");
            var rs = parseInt(t);
          
            if (!isNaN(rs))
                return rs;
            else
                console.log('int: '+val);
        }
        return 0;
    },
    toMoney: function (val) {
        if (val != null && val != "" && !isNaN(val)) {
            var mon = this.toRound(val, 0);
            while (/(\d+)(\d{3})/.test(mon.toString())) {
                mon = mon.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
            }
            return mon;
        }
        return 0;
    },
    //addComma: function (id) {
    //    $('body').on('keyup', id, function (event) {
    //        if (event.which >= 37 && event.which <= 40) return;

    //        $(this).val(function (index, value) {
    //            return value
    //                .replace(/\D/g, "")
    //                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
    //                ;
    //        });
    //    });
    //},
    toRound: function (val, r) {
        // làm tròn val, lấy sau đấu phẩu r chữ số
        return this.toFloat(val).toFixed(r);

    },
   
    // mot cach
    addFloat:function (a, b, precision) {
        var x = Math.pow(10, precision || 2); // ma dinh la 2
        return (Math.round(a * x) + Math.round(b * x)) / x;
    },
    checkFileType: function (fileExt, validExts) {
        var temp = fileExt.substring(fileExt.lastIndexOf('.'));
        if (validExts.indexOf(temp) < 0) {
            alert("Tệp được chọn không chính xác,vui lòng chọn lại các tệp có đuôi là: " +
                validExts.toString() + ".");
            return false;
        }
        else return true;
    }
}

$(document).ajaxSend(function (e, xhr, options) {
    if (options.type.toUpperCase() == "POST" || options.type.toUpperCase() == "PUT") {
        var token = $('form').find("input[name='__RequestVerificationToken']").val();
        xhr.setRequestHeader("RequestVerificationToken", token); // đẩy vào header của request để verify
    }
});
$('.money').mask('000,000,000,000,000', { reverse: true });