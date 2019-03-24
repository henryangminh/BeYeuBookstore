var productController = function () {
    var self = this;
    var saveSelectOption;
    var cachedObj = {
        attributes: [],
        attributeValues: []
    }
    var gId = 0;
    var count = 1;
    var listProduct = [];
    var listTemp = [];
    var listProductLink = [];
    var listProductAttribute = [];
    var listAttr = [];
    var listKeyIdDel = [];
    var idParent = 0;
    var productChild;
    var isAddNew = false;
    var gNChild;
    this.initialize = function () {
        loadData();
        loadAttributes();
        registerEvents();
    }

    function registerEvents() {
        loadProductType();
        loadUnit(function (id) { });
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $("#dtDateMotified").prop('disabled', true);
        $("#chkStatus").prop('checked', true);
        $("#chkIsSimple").prop('checked', false);
        $('#circularG').hide();
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
        $(document).click(function (e) {
            if (!$(e.target).is('.contain-attributeValue')) {
                $('.contain-attributeValue').hide();
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
            gId = 0;
            isAddNew = true;
            $('tfoot').show();
            $('#add-same-product').hide();
            resetFormMaintainance(); 
            loadUnit(function (id) {
                $('#selUnit').val(id);
            });
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
                    general.notify('Upload image succesful!', 'success');

                },
                error: function () {
                    general.notify('There was error uploading files!', 'error');
                }
            });
        });

        $('body').on('click', '.btn-edit-child', function (e) {
            isAddNew = false;
            e.preventDefault();
            gId = 0;
            resetFormMaintainance();
            $('tfoot').show();
            $('#txtProductCode').prop('disabled', true);
            $('#add-same-product').show();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/WoodProduct/GetById",
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
                    var s = data.ProductName.split('-');
                    $('#txtProductName').val(s[0]);
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
                    //var arr = [];
                    //data.TblProduct_ProductAttributes.forEach(function (item, i) {
                    //    var j;
                    //    for (j = 0; j < arr.length; j++)
                    //        if (item.Product_AttributeFk == arr[j].Product_AttributeFk) {
                    //            arr[j].value += ',' + item.Attribute_value;
                    //            break;
                    //        }
                    //    if (j == arr.length) {
                    //        var obj = {
                    //            Product_AttributeFk: item.Product_AttributeFk,
                    //            value: item.Attribute_value
                    //        }
                    //        arr.push(obj);
                    //    }
                    //})
                    data.TblProduct_ProductAttributes.forEach(function (item, i) {
                        var template = $('#template-table-load').html();
                        var render = Mustache.render(template, {
                            KeyId: item.KeyId,
                            Atributes: getAttributeOptions(item.Product_AttributeFk),
                            value: item.Attribute_value,
                        });
                        $('#table-quantity-content').append(render);
                        listAttr.push(item.Product_AttributeFk);
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
                        if (obj.ValueName != null)
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
                                saveSelectOption.append('<option value = "' + response.KeyId + '" selected="selected" >' + response.Name + '</option >');
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
            var fileUpload = $("#btnImportExcel").get(0);
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
        var attributeBefore;
        $('body').on('focusin', '.ddlColorId', function () {
            attributeBefore = $(this).find('option:selected').val();
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
                    var attrId = $(this).find('option:selected').val();
                    var i = 0;
                    for (; i < listAttr.length; i++)
                        if (listAttr[i] == attrId)
                            break;
                    if (i >= listAttr.length) {
                        listAttr.push(attrId);
                        $(this).prop('disabled', true);
                    }
                    else {
                        $(this).val(attributeBefore);
                        general.notify('Thuộc tính đã được chọn!', 'error');
                    }
                }
        });
        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            var keyid = general.toInt($(this).data('id'));
            if (keyid > 0)
                listKeyIdDel.push(keyid);
            var listDelete = [];
            var attrId = $(this).parent('td').siblings('#atribute-col').children('.ddlColorId').find('option:selected').val();
            $(this).parent('td').siblings('#attributeValue-col').children('.tags-value').children('ul').children('li').each(function () {
                var a = $(this).text();
                listDelete.push(a);
            });
            var i = 0;
            while (i < listDelete.length - 1) {
                var j = 0;
                while (j < listProduct.length) {
                    if (listProduct[j].ProductName.indexOf(listDelete[i]) !== -1) {
                        listProduct.splice(j, 1);
                        j--;
                    }
                    j++;
                }
                i++;
            }
            var value = listDelete[listDelete.length - 1];
            for (var i = 0; i < listProduct.length; i++) {
                var productName = listProduct[i].ProductName;
                if (productName.indexOf('-' + value) !== -1) {
                    listProduct[i].ProductName = listProduct[i].ProductName.replace('-' + value, '');
                    var temp = listProduct[i].tblProduct_ProductAttributes;
                    for (var j = 0; j < temp.length; j++)
                        if (temp[j].Attribute_value == value) {
                            temp.splice(j, 1);
                            break;
                        }
                    if (temp.length == 0)
                        listProduct.splice(i, 1);
                }
                else if (productName.indexOf(value + '-') !== -1) {
                    listProduct[i].ProductName = listProduct[i].ProductName.replace(value + '-', '');
                    var temp = listProduct[i].tblProduct_ProductAttributes;
                    for (var j = 0; j < temp.length; j++)
                        if (temp[j].Attribute_value == value) {
                            temp.splice(j, 1);
                            break;
                        }
                    if (temp.length == 0)
                        listProduct.splice(i, 1);
                }
                else if (productName.indexOf(value) !== -1)
                    listProduct.splice(i, 1);
            }
            $(this).closest('tr').remove();
            UpdateProductCode(listProduct, $('#txtProductCode').val(), function () {
                var template = $('#template-table-product').html();
                var render = '';
                listProduct.forEach(function (item) {
                    render += Mustache.render(template, {
                        ProductCode: item.ProductCode,
                        ProductName: item.ProductName
                    });
                });
                $('#table-same-product-content').html(render);
            });
            //xóa attribute trong listAttr
            var k = 0;
            for (; k < listAttr.length; k++)
                if (listAttr[k] == attrId)
                    break;
            if (k < listAttr.length)
                listAttr.splice(k, 1);
        });
        $('body').on('click', 'btn-delete-product', function (e) {
            e.preventDefault();
            var productCode = $(this).parent('td').siblings('#productCode').text();
            for (var i = 0; i < listProduct.length; i++) {
                if (listProduct[i].ProductCode == productCode) {
                    listProduct.splice(i, 1);
                    break;
                }
                    
            }
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

        //$('body').on('click', '#singleFieldTags2', function () {
        //    var a = $(this).parent('#wrapper').parent('#attributeValue-col').siblings('#atribute-col').children('.ddlColorId').find('option:selected').text();
        //    if (a != "--chọn--" && a != "Tao mới \"thuộc tính\"") {
        //        $(this).tagbox(
        //            {
        //                hasDownArrow: true,
        //                data: getAttributeValue(a),
        //                valueField: 'ValueName',
        //                textField: 'ValueName',
        //                limitToList: true,
        //                prompt: 'Chọn ' + a
        //            });
        //        $(this).prop('disabled', true);
        //    };
        //});
        $('#txtProductCode').on('change', function () {
            UpdateProductCode(listProduct, $(this).val(), function () {
                var template = $('#template-table-product').html();
                var render = '';
                listProduct.forEach(function (item) {
                    render += Mustache.render(template, {
                        ProductCode: item.ProductCode,
                        ProductName: item.ProductName
                    });
                });
                $('#table-same-product-content').html(render);
            });
        });
        $('body').on('keyup', '#txtValue', function (e) {
            var template = $('#value-template').html();
            var render = '';
            var keyword = $(this).val();
            var keyAttr = $(this).parent('.tags-value').parent('#attributeValue-col').siblings('#atribute-col').children('.ddlColorId').find('option:selected').text();
            $.each(cachedObj.attributes, function (i, item) {
                if (item.Name == keyAttr || item.KeyId == keyAttr) {
                    if (keyword!='')
                        $.each(item.tblProduct_Attribute_Values, function (j, temp) {
                            if (temp.ValueName.toUpperCase().indexOf(keyword.toUpperCase()) !== -1) {
                                render += Mustache.render(template, {
                                    KeyId: temp.KeyId,
                                    ValueName: temp.ValueName
                                });
                            }
                        });
                    else
                        $.each(item.tblProduct_Attribute_Values, function (j, temp) {
                            render += Mustache.render(template, {
                                KeyId: temp.KeyId,
                                ValueName: temp.ValueName
                            });
                        });
                }
            });
            if (render != '') {
                $(this).siblings('.contain-attributeValue').html(render);
                $(this).siblings('.contain-attributeValue').show();
            }
                
            if (e.keyCode == 13) {
                var value = $(this).val();
                $(this).siblings('ul').append('<li>' + value + '<span class="fa fa-close red"></span></li>');
            }
        });
        $('body').on('click', '.contain-attributeValue .ddlproduct', function () {
            var valueName = $(this).children('#atrributevalue').text();
            if (gId == 0 && isAddNew) {
                $('#list-same-product').show();
                var productCode = $('#txtProductCode').val();
                
                var keyAttr = $(this).parent('.contain-attributeValue').parent('.tags-value').parent('#attributeValue-col').siblings('#atribute-col').children('.ddlColorId').find('option:selected').val();
                var flag = 0;
                $(this).parent('.contain-attributeValue').siblings('ul').children('li').each(function () {
                    var oldValue = $(this).text();
                    if (valueName == oldValue) {
                        flag = 1;
                    }
                });
                if (flag == 0) {
                    listTemp = [];
                    $(this).parent('.contain-attributeValue').siblings('ul').append('<li>' + valueName + '<span class="fa fa-close red"></span></li>');
                    var template = $('#template-table-product').html();
                    var render = '';
                    var kt = false;
                    for (var i = 0; i < listProductAttribute.length; i++)
                        if (listProductAttribute[i] == keyAttr) {
                            kt = true;
                            break;
                        }
                    if (kt == false)
                        listProductAttribute.push(keyAttr);
                    if (listProduct.length == 0) {
                        var productAttributeValue = {
                            Product_AttributeFk: keyAttr,
                            Attribute_value: valueName
                        };
                        var listAttr = [];
                        listAttr.push(productAttributeValue);
                        var product = {
                            ProductCode: $('#txtProductCode').val(),
                            ProductName: valueName,
                            tblProduct_ProductAttributes: listAttr
                        };
                        listProduct.push(product);
                    }
                    else {
                        if (kt == true) {//nếu loại attribute đã tồn tại rồi
                            for (var j = 0; j < listProduct.length; j++) {
                                var product;
                                var temp = listProduct[j].tblProduct_ProductAttributes;
                                var nametmp;
                                var k = 0;
                                for (; k < temp.length; k++) {
                                    if (temp[k].Product_AttributeFk == keyAttr) {
                                        nametmp = temp[k].Attribute_value;
                                        break;
                                    }
                                }
                                if (k > temp.length) {//chưa có giá trị đó
                                    var productAttributeValue = {
                                        Product_AttributeFk: keyAttr,
                                        Attribute_value: valueName
                                    };
                                    listProduct[j].tblProduct_ProductAttributes.push(productAttributeValue);//thêm vào
                                }
                                else {//đã có giá trị đó, tạo mới sản phẩm
                                    var t = [];
                                    for (var m = 0; m < temp.length; m++) {
                                        var a = {
                                            Product_AttributeFk: temp[m].Product_AttributeFk,
                                            Attribute_value: temp[m].Attribute_value
                                        };
                                        t.push(a);
                                    }
                                    t[k].Attribute_value = valueName;
                                    product = {
                                        ProductCode: productCode + '-' + count,
                                        ProductName: listProduct[j].ProductName.replace(nametmp, valueName),
                                        tblProduct_ProductAttributes: t
                                    };
                                    listTemp.push(product);
                                    count++;
                                }

                            }

                        }
                        else//Loại Attribute chưa tồn tại
                        {
                            var productAttributeValue = {
                                Product_AttributeFk: keyAttr,
                                Attribute_value: valueName
                            };
                            for (var i = 0; i < listProduct.length; i++) {
                                listProduct[i].ProductName = listProduct[i].ProductName + '-' + valueName;
                                listProduct[i].tblProduct_ProductAttributes.push(productAttributeValue);
                            }
                        }
                    }
                    if (listTemp.length > 0)
                        listTemp.forEach(function (b) {
                            listProduct.push(b);
                        });
                    listProduct.forEach(function (item) {
                        render += Mustache.render(template, {
                            ProductCode: item.ProductCode,
                            ProductName: item.ProductName
                        });
                    });
                    $('#table-same-product-content').html(render);
                }
                else
                    general.notify('Giá trị đã được chọn!', 'error');

                $(this).parent('.contain-attributeValue').siblings('#txtValue').val('');
                
            }
            else {
                $(this).parent('.contain-attributeValue').siblings('#txtValue').val(valueName);
            }
            $(this).parent('.contain-attributeValue').hide();
        });
        $('body').on('click', 'span.fa-close', function () {
            var template = $('#template-table-product').html();
            var render = '';
            var value = $(this).closest('li').text();
            var dem = 0;
            $(this).closest('ul').children('li').each(function () {
                dem++;
            });
            if (dem > 1) {
                var i = 0;
                while (i < listProduct.length) {
                    var a = listProduct[i].ProductName;
                    if (a.indexOf(value) !== -1) {
                        listProduct.splice(i, 1);
                        i--;
                    }
                    i++;    
                }
            }
            else {
                for (var i = 0; i < listProduct.length; i++) {
                    var productName = listProduct[i].ProductName;
                    if (productName.indexOf('-' + value) !== -1) {
                        listProduct[i].ProductName = listProduct[i].ProductName.replace('-' + value, '');
                        var temp = listProduct[i].tblProduct_ProductAttributes;
                        for (var j = 0; j < temp.length; j++)
                            if (temp[j].Attribute_value == value) {
                                temp.splice(j, 1);
                                break;
                            }
                        if (temp.length == 0)
                            listProduct.splice(i, 1);
                    }
                    else if (productName.indexOf(value + '-') !== -1) {
                        listProduct[i].ProductName = listProduct[i].ProductName.replace(value + '-', '');
                        var temp = listProduct[i].tblProduct_ProductAttributes;
                        for (var j = 0; j < temp.length; j++)
                            if (temp[j].Attribute_value == value) {
                                temp.splice(j, 1);
                                break;
                            }
                        if (temp.length == 0)
                            listProduct.splice(i, 1);
                    }
                    else if (productName.indexOf(value) !== -1)
                        listProduct.splice(i, 1);
                }
            }
            UpdateProductCode(listProduct, $('#txtProductCode').val(), function () {
                listProduct.forEach(function (item) {
                    render += Mustache.render(template, {
                        ProductCode: item.ProductCode,
                        ProductName: item.ProductName
                    });
                });
                $('#table-same-product-content').html(render);
            });
            $(this).closest('li').remove();
        });
        $('#tbl-content').on('click', 'tr', function () {
            if ($(this).children('th').children('.btn-edit-child').length == 0) {
                //$('#circularG').show();
                var index = $(this).index();
                var row = $('#tbl-content #row-parent');
                var id = general.toInt($(this).children('#keyid').text());
                loadDataChild(id, row, index);
                row.eq(index + 1).toggle();
            //$('#circularG').hide();
            }

        });
        $('body').on('click', '#add-same-product', function () {
            isAddNew = false;
            if (gId != 0) {
                productChild = gId;
               
            }
            resetFormMaintainance();
            //var index = $(this).parent('article').parent('.padding-td').parent('tr').index();
            //var code = $('#tbl-content tr').eq(index - 1).data('nchild');
            var code = gNChild+1;
            $.ajax({
                type: "GET",
                url: "/WoodProduct/GetById",
                data: { id: productChild },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    gId = 0;
                    console.log(response);
                    var data = response;
                    //gId = data.KeyId;
                    
                    $('#txtProductCode').val(data.ProductCode+'-'+code);
                    var s = data.ProductName.split('-');
                    $('#txtProductName').val(s[0]);
                    $('#selProductType').val(data.ProductTypeFK);
                    $('#selUnit').val(data.ProductUnit);
                    $('#txtImage').val(data.Image);
                    //if (data.LastUpdateByFk != null)
                    //    $('#txtLastUpdatedByFK').val(data.LastUpdateByFkNavigation.FullName);

                    //$('#dtDateMotified').val(general.dateFormatJson(data.DateModified, true));
                    if (data.Status == 1)
                        $('#chkStatus').prop('checked', true);
                    else
                        $('#chkStatus').prop('checked', false);

                    if (data.is_simple == 1)
                        $('#chkIsSimple').prop('checked', true);
                    else
                        $('#chkIsSimple').prop('checked', false);
                    
                    data.TblProduct_ProductAttributes.forEach(function (item, i) {
                        var template = $('#template-table-load').html();
                        var render = Mustache.render(template, {
                            KeyId:0,
                            Atributes: getAttributeOptions(item.Product_AttributeFk),
                            value: item.Attribute_value,
                        });
                        $('#table-quantity-content').append(render);
                        listAttr.push(item.Product_AttributeFk);
                    });
                    $('.ddlColorId').prop('disabled', true);
                    //$('tfoot').hide();
                    $('#modal-add-edit').modal('show');
                    general.stopLoading();
                },
                error: function (status) {
                    general.notify('Có lỗi xảy ra', 'error');
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
        var productCode = $('#txtProductCode').val();
        var image = $('#txtImage').val();
        var lastUpdatedBy = $('#user').data('userid');
        var lastUpdatedDate = $('#dtDateModified').val();
     
        var a = $("#chkStatus").is(':checked');
        if (a == true)
            status = 1;
        else
            status = 0;
        if (isAddNew)//Nếu là tạo mới
        {
            var data = {
                KeyId: gId,
                ProductName: productName,
                productCode: listProduct.length == 0 ? productCode : "SP",
                ProductTypeFK: productType,
                ProductUnit: productUnit,

                //tblProduct_ProductAttributes: arr,

                Image: image,
                LastUpdateByFk: lastUpdatedBy,
                DateModified: lastUpdatedDate,
                is_simple: false,
                Status: status
            };
            for (var i = 0; i < listProduct.length; i++) {
                listProduct[i].ProductName = productName + '-' + listProduct[i].ProductName;
                listProduct[i].ProductTypeFK = productType;
                listProduct[i].ProductUnit = productUnit;
                listProduct[i].LastUpdateByFk = lastUpdatedBy;
                listProduct[i].is_simple = true;
                listProduct[i].Status = status;
            }
            listProduct.push(data);
            $.ajax({
                type: "POST",
                url: "/WoodProduct/SaveAdd",
                data: {
                    listproductVm: listProduct
                },
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
        else//Cập nhật hoặc thêm thuộc tính cùng loại
        {
            var arr = [];
            $('#table-quantity-content tr').each(function () {
                var attributeFk = $(this).children('#atribute-col').children('.ddlColorId').find('option:selected').val();
                var value = $(this).children('#attributeValue-col').children('.tags-value').children('input').val();
                var keyid = $(this).children('td').children('.btn-delete-quantity').data('id');
                var temp = {
                    KeyId: keyid,
                    product_fk: gId,
                    Product_AttributeFk: attributeFk,
                    Attribute_value: value
                };
                productName += '-'+ value;
                arr.push(temp);
            });
            var data = {
                KeyId: gId,
                ProductCode: productCode,
                ProductName: productName,
                ProductTypeFK: productType,
                ProductUnit: productUnit,
                TblProduct_ProductAttributes: arr,
                Image: image,
                LastUpdateByFk: lastUpdatedBy,
                DateModified: lastUpdatedDate,
                is_simple: true,
                Status: status
            };
            var obj = {
                GN_ProductVm: data,
                IdParent: idParent,
                ListDelete: listKeyIdDel
            };
            $.ajax({
                type: "POST",
                url: "/WoodProduct/SaveUpdate",
                data: {ItemVm: obj},
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    general.notify('Ghi thành công!', 'success');
                    $('#modal-add-edit').modal('hide');
                    resetFormMaintainance();
                    gNChild = '';
                    general.stopLoading();
                    loadData(true);
                },
                error: function (err) {
                    console.log(err);
                    general.notify('Có lỗi trong khi ghi ! ' + err.responseText, 'error');
                    general.stopLoading();
                }
            });
        }
    }
    function checkproductCode(productCode, callback) {
        $.ajax({
            type: 'POST',
            url: '/product/checkproductRef',
            data: {productRef : productCode },
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
        $('#list-same-product').hide();
        listAttr = [];
        listProduct = [];
        listProductAttribute = [];
        listKeyIdDel = [];
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
            success: function (response) {
                cachedObj.attributes = response;
            },
            error: function () {
                general.notify('Có lỗi xảy ra', 'error');
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
    function loadDataChild(id, row, index, isPageChanged) {
        var template = $('#child-template').html();
        var render = '';
        if (id > 0) {
            idParent = id;
            //if (row.eq(index + 1).children('td').children('div').children('table').children('#content-child').children('tr').length == 0) {
                $.ajax({
                    type: 'GET',
                    url: '/WoodProduct/GetListChildren',
                    data: {
                        productId: id,
                        page: general.configs.pageIndex,
                        pageSize: general.configs.pageSize
                    },
                    success: function (res) {
                        res.Results.forEach(function (item) {
                            render += Mustache.render(template, {
                                KeyIdChild: item.Linked_product_id,
                                ProductCodeChild: item.TblProduct_link.ProductCode,
                                ProductNameChild: item.TblProduct_link.ProductName,
                                ProductTypeChild: item.TblProduct_link.ProductTypeNavigation.ProductTypeName,
                                ProductUnitChild: item.TblProduct_link.ProductUnitNavigation.UnitName,
                                ImageChild: item.Image == null ? '<img src="/admin-side/images/user.png" width=25' : '<img src="' + item.TblProduct_link.Image + '" width=25 />',
                                StatusChild: general.getStatus(item.TblProduct_link.Status)
                            });
                        });
                        productChild = res.Results[0].TblProduct_link.KeyId;
                        gNChild = res.RowCount;
                        $('#lblTotalRecordsChild').text(res.RowCount);
                        if (render != '') {
                            //row.eq(index + 1).toggle();
                            row.eq(index + 1).children('td').children('div').children('table').children('#content-child').html(render);
                            row.eq(index + 1).show();
                        }
                        wrapPagingChild(res.RowCount, function () {
                            loadDataChild(id, row, index);
                        }, isPageChanged);
                    },
                    error: function (err) {
                        general.notify('Lấy dữ liệu thất bại!', 'error');
                    }
                });
           // }

        }
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

function loadUnit(callback) {
    $('#selUnit option').remove();
    $.ajax({
        type: 'GET',
        url: '/unit/GetAllIdAndName',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            var defaultId;
            $('#selUnit').append("<option value=''>Chọn đơn vị tính</option>");
            $.each(response, function (i, item) {
                $('#selUnit').append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                
                if (item.Name == general.unitName.Tam) {
                    defaultId = item.Id;
                }
                    
            });
            callback(defaultId);
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
        url: '/WoodProduct/GetAllPaging',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    ProductCode: item.GnProduct_link_parent.length == 0 ? item.ProductCode : item.GnProduct_link_parent.length+' '+item.ProductCode,
                    ProductName: item.ProductName,
                    ProductType: item.ProductTypeNavigation == null ? "" : item.ProductTypeNavigation.ProductTypeName,
                    ProductUnit: item.ProductUnitNavigation.UnitName,
                    Image: item.Image == null ? '<img src="/admin-side/images/user.png" width=25' : '<img src="' + item.Image + '" width=25 />',
                    Status: general.getStatus(item.Status),
                    btnEdit: item.GnProduct_link_parent.length == 0 ? '<button class="btn btn-default btn-sm btn-edit-child" data-id="' + item.KeyId + '"><i class="fa fa-pencil"></i></button>' : '',
                    NOChild: item.GnProduct_link_parent.length
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
function UpdateProductCode(list, code, callback) {
    var count = 0;
    if (list.length>0)
        list[0].ProductCode = code;
    for (var i = 1; i < list.length; i++) {
        list[i].ProductCode = code + '-' + i;
        count++;
    }
    if (count == list.length - 1 || count == 0)
        callback();
}
function wrapPagingChild(recordCount, callBack, changePageSize) {
    var totalsize = Math.ceil(recordCount / general.configs.pageSize);
    //Unbind pagination if it existed or click change pagesize
    if ($('#paginationULChild a').length === 0 || changePageSize === true) {
        $('#paginationULChild').empty();
        $('#paginationULChild').removeData("twbs-pagination");
        $('#paginationULChild').unbind("page");
    }
    //Bind Pagination Event
    $('#paginationULChild').twbsPagination({
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