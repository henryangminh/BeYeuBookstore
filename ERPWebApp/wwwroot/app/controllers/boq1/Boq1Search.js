var Boq1SearchController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    this.initData = function () {
        loadData();
    }
    function registerEvents() {
        $('#ddlShowPage').on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        //-------------  event button main -------------------
        $('#btnSearch_search').on('click', function () {
            loadData(true);
        });
        $('#txtKeyword_search').on('keypress', function (e) {
            if (e.which === 13) {
                loadData(true);
            }
        });
        $("#btnCreate").on('click', function () {
            if (boq1.lengthListProject() == 0) {
                general.notify('Không có công trình để tạo mới vui lòng kiểm tra lại !', 'error');
                return;
            }
            var id = 0;
            $('#txtId').val(id);
            $('#Page-searchBoq1').hide();
            $('#Page-mainBoq1').show();
            boq1.loadbyid(id, false);
        });
        $('#btnRefresh-search').on('click', function () {
            $('#btnRefresh-search').hide();
            datarefresh(function () {
                $('#btnRefresh-search').show();
            });
        });
        $('#btn-cancel-boq1').on('click', function () {
            boq1.reset_ControlForm();
            $('#Page-searchBoq1').show();
            $('#Page-mainBoq1').hide();
            // load lại data;
            $('#btnSearch_search').click();

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var SalesOrderId = $(this).data('id');
            if (SalesOrderId > 0) {
                $('#txtId').val(SalesOrderId);
                $('#Page-searchBoq1').hide();
                $('#Page-mainBoq1').show();
               
                boq1.loadbyid(SalesOrderId, false);
            }

        });
        $('body').on('click', '.btn-view', function (e) {
            e.preventDefault();
            var SalesOrderId = $(this).data('id');
            if (SalesOrderId > 0) {
                $('#txtId').val(SalesOrderId);
                $('#Page-searchBoq1').hide();
                $('#Page-mainBoq1').show();
                boq1.loadbyid(SalesOrderId, true);
            }
            //$("#horizontal-form").addClass("disabledbutton");
        });
        //---------------end search--------------------------

        //----------------event button main ---------------------

        $('#btn-import').on('click', function () {
            $('#fileimportexcel-od').click();
        })
        $("#fileimportexcel-od").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            //check type
            var validExts = new Array(".xlsx", ".xls");
            var fileExt = fileUpload.value;
            if (general.checkFileType(fileExt, validExts)) {
                $(this).val('');
                $('#preloader').show();
                $('body').css({ 'opacity': 0.5 });
                // chưa làm
                $.ajax({
                    url: 'SalesOrderBOQ1/ImportExcelCons',
                    type: 'POST',
                    data: fileData,
                    processData: false,  // tell jQuery not to process the data
                    contentType: false,  // tell jQuery not to set contentType
                    success: function (data) {
                        console.log(data);
                        if (data != null) {
                            var _listOrderdetail = data.tblboqOrderdetail;

                            if (data.listError != "") {
                                general.confirm("Có một vài mã bị lỗi: " + data.listError + ".   \n Bạn có muốn tiếp tục ?", function () {
                                    // xữ lý tính tổng và hiển thị
                                    boq1.editPriceAfterImportEx(_listOrderdetail);
                                })
                                general.stopLoad();
                            }
                            else {
                                // xữ lý tính tổng và hiển thị
                                boq1.editPriceAfterImportEx(_listOrderdetail);
                            }
                        }
                    },
                    error: function (err) {
                        console.log(err);
                        $('#preloader').hide();
                    }
                });

            }
            return false;
        });
        $('#btn-export').on('click', function () {
            $('#modal-print-report').modal('show');
        });
        $('#btnConfirmTT').on('click', function () {
            var keyId = $('#txtId').val();
            var projectId = $('#txtProjectid').data('id');
            var _rev = $('#lblSO_ref').text();
            var gstatusid = boq1.getstatusSo();
            if (keyId > 0 && projectId != '' && gstatusid != general.soStatus.CompletedBoq1) {
                general.startLoad();
                if (_rev == '') // không phải là rev
                    gupdatestatus.projectForSo(projectId, keyId, function () {
                        boq1.setSoStatusForgstatus(general.soStatus.CompletedBoq1, function () {
                            boq1.confirmTT(keyId, function () { loadData(true); });
                        });
                    });
                else {
                    boq1.setSoStatusForgstatus(general.soStatus.CompletedBoq1, function () {
                        boq1.confirmTT(keyId, function () { loadData(true); });
                    });
               }
                $('#btnConfirm').hide();
                $('#btnUnConfirm').hide();
                $('#btnConfirmTT').hide();
               
            }
        });

        $('#btnClose').on('click', function () {
            general.confirm('Bạn có chắc chắn không ?', function () {
                boq1.reset_ControlForm();
                $('#Page-searchBoq1').show();
                $('#Page-mainBoq1').hide();
            });
        });

        

        //----------------end event button main ---------------------



        function datarefresh(callback) {
            loadData();
            boq1.reloadProject();
            if (callback != undefined)
            callback();
        }
    }

}

function loadData(isPageChanged) {
    var template = $('#table-template-search').html();
    var render = "";
    $.ajax({
        type: 'GET',
        data: {
            keyword: $('#txtKeyword_search').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize
        },
        url: '/SalesOrderBOQ1/GetAllPaging',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    stt: i + 1,
                    KeyId: item.KeyId,
                    ConstructionId: item.ConstructionId,
                    ConstructionName: item.ConstructionName,
                    CustomerName: item.CustomerName,
                    version: item.version,
                    StatusName: item.Status
                });
            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content_search').html(render);
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
