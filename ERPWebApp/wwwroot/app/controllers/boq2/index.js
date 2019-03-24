var boq2Controller = function () {
    var gtblListOrderDetail = [];
    var dataMaterialOfProject = [];
    var glistOrderStatus = [];
    var gstatusid = 1;
    var gstatusName = '';
    var gboq1;
    var gProject_FK;
    //bomedetail
    var ListIndexRowUpdate = [];
    var valueNum_old;
    var valueStr_old;
    var gbomcodeFK;
    var _IdRow_Save = -1;
    //material
    var gPrice_old;
    var gStr_old;
    var gKeyId = 0;
    var ListIndexRowUpdateMS = [];
    this.initialize = function () {
        $('#btn-Save-Boq2').hide();
        $('#btn-cancel-Boq2').hide();
        $('#btnRequireCreateBOQ2').hide();
        $('#btnConfirm').hide();
        $('#btnUnConfirm').hide();

        $('#btn-import').hide();
        $('#btn-templates').hide();
        $('#btn-export').hide();
        $('#btn-print').hide();

        registerEvents();
        loadOrderStatus();
        delayStatus(function () {
            //var status = $('#lblSoStatus').text();
            //if (status == "") {
            $('#lblSoStatus').text(gstatusName);
            //}
            //else
            //    $('#lblSoStatus').text('');
            var prop = $('#lblSoStatus').css('visibility');
            if (prop == 'visible')
                $('#lblSoStatus').css({ 'visibility': 'hidden' });
            else
                $('#lblSoStatus').css({ 'visibility': 'visible' });
        }, 500)
    }
    this.loadbyid = function (id, IsView) {
        if (id != 0) {
            general.startLoad();
            $('#txtDescription').prop('disabled', IsView);
            $('#txtK4_rate').prop('disabled', IsView);

            $('#txtK2').prop('disabled', IsView);
            $('#btn-applyK2').prop('disabled', IsView);

            $('#txtK3').prop('disabled', IsView);
            $('#btn-applyK3').prop('disabled', IsView);
            // khóa trong table

            general.startLoad();
            resetForm();
            getValue(id, IsView, function () {
                getDetail(id, function (res) {
                    loadDetail(res);
                    general.stopLoad();
                });
            });

        }
    }
    this.reset_ControlForm = function () {
        resetForm();
    }
    this.importExcel = function (data) {
        console.log(data);
        // load tòa bộ dữ liệu lên lại table( Name, des, qty, note)
        var row = $('#table-quantity-content tr');
        var rowcount = row.length;
        if (data.length < rowcount)
            rowcount = data.length;
        var listError = '';
        if (rowcount > 0) {
           var i = 0;
            while (i < rowcount)
            {
                var code = row.eq(i).children('.col-productCode').find('input').val();
                if (code == data[i].BOMCode_FK) {
                    row.eq(i).children('.productName').children('.tooltip-name').children('#txtProductName').val(data[i].Product_Name);
                    row.eq(i).children('#description').children('#txtDescription').val(data[i].Description);
                    row.eq(i).children('#productqtyB2').children('#txtProductQtyB2').val(data[i].QtyB2);
                    row.eq(i).children('#note').children('#txtNote').val(data[i].Note);
                    var priceTotal = general.toFloat(row.eq(i).children('#totalPriceB2').data('tpriceb2'));
                    var _sum = data[i].QtyB2 * priceTotal;
                    row.eq(i).children('#subTotalB2').data('valueb2', _sum);
                    row.eq(i).children('#subTotalB2').text(general.toMoney(Math.round(_sum)));
                }
                else
                    listError += data[i].BOMCode_FK +', '
                i++;
            }

            if (listError != '')
                general.confirm('Các mã không tồn tại: ' + listError + ' Vui lòng kiểm tra lại !');

            UpdateTotalAll(function (_S) {
                $('#txtSubTotal').text(general.commaSeparateNumber(_S));
                UpdateSumTXT();
            });
        }
    }
    function registerEvents() {
        $('.form_date').datetimepicker({
            language: 'vi',
            weekStart: 1,
            todayBtn: 1,
            autoclose: 1,
            todayHighlight: 1,
            startView: 2,
            minView: 2,
            forceParse: 0
        });
        $("#txtDescription").attr('maxlength', '500');
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtCustomerPONumber: {
                    required: true
                },
                dtCustomerPODate: {
                    required: true
                },
                txtK4_rate: {
                    required: true,
                    number:true
                },
                txtK2: {
                    required: true,
                    number: true
                },
                txtK3: {
                    required: true,
                    number: true
                }
            }
        });
        $('#frmMaintainance-BomDetail').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtPriceB1: {
                    required: true,
                    number:true
                }
            }
        });
        $('#frmMaintainance-Material').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtUnitPriceB2: {
                    required: true,
                    number: true
                }
            }
        });
        //Set Key up
        $('body').on('keyup', '#txtPriceB2', function (event) {
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return value
                    .replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    ;
            });
        });
      
        $('#btn-info').on('click', function () {
            $('.contain-info').toggle();
        });
        $('body').on('change', '#txtProductQtyB2', function () {
           // e.preventDefault();
            var valueQty = general.toFloat($(this).val());
            var unitId = general.toInt($(this).parent('#productqtyB2').siblings('#unitReal').text());
            if (valueQty == 0 || (unitId != general.unit.Note && valueQty < 0))
                $(this).val(valueQty);
            var indexrow = $(this).parent('#productqtyB2').parent('tr').index()+1;
            updateQty(indexrow,null);
        });

        $('body').on('click', '.btn-plus', function () {
            var indexrow = $(this).parent('#btn').parent('tr').index() + 1;
            var a = $(this).data('btn');
            if (a == 1) {
                Zoom(indexrow, false);
                $(this).children('i').addClass('fa-plus').removeClass('fa-minus');
                $(this).data('btn', 0);
            }
            else {
                Zoom(indexrow, true);
                $(this).children('i').addClass('fa-minus').removeClass('fa-plus');
                $(this).data('btn', 1);
            }
        });
        $('body').on('click', '.btn-edit-bomdetail', function () {
            var template = $('#template-bom-detail').html();
            var render = '';
            var idrow = $(this).parent('#contain-btn-edit').siblings('#idrow').text();
            _IdRow_Save = idrow;
            console.log(gtblListOrderDetail);
            for (var j = 0; j < gtblListOrderDetail.length; j++) {
                if (gtblListOrderDetail[j].IdRow == idrow) {
                    var bomdetail = gtblListOrderDetail[j].BoqOrderDetailBoms;
                    gbomcodeFK = gtblListOrderDetail[j].BOMCode_FK;
                    var i = 1;
                    bomdetail.forEach(function (item) {
                        var productType = item.Product_Type;
                        render += Mustache.render(template, {
                            KeyId: i,
                            ProductCode: item.BomDetailFK,
                            ProductName: item.NameBomdetail,
                            ProductUnit: item.UnitFK,
                            UnitName: item.UnitName == null ? item.tblUnit.UnitName : item.UnitName,
                            ProductQty: item.Qty,
                            ProductTypeName: item.tblProductType.ProductTypeName,
                            PriceB2: general.commaSeparateNumber(item.PriceB2),
                            PriceB1: general.commaSeparateNumber(item.PriceB1),
                            Note: item.Note,
                            ProductType: item.Product_Type
                        });
                        i++;
                    });
                    if (render != '')
                        $('#table-bomdetail-content').html(render);
                    break;
                }
            }
            $('#modal-edit-bomdetail').modal('show');
        });
        $('#btn-material-sysnthetic').on('click', function () {
            var template = $('#template-material').html();
            var render = '';
            var i = 1;
            dataMaterialOfProject.forEach(function (item) {
                render += Mustache.render(template, {
                    KeyId: i,
                    ProductCode: item.BomDetailFK,
                    ProductName: item.NameBomdetail,
                    ProductUnit: item.Product_Unit,
                    UnitName: item.UnitName,
                    ProductTypeName: item.ProductTypeName,
                    UnitPriceB2: general.toMoney(item.PriceB2),
                    UnitPriceB1: general.toMoney(item.PriceB1),
                    Note: item.Note,
                    ProductType: item.Product_Type,
                });
                i++;
            });
            if (render != '') {
                $('#table-material-content').html(render);
            }
            $('#modal-edit-material').modal('show');
        });
        $('#chkHidePriceBOQ2').on('change', function () {
            hideColumn($(this));
        });
        $('#chkShowPriceBOQ1').on('change', function () {
            showColumn($(this));
        });
        
        $('#btn-applyK2').on('click', function () {
            if ($('#txtK2').val() != "") {
                var _rate = general.toFloat($('#txtK2').val());
                if (_rate < 0 || _rate >= 100) {
                    $('#txtK2').val('');
                    general.notify("Giá trị K2 thuộc [0;100), vui lòng nhập lại !", 'error');
                    return false;
                }
                //update thông tin bảng projectbomdetail và bảng tổng hợp vật tư
                Update_K2(_rate);
                $('#txtK2').data('k2', _rate);
            }
        });
        $('#btn-applyK3').on('click', function () {
            if ($('#txtK3').val() != "") {
                var _rate = general.toFloat($('#txtK3').val());
                if (0 > _rate || _rate >= 100) {
                    $('#txtK3').val('');
                    general.notify("Giá trị K3 thuộc [0;100), vui lòng nhập lại !", 'error');
                    return false;
                }
                //update thông tin bảng projectbomdetail và bảng tổng hợp vật tư
                UpdateBy_K3(_rate);
                //Cập nhật giá lên dgv
                UpdatePriceOnDgv(gtblListOrderDetail, 0, function () {
                    UpdateTotalAll(function (s) {
                        $('#txtSubTotal').text(general.toMoney(s));
                        UpdateSumTXT();
                        $('#txtK3').data('k3', _rate);
                    });
                });
            }
        });
        $('#txtK4_rate').on('change', function () {
            UpdateSumTXT();
        });
        $('#btnRequireCreateBOQ2').on('click', function () {
            $('#txtNDatePU').val(7);
            $('#dtDateRequestBOQ2PU').data('datetimepicker').setValue(new Date());
            $('#ndate-modal').modal('show');
        });
        $('#btnSaveNDate').on('click', function () {
            var ndate = $('#txtNDatePU').val();
            var requestDateBOQ2='';
            var dtStartDate = $('#dtDateRequestBOQ2PU');
            if (dtStartDate.data('date') != '') // co chon tg
                requestDateBOQ2 = general.dateFormatJson(dtStartDate.data("datetimepicker").getDate(), false);
            var id = $('#txtId').text();
            if (id == '' || ndate == '' || requestDateBOQ2 == '') {
                general.notify('Vui lòng điền đầy đủ thông tin (*) !', 'error');
                return false;
            }
            RequireCreateBOQ2(id, ndate, requestDateBOQ2);
            
        });
        //Save
        $('#btn-Save-Boq2').on('click', function () {
            general.startLoad();
            UpdateTotal(function (s) {
                $('#txtSubTotal').text(general.toMoney(s));
                UpdateSumTXT();
                var keyId = general.toInt($('#txtId').val());
                var qty_K2 = general.toFloat($('#txtK2').data('k2'));
                var price_K3 = general.toFloat($('#txtK3').data('k3'));
                var description = $('#txtDescription').val();
                var subTotalBOQ2 = general.toFloat($('#txtSubTotal').text());
                var provision_K4 = general.toFloat($('#txtK4_rate').val());
                var provision_Amount = general.toFloat($('#txtProvision_Amount').text());
                //var generalExpensesRate = general.toFloat($('#txtGeneralExpensesRate').val());
                var generalExpensesAmountBOQ2 = general.toFloat($('#txtGeneralExpensesAmount').text());
                //var profitRate = general.toFloat($('#txtprofitRate').val());
                var profitAmountBOQ2 = general.toFloat($('#txtProfitAmount').text());
                //var vatRate = general.toFloat($('#txtVATRate').val());
                var taxAmountBOQ2 = general.toFloat($('#txtTax_Amount').text());
                var total_OrderBOQ2 = general.toFloat($('#txtTotal_Order').text());
             
                getOrderDetail(gtblListOrderDetail, function (listOD) {
                    var boq2 = {
                        KeyId: keyId,
                        Description: description,
                        Qty_K2: qty_K2,
                        Price_K3: price_K3,
                        Provision_K4: provision_K4,
                        Provision_Amount: provision_Amount,
                        Provision_AmountUserd:0,
                        GeneralExpensesAmountBOQ2: generalExpensesAmountBOQ2,
                        ProfitAmountBOQ2: profitAmountBOQ2,
                        Tax_AmountBOQ2: taxAmountBOQ2,
                        SubtotalBOQ2: subTotalBOQ2,
                        Total_OrderBOQ2: total_OrderBOQ2,
                       
                        SOStatus_FK: general.soStatus.buildingBoq2,
                        tblBoqOrderDetail: listOD
                    };
                    $.ajax({
                        type: 'POST',
                        url: '/SalesOrderBOQ2/UpdateBOQ2',
                        data: boq2,
                        success: function (res) {
                            general.stopLoad();
                            $('#btn-cancel-Boq2').click();
                            general.notify('Ghi thành công!', 'success');
                        },
                        error: function (err) {
                            general.stopLoad();
                        }
                    });
                });
            });

        });
        //yêu cầu xác nhận BOQ2
        $('#btnRequireConfirm').on('click', function () {
            var keyId = $('#txtId').val();
            if (gstatusid == general.soStatus.buildingBoq2 && keyId > 0) {
                setSoStatusFk(general.soStatus.RequesteConfirmBoq2, function () {
                    RequireConfirm(keyId);
                })
            }
        });
        $('#btnConfirm').on('click', function () {
            if (gstatusid < general.soStatus.buildedBoq2) {
                var keyId = $('#txtId').val();
                //Xác nhận đã hoàn thành BOQ2
                gupdatestatus.projectstatus(keyId, general.projectStatus.buildedBoq2);
                setSoStatusFk(general.soStatus.buildedBoq2, function () {
                    // update bẳng tổng hợp vật tư của boq2
                    // tạo tổng hợp vật tư có tổng khối lượng theo b2
                    updateTHVT(function (data) {
                        Confirm(keyId, data);
                    });
                });
                
                
                
            }
        });
        $('#btnUnConfirm').on('click', function () {
            if (gstatusid == general.soStatus.RequesteConfirmBoq2) {
                var keyId = $('#txtId').val();
                setSoStatusFk(general.soStatus.RequesteCreateBoq2, function () {
                    UnConfirm(keyId);
                });
            }
            else {
                general.notify('Không thể thực hiện được. Vui lòng kiểm tra lại.', 'warning');
            }
        });
        //project bom detail
        $('body').on('focusin', '#frmMaintainance-BomDetail input', function () {
            var id = $(this).attr('id');
            if (id == 'txtPriceB2')
                valueNum_old = general.toFloat($(this).val());
            else if (id == 'txtProductNameDetail' || id == 'txtNoteDetail')
                valueStr_old = $(this).val();
        });
        $('body').on('focusout', '#frmMaintainance-BomDetail input', function () {
            var id = $(this).attr('id');
            var kt = false;
            if (id == "txtPriceB2") {
                var price1_new = general.toMoney($(this).val());
                if (valueNum_old != price1_new) {
                    var unitid = general.toInt($(this).parent('#priceB2').siblings('#productunit').data('unitid'));
                    if (unitid == general.unit.Percent)
                        $(this).val(0);
                    else {
                        kt = true;
                        $(this).val(price1_new);
                    }
                }
            }
            else if (id == "txtProductNameDetail" || id == "txtNoteDetail") {
                var str_new = $(this).val();
                if (str_new != valueStr_old)
                    kt = true;
            }

            if (kt) {
                //kiểm tra xem đã có lưu index đó chưa
                var i = 0;
                var indexrow = $(this).parent('#priceB2').parent('tr').index()+1;
                for (; i < ListIndexRowUpdate.length; i++) {
                    if (ListIndexRowUpdate[i] == indexrow)
                        break;
                }
                if (i >= ListIndexRowUpdate.length)
                    ListIndexRowUpdate.push(indexrow);
            }
        });
        //Show select option when click
        $("input[type='radio']").change(function () {
            if (this.value == 'Bảng phân tích vật tư') {
                $("#material-dropdown").show();
            } else {
                $("#material-dropdown").hide();
            }
        });
        $('#btnSaveDetail').on('click', function () {
            if (ListIndexRowUpdate.length != 0) {
                var row = $('#bomdetail tr');
                var ListUpdateSum = [];
                for (var i = 0; i < ListIndexRowUpdate.length; i++) {
                    //khởi tạo mặc định
                    var indexrow = ListIndexRowUpdate[i];
                    var price = general.toFloat(row.eq(indexrow).children('#priceB2').children('#txtPriceB2').val());
                    var _bomdetailfk = row.eq(indexrow).children('.col-productCode').text();
                    var _NameBomDetail = row.eq(indexrow).children('.productName').children('#txtProductNameDetail').val();
                    var _note = row.eq(indexrow).children('#note').children('#txtNoteDetail').val();
                    var _ProductType = general.toInt(row.eq(indexrow).children('#type').text());
                    // cập nhật lại giá cho bảng tổng hợp vật tư của dự án
                    for (var j = 0; j < dataMaterialOfProject.length; j++) {
                        var item = dataMaterialOfProject[j];
                        if (item.BomDetailFK == _bomdetailfk) {
                            item.NameBomdetail = _NameBomDetail;
                            item.PriceB2 = price;
                            item.Product_Type = _ProductType;
                            break;
                        }
                    }
                    // sửa giá của các mặt hàng trong ProjectBomdetail có chưa mã bomdetail
                    gtblListOrderDetail.forEach(function (temp) {
                        if (temp.BoqOrderDetailBoms != null)
                            if (temp.BoqOrderDetailBoms.length != 0) {
                                var data = temp.BoqOrderDetailBoms;
                                data.forEach(function (item) {
                                    if (item.BomDetailFK == _bomdetailfk) {
                                        item.NameBomdetail = _NameBomDetail;
                                        
                                        //thực hiện sửa giá
                                        item.PriceB2 = price;//giá này đã giảm rồi
                                        item.SubTotalB2 = item.PriceB2 * item.Qty;//tổng này giảm rồi
                     
                                        if (temp.IdRow == _IdRow_Save)
                                            item.Note = _note;//chỉ cập nhật ghi chú cho order detail hiện tại
                                        //thêm vào ds cần update lại giá
                                        var tt = ListUpdateSum.indexOf(temp.IdRow);
                                        if (tt == -1)
                                            ListUpdateSum.push(temp.IdRow);
                                    }
                                });
                            }
                    });
                    
                }
                // cập nhật lại tổng tiền cho các thành phẩm đã bị update
                if (ListUpdateSum.length > 0) {
                    for (var k = 0; k < gtblListOrderDetail.length; k++) {
                        var i = 0;
                        for (; i < ListUpdateSum.length; i++)
                            if (gtblListOrderDetail[k].IdRow == ListUpdateSum[i])
                                break;
                        if (i < ListUpdateSum.length) {
                            //var p1 = 0, p2 = 0, p3 = 0;
                            if (gtblListOrderDetail[k].BoqOrderDetailBoms != null) {
                                var item = gtblListOrderDetail[k].BoqOrderDetailBoms;
                                UpdateSUMBoq2(item, function (abc, p1, p2, p3) {
                                    gtblListOrderDetail[k].Material_PriceB2 = p1;
                                    gtblListOrderDetail[k].Labor_PriceB2 = p2;
                                    gtblListOrderDetail[k].Equipment_PriceB2 = p3;
                                    ListUpdateSum.splice(i, 1);
                                });
                            }
                        }
                        if (ListUpdateSum.length == 0)
                            break;

                    }

                }
                //cập nhật giá lên table
                UpdatePriceOnDgv(gtblListOrderDetail, 0, function () {
                    //cập nhật lại tổng cho toàn bộ bảng (vì giá vật liệu ảnh hưởng đến nhiều bảng.
                    var _S = 0;
                    UpdateTotalAll(function (_S) {
                        $('#txtSubTotal').text(general.commaSeparateNumber(_S));
                        UpdateSumTXT();
                    });
                });
            }
            $('#modal-edit-bomdetail').modal('hide');
        });
        //Tổng hợp vật tư
        $('body').on('focusin', '#frmMaintainance-Material input', function () {
            var id = $(this).attr('id');
            if (id == 'txtProductNameMS' || id == 'txtNoteMS')
                gStr_old = $(this).val();
            else
                if (id == 'txtUnitPriceB2')
                    gPrice_old = general.toFloat($(this).val());
        });
        $('body').on('focusout', '#frmMaintainance-Material input', function () {
            var kt = false;
            var rowindex = $(this).parent('td').parent('tr').index() + 1;
            var id = $(this).attr('id');
            if (id == 'txtUnitPriceB2') {
                var price1_new = general.toFloat($(this).val());
                if (price1_new < 0)
                    price1_new = 0;
                $(this).val(price1_new);
                if (gPrice_old != price1_new) {
                    var unitid = general.toInt($(this).parent('#priceB2').siblings("#unit").data('unitid'));
                    if (unitid == general.unit.Percent)
                        $(this).val(0);
                    else
                        kt = true;
                }
            }
            else if (id == 'txtProductNameMS' || id == 'txtNoteMS') {
                var str_new = $(this).val();
                if (str_new != gStr_old)
                    kt = true;
            }
            //nếu dữ liệu thay đổi
            if (kt) {
                var i = 0;
                for (; i < ListIndexRowUpdateMS.length; i++)
                    if (ListIndexRowUpdateMS[i] == rowindex)
                        break;
                if (i >= ListIndexRowUpdateMS.length)
                    ListIndexRowUpdateMS.push(rowindex);
            }
        });
        $('body').on('click', '#btnPrintReport', function () {
            console.log("Hello world");
            var getCheck = document.querySelector('input[name="rdPtvt"]:checked').value;
            var that = $('#txtId').val();

            if (getCheck == 'Bảng phân tích vật tư') {
                $.ajax({
                    type: "POST",
                    url: "/SalesOrderBOQ2/ExportMaterialAnalyze",
                    data: {
                        id: that,
                        constructionid: $('#txtProjectid').val(),
                        material_id: $('#material-dropdown').val(),
                        material_name: $('#material-dropdown option:selected').text(),
                    },

                    beforeSend: function () {
                    },
                    success: function (response) {
                        console.log(response);
                        window.location.href = response;
                        general.stopLoading();
                    },
                    error: function (status) {
                        console.log('Có lỗi xảy ra', 'error');
                    }
                });
            } else if (getCheck == 'Bảng tổng hợp vật tư') {
                $.ajax({
                    type: "POST",
                    url: "/SalesOrderBOQ2/ExportMaterialDetail",
                    data: {
                        id: that,
                        constructionid: $('#txtProjectid').val()
                    },

                    beforeSend: function () {
                    },
                    success: function (response) {
                        console.log(response);
                        window.location.href = response;
                        general.stopLoading();
                    },
                    error: function (status) {
                        console.log('Có lỗi xảy ra', 'error');
                    }
                });
            } else if (getCheck == 'Biểu giá dự thầu') {
                $.ajax({
                    type: "POST",
                    url: "SalesOrderBOQ2/ExportExcel",
                    data: {
                        soid: $('#txtId').val(),
                        constructionid: $('#txtProjectid').val()
                    },
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
            } else if (getCheck == 'Bảng phân tích định mức hao phí') {
                $.ajax({
                    type: "POST",
                    url: "SalesOrderBOQ2/ExportMaterialUnitPriceDetail",
                    data: {
                        soid: $('#txtId').val(),
                        constructionid: $('#txtProjectid').val()
                    },
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
            }

            ////var win = window.open('/BOQ1Report', '_blank');
            //if (win)
            //    win.focus();
        });
        $('body').on('click', '#material-dropdown', function (e) {
            dropdown = $('#material-dropdown');
            $('#material-dropdown').select2();
            dropdown.empty();
            // Populate dropdown with list of provinces
            for (var i = 0; i < dataMaterialOfProject.length; i++) {
                dropdown.append($('<option></option>').attr('value', dataMaterialOfProject[i].BomDetailFK).text(dataMaterialOfProject[i].BomDetailFK + ' ' + dataMaterialOfProject[i].NameBomdetail));
            }
        });
        $('#btn-apply-material').on('click', function () {
            if (ListIndexRowUpdateMS.length != 0) {
                var row = $('#material tr');
                var listUpdateSO = [];
                for (var i = 0; i < ListIndexRowUpdateMS.length; i++) {
                    var indexrow = ListIndexRowUpdateMS[i];
                    var priceB2 = general.toFloat(row.eq(indexrow).children('#priceB2').children('#txtUnitPriceB2').val());
                    //var priceB2_Old = 0;
                    //var _rate = 100 - general.toFloat($('#txtK1').val());
                    //lấy giá mới theo hàng đã được chỉnh sửa !
                    // giá này là giá có % giảm rồi !
                    var _bomdetailfk = row.eq(indexrow).children('.col-productCode').text();
                    var _NameBomdetail = row.eq(indexrow).children('.productName').children('#txtProductNameMS').val();
                    var _note = row.eq(indexrow).children('#note').children('#txtNoteMS').val();
                    var _ProductType = general.toInt(row.eq(indexrow).children('#type').text());
                    // cập nhật lại giá gốc theo giá mới chỉnh sữa và % giảm
                    //if (_rate != 0)  // % giảm nhỏ hơn 100%
                    //{
                    //    if (priceB1 != 0) //nếu !0
                    //        priceB1_Old = Math.round(priceB1 / _rate * 100);
                    //}
                    // cập nhật lại giá cho bảng tổng hợp vật tư của dự án
                    for (var j = 0; j < dataMaterialOfProject.length; j++) {
                        if (dataMaterialOfProject[j].BomDetailFK == _bomdetailfk) {
                            dataMaterialOfProject[j].NameBomdetail = _NameBomdetail;
                            dataMaterialOfProject[j].PriceB2 = priceB2;
                            dataMaterialOfProject[j].Note = _note;
                            break;
                        }
                    }
                    // sửa giá của các mặt hàng trong dataOrderDetail có chưa mã bomdetail
                    for (var k = 0; k < gtblListOrderDetail.length; k++) {
                        var temp = gtblListOrderDetail[k];
                        if (temp.BoqOrderDetailBoms != null) {
                            if (temp.BoqOrderDetailBoms.length != 0) {
                                var tt = temp.BoqOrderDetailBoms;
                                for (var i = 0; i < tt.length; i++) {
                                    var item = tt[i];
                                    if (item.BomDetailFK == _bomdetailfk) {
                                        item.NameBomdetail = _NameBomdetail;
                                        //thực hiện sửa giá
                                        item.PriceB2 = priceB2;
                                        item.SubTotalB2 = priceB2 * item.Qty;
                                        //item.PriceB1_Old = priceB1_Old;
                                        if (_ProductType != 0)
                                            item.Product_Type = _ProductType;
                                        item.Note = _note;
                                        var od = listUpdateSO.indexOf(temp.IdRow);
                                        if (od == -1)
                                            listUpdateSO.push(temp.IdRow);
                                    }
                                }
                            }
                        }
                    }
                }
                //update lại giá của các thành phẩm 
                // cập nhật lại tổng tiền cho các thành phẩm đã bị update
                if (listUpdateSO.length > 0) {
                    for (var j = 0; j < gtblListOrderDetail.length; j++) {
                        var temp = gtblListOrderDetail[j];
                        var i = 0;
                        for (; i < listUpdateSO.length; i++)
                            if (temp.IdRow == listUpdateSO[i])
                                break;
                        if (i < listUpdateSO.length) {
                            if (temp.BoqOrderDetailBoms != null) {
                                var item = temp.BoqOrderDetailBoms;
                                UpdateSUMBoq2(item, function (abc, p1, p2, p3) {
                                    temp.Material_PriceB2 = p1;
                                    temp.Labor_PriceB2 = p2;
                                    temp.Equipment_PriceB2 = p3;
                                    listUpdateSO.splice(i, 1);
                                });
                            }
                        }
                        if (listUpdateSO.length == 0)
                            break;
                    }
                }
                UpdatePriceOnDgv(gtblListOrderDetail, 0, function () {
                    UpdateTotalAll(function (s) {
                        $('#txtSubTotal').text(general.commaSeparateNumber(s));
                        UpdateSumTXT();
                        $('#modal-edit-material').modal('hide');
                    });
                });
                ListIndexRowUpdateMS = [];
            }

        });

       
    }

    function getValue(id, Isview, callback) {
        if (id != 0) {
            $.ajax({
                type: 'GET',
                url: '/SalesOrderBOQ2/GetById',
                data: { id: id },
                async: false,
                success: function (response) {
                    console.log(response);
                    if (response != null) {
                        gboq1 = response;
                        var data = response;
                        callback();
                        if (data.KeyId > 0) {
                            gKeyId = data.KeyId ;
                            $('#txtId').text(data.KeyId);
                            $('#txtProjectid').data('id', data.ProjectFK);
                            $('#txtCustomerName').text(data.Gnproject.CustomerFkNavigation.UserBy.FullName);
                            $('#txtProjectid').text(data.Gnproject.ConstructionName);
                            $('#dtDateCreated').text(general.dateFormatJson(data.DateCreated, true));
                            $('#txtVersionName').text(data.Version_name);
                            $('#lblSO_ref').text(data.SO_ref);
                            $('#txtDescription').val(data.Description);
                            $('#txtSubTotal').text(general.commaSeparateNumber(data.SubtotalBOQ2));
                            $('#txtGeneralExpensesRate').val(data.GeneralExpensesRateBOQ2);
                            $('#txtGeneralExpensesAmount').text(general.commaSeparateNumber(data.GeneralExpensesAmountBOQ2));
                            $('#txtprofitRate').val(data.ProfitRateBOQ2);
                            $('#txtProfitAmount').text(general.commaSeparateNumber(data.ProfitAmountBOQ2));
                            $('#txtVATRate').val(data.TaxRateBOQ2);
                            $('#txtTax_Amount').text(general.commaSeparateNumber(data.Tax_AmountBOQ2));
                            $('#txtTotal_Order').text(general.commaSeparateNumber(data.Total_OrderBOQ2));
                            $('#txtK2').val(data.Qty_K2);
                            $('#txtK2').data('k2', data.Qty_K2);
                            $('#txtK3').val(data.Price_K3);
                            $('#txtK3').data('k3', data.Price_K3);
                            $('#txtCustomerPONumber').val(data.CustomerPONumber);
                            $('#dtCustomerPODate').val(general.dateFormatJson(data.CustomerPODate, true));
                            $('#txtK4_rate').val(data.Provision_K4);
                            $('#txtProvision_Amount').text(general.toMoney(data.Provision_Amount));
                           
                            $('#dtCustomerPODate').val(general.dateFormatJson(data.CustomerPODate, true));

                            $('#txtNDate').text(data.Ndate);
                            $('#dtDateRequestBOQ2').text(general.dateFormatJson(data.DateRequestBOQ2, true));

                            setSoStatusFk(data.SOStatus_FK, function () {
                                controlButtonByStatusKF(id, gstatusid, Isview);
                            });
                        }
                    }
                    else {
                        general.notify('Lỗi ! Đã gửi phản hồi về lỗi !', 'error');
                    }
                },
                error: function (error) {
                    $('#Page-searchContract').show();
                    $('#Page-mainContract').hide();
                    general.stopLoad();
                    return false;
                }
            });
        }

    }
    function getDetail(id, callback) {

        if (id != 0) {
            $.ajax({
                type: 'GET',
                url: '/SalesOrderBOQ2/GetListDetailWithSoid',
                data: { Soid: id },
                success: function (response) {
                    console.log(response);
                    gtblListOrderDetail = response;
                    callback(response);
                },
                error: function (error) {
                    general.stopLoad();
                }
            });
        }
    }
    function loadDetail(data) {
        gtblListOrderDetail = data;
        var template = $('#template-table-detail').html();
        var render = '';
        data.forEach(function (item) {
            render += Mustache.render(template, {
                KeyId: item.KeyId,
                btn: item.LevelHeader == null ? '' : "<button class='btn btn-xs btn-default btn-plus' data-btn=1 type='button'><i class='fa fa-minus'></i></button>",
                LevelHeader: item.LevelHeader,
                stt: item.stt,
                LevelName: item.NameHeader,
                ProductCode: item.BOMCode_FK,
                ProductName: item.Product_Name,
                Description: item.Description,
                ProductUnit: item.UnitName,
                ProductQty: item.QtyOrdered,
                ProductQtyB2: item.QtyB2 == null ? 0 : item.QtyB2,
                MaterialPrice: general.toMoney(Math.round(item.Material_Price)),
                MaterialPriceB2: general.toMoney(Math.round(item.Material_PriceB2)),
                LaborPrice: general.toMoney(Math.round(item.Labor_Price)),
                LaborPriceB2: general.toMoney(Math.round(item.Labor_PriceB2)),
                EquipmentPrice: general.toMoney(Math.round(item.Equipment_Price)),
                EquipmentPriceB2: general.toMoney(Math.round(item.Equipment_PriceB2)),
                TotalPrice: general.toMoney(Math.round(item.TotalPrice)),
                totalPriceValue: item.TotalPrice,
                TotalPriceB2: general.toMoney(Math.round(item.TotalPriceB2)),
                totalPriceValueB2:item.TotalPriceB2,
                SubTotal: general.toMoney(Math.round(item.Subtotal)),
                SubTotalValue:item.Subtotal,
                SubTotalB2: general.toMoney(Math.round(item.SubtotalB2)),
                SubTotalValueB2: item.SubtotalB2,
                Note: item.Note,
                IdRow: item.IdRow,
                OrderDetailOld: item.KeyId,
                UnitReal: item.UnitFK,
                ParentId: item.Parent_Id
            });
            if (item.BoqOrderDetailBoms != null && item.BoqOrderDetailBoms.length > 0) {
                var projectbomdetail = item.BoqOrderDetailBoms;
                for (var i = 0; i < projectbomdetail.length; i++) {
                    var tt = projectbomdetail[i];
                    var j = 0;
                    var flag = 0;
                    while (j < dataMaterialOfProject.length) {
                        if (tt.BomDetailFK == dataMaterialOfProject[j].BomDetailFK) {
                            flag = 1;
                            break;
                        }
                        else
                            j++;
                    }
                    if (flag == 0) {
                        var ttvl = {
                            SaleOrderFK: gboq1.KeyId,
                            BomDetailFK: tt.BomDetailFK,
                            NameBomdetail: tt.NameBomdetail,
                            UnitFK: tt.UnitFK,
                            UnitName: tt.UnitName,
                            Product_Type: tt.Product_Type,
                            ProductTypeName: tt.tblProductType.ProductTypeName,
                            PriceB1: tt.PriceB1,
                            PriceB2: tt.PriceB2,
                            TotalQtyB2: 0,
                            TotalQtyOrdered: 0,
                            Note: ""
                        };
                        dataMaterialOfProject.push(ttvl);
                    }
                    else
                        tt.TotalQtyB1 += item.QtyOrdered * tt.Qty;
                }
            }
        });
        if (render != '')
            $('#table-quantity-content').html(render);
        //thiết lập loại row
        $('#table-quantity-content tr').each(function () {
            var unitid = general.toInt($(this).children('#unitReal').text());
            if (unitid == general.unit.Header) {
                $(this).addClass('header-style');
            }
            else if (unitid == general.unit.Note) {
                $(this).addClass('note-style');
            }
            else
                $(this).addClass('bom-style');
        });
       
    }
    //load status
    function loadOrderStatus() {
        glistOrderStatus = [];
        $.ajax({
            type: 'GET',
            url: '/SalesOrderBOQ1/GetListStatus',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                if (response.length > 0) {
                    glistOrderStatus = response;
                }
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    var delayStatus = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearInterval(timer)
            timer = setInterval(callback, ms);
        };
    })();
    function setSoStatusFk(statusid, callback) {
        for (var i = 0; i < glistOrderStatus.length; i++)
            if (glistOrderStatus[i].Id == statusid) {
                gstatusid = statusid;
                gstatusName = glistOrderStatus[i].Name;
                callback();
                break;
            }
        return false;
    }
    function controlButtonByStatusKF(id, SOstatus, IsView) {
        if (id > 0) {
            if (IsView) {
                $('#btn-export').show();
                $('#btn-print').show();
                if (SOstatus< general.soStatus.buildedBoq2) {
                    $('#btnConfirm').show();
                    $('#btnUnConfirm').show();
                }
                if (SOstatus < general.soStatus.buildedBoq2)
                    $('#btnRequireCreateBOQ2').show();

                $('#btn-export').show();
                $('#btn-print').show();
            }
            else {
                if (SOstatus < general.soStatus.buildedBoq2) {
                    $('#btn-templates').show();
                    $('#btn-import').show();
                    $('#btn-Save-Boq2').show();
                    $('#btn-cancel-Boq2').show();
                }
            }
        }
    }
    function resetForm() {
        gKeyId = 0;
        $('#txtId').text('');
        $('#txtProjectid').data('id',0);
        $('#txtCustomerName').text('');
        $('#txtProjectid').text('');
        $('#dtDateCreated').text('');
        $('#txtVersionName').text('');
        $('#lblSO_ref').text('');
        $('#txtDescription').val('');
        $('#txtSubTotal').text('');
        $('#txtGeneralExpensesRate').val('');
        $('#txtGeneralExpensesAmount').text('');
        $('#txtprofitRate').val('');
        $('#txtProfitAmount').text('');
        $('#txtVATRate').val('');
        $('#txtTax_Amount').text('');
        $('#txtTotal_Order').text('');
        $('#txtK2').val(0);
        $('#txtK2').data('k2', 0);
        $('#txtK3').val(0);
        $('#txtK3').data('k3',0);
        $('#txtCustomerPONumber').val('');
        $('#dtCustomerPODate').val('');
        $('#txtK4_rate').val(0);
        $('#txtProvision_Amount').text('');
        $('#txtNDate').text('');
        $('#dtDateRequestBOQ2').text('');
        $('#table-quantity-content tr').each(function () {
            $(this).remove();
        });
        $('#chkShowPriceBOQ1').prop('checked', false);
        $('#btn-Save-Boq2').hide();
        $('#btn-cancel-Boq2').hide();
        $('#btnRequireCreateBOQ2').hide();
        $('#btnConfirm').hide();
        $('#btnUnConfirm').hide();
        $('#btn-templates').hide();
        $('#btn-import').hide();
        $('#btn-export').hide();
        $('#btn-print').hide();
        showColumn($('#chkShowPriceBOQ1'));
        gtblListOrderDetail = [];
        dataMaterialOfProject = [];
        gstatusid = 1;
        gstatusName = '';
        gboq1 = '';
        glistod = [];
    }
    function updateQty(indexrow, _qty) {
        var row = $('#table-orderDetail tr');
        var tt = general.toInt(row.eq(indexrow).children('#unitReal').text());
        if (tt != general.unit.Header) {
            var value = general.toFloat(row.eq(indexrow).children('#productqtyB2').children('#txtProductQtyB2').val());
            if (value == 0) {
                row.eq(indexrow).children('#productqtyB2').children('#txtProductQtyB2').css('color', 'Red');
            }
            else {
                row.eq(indexrow).children('#productqtyB2').children('#txtProductQtyB2').css('color', 'Black');
            }
            //update theo unitId
            if (tt == general.unit.Note) {
                SumQtyNoteOfBom(indexrow);
            }
            else //không phải note củng không phải header thì là bom --> công tổng lên header
            {
                UpdateQtyByRow(indexrow, _qty);
                SumBomOfHeader(indexrow);
            }
        }
    }
    function SumQtyNoteOfBom(indexrow) {
        var ret = 0;
        var parent = -1;
        var i = indexrow;
        var row = $('#table-orderDetail tr');
        var rowcount = $('#table-orderDetail tr').length;
        if (i > 0 && i < rowcount) {
            //đi lên cho hết vùng chú thích, lưu lại cha
            while (i >= 0) {
                var tt = general.toInt(row.eq(i).children('#unitReal').text());
                if (tt != 0) {
                    if (tt == general.unit.Note)
                        ret = general.addFloat(ret,general.toFloat(row.eq(i).children('#productqtyB2').children('#txtProductQtyB2').val()),2);
                    else {
                        parent = i;//trên thằng note là cha nó
                        break;
                    }
                }
                i--;
            }
            if (i >= 0)//có tìm thấy cha
            {
                //đi xuống cho hết vùng chú thích
                i = indexrow + 1;
                while (i < rowcount) {
                    var tt = general.toInt(row.eq(i).children('#unitReal').text());
                    if (tt != 0) {
                        if (tt == general.unit.Note)
                            ret = general.addFloat(ret,general.toFloat(row.eq(i).children('#productqtyB2').children('#txtProductQtyB2').val()),2);
                        else
                            break;
                    }
                    i++;
                }
            }
            else //không tìm thấy cha
            {
                ret = 0;
            }
        }
        if (parent != -1)//có cha có tính toán được
        {
            //gán là con của cha nó
            var Id_Parent = general.toInt(row.eq(parent).children('#idrow').text());
            row.eq(indexrow).children('#parentId').text(Id_Parent);
            //update cha nó (theo khối lượng đã thay đổi)
            UpdateQtyByRow(parent, ret);
            // update thay đổi  lên header của cha nó
            SumBomOfHeader(parent);
        }
    }
    //thay đổi khối lượng cập nhật giá
    function UpdateQtyByRow(indexrow, _qty_order) {
        var row = $('#table-orderDetail tr');
        var _qtyOrder;
        if (_qty_order != null)
            _qtyOrder = _qty_order;
        else
            _qtyOrder = general.toFloat(row.eq(indexrow).children('#productqtyB2').children('#txtProductQtyB2').val());
        var _price = general.toFloat(row.eq(indexrow).children('#totalPriceB2').data('tpriceb2'));
        var _subtotal = _qtyOrder * _price;
        row.eq(indexrow).children('#productqtyB2').children('#txtProductQtyB2').val(_qtyOrder);
        row.eq(indexrow).children('#subTotalB2').data('valueb2', _subtotal);
        row.eq(indexrow).children('#subTotalB2').text(general.commaSeparateNumber(Math.round(_subtotal)));//thành tiền đã bao gồm % giảm giá
    }
    function SumBomOfHeader(indexrow) {
        var ret = 0;
        var parent = -1;
        var i = indexrow;
        var rowcount = $('#table-orderDetail tr').length;
        var row = $('#table-orderDetail tr');
        if (i > 0 && i < rowcount) {
            //đi lên cho hết vùng chú thích, lưu lại cha
            while (i >= 0) {
                var tt = general.toInt(row.eq(i).children('#unitReal').text());
                if (tt != general.unit.Header && tt != general.unit.Note) {
                    ret += general.toFloat(row.eq(i).children('#subTotalB2').text());
                }
                else if (tt == general.unit.Header) {
                    parent = i;//trên thằng note là cha nó
                    break;
                }
                i--;
            }
            if (i >= 0)//tìm thấy cha
            {
                var lvHeader = general.toInt(row.eq(parent).children('#levelHeader').children('#txtLevelHeader').val());
                //đi xuống cho hết vùng chú thích
                i = indexrow + 1;
                while (i < rowcount) {
                    var tt = general.toInt(row.eq(i).children('#unitReal').text());
                    if (tt != general.unit.Header && tt != general.unit.Note)
                        ret += general.toFloat(row.eq(i).children('#subTotalB2').text());
                    else if (tt == general.unit.Header) {
                        //tính tổng các header con của nó nếu có
                        while (i < rowcount) {
                            tt = general.toInt(row.eq(i).children('#unitReal').text());
                            if (tt == general.unit.Header) {
                                var lvHeader_Child = general.toInt(row.eq(i).children('#levelHeader').children('#txtLevelHeader').val());
                                if (lvHeader_Child == lvHeader + 1)//là con
                                    ret += general.toFloat(row.eq(i).children('#subTotalB2').text());
                                else if (lvHeader_Child <= lvHeader)
                                    break;
                            }
                            i++;
                        }
                        break;
                    }
                    i++;
                }
            }
            else
                ret = 0;
        }
        if (parent != -1)//có cha có tính toán được
        {
            //gán cha cho nó
            var Id_Parent = row.eq(parent).children('#idrow').text();
            row.eq(indexrow).children('#parentId').text(Id_Parent);
            //update cha cho nó
            var totalParent_old = general.toFloat(row.eq(parent).children('#subTotalB2').text());
            UpdateHeaderToHeaderParent(parent, ret - totalParent_old);

        }

    }
    function UpdateHeaderToHeaderParent(indexrowParent, _effect) {
        var row = $('#table-orderDetail tr');
        var i = indexrowParent;
        var _idParent = general.toInt(row.eq(i).children('#idrow').text());
        var _id;
        while (i >= 0) {
            var total_old = general.toFloat(row.eq(i).children('#subTotalB2').text());
            row.eq(i).children('#subTotalB2').text(general.toMoney(total_old + _effect));
            _id = _idParent;//cha gán bằng con
            _idParent = general.toInt(row.eq(i).children('#parentId').text());//lấy cha mới
            if (_id != _idParent) {
                //tìm index của cha nó
                while (i > 0)
                    if (general.toInt(row.eq(i).children('#idrow').text()) != _idParent)
                        i--;
                    else
                        break;
            }
            else
                break;
        }
    }
    function Zoom(indexrow, IsZoomIn) {
        //kt nếu là header thì làm
        var row = $('#table-orderDetail tr');
        var rowcount = row.length;
        var _unit = general.toInt(row.eq(indexrow).children('#unitReal').text());
        if (_unit == general.unit.Header) {
            var max = rowcount;
            var _levelHeader = general.toInt(row.eq(indexrow).children('#levelHeader').children('#txtLevelHeader').val());
            var i = indexrow + 1;
            while (i < max) {
                var _unit_i = general.toInt(row.eq(i).children('#unitReal').text());
                if (_unit_i == general.unit.Header) {
                    var lv = general.toInt(row.eq(i).children('#levelHeader').children('#txtLevelHeader').val());
                    if (lv <= _levelHeader)
                        break;
                }
                if (IsZoomIn)
                    row.eq(i).show();
                else
                    row.eq(i).hide();
                i++;
            }
        }
    }
    function UpdateSUMBoq2(abc, callback) {
        var Sp1 = 0, Sp2 = 0, Sp3 = 0;
        var i = 0;
        for (; i < abc.length; i++) {
            if (abc[i].UnitFK == general.unit.Percent)// nếu là % thì tính lại tổng trước sau đó mới cập nhật tổng
            {
                var sum = 0;
                var bomtotalfk = abc[i].BOMofTotal_FK;
                if (bomtotalfk == "" || bomtotalfk == undefined || bomtotalfk == null)//Không phải là tổng hợp của các thành phẩm
                {
                    for (var j = 0; j < abc.length; j++)
                        if (abc[j].UnitFK != general.unit.Percent && abc[j].Product_Type == abc[i].Product_Type)
                            sum += abc[j].SubTotalB2;
                }
                else//là tổng hợp của các thành phẩm
                {
                    for (var j = 0; j < abc.length; j++)
                        if (abc[j].UnitFK != general.unit.Percent && abc[j].Product_Type == abc[i].Product_Type && bomtotalfk == abc[j].BOMofTotal_FK)
                            sum += abc[j].SubTotalB2;
                }
                //có tổng rồi thì tính
                var percent = abc[i].Qty;
                abc[i].SubTotalB2 = percent * sum / 100;
            }
            if (abc[i].Product_Type == general.productType.Material)
                Sp1 += abc[i].SubTotalB2;
            else if (abc[i].Product_Type == general.productType.Labor)
                Sp2 += abc[i].SubTotalB2;
            else
                Sp3 += abc[i].SubTotalB2;

        }
        callback(abc, Sp1, Sp2, Sp3);
    }
    //cập nhật giá lên table order detail
    function UpdatePriceOnDgv(listOrderDetail, roundPriceDefault, callback) {
        //truyền lên table
        var rowcount = $('#table-orderDetail tr').length;
        var row = $('#table-orderDetail tr');
        var count = 0;
        listOrderDetail.forEach(function (temp) {
            if (temp.BoqOrderDetailBoms != null)
                if (temp.BoqOrderDetailBoms.length != 0) {
                    var i = 0;
                    for (; i < rowcount; i++)
                        if (general.toInt(row.eq(i).children('#idrow').text()) == temp.IdRow)
                            break;
                    if (i < rowcount)//tồn tại
                    {
                        var _Qty = general.toFloat(row.eq(i).children('#productqtyB2').children('#txtProductQtyB2').val());
                        // cập nhật lại giá và làm tròn giá
                        // xem xet lại việc làm tròn giá nếu ....
                        var p1 = temp.Material_PriceB2;
                        var p2 = temp.Labor_PriceB2;
                        var p3 = temp.Equipment_PriceB2;
                        row.eq(i).children('#materialPriceB2').text(general.commaSeparateNumber(Math.round(p1)));
                        row.eq(i).children('#laborPriceB2').text(general.commaSeparateNumber(Math.round(p2)));
                        row.eq(i).children('#equipmentPriceB2').text(general.commaSeparateNumber(Math.round(p3)));
                        var _sum = p1 + p2 + p3;
                        row.eq(i).children('#totalPriceB2').data('tpriceb2', _sum);
                        row.eq(i).children('#totalPriceB2').text(general.commaSeparateNumber(Math.round(_sum)));
                        var _sub = _sum * _Qty;
                        row.eq(i).children('#subTotalB2').data('valueb2', _sub);
                        row.eq(i).children('#subTotalB2').text(general.commaSeparateNumber(Math.round(_sub)));
                    }
                }
            count++;
        });
        if (count == listOrderDetail.length)
            callback();
    }
    function UpdateTotalAll(callback) {
        var row = $('#table-orderDetail tr');
        var rowcount = row.length;
        var S_Total = 0;
        if (rowcount > 0) {
            //bắt buộc thằng đầu tiên luôn là header và luôn là lv1
            var _Unit = general.toInt(row.eq(1).children('#unitReal').text());
            if (_Unit != general.unit.Header) {
                alert('Cần tiêu đề để tính tổng! Vui lòng kiểm tra lại');
            }
            else {
                //khởi tạo ban đầu
                var last_Stack = 0;
                var listIndexHeader = [];
                var listStack = [];
                var listTotal = [];

                var _id = general.toInt(row.eq(1).children('#idrow').text());
                listIndexHeader.push(0);
                listStack.push(_id);
                listTotal.push(0);
                var i;
                var j = 1;

                while (j < rowcount - 1) {
                    var _idParent_i = listStack[last_Stack];
                    i = listIndexHeader[last_Stack];
                    var _unit;
                    // tìm con của nó và tính tổng totol cho header đó luôn
                    while (j < rowcount) {
                        _unit = general.toInt(row.eq(j).children('#unitReal').text());
                        if (_unit != general.unit.Note)//chú thích thì bỏ qua
                        {
                            var _id_Parent_j = general.toInt(row.eq(j).children('#parentId').text());
                            if (_id_Parent_j == _idParent_i)//j là con của i
                            {
                                if (_unit == general.unit.Header)//là header
                                {
                                    //thêm vào các list
                                    last_Stack++;
                                    listIndexHeader.push(j);
                                    _id = general.toInt(row.eq(j).children('#idrow').text());
                                    listStack.push(_id);
                                    listTotal.push(0);
                                    j++;
                                    break;
                                }
                                else//là bom thì cộng vào
                                {
                                    var _total = general.toFloat(row.eq(j).children('#subTotalB2').text());
                                    listTotal[last_Stack] += _total;
                                }
                            }
                            else //j không còn là con của i nữa (chắc chắn j phải là một header)
                            {
                                //update giá trị cuả i vào bảng
                                row.eq(i).children('#subTotalB2').data('valueb2', listTotal[last_Stack]);
                                row.eq(i).children('#subTotalB2').text(general.commaSeparateNumber(listTotal[last_Stack]));
                                //update giá trị cho cha i trong stack nếu có
                                if (last_Stack > 0)//=0 thì bỏ qua
                                {
                                    listTotal[last_Stack - 1] += listTotal[last_Stack];
                                }
                                else if (last_Stack == 0)//nó là thằng gốc
                                    S_Total += listTotal[last_Stack];
                                //xóa i khỏi stack
                                listIndexHeader.splice(last_Stack, 1);
                                listStack.splice(last_Stack, 1);
                                listTotal.splice(last_Stack, 1);
                                last_Stack--;
                                if (j < rowcount && last_Stack == -1) {
                                    //add j cào stack
                                    _id = general.toInt(row.eq(j).children('#idrow').text());
                                    listIndexHeader.push(j);
                                    listStack.push(_id);
                                    listTotal.push(0);
                                    last_Stack++;
                                    j++;
                                }
                                break;
                            }
                        }
                        j++;
                    }
                }
                //load phần còn lại của stack vào dgv
                while (last_Stack != -1) {
                    i = listIndexHeader[last_Stack];
                    //update giá trị của i vào bảng
                    row.eq(i).children('#subTotalB2').data('valueb2', listTotal[last_Stack]);
                    row.eq(i).children('#subTotalB2').text(general.commaSeparateNumber(listTotal[last_Stack]));
                    //update giá trị cho cha i trong stack (nếu có)
                    if (last_Stack > 0)//=0 thì bỏ qua
                    {
                        listTotal[last_Stack - 1] += listTotal[last_Stack];
                    }
                    else if (last_Stack == 0)
                        S_Total += listTotal[last_Stack];
                    //xóa i khỏi stack
                    listIndexHeader.splice(last_Stack, 1);
                    listStack.splice(last_Stack, 1);
                    listTotal.splice(last_Stack, 1);
                    last_Stack--;
                }
            }
        }
        callback(S_Total);
    }
    function UpdateSumTXT() {
        var _subtotal = general.toFloat($('#txtSubTotal').text());

        var _K4Rate = $('#txtK4_rate').val();
        var _K4Amount = 0;
        if (_K4Rate < 0 || _K4Rate >= 100)
            _K4Rate = 0;
        else
          _K4Amount = Math.round(_subtotal * _K4Rate / 100);

        $('#txtProvision_Amount').text(general.toMoney(_K4Amount));

        var _GeneralExpensesRate = general.toFloat($('#txtGeneralExpensesRate').val());
        _GeneralExpensesRate = general.toRound(_GeneralExpensesRate, 2);
        $('#txtGeneralExpensesRate').val(_GeneralExpensesRate);
        var _GeneralExpensesAmount = Math.round(_subtotal * _GeneralExpensesRate / 100);
        $('#txtGeneralExpensesAmount').text(general.commaSeparateNumber(_GeneralExpensesAmount));

        var _ProfitRate = general.toFloat($('#txtprofitRate').val());
        _ProfitRate = general.toRound(_ProfitRate, 2);
        $('#txtprofitRate').val(_ProfitRate);
        var _ProfitAmount = Math.round((_subtotal + _GeneralExpensesAmount) * _ProfitRate / 100);
        $('#txtProfitAmount').text(general.commaSeparateNumber(_ProfitAmount));

        var _vatRate = general.toFloat($('#txtVATRate').val());
        _vatRate = general.toRound(_vatRate, 2);
        $('#txtVATRate').val(_vatRate);
        var _tax = Math.round((_subtotal + _GeneralExpensesAmount + _ProfitAmount) * _vatRate / 100);
        $('#txtTax_Amount').text(general.commaSeparateNumber(_tax));
        $('#txtTotal_Order').text(general.commaSeparateNumber(_subtotal + _GeneralExpensesAmount + _ProfitAmount + _tax));
    }
    function hideColumn(check) {
        if (check.is(':checked')) {
            $('#materialPrice-th2').hide();
            $('#laborPrice-th2').hide();
            $('#equipmentPrice-th2').hide();
            $('#table-quantity-content tr').each(function () {
                $(this).children('#materialPriceB2').hide();
                $(this).children('#laborPriceB2').hide();
                $(this).children('#equipmentPriceB2').hide();
            });
        }
        else {
            $('#materialPrice-th2').show();
            $('#laborPrice-th2').show();
            $('#equipmentPrice-th2').show();
            $('#table-quantity-content tr').each(function () {
                $(this).children('#materialPriceB2').show();
                $(this).children('#laborPriceB2').show();
                $(this).children('#equipmentPriceB2').show();
            });
        }
    }
    function showColumn(check) {
        if (check.is(':checked')) {
            $('#materialPrice-th1').show();
            $('#laborPrice-th1').show();
            $('#equipmentPrice-th1').show();
            $('#totalPrice-th1').show();
            $('#subtotal-th1').show();
            $('#qty1').show();
            $('#table-quantity-content tr').each(function () {
                $(this).children('#materialPrice').show();
                $(this).children('#laborPrice').show();
                $(this).children('#equipmentPrice').show();
                $(this).children('#totalPrice').show();
                $(this).children('#subTotal').show();
                $(this).children('#productqty').show();
            });
        }
        else {
            $('#materialPrice-th1').hide();
            $('#laborPrice-th1').hide();
            $('#equipmentPrice-th1').hide();
            $('#totalPrice-th1').hide();
            $('#subtotal-th1').hide();
            $('#qty1').hide();
            $('#table-quantity-content tr').each(function () {
                $(this).children('#materialPrice').hide();
                $(this).children('#laborPrice').hide();
                $(this).children('#equipmentPrice').hide();
                $(this).children('#totalPrice').hide();
                $(this).children('#subTotal').hide();
                $(this).children('#productqty').hide();
            });
        }
    }
    function Update_K2(_rate) {
        //tính toán lại Qty theo K2
        $('#table-quantity-content tr').each(function () {
            var qty = general.toFloat($(this).children('#productqty').text());
            var qtyB2 = qty * _rate / 100;
            $(this).children('#productqtyB2').children('#txtProductQtyB2').val(qtyB2);
            var index = $(this).index();
            updateQty(index,null);
        });
        UpdateTotalAll(function (s) {
            $('#txtSubTotal').text(general.toMoney(s));
            UpdateSumTXT();
        });
    }
    function UpdateBy_K3(_rate) {
        var _discount_RateNew = _rate;
        // cập nhật lại toàn bộ giá cho chi tiết ! rồi load lại vào datagridview
        for (var i = 0; i < gtblListOrderDetail.length; i++)
            if (gtblListOrderDetail[i].BoqOrderDetailBoms != null)
                if (gtblListOrderDetail[i].BoqOrderDetailBoms.length != 0) {
                    var HaveUnitPercent = false;
                    var Sump1 = 0;
                    var Sump2 = 0;
                    var Sump3 = 0;
                    var temp = gtblListOrderDetail[i].BoqOrderDetailBoms;
                    for (var j = 0; j < temp.length; j++) {
                        var item = temp[j];
                        if (item.UnitFK == general.unit.Percent) {
                            HaveUnitPercent = true;
                        }
                        var priceB1 = item.PriceB1;
                        var priceB2 = priceB1 * _discount_RateNew / 100;
                        for (var k = 0; k < dataMaterialOfProject.length; k++) {
                            if (dataMaterialOfProject[k].BomDetailFK == item.BomDetailFK) {
                                dataMaterialOfProject[k].PriceB2 = priceB2;
                                break;
                            }
                        }
                        //cập nhật lại giá giảm
                        var qty = item.Qty;
                        item.PriceB2 = priceB2;
                        item.SubTotalB2 = priceB2 * qty;
                        if (!HaveUnitPercent) {
                            //thêm vào giá tổng của bom: giá này ko làm tròn (cộng tổng lại hết mới làm tròn)
                            var type = item.Product_Type;
                            if (type == general.productType.Material)
                                Sump1 += item.SubTotalB2;
                            else if (type == general.productType.Labor)
                                Sump2 += item.SubTotalB2;
                            else
                                Sump3 += item.SubTotalB2;
                        }
                    }
                    if (HaveUnitPercent)
                        UpdateSUMBoq2(temp, function (abc, p1, p2, p3) {
                            gtblListOrderDetail[i].Material_PriceB2 = p1;
                            gtblListOrderDetail[i].Labor_PriceB2 = p2;
                            gtblListOrderDetail[i].Equipment_PriceB2 = p3;
                        });
                    else {
                        gtblListOrderDetail[i].Material_PriceB2 = Sump1;
                        gtblListOrderDetail[i].Labor_PriceB2 = Sump2;
                        gtblListOrderDetail[i].Equipment_PriceB2 = Sump3;
                    }
                }
    }
    function UpdateNDate(id,ndate,requestDateBOQ2,callback) {
        $.ajax({
            type: 'POST',
            url: 'SalesOrderBOQ2/UpdateNDate',
            data: {
                id: id,
                ndate: ndate,
                date: requestDateBOQ2
            },
            success: function (res) {
                callback(res);
            }, error: function (err) {
                general.notify('Lỗi khi ghi số ngày(N).', 'error');
            }
        });
    }
    function UpdateTotal(callback) {
        var i = 0;
        var _sum = 0;
        //Cập nhật thông tin cho total trước
        $('#table-quantity-content tr').each(function () {
            var _unit = general.toInt($(this).children('#unitReal').text());
            var _lv = general.toInt($(this).children('#levelHeader').children('#txtLevelHeader').val());
            if (_unit == general.unit.Header && _lv == 1)
                _sum += general.toFloat($(this).children('#subTotalB2').text());
            i++;
        });
        if (i == $('#table-quantity-content tr').length)
            callback(_sum);
    }
    function getOrderDetail(listOrderDetail, callback) {
        var count = 0;
        $('#table-quantity-content tr').each(function (istt) {
            var keyid = general.toInt($(this).children('#orderDetailOld').text());
            for (var i = 0; i < gtblListOrderDetail.length; i++) {
                var temp = gtblListOrderDetail[i];
                if (temp.KeyId == keyid) {
                    var qtyB2 = general.toFloat($(this).children('#productqtyB2').children('#txtProductQtyB2').val());
                    var totalPrice = temp.Material_PriceB2 + temp.Labor_PriceB2 + temp.Equipment_PriceB2;
                    var subtotal = qtyB2 * totalPrice;
                    var productName = $(this).children('.productName').children('.tooltip-name').children('#txtProductName').val();
                    var note = $(this).children('#note').children('#txtNote').val();
                    var description = $(this).children('#description').children('#txtDescription').val();
                    var totalHeader = general.toFloat($(this).children('#subTotalB2').data('valueb2'));
                    temp.QtyB2 = qtyB2;
                    temp.TotalPriceB2 = totalPrice;
                    if (temp.UnitFK == general.unit.Header)
                        temp.SubtotalB2 = totalHeader;
                    else
                        temp.SubtotalB2 = subtotal;
                    temp.Product_Name = productName;
                    temp.Description = description;
                    temp.Note = note;
                    temp.tblUnit = null;
                    var bomdetail = temp.BoqOrderDetailBoms;
                    if (bomdetail != null)
                        for (var j = 0; j < bomdetail.length; j++) {
                            bomdetail[j].tblUnit = null;
                            bomdetail[j].tblProductType = null;
                        }
                    break;
                }
            }
            count++;
        });
        if (count == $('#table-quantity-content tr').length) {
            callback(listOrderDetail);
        }
    }
    function updateTHVT(callback) {
        var flag = false;
        for (var i = 0; i < gtblListOrderDetail.length; i++) {
            var od = gtblListOrderDetail[i];
            for (var j = 0; j < od.BoqOrderDetailBoms.length; j++) {
                var item = od.BoqOrderDetailBoms[j];
                var k = 0;
                for (; k < dataMaterialOfProject.length; k++)
                    if (dataMaterialOfProject[k].BomDetailFK == item.BomDetailFK) {
                        flag = true;
                        break;
                    }
                if (flag == false) {
                    var ttvl = {
                        SalesOrderFK: keyId,
                        BomDetailFK: item.BomDetailFK,
                        NameBomdetail: item.NameBomdetail,
                        PriceB2: item.PriceB2,
                        TotalQtyB2: od.QtyB2,
                        Note: item.Note
                    };
                    dataMaterialOfProject.push(ttvl);
                }
                else {

                    dataMaterialOfProject[k].TotalQtyB2 += (general.toFloat(od.QtyB2) * general.toFloat(item.Qty));
                }

            }
        }
        callback(dataMaterialOfProject);
    }
    function RequireCreateBOQ2(keyId,ndate,date) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ2/RequireCreateBOQ2',
            data: {
                KeyId: keyId,
                ndate: ndate,
                date: date
            },
            success: function () {
                general.notify('Yêu cầu tạo BOQ2 thành công!', 'success');
                setSoStatusFk(general.soStatus.RequesteCreateBoq2, function () {
                    $('#btnSearch_search').click(); //load lại dữ liệu
                    $('#ndate-modal').modal('hide');
                    $('#Page-mainBoq2').hide();
                    $('#Page-searchBoq2').show();
                });
            },
            error: function (err) {
                general.notify('Có lỗi khi yêu cầu tạo BOQ2', 'error');
            }
        });
    }
    function RequireConfirm(keyId) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ2/RequireConfirm',
            data: { KeyId: keyId },
            success: function () {
                general.notify('Yêu cầu xác nhận BOQ2 thành công!', 'success');
                $('#btn-cancel-Boq2').click();
            },
            error: function (err) {
                general.notify('Có lỗi khi yêu cầu xác nhận BOQ2', 'error');
            }
        });
    }
    function Confirm(keyId,list) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ2/Confirm',
            data: { KeyId: keyId ,list:list},
            success: function () {
                general.notify('Xác nhận BOQ2 thành công!', 'success');
                $('#btn-cancel-Boq2').click();
            },
            error: function (err) {
                general.notify('Có lỗi khi xác nhận BOQ2', 'error');
            }
        });
    }
    function UnConfirm(keyId) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ2/UnConfirm',
            data: { KeyId: keyId },
            success: function () {
                general.notify('Không xác nhận BOQ2 thành công!', 'success');
                $('#btn-cancel-Boq2').click();
            },
            error: function (err) {
                general.notify('Có lỗi khi không xác nhận BOQ2', 'error');
            }
        });
    }
}