var productController = function () {
    var self = this;
    var saveSelectOption;
    var cachedObj = {
        attributes: [],
        attributeValues: []
    }
    var gId = 0;
    var gImageAddress;
    this.initialize = function () {
        loadData();
        loadAttributes();
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
                }
            }
        });
        $('#frmAttribute').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtAttributeName: {
                    required: true
                },
                txtattributevalue: {
                    required: true
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

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $("#fileInputImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txtImage').val(path);
                    gImageAddress = path;
                    general.notify('Upload image succesful!', 'success');

                },
                error: function () {
                    general.notify('There was error uploading files!', 'error');
                }
            });
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            gId = 0;
            resetFormMaintainance();
            $('#txtProductCode').prop('disabled', true);
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/product/GetById",
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
                    $('#txtImage').val(data.Image);
                    if (data.LastUpdateByFk != null)
                        $('#txtLastUpdatedByFK').val(data.LastUpdateByFkNavigation.FullName);

                    $('#dtDateMotified').val(general.dateFormatJson(data.DateModified, true));
                    if (data.Status == 1)
                        $('#chkStatus').prop('checked', true);
                    else
                        $('#chkStatus').prop('checked', false);

                    if (data.is_simple == 1)
                        $('#chkIsSimple').prop('checked', true);
                    else
                        $('#chkIsSimple').prop('checked', false);
                    var arr = [];
                    if (data.TblProduct_ProductAttributes.length>0)
                    data.TblProduct_ProductAttributes.forEach(function (item, i) {
                        var j;
                        for (j = 0; j < arr.length; j++)
                            if (item.Product_AttributeFk == arr[j].Product_AttributeFk) {
                                arr[j].value += ',' + item.Attribute_value;
                                break;
                            }
                        if (j == arr.length) {
                            var obj = {
                                Product_AttributeFk: item.Product_AttributeFk,
                                value: item.Attribute_value
                            }
                            arr.push(obj);
                        }
                    })
                    arr.forEach(function (item, i) {
                        var template = $('#template-table-load').html();
                        var render = Mustache.render(template, {
                            KeyId: 0,
                            Atributes: getAttributeOptions(item.Product_AttributeFk),
                            value: item.value,
                        });
                        $('#table-quantity-content').append(render);
                    })
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
        $('#btnAttributeSave').on('click', function (e) {
            if ($('#frmAttribute').valid()) {
                e.preventDefault();

                var attributeName = $('#txtAttributeName').val();
                if (attributeName != "") {
                    var attributeList = [];
                    $('#table-atrribute-content tr').each(function (i, item) {
                        var obj = {
                            ValueName: $(item).find('#txtattributevalue').first().val(),
                            //SizeId: $(item).find('select.ddlSizeId').first().val(),
                            //ColorId: $(item).find('select.ddlColorId').first().val(),
                        };
                        if (obj.ValueName !=null)
                            attributeList.push(obj);
                    });
                    if (attributeList.length != 0) {
                        $.ajax({
                            url: '/ProductAtrribute/SaveEntity',
                            data: {
                                Code: attributeName,
                                Name: attributeName,
                                is_required: true,
                                is_unique: true,
                                tblProduct_Attribute_Values: attributeList
                            },
                            type: 'post',
                            dataType: 'json',
                            success: function (response) {
                                general.notify('Ghi thành công!', 'success');
                                $('#modal-create-attribute').modal('hide');
                                $('#table-attribute-content').html('');
                                cachedObj.attributes.push(response);
                                saveSelectOption.append('<option value = "' + response.KeyId + '" selected="selected" >'+ response.Name + '</option >');
                                saveSelectOption.prop('disabled', true);
                            },
                            error: function () {
                                general.notify('Có lỗi trong khi ghi !', 'error');
                                general.stopLoading();
                            }
                        });
                        return;
                    }
                }

                general.notify("Vui lòng điền đầy đủ các trường.", "notify");
            }
        });
        $('#btn-import').on('click', function () {
            $('#modal-import-excel').modal('show');
        });

        $('#btnImportExcel').on('click', function () {
            //var file = $('#fileInputExcel')[0].files[0];
            //console.log(file);
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            $.ajax({
                url: 'Product/ImportExcel',
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
            $.ajax({
                type: "POST",
                url: "Product/ExportExcel",
                data: { keysearch: $('#txtKeyword').val() },
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

        $('body').on('change', '.ddlColorId', function () {
            var a = $(this).find('option:selected').text();
            if (a == "Tao mới \"thuộc tính\"") {
                saveSelectOption = $(this);
                var temp = $(this).parent('#atribute-col').siblings('#attributeValue-col').children('#wrapper').children('#singleFieldTags2');
                temp.val('');
                resetFormAtrubute();
                $('#modal-create-attribute').modal('show');
            }
            else
                if (a != "--chọn--") {
                    var temp = $(this).parent('#atribute-col').siblings('#attributeValue-col').children('#wrapper').children('#singleFieldTags2');
                    temp.val('');
                    temp.tagbox(
                        {
                            hasDownArrow: true,
                            data: getAttributeValue(a),
                            valueField: 'ValueName',
                            textField: 'ValueName',
                            limitToList: true,
                            prompt: 'Chọn ' + a
                        });
                    $(this).prop('disabled', true);
                }
        });
        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });
        $('body').on('click', '.btn-delete-attribute', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });

      
        $('#btn-add-quantity').on('click', function () {
            var template = $('#template-table-quantity').html();
            var render = Mustache.render(template, {
                KeyId: 0,
                Atributes: getAttributeOptions(null)
            });
            $('#table-quantity-content').append(render);
        });
        $('#btn-add-attribute').on('click', function () {
            var template = $('#template-table-attribute').html();
            var render = Mustache.render(template, {});
            $('#table-atrribute-content').append(render);
        });
    
        $('body').on('click', '#singleFieldTags2', function () {
            var a = $(this).parent('#wrapper').parent('#attributeValue-col').siblings('#atribute-col').children('.ddlColorId').find('option:selected').text();
            if (a != "--chọn--" && a != "Tao mới \"thuộc tính\"") {
                $(this).tagbox(
                    {
                        hasDownArrow: true,
                        data: getAttributeValue(a),
                        valueField: 'ValueName',
                        textField: 'ValueName',
                        limitToList: true,
                        prompt: 'Chọn ' + a
                    });
                $(this).prop('disabled', true);
            };
        });
    }
    function save(productCode) {
        var status;
        var productName = $('#txtProductName').val();
        var productType = $('#selProductType option:selected').val();
        var productUnit = $('#selUnit option:selected').val();

        var image = gImageAddress;
        //var lastUpdatedBy = $('#user').data('userid');
        //var lastUpdatedDate = $('#dtDateModified').val();
        var arr = [];
        $('#table-quantity-content tr').each(function () {
            var opt_Select = $(this).children('#atribute-col').children('.ddlColorId').find('option:selected');
            var attributeId = opt_Select.val();
            var attributeName = opt_Select.text();
            if (attributeId != "") {
                $(this).children('#attributeValue-col').children('#wrapper').children('.combo').children('.textbox-value').each(function () {
                    var a = $(this).val();
                    var obj = {
                        Product_AttributeFk: attributeId,
                        Attribute_value: a
                    }
                    arr.push(obj);
                });
            }
        });
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

            tblProduct_ProductAttributes: arr,

            Image: image,
            //LastUpdateByFk: lastUpdatedBy,
            //DateModified: lastUpdatedDate,
            is_simple: false,
            Status: status
        };

        $.ajax({
            type: "POST",
            url: "/product/SaveEntity",
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
            url: '/product/checkproductRef',
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

        $('#txtImage').val('');
        $('#txtLastUpdatedByFK').val('');
        $('#dtDateModified').val('');
        $("#chkStatus").prop('checked', true);
        $("#chkIsSimple").prop('checked', false);
        $('#table-quantity-content tr').each(function () { $(this).remove(); });
    }
    function resetFormAtrubute() {
        $('#txtAttributeName').val('');
        $('#table-atrribute-content tr').each(function () { $(this).remove(); });
    }
    function loadAttributes() {
        return $.ajax({
            type: "GET",
            url: "/ProductAtrribute/GetAll",
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                cachedObj.attributes = response;
                general.stopLoading();
            },
            error: function () {
                general.notify('Có lỗi xảy ra', 'error');
                general.stopLoading();
            }
        });
    }

    function getAttributeOptions(selectedId) {
        var attributes = "<select class='form-control ddlColorId'>";
        attributes += '<option value="" selected="select" style="color:red">' + "--chọn--" + '</option>';
        $.each(cachedObj.attributes, function (i, attribute) {

            if (selectedId === attribute.KeyId)
                attributes += '<option value="' + attribute.KeyId + '" selected="select">' + attribute.Name + '</option>';
            else
                attributes += '<option value="' + attribute.KeyId + '">' + attribute.Name + '</option>';
        });
        attributes += '<option value=-1 style="color:blue">' + "Tao mới \"thuộc tính\"" + '</option>';
        attributes += "</select>";
        return attributes;
    }
    function getAttributeValue(key) {
        var arr = [];
        $.each(cachedObj.attributes, function (i, item) {
            if (item.Name == key || item.KeyId == key) {
                $.each(item.tblProduct_Attribute_Values, function (j, temp) {
                    var obj = {
                        KeyId: temp.KeyId,
                        ValueName: temp.ValueName
                    };
                    arr.push(obj);
                });
            }
        });
        return arr;
    }

    function loadAttributeValue(key) {
        var template = $('#attributeValue-template').html();
        var render = "";
        $.each(cachedObj.attributeValues, function (i, item) {
            render += Mustache.render(template, {
                KeyId: item.KeyId,
                AttributeValue: item.ValueName
            });
            if (render != '') {
                $('#contain-listAttributeValue').html(render);
            }
        });
    }
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
        beforeSend: function () {
            general.startLoading();
        },
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    ProductCode: item.ProductCode,
                    ProductName: item.ProductName,
                    ProductType: item.ProductTypeNavigation == null ? "" : item.ProductTypeNavigation.ProductTypeName,
                    ProductUnit: item.ProductUnitNavigation.UnitName,
                    Image: item.Image == null ? '<img src="/admin-side/images/user.png" width=25' : '<img src="' + item.Image + '" width=25 />',
                    Status: general.getStatus(item.Status)
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


