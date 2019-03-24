var inventoryController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        loadCategory();
        loadWarehouse();
        loadProduct();
        loadUnit();
        loadLedger();
        $("#txtId").prop('disabled', true);
        $("#dtpLastSaleDate").prop('disabled', true);
        $("#dtpLastPODate").prop('disabled', true);
        $("#dtpLastReceiptDate").prop('disabled', true);
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $("#chkFixedAsset").prop('checked', true);
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {

                selProduct: {
                    required: true
                },
                selCategory: {
                    required: true
                },
                selUnit: {
                    required: true
                },
                txtROP: {
                    required: true,
                    number: true
                },
                txtEOQ: {
                    required: true,
                    number: true
                },
                txtUnit_Price: {
                    required: true,
                    number: true
                },
                txtAverageCost: {
                    number: true
                },
                txtLastCost: {
                    number: true
                },
                txtLeadTime: {
                    number: true
                },
                txtQty_On_Order: {
                    number: true
                },
                txtQty_On_Hand: {
                    number: true
                },
                txtQty_In_Stock: {
                    number: true
                },
                txtQty_Allocated: {
                    number: true
                },
                txtTotalInventoryValue: {
                    number: true
                },
                txtTaxRate: {
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
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            resetFormMaintainance();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/inventory/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    console.log(response);
                    var data = response;
                    $('#txtId').val(data.KeyId);
                    $('#selWarehouse').val(data.WarehouseFk);
                    $('#selCategory').val(data.CategoryFk);
                    $('#selProduct').val(data.ProductFk);
                    $('#selUnit').val(data.UnitFk);
                    $('#txtQuantityPerUnit').val(data.QuantityPerUnit);
                    $('#txtLocation').val(data.Location);
                    $('#txtTaxRate').val(data.TaxRate);
                    $('#txtROP').val(data.ROP);
                    $('#txtEOQ').val(data.EOQ);
                    $('#txtUnit_Price').val(data.UnitPrice);
                    $('#txtAverageCost').val(data.AverageCost);
                    $('#txtLastCost').val(data.LastCost);
                    $('#dtpLastSaleDate').val(data.LastSaleDate);
                    $('#dtpLastPODate').val(data.LastPODate);
                    $('#dtpLastReceiptDate').val(data.LastReceiptDate);
                    $('#txtLeadTime').val(data.LeadTime);
                    $('#txtQty_On_Order').val(data.Qty_On_Order);
                    $('#txtQty_On_Hand').val(data.Qty_On_Hand);
                    $('#txtQty_In_Stock').val(data.Qty_In_Stock);
                    $('#txtQty_Allocated').val(data.Qty_Allocated);
                    $('#txtTotalInventoryValue').val(data.TotalInventoryValue);
                    $('#selGL_Asset').val(data.GL_Asset);
                    $('#selGL_COGS').val(data.GL_COGS);
                    $('#selGL_Sales').val(data.GL_Sales);
                    $('#selGL_NonTaxSales').val(data.GL_NonTaxSales);
                    if (data.LastUpdateByFk != null)
                        $('#txtLastUpdatedByFK').val(data.LastUpdateByFkNavigation.FullName);

                    $('#dtDateMotified').val(general.dateFormatJson(data.DateModified, false));
                    if (data.FixedAsset == 1)
                        $('#chkFixedAsset').prop('checked', true);
                    else
                        $('#chkFixedAsset').prop('checked', false);
                    $('#modal-add-edit').modal('show');
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
                var warehouseFk = $('#selWarehouse option:selected').val();
                var categoryFk = $('#selCategory option:selected').val();
                var id = $('#txtId').val();

                var productFk = $('#selProduct option:selected').val();

                var unitFk = $('#selUnit option:selected').val();
                var quantityPerUnit = $('#txtQuantityPerUnit').val();

                var location = $('#txtLocation').val();
                var taxRate = $('#txtTaxRate').val();
                var rop = $('#txtROP').val();
                var eoq = $('#txtEOQ').val();
                var unitPrice = $('#txtUnit_Price').val();
                var averageCost = $('#txtAverageCost').val();
                var lastCost = $('#txtLastCost').val();
                var lastSaleDate = $('#dtpLastSaleDate').val();
                var lastPODate = $('#dtpLastPODate').val();
                var lastReceiptDate = $('#dtpLastReceiptDate').val();
                var leadTime = $('#txtLeadTime').val();
                var qty_on_order = $('#txtQty_On_Order').val();
                var qty_on_hand = $('#txtQty_On_Hand').val();
                var qty_in_stock = $('#txtQty_In_Stock').val();
                var qty_Allocated = $('#txtQty_Allocated').val();
                var totalInventoryValue = $('#txtTotalInventoryValue').val();
                var gl_asset = $('#selGL_Asset option:selected').val();
                var gl_cogs = $('#selGL_COGS option:selected').val();
                var gl_sales = $('#selGL_Sales option:selected').val();
                var gl_nonTaxSales = $('#selGL_NonTaxSales option:selected').val();
                //var lastUpdatedBy = $('#user').data('userid');
                var fixedAsset = $("#chkFixedAsset").is(':checked');
                
                var data = {
                    KeyId: id,
                    CategoryFk: categoryFk,
                    WarehouseFk: warehouseFk,
                    ProductFk: productFk,
                    UnitFk: unitFk,
                    FixedAsset: fixedAsset,
                    QuantityPerUnit: quantityPerUnit,
                    Location: location,
                    TaxRate: taxRate,
                    ROP: rop,
                    EOQ: eoq,
                    UnitPrice: unitPrice,
                    AverageCost: averageCost,
                    LastCost: lastCost,
                    LastSaleDate: lastSaleDate,
                    LastPODate: lastPODate,
                    LastReceiptDate: lastReceiptDate,
                    LeadTime: leadTime,
                    Qty_On_Order: qty_on_order,
                    Qty_On_Hand: qty_on_hand,
                    Qty_In_Stock: qty_in_stock,
                    Qty_Allocated: qty_Allocated,
                    TotalInventoryValue: totalInventoryValue,
                    GL_Asset: gl_asset,
                    GL_COGS: gl_cogs,
                    GL_Sales: gl_sales,
                    GL_NonTaxSales: gl_nonTaxSales,
                    //LastUpdateByFk: lastUpdatedBy
                };

                $.ajax({
                    type: "POST",
                    url: "/inventory/SaveEntity",
                    data: data,
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoading();
                    },
                    success: function (response) {
                        general.notify('Ghi thành công!', 'success');
                        $('#modal-add-edit').modal('hide');
                        //resetFormMaintainance();
                        general.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        general.notify('Có lỗi trong khi ghi hàng tồn kho!', 'error');
                        general.stopLoading();
                    }
                });
                return false;
            }

        });
    }

    function loadWarehouse() {
        $.ajax({
            type: 'GET',
            url: '/inventory/GetAllWarehouse',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $('#selWarehouse').append("<option value=''>Chọn kho</option>");
                $.each(response, function (i, item) {
                    $('#selWarehouse').append("<option value='" + item.KeyId + "'>" + item.WarehouseName + "</option>");
                });
            },
            error: function (error) {
                console.log(error);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }

    function loadCategory() {
        $.ajax({
            type: 'GET',
            url: '/inventory/GetAllCategory',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $('#selCategory').append("<option value=''>Chọn chủng loại</option>");
                $.each(response, function (i, item) {
                    $('#selCategory').append("<option value='" + item.KeyId + "'>" + item.CategoryText + "</option>");
                });
            },
            error: function (error) {
                console.log(error);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }

    function loadProduct() {
        $.ajax({
            type: 'GET',
            url: '/inventory/GetAllProduct',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $('#selProduct').append("<option value=''>Chọn mặt hàng</option>");
                $.each(response, function (i, item) {
                    $('#selProduct').append("<option value='" + item.KeyId + "'>" + item.ProductName + "</option>");
                });
            },
            error: function (error) {
                console.log(error);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }

    function loadUnit() {
        $.ajax({
            type: 'GET',
            url: '/inventory/GetAllUnit',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $('#selUnit').append("<option value=''>Chọn ĐVT</option>");
                $.each(response, function (i, item) {
                    $('#selUnit').append("<option value='" + item.KeyId + "'>" + item.UnitName + "</option>");
                });
            },
            error: function (error) {
                console.log(error);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }

    function loadLedger() {
        $.ajax({
            type: 'GET',
            url: '/inventory/GetAllLedger',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $('#selGL_Asset').append("<option value=''>Chọn tài khoản</option>");
                $('#selGL_COGS').append("<option value=''>Chọn tài khoản</option>");
                $('#selGL_Sales').append("<option value=''>Chọn tài khoản</option>");
                $('#selGL_NonTaxSales').append("<option value=''>Chọn tài khoản</option>");
                $.each(response, function (i, item) {
                    $('#selGL_Asset').append("<option value='" + item.AccountNo + "'>" + item.AccountNo + "</option>");
                    $('#selGL_COGS').append("<option value='" + item.AccountNo + "'>" + item.AccountNo + "</option>");
                    $('#selGL_Sales').append("<option value='" + item.AccountNo + "'>" + item.AccountNo + "</option>");
                    $('#selGL_NonTaxSales').append("<option value='" + item.AccountNo + "'>" + item.AccountNo + "</option>");
                });
            },
            error: function (error) {
                console.log(error);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }

    
    function resetFormMaintainance() {
        $('#txtId').val('0');
        $('#selWarehouse').val('');
        $('#selCategory').val('');
        $('#selProduct').val('');
        $('#selUnit').val('');
        $('#txtQuantityPerUnit').val('');
        $('#txtLocation').val('');
        $('#txtTaxRate').val('');
        $('#txtROP').val('');
        $('#txtEOQ').val('');
        $('#txtUnit_Price').val('');
        $('#txtAverageCost').val('');
        $('#txtLastCost').val('');
        $('#dtpLastSaleDate').val('');
        $('#dtpLastPODate').val('');
        $('#dtpLastReceiptDate').val('');
        $('#txtLeadTime').val('');
        $('#txtQty_On_Order').val('');
        $('#txtQty_On_Hand').val('');
        $('#txtQty_In_Stock').val('');
        $('#txtQty_Allocated').val('');
        $('#txtTotalInventoryValue').val('');
        $('#selGL_Asset').val('');
        $('#selGL_COGS').val('');
        $('#selGL_Sales').val('');
        $('#selGL_NonTaxSales').val('');
        $('#txtLastUpdatedByFK').val('');
        $('#dtDateMotified').val('');
        $('#chkFixedAsset').prop('checked', true);
        $('#modal-add-edit').modal('show');
    }
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
        url: '/inventory/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoading();
        },
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    ProductRef: item.ProductFkNavigation.ProductRef,
                    ProductName: item.ProductFkNavigation.ProductName,
                    Warehouse: item.WarehouseFkNavigation.WarehouseName,
                    //MobilePhone: item.CategoryFkNavigation.CategoryText,
                    Qty_In_Stock: item.Qty_In_Stock,
                    TotalInventoryValue: item.TotalInventoryValue
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
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            general.stopLoading();
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
