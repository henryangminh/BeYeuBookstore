var deliveryController = function () {
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


        $('#dtBegin').on('change', function () {
            loadData();
        });

        $('#dtEnd').on('change', function () {
            loadData();
        });

        $('#selStatus').on('change', function () {
            loadData();
        });

        //$('#modal-add-edit').on('hide', function () {
        //    resetForm();
        //});

        $('#btnCreate').on('click', function () {
            resetForm();
            $('#modal-add-edit').modal('show');

        });

        $('#txtKeyword').on('keyup', function (e) {
            if (e.keyCode === 13) {
                loadData();
            }
        });

        $('#btnCancel').on('click', function () {

            $('#frmMaintainance').trigger('reset');
        });
        //Reset Form

        //Edit

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadDetail(that);

        });

        //Validate
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                selRequestTo:
                {
                    required: true
                },
                seldetailTimeKeepingType:
                {
                    required: true
                },
                txtFromdate: {
                    required: true
                },
                txtTodate: {
                    required: true
                },
                txtReason: {
                    required: true
                }
            }
        });
        //Save 

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var keyId;
                if ($('#txtId').val() == "") {
                    keyId = 0;
                }
                else {
                    keyId = parseInt($('#txtId').val());
                }
                var status = $('#selDeliStatus option:selected').val();
                var orderPrice = general.toFloat($('#txtOrderPrice').val());
                var shipPrice = $('#txtShip').val();
                var note = $('#txtNote').val();
                $.ajax({
                    type: 'POST',
                    url: '/Delivery/UpdateStatus',
                    data: {
                        KeyId: keyId,
                        DeliveryStatus: status,
                        OrderPrice: orderPrice,
                        ShipPrice: shipPrice,
                        Note: note,
                    },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (response) {

                        $('#modal-add-edit').modal('hide');
                        general.notify('Ghi thành công!', 'success');
                        resetForm();
                        $('#frmMaintainance').trigger('reset');
                        general.stopLoad();
                        loadData();
                    },
                    error: function (err) {
                        general.notify('Có lỗi trong khi ghi !', 'error');
                        general.stopLoad();

                    },
                });
                return false;
            }
        });

        $('#txtShip').on('keyup change', function (e) {
            loadTotalPrice();
        }); 



    }

    function resetForm() {

        $('#frmMaintainance').trigger('reset');
        $('#txtDeliTotal').text('');
        $('#txtId').val('');
        $('#txtName').val('')


    }

    function loadDetail(that) {
        resetForm();
        var template = $('#InvoiceDetail').html();
        var render = "";
        $.ajax({
            type: "GET",
            url: "/Delivery/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                switch (response.DeliveryStatus) {
                    case general.deliStatus.UnConfirm:
                        $('#selDeliStatus')
                            .find('option')
                            .remove()
                            .end()
                            .append('<option value="1">Chưa xác nhận</option><option value="2">Xác nhận</option><option value = "6" > Thất bại</option >');
                        $('#txtShip').removeAttr("readonly");
                        break;
                    case general.deliStatus.Confirm:
                        $('#selDeliStatus')
                            .find('option')
                            .remove()
                            .end()
                            .append('<option value="2">Xác nhận</option><option value="3">Đang đóng gói</option><option value = "6" > Thất bại</option >');
                        $('#txtShip').removeAttr("readonly");
                        break;
                    case general.deliStatus.Packaged:
                        $('#selDeliStatus')
                            .find('option')
                            .remove()
                            .end()
                            .append('<option value="3">Đang đóng gói</option><option value="4">Đang vận chuyển</option><option value = "6" > Thất bại</option >');
                        $('#txtShip').attr("readonly", "readonly");
                        break;
                    case general.deliStatus.OnDelivery:
                        $('#selDeliStatus')
                            .find('option')
                            .remove()
                            .end()
                            .append('<option value="4">Đang vận chuyển</option><option value="5">Giao thành công</option><option value = "6" >Thất bại</option >');
                        $('#txtShip').attr("readonly", "readonly");
                        break;
                    case general.deliStatus.Success:
                        $('#selDeliStatus')
                            .find('option')
                            .remove()
                            .end()
                            .append('<option value="5">Giao thành công</option>');
                        $('#txtShip').attr("readonly", "readonly");
                        break;
                    case general.deliStatus.Fail:
                        $('#selDeliStatus')
                            .find('option')
                            .remove()
                            .end()
                            .append('<option value="6">Thất bại</option>');
                        $('#txtShip').attr("readonly", "readonly");
                        break;
                }
                console.log("loaddetail", response);
                 

                $('#txtId').val(response.KeyId);
                $('#txtInvoiceId').val(response.InvoiceFK);
                $('#txtCustomer').val(response.InvoiceFKNavigation.CustomerFKNavigation.UserBy.FullName);
                $('#txtCustomerId').val(response.InvoiceFKNavigation.CustomerFK);
                $('#txtUserName').val(response.InvoiceFKNavigation.CustomerFKNavigation.UserBy.UserName);
                $('#txtDeliName').val(response.InvoiceFKNavigation.DeliContactName);
                $('#txtDeliHotline').val(response.InvoiceFKNavigation.DeliContactHotline);    
                $('#txtDeliAddress').val(response.InvoiceFKNavigation.DeliAddress);
                $('#txtOrderPrice').val(general.toMoney(response.OrderPrice));
                $('#txtShip').val(general.toMoney(response.ShipPrice));
                $('#txtNote').val(response.Note);
               
                $('#selDeliStatus').val(response.DeliveryStatus);
                $.ajax({
                    type: "GET",
                    url: "/Delivery/GetInvoiceDetailByInvoiceId",
                    data: { invoiceId: response.InvoiceFK },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (innerresponse) {
                        $.each(innerresponse, function (i, item) {
                            var _unitPrice = general.toMoney(item.BookFKNavigation.UnitPrice);
                            var _price = general.toMoney(item.SubTotal);
                            render += Mustache.render(template, {

                                BookId: item.BookFK,
                                BookName: item.BookFKNavigation.BookTitle,
                                Qty: item.Qty,
                                UnitPrice: _unitPrice,
                                Price: _price,

                            });

                        });
                  
                        
                        $('#Delivery-tbl-content').html(render);
                        loadTotalPrice();
                        $('#modal-add-edit').modal('show');
                        general.stopLoad();

                    },
                    error: function (status) {
                        general.notify('Có lỗi xảy ra khi load mặt hàng', 'error');
                        general.stopLoad();
                    }
                });

                general.stopLoad();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra khi xem phiếu giao hàng này!', 'error');
                general.stopLoad();
            }
        });


    }
}
function loadTotalPrice()
{
    var _delitotal = general.toInt($('#txtOrderPrice').val()) + general.toInt($('#txtShip').val());
    var template = $('#DeliTotal-template').html();
    var renderDetail = "";
    renderDetail += Mustache.render(template, {

        DeliTotal: _delitotal,

    });
    $('#txtDeliTotal').html(general.toMoney(renderDetail));

}
function loadData(isPageChanged) {

    var template = $('#table-template').html();
    var render = "";

    $.ajax({
        type: 'GET',
        data: {
            status: $('#selStatus option:selected').val(),
            fromdate: $('#dtBegin').val(),
            todate: $('#dtEnd').val(),
            keyword: $('#txtKeyword').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/Delivery/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log("data", response);
            $.each(response.Results, function (i, item) {
                var _color = '';
                var _statusName = '';
                switch (item.DeliveryStatus) {
                    case general.deliStatus.UnConfirm:
                        _color = 'black';
                        _statusName = 'Chưa xác nhận';
                        break;
                    case general.deliStatus.Confirm:
                        _color = 'blue';
                        _statusName = 'Xác nhận';
                        break;
                    case general.deliStatus.Packaged:
                        _color = 'orange';
                        _statusName = 'Đang đóng gói';
                        break;
                    case general.deliStatus.OnDelivery:
                        _color = 'purple';
                        _statusName = 'Đang vận chuyển';
                        break;
                    case general.deliStatus.Success:
                        _color = 'green';
                        _statusName = 'Giao thành công';
                        break;
                    case general.deliStatus.Fail:
                        _color = 'red'
                        _statusName = 'Thất bại';
                        break;
                }
                var _dateCreate = moment(item.DateCreated).format("DD/MM/YYYY");
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                    CustomerName: item.InvoiceFKNavigation.CustomerFKNavigation.UserBy.FullName,
                    DateCreated: _dateCreate,
                    Status: '<span class="badge bg-' + _color + '">' + _statusName + '</span>',

                });
           

            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            general.stopLoad();
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            general.stopLoad();
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
    if (totalsize > 0)
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




