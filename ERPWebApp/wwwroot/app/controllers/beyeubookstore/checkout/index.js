var checkOutController = function () {
    this.initialize = function () {
        registerEvents();
    }
}

function registerEvents() {
    $('#btnCheckOutHolder').on('click', function () {
        if (document.getElementById('btnCheckOut').disabled) {
            general.notify("Bạn phải chọn phương thức thanh toán!", "error");
        }
    });
    $('body').on('click', '#btnCheckOut', function () {
      
        if (Validated()) {
            var total = 0;
            $.ajax({
                url: '/Cart/GetCart',
                dataType: 'json',
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (respond) {
                    var ListInvoiceDetail = [];
                    $.each(respond, function (i, item) {
                        var InvoiceDetail = new Object();
                        InvoiceDetail.InvoiceFK = 0;
                        InvoiceDetail.BookFK = item.Book.KeyId;
                        InvoiceDetail.Qty = item.Quantity;
                        InvoiceDetail.UnitPrice = item.UnitPrice;
                        InvoiceDetail.MerchantFK = item.Book.MerchantFK,
                        InvoiceDetail.SubTotal = item.Quantity * item.UnitPrice;

                        total += item.Quantity * item.UnitPrice;

                        ListInvoiceDetail.push(InvoiceDetail);
                    })

                    var Invoice = new Object();
                    Invoice.CustomerFK = 0;
                    Invoice.TotalPrice = total;
                    Invoice.DeliContactName = $("#txtDeliName").val();
                    Invoice.DeliAddress = $("#txtDeliAddress").val();
                    Invoice.DeliContactHotline = $("#txtDeliPhone").val();

                    $.ajax({
                        type: 'POST',
                        url: '/Invoice/SaveEntity',
                        data: {
                            invoiceVm: Invoice,
                            invoiceDetailVms: ListInvoiceDetail,
                        },
                        success: function (respond) {
                            switch (respond) {
                                case "true":
                                    general.notify("Đặt hàng thành công", 'success');
                                    window.location.href = "/";
                                    break;
                                case "customer":
                                    general.notify("Đặt hàng thất bại, vui lòng đăng nhập", 'error');
                                    break;
                                case "quantity":
                                    general.notify("Đặt hàng thất bại, vì có vật phẩm quá với số lượng tồn", 'error');
                                    window.location.href = "/Cart";
                                    break;
                            }
                            general.stopLoad();
                        },
                        error: function () {
                            general.notify("Đặt hàng thất bại", error);
                        }
                    })
                    general.stopLoad();
                }
            })
        }
    })
}

function Validated() {
    var validated = true;
    var phoneRegex = /^0+[0-9]{9}$/;
    if ($("#txtDeliPhone").val() != "") {
        if ($('#txtDeliPhone').val().match(phoneRegex) == null) {
            $('#txtPhoneMessage').text("Số điện thoại sai");
            validated = false;
        }
    }
    return validated;
}