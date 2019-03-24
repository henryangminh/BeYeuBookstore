var employeeController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    var gId = 0;
    var userName;
    function registerEvents() {
        loadNationality();
        loadCity();
        loadDepartment();
        loadPosition();
        loadNation();
        loadReligion();
        $("#txtId").prop('disabled', true);
        $("#txtGender").prop('disabled', true);
        $("#txtDOB").prop('disabled', true);
        $("#txtIDNumber").prop('disabled', true);
        $("#txtPhoneNumber").prop('disabled', true);
        $("#txtEmail").prop('disabled', true);
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $("#txtEmployee").prop('disabled', true);
        $("#chkStatus").prop('checked', true);
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {

                txtEmployee: {
                    required: true
                },
                selDepartmentFK: {
                    required: true
                },
                selPositionFK: {
                    required: true
                },
                selOriginFK: {
                    required: true
                },
                txtTaxIDNumber: {
                    number: true
                },
                txtBankAccountNumber: {
                    number: true
                }
            }
        });
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
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
            gId = 0;
            resetFormMaintainance();
            loadSelEmployee(false, general.addressBookType.Employee, function (arr) {
                autoAddressbook(document.getElementById("txtEmployee"), arr);
                $('#modal-add-edit').modal('show');
            });


        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            $('#txtEmployee').prop('disabled', true);
            var that = $(this).data('id');
            loadDetail(that);
        });
        var status;
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var addressBookFk = document.getElementById('txtEmployee').getAttribute('data-addressbookid');
                if (addressBookFk == '') {
                    general.notify('Vui lòng chọn nhân viên trước !', 'error');
                    return false;
                }
                var id = $('#txtId').val();
                var Idcard = $('#txtIdCard').val();
                var birthplace = $('#selBirthplace option:selected').val();
                var originFK = $('#selOriginFK option:selected').val();
                var nationFK = $('#selNationFK option:selected').val();
                var nationalityFK = $('#selNationality option:selected').val();
                var permanentResidence = $('#txtPermanentResidence').val();
                var districtPR = $('#selDistrictPR option:selected').val();
                var provincePR = $('#selProvincePR option:selected').val();
                var accommodationCurrent = $('#txtAccommodationCurrent').val();
                var districtAC = $('#selDistrictAC option:selected').val();
                var provinceAC = $('#selProvinceAC option:selected').val();
                var aTMAccountName = $('#txtATMAccountName').val();
                var iDAccount = $('#txtIDAccount').val();
                var nOChildren = $('#txtNOChildren').val();
                var numberOfProfile = $('#txtNumberOfProfile').val();
                var numberOfContract = $('#txtNumberOfContract').val();
                var laborContractType = $('#selLaborContractType').val();
                //var signDate = $('#dtSignDate').val();
                //var timeExpireContract = $('#dtTimeExpireContract').val();
                //var timeExpireProbation = $('#dtTimeExpireProbation').val();
                //var startDate = $('#dtStartDate').val();
                var infoSaveFile = $('#txtInfoSaveFile').val();
                //var layOffDate = $('#dtLayOffDate').val();
                var literacy = $('#selLiteracy option:selected').val();
                var iDSocialInsurance = $('#txtIDSocialInsurance').val();
                var salarySocialInsurance = $('#txtSalarySocialInsurance').val();
                //var socialInsuranceDate = $('#txtSocialInsuranceDate').val();
                var salary = $('#txtSalary').val();


                var religionFK = $('#selReligionFK option:selected').val();
                var taxIdNumber = $('#txtTaxIDNumber').val();
                var bankAccountNumber = $('#txtBankAccountNumber').val();
                var departmentFK = $('#selDepartmentFK option:selected').val();
                var positionFK = $('#selPositionFK option:selected').val();
                var note = $('#txtNotes').val();



                var a = $("#chkStatus").is(':checked');
                if (a == true) status = 1;
                else status = 0;

                var FamilyC = $("input[name='optradio']:checked").val();

                var data = {
                    AccommodationCurrent: accommodationCurrent,
                    KeyId: gId,
                    Id: id,
                    IDAccount: iDAccount,
                    IdCard: Idcard,
                    InfoSaveFile: infoSaveFile,
                    IDSocialInsurance: iDSocialInsurance,
                    User_FK: addressBookFk,
                    BankName: aTMAccountName,
                    Birthplace: birthplace,
                    DistrictAC: districtAC,
                    DistrictPR: districtPR,
                    FamilyCircumstances: FamilyC,
                    OriginFk: originFK,
                    PermanentResidence: permanentResidence,
                    LaborContractType: laborContractType,
                    LiteracyFk: literacy,
                    NationFk: nationFK,
                    NationalityFk: nationalityFK,
                    NOChildren: nOChildren,
                    NumberOfContract: numberOfContract,
                    NumberOfProfile: numberOfProfile,
                    ProvinceACFk: provinceAC,
                    ProvincePRFk: provincePR,
                    ReligionFk: religionFK,
                    Salary: salary,
                    SalarySocialInsurance: salarySocialInsurance,
                    Department_FK: departmentFK,
                    PositionFk: positionFK,
                    TaxIdnumber: taxIdNumber,
                    BankAccountNumber: bankAccountNumber,
                    Note: note,
                    //SignContractDate: signDate,
                    //TimeExpireContract: timeExpireContract,
                    //TimeExpireProbation: timeExpireProbation,
                    //StartDate: startDate,
                    //LayOffDate: layOffDate,
                    //SocialInsuranceDate: socialInsuranceDate,
                    //LastUpdatedByFk: lastUpdatedBy,
                    //DateModified: lastUpdatedDate,
                    Status: status
                };

                $.ajax({
                    type: "POST",
                    url: "/employee/SaveEntity",
                    data: data,
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoading();
                    },
                    success: function (response) {
                        if (gId==0)
                            updateIsEmployee(addressBookFk);
                        general.notify('Ghi thành công!', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        general.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        general.notify('Có lỗi trong khi ghi danh bạ!', 'error');
                        general.stopLoading();
                    }
                });
                return false;
            }

        });
        $('#btn-add-relationship').on('click', function () {
            var template = $('#FamilyRelationship-template').html();
            var render = Mustache.render(template, {
                stt: '',
                FullName: '',
                YearOfBirth: '',
                Gender: '',
                Relationship: '',
                Job: '',
                Died:''
            });
            $('#tbl-FamilyRelationship').append(render);
        });

        $('#btn-add-working-process').on('click', function () {
            var template = $('#WorkingProcess-template').html();
            var render = Mustache.render(template, {
                MobilizeDate: '',
                NumberDesignation: '',
                OldWorkPlace: '',
                OldPosition: ''
            });
            $('#tbl-WorkingProcess').append(render);
        });
        $('#btn-add-language').on('click', function () {
            var template = $('#Languages-template').html();
            var render = Mustache.render(template, {
                stt: '',
                Language: '',
                Level: '',
                Certificate: ''
            });
            $('#tbl-Languages').append(render);
        });
        $('#btn-add-expertise').on('click', function () {
            var template = $('#Expertise-template').html();
            var render = Mustache.render(template, {
                stt: '',
                Degree: '',
                Major: '',
                TrainingSystem: ''
            });
            $('#tbl-Expertise').append(render);
        });
        $('#btn-add-salary-process').on('click', function () {
            var template = $('#SalaryProcess-template').html();
            var render = Mustache.render(template, {
                stt: '',
                SalaryDate: '',
                NumberDesignation: '',
                SignDate: '',
                SignOrganization: '',
                Quota: '',
                CoefficientSalary: '',
                Rank: '',
                PosAllowanceCoefficient: '',
                HarmfulAllowanceCoefficient: '',
                Note:''
            });
            $('#tbl-SalaryProcess').append(render);
        });
    }
    function loadDetail(that) {
        resetFormMaintainance();
        $.ajax({
            type: "GET",
            url: "/employee/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                var data = response;
                gId = data.KeyId;
                //getLastUpdateBy(data.LastupdatedByFk);
                $('#txtId').val(data.Id);
                if (data.UserBy != null)
                    $('#txtEmployee').val(data.UserBy.FullName);
                document.getElementById('txtEmployee').setAttribute('data-addressbookid', data.User_FK);
                loadGeneralDetail(data.User_FK);

                $('#txtIdCard').val(data.IdCard);
                $('#selBirthplace').val(data.Birthplace);
                $('#selOriginFK').val(data.OriginFk);
                $('#selNationFK').val(data.NationFk);
                $('#selNationality').val(data.NationalityFk);

                if (data.IDDate) general.setDateTimePicker($('#dtIDDate'), data.IDDate);
                $('#txtPermanentResidence').val(data.PermanentResidence);
                $('#selDistrictPR').val(data.DistrictPR);
                $('#selProvincePR').val(data.ProvincePRFk);

                $('#txtAccommodationCurrent').val(data.AccommodationCurrent);
                $('#selDistrictAC').val(data.DistrictAC);
                $('#selProvinceAC').val(data.ProvinceACFk);
                $('#txtATMAccountName').val(data.BankName);
                $('#txtIDAccount').val(data.IDAccount);
                


                $('#selReligionFK').val(data.ReligionFk);
                $('#selDepartmentFK').val(data.Department_FK);
                $('#selPositionFK').val(data.PositionFk);
                $('#txtTaxIDNumber').val(data.TaxIdnumber);
                $('#txtBankAccountNumber').val(data.BankAccountNumber);
                $('#txtNotes').val(data.Note);
                if (data.LastupdatedByFk != null)
                    $('#txtLastUpdatedByFK').val(data.LastupdatedBy.FullName);
                $('#dtDateMotified').val(general.dateFormatJson(data.DateModified, true));
                if (data.Status == 1)
                    $('#chkStatus').prop('checked', true);
                else
                    $('#chkStatus').prop('checked', false);


                $("[name='optradio']").prop("checked", false);
                var FamilyC = data.FamilyCircumstances;
                if (FamilyC) {
                    var rdo = $('*[name="optradio"]');
                    $.each(rdo, function (keyT, valT) {
                        if ((valT.value == $.trim(FamilyC)) && ($.trim(FamilyC) != '') && ($.trim(FamilyC) != null))
                            $('*[name="optradio"][value="' + (FamilyC) + '"]').prop('checked', true);
                    });
                }
                $('#txtNOChildren').val(data.NOChildren);



                loadFamilyCircumstances(data.KeyId);
                loadHpWorkingProcessdetail(data.KeyId);
                loadHpExpertiseDetail(data.KeyId);
                loadHpLanguageDetail(data.KeyId);



                $('#txtNumberOfProfile').val(data.NumberOfProfile);
                $('#txtNumberOfContract').val(data.NumberOfContract);
                $('#selLaborContractType').val(data.LaborContractType);
                if (data.SignContractDate) general.setDateTimePicker($('#dtSignDate'), data.SignContractDate);
                if (data.TimeExpireContract) general.setDateTimePicker($('#dtTimeExpireContract'), data.TimeExpireContract);
                if (data.TimeExpireProbation) general.setDateTimePicker($('#dtTimeExpireProbation'), data.TimeExpireProbation);
                if (data.StartDate) general.setDateTimePicker($('#dtStartDate'), data.StartDate);
                $('#txtInfoSaveFile').val(data.InfoSaveFile);
                if (data.LayOffDate) general.setDateTimePicker($('#dtLayOffDate'), data.LayOffDate);

                $('#selLiteracy').val(data.LiteracyFk);

                $('#txtIDSocialInsurance').val(data.IDSocialInsurance);
                $('#txtSalarySocialInsurance').val(data.SalarySocialInsurance);
                $('#txtSocialInsuranceDate').val(data.SocialInsuranceDate);
                $('#txtSalary').val(data.Salary);

                $('#modal-add-edit').modal('show');
                general.stopLoading();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
            }
        });
    }

    function loadGeneralDetail(that) {
        $.ajax({
            type: "GET",
            url: "/addressbook/GetById",
            data: { id: that },
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    var data = response;
                    $('#txtGender').val(data.Gender == 0 ? "Nam" : "Nữ");
                    $('#txtDOB').val(general.dateFormatJson(data.Dob, true));
                    $('#txtIDNumber').val(data.IdNumber);
                    $('#txtPhoneNumber').val(data.PhoneNumber);
                    $('#txtEmail').val(data.Email);
                }
            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
            }
        });
    }

    function loadFamilyCircumstances(that) {
        $.ajax({
            type: "GET",
            url: "/employee/GetHpFamilyCircumstancesDetail",
            data: { id: that },
            dataType: "json",
            success: function (response) {
                console.log("Family");
                console.log(response);
                if (response != null) {
                    var template = $('#FamilyRelationship-template').html();
                    var render;
                    $.each(response, function (i, item) {
                        render += Mustache.render(template, {
                            stt: i+1,
                            FullName: item.RelativesName,
                            YearOfBirth: item.YearOfBirth,
                            Gender: general.getGender(item.Gender),
                            Relationship: item.Relationship,
                            Job: item.Job,
                            Died: item.Died
                        });
                    });
                    $('#tbl-FamilyRelationship').append(render);
                }
            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
            }
        });
    }

    function loadHpWorkingProcessdetail(that) {
        $.ajax({
            type: "GET",
            url: "/employee/GetHpWorkingProcessDetail",
            data: { id: that },
            dataType: "json",
            success: function (response) {
                console.log("Working");
                console.log(response);
                if (response != null) {
                    var template = $('#WorkingProcess-template').html();
                    var render;
                    $.each(response, function (i, item) {
                        render += Mustache.render(template, {

                            MobilizeDate: general.dateFormatJson(item.MobilizeDate,false),
                            NumberDesignation: item.NumberDesignation,
                            OldWorkPlace: item.OldWorkPlace,
                            OldPosition: item.OldPosition
                        });
                    });
                $('#tbl-WorkingProcess').append(render);
                }
            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
            }
        });
    }

    function loadHpExpertiseDetail(that) {
        $.ajax({
            type: "GET",
            url: "/employee/GetHpExpertiseDetail",
            data: { id: that },
            dataType: "json",
            success: function (response) {
                console.log("Expertise");
                console.log(response);
                if (response != null) {
                    var template = $('#Expertise-template').html();
                    var render;
                    $.each(response, function (i, item) {
                        render += Mustache.render(template, {
                            stt: i+1,
                            Degree: item.DegreeFK,
                            Major: item.MajorFK,
                            TrainingSystem: item.TrainingSystemFK
                        });
                    });
                    $('#tbl-Expertise').append(render);
                }
            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
            }
        });
    }

    function loadHpLanguageDetail(that) {
        $.ajax({
            type: "GET",
            url: "/employee/GetHpLanguageDetail",
            data: { id: that },
            dataType: "json",
            success: function (response) {
                console.log("Language");
                console.log(response);
                if (response != null) {
                    var template = $('#Languages-template').html();
                    var render;
                    $.each(response, function (i, item) {
                        render += Mustache.render(template, {
                            stt: i+1,
                            Language: item.LanguageFK,
                            Level: item.LevelFK,
                            Certificate: item.Certificate
                        });
                    });
                    $('#tbl-Languages').append(render);
                }
            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
            }
        });
    }








    function getLastUpdateBy(Id) {
        $.ajax({
            type: 'GET',
            url: '/employee/GetFullName',
            data: { Id },
            dataType: 'json',
            success: function (response) {

                userName = response.responseText;
                console.log(userName);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    function updateIsEmployee(id) {
        $.ajax({
            type: 'POST',
            url: '/addressbook/UpdateIsEmployee',
            data: { id },
            dataType: 'json',
            success: function (response) {

            },
            error: function (error) {

            }
        });
    }
    function resetFormMaintainance() {
        $('#txtEmployee').prop('disabled', false);
        $('#txtId').val(0);
        $('#txtEmployee').val('');
        $('#selBirthplace').val(null);
        $('#selOriginFK').val('');
        $('#selNationFK').val('');
        $('#selReligionFK').val('');
        $('#txtTaxIDNumber').val('');
        $('#txtBankAccountNumber').val('');
        $('#selDepartmentFK').val('');
        $('#selPositionFK').val('');
        $('#txtLastUpdatedByFK').val('');
        $('#txtNotes').val('');
        $('#dtpLast_Purchase_Date').val('');
        $('#dtpLast_Payment_Date').val('');
        $('#dtDateModified').val('');
        $("#chkStatus").prop('checked', true);
        $('#tbl-FamilyRelationship').empty();
        $('#tbl-WorkingProcess').empty();
        $('#tbl-Expertise').empty();
        $('#tbl-Languages').empty();
    }
}

function loadSelEmployee(status, type, callback) {
    $.ajax({
        type: 'GET',
        data: {
            status: status,
            Type: type
        },
        url: '/addressbook/GetListFullName',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            if (response != null && response.length > 0)
                callback(response);
            else {
                general.notify('Hết danh sách nhân viên cần tạo mới. Vui lòng thêm ở danh bạ trước!', 'error');
                return false;
            }
            return true;

        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
        }
    });
}



function loadNationality() {
    $('#selNationality option').remove();
    $.ajax({
        type: 'GET',
        url: '/employee/GetAllNationality',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $('#selNationality').append("<option value=''>Chọn quốc tịch</option>");
            $.each(response, function (i, item) {
                $('#selNationality').append("<option value='" + item.KeyId + "'>" + item.NationalityName + "</option>");
            });
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
        }
    });
}



function loadCity() {
    $('#selBirthplace option').remove();
    $('#selProvincePR option').remove();
    $('#selProvinceAC option').remove();
    $.ajax({
        type: 'GET',
        url: '/employee/GetAllCity',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $('#selBirthplace').append("<option value=''>Chọn nơi sinh</option>");
            $('#selOriginFK').append("<option value=''>Chọn quê quán</option>");
            $('#selProvincePR').append("<option value=''>Chọn Tỉnh/TP</option>");
            $('#selProvinceAC').append("<option value=''>Chọn Tỉnh/TP</option>");
            $.each(response, function (i, item) {
                $('#selBirthplace').append("<option value='" + item.KeyId + "'>" + item.CityName + "</option>");
                $('#selOriginFK').append("<option value='" + item.KeyId + "'>" + item.CityName + "</option>");
                $('#selProvincePR').append("<option value='" + item.KeyId + "'>" + item.CityName + "</option>");
                $('#selProvinceAC').append("<option value='" + item.KeyId + "'>" + item.CityName + "</option>");
            });
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
        }
    });
}
function loadNation() {
    $('#selNationFK option').remove();
    $.ajax({
        type: 'GET',
        url: '/employee/GetAllNation',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $('#selNationFK').append("<option value=''>Chọn dân tộc</option>");
            $.each(response, function (i, item) {
                $('#selNationFK').append("<option value='" + item.KeyId + "'>" + item.NationName + "</option>");
            });
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
        }
    });
}
function loadReligion() {
    $('#selReligionFK option').remove();
    $.ajax({
        type: 'GET',
        url: '/employee/GetAllReligion',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $('#selReligionFK').append("<option value=''>Chọn tôn giáo</option>");
            $.each(response, function (i, item) {
                $('#selReligionFK').append("<option value='" + item.KeyId + "'>" + item.ReligionName + "</option>");
            });
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
        }
    });
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



function loadPosition() {
    $('#selPositionFK option').remove();
    $.ajax({
        type: 'GET',
        url: '/employee/GetAllPosition',
        success: function (response) {
            console.log(response);
            $('#selPositionFK').append("<option value=''>Chọn chức vụ</option>");
            $.each(response, function (i, item) {
                $('#selPositionFK').append("<option value='" + item.KeyId + "'>" + item.PositionName + "</option>");
            });
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
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
        url: '/employee/GetAllPaging',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    Id: item.Id,
                    FullName: item.UserBy != null ? item.UserBy.FullName : '',
                    MobilePhone: item.UserBy != null ? item.UserBy.PhoneNumber : '',
                    TaxIDNumber: item.UserBy != null ? item.UserBy.TaxIdnumber : "",
                    Email: item.UserBy != null ? item.UserBy.Email : '',
                    DepartmentName: item.DepartmentFkNavigation != null ? item.DepartmentFkNavigation.DepartmentName : "",
                    PositionName: item.PositionFkNavigation != null ? item.PositionFkNavigation.PositionName : ""
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
    if (totalsize>0)
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
