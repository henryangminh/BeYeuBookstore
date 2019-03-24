var unitController = function () {
    this.initialize = function () {
        loadUnitCategory();
        loadDataUnit();
        registerEvents();
    }
    var gId = 0;
    function registerEvents() {
        $("#txtUnitKeyid").prop('disabled', true);
        $("#txtUnitLastUpdatedByFK").prop('disabled', true);
        $("#dtUnitDateModified").prop('disabled', true);
        $("#chkUnitStatus").prop('checked', true);

        $("#txtUnitName").attr('maxlength', '20');
        $("#txtFactor").attr('maxlength', '10');
        $("#txtRounding").attr('maxlength', '2');

        $('#frmUnit').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtUnitName: {
                    required: true
                },
                cbbUnitCategory: {
                    required: true
                },
                txtFactor: {
                    required: true,
                    number: true
                },
                txtRounding: {
                    required: true,
                    number: true
                },
                txtUnitPrice: {
                    required: true   
                }
            }
        });
        $('#ddlUnitShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadDataUnit(true);
        });

        $('#btnUnitSearch').on('click', function () {
            loadDataUnit(true);
        });
        $('#txtUnitKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadDataUnit(true);
            }
        });

        $("#btnUnitCreate").on('click', function () {
            loadUnitCategory();
            gId = 0;
            resetFormMaintainance();
            $('#modal-add-edit-unit').modal('show');

        });

    
        $('body').on('click', '.btn-editunit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Unit/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    console.log(response);
                    var data = response;
                    gId = data.KeyId;
                    $('#txtUnitKeyid').val(data.KeyId);
                    $('#txtUnitName').val(data.UnitName);
                    $('#cbbUnitCategory').val(data.UnitCategoryFk);
                    $('#txtFactor').val(data.Factor);
                    $('#txtRounding').val(data.Rounding);
                    if (data.LastUpdatedBy != null)
                        $('#txtUnitLastUpdatedByFK').val(data.LastUpdatedByNavigation.FullName);

                    $('#dtUnitDateModified').val(general.dateFormatJson(data.DateModified, true));
                    if (data.Status == 1)
                        $('#chkUnitStatus').prop('checked', true);
                    else
                        $('#chkUnitStatus').prop('checked', false);
                    $('#modal-add-edit-unit').modal('show');
                    general.stopLoading();

                },
                error: function (status) {
                    general.notify('Có lỗi xảy ra', 'error');
                    general.stopLoading();
                }
            });
        });

        $('#btnUnitSave').on('click', function (e) {
            if ($('#frmUnit').valid()) {
                e.preventDefault();
                save();
            }

        });
    }
    function save() {
        var status;
        var txtUnitName = $('#txtUnitName').val();
        var cbbUnitCategory = $('#cbbUnitCategory option:selected').val();
        var txtFactor = general.toFloat($('#txtFactor').val());
        var txtRounding =general.toInt($('#txtRounding').val());
        var a = $("#chkUnitStatus").is(':checked');
        if (a == true)
            status = 1;
        else
            status = 0;
        var data = {
            KeyId: gId,
            UnitName: txtUnitName,
            UnitCategoryFk: cbbUnitCategory,
            Factor: txtFactor,
            Rounding: txtRounding,
            Status: status
        };

        $.ajax({
            type: "POST",
            url: "/Unit/SaveEntity",
            data: data,
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                general.notify('Ghi thành công!', 'success');
                $('#modal-add-edit-unit').modal('hide');
                resetFormMaintainance();

                general.stopLoading();
                loadDataUnit(true);
            },
            error: function () {
                general.notify('Có lỗi trong khi ghi !', 'error');
                general.stopLoading();
            }
        });
    }

   

    function resetFormMaintainance() {
        $('#txtUnitKeyid').val('');
        $('#txtUnitName').val('');
        $('#cbbUnitCategory').val('');
        $('#txtFactor').val('');
        $('#txtRounding').val('');
        $('#txtUnitLastUpdatedByFK').val('');
        $('#dtUnitDateModified').val('');
        $("#chkUnitStatus").prop('checked', true);
    }
}

function loadUnitCategory() {
    $('#cbbUnitCategory option').remove();
    $.ajax({
        type: 'GET',
        url: '/UnitCategory/GetAll',
        dataType: 'json',
        beforeSend: function () {
            general.startLoading();
        },
        success: function (response) {
            console.log(response);
            $('#cbbUnitCategory').append("<option value=''>------ Chọn loại đơn vị tính ------</option>");
            $.each(response, function (i, item) {
                $('#cbbUnitCategory').append("<option value='" + item.KeyId + "'>" + item.UnitCategoryName + "</option>");
            });
            general.stopLoading();
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            general.stopLoading();
        }
    });
}
function loadDataUnit(isPageChanged) {
    var template = $('#table-unit').html();
    var render = "";
    $.ajax({
        type: 'GET',
        data: {
            keyword: $('#txtUnitKeyword').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize
        },
        url: '/Unit/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoading();
        },
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    stt: i + 1,
                    KeyId: item.KeyId,
                    UnitName: item.UnitName,
                    UnitCategory_FK: item.UnitCategoryFkNavigation.UnitCategoryName,
                    Factor: item.Factor,
                    Rounding: item.Rounding,
                    Status: general.getStatus(item.Status)
                });
                
                
            });
            $('#lblTotalUnitRecords').text(response.RowCount);
            $('#tbl-Unitcontent').html(render);
            wrapPagingUnit(response.RowCount, function () {
                loadDataUnit();
            }, isPageChanged);
            general.stopLoading();
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            general.stopLoading();
        }
    });
}
function wrapPagingUnit(recordCount, callBack, changePageSize) {
    var totalsize = Math.ceil(recordCount / general.configs.pageSize);
    //Unbind pagination if it existed or click change pagesize
    if ($('#paginationUnitUL a').length === 0 || changePageSize === true) {
        $('#paginationUnitUL').empty();
        $('#paginationUnitUL').removeData("twbs-pagination");
        $('#paginationUnitUL').unbind("page");
    }
    //Bind Pagination Event
    if (totalsize>0)
    $('#paginationUnitUL').twbsPagination({
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
