var bomController = function () {
    this.initialize = function () {
        loadData();
        loadUnit();
        //getProduct();
        registerEvents();
    }
    var gId = 0;
    var listDelete =[];
    function registerEvents() {
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $("#chkStatus").prop('checked', true);

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
            gId = 0;
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });


        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadProductBom(that, function (data) {
                gId = data.KeyId;
                loadBom(data.KeyId);
            });
            $('#modal-add-edit').modal('show');
        });


        var count = 1;
        $('#btn-add-product').on('click', function () {
            var template = $('#template-table-detail').html();
            var render = Mustache.render(template, {
                KeyId: count,
                ProductCode: "",
                ProductName: "",
                ProductUnit: '',
                ProductQty: 0
            });
            count++;
            $('#table-quantity-content').append(render);
        });
        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            var keyid = $(this).data('id');
            $(this).closest('tr').remove();
            if (keyid > 0)
                listDelete.push(keyid);
        });

        $('body').on('input', '#txtProductCode', function (e) {
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
        $('body').on('click', '.contain-productCode .ddlproduct', function (e) {
            var a = $(this).data('idproduct');
            var that = $(this);
            var temp = $(this).parent('.contain-productCode').parent('.autocomplete').parent('.col-productCode');
            $.ajax({
                type: 'GET',
                url: '/bom/GetById',
                data: { id: a },
                success: function (res) {
                    console.log(res);
                    that.parent('.contain-productCode').siblings('.code').val(res.ProductCode);
                    that.parent('.contain-productCode').siblings('.code').data('keyid', a);
                    temp.siblings('.productName').text(res.ProductName);
                    temp.siblings('#productunit').text(res.ProductUnitNavigation.UnitName);
                    temp.siblings('#btn').children('.btn-delete-quantity').data('id', 0);
                    $('.contain-productCode').hide();
                },
                error: function (err) {

                }
            });

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
        $(document).click(function (e) {
            if (!$(e.target).is('.contain-productCode')) {
                $('.contain-productCode').hide();
            }
        });
    }
    
    
}
function deleteBom(listDelete,callback) {
    $.ajax({
        type: 'POST',
        url: '/bom/Delete',
        data: { list: listDelete },
        success: function (response) {
            callback(response);
        },
        error: function (err) {
            general.notify('Có lỗi khi xóa bom!', 'error');
        }
    });
}
function resetFormMaintainance() {
    $('#txtProductBOMCode').val('');
    $('#txtProductName').val('');
    $('#selUnit').val('');
    $('#txtDiscription').val('');
    $('#dtDateMotified').val('');
    $('#txtLastUpdatedByFK').val('');
    $("#chkStatus").prop('checked', true);
    $('#table-quantity-content tr').each(function () {
        $(this).remove();
    });
}
function SaveBom(data,callback) {
    $.ajax({
        type: 'POST',
        url: '/bom/SaveList',
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
            general.startLoading();
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
            general.stopLoading();
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
function loadProductBom(that,callback) {
    $.ajax({
        type: "GET",
        url: "/bom/GetById",
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
        url: '/bom/GetByProductFk',
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
function loadProduct(key,temp, callback) {
    var template = $('#productCode-template').html();
    var render = "";
    var a = temp.siblings('.contain-productCode');
    $.ajax({
        type: 'GET',
        url: '/product/GetAllKeyWord',
        data: {keyword:key},
        dataType: 'json',
        success: function (response) {
            console.log(response);
            //listProduct = response;
            $.each(response, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.Id,
                    ProductCode: item.Name,
                    ProductName: item.Des
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
function loadUnit() {
    $('#selUnit option').remove();
    $.ajax({
        type: 'GET',
        url: '/product/GetAllUnit',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $('#selUnit').append("<option value=''>Chọn đơn vị tính</option>");
            $.each(response, function (i, item) {
                $('#selUnit').append("<option value='" + item.KeyId + "'>" + item.UnitName + "</option>");
            });
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
        }
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
        url: '/bom/GetAllPaging',
        dataType: 'json',
        beforeSend: function () {
            general.startLoading();
        },
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    stt: i+1,
                    KeyId: item.KeyId,
                    ProductCode: item.ProductCode,
                    ProductName: item.ProductName,
                    ProductUnit: item.ProductUnitNavigation.UnitName,
                    Status: general.getStatus(item.Status)
                });
                
            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            wrapPagingUnit(response.RowCount, function () {
                loadData();
            }, isPageChanged);
            general.stopLoading();
            //if (render == '')
            //   $('#datatable-checkbox_paginate').hide();
            //else
            //   $('#datatable-checkbox_paginate').show();
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
            general.stopLoading();
        }
    });
}

function wrapPagingUnit(recordCount, callBack, changePageSize) {
    var totalsize = Math.ceil(recordCount / general.configs.pageSize);
    //Unbind pagination if it existed or click change pagesize
    if ($('#paginationUL a').length === 0 || changePageSize === true) {
        $('#paginationUL').empty();
        $('#paginationUL').removeData("twbs-pagination");
        $('#paginationUL').unbind("page");
    }
    //Bind Pagination Event
    if (totalsize > 0) {
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
    
}
var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();
