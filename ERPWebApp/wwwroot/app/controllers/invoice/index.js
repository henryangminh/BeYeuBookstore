var invoiceController = function () {
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


        //Delete
        $('body').on('click', '.btn-delete', function (e) {
            if (confirm("Bạn chắc chắn muốn xóa?")) {
                var that = $(this).data('id');
                $.ajax({
                    type: 'POST',
                    url: '/Invoice/Delete',
                    data: { id: that },
                    dataType: 'json',
                    beforeSend: function () {
                        general.startLoading();
                    },
                    success: function (response) {

                        general.notify('Xóa thành công!', 'success');
                        loadData();
                        general.stopLoading();

                    },
                    error: function (status) {
                        general.notify('Có lỗi trong khi xóa !', 'error');
                        general.stopLoading();
                    },
                });
            }
            else {
                alert("Bạn vừa bấm hủy");
            }

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
                var statusFK;
                var name = $('#txtName').val();
                $.ajax({
                    type: 'POST',
                    url: '/Invoice/SaveEntity',
                    data: {
                        KeyId: keyId,
                        BookCategoryName: name,
                    },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoading();
                    },
                    success: function (response) {

                        $('#modal-add-edit').modal('hide');
                        general.notify('Ghi thành công!', 'success');
                        resetForm();
                        $('#frmMaintainance').trigger('reset');
                        general.stopLoading();
                        loadData();
                    },
                    error: function (err) {
                        general.notify('Có lỗi trong khi ghi !', 'error');
                        general.stopLoading();

                    },
                });
                return false;
            }
        });




    }

    function resetForm() {
        $('#txtId').val('');
        $('#txtName').val('')


    }

    function loadDetail(that) {
        var template = $('#InvoiceDetail-table-template').html();
        var renderDetail = "";
        $.ajax({
            type: "GET",
            url: "/Invoice/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                console.log("loaddetail", response);
                var data = response;

                $('#txtId').val(data.KeyId);
                $('#txtCustomer').val(data.CustomerFKNavigation.UserBy.UserName);
                $('#txtCustomerId').val(data.CustomerFK);
                $('#dtDateCreated').val(moment(data.DateCreated).format("DD/MM/YYYY"));
                $('#txtTotalPrice').val(general.toMoney(data.TotalPrice));
             
                $.ajax({
                    type: "GET",
                    url: "/Invoice/GetAllInvoiceDetailById",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoad();
                    },
                    success: function (innerresponse) {
                      
                        console.log("InvoiceDetail", innerresponse);
                        $.each(innerresponse, function (i, item) {
                            var _subTotal = general.toMoney(item.SubTotal);
                            var _unitPrice = general.toMoney(item.BookFKNavigation.UnitPrice);
                            renderDetail += Mustache.render(template, {

                                InvoiceId: item.InvoiceFK,
                                BookName: item.BookFKNavigation.BookTitle,
                                Qty: item.Qty,
                                UnitPrice: _unitPrice,
                                Price: _subTotal,

                            });
                           
                        });
                        $('#InvoiceDetail-tbl-content').html(renderDetail);
                        console.log("ahihi",renderDetail);
                        $('#modal-add-edit').modal('show');
                       
                        
                        general.stopLoad();

                    },
                    error: function (status) {
                        general.notify('Có lỗi xảy ra', 'error');
                        general.stopLoad();
                    }
                });
                
                general.stopLoad();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoad();
            }
        });


    }
}
function loadData(isPageChanged) {

    var template = $('#table-template').html();
    var render = "";

    $.ajax({
        type: 'GET',
        data: {
            fromdate: $('#dtBegin').val(),
            todate: $('#dtEnd').val(),
            keyword: $('#txtKeyword').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize,
        },
        url: '/Invoice/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoad();
        },
        success: function (response) {
            console.log("data", response);
            var order = 1;
            $.each(response.Results, function (i, item) {

                var _dateCreate = moment(item.DateCreated).format("DD/MM/YYYY");
                var _totalPrice = general.toMoney(item.TotalPrice);
                render += Mustache.render(template, {

                    KeyId: item.KeyId,
                    CustomerName: item.CustomerFKNavigation.UserBy.FullName,
                    DateCreated: _dateCreate,
                    TotalPrice: _totalPrice,

                });
                order++;

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




