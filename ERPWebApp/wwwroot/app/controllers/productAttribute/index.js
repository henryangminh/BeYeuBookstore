var productController = function () {
    var cachedObj = {
        attributes: [],
        attributeValues: []
    }
    var stt = 1;
    var arr = 1;
    var gidDelete = [];
    var gId = 0;
    this.initialize = function () {
        loadData();
        loadAttributes();
        registerEvents();
    }

    function registerEvents() {
        $("#txtAttributeCode").attr('maxlength', '25');
        $("#txtAttributeCodeCreate").attr('maxlength', '25');
        $("#txtAttributeName").attr('maxlength', '100');
        $("#txtAttributeNameCode").attr('maxlength', '100');
        $('#frmAttribute').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtAttributeNameCreate: {
                    required: true
                },
                txtAttributeCodeCreate: {
                    required: true
                },
                txtattributevalue: {
                    required: true
                }
            }
        });
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtAttributeCode: {
                    required: true
                },
                txtAttributeName: {
                    required: true
                },
                txtattributevalue1: {
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
            loadData();
        });
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData();
            }
        });
        $("#btnCreate").on('click', function () {
            gId = 0;
            resetAttribute();
            $('#modal-create-attribute').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            stt = 1;
            var render = '';
            $('#txtProductCode').prop('disabled', true);
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/WoodProductAttribute/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    console.log(response);
                    var data = response;
                    gId = data.KeyId;
                    $('#txtAttributeCode').val(data.Code);
                    $('#txtAttributeName').val(data.Name);
                    $('#txtAttributeNote').val(data.note);
                    $('#txtAttributeRequire').prop('checked', data.is_required == 1);

                    data.tblProduct_Attribute_Values.forEach(function (item, i) {
                        var template = $('#template-table-load').html();
                        render += Mustache.render(template, {
                            STT: stt,
                            KeyId: item.KeyId,
                            Atributes: item.ValueName
                        });
                        stt++;
                    })
                    $('#table-atrribute-content').html(render);
                    $('#modal-edit-create').modal('show');
                    general.stopLoading();
                },
                error: function (status) {
                    general.notify('Có lỗi xảy ra', 'error');
                    general.stopLoading();
                }
            });
        });
        $('#btnRefresh-search').on('click', function () {
            datarefresh(function () {
                $('#btnRefresh-search').show();
            });
        });
        $('#btnAttributeSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                console.log(gidDelete);
                var attribute = $('#txtAttributeCode').val();
                if (gId == 0) {
                    checkproductCode(productCode, function (response) {
                        if (response == false)
                            save(attribute);
                        else {
                            general.notify("Mã sản phẩm đã tồn tại.", "error");
                        }
                    });
                }
                else
                    save(attribute);
            }
        });
        $('#btnAttributeCreateNew').on('click', function (e) {
            if ($('#frmAttribute').valid()) {
                e.preventDefault();
                var attributeName = $('#txtAttributeNameCreate').val();
                var attributeCode = $('#txtAttributeCodeCreate').val();
                var attributeNote = $('#txtAttributeNoteCreate').val();

                var is_required = $('#txtAttributeRequireCreate').prop('checked') == true ? true : false;
                if (attributeName != "" && attributeCode != "") {
                    var attributeList = [];
                    var sameList = [];
                    $('#table-atrribute-content-create tr').each(function (i, item) {
                        var obj = {
                            ValueName: $(item).find('#txtattributevalue').first().val(),
                        };
                        if (obj.ValueName != null && obj.ValueName != "")
                            attributeList.push(obj);
                    });
                    console.log(attributeList.length);
                    for (let i = 0; i < attributeList.length; i++) {
                        var temp1 = "";
                        var temp = attributeList[i].ValueName;
                        console.log(temp);
                        for (var j = i + 1; j < attributeList.length; j++) {
                            if (attributeList[j].ValueName == temp) {
                                temp1 = attributeList[j].ValueName;
                                sameList.push(temp1);
                            }
                        }
                    }

                    if (sameList.length > 0) {
                        console.log(sameList);
                        general.notify("Giá trị thuộc tính " + sameList + " bị trùng", "error");
                        attributeList = [];
                        sameList = [];
                        return;
                    }

                    var data = {
                        Code: attributeCode,
                        Name: attributeName,
                        note: attributeNote,
                        is_required: is_required,

                        tblProduct_Attribute_Values: attributeList
                    };
                    if (attributeList.length != 0) {
                        $.ajax({
                            type: "POST",
                            url: "/WoodProductAttribute/SaveEntity",
                            data: { VendorVm: data },
                            dataType: "json",
                            beforeSend: function () {
                                general.startLoading();
                            },
                            success: function (response) {
                                console.log(response);

                                if (response == null) {
                                    general.notify('Mã thuộc tính bị trùng!', 'error');
                                } else {
                                    general.notify('Ghi thành công!', 'success');
                                    $('#modal-create-attribute').modal('hide');
                                    resetAttribute();
                                    general.stopLoading();
                                    loadData(true);
                                }

                                //$('#table-attribute-content').html('');
                                //cachedObj.attributes.push(response);
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

        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            var that = $(this).closest('tr').data('id');
            $(this).closest('tr').remove();

            console.log(that);

            if (that != "") {
                gidDelete.push(that);
            }
        });
        $('body').on('click', '.btn-delete-attribute', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });
        $('#btnSearch').on('click', function () {
            loadData(true);
        });
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData(true);
            }
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
            var render = Mustache.render(template, { STT: stt });
            stt++;
            $('#table-atrribute-content').append(render);
        });
        $('#btn-add-attribute-create').on('click', function () {
            var template = $('#template-table-attribute-create').html();
            var render = Mustache.render(template, { STT: arr });
            arr++;
            $('#table-atrribute-content-create').append(render);
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
        $('#txtAttributeCode').prop('disabled', false);
        var attributeName = $('#txtAttributeName').val();
        var attributeCode = $('#txtAttributeCode').val();
        var attributeNote = $('#txtAttributeNote').val();
        var is_required = $('#txtAttributeRequire').prop('checked') == true ? true : false;

        var attributeList = [];
        var sameList = [];
        i = 1;
        var count = 0;
        $('#table-atrribute-content tr').each(function (i, item) {
            var attributeId = $(this).data('id');
            if ($(item).find('#txtattributevalue1').first().val() != "") {
                if (attributeId == "") {
                    attributeId = 0;
                }
                var obj = {
                    KeyId: attributeId,
                    ValueName: $(item).find('#txtattributevalue1').first().val(),
                };
                if (obj.ValueName != null)
                    attributeList.push(obj);
            }
        });
        for (let i = 0; i < attributeList.length; i++) {
            var temp1 = "";
            var temp = attributeList[i].ValueName;
            console.log(temp);
            for (var j = i + 1; j < attributeList.length; j++) {
                if (attributeList[j].ValueName == temp) {
                    temp1 = attributeList[j].ValueName;
                    sameList.push(temp1);
                }
            }
        }

        if (sameList.length > 0) {
            console.log(sameList);
            general.notify("Giá trị thuộc tính " + sameList + " bị trùng", "error");
            attributeList = [];
            sameList = [];
            return;
        }
        console.log(attributeList);
        console.log(i);

        var data = {
            KeyId: gId,
            Code: attributeCode,
            Name: attributeName,
            note: attributeNote,
            is_required: is_required,

            tblProduct_Attribute_Values: attributeList
        };
        $.ajax({
            type: "POST",
            url: "/WoodProductAttribute/SaveEntity",
            data: { VendorVm: data, gidDelete },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                general.notify('Ghi thành công!', 'success');
                $('#modal-edit-create').modal('hide');
                gidDelete.length = 0;

                general.stopLoading();
                console.log(gidDelete);
                loadData(true);
            },
            error: function () {
                general.notify('Có lỗi trong khi ghi !', 'error');
                general.stopLoading();
            }
        });
    }
    function loadAttributes() {
        $.ajax({
            type: "GET",
            url: "/WoodProductAttribute/GetAllPaging",
            dataType: "json",
            success: function (response) {
                cachedObj.attributes = response;
            },
            error: function () {
                general.notify('Có lỗi xảy ra', 'error');
            }
        });
    }

    function datarefresh() {
        $("#txtKeyword").val("");
        loadData(true);
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
}
function resetAttribute() {
    $('#txtAttributeCodeCreate').val('');
    $('#txtAttributeNameCreate').val('');
    $('#txtAttributeNoteCreate').val('');
    $('#table-atrribute-content-create tr').each(function () { $(this).remove(); });
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
        url: '/WoodProductAttribute/GetAllPaging',
        dataType: 'json',
        success: function (response) {
            console.log(response);
            $.each(response.Results, function (i, item) {
                render += Mustache.render(template, {
                    KeyId: item.KeyId,
                    Code: item.Code,
                    Name: item.Name,
                    Require: general.getCheck(item.is_required),
                    Note: item.note
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