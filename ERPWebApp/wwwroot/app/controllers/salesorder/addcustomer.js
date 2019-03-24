var addressBookController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $("#txtKeyId").prop('disabled', true);
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $('#frmCustomer').validate({
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
        
        $('#selType').on('change', function () {
            var i = $(this).val();
            if (i == "0")//khách hàng cá nhân
            {
                $('#lbFullName').text("Họ và tên *");
                $('#selGender').show();
                $('#lbGender').show();
                $('#dtDOB').show();
                $('#lbDOB').show();
                $('#lbIDNumber').text("Số CMNN");
            }
            else //Khách hàng công ty
            {
                $('#lbFullName').text("Tên công ty *");
                $('#selGender').hide();
                $('#lbGender').hide();
                $('#dtDOB').hide();
                $('#lbDOB').hide();
                $('#lbIDNumber').text("Mã số thuế");
            }
        });


        $('#btnSaveCustomer').on('click', function (e) {
            if ($('#frmCustomer').valid()) {
                e.preventDefault();
                var type = $('#selType option:selected').val();
                var keyId = $('#txtId').val();
                var fullName = $('#txtFullName').val();
                var gender = $('#selGender option:selected').val();

                var dob = $('#dtDOB').val();
                var idNumber = $('#txtIDNumber').val();

                var address = $('#txtAddress').val();
                var mobilePhone = $('#txtMobilePhone').val();
                var email = $('#txtEmail').val();
                var fax = $('#txtFax').val();
                var note = $('#txtNotes').val();
                //var lastUpdatedBy = $('#user').data('userid');
                //var lastUpdatedDate = $('#dtDateModified').val();
                var iscustomer;
                var isemployee;
                var isvendor;
                var data;
                if (keyId == "0") {
                    iscustomer = true;
                    isemployee = false;
                    isvendor = false;
                }
                if (type == "0")//Cá nhân
                    data = {
                        KeyId: keyId,
                        FullName: fullName,
                        Gender: gender,
                        DOB: dob,
                        IDNumber: idNumber,
                        Address: address,
                        MobilePhone: mobilePhone,
                        Email: email,
                        Fax: fax,
                        Notes: note,
                        //LastUpdatedByFK: lastUpdatedBy,
                        //DateModified: lastUpdatedDate,
                        isPersonal: true,
                        isCustomer: iscustomer,
                        isEmployee: isemployee,
                        isVendor: isvendor
                    }
                else
                    data = {
                        KeyId: keyId,
                        FullName: fullName,
                        TaxIDNumber: idNumber,
                        Address: address,
                        MobilePhone: mobilePhone,
                        Email: email,
                        Fax: fax,
                        Notes: note,
                        //LastUpdatedByFK: lastUpdatedBy,
                        //DateModified: lastUpdatedDate,
                        isPersonal: false,
                        isCustomer: iscustomer,
                        isEmployee: isemployee,
                        isVendor: isvendor
                    }
                Save(data, function (res) {
                    $('#txtCustomerName').val(res.FullName);
                    var a = res.KeyId;
                    var id = general.getCode(general.addressBookType.CustomerC, a.toString(), general.addressBookType.max_length);
                    var customer = {
                        KeyId: 0,
                        Id: id,
                        AddressBook_FK: a,
                        //CreditLimit: creditlimit,
                        //LastRevisedCreditLimitDate: lastRevisedCreditLimitDate,
                        //CustomerType: customerType,
                        //EvaluatedCustomer: evaluatedCustomer,
                        //Notes: note,
                        LastUpdatedByFk: lastUpdatedBy,
                        Status: 1
                    };
                    SaveCustomer(customer, function (response) {
                        $('#txtCustomerName').data('keyidcus', response.KeyId);
                    });
                });
                return false;
            }

        });
    }

    function Save(data, callback) {
        $.ajax({
            type: "POST",
            url: "/addressbook/SaveEntity",
            data: data,
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                callback(response);
                general.notify('Ghi thành công!', 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();
                general.stopLoading();
            },
            error: function () {
                general.notify('Có lỗi trong khi ghi danh bạ!', 'error');
                general.stopLoading();
            }
        });
    }
    function SaveCustomer(data,callback) {
        $.ajax({
            type: "POST",
            url: "/customer/SaveEntity",
            data: data,
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                callback(response);
            },
            error: function () {
                general.notify('Có lỗi trong khi ghi danh bạ!', 'error');
                general.stopLoading();
            }
        });
    }
    function resetFormMaintainance() {
        $('#selType').prop('disabled', false);
        $('#txtId').val(0);
        $('#txtFullName').val('');

        $('#txtIDNumber').val('');

        $('#txtAddress').val('');
        $('#txtEmail').val('');
        $('#txtFax').val('');

        $('#txtMobilePhone').val('');
        $('#txtLastUpdatedByFK').val('');
        $('#txtNotes').val('');
        $('#dtDOB').val('');
        $('#dtDateModified').val('');



    }



}