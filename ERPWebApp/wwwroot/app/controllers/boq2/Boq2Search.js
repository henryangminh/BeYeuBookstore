var Boq2SearchController = function () {
    this.initData = function () {
        loadData();
    }
    this.initialize = function () {
        loadData();
        registerEvents();
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
     
        $('#btnRefresh-search').on('click', function () {
            $('#btnRefresh-search').hide();
            datarefresh(function () {
                $('#btnRefresh-search').show();
            });
        });
        $('#btn-cancel-Boq2').on('click',function() {
            boq2.reset_ControlForm();
            $('#Page-searchBoq2').show();
            $('#Page-mainBoq2').hide();
            // load lại data;
            $('#btnRefresh-search').click();

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var SalesOrderId = $(this).data('id');
            if (SalesOrderId > 0) {
                $('#txtId').val(SalesOrderId);
                $('#Page-searchBoq2').hide();
                $('#Page-mainBoq2').show();
                boq2.loadbyid(SalesOrderId, false);
            }

        });
        
        $('body').on('click', '.btn-view', function (e) {
            e.preventDefault();
            var SalesOrderId = $(this).data('id');
            if (SalesOrderId > 0) {
                $('#txtId').val(SalesOrderId);
                $('#Page-searchBoq2').hide();
                $('#Page-mainBoq2').show();
                boq2.loadbyid(SalesOrderId, true);
            }
            //$("#horizontal-form").addClass("disabledbutton");
        });
        //---------------end search--------------------------

        //----------------event button main ---------------------
        $('#btn-templates').on('click', function () {
            $.ajax({
                type: "POST",
                url: "SalesOrderboq2/templatesExcel",
                data: {
                    soid: $('#txtId').val(),
                    constructionid: $('#txtProjectid').text
                },
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    window.location.href = response;
                    general.stopLoading();
                },
                error: function () {
                    general.notify('Quá trình bị lỗi', 'error');
                    general.stopLoading();
                }
            });
        })
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
                general.startLoad();
                // chưa làm
                $.ajax({
                    url: 'SalesOrderBOQ2/ImportExcelCons',
                    type: 'POST',
                    data: fileData,
                    processData: false,  // tell jQuery not to process the data
                    contentType: false,  // tell jQuery not to set contentType
                    success: function (data) {
                        console.log(data);
                        if (data.length>0) {
                           //ghi dữ liệu lên table và cập nhật lại tổng
                            boq2.importExcel(data);
                        }
                        general.stopLoad();
                    },
                    error: function (err) {
                        console.log(err);
                        general.stopLoad();
                    }
                });

            }
            return false;
        });
        $('#btn-export').on('click', function () {
            $('#modal-print-report-boq2').modal('show');
        });
      
        $('#btnClose').on('click', function () {
            general.confirm('Bạn có chắc chắn không ?', function () {
                boq2.reset_ControlForm();
                $('#Page-searchBoq2').show();
                $('#Page-mainBoq2').hide();
            });
        });

        //----------------end event button main ---------------------



        function datarefresh(callback) {
            loadData(true);
            //boq2.reloadProject();
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
        url: '/SalesOrderboq2/GetAllPaging',
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
                    SO_ref: item.SO_ref,
                    version: item.version,
                    StatusName: item.Status,
                    StartDate: general.dateFormatJson(item.DateRequestBOQ2,true),
                    NDay: item.Ndate,
                    EndDate: item.DateRequestBOQ2 == null ? '' : general.dateFormatJson(general.addDays(item.DateRequestBOQ2, item.Ndate), true),
                    // tạm thời sữa sau
                    isdisplay: item.Status =='Đã Xác nhận BOQ 2' ? 'none;':''
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
