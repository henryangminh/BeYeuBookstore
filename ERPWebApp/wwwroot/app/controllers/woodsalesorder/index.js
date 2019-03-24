var salesorderController = function () {
    var listDelete = [];
    var gId = 0;
    var glistOrderStatus = [];
    var gstatusid = general.soStatusWood.create;
    var gstatusName = 'Test thử';
    var count = 1;
    var listProduct = [];
    this.initialize = function () {
        loadData();
        loadCity();
        registerEvents();
        loadOrderStatus();
        delayStatus(function () {
            var status = $('#lblSoStatus').text();
            if (status == "") {
                $('#lblSoStatus').text(gstatusName);
            }
            else
                $('#lblSoStatus').text('');
        }, 500)
    }

    this.initData = function () {
        loadData();
    }
    function registerEvents() {
        $('#btn-add-product').prop('disabled', true);
        $('#dtDateCreated').prop('disabled', true);
        $('#txtSOId').prop('disabled', true);
        $('#txtEmployee').prop('disabled', true);
        $('#btnConfirm').hide();
        $('#btnUnConfirm').hide();
        $('#btnSave').hide();
        $('#btnCancel').hide();
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtCustomerName: {
                    required: true
                },
                txtPhoneNumber: {
                    number: true
                }
            }
        });

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
            resetForm();
            var id = 0;
            $('#Page-searchSO').hide();
            $('#Page-mainSO').show();
            loadbyid(id, false);
            $('#txtCustomerName').prop('disabled', false);
            $('#btn-add-product').prop('disabled', true);
            $('#btn-add-customer').show();
        });
        $('#btnRefresh-search').on('click', function () {
            $('#btnRefresh-search').hide();
            datarefresh(function () {
                $('#btnRefresh-search').show();
            });
        });
        $('#btnCancel').on('click', function () {
            resetForm();
            $('#Page-searchSO').show();
            $('#Page-mainSO').hide();
            // load lại data;
            $('#btnSearch_search').click();

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var SalesOrderId = $(this).data('id');
            if (SalesOrderId > 0) {
                $('#txtCustomerName').prop('disabled', true);
                $('#btn-add-product').prop('disabled', false);
                $('#btn-add-customer').hide();
                $('#txtId').val(SalesOrderId);
                $('#Page-searchSO').hide();
                $('#Page-mainSO').show();
                loadbyid(SalesOrderId, false);
            }

        });
        $('body').on('click', '.btn-view', function (e) {
            e.preventDefault();
            var SalesOrderId = $(this).data('id');
            $('#txtCustomerName').prop('disabled', true);
            $('#btn-add-product').prop('disabled', false);
            $('#btn-add-customer').hide();
            if (SalesOrderId > 0) {
                $('#txtId').val(SalesOrderId);
                $('#Page-searchSO').hide();
                $('#Page-mainSO').show();
                loadbyid(SalesOrderId, true);
            }
            //$("#horizontal-form").addClass("disabledbutton");
        });
        //---------------end search--------------------------

        //----------------event button main ---------------------

        
        $('#btn-export').on('click', function () {
            $.ajax({
                type: "POST",
                url: "SalesOrderBOQ1/ExportExcel",
                data: {
                    soid: $('#txtId').val(),
                    constructionid: $('#txtProjectid').val()
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
        });

        $('#btnClose').on('click', function () {
            general.confirm('Bạn có chắc chắn không ?', function () {
                resetForm();
                $('#Page-searchSO').show();
                $('#Page-mainSO').hide();
            });
        });
        $('#txtCustomerName').on('keyup', function (e) {
            if ($('#txtCustomerName').val() != "") {
                loadCustomer(function (render) {
                    if (render != "")
                        $('#contain-listcustomer').show();
                });

            }
            else {
                $('#contain-listcustomer').hide();
            }
        });
        $('#contain-listcustomer').on('click', '#ddlcustomer', function (e) {
            var a = $(this).children("#fullname").text();
            var b = $(this).data('idcustomer');
            $('#txtCustomerName').val(a);
            $('#txtCustomerName').data('keyidcus', b);
            $('#contain-listcustomer').hide();
        });
        $(".btn-add").on("click", function (e) {
            $('#modal-add-edit').modal('show');
        });
        
        $('#btn-add-product').on('click', function () {
            var template = $('#template-table-detail').html();
            var render = Mustache.render(template, {
                stt: count,
                ProductCode: "",
                ProductName: "",
                ProductUnit: '',
                ProductQty: 0,
                UnitPrice: 0,
                SubTotal:0
            });
            count++;
            $('#table-quantity-content').append(render);
        });
        $('body').on('input ', '#txtProductCode', function (e) {
            e.preventDefault();
            var t = $(this);
            var temp = $(this).siblings('.contain-productCode');
            var keycode = $(this).val();
            delay(function () {
                if (keycode != "") {
                    loadProduct(keycode, t, function (render) {
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
        var productCodeOld = '';
        $('body').on('focusin', '#txtProductCode', function () {
            productCodeOld = $(this).val();
        });
        $('body').on('click', '.contain-productCode .ddlproduct', function (e) {
            var a = $(this).data('idproduct');
            var that = $(this);
            var temp = $(this).parent('.contain-productCode').parent('.autocomplete').parent('.col-productCode');
            if (listProduct.indexOf(a) == -1) {
                listProduct.push(a);
                var customerFk = $('#txtCustomerName').data('keyidcus');
                $.ajax({
                    type: 'GET',
                    url: '/WoodInventory/GetById',
                    data: { id: a },
                    success: function (res) {
                        console.log(res);
                        that.parent('.contain-productCode').siblings('.code').val(res.ProductFkNavigation.ProductCode);
                        that.parent('.contain-productCode').siblings('.code').data('keyid', a);
                        temp.siblings('.productName').text(res.ProductFkNavigation.ProductName);
                        temp.siblings('#productunit').text(res.ProductFkNavigation.ProductUnitNavigation.UnitName);
                        temp.siblings('#productunit').data('unitfk', res.ProductFkNavigation.ProductUnit);
                        temp.siblings('#btn').children('.btn-delete-quantity').data('id', 0);
                        temp.siblings('#tax').children('#chkTax').data('taxrate', res.TaxRate);
                        for (var i = 0; i < res.IcInventoryCustomerPrices.length; i++)
                            if (res.IcInventoryCustomerPrices[i].CustomerFk == customerFk) {
                                temp.siblings('#unitprice').children('#txtUnitPrice').val(general.toMoney(res.IcInventoryCustomerPrices[i].UnitPrice));
                                break;
                            }
                        temp.siblings('#productqty').children('#txtProductQty').val(0);
                        temp.siblings('#subtotal').text('');
                        temp.siblings('#tax').children('#chkTax').prop('checked', false);
                        temp.siblings('#taxrate').children('#txtTaxRate').val('');
                        temp.siblings('#taxAmount').text('');
                        temp.siblings('#subtotalAfterTax').text('');
                        $('.contain-productCode').hide();
                        UpdateStt();
                    },
                    error: function (err) {

                    }
                });
            }
            else {
                general.notify('Sản phẩm đã được chọn!', 'error');
                that.parent('.contain-productCode').siblings('.code').val(productCodeOld);
                return;
            } 

        });
        $('#txtCustomerName').on('change', function () {
            var t = $(this).val();
            if (t != '')
                $('#btn-add-product').prop('disabled', false);
        });
        $('body').on('change', '#txtProductQty', function () {
            var qty = general.toInt($(this).val());
            if (qty > 0) {
                var price = general.toInt($(this).parent('#productqty').siblings('#unitprice').children('#txtUnitPrice').val());
                var total = qty * price;
                $(this).parent('#productqty').siblings('#subtotal').text(general.toMoney(total));
                var taxrate = general.toFloat($(this).parent('#productqty').siblings('#taxrate').children('#txtTaxRate').val());
                var taxamount = taxrate * total/100;
                var totalAfterTax = total + taxamount;
                $(this).parent('#productqty').siblings('#subtotalAfterTax').text(general.toMoney(totalAfterTax));
                $(this).parent('#productqty').siblings('#taxAmount').text(general.toMoney(taxamount));
                UpdateTotal();
            }
            else
                $(this).val(0);
        });
        $('body').on('click', '.btn-delete-product', function () {
            var id = general.toInt($(this).data('id'));
            if (id > 0)
                listDelete.push(id);
            $(this).closest('tr').remove();
            UpdateTotal();
            UpdateStt();
        });
        $('body').on('change', '#chkTax', function () {
            var subtotal = general.toInt($(this).parent('#tax').siblings('#subtotal').text());
            if ($(this).is(':checked')) {
                var taxrate = general.toFloat($(this).data('taxrate'));
                $(this).parent('#tax').siblings('#taxrate').children('#txtTaxRate').val(taxrate);
                var taxamount = Math.round(subtotal * taxrate / 100);
                $(this).parent('#tax').siblings('#taxAmount').text(general.toMoney(taxamount));
                $(this).parent('#tax').siblings('#subtotalAfterTax').text(general.toMoney(subtotal + taxamount));
                UpdateTotal();
            }
            else {
                $(this).parent('#tax').siblings('#taxrate').children('#txtTaxRate').val('');
                $(this).parent('#tax').siblings('#taxAmount').text('');
                $(this).parent('#tax').siblings('#subtotalAfterTax').text(general.toMoney(subtotal));
                UpdateTotal();
            }
        });
        $('body').on('change', '#txtTaxRate', function () {
            var taxrate = general.toFloat($(this).val());
            var subtotal = general.toInt($(this).parent('#taxrate').siblings('#subtotal').text());
            var taxamount = Math.round(subtotal * taxrate / 100);
            $(this).parent('#taxrate').siblings('#taxAmount').text(general.toMoney(taxamount));
            $(this).parent('#taxrate').siblings('#subtotalAfterTax').text(general.toMoney(subtotal + taxamount));
            UpdateTotal();
        });
        $('#btnSave').on('click', function () {
            getOrderDetail(function (listod,listInv) {
                var customerFk = $('#txtCustomerName').data('keyidcus');
                var addressNumber = $('#txtAddressNumber').val();
                var street = $('#txtStreet').val();
                var ward = $('#txtWard').val();
                var district = $('#txtDistrict').val();
                var cityFk = $('#selCity option:selected').val();
                var subtotal = general.toInt($('#txtSubTotal').text());
                var taxAmount = general.toInt($('#txtTaxAmount').text());
                var totalOrder = general.toInt($('#txtTotalOrder').text());
                var data = {
                    KeyId: gId,
                    CustomerFK: customerFk,
                    AddressNumber: addressNumber,
                    Street: street,
                    ward: ward,
                    District: district,
                    City: cityFk,
                    TaxAmount: taxAmount,
                    Subtotal: subtotal,
                    TotalOrder: totalOrder,
                    SoOrderDetail: listod,
                    SostatusFk: general.soStatusWood.create
                };
                var list = {
                    SO_SalesOrderVm: data,
                    listDelete: listDelete,
                    listInventory: listInv
                }
                $.ajax({
                    type: 'POST',
                    url: '/WoodSalesOrder/SaveEntity',
                    data: { ItemVm: list },
                    success: function (res) {
                        general.notify('Ghi thành công!', 'success');
                        $('#Page-searchSO').show();
                        $('#Page-mainSO').hide();
                        loadData();
                    },
                    error: function (err) {
                        general.notify('Có lỗi trong khi ghi!', 'error');
                    }
                });
            });
        });
        $('#btnConfirm').on('click', function () {
            var listInv = [];
            $('#table-quantity-content tr').each(function () {
                var inventoryFk = $(this).children('.col-productCode').children('.autocomplete').children('#txtProductCode').data('keyid');
                var qtyOrdered = general.toInt($(this).children('#productqty').children('#txtProductQty').val());
                var inventory = {
                    Id: inventoryFk,
                    Qty: qtyOrdered
                };

                listInv.push(inventory);
            });
            $.ajax({
                type: 'POST',
                url: '/WoodSalesOrder/ConfirmSO',
                data: { SOId: gId, list: listInv },
                success: function (res) {
                    general.notify('Xác nhận thành công!', 'success');
                    $('#Page-searchSO').show();
                    $('#Page-mainSO').hide();
                    loadData();
                },
                error: function (err) {
                    general.notify('Có lỗi khi xác nhận!', 'error');
                }
            });
        });
        //----------------end event button main ---------------------
        $(document).click(function (e) {
            if (!$(e.target).is('.contain-productCode')) {
                $('.contain-productCode').hide();
            }
        });


        function datarefresh(callback) {
            loadData();
            boq1.reloadProject();
            if (callback != undefined)
                callback();
        }
    }
    function setSoStatusFk(statusid, callback) {
        var kt = false;
        for (var i = 0; i < glistOrderStatus.length; i++)
            if (glistOrderStatus[i].KeyId == statusid) {
                gstatusid = statusid;
                gstatusName = glistOrderStatus[i].SOStatusName;
                kt = true;
                callback();
                break;
            }
        if (kt == false) {
            general.notify('Chưa tìm thấy trạng thái !');
        }
        return false;
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
            url: '/WoodSalesOrder/GetAllPaging',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        stt: i + 1,
                        KeyId: item.KeyId,
                        DateCreated: general.dateFormatJson(item.DateCreated, true),
                        CustomerName: item.CustomerFKNavigation.UserBy.FullName,
                        Total: general.toMoney(item.TotalOrder),
                        StatusName: item.SostatusFkNavigation.SOStatusName
                    });
                    $('#lblTotalRecords').text(response.RowCount);
                    if (render != '') {
                        $('#tbl-content_search').html(render);
                    }
                    else
                        $('#tbl-content_search tr').forEach(function () {
                            $(this).remove();
                        });
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
    function resetForm() {
        $('#txtSOId').val(0);
        var now = new Date();
        $('#dtDateCreated').val(general.dateFormatJson(now, true));
        var employee = $('#user').text();
        $('#txtEmployee').val(employee);
        $('#txtCustomerName').val('');
        $('#txtCustomerName').data('keyidcus', '');
        $('#txtPhoneNumber').val('');
        $('#txtAddressNumber').val('');
        $('#txtStreet').val('');
        $('#txtWard').val('');
        $('#txtDistrict').val('');
        $('#txtCity').val('');
        $('#selCity').val('');
        $('#txtSubTotal').text('');
        $('#txtTaxAmount').text('');
        $('#txtTotalOrder').text('');

        $('#table-quantity-content tr').each(function () {
            $(this).remove();
        });

        $('#btnConfirm').hide();
        $('#btnUnConfirm').hide();
        $('#btnSave').hide();
        $('#btnCancel').hide();
        gstatusName = '';
    }
    function loadCustomer(callback) {
        var template = $('#customer-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txtCustomerName').val()
            },
            url: '/salesorder/GetAllCustomer',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        KeyId: item.KeyId,
                        FullName: item.UserBy.FullName,
                        Id: item.Id
                    });

                });
                if (render != '') {
                    $('#contain-listcustomer').html(render);
                }
                callback(render);
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    function loadCity() {
        $('#selCity option').remove();
        $.ajax({
            type: 'GET',
            url: '/employee/GetAllCity',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $('#selCity').append("<option value=''>Chọn tỉnh/TP</option>");
                $.each(response, function (i, item) {
                    $('#selCity').append("<option value='" + item.KeyId + "'>" + item.CityName + "</option>");
                });
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    function loadProduct(key, temp, callback) {
        general.notify('Đang tìm kiếm', 'success');
        var template = $('#productCode-template').html();
        var render = "";
        var a = temp.siblings('.contain-productCode');
        $.ajax({
            type: 'GET',
            url: '/WoodInventory/GetAll',
            data: { keyword: key },
            dataType: 'json',
            success: function (response) {
                console.log(response);
                //listProduct = response;
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        KeyId: item.KeyId,
                        ProductCode: item.ProductFkNavigation.ProductCode,
                        ProductName: item.ProductFkNavigation.ProductName,
                        Qty_In_Stock: item.Qty_In_Stock,
                        Qty_Allocated: item.Qty_Allocated,
                        WarehouseName: item.WarehouseFkNavigation.WarehouseName
                    });

                });
                if (render != '') {
                    a.html(render);
                }
                callback(render);
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
    function UpdateTotal() {
        var s = 0;
        var sTax = 0;
        var stotal = 0;
        $('#table-quantity-content tr').each(function () {
            var subtotal = general.toInt($(this).children('#subtotal').text());
            var taxamount = general.toInt($(this).children('#taxAmount').text());
            var subtotalAfterTax = general.toInt($(this).children('#subtotalAfterTax').text());
            s += subtotal;
            sTax += taxamount;
            stotal += subtotalAfterTax;
        });
        $('#txtSubTotal').text(general.toMoney(s));
        $('#txtTaxAmount').text(general.toMoney(sTax));
        $('#txtTotalOrder').text(general.toMoney(stotal));
    }
    function UpdateStt() {
        var stt = 1;
        $('#table-quantity-content tr').each(function () {
            $(this).children('#stt').text(stt);
            stt++;
        });
    }
    function loadCity() {
        $('#selCity option').remove();
        $.ajax({
            type: 'GET',
            url: '/employee/GetAllCity',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                $('#selCity').append("<option value=''>Chọn tỉnh/TP</option>");
                $.each(response, function (i, item) {
                    $('#selCity').append("<option value='" + item.KeyId + "'>" + item.CityName + "</option>");
                });
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    function getOrderDetail(callback) {
        var listDetail = [];
        var listInventory = [];
        var i = 0;
        $('#table-quantity-content tr').each(function () {
            var inventoryFk = $(this).children('.col-productCode').children('.autocomplete').children('#txtProductCode').data('keyid');
            var unitFk = $(this).children('#productunit').data('unitfk');
            var unitPrice = general.toInt($(this).children('#unitprice').children('#txtUnitPrice').val());
            var qtyOrdered = general.toInt($(this).children('#productqty').children('#txtProductQty').val());
            var qtyOld = general.toInt($(this).children('#productqty').children('#txtProductQty').data('qty'));
            var subtotal = general.toInt($(this).children('#subtotal').text());
            var taxrate = $(this).children('#tax').children('#chkTax').data('taxrate');
            var taxamount = general.toInt($(this).children('#taxAmount').text());
            var keyid = general.toInt($(this).children('#btn').children('.btn-delete-product').data('id'));
            var obj = {
                KeyId: keyid,
                InventoryFk: inventoryFk,
                SoFk: gId,
                UnitFk: unitFk,
                UnitPrice: unitPrice,
                QtyOrdered: qtyOrdered,
                Subtotal: subtotal,
                TaxRate: taxrate,
                Tax: taxamount
            };
            var inventory = {
                Id: inventoryFk,
                Qty: qtyOrdered - qtyOld
            };
            i++;
            listDetail.push(obj);
            listInventory.push(inventory);
        });
        if (i == $('#table-quantity-content tr').length)
            callback(listDetail, listInventory);
    }
    function loadbyid(SalesOrderId, isView) {
        if (SalesOrderId != 0) {
            general.startLoad();
            getValue(SalesOrderId, isView, function () {
                getDetail(SalesOrderId, function (listOD) {
                    loadDetail(listOD);
                });
            });
        }
        else {
            setSoStatusFk(general.soStatus.create, function () {
                controlButtonByStatusKF(SalesOrderId, gstatusid, isView);
            });
        }
    }
    function getValue(id, Isview, callback) {
        if (id != 0) {
            $.ajax({
                type: 'GET',
                url: '/WoodSalesOrder/GetById',
                data: { id: id },
                async: false,
                success: function (response) {
                    console.log(response);
                    if (response != null) {
                        var data = response;
                        callback();
                        gId = data.KeyId;
                        setSoStatusFk(data.SostatusFk, function () {
                            controlButtonByStatusKF(id, gstatusid, Isview);
                        });
                        $('#dtDateCreated').val(general.dateFormatJson(data.DateCreated, true));
                        $('#txtSOId').val(data.KeyId);
                        $('#txtEmployee').val(data.CreatedByFkNavigation.FullName);
                        $('#txtCustomerName').val(data.CustomerFKNavigation.UserBy.FullName);
                        $('#txtCustomerName').data('keyidcus', data.CustomerFK);
                        $('#txtPhoneNumber').val();
                        $('#txtAddressNumber').val(data.AddressNumber);
                        $('#txtStreet').val(data.Street);
                        $('#txtWard').val(data.Ward);
                        $('#txtDistrict').val(data.District);
                        $('#selCity').val(data.City);
                        $('#txtSubTotal').text(general.toMoney(data.Subtotal));
                        $('#txtTaxAmount').text(general.toMoney(data.TaxAmount));
                        $('#txtTotalOrder').text(general.toMoney(data.TotalOrder));
                    }
                    else {
                        general.notify('Lỗi ! Đã gửi phản hồi về lỗi !', 'error');
                    }
                },
                error: function (error) {
                    $('#Page-searchBoq1').show();
                    $('#Page-mainBoq1').hide();
                    return false;
                }
            });
        }

    }
    function getDetail(id, callback) {

        if (id != 0) {
            $.ajax({
                type: 'GET',
                url: '/WoodSalesOrder/GetListDetailWithSoid',
                data: { Soid: id },
                success: function (response) {
                    console.log(response);
                    callback(response);
                },
                error: function (error) {
                    general.notify('Có lỗi khi tải dữ liệu', 'error');
                }
            });
        }
    }
    function loadDetail(listOD) {
        var template = $('#template-table-detail').html();
        var render = '';
        var i = 1;
        listOD.forEach(function (item) {
            render += Mustache.render(template, {
                stt: i,
                KeyId: item.KeyId,
                ProductCode: item.InventoryFKNavigation.ProductFkNavigation.ProductCode,
                ProductName: item.InventoryFKNavigation.ProductFkNavigation.ProductName,
                InventoryFk: item.InventoryFK,
                UnitName: item.UnitFkNavigation.UnitName,
                UnitFk: item.UnitFk,
                SubTotal: general.toMoney(item.Subtotal),
                ProductQty: item.QtyOrdered,
                UnitPrice: general.toMoney(item.UnitPrice),
                Tax:item.TaxRate,
                TaxRate: item.Tax>0?item.TaxRate:'',
                TaxAmount: general.toMoney(item.Tax),
                SubTotalAfterTax: general.toMoney(item.Subtotal + item.Tax)
            });
            i++;
        });
        count = listOD.length + 1;
        $('#table-quantity-content').html(render);
        $('#table-quantity-content tr').each(function () {
            var tax = general.toFloat($(this).children('#taxrate').children('#txtTaxRate').val());
            if (tax > 0)
                $(this).children('#tax').children('#chkTax').prop('checked', true);
        });
        general.stopLoad();
    }
    function controlButtonByStatusKF(id, SOstatus, IsView) {
        if (id > 0) {
            if (IsView) {
                $('#btn-export').show();
                $('#btn-print').show();
                // có quyền xác nhận
                if (SOstatus != general.soStatusWood.Confirmed) {
                    $('#btnConfirm').show();
                    if (SOstatus != general.soStatus.UnConfirmed)
                        $('#btnUnConfirm').show();
                }
                $('tfoot').hide();
            }
            else {
                if (SOstatus != general.soStatusWood.Confirmed) {
                    $('#btn-import').show();

                    $('#btnSave').show();
                    $('#btnCancel').show();
                    $('tfoot').show();
                }
            }


        }
        else //tạo mới
        {
            // 
            var btn = $('#btn-import');
            $('#btnSave').show();
            $('#btnCancel').show();
        }
    }

    //load status
    function loadOrderStatus() {
        glistOrderStatus = [];
        $.ajax({
            type: 'GET',
            url: '/WoodSalesOrder/GetListStatus',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                if (response.length > 0) {
                    glistOrderStatus = response;
                }
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    var delayStatus = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearInterval(timer)
            timer = setInterval(callback, ms);
        };
    })();
}

