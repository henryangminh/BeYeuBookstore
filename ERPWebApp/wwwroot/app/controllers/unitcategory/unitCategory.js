var UnitCategoryController = function () {
    this.initialize = function () {
        loadDataUnitCategory();
        registerEvents();
    }
    var gId = 0;
    function registerEvents() {
        $("#txtUnitCategoryKeyid").prop('disabled', true);
        $("#txtUnitCategoryLastUpdatedByFK").prop('disabled', true);
        $("#dtUnitCategoryDateModified").prop('disabled', true);
        $("#chkUnitCategoryStatus").prop('checked', true);

        $("#txtUnitCategoryName").attr('maxlength', '20');
        $("#txtDescription").attr('maxlength', '255');

        $('#frmUnitCategory').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtUnitCategoryName: {
                    required: true
                }
            }
        });
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadDataUnitCategory(true);
        });

        $('#btnUnitCategorySearch').on('click', function () {
            loadDataUnitCategory();
        });
        $('#txtUnitCategoryKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadDataUnitCategory(true);
            }
        });
        $("#btnUnitCategoryCreate").on('click', function () {
            gId = 0;
            resetFormUnitCategory(true);
            $('#modal-add-edit-unit-category').modal('show');

        });

        $('body').on('click', '.btn-editunitcategory', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/UnitCategory/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    console.log(response);
                    var data = response;
                    gId = that;
                    $('#txtUnitCategoryKeyid').val(that);
                    $('#txtUnitCategoryName').val(data.UnitCategoryName);
                    $('#txtDescription').val(data.UnitCategoryDescription);
                    if (data.LastUpdatedBy != null)
                        $('#txtUnitCategoryLastUpdatedByFK').val(data.LastUpdatedByNavigation.FullName);

                    $('#dtUnitCategoryDateModified').val(general.dateFormatJson(data.DateModified, true));
                    if (data.Status == 1)
                        $('#chkUnitCategoryStatus').prop('checked', true);
                    else
                        $('#chkUnitCategoryStatus').prop('checked', false);
                    $('#modal-add-edit-unit-category').modal('show');
                    general.stopLoading();

                },
                error: function (status) {
                    general.notify('Có lỗi xảy ra', 'error');
                    general.stopLoading();
                }
            });
        });

        $('#btnUnitCategorySave').on('click', function (e) {
            if ($('#frmUnitCategory').valid()) {
                e.preventDefault();
                save();
            }

        });

        function save() {
            var status;
            var txtUnitCategoryName = $('#txtUnitCategoryName').val();
            var txtDescription = $('#txtDescription').val();
           
            var a = $("#chkUnitCategoryStatus").is(':checked');
            if (a == true)
                status = 1;
            else
                status = 0;
            var data = {
                KeyId: gId,
                UnitCategoryName: txtUnitCategoryName,
                UnitCategoryDescription: txtDescription,
                Status: status
            };

            $.ajax({
                type: "POST",
                url: "/UnitCategory/SaveEntity",
                data: data,
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    general.notify('Ghi thành công!', 'success');
                    $('#modal-add-edit-unit-category').modal('hide');
                    resetFormUnitCategory();

                    general.stopLoading();
                    loadDataUnitCategory(true);
                },
                error: function () {
                    general.notify('Có lỗi trong khi ghi !', 'error');
                    general.stopLoading();
                }
            });
        }

        function resetFormUnitCategory() {
            $('#txtUnitCategoryKeyid').val('');
            $('#txtUnitCategoryName').val('');
            $('#txtDescription').val('');
            $('#txtUnitCategoryLastUpdatedByFK').val('');
            $('#dtUnitCategoryDateModified').val('');
            $("#chkUnitCategoryStatus").prop('checked', true);
        }
    }
}
    function loadDataUnitCategory(isPageChanged) {
        var template = $('#table-unitCategory').html();
        var render = "";
        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txtUnitCategoryKeyword').val(),
                page: general.configs.pageIndex,
                pageSize: general.configs.pageSize
            },
            url: '/UnitCategory/GetAllPaging',
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
                        UnitCategoryName: item.UnitCategoryName,
                        UnitCategoryDescription: item.UnitCategoryDescription,
                        Status: general.getStatus(item.Status)
                    });
                    
                });
                $('#lblTotalUnitCategoryRecords').text(response.RowCount);
                $('#tbl-UnitCategorycontent').html(render);
                wrapPagingUnitCategory(response.RowCount, function () {
                    loadDataUnitCategory();
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
    function wrapPagingUnitCategory(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / general.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUnitCategoryUL a').length === 0 || changePageSize === true) {
            $('#paginationUnitCategoryUL').empty();
            $('#paginationUnitCategoryUL').removeData("twbs-pagination");
            $('#paginationUnitCategoryUL').unbind("page");
        }
        //Bind Pagination Event
        if (totalsize>0)
        $('#paginationUnitCategoryUL').twbsPagination({
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
