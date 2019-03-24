var productController = function () {
    var gId = 0;
    var gImageAddress;
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        loadProductType();
        loadUnit();
       
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $("#chkStatus").prop('checked', true);
        $("#chkIsSimple").prop('checked', false);
        //$('#fileInputExcel').inputFileText({
        //    text: 'Chọn file excel'
        //});
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtProductName: {
                    required: true
                },
                txtProductCode: {
                    required: true
                },
                selProductType: {
                    required: true
                },
                selUnit: {
                    required: true
                },
                txtUnitPrice: {
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
        
        //$('#btnSelectImg').on('click', function () {
        //    $('#fileInputImage').click();
        //});

        //$("#fileInputImage").on('change', function () {
        //    var fileUpload = $(this).get(0);
        //    var files = fileUpload.files;
        //    var data = new FormData();
        //    for (var i = 0; i < files.length; i++) {
        //        data.append(files[i].name, files[i]);
        //    }
        //    $.ajax({
        //        type: "POST",
        //        url: "/Upload/UploadImage",
        //        contentType: false,
        //        processData: false,
        //        data: data,
        //        success: function (path) {
        //            $('#txtImage').val(path);
        //            gImageAddress = path;
        //            general.notify('Upload image succesful!', 'success');

        //        },
        //        error: function () {
        //            general.notify('There was error uploading files!', 'error');
        //        }
        //    });
        //});

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            gId = 0;
            resetFormMaintainance();
            $('#txtProductCode').prop('disabled', true);
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/ProductCons/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    console.log(response);
                    var data = response;
                    gId = data.KeyId;
                    $('#txtProductCode').val(data.ProductCode);
                    $('#txtProductName').val(data.ProductName);
                    $('#selProductType').val(data.ProductTypeFK);
                    $('#selUnit').val(data.ProductUnit);
                    $('#txtUnitPrice').val(general.toMoney(data.UnitPrice));
                    loadRight();
                    if (data.LastUpdateByFk != null)
                        $('#txtLastUpdatedByFK').val(data.LastUpdateByFkNavigation.FullName);

                    $('#dtDateMotified').val(general.dateFormatJson(data.DateModified, true));
                    if (data.Status == 1)
                        $('#chkStatus').prop('checked', true);
                    else
                        $('#chkStatus').prop('checked', false);

                    $('#modal-add-edit').modal('show');
                    general.stopLoading();
                },
                error: function (status) {
                    general.notify('Có lỗi xảy ra', 'error');
                    general.stopLoading();
                }
            });
        });
        $('#txtUnitPrice').keyup(function (event) {

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
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var productCode = $('#txtProductCode').val();
                if (gId == 0) {
                    checkproductCode(productCode, function (response) {
                        if (response == false)
                            save(productCode);
                        else {
                            general.notify("Mã sản phẩm đã tồn tại.", "error");
                        }
                    });
                }
                else
                    save(productCode);
            }

        });
       
        $('#btn-import').on('click', function () {
            $('#modal-import-excel').modal('show');
        });
        function loadRight() {
            document.getElementById('txtUnitPrice').style.textAlign = "right";
        }
        $('#btnImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            $.ajax({
                url: 'ProductCons/ImportExcel',
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    console.log(data);
                    if (data.length > 0) {
                        s
                        var s = '';
                        data.forEach(function (item, i) {
                            s += item + ", ";
                        })
                        alert("Có một vài mã bị lỗi. Vui lòng kiểm tra lại đơn vị tính và loại sản phẩm cho đúng tên: " + s)
                    }
                    $('#modal-import-excel').modal('hide');
                    loadData(true);
                }
            });
            return false;
        });

        $('#btn-export').on('click', function () {
            var keyword = $('#txtKeyword').val();
            $.ajax({
                type: "POST",
                url: "ProductCons/ExportExcel",
                data: { keysearch: keyword },
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

    }
    function save(productCode) {
        var status;
        var productName = $('#txtProductName').val();
        var productType = $('#selProductType option:selected').val();
        var productUnit = $('#selUnit option:selected').val();
        var unitPrice = general.toFloat($('#txtUnitPrice').val());
        //var image = gImageAddress;
        //var lastUpdatedBy = $('#user').data('userid');
        //var lastUpdatedDate = $('#dtDateModified').val();
        
        var a = $("#chkStatus").is(':checked');
        if (a == true)
            status = 1;
        else
            status = 0;

        var data = {
            KeyId: gId,
            ProductName: productName,
            productCode: productCode,
            ProductTypeFK: productType,
            ProductUnit: productUnit,
            UnitPrice: unitPrice,
            Status: status
        };

        $.ajax({
            type: "POST",
            url: "/ProductCons/SaveEntity",
            data: data,
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                general.notify('Ghi thành công!', 'success');
                $('#modal-add-edit').modal('hide');
                resetFormMaintainance();

                general.stopLoading();
                loadData(true);
            },
            error: function () {
                general.notify('Có lỗi trong khi ghi !', 'error');
                general.stopLoading();
            }
        });
    }

    function checkproductCode(productCode, callback) {
        $.ajax({
            type: 'POST',
            url: '/ProductCons/checkproductRef',
            data: { productCode },
            dataType: 'json',
            success: function (response) {
                console.log(response);
                callback(response);
            },
            error: function (error) {

            }
        });
    }
    function resetFormMaintainance() {
        $('#txtProductCode').prop('disabled', false);
        $('#txtProductCode').val('');
        $('#txtProductName').val('');
        $('#selProductType').val('');
        $('#selUnit').val('');

        $('#txtUnitPrice').val('');
        $('#txtLastUpdatedByFK').val('');
        $('#dtDateModified').val('');
        $("#chkStatus").prop('checked', true);
        $("#chkIsSimple").prop('checked', false);
        $('#table-quantity-content tr').each(function () { $(this).remove(); });
    }
    //function resetFormAtrubute() {
    //    $('#txtAttributeName').val('');
    //    $('#table-atrribute-content tr').each(function () { $(this).remove(); });
    //}
    //function loadAttributes() {
    //    return $.ajax({
    //        type: "GET",
    //        url: "/ProductAtrribute/GetAll",
    //        dataType: "json",
    //        success: function (response) {
    //            cachedObj.attributes = response;
    //        },
    //        error: function () {
    //            general.notify('Có lỗi xảy ra', 'error');
    //        }
    //    });
    //}

    //function getAttributeOptions(selectedId) {
    //    var attributes = "<select class='form-control ddlColorId'>";
    //    attributes += '<option value="" selected="select" style="color:red">' + "--chọn--" + '</option>';
    //    $.each(cachedObj.attributes, function (i, attribute) {

    //        if (selectedId === attribute.KeyId)
    //            attributes += '<option value="' + attribute.KeyId + '" selected="select">' + attribute.Name + '</option>';
    //        else
    //            attributes += '<option value="' + attribute.KeyId + '">' + attribute.Name + '</option>';
    //    });
    //    attributes += '<option value=-1 style="color:blue">' + "Tao mới \"thuộc tính\"" + '</option>';
    //    attributes += "</select>";
    //    return attributes;
    //}
    //function getAttributeValue(key) {
    //    var arr = [];
    //    $.each(cachedObj.attributes, function (i, item) {
    //        if (item.Name == key || item.KeyId == key) {
    //            $.each(item.tblProduct_Attribute_Values, function (j, temp) {
    //                var obj = {
    //                    KeyId: temp.KeyId,
    //                    ValueName: temp.ValueName
    //                };
    //                arr.push(obj);
    //            });
    //        }
    //    });
    //    return arr;
    //}

    //function loadAttributeValue(key) {
    //    var template = $('#attributeValue-template').html();
    //    var render = "";
    //    $.each(cachedObj.attributeValues, function (i, item) {
    //        render += Mustache.render(template, {
    //            KeyId: item.KeyId,
    //            AttributeValue: item.ValueName
    //        });
    //        if (render != '') {
    //            $('#contain-listAttributeValue').html(render);
    //        }
    //    });
    //}
}

function loadProductType() {
    $('#selProductType option').remove();
    $.ajax({
        type: 'GET',
        url: '/product/GetAllProductType',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $('#selProductType').append("<option value=''>Chọn loại sản phẩm</option>");
            $.each(response, function (i, item) {
                $('#selProductType').append("<option value='" + item.KeyId + "'>" + item.ProductTypeName + "</option>");
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
    general.startLoading();
    var template = $('#table-template').html();
    var render = "";
    $.ajax({
        type: 'GET',
        data: {
            keyword: $('#txtKeyword').val(),
            page: general.configs.pageIndex,
            pageSize: general.configs.pageSize
        },
        url: '/product/GetAllPaging',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    ProductCode: item.ProductCode,
                    ProductName: item.ProductName,
                    ProductType: item.ProductTypeNavigation == null ? "" : item.ProductTypeNavigation.ProductTypeName,
                    ProductUnit: item.ProductUnitNavigation.UnitName,
                    //Image: item.Image == null ? '<img src="/admin-side/images/user.png" width=25' : '<img src="' + item.Image + '" width=25 />',
                    UnitPrice: general.toMoney(item.UnitPrice),
                    Status: general.getStatus(item.Status)
                });

                
            });
            $('#lblTotalRecords').text(response.RowCount);
            $('#tbl-content').html(render);
            wrapPaging(response.RowCount, function () {
                loadData();
            }, isPageChanged);
        },
        error: function (status) {
            console.log(status);
            general.notify('Không thể load dữ liệu', 'error');
        }
    });
    general.stopLoading();
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


