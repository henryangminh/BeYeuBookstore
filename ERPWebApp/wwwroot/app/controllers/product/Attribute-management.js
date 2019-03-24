var AttributeManagement = function () {
   
    var self = this;
    var cachedObj = {
        attributes: [],
        attributeValues: []
    }

    this.initialize = function () {
        loadAttributes();
        loadAttributeValues();
        registerEvents();
    }
    this.loadAtribute = function (key) {
        getAttributeOptions(key);
    }
    function registerEvents() {
        //$('body').on('click', '.btn-quantity', function (e) {
        //    e.preventDefault();
        //    var that = $(this).data('id');
        //    $('#hidId').val(that);
        //    loadQuantities();
        //    $('#modal-quantity-management').modal('show');
        //});
        $('body').on('change', '.ddlColorId', function () {         
            var a = $(this).find('option:selected').text();
            $(this).parent('#atribute-col').siblings('#attributeValue-col').children('#wrapper').children('#singleFieldTags2').tagbox(
                {
                   // hasDownArrow: false,
                  //  data: getAttributeValue(a),
                  //  valueField: 'ValueName',
                  //  textField: 'ValueName',
                  //  limitToList: false,
                    prompt: 'Chọn '+a
                });
            $(this).prop('disabled', true);
        });
        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });

        $('body').on('keyup','#txtAttributeValue', function (e) {
            if ($(this).val() != "") {
                var template = $('#attributeValue-template').html();
                var render = "";
                var a = $(this).parent('.autocomplete').parent('#attributeValue-col').siblings('#atribute-col').children('.ddlColorId').find('option:selected').text();
                $.each(cachedObj.attributes, function (i, item) {
                    if (item.Name == a)
                        $.each(item.tblProduct_Attribute_Values, function (j, temp) {
                            render += Mustache.render(template, {
                                KeyId: temp.KeyId,
                                AttributeValue: temp.ValueName
                            });
                        });
                });
                if (render != '') {
                    $(this).siblings('#contain-listAttributeValue').html(render);
                }
                $(this).siblings('#contain-listAttributeValue').show();
            }
            else
                $(this).siblings('#contain-listAttributeValue').hide();
        });

        $('#btn-add-quantity').on('click', function () {
            var template = $('#template-table-quantity').html();
            var render = Mustache.render(template, {
                KeyId: 0,
                Atributes: getAttributeOptions(null)
            });
            $('#table-quantity-content').append(render);
        });
        $("#btnSaveQuantity").on('click', function () {
            var quantityList = [];
            $.each($('#table-quantity-content').find('tr'), function (i, item) {
                quantityList.push({
                    Id: $(item).data('id'),
                    ProductId: $('#hidId').val(),
                    Quantity: $(item).find('input.txtQuantity').first().val(),
                    SizeId: $(item).find('select.ddlSizeId').first().val(),
                    ColorId: $(item).find('select.ddlColorId').first().val(),
                });
            });
            $.ajax({
                url: '/admin/Product/SaveQuantities',
                data: {
                    productId: $('#hidId').val(),
                    quantities: quantityList
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    $('#modal-quantity-management').modal('hide');
                    $('#table-quantity-content').html('');
                }
            });
        });
       
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

    function loadAttributeValues() {
        return $.ajax({
            type: "GET",
            url: "/ProductAtrributeValue/GetAll",
            dataType: "json",
            success: function (response) {
                cachedObj.attributeValues = response;
            },
            error: function () {
                general.notify('Có lỗi xảy ra', 'error');
            }
        });
    }
    function getAttributeOptions(selectedId) {
        var attributes = "<select class='form-control ddlColorId'>";
        attributes += '<option value="" selected="select">' + "--chọn--" + '</option>';
        $.each(cachedObj.attributes, function (i, attribute) {

            if (selectedId === attribute.KeyId)
                attributes += '<option value="' + attribute.KeyId + '" selected="select">' + attribute.Name + '</option>';
            else
                attributes += '<option value="' + attribute.KeyId + '">' + attribute.Name + '</option>';
        });
        attributes += "</select>";
        return attributes;
    }
    function getAttributeValue(key) {
        var arr = [];
        $.each(cachedObj.attributes, function (i, item) {
            if (item.Name == key) {
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
