var addressBookController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    var gid = '';
    var gmailOld = '';
    function registerEvents() {
        $('.form_date').datetimepicker({
            language: 'vi',
            weekStart: 1,
            todayBtn: 1,
            autoclose: 1,
            todayHighlight: 1,
            startView: 2,
            minView: 2,
            forceParse: 0
        });

        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtFullName: { required: true },
                //selGender: { required: true },
                txtIDNumber: {
                    required: true,
                    number: true
                },
                txtMobilePhone: {
                    number: true
                },
                txtFax: {
                    number: true
                },
                txtTaxIDNumber: {
                    number: true
                },
                txtEmail: {
                    required: true
                },
                selType: {
                    required: true
                }
            }
        });
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $('#selType').on('change', function () {
            var i = $(this).val();
            if (i == "0")//khách hàng cá nhân
            {
                $('#lbFullName').html("Họ và tên<span class='star'>*</span>");
                $('#selGender').show();
                $('#lbGender').show();
                $('#dtDOB').show();
                $('#lbDOB').show();
                $('#lbIDNumber').html("Số CMND<span class='star'>*</span>");
            }
            else //Khách hàng công ty
            {
                $('#lbFullName').html("Tên công ty<span class='star'>*</span>");
                $('#selGender').hide();
                $('#lbGender').hide();
                $('#dtDOB').hide();
                $('#lbDOB').hide();
                $('#lbIDNumber').html("Mã số thuế<span class='star'>*</span>");
            }
        });



        $('#btnSearch').on('click', function () {
            loadData(true);
        });
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData(true);
            }
        });
        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
            //initTreeDropDownCategory();
            $('#modal-add-edit').modal('show');

        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            $('#selType').prop('disabled', true);
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/addressbook/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    if (response != null) {
                        var data = response;
                        gid = data.Id;
                        gmailOld = data.Email;

                        $('#txtFullName').val(data.FullName);
                        if (data.IsPersonal == true) {
                            $('#txtIDNumber').val(data.IdNumber);
                            $('#selGender').val(data.Gender);
                            general.setDateTimePicker($('#dtDOB'), data.Dob)
                          //  $('#dtDOB').val(general.dateFormatJson(data.Dob));
                            $('#selType').val("0");
                        }
                        else {
                            $('#txtIDNumber').val(data.TaxIdnumber);
                            $('#selType').val("1");
                        }

                        $('#txtMobilePhone').val(data.PhoneNumber);
                        $('#txtStreet').val(data.Street);
                        $('#txtWard').val(data.Ward);
                        $('#txtDistrict').val(data.District);
                        $('#txtCity').val(data.City);
                        $('#txtCountry').val(data.Country);
                        $('#txtFax').val(data.Fax);
                        $('#txtEmail').val(data.Email);
                        $('#txtNotes').val(data.Notes);
                        $('#txtLastUpdatedByFK').val(data.LastupdatedName);
                        $('#dtDateMotified').val(general.dateFormatJson(data.DateModified, true));

                        $('#modal-add-edit').modal('show');
                    }
                    general.stopLoading();
                },
                error: function (status) {
                    general.notify('Có lỗi xảy ra', 'error');
                    general.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var email = $('#txtEmail').val();
                // check mail truy cập trước
                if (gid == '' || gmailOld != email) {
                    // check mail đã tồn tại chưa !
                    $.ajax({
                        type: "GET",
                        url: "/addressbook/CheckExistEmail",
                        data: { email: email },
                        dataType: "json",
                        beforeSend: function () {
                            general.startLoading();
                        },
                        success: function (response) {
                            if (response) {
                                save(email);
                            }
                            else {
                                general.notify('Địa chỉ MAIL đã tồn tại, vui lòng kiểm tra lại !', 'error');
                            }
                        },
                        error: function () {
                            general.notify('Lỗi, Trong quá trình kiểm tra địa chỉ mail.', 'error');
                            general.stopLoading();
                        }
                    });
                }
                else {
                    save(email);
                }
               
            }

        });
    }

    function resetFormMaintainance() {
        $('#selType').prop('disabled', false);
        gid = '';
        gmailOld = '';
        $('#txtFullName').val('');

        $('#txtIDNumber').val('');

        $('#txtStreet').val('');
        $('#txtWard').val('');
        $('#txtDistrict').val('');
        $('#txtWard').val('');
        $('#txtCity').val('');
        $('#txtCountry').val('');
        $('#txtEmail').val('');
        $('#txtFax').val('');
        $('#txtMobilePhone').val('');
        $('#txtLastUpdatedByFK').val('');
        $('#txtNotes').val('');
        general.resetDateTimePicker($('#dtDOB'));
        $('#dtDateModified').val('');

    }
    function save(email) {

        var type = $('#selType option:selected').val();
        var fullName = $('#txtFullName').val();
        var gender = $('#selGender option:selected').val();

        var dob = general.getDateTimePicker($('#dtDOB'));
        var idNumber = $('#txtIDNumber').val();
        var mobilePhone = $('#txtMobilePhone').val();

        var fax = $('#txtFax').val();
        var Street = $('#txtStreet').val();
        var Ward = $('#txtWard').val();
        var District = $('#txtDistrict').val();
        var City = $('#txtCity').val();
        var Country = $('#txtCountry').val();
        var note = $('#txtNotes').val();

        var iscustomer;
        var isemployee;
        var isvendor;
        var data;
        if (gid == '') {
            iscustomer = false;
            isemployee = false;
            isvendor = false;
        }
        if (type == "0")//Cá nhân
            data = {
                Id: gid,
                FullName: fullName,
                Gender: gender,
                Dob: dob,
                IDNumber: idNumber,
                Street: Street,
                Ward: Ward,
                District: District,
                City: City,
                Country: Country,
                PhoneNumber: mobilePhone,
                Email: email,
                Fax: fax,
                Notes: note,
                isPersonal: true,
                isCustomer: iscustomer,
                isEmployee: isemployee,
                isVendor: isvendor
            }
        else
            data = {
                Id: gid,
                FullName: fullName,
                TaxIDNumber: idNumber,
                PhoneNumber: mobilePhone,
                Email: email,
                Fax: fax,
                Notes: note,
                Street: Street,
                Ward: Ward,
                District: District,
                City: City,
                Country: Country,
                isPersonal: false,
                isCustomer: iscustomer,
                isEmployee: isemployee,
                isVendor: isvendor
            }
        $.ajax({
            type: "POST",
            url: "/addressbook/SaveEntity",
            data: data,
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                if (response) {
                    loadData(true);
                    general.notify('Ghi thành công!', 'success');
                    resetFormMaintainance();
                  
                    $('#modal-add-edit').modal('hide');
                }
                else {
                    general.notify('Lỗi, khi ghi !', 'error');
                }
                general.stopLoading();
            },
            error: function () {
                general.notify('Có lỗi trong khi ghi danh bạ!', 'error');
                general.stopLoading();
            }
        });
    }
    function loadData(isPageChanged) {

        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txtKeyword').val(),
                page: general.configs.pageIndex,
                pageSize: general.configs.pageSize
            },
            url: '/addressbook/GetAllPaging',
            dataType: 'json',
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        stt: i + 1,
                        KeyId: item.Id,
                        FullName: item.FullName,
                        DOB: general.dateFormatJson(item.Dob, true),
                        Gender: general.getGender(item.Gender),
                        Email: item.Email,
                        MobilePhone: item.PhoneNumber,
                        IDNumber: item.IdNumber
                    });


                });
                $('#lblTotalRecords').text(response.RowCount);
                $('#tbl-content').html(render);
                wrapPaging(response.RowCount, function () {
                    loadData();
                }, isPageChanged);
                general.stopLoading();
            },
            error: function (status) {
                general.stopLoading();
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
}
