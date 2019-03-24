var PurchaseOrderController = function () {
    this.initialize = function () {
        general.startLoad();

        loadProjectAndSO();
        loadEmployee();
        loadVendor();
        loadPoStatus();
        registerEvents();
        delayStatus(function () {
            var status = $('#lblPOStatus').text();
            if (status == "") {
                $('#lblPOStatus').text(gStatusPurchaseOrder_Name);
            }
            else
                $('#lblPOStatus').text('');
        }, 500)
        general.stopLoad();
    }
    var gId = 0;
    var gSOId = 0;
    var listDelete = [];
    var glistStatus = [];
    var gStatusPurchaseOrder_FK = 0;
    var gStatusPurchaseOrder_Name = 'test';
    var gProjectAndSO = [];// lưu các dự án đang thi công
    var gBomOfProject = [];
    var gPurchaseOrder;
    var glistPurchaseOrderDetail = [];
    var glistVendor = [];
    var glistEmployee = [];
    this.loadById = function (id, Is_view) {
        //reset data
        //load data
        gId = id;
        if (id == 0) {
            if (gProjectAndSO.length == 0) {
                $('#btnClose').click();
                general.notify('Hiện không có công trình nào để chọn !', 'error');

            }
            else
                if (glistVendor.length == 0) {
                    $('#btnClose').click();
                    general.notify('Hiện chưa có nhà cung cấp vui lòng thêm nhà cung cấp trước !', 'error');
                }
                else {
                    autoProjectAndSo(document.getElementById('txtConstructionFk'), gProjectAndSO);
                    autoAddressbook(document.getElementById('txtVendor'), glistVendor);
                    autoAddressbook(document.getElementById('txtBuyerFk'), glistEmployee); 
                    $('#btn-add').hide();
                    setPOStatusFk(general.poStatus.Draft);                   
                }
        }
        else
            if (id > 0) {

                loadDataById(id);
            }
    }
   
    this.resetFormSelectDetail = function () {
        resetSelectDetail();
    }
    function registerEvents() {
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $("#txtPOId").prop('disabled', true);
        $("#txtCreateDate").prop('disabled', true);
        $("#txtSOFk").prop('disabled', true);

        // ---------select bom to order -------------------
        $('#txtSelectCode').prop('disabled', true);
        $('#txtSelectUnit').prop('disabled', true);
        $('#txtSelectQtyB2').prop('disabled', true);
        $('#txtSelectPrice').prop('disabled', true);
        $('#txtSelectSubTotal').prop('disabled', true);
        $('#txtSelectQty').attr('maxlength', '12');
        $('#txtSelectDes').attr('maxlength', '500');
        // ------------------end ----------------------
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtProductCode: {
                    required: true
                },
                selUnit: {
                    required: true
                },
                txtProductName: {
                    required: true
                },
                txtProductQty: {
                    required: true,
                    number: true
                }
            }
        });

        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            var keyid = $(this).data('id');
            $(this).closest('tr').remove();
            if (keyid > 0)
                listDelete.push(keyid);
        });
        $('#btnSave').on('click', function () {
            if ($('#frmMaintainance').valid()) {
                var bomArr = [];
                $('#table-quantity-content tr').each(function () {
                    var productId = $(this).children('.col-productCode').children('.autocomplete').children('.code').data('keyid');
                    var qty = $(this).children('#productqty').children('.qty').val();
                    var keyId = $(this).children('#btn').children('.btn-delete-quantity').data('id');
                    var temp = {
                        KeyId: keyId,
                        ProductBomFk: gId,
                        ProductFk: productId,
                        ProductQty: qty

                    };
                    bomArr.push(temp);
                });
                var data = {
                    KeyId: gId,
                    ProductCode: $('#txtProductBOMCode').val(),
                    ProductName: $('#txtProductName').val(),
                    ProductUnit: $('#selUnit option:selected').val(),
                    ProductTypeFK: general.productType.FinishedProducts,
                    Status: general.status.Active,
                    ProductBomFkNavigations: bomArr
                };
                if (listDelete.length > 0) {
                    deleteBom(listDelete, function () {
                        saveProduct(data);
                    });
                }
                else
                    saveProduct(data);
            }
        });
       
        $('#txtSOFk').on('change', function () {
            var soid = document.getElementById('txtConstructionFk').getAttribute('data-id');
            var id = general.toInt(soid);
            //load dữ liệu theo id
          
                if (gSOId != id && id > 0) {
                    gSOId = id;
                    // load theo id
                    gBomOfProject = [];
                    $.ajax({
                        type: 'GET',
                        url: '/PurchaseOrderCons/FindBomOfProject',
                        data: { sofk:id },
                        dataType: 'json',
                        beforeSend: function () {
                            general.startLoad();
                        },
                        success: function (response) {
                            console.log(response);
                            gBomOfProject = response;
                                autoBom(document.getElementById('txtSelectName'), gBomOfProject);
                            $('#btn-add').show();
                            general.stopLoad();
                        },
                        error: function (err) {
                            general.notify('load dự án bị lỗi !', 'error');
                            general.stopLoad();
                        }
                    });
            }
            else
                {
                    // reset dữ liệu
                    $('#btn-add').hide();
                    $('#table-PurchaseOrder-content tr').each(function () {
                        $(this).remove();
                    });
                }
        })
        $('#btn-add').on('click', function () {
            var soid = document.getElementById('txtConstructionFk').getAttribute('data-id');
            var id = general.toInt(soid);
            if (id > 0) {
                $('#modal-add-edit-orderdetail').modal('show');
            }
        })
        //--------- event select bom to order ----------
        $('#txtSelectCode').on('change', function () {
            var code = document.getElementById('txtSelectName').getAttribute('data-code');
            if (code != '') {
                for (var i = 0; i < gBomOfProject.length; i++)
                    if (gBomOfProject[i].Code == code) {
                        $('#txtSelectDes').val(gBomOfProject[i].Description);
                        var temp = $('#txtSelectPrice');
                        temp.data('value', gBomOfProject[i].PriceB2);
                        temp.val(general.toMoney(gBomOfProject[i].PriceB2));
                        temp = $('#txtSelectUnit');
                        temp.data('id', gBomOfProject[i].UnitFK);
                        temp.val(gBomOfProject[i].UnitName);
                        var qty = gBomOfProject[i].TotalQtyB2 - gBomOfProject[i].TotalQtyOrdered;
                        $('#txtSelectQty').val(qty);
                        $('#txtSelectQtyB2').val(qty);
                        var subtotal = qty * gBomOfProject[i].PriceB2;
                        temp = $('#txtSelectSubTotal');
                        temp.data('value', subtotal);
                        temp.val(general.toMoney(subtotal));

                        // load chi tiết nếu có
                        if (gBomOfProject[i].PoPurchaseOrderBomDetails.length > 0) {
                            var listdetail = gBomOfProject[i].PoPurchaseOrderBomDetails;
                            var template = $('#table-template-detail').html();     
                            var render = "";
                            for (var j = 0; j < listdetail.length; j++) {
                                render += Mustache.render(template, {
                                    detailcode: listdetail[j].BomDetaiFK,
                                    detailname: listdetail[j].NameBomDetail,
                                    UnitFk: listdetail[j].UnitFK,
                                    UnitName: listdetail[j].UnitName,
                                    qtyvalue: listdetail[j].QtyB2,
                                    detailqty: listdetail[j].QtyB2,
                                    unitpricevalue: listdetail[j].PriceB2,
                                    unitprice: general.toMoney(listdetail[j].PriceB2),
                                    SubTatalvalue: listdetail[j].SubTotal,
                                    SubTotal: general.toMoney(listdetail[j].SubTotal),
                                    note: listdetail[j].Note,

                                })
                            }
                            $('#table-selectdetail-content').html(render);
                            $('#table-select-detail').show();
                        }
                        else {
                            $('#table-select-detail').hide();
                            $('#table-selectdetail-content').html('');
                        }
                        break;
                    }
            }
            else {
                resetSelectDetail();
            }
        })
        $('#txtSelectQty').on('change', function () {

        })
        //---end----------------
    }
    function loadDataById(id) {

    }
    function loadProjectAndSO() {
        gProjectAndSO = [];
        $.ajax({
            type: 'GET',
            url: 'PurchaseOrderCons/FindForProjectAndSo',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                gProjectAndSO = response;
            },
            error: function (err) {
                console.log(err);
                general.notify('Không thể load thông tin công trình', 'error');
            }
        })
    }
    function loadVendor() {
        glistVendor = [];
        $.ajax({
            type: 'GET',
            data: {
                status: general.status.Active
            },
            url: '/Vendor/GetListFullName',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                if (response.length > 0) {
                    glistVendor = response;
                }
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu nhà cung cấp', 'error');
            }
        });
    }
    function loadEmployee() {
        glistEmployee = [];
        $.ajax({
            type: 'GET',
            data: {
                status: general.status.Active
            },
            url: '/Customer/GetListFullName',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                if (response.length > 0) {
                    glistEmployee = response;
                }
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu người liên lạc', 'error');
            }
        });
    }
    function loadPoStatus() {
        glistStatus = [];
        $.ajax({
            type: 'GET',
            url: '/PurchaseOrdercons/GetALLStatus',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                glistStatus = response;            
            },
            error: function (err) {
                general.notify('Không thể load dữ liệu trạng thái PO', 'error');
            }
        })
    }
    function controlButtonByStatus(id, Statusid, IsView) {

    }
    function resetFormMaintainance() {
        $('#txtProductBOMCode').val('');
        $('#txtProductName').val('');
        $('#selUnit').val('');
        $('#txtDiscription').val('');
        $('#dtDateMotified').val('');
        $('#txtLastUpdatedByFK').val('');
        $("#chkStatus").prop('checked', true);
        $('#table-PurchaseOrder-content tr').each(function () {
            $(this).remove();
        });
    }
    function SaveBom(data, callback) {
        $.ajax({
            type: 'POST',
            url: '/PurchaseOrderCons/SaveList',
            data: { BomVm: data },
            success: function (response) {
                callback(response);
            },
            error: function (error) {
                general.notify('Có lỗi khi ghi bom!', 'error');
            }
        });
    }
    function saveProduct(data) {
        $.ajax({
            type: 'POST',
            url: 'bom/SaveEntity',
            data: data,
            dataType: 'json',
            beforeSend: function () {
                general.startLoad();
            },
            success: function (res) {
                console.log(res);
                general.notify('Ghi thành công!', 'success');
                resetFormMaintainance();
                $('#modal-add-edit').modal('hide');
                loadData();
            },
            error: function (error) {
                general.notify('Có lỗi khi ghi', 'error');
                general.stopLoad();
            }
        });
    }
    function updateProduct(data) {
        $.ajax({
            type: 'POST',
            url: 'bom/UpdateEntity',
            data: data,
            dataType: 'json',
            beforeSend: function () {
                general.startLoading();
            },
            success: function (res) {
                console.log(res);
                general.notify('Ghi thành công!', 'success');
                resetFormMaintainance();
                general.stopLoading();
                $('#modal-add-edit').modal('hide');
                loadData();

            },
            error: function (error) {
                general.notify('Có lỗi khi cập nhật!', 'error');
                general.stopLoading();
            }
        });
    }
    function loadProductBom(that, callback) {
        $.ajax({
            type: "GET",
            url: "/PurchaseOrderCons/GetById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                var data = response;
                gId = data.KeyId;
                $('#txtProductBOMCode').val(data.ProductCode);
                $('#selUnit').val(data.ProductUnit);
                $('#txtProductName').val(data.ProductName);
                $('#txtDiscription').val(data.Description);
                if (data.LastUpdateByFk != null)
                    $('#txtLastUpdatedByFK').val(data.LastUpdateByFkNavigation.FullName);

                $('#dtDateMotified').val(general.dateFormatJson(data.DateModified, true));
                if (data.Status == 1)
                    $('#chkStatus').prop('checked', true);
                else
                    $('#chkStatus').prop('checked', false);
                callback(data);

                general.stopLoading();

            },
            error: function (status) {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
            }
        });
    }
    function loadBom(pro) {
        var template = $('#template-table-detail').html();
        var render = '';
        $.ajax({
            type: 'GET',
            url: '/PurchaseOrderCons/GetByProductFk',
            data: { productFk: pro },
            success: function (res) {
                $.each(res, function (i, item) {
                    render += Mustache.render(template, {
                        stt: i + 1,
                        KeyId: item.KeyId,
                        ProductFk: item.ProductFk,
                        ProductCode: item.ProductFkNavigation.ProductCode,
                        ProductName: item.ProductFkNavigation.ProductName,
                        ProductUnit: item.ProductFkNavigation.ProductUnitNavigation.UnitName,
                        ProductQty: item.ProductQty
                    });
                });
                if (render != '') {
                    $('#table-quantity-content').html(render);
                }
            },
            error: function (error) {

            }
        });
    }
    function getProduct() {
        $.ajax({
            type: 'GET',
            url: '/product/GetAll',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                listProduct = response;
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
    var delayStatus = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearInterval(timer)
            timer = setInterval(callback, ms);
        };
    })();
    function setPOStatusFk(statusid, callback) {
        var kt = false;
        for (var i = 0; i < glistStatus.length; i++)
            if (glistStatus[i].Id == statusid) {
                gStatusPurchaseOrder_FK = statusid;
                gStatusPurchaseOrder_Name = glistStatus[i].Name;
                kt = true;
                if(callback!=undefined)
                    callback();
                break;
            }
        if (kt == false) {
            general.notify('Chưa tìm thấy trạng thái !');
        }
        return false;
    }
    function resetSelectDetail() {
        $('#txtSelectDes').val('');
        var temp = $('#txtSelectPrice');
        temp.data('value',0);
        temp.val('');
        temp = $('#txtSelectUnit');
        temp.data('id', 0);
        temp.val('');
        $('#txtSelectQty').val('');
        $('#txtSelectQtyB2').val('');
        temp = $('#txtSelectSubTotal');
        temp.data('value', 0);
        temp.val(general.toMoney(''));

        $('#table-select-detail').hide();
        $('#table-selectdetail-content').html('');
    }
}