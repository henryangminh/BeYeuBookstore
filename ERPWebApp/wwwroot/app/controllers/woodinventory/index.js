var inventoryController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    var listDelete = [];
    var gId = 0;
    var gListverdor=[];
    function registerEvents() {
        loadVendor();
        loadWarehouse();
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
                },
                txtHeight: {
                    number: true
                },
                txtWidth: {
                    number: true
                },
                txtLength: {
                    number: true
                },
                txtSalePrice: {
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
            loadData();
        });
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData();
            }
        });

        $("#btnCreate").on('click', function () {
            resetFormMaintainance(false);
            autocomplete(document.getElementById('txtVendor'), gListverdor);
            $('#modal-add-edit').modal('show');

        });
        $('#btn-add-price').on('click', function () {
            var template = $('#template-table-price').html();
            var render = Mustache.render(template, {
                CustomerFk: "",
                CustomerId: "",
                CustomerName: '',
                SalePrice: 0
            });
            $('#table-price-content').append(render);
        });

        // search product---
        $('#txtProduct').on('input', function (e) {
            e.preventDefault();
            var t = $(this);
            var temp = $('.contain-product');
            var keycode = $(this).val();
            delay(function () {
                if (keycode != "") {
                    loadSearchProduct(keycode, function (render) {
                        if (render != "")
                            temp.show();
                        else
                            temp.hide();
                    });

                }
                else {
                    temp.hide();
                }
            }, 350);
            return false;
        });

        $('.contain-product').on('click', '#ddlproduct', function (e) {
            var productId = $(this).data('id');
            var ProductName = $(this).children('#ProductName').text();
            var ProductCode = $(this).children('#ProductCode').text();
            $('#txtProduct').val(ProductCode + ' | ' + ProductName);
            $('#txtProduct').data('keyid', productId);
            $(this).parent('.contain-customer').hide();
        });
        // an khi click ra khoi vung chu thich
        $(document).click(function (e) {
            if (!$(e.target).is('.contain-product')) {
                $('.contain-product').hide();
            }
        });
        //----------------------------
        $('body').on('input', '#txtCustomerId', function (e) {
            e.preventDefault();
            var t = $(this);
            var temp = $(this).siblings('.contain-customer');
            var keycode = $(this).val();
            delay(function () {
                if (keycode != "") {
                    loadCustomer(keycode, t, function (render) {
                        if (render != "")
                            temp.show();
                        else
                            temp.hide();
                    });

                }
                else {
                    temp.hide();
                }
            }, 350);
            return false;
        });
        
        $('body').on('click', '.contain-customer #ddlcustomer', function (e) {
            var customerId = $(this).data('idcustomer');
            var customerName = $(this).children('#fullname').text();
            var id = $(this).children('#id').text();
            $(this).parent('.contain-customer').parent('.autocomplete').parent('.col-customerId').siblings('.customerName').text(customerName);
            $(this).parent('.contain-customer').siblings('#txtCustomerId').val(id);
            $(this).parent('.contain-customer').siblings('#txtCustomerId').data('keyid', customerId);
            $(this).parent('.contain-customer').hide();
        });
        $('body').on('click', '.btn-delete-price', function () {
            var id = $(this).data('id');
            $(this).closest('tr').remove();
            if (gId > 0)
                listDelete.push(id);
        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            resetFormMaintainance(true);
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/WoodInventory/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    console.log(response);
                    var data = response;
                    gId = data.KeyId;
                    $('#txtId').val(data.KeyId);
                    $('#selWarehouse').val(data.WarehouseFk);
                    $('#txtVendor').data('id', data.VendorNumberFk);
                    $('#txtVendor').val(data.VendorNumberFkNavigation.UserBy.FullName);
                    $('#selCategory').val(data.CategoryFk);
                    $('#txtProduct').data('keyid', data.ProductFk);
                    $('#txtProduct').val(data.ProductFkNavigation.ProductName);
                    $('#selUnit').val(data.ProductFkNavigation.ProductUnit);
                    $('#txtQuantityPerUnit').val(data.QuantityPerUnit);
                    $('#txtLocation').val(data.Location);
                    $('#txtTaxRate').val(data.TaxRate);
                    $('#txtROP').val(data.ROP);
                    $('#txtEOQ').val(data.EOQ);
                    //$('#txtUnit_Price').val(data.UnitPrice);
                    $('#txtAverageCost').val(general.toMoney(data.AverageCost));
                    $('#txtLastCost').val(general.toMoney(data.LastCost));
                    $('#dtpLastSaleDate').val(general.dateFormatJson(data.LastSaleDate,true));
                    $('#dtpLastPODate').val(general.dateFormatJson(data.LastPODate,true));
                    $('#dtpLastReceiptDate').val(general.dateFormatJson(data.LastReceiptDate,true));
                    $('#txtLeadTime').val(data.LeadTime);
                    $('#txtQty_On_Order').val(data.Qty_On_Order);
                    $('#txtQty_On_Hand').val(data.Qty_On_Hand);
                    $('#txtQty_In_Stock').val(data.Qty_In_Stock);
                    $('#txtQty_Allocated').val(data.Qty_Allocated);
                    $('#txtTotalInventoryValue').val(general.toMoney(data.TotalInventoryValue));
                    $('#selGL_Asset').val(data.GL_Asset);
                    $('#selGL_COGS').val(data.GL_COGS);
                    $('#selGL_Sales').val(data.GL_Sales);
                    $('#selGL_NonTaxSales').val(data.GL_NonTaxSales);
                    $('#txtLength').val(data.Length);
                    $('#txtWidth').val(data.Width);
                    $('#txtHeight').val(data.Height);
                    if (data.LastUpdateByFk != null)
                        $('#txtLastUpdatedByFK').val(data.LastUpdateByFkNavigation.FullName);

                    $('#dtDateMotified').val(general.dateFormatJson(data.DateModified, true));
                    if (data.FixedAsset == 1)
                        $('#chkFixedAsset').prop('checked', true);
                    else
                        $('#chkFixedAsset').prop('checked', false);
                    loadPrice(data.IcInventoryCustomerPrices, function () {
                        $('#modal-add-edit').modal('show');
                        general.stopLoading();
                    });


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

                var productFk = $('#txtProduct').data('keyid');

                var unitFk = $('#selUnit option:selected').val();
                //var quantityPerUnit = $('#txtQuantityPerUnit').val();
                var vendorNumberFk = $('#txtVendor').data('id');
                var location = $('#txtLocation').val();
                var taxRate = general.toFloat($('#txtTaxRate').val());
                var rop = general.toInt($('#txtROP').val());
                var eoq = $('#txtEOQ').val();
                //var unitPrice = $('#txtUnit_Price').val();
                var averageCost = general.toInt($('#txtAverageCost').val());
                var lastCost = general.toInt($('#txtLastCost').val());
                var lastSaleDate = $('#dtpLastSaleDate').val();
                var lastPODate = $('#dtpLastPODate').val();
                var lastReceiptDate = $('#dtpLastReceiptDate').val();
                var leadTime = general.toInt($('#txtLeadTime').val());
                var qty_on_order = general.toFloat($('#txtQty_On_Order').val());
                var qty_on_hand = general.toFloat($('#txtQty_On_Hand').val());
                var qty_in_stock = general.toFloat($('#txtQty_In_Stock').val());
                var qty_Allocated = general.toFloat($('#txtQty_Allocated').val());
                var totalInventoryValue = general.toInt($('#txtTotalInventoryValue').val());
                var gl_asset = $('#selGL_Asset option:selected').val();
                var gl_cogs = $('#selGL_COGS option:selected').val();
                var gl_sales = $('#selGL_Sales option:selected').val();
                var gl_nonTaxSales = $('#selGL_NonTaxSales option:selected').val();
                var lastUpdatedBy = $('#user').data('userid');
                var fixedAsset = $("#chkFixedAsset").is(':checked');
                var length = general.toFloat($('#txtLength').val());
                var width = general.toFloat($('#txtWidth').val());
                var height = general.toFloat($('#txtHeight').val());
                GetCustomerPrice(function (listPrice) {
                    var data = {
                        KeyId: gId,
                        CategoryFk: categoryFk,
                        WarehouseFk: warehouseFk,
                        ProductFk: productFk,
                        UnitFk: unitFk,
                        FixedAsset: fixedAsset,
                        VendorNumberFk: vendorNumberFk,
                        Location: location,
                        TaxRate: taxRate,
                        ROP: rop,
                        EOQ: eoq,
                        Length: length,
                        Width: width,
                        Height: height,
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
                        LastUpdateByFk: lastUpdatedBy,
                        IcInventoryCustomerPrices: listPrice
                    };
                    if (listDelete.length > 0) {
                        Delete(listDelete, function () {
                            Save(data);
                        });
                    }
                    else
                        Save(data);
                });



                return false;
            }

        });
    }

    function loadWarehouse() {
        $.ajax({
            type: 'GET',
            url: '/WoodInventory/GetAllWarehouse',
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

    function loadVendor() {
        $.ajax({
            type: 'GET',
            url: '/Vendor/GetListFullName',
            data: {status:1},
            dataType: 'json',
            success: function (response) {
                console.log(response);
                if (response.length > 0)
                    gListverdor = response;
                else
                    general.notify('Chưa có nhà cung cấp nào vui lòng thêm nhà cung cấp !', 'error');
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
            url: '/WoodInventory/GetAllUnit',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $('#selUnit').append("<option value=''>Chọn ĐVT</option>");
                $.each(response, function (i, item) {
                    $('#selUnit').append("<option value='" + item.Id + "'>" + item.Name + "</option>");
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
            url: '/WoodInventory/GetAllLedger',
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

    function resetFormMaintainance(isEdit) {
        if (isEdit) {
            $('#selWarehouse').prop('disabled', true);
            $('#txtProduct').prop('disabled', true);
            $('#txtVendor').prop('disabled', true);
        }
        else {
            $('#selWarehouse').prop('disabled', false);
            $('#txtProduct').prop('disabled', false);
            $('#txtVendor').prop('disabled', false);
        }
        gId = 0;
        $('#txtId').val('0');
        $('#selWarehouse').val('');
        $('#selCategory').val('');
        $('#txtProduct').val('');
        $('#txtVendor').val('');
        $('#selUnit').val('');
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
        $('#txtLength').val('');
        $('#txtHeight').val('');
        $('#txtWidth').val('');
        $('#table-price-content tr').each(function () {
            $(this).remove();
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
            url: '/WoodInventory/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        KeyId: item.KeyId,
                        ProductCode: item.ProductFkNavigation.ProductCode,
                        ProductName: item.ProductFkNavigation.ProductName,
                        Warehouse: item.WarehouseFkNavigation.WarehouseName,
                        Category: '',
                        UnitName:'',
                        //MobilePhone: item.CategoryFkNavigation.CategoryText,
                        Qty_In_Stock: item.Qty_In_Stock,
                        TotalInventoryValue: general.toMoney(item.TotalInventoryValue)
                    });
                    $('#lblTotalRecords').text(response.RowCount);
                    if (render != '') {
                        $('#tbl-content').html(render);
                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);
                });
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
    function loadCustomer(key, temp, callback) {
        var template = $('#customer-template').html();
        var render = "";
        var a = temp.siblings('.contain-customer');
        $.ajax({
            type: 'GET',
            url: '/customer/GetAllCustomer',
            data: { keyword: key },
            dataType: 'json',
            success: function (response) {
                console.log(response);
                //listProduct = response;
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        KeyId: item.Id,
                        Id: item.sId,
                        FullName: item.Name
                    });
                    if (render != '') {
                        a.html(render);
                    }
                    callback(render);
                });
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    var delay = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearTimeout(timer);
            timer = setTimeout(callback, ms);
        };
    })();
    function GetCustomerPrice(callback) {
        var arr = [];
        var count = 0;
        $('#table-price-content tr').each(function () {
            var customerFk = $(this).children('.col-customerId').children('.autocomplete').children('#txtCustomerId').data('keyid');
            var price = general.toInt($(this).children('#unitPrice').children('#txtSalePrice').val());
            var keyId = general.toInt($(this).children('#btn').children('.btn-delete-price').data('id'));
            var customerId = $(this).children('.col-customerId').children('.autocomplete').children('#txtCustomerId').val();
            var customerName = $(this).children('.customerName').text();
            var temp = {
                KeyId: keyId,
                InventoryFk: gId,
                CustomerFk: customerFk,
                CustomerId: customerId,
                CustomerName: customerName,
                UnitPrice: price
            };
            arr.push(temp);
            count++;
        });
        if (count == $('#table-price-content tr').length)
            callback(arr);
    }

    function Save(data) {
        $.ajax({
            type: "POST",
            url: "/WoodInventory/SaveEntity",
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
            error: function (err) {
                general.notify('Có lỗi trong khi ghi hàng tồn kho!', 'error');
                general.stopLoading();
            }
        });
    }
    function Delete(list, callback) {
        $.ajax({
            type: 'POST',
            url: '/WoodInventory/Delete',
            data: { listId: list },
            success: function (res) {
                callback();
            },
            error: function (err) {
                general.notify('Có lỗi khi xóa dữ liệu!', 'error');
            }
        });
    }
    function loadPrice(list, callback) {
        var template = $('#template-table-price').html();
        var render = '';
        $.each(list, function (i, item) {
            render += Mustache.render(template, {
                KeyId: item.KeyId,
                CustomerFk: item.CustomerFk,
                CustomerId: item.CustomerId,
                CustomerName: item.CustomerName,
                SalePrice: general.toMoney(item.UnitPrice)
            });

        });
        if (render != '') {
            $('#table-price-content').html(render);
        }
        callback();
    }
    function loadSearchProduct(key,callback) {
        var template = $('#product-template').html();
        var render = "";
        var body = $('.contain-product');
        $.ajax({
            type: 'GET',
            url: '/Woodinventory/GetSearchProduct',
            data: { keyword: key },
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        KeyId: item.Id,
                        ProductCode: item.Name,
                        ProductName: item.Des
                    });
                });
                body.html(render);
                callback(render);
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
}