var salesorderController = function () {

    this.initialize = function () {
        loadUnit();
        registerEvents();
    }
    var cachedObj = {
        units: []
    };
    var discountAmt = false;
    var listInventory;
    var paidAmount;
    function registerEvents() {

        
        $('#txtId').prop('readonly', true);
        $('#txtId').val(0);
        $('#txtTotal').prop('readonly', true);
        $('#txtFinalTotal').prop('readonly', true);
        $('#txtCustomerPay').prop('readonly', true);
        $('#txtReturn').prop('readonly', true);
        $('#txtFreight').prop('readonly', true);
        $('#dtDateCreated').prop('readonly', true);
        $('#chkDirection').prop('checked', true);
        var now = new Date();
        $('#CustomerPay').hide();
        $('#Return').hide();
        $('#dtDateCreated').val(general.dateFormatJson(now), true)
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtTax: {
                    number: true
                },
                txtDiscountAmt: {
                    number: true
                },
                txtFreight: {
                    number: true
                },
                txtCustomerName: {
                    required: true
                }
            }
        });
        $('#frmDiscount').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtDiscount: {
                    number:true
                }
            }
        });
        $('#txtKeyword').on('keyup', function (e) {
            if ($('#txtKeyword').val() != "") {
                loadInventory(function (render) {
                    if(render!="")
                        $('#contain-listproduct').show();
                });
            }
            else 
                $('#contain-listproduct').hide();
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
        var count = 1;
        $('#contain-listproduct').on('click', '#ddlrow', function (e) {
            var template = $('#product-template').html();
            var render = "";
            var that = $(this).data('id');
            var flag = 0;
            $.ajax({
                type: 'GET',
                data: {
                    id: that
                },
                url: '/salesorder/GetInventoryById',
                dataType: 'json',
                success: function (response) {
                    console.log(response);
                    var item = response;
                    $(".product-cart-list .row-product").each(function () {
                        var id = $(this).children(".contain-btn").children(".btn-delete").data("id1");
                        if (id === item.KeyId) {
                            flag = 1;
                            var t = parseInt($(this).children(".contain-qty").children("#qty").val());
                            $(this).children(".contain-qty").children("#qty").val(t + 1);
                            var unitprice = general.ignoreCommaNumber($(this).children(".contain-price").children("#unitPrice").val());
                            var temp = (t + 1) * unitprice;
                            $(this).children("#subtotal").text(general.commaSeparateNumber(temp));
                            sum(unitprice);
                            $('#contain-listproduct').hide();
                            //break;
                        }
                    });
                    if (flag == 0) {
                        render = Mustache.render(template, {
                            KeyId: item.KeyId,
                            Id: count,
                            ProductRef: item.ProductFkNavigation.ProductRef,
                            ProductName: item.ProductFkNavigation.ProductName,
                            UnitFK: getUnitOptions(item.ProductFkNavigation.ProductUnit, item.ProductFkNavigation.ProductUnitNavigation.UnitCategoryFk),
                            UnitPrice: general.commaSeparateNumber(item.UnitPrice),
                            Qty: 1,
                            SubTotal: general.commaSeparateNumber(item.UnitPrice)
                        });
                        if (render != '') {
                            $('.product-cart-list').append(render);
                            sum(item.UnitPrice);
                            $('#contain-listproduct').hide();
                            $('#txtKeyword').val('');
                        }
                        countSTT();
                    }
                    
                },
                error: function (status) {
                    console.log(status);
                    general.notify('Không thể load dữ liệu', 'error');
                }
            });
        });


        $("#chkShip").on("change", function () {
            if (this.checked) {
                $(".position-ship").addClass("ship");
            }
            else {
                $(".position-ship").removeClass("ship");
            }
        });
        $(".position-ship").on("click", function () {
            loadCity();
            if ($("#chkShip").is(':checked'))
                $('#modal-add-edit-shipping').modal('show');
        });
        var qtybefore;
        $("body").on("focusin", "#qty", function () {
            qtybefore = general.ignoreCommaNumber($(this).val());
        });
        $("body").on("change", "#qty", function () {
            var inventory = $(this).parent(".contain-qty").parent(".row-product").data("id3");
            var qty = general.ignoreCommaNumber($(this).val());
            var unitprice = general.ignoreCommaNumber($(this).parent(".contain-qty").siblings(".contain-price").children("#unitPrice").val());
            var temp = qty * unitprice;
            $(this).parent(".contain-qty").siblings("#subtotal").text(general.commaSeparateNumber(temp));
            var r = qtybefore - qty;
            var p = r * unitprice;
            sub(p);
            if (r < 0)//số lượng tăng lên
            {
                //var p = -(r * unitprice);
                //sum(p);
                listInventory.forEach(function (item) {
                    if (item.KeyId == inventory) {
                        if (qty > item.Qty_In_Stock) {
                            general.notify("Khối lượng đặt đã vượt quá tồn kho!","error");
                            $(this).val(10);
                        }
                    }
                });
            }
            qtybefore = qty;
            //sum(temp);
        });
        
        $("body").on("click", ".btn-delete", function (e) {
            $(this).parent(".contain-btn").parent(".row-product").remove();
            var subtotal = general.ignoreCommaNumber(($(this).parent(".contain-btn").siblings("#subtotal").text()));
            sub(subtotal);
            countSTT();
        });

        $("body").on("click", "#btnDel", function (e) {
            $(this).parent(".contain-btn").parent(".row-payment").remove();
        });

        $(".btn-add").on("click", function (e) {
            $('#modal-add-edit').modal('show');
        });
        var ship = {
            NameShipTo: "",
            PhoneNumber: "",
            AddressNumber: "",
            Street:"",
            Ward: "",
            District: "",
            City: "",
            ShipperFK: "",
            Freight: "",
            ShipNo: "",
            ShipDate: "",
            StatusShip: "",
            Note: ""
        };
        $("#btnSaveShip").on("click", function () {
            ship.NameShipTo = $("#txtReceiver").val();
            ship.PhoneNumber = $("#txtPhoneNumber").val();
            ship.AddressNumber = $("#txtAddress").val();
            ship.Street = $("#txtStreet").val();
            ship.Ward = $("#txtWard").val();
            ship.District = $("#txtDistrict").val();
            ship.City = $("#selCity option:selected").val();
            ship.ShipperFK = $("#selShipper option:selected").val();
            ship.Freight = $("#txtFreight1").val();
            ship.ShipNo = $("#txtShipNo").val();
            ship.ShipDate = $("#dtShipDate").val();
            ship.StatusShip = $("#selStatusShip option:selected").val();
            ship.Note = $("#txtNotes").val();
            $('#modal-add-edit-shipping').modal('hide');
            $('#txtFreight').val(general.commaSeparateNumber(ship.Freight));
        });
        $('#txtDiscount').keyup(function (event) {

            // skip for arrow keys
            if (event.which >= 37 && event.which <= 40) return;

            // format number
            $(this).val(function (index, value) {
                return value
                    .replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    ;
            });
        });
        $('#txtSubtotal').keyup(function (event) {

            // skip for arrow keys
            if (event.which >= 37 && event.which <= 40) return;

            // format number
            $(this).val(function (index, value) {
                return value
                    .replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    ;
            });
        });
        $('.btn-close').on('click', function () {
            if($('#txtFinalTotal').val()!='')
                $('#yesno-modal').modal('show');
        });
        $('#btnYes').on('click', function () {
            resetFormMaintainance();
            $(".product-cart-list .row-product").each(function () {
                $(this).remove();
            });
            $(".position-ship").removeClass("ship");
            $('#yesno-modal').modal('hide');
        });
        //Save data to database
        var freightbefore = general.ignoreCommaNumber($("#txtFreight1").val());
        $("#txtFreight1").on("change", function () {
            var t1 = $('#txtFinalTotal').val() == "" ? 0 : general.ignoreCommaNumber($('#txtFinalTotal').val());
            var freight = general.ignoreCommaNumber($(this).val());
            var r = freightbefore - freight;
            $('#txtFinalTotal').val(general.commaSeparateNumber(t1 - r));
            freightbefore = freight;
        });
        $("#btnSave").on("click", function () {
            general.startLoading();
            if ($('#frmMaintainance').valid()) {
                var customerFK = $("#txtCustomerName").data("keyidcus");
                var subtotal = general.ignoreCommaNumber($("#txtTotal").val());
                var discountAmt = general.ignoreCommaNumber($("#txtDiscountAmt").val());
                var totalorder = general.ignoreCommaNumber($("#txtFinalTotal").val());
                //var lastupdatedBy = $('#user').data('userid');
                //var createdBy = $('#user').data('userid');
                var keyId = $("#txtId").val();
                var note = $("#txtNote").val();
                var arr = [];
                var pickticketDetail = [];
                var inventoriesList = [];
                var invoiceDetail = [];
                $(".product-cart-list .row-product").each(function () {
                    var inventoryFK = $(this).children(".contain-btn").children(".btn-delete").data("id1");
                    var unitFk = $(this).children(".unitFK").children('.ddlUnitId').find('option:selected').val();
                    var qtyOrdered = general.ignoreCommaNumber($(this).children(".contain-qty").children("#qty").val());
                    var disAmount;
                    var disPercent;
                    var oldPrice = general.ignoreCommaNumber($(this).children(".contain-price").data('price'));
                    var discount = $(this).children(".contain-price").children(".discount").text().substr(1);
                    if (discount.substr(discount.length - 1) == "%") {
                        discount = discount.slice(0, -1);
                        disPercent = discount;
                        disAmount = oldPrice * parseInt(discount)/100;
                    }
                    else {
                        discount = general.ignoreCommaNumber(discount);
                        disAmount = discount;
                        disPercent = discount / oldPrice*100;
                    }
                    var temp = {
                        SoFk: keyId,
                        InventoryFK: inventoryFK,
                        UnitFk: unitFk,
                        UnitPrice: general.ignoreCommaNumber($(this).children(".contain-price").children("#unitPrice").val()),
                        QtyOrdered: qtyOrdered,
                        Subtotal: general.ignoreCommaNumber($(this).children("#subtotal").text()),
                        DiscountPercent: disPercent,
                        Discount: disAmount

                    }
                    arr.push(temp);
                    var temp1 = {
                        InventoryFK: inventoryFK,
                        UnitFK: unitFk,
                        QtyOrderedNow: qtyOrdered,
                        QtyPicked: qtyOrdered,
                        QtyPickedShipped: qtyOrdered
                    }
                    pickticketDetail.push(temp1);
                    var temp2 = {
                        KeyId: inventoryFK,
                        Qty_In_Stock: qtyOrdered
                    }
                    inventoriesList.push(temp2);
                    var temp3 = {
                        Description: "",
                        InventoryFk: inventoryFK,
                        UnitFk: unitFk,
                        UnitPrice: general.ignoreCommaNumber($(this).children(".contain-price").children("#unitPrice").val()),
                        QtyOrderedNow: qtyOrdered,
                        QtyShipped: qtyOrdered,
                        QtyBackOrdered:0,
                        Subtotal: general.ignoreCommaNumber($(this).children("#subtotal").text()),
                        DiscountPercent:disPercent,
                        Discount: disAmount,
                        TaxRate:"",
                        Tax:""
                    }
                    invoiceDetail.push(temp3);
                });
                var pickticket = {
                    PickClerkNo: '',
                    SO_FK: keyId,
                    ShipDate: ship.ShipDate,
                    ShippingClerkNo: '',
                    Freight: ship.Freight,
                    StatusShip: ship.StatusShip,
                    ShipNo: ship.ShipNo,
                    Note: ship.Note,
                    SoPickTicketDetails: pickticketDetail
                };
                var arr1 = [];
                arr1.push(pickticket);
                var invoice = {
                    SoId: keyId,
                    CustomerFK: customerFK,
                    Description: note,
                    PaymentTermFk: "",
                    SalespersonNo: lastupdatedBy,
                    SalesAmount: subtotal,
                    Freight: ship.Freight,
                    Discount: discountAmt,
                    TotalTax: "",
                    Amount: subtotal,
                    TotalInvoice: totalorder,
                    PaidAmount: paidAmount,
                    ArInvoiceDetail: invoiceDetail
                };
                var arr2 = [];
                arr2.push(invoice);
                var data = {
                    KeyId: keyId,
                    CustomerFK: customerFK,
                    Subtotal: subtotal,
                    DiscountAmt: discountAmt,
                    TotalOrder: totalorder,
                    //LastUpdatedByFk: lastupdatedBy,
                    //CreatedByFk: lastupdatedBy,
                    Description: note,
                    NameShipTo: ship.NameShipTo,
                    AddressNumber: ship.AddressNumber,
                    Street: ship.Street,
                    Ward: ship.Ward,
                    District: ship.District,
                    City: ship.City,
                    Freight: ship.Freight,
                    SoOrderDetail: arr,
                    SoPickTickets: arr1,
                    ArInvoices: arr2
                }
                Save(data, inventoriesList, function (response) {
                    getIdInvoice(response, function (res) {
                        var salesEvent = {
                            InvoiceFk: res.KeyId,
                            InvoiceDate: res.DateCreated,
                            CustomerAddressBookFk: customerFK,
                            SalesPersonNo: lastupdatedBy,
                            SalesAmount: subtotal,
                            Freight: ship.Freight,
                            Discount: discountAmt,
                            TaxAmount: "",
                            TotalInvoice: totalorder
                        };
                        var accountReceivable = {
                            TransactNo: res.KeyId,
                            Transaction_Date: res.DateCreated,
                            ArNo: "131",
                            CustomerFk: customerFK,
                            Amount: totalorder,
                            Type: general.code.Invoice
                        };
                        var generalJournal = {
                            DebitAccount: "131",
                            CreditAccount: "511",
                            Description: "Doanh thu",
                            Reference: res.KeyId,
                            ReferenceDate: res.DateCreated,
                            Status: 0,
                            Amount: subtotal,
                            Type: general.code.Invoice
                        };
                        var CRAccount = {
                            DebitAccount:'111',
                            CreditAccount:'131',
                            Amount: paidAmount
                            
                        }; 
                        var arrCRAccount = [];
                        arrCRAccount.push(CRAccount);
                        var CRDetail = {
                            InvoiceNo: res.KeyId,
                            Invoice_Date: res.DateCreated,
                            InvoiceAmount: totalorder,
                            AccumulatedLastAmountAcc: '',
                            CollectedAmount: paidAmount,
                            RemainedAmount: totalorder - paidAmount
                        };
                        var arrCRDetail = [];
                        arrCRDetail.push(CRDetail);
                        var cashReceipt = {
                            CustomerFk: customerFK,
                            Amount: paidAmount,
                            CreatedByFk: lastupdatedBy,
                            LastUpdatedByFk: lastupdatedBy,
                            Payment: true,
                            CrCashReceiptsAccountDetail: arrCRAccount,
                            CrCashReceiptsDetail: arrCRDetail

                        };
                        saveSalesEvent(salesEvent);
                        saveAccountReceivable(accountReceivable);
                        saveGeneralJournal(generalJournal);
                        saveCashReceipt(cashReceipt, function (response) {
                            console.log(response);
                            var accountReceivableCR = {
                                TransactNo: response.KeyId,
                                Transaction_Date:response.DateCreated,
                                ArNo: "131",
                                CustomerFk: customerFK,
                                Amount: response.Amount,
                                Type: general.code.CashReceipt
                            };
                            var generalJournalCR = {
                                DebitAccount: "111",
                                CreditAccount: "131",
                                Description: "Thu tiền",
                                Reference: response.KeyId,
                                ReferenceDate: response.DateCreated,
                                Status: 0,
                                Amount: response.Amount,
                                Type: general.code.CashReceipt
                            };
                            saveAccountReceivable(accountReceivableCR);
                            saveGeneralJournal(generalJournalCR);
                        });
                        $(".product-cart-list .row-product").each(function () {
                            $(this).remove();
                        });
                        $(".position-ship").removeClass("ship");
                    });
                });
            }
            general.stopLoading();
        });
        $('#calculator').on('click', function () {
            $('#TotalNeedPay').text($('#txtFinalTotal').val());
            $('#modal-add-edit-calculator').modal('show');
        });
        //Payment
        
        $('#selPaymentType').on('change', function () {
            var subtotal = general.ignoreCommaNumber($('#txtSubtotal').val());
            var paymentType = $('#selPaymentType option:selected').text();
            var a = $('#selPaymentType option:selected').val();
            if (a != "") {
                var template = $('#payment-template').html();
                var render = "";
                var flag = 0;
                $('.contain-paid .row-payment').each(function () {
                    var id = $(this).data('id3');
                    if (id == a)
                        flag = 1;
                });
                if (flag == 0) {
                    render = Mustache.render(template, {
                        PaymentType: paymentType,
                        SubTotal: subtotal,
                        KeyId: a
                    });
                    if (render != '') {
                        $('.contain-paid').append(render);
                    }
                    if (a == '1')
                        $('#selAccountNumber').hide();
                }
                
                var paid = general.ignoreCommaNumber($('#TotalPaid').val());
                $('#TotalPaid').text(general.commaSeparateNumber(paid + subtotal));
            }
            
        });
        $('#btnSavePayment').on('click', function () {
            if (isNaN(general.ignoreCommaNumber($('#TotalPaid').text()))) {
                paidAmount = general.ignoreCommaNumber($('#txtSubtotal').val());
                $('#txtCustomerPay').val($('#txtSubtotal').val());
            }
            else {
                paidAmount = general.ignoreCommaNumber($('#TotalPaid').text());
                $('#txtCustomerPay').val($('#TotalPaid').text());
            }
                
            var finalTotal = general.ignoreCommaNumber($('#txtFinalTotal').val());
            var r = paidAmount - finalTotal;
            if (paidAmount > finalTotal)
                paidAmount = finalTotal;
            
            $('#txtReturn').val(general.commaSeparateNumber(r));
            $('#CustomerPay').show();
            $('#Return').show();
            $('#modal-add-edit-calculator').modal('hide');
        });
        //Chiết khấu
        var unitPrice;
        var subtotal;
        var qty;
        var discount;
        var oldUnitPrice;
        $('body').on('click', '#unitPrice', function () {
            $('#discount-modal').modal('show');
            unitPrice = $(this);
            oldUnitPrice = $(this).parent('.contain-price');
            subtotal = $(this).parent('.contain-price').siblings('#subtotal');
            qty = $(this).parent('.contain-price').siblings('.contain-qty').children('#qty').val();
            discount = $(this).siblings('.discount');
        });
        $('#btnSaveDiscount').on('click', function () {
            if (discountAmt == true) {
                var dis = general.ignoreCommaNumber($('#txtDiscount').val());
                var total = general.ignoreCommaNumber($('#txtTotal').val());
                if ($('#rdoVND').is(':checked')) {
                    $('#txtDiscountAmt').val(general.commaSeparateNumber(dis));
                    var t = general.ignoreCommaNumber($('#txtFinalTotal').val());
                    $('#txtFinalTotal').val(general.commaSeparateNumber(t - dis));
                }
                else {
                    $('#txtDiscountAmt').val(general.commaSeparateNumber(dis*total/100));
                    var t = general.ignoreCommaNumber($('#txtFinalTotal').val());
                    $('#txtFinalTotal').val(general.commaSeparateNumber(t - dis * total / 100));
                }
                
            }
            else {
                var a = general.ignoreCommaNumber($('#txtDiscount').val());
                var b = general.ignoreCommaNumber(unitPrice.val());
                var oldPrice = general.ignoreCommaNumber(oldUnitPrice.data('price'));
                var priceAfter;
                var total = general.ignoreCommaNumber(subtotal.text());
                var totalAfter;
                if ($('#rdoVND').is(':checked')) {
                    priceAfter = oldPrice - a;
                    discount.text(general.commaSeparateNumber(-a));
                }
                else {
                    priceAfter = oldPrice - a * oldPrice / 100;
                    discount.text(-a+" %");
                }
                totalAfter = priceAfter * qty;
                unitPrice.val(general.commaSeparateNumber(priceAfter));
                subtotal.text(general.commaSeparateNumber(totalAfter));
                sub(total - totalAfter);
                
            //var t = general.ignoreCommaNumber($('#txtFinalTotal').val());
            //$('#txtFinalTotal').val(general.commaSeparateNumber(t - total + totalAfter));
            //var discount = general.ignoreCommaNumber($('#txtDiscountAmt').val());
            //$('#txtDiscountAmt').val(general.commaSeparateNumber(discount + total - totalAfter));
            }
            
            $('#discount-modal').modal('hide');
        });
        $('#txtDiscountAmt').on('click', function () {
            $('#discount-modal').modal('show');
            discountAmt = true;
        });
        $('#txtDiscount').on('input', function () {
            if ($('#rdoPercent').is(':checked')) {
                var value = $(this).val();

                if ((value !== '') && (value.indexOf('.') === -1)) {

                    $(this).val(Math.max(Math.min(value, 100), 0));
                }
            }
        });
        
    }
   
    function saveCashReceipt(data,callback) {
        $.ajax({
            type: 'POST',
            url: '/cashreceipts/SaveEntity',
            data: data,
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                callback(response);
                general.stopLoading();
            },
            error: function (error) {
                general.stopLoading();
            }
        });
    }
    function saveSalesEvent(data) {
        $.ajax({
            type: 'POST',
            url: '/salesevent/SaveEntity',
            data: data,
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                general.stopLoading();
            },
            error: function (error) {
                general.stopLoading();
            }
        });
    }
    function saveGeneralJournal(data) {
        $.ajax({
            type: 'POST',
            url: '/generaljournal/SaveEntity',
            data: data,
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                general.stopLoading();
            },
            error: function (error) {
                general.stopLoading();
            }
        });
    }
    function saveAccountReceivable(data) {
        $.ajax({
            type: 'POST',
            url: '/accountreceivable/SaveEntity',
            data: data,
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                general.stopLoading();
            },
            error: function (error) {
                general.stopLoading();
            }
        });
    }
    function getIdInvoice(SOId,callback) {
        $.ajax({
            type: "GET",
            url: "/invoice/GetInvoiceIdBySOID",
            data: { SOId },
            success: function (response) {
                console.log(response);
                callback(response);
            },
            error: function (error) {

            }
        });
    }
    function Save(data,inventoriesList,callback) {
        $.ajax({
            type: "POST",
            url: "/salesorder/SaveEntity",
            data: data,
            dataType: 'json',
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                callback(response);
                UpdateInventory(inventoriesList);
                general.notify('Ghi thành công!', 'success');
                resetFormMaintainance();
                general.stopLoading();
            },
            error: function () {
                general.notify('Có lỗi trong khi ghi xuống csdl!', 'error');
                general.stopLoading();
            }
        });
    }
    function UpdateInventory(list) {
        $.ajax({
            
            type: 'POST',
            url: "/inventory/UpdateQtyInStock",
            data: { inventories: list },
            dataType: 'json',
            success: function (response) {
                console.log(response);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

    function countSTT() {
        var stt = 1;
        var count = 0;
        $(".product-cart-list .row-product").each(function () {
            $(this).children(".stt").text(stt);
            stt++;
            count++;
        });
        if (count == 0) {
            resetFormMaintainance();
        }
    }
    function sum(subtotal) {
        var t = $('#txtTotal').val() == "" ? 0 : general.ignoreCommaNumber($('#txtTotal').val());
        var t1 = $('#txtFinalTotal').val() == "" ? 0 : general.ignoreCommaNumber($('#txtFinalTotal').val());
        $('#txtTotal').val(general.commaSeparateNumber(t + subtotal));
        $('#txtFinalTotal').val(general.commaSeparateNumber(t1 + subtotal));
    }
    function sub(subtotal) {
        var t = $('#txtTotal').val() == "" ? 0 : general.ignoreCommaNumber($('#txtTotal').val());
        var t1 = $('#txtFinalTotal').val() == "" ? 0 : general.ignoreCommaNumber($('#txtFinalTotal').val());
        $('#txtTotal').val(general.commaSeparateNumber(t - subtotal));
        $('#txtFinalTotal').val(general.commaSeparateNumber(t1 - subtotal));
    }
    function loadInventory(callback) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            data: {
                keyword: $('#txtKeyword').val()
            },
            url: '/salesorder/GetAllInventory',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                listInventory = response;
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        KeyId: item.KeyId,
                        ProductRef: item.ProductFkNavigation.ProductRef,
                        ProductName: item.ProductFkNavigation.ProductName,
                        UnitPrice: general.commaSeparateNumber(item.UnitPrice),
                        Image: item.ProductFkNavigation.Image,
                        Qty_In_Stock: general.commaSeparateNumber(item.Qty_In_Stock),
                        Qty_Allocated: general.commaSeparateNumber(item.Qty_Allocated)
                    });
                    if (render != '') {
                        $('#contain-listproduct').html(render);
                    }
                });
                callback(render);
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
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
    function resetFormMaintainance() {
        $("#txtId").val(0);
        var now = new Date();
        $('#dtDateCreated').val(general.dateFormatJson(now), true);
        $("#txtCustomerName").val("");
        $("#chkShip").prop("checked", false);
        $("#selPaymentTerm").val("");
        $("#txtTotal").val(0);
        $("#txtDiscountAmt").val("");
        $("#txtFinalTotal").val(0);
        $("#txtNote").val();
        $('#CustomerPay').hide();
        $('#Return').hide();
    }
    function loadUnit() {
        return $.ajax({
            type: "GET",
            url: "/unit/GetAll",
            dataType: "json",
            success: function (response) {
                cachedObj.units = response;
            },
            error: function () {
                tedu.notify('Có lỗi xảy ra', 'error');
            }
        });
    }
    function getUnitOptions(selectedId,unitType) {
        var units = "<select class='form-control ddlUnitId'>";
        $.each(cachedObj.units, function (i, unit) {
            if (unitType === unit.UnitCategoryFk) {
                if (selectedId === unit.KeyId)
                    units += '<option value="' + unit.KeyId + '" selected="select">' + unit.UnitName + '</option>';
                else
                    units += '<option value="' + unit.KeyId + '">' + unit.UnitName + '</option>';
            }
            
        });
        units += "</select>";
        return units;
    }
}