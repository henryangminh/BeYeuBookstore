var boq1Controller = function () {
    var gId = 0;
    var glistProject = [];
    var glistOrderStatus = [];
    var gstatusid = general.soStatus.create;
    var gstatusName = 'Test thử';
    var gtblSalesOrder;
    var gtblListOrderDetail = [];//lưu orderdetail
    var dataMaterialOfProject = [];
    var ListCodeOrderDetailDel = [];
    var gIdRow = 1;
    var gListUnit = [];
    //bomdetail
    var valueNum_old;
    var valueStr_old;
    var gbomcodeFK;
    var ListIndexRowUpdate = [];
    var _IdRow_Save = -1;
    //material
    var gPrice_old;
    var gStr_old;
    //Get all array
    var allinfo;
    var ListIndexRowUpdateMS = [];
    this.setSoStatusForgstatus = function (statusid, callback) {
        setSoStatusFk(statusid, callback);
    }
    this.initialize = function () {
        general.startLoading();
        registerEvents();
        this.reloadProject();
        loadOrderStatus();
        delayStatus(function () {
            var status = $('#lblSoStatus').text();
            if (status == "") {
                $('#lblSoStatus').text(gstatusName);
            }
            else
                $('#lblSoStatus').text('');
        }, 500)
        var SO_ref = getUrlParameter('soref');
        var projectId = getUrlParameter('projectId');
        if (SO_ref != '' && SO_ref != undefined && projectId != '' && projectId != undefined) {
            $('#Page-searchBoq1').hide();
            $('#Page-mainBoq1').show();
            LoadInitProject(function (res) {
                resetForm();
                glistProject = res;
                console.log(res);

                $('#lblSO_ref').text(SO_ref);
                document.getElementById('txtProjectid').setAttribute('data-id', projectId);
                for (var i = 0; i < glistProject.length; i++) {
                    if (glistProject[i].Id == projectId) {
                        $('#txtProjectid').val(glistProject[i].Name);
                        $('#txtCustomerName').text(glistProject[i].Des);
                        break;
                    }
                }
                $('#txtProjectid').prop('disabled', true);
                setSoStatusFk(general.soStatus.create, function () {
                    controlButtonByStatusKF(0, gstatusid, false);
                });
            });
        }
        general.stopLoading();
    }
    this.lengthListProject = function (callback) {
        var createProject = [];
        for (var i = 0; i < glistProject.length; i++)
            if (glistProject[i].SoStatusKf == general.projectStatus.create)
                createProject.push(glistProject[i]);
        if (callback != undefined) {
            callback(createProject);
        }
        return createProject.length;
    }
    this.getstatusSo = function () {
        return gstatusid;
    }
    this.reloadProject = function () {
        LoadInitProject(function (ret) {
            glistProject = ret;
            console.log(ret);
        });
    }

    this.loadbyid = function (id, IsView) {
        if (id != 0) {
            general.startLoad();
            gId = id;
            getValue(id, IsView, function () {
                getDetail(id, function (res) {
                    loadDetail(res);

                    general.stopLoad();
                });
            });
        }
        else {
            setSoStatusFk(general.soStatus.create, function () {
                controlButtonByStatusKF(id, gstatusid, IsView);
            });
            this.lengthListProject(function (ret) {
                autocomplete(document.getElementById("txtProjectid"), ret);
            })
        }
    }
    this.confirmTT = function (keyId, callback) {
        ConfirmTT(keyId);
        callback();
    }
    this.reset_ControlForm = function () {
        resetForm();
    }
    this.editPriceAfterImportEx = function (listOrderDetail) {
        EditPriceAfterImportEx(listOrderDetail, function (listod) {
            loadODImport(listod);
        });
    }
    function registerEvents() {
        general.stopLoad();
        $('#txtId').prop('disabled', true);
        $("#txtLastUpdatedByFK").prop('disabled', true);
        $('#btn-update-header').prop('disabled', true);
        $("#dtDateCreated").prop('disabled', true);
        $('.contain-productCode').hide();
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtGeneralExpensesRate: {
                    number: true
                },
                txtprofitRate: {
                    number: true
                },
                txtVATRate: {
                    number: true
                },
                txtK1: {
                    number: true
                },
            }
        });
        $('#frmMaintainance-BomDetail').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtProductNameDetail: {
                    required: true
                },
                txtProductQtyDetail: {
                    required: true,
                    number: true
                },
                txtMaterialPriceDetail: {
                    required: true,
                    number: true
                },
                txtLaborPriceDetail: {
                    required: true,
                    number: true
                },
                txtEquipmentPriceDetail: {
                    required: true,
                    number: true
                },
            }
        });
        $('#btn-info').on('click', function () {
            $('.contain-info').toggle();
        });
        $('#btn-Save-rev').hide();
        $('#btn-Save-boq1').hide();
        $('#btn-cancel-boq1').hide();
        $('#btnConfirm').hide();
        $('#btnUnConfirm').hide();
        $('#btnRequiteConfirm').hide();
        $('#btnConfirmTT').hide();
        $('#btn-import').hide();
        $('#btn-export').hide();
        $('#btn-print').hide();
        var count = 1;
        $('body').on('click', "input", function () {
            $(this).select();
        });
        $('#btn-add').on('click', function () {
            var template = $('#template-table-detail').html();
            var render = Mustache.render(template, {
                KeyId: 0,
                ProductCode: "",
                ProductName: "",
                Description: "",
                ProductUnit: '',
                ProductQty: 0,
                MaterialPrice: 0,
                LaborPrice: 0,
                EquipmentPrice: 0,
                TotalPrice: 0,
                TotalPriceValue: 0,
                SubTotal: 0,
                SubTotalValue: 0,
                Note: ''
            });
            $('#table-quantity-content').append(render);
            var newrow = $('#table-quantity-content tr:last');
            showHideCol(newrow);
        });
        $('body').on('input ', '#txtProductCode', function (e) {
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
        var productCodeBefore;
        $("body").on("focusin", "#txtProductCode", function () {
            productCodeBefore = $(this).val();
        });
        $('body').on('focusout', '#txtProductCode', function () {
            var codeHide = $(this).data('keyid');
            var codeShow = $(this).val();
            if (codeHide != codeShow)
                $(this).val(codeHide);
        });
        $('body').on('focusout', '#txtLevelHeader', function () {
            var temp = $(this);
            var _unit = general.toInt($(this).parent('#levelHeader').siblings('#unitReal').text());
            if (_unit != general.unit.Header)
                $(this).val('');
            else {
                var lv = general.toInt($(this).val());
                var indexrow = $(this).parent('#levelHeader').parent('tr').index();
                if (indexrow == 0) {
                    if (lv != 1) {
                        general.notify('Hàng đầu tiền phải có cấp tiêu đề là 1!', 'warning');
                        $(this).val(1);
                    }
                }
                else {
                    var _level;
                    getLevelHeaderNear(indexrow, function (level) {
                        _level = level + 1;
                        if (lv > _level) {
                            general.notify('Nhập số trong đoạn [1,' + _level + ']. Hãy nhập lại!', 'warning');
                            temp.val(_level);
                        }
                    });
                }
            }
        });
        //sự kiện nhập product name
        $('body').on('focusin', '#txtProductName', function () {
            var name = $(this).val();
            var temp = $(this).parent('.tooltip-name');
            temp.html("<textarea rows='1' id='taProductName'>" + name + "</textarea>");
            $('#taProductName').focus().css({ position: 'absolute', 'background-color': '#f2e6ff', 'z-index': 1 });
            $('#taProductName').css('overflow', 'hidden');
        });
        $('body').on('keydown', '#taProductName', function () {
            var el = $(this);
            setTimeout(function () {
                el.css({ height: 'auto' });
                el.css({ height: el.prop('scrollHeight') + 'px' });
            }, 0);
        });
        $('body').on('focusout', '#taProductName', function () {
            var name = $(this).val();
            var temp = $(this).parent('.tooltip-name');
            temp.html("<input type='text' id='txtProductName' value = '" + name + "'/><span class='tooltiptext'>" + name + "</span>");
        });
        //sư kiện nhập mô tả
        $('body').on('focusin', '#txtDescription', function () {
            var name = $(this).val();
            var temp = $(this).parent('#description');
            temp.html("<textarea rows='1' id='taDescription'>" + name + "</textarea>");
            $('#taDescription').focus().css({ position: 'absolute', 'background-color': '#f2e6ff', 'z-index': 1 });
            $('#taDescription').css('overflow', 'hidden');
        });
        $('body').on('keydown', '#taDescription', function () {
            var el = $(this);
            setTimeout(function () {
                el.css({ height: 'auto' });
                el.css({ height: el.prop('scrollHeight') + 'px' });
            }, 0);
        });
        $('body').on('focusout', '#taDescription', function () {
            var name = $(this).val();
            var temp = $(this).parent('#description');
            temp.html("<input type='text' id='txtDescription' value = '" + name + "'/>");
        });
        //sự kiện nhập chú thích
        $('body').on('focusin', '#txtNote', function () {
            var name = $(this).val();
            var temp = $(this).parent('#note');
            temp.html("<textarea rows='1' id='taNote'>" + name + "</textarea>");
            $('#taNote').focus().css({ position: 'absolute', 'background-color': '#f2e6ff', 'z-index': 1 });
            $('#taNote').css('overflow', 'hidden');
        });
        $('body').on('keydown', '#taNote', function () {
            var el = $(this);
            setTimeout(function () {
                el.css({ height: 'auto' });
                el.css({ height: el.prop('scrollHeight') + 'px' });
            }, 0);
        });
        $('body').on('focusout', '#taNote', function () {
            var name = $(this).val();
            var temp = $(this).parent('#note');
            temp.html("<input type='text' id='txtNote' value = '" + name + "'/>");
        });
        $('body').on('click', '.contain-productCode .ddlproduct', function (e) {
            e.preventDefault();
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
                    that.parent('.contain-productCode').siblings('.code').data('keyid', res.ProductCode);

                    $('.contain-productCode').hide();
                    if (res.ProductCode != productCodeBefore) {
                        var indexrow = temp.parent('tr').index() + 1;
                        if (productCodeBefore != "") {
                            var id = temp.siblings('#idrow').text();
                            var isSave = temp.siblings('#orderDetailOld').text();
                            loadDataRow(res, false, temp, indexrow, function (tblOrderDetail) {
                                //đổi màu nếu là header
                                if (that.parent('.contain-productCode').siblings('.code').val() != "") {
                                    var _unit = general.toInt(temp.siblings('#unitReal').text());
                                    if (_unit == general.unit.Header) {
                                        temp.parent('tr').addClass('header-style');
                                    }
                                    else if (_unit == general.unit.Note) {
                                        temp.parent('tr').addClass('note-style');
                                    }
                                }
                                //tự động chèn dòng mới
                                //var template = $('#template-table-detail').html();
                                //var render = Mustache.render(template, {
                                //    KeyId: 0,
                                //    ProductCode: "",
                                //    ProductName: "",
                                //    Description: "",
                                //    ProductUnit: '',
                                //    ProductQty: 0,
                                //    MaterialPrice: 0,
                                //    LaborPrice: 0,
                                //    EquipmentPrice: 0,
                                //    TotalPrice: 0,
                                //    SubTotal: 0,
                                //    Note: ''
                                //});
                                ////count++;
                                //$('#table-quantity-content tr').eq(indexrow-1).after(render);
                            });
                        }
                        else
                            loadDataRow(res, true, temp, indexrow, function (tblOrderDetail) {
                                //đổi màu nếu là header
                                if (that.parent('.contain-productCode').siblings('.code').val() != "") {
                                    var _unit = general.toInt(temp.siblings('#unitReal').text());
                                    if (_unit == general.unit.Header) {
                                        temp.parent('tr').addClass('header-style');
                                        //that.parent('.contain-productCode').siblings('.code').addClass('header-style');
                                    }
                                    else if (_unit == general.unit.Note) {
                                        temp.parent('tr').addClass('note-style');
                                    }
                                    else {
                                        SumBomOfHeader(indexrow);
                                        temp.parent('tr').addClass('bom-style');
                                    }
                                }
                                //tự động chèn dòng mới
                                //var template = $('#template-table-detail').html();
                                //var render = Mustache.render(template, {
                                //    KeyId: 0,
                                //    ProductCode: "",
                                //    ProductName: "",
                                //    Description: "",
                                //    ProductUnit: '',
                                //    ProductQty: 0,
                                //    MaterialPrice: 0,
                                //    LaborPrice: 0,
                                //    EquipmentPrice: 0,
                                //    TotalPrice: 0,
                                //    SubTotal: 0,
                                //    Note: ''
                                //});
                                ////count++;
                                //$('#table-quantity-content tr').eq(indexrow - 1).after(render);
                            });
                    }
                },
                error: function (err) {
                }
            });
        });
        $('body').on('click', '.btn-edit-bomdetail', function () {
            $('#table-bomdetail-content tr').each(function () {
                $(this).remove();
            });
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
                            MaterialPrice: productType == general.productType.Material ? general.commaSeparateNumber(item.PriceB1) : 0,
                            LaborPrice: productType == general.productType.Labor ? general.commaSeparateNumber(item.PriceB1) : 0,
                            EquipmentPrice: productType == general.productType.Equipment ? general.commaSeparateNumber(item.PriceB1) : 0,
                            Note: item.Note,
                            ProductType: item.Product_Type,
                            DiscountRate: item.Discount_Rate
                        });
                        i++;
                    });
                    if (render != '')
                        $('#table-bomdetail-content').html(render);
                }
            }
            $('#modal-edit-bomdetail').modal('show');
        });
        $('body').on('click', '.btn-delete-bom', function (e) {
            e.preventDefault();
            var _s = 0;
            var rowindex = $(this).parent('#contain-btndel').parent('tr').index() + 1;
            var rowcount = $('#table-orderDetail tr').length;
            var row = $('#table-orderDetail tr');
            var unitid = general.toInt(row.eq(rowindex).children('#unitReal').text());
            var unitBefore = general.toInt(row.eq(rowindex - 1).children('#unitReal').text());
            var unitAfter = general.toInt(row.eq(rowindex + 1).children('#unitReal').text());

            Delete_Row(rowindex, row);
            if (unitid != general.unit.Note) {
                rowindex++;
                while (rowindex < rowcount && general.toInt(row.eq(rowindex).children('#unitReal').text()) == general.unit.Note) {
                    Delete_Row(rowindex, row);
                    rowindex++;
                }
            }
            else
                SumQtyNoteOfBom(rowindex);
            if (unitBefore != general.unit.Note && unitAfter != general.unit.Note)
                row.eq(rowindex - 1).children('#productqty').children('#txtProductQty').prop('readonly', false);
            // xóa xong hết, chỉnh sửa lại ref
            if ($('#table-orderDetail tr').length > 1) {
                UpdateParentIdByLevelHeader(function () {
                    UpdateTotalAll(function (s) {
                        $('#txtSubTotal').text(general.commaSeparateNumber(s));
                        UpdateSumTXT();
                    });
                });
            }
        });
        $('body').on('change', '#txtProductQty', function () {
            var qty = general.toFloat($(this).val());
            var totalPrice = general.toFloat($(this).parent('#productqty').siblings('#totalPrice').data('value'));
            var indexrow = $(this).parent('#productqty').parent('tr').index() + 1;
            var unitId = general.toInt($(this).parent('#productqty').siblings('#unitReal').text());
            var unitName = $(this).parent('#productqty').siblings('#productunit').text();
            if (qty == 0 || (unitId != general.unit.Note && qty < 0))
                $(this).val(0);
            //else
            //$(this).val(unitround.RoundByUnit(qty, unitName));
            updateQty(indexrow, null);
            //var subtotal = qty * totalPrice;
            //$(this).parent('#productqty').siblings('#subTotal').text(general.commaSeparateNumber(subtotal));
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
        //----------------------Set Key Up ----------------------------
        $('body').on('keyup','#txtMaterialPriceDetail', function (event) {
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return value
                    .replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    ;
            });
        });
        $('body').on('keyup', '#txtProductQtyDetail', function (event) {
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return value
                    .replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    ;
            });
        });
        $('body').on('keyup', '#txtEquipmentPriceDetail', function (event) {
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return value
                    .replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    ;
            });
        });
        $('body').on('keyup', '#txtLaborPriceDetail', function (event) {
            if (event.which >= 37 && event.which <= 40) return;

            $(this).val(function (index, value) {
                return value
                    .replace(/\D/g, "")
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                    ;
            });
        });

        $('#txtGeneralExpensesRate').on('change', function () {
            UpdateSumTXT();
        });
        $('#txtprofitRate').on('change', function () {
            UpdateSumTXT();
        });
        $('#txtVATRate').on('change', function () {
            UpdateSumTXT();
        });
        $('#btn-applyK1').on('click', function () {
            if ($('#txtK1').val()) {
                var _rate = general.toFloat($('#txtK1').val());
                if (0 > _rate || _rate >= 100) {
                    $('#txtK1').val('');
                    general.notify("Giá trị K1 thuộc [0;100), vui lòng nhập lại !", 'error');
                    return false;
                }
                general.startLoad();
                //update thông tin bảng projectbomdetail và bảng tổng hợp vật tư
                UpdateBy_K1(_rate, gtblListOrderDetail, dataMaterialOfProject);
                //Cập nhật giá lên datagridview
                UpdatePriceOnDgv(gtblListOrderDetail, 0, function () {
                    UpdateTotalAll(function (s) {
                        $('#txtSubTotal').text(general.commaSeparateNumber(s));
                        UpdateSumTXT();
                        general.stopLoad();
                    });
                });
            };
        });
        $('#btn-update-header').on('click', function () {
            if ($('#table-quantity-content tr').length > 0) {
                general.startLoad();
                UpdateParentIdByLevelHeader(function () {
                    UpdateTotalAll(function (s) {
                        $('#txtSubTotal').text(general.toMoney(s));
                        UpdateSumTXT();
                        if (gstatusid !== general.soStatus.create)
                            $('#btn-Save-rev').show();
                        $('#btn-Save-boq1').show();
                        // ẩn COL level header
                        $('#levelHeader-th').hide();
                        $('#table-quantity-content tr').each(function () {
                            $(this).children('#levelHeader').hide();
                            var lv = general.toInt($(this).children('#levelHeader').children('#txtLevelHeader').val());
                            if (lv == 0)
                                $(this).show();
                            else {
                                $(this).children('#btn').children('.btn-plus').children('i').addClass('fa-minus').removeClass('fa-plus');
                                $(this).children('#btn').children('.btn-plus').data('btn', 1);
                            }
                        });
                        general.stopLoad();
                    });
                });
            }

            $('#chkEditHeader').prop('checked', false);
            $('#chkEditHeader').prop('disabled', false);
            $('#btn-update-header').prop('disabled', true);
            $('#btn-Save-boq1').show();
            if (gId > 0)
                $('#btn-Save-rev').show();
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
                    UnitPrice: general.commaSeparateNumber(item.priceB1),
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
        var indexrow;
        $('#table-orderDetail').on('contextmenu', 'tr', function (e) {
            e.preventDefault();
            indexrow = $(this).index();
            $("#contextMenu").css({
                display: "block",
                left: e.pageX,
                top: e.pageY
            });
            return false;
        });

        $(document).click(function (e) {
            if (!$(e.target).is('#contextMenu ,.contain-productCode')) {
                $("#contextMenu").hide();
                $('.contain-productCode').hide();
            }
        });
        $('body').on('click', '#add', function () {
            var template = $('#template-table-detail').html();
            var render = Mustache.render(template, {
                KeyId: 0,
                ProductCode: "",
                ProductName: "",
                Description: "",
                ProductUnit: '',
                ProductQty: 0,
                MaterialPrice: 0,
                LaborPrice: 0,
                EquipmentPrice: 0,
                TotalPrice: 0,
                SubTotal: 0,
                Note: ''
            });
            //count++;
            $('#table-quantity-content tr').eq(indexrow).before(render);
            showHideCol($('#table-quantity-content tr').eq(indexrow));
        });
        $('#chkEditHeader').on('change', function () {
            hideLevelColumn($(this));
        });
        $('#chkHidePrice').on('change', function () {
            hideColumn($(this));
        });
        $('body').on('mouseover', '.tooltip-name', function () {
            var s = $(this).children('span').text();
            if (s.length > 22)
                $(this).children('span').css('visibility', 'visible');
        });
        $('body').on('mouseout', '.tooltip-name', function () {
            $(this).children('span').css('visibility', 'hidden');
        });
        $('#btnRequiteConfirm').on('click', function () {
            var keyId = $('#txtId').val();
            if (gstatusid == general.soStatus.create && keyId > 0) {
                setSoStatusFk(general.soStatus.RequestedConfirmBoq1, function () {
                    RequireConfirm(keyId);
                })
            }
        });
        $('#btnConfirm').on('click', function () {
            var keyId = $('#txtId').val();
            if (keyId > 0 && gstatusid != general.soStatus.CompletedBoq1) {
                if ($("#lblSO_ref").text() != "") { // là phụ lục
                    setSoStatusFk(general.soStatus.CompletedBoq1, function () {
                        ConfirmTT(keyId);
                        $('#btnConfirmTT').hide();
                    });
                }
                else {
                    setSoStatusFk(general.soStatus.ConfirmedBoq1, function () {
                        Confirm(keyId);
                    });
                }
                $(this).hide();
                $('#btnUnConfirm').hide();
                boq1Search.initData();
            }
        });
        $('#btnUnConfirm').on('click', function () {
            var keyId = $('#txtId').val();
            if (keyId > 0 && gstatusid != general.soStatus.CompletedBoq1) {
                setSoStatusFk(general.soStatus.NoConfirmBoq1, function () {
                    UnConfirm(keyId);
                    $(this).hide();
                    $('#btnConfirmTT').hide();
                    ('#btnConfirm').hide();
                });
            }
        });
        $('#btn-print').on('click', function () {
        });
        //Show select option when click
        $("input[type='radio']").change(function () {
            if (this.value == 'Bảng phân tích vật tư') {
                $("#material-dropdown").show();
            } else {
                $("#material-dropdown").hide();
            }
        });

        //----------------event button main ---------------------
        $('#btn-Save-boq1').on('click', function () {
            if ($('#frmMaintainance').valid() && $('#frmMaintainance-BomDetail').valid()) {
                general.startLoad();
                var table_len = $('#table-quantity-content tr').length;
                deleteEmptyRow(table_len, function (s, len) {
                    if (len > 0) {
                        //kiểm tra thằng đầu tiên có phải header ko nếu ko thì báo lỗi thoát ?
                        var Unitid_Row0 = general.toInt($('#table-quantity-content tr').eq(0).children('#unitReal').text());
                        if (Unitid_Row0 != general.unit.Header) {
                            general.notify('Vui lòng nhập hàng đầu tiên là một \"TIÊU ĐỀ\"');
                            general.stopLoad();
                            return false;
                        }
                        $('#txtSubTotal').text(Math.round(s));
                        UpdateSumTXT();
                        var keyId = general.toInt($('#txtId').val());
                        var projectId = document.getElementById('txtProjectid').getAttribute('data-id');
                        var versionName = $('#txtVersionName').val();
                        var soRef = $('#lblSO_ref').text();
                        var description = $('#txtDescription').val();
                        var subTotal = general.toFloat($('#txtSubTotal').text());
                        var generalExpensesRate = general.toFloat($('#txtGeneralExpensesRate').val());
                        var generalExpensesAmount = general.toFloat($('#txtGeneralExpensesAmount').text());
                        var profitRate = general.toFloat($('#txtprofitRate').val());
                        var profitAmount = general.toFloat($('#txtProfitAmount').text());
                        var vatRate = general.toFloat($('#txtVATRate').val());
                        var taxAmount = general.toFloat($('#txtTax_Amount').text());
                        var total_Order = general.toFloat($('#txtTotal_Order').text());
                        var discountRate = general.toFloat($('#txtK1').val());
                        //var userId = $('#user').data('userid');
                        getOrderDetail(gtblListOrderDetail, function (listOD) {
                            var boq1 = {
                                KeyId: keyId,
                                SOStatus_FK: gstatusid,
                                ProjectFK: projectId,
                                Version_name: versionName,
                                Description: description,
                                SO_ref: soRef,
                                GeneralExpensesRate: generalExpensesRate,
                                ProfitRate: profitRate,
                                TaxRate: vatRate,
                                Discount_K1: discountRate,
                                GeneralExpensesAmount: generalExpensesAmount,
                                ProfitAmount: profitAmount,
                                Tax_Amount: taxAmount,
                                Subtotal: subTotal,
                                Total_Order: total_Order,
                                //CreatedBy_FK: userId,
                                //LastUpdatedBy_FK: userId,
                                maxIdOrderdetail: gIdRow,
                                tblBoqOrderDetail: listOD
                            };
                            if (ListCodeOrderDetailDel.length != 0 && keyId != 0)
                                DeleteOrderDetail(ListCodeOrderDetailDel, function (res) {
                                    Save(boq1);
                                });
                            else
                                Save(boq1);
                            return false;
                        });
                    }
                });
            }
        });
        $('#btn-Save-rev').on('click', function () {
            if ($('#frmMaintainance').valid() && $('#frmMaintainance-BomDetail').valid()) {
                var table_len = $('#table-quantity-content tr').length;
                deleteEmptyRow(table_len, function (s, len) {
                    if (len > 0) {
                        //kiểm tra thằng đầu tiên có phải header ko nếu ko thì báo lỗi thoát ?
                        var Unitid_Row0 = general.toInt($('#table-quantity-content tr').eq(0).children('#unitReal').text());
                        if (Unitid_Row0 != general.unit.Header) {
                            general.notify('Vui lòng nhập hàng đầu tiên là một \"TIÊU ĐỀ\"');
                            return false;
                        }

                        //var keyId = $('#txtId').val();
                        var projectId = document.getElementById('txtProjectid').getAttribute('data-id');
                        var versionName = $('#txtVersionName').val();
                        var soRef = $('#lblSO_ref').text();
                        var description = $('#txtDescription').val();
                        var subTotal = general.toFloat($('#txtSubTotal').text());
                        var generalExpensesRate = general.toFloat($('#txtGeneralExpensesRate').val());
                        var generalExpensesAmount = general.toFloat($('#txtGeneralExpensesAmount').text());
                        var profitRate = general.toFloat($('#txtprofitRate').val());
                        var profitAmount = general.toFloat($('#txtProfitAmount').text());
                        var vatRate = general.toFloat($('#txtVATRate').val());
                        var taxAmount = general.toFloat($('#txtTax_Amount').text());
                        var total_Order = general.toFloat($('#txtTotal_Order').text());
                        var discountRate = general.toFloat($('#txtK1').val());
                        //var userId = $('#user').data('userid');
                        getOrderDetail(gtblListOrderDetail, function (listOD) {
                            //chỉnh sửa keyid và orderdetail key trong boq1orderdetailbom
                            for (var i = 0; i < listOD.length; i++) {
                                listOD[i].KeyId = 0;
                                listOD[i].SO_FK = 0;
                                if (listOD[i].BoqOrderDetailBoms.length > 0) {
                                    var item = listOD[i].BoqOrderDetailBoms;
                                    if (item != null)
                                        for (var j = 0; j < item.length; j++) {
                                            item[j].KeyId = 0;
                                            item[j].OrderDetailFK = 0;
                                        }
                                }
                            }
                            var boq1 = {
                                KeyId: 0,
                                SOStatus_FK: general.soStatus.create,
                                ProjectFK: projectId,
                                Version_name: versionName,
                                Description: description,
                                SO_ref: soRef,
                                GeneralExpensesRate: generalExpensesRate,
                                ProfitRate: profitRate,
                                TaxRate: vatRate,
                                Discount_K1: discountRate,
                                GeneralExpensesAmount: generalExpensesAmount,
                                ProfitAmount: profitAmount,
                                Tax_Amount: taxAmount,
                                Subtotal: subTotal,
                                Total_Order: total_Order,
                                //CreatedBy_FK: userId,
                                //LastUpdatedBy_FK: userId,
                                maxIdOrderdetail: gIdRow,
                                tblBoqOrderDetail: listOD
                            };
                            $.ajax({
                                type: 'POST',
                                url: '/SalesOrderBOQ1/SaveEntity',
                                data: boq1,
                                dataType: "json",
                                beforeSend: function () {
                                    general.startLoading();
                                },
                                success: function (res) {
                                    console.log(res);
                                    setSoStatusFk(general.soStatus.create, function () { })
                                    general.notify('Ghi thành công!', 'success');
                                    //ghi mới
                                    $('#btn-cancel-boq1').click();
                                    general.stopLoading();
                                },
                                error: function (error) {
                                    general.notify('Có lỗi trong khi ghi!', 'error');
                                    general.stopLoading();
                                }
                            });
                            return false;
                        });
                    }
                });
            }
            //sau khi kết thúc việc lưu
            $('#btn-cancel-boq1').click();
        });

        //project bom detail
        $('body').on('focusin', '#frmMaintainance-BomDetail input', function () {
            var id = $(this).attr('id');
            if (id == 'txtMaterialPriceDetail' || id == 'txtLaborPriceDetail' || id == 'txtEquipmentPriceDetail')
                valueNum_old = general.toFloat($(this).val());
            else if (id == 'txtProductQtyDetail')
                valueNum_old = general.toFloat($(this).val());
            else if (id == 'txtProductNameDetail' || id == 'txtNoteDetail')
                valueStr_old = $(this).val();
        });

        $('body').on('focusout', '#frmMaintainance-BomDetail input', function () {
            var rowindex = $(this).parent('td').parent('tr').index() + 1;
            var kt = false;
            var id = $(this).attr('id');
            var productQtyId = general.toMoney('txtProductQtyDetail');
            var laborPriceId = general.toMoney('txtLaborPriceDetail');
            var equipmentPriceId = general.toMoney('txtEquipmentPriceDetail');
            var noteId = 'txtNoteDetail';
            var productnameId = 'txtProductNameDetail';
            var materialPriceId = general.toMoney('txtMaterialPriceDetail');
            if (id == productQtyId || id == materialPriceId || id == laborPriceId || id == equipmentPriceId) {
                var price1_new = general.toFloat($(this).val());
                if (price1_new < 0)
                    price1_new = 0;
                $(this).val(price1_new);
                if (valueNum_old != price1_new)//Giá trị chỉnh sửa có bị thay đổi không
                {
                    var unitid = $(this).parent('td').siblings('#productunit').data('unitid');
                    if (unitid == general.unit.Percent && id != productQtyId) {
                        $(this).val(0);
                    }
                    else {
                        kt = true;
                        if (id == materialPriceId) {
                            $(this).parent('#material').siblings('#labor').children('#' + laborPriceId).val(0);
                            $(this).parent('#material').siblings('#equipment').children('#' + equipmentPriceId).val(0);
                            $(this).parent('#material').siblings('#type').text(general.productType.Material);
                        }
                        else if (id == laborPriceId) {
                            $(this).parent('#labor').siblings('#material').children('#' + materialPriceId).val(0);
                            $(this).parent('#labor').siblings('#equipment').children('#' + equipmentPriceId).val(0);
                            $(this).parent('#labor').siblings('#type').text(general.productType.Labor);
                        }
                        else if (id == equipmentPriceId) {
                            $(this).parent('#equipment').siblings('#labor').children('#' + laborPriceId).val(0);
                            $(this).parent('#equipment').siblings('#material').children('#' + materialPriceId).val(0);
                            $(this).parent('#equipment').siblings('#type').text(general.productType.Equipment);
                        }
                        //khối lượng thay đổi thì không cần làm gì
                    }
                }
            }
            else if (id == productnameId || id == noteId) {
                var str_new = $(this).val();
                if (str_new != valueStr_old)
                    kt = true;
            }
            //nếu dữ liệu có thay đổi
            if (kt) {
                //kiểm tra xem đã có lưu index đó chưa
                var i = 0;
                for (; i < ListIndexRowUpdate.length; i++)
                    if (ListIndexRowUpdate[i] == rowindex)
                        break;
                if (i >= ListIndexRowUpdate.length)//chưa tồn tại
                    ListIndexRowUpdate.push(rowindex);
            }
        });

        $('#btnSaveDetail').on('click', function () {
            if (ListIndexRowUpdate.length != 0)//có dữ liệu được update
            {
                var productQtyId = '#txtProductQtyDetail';
                var laborPriceId = '#txtLaborPriceDetail';
                var equipmentPriceId = '#txtEquipmentPriceDetail';
                var noteId = '#txtNoteDetail';
                var productnameId = '#txtProductNameDetail';
                var materialPriceId = '#txtMaterialPriceDetail';
                var row = $('#bomdetail tr');
                var ListUpdateSum = [];
                ListIndexRowUpdate.forEach(function (indexrow) {
                    var p1 = general.toFloat(row.eq(indexrow).children('#material').children(materialPriceId).val());
                    var p2 = general.toFloat(row.eq(indexrow).children('#labor').children(laborPriceId).val());
                    var p3 = general.toFloat(row.eq(indexrow).children('#equipment').children(equipmentPriceId).val());
                    var p1_Old = 0;
                    var p2_Old = 0;
                    var p3_Old = 0;
                    var qty = general.toFloat(row.eq(indexrow).children('#productqty').children(productQtyId).val());
                    var _rate = general.toFloat(row.eq(indexrow).children('#discountRate').text());
                    //lấy giá mới theo hàng đã được chỉnh sửa !
                    // giá này là giá có % giảm rồi !
                    var _bomdetailfk = row.eq(indexrow).children('.col-productCode').text();
                    var _NameBomdetail = row.eq(indexrow).children('.productName').children(productnameId).val();
                    var _note = row.eq(indexrow).children('#note').children(noteId).val();
                    var _ProductType = general.toInt(row.eq(indexrow).children('#type').text());
                    // cập nhật lại giá gốc theo giá mới chỉnh sữa và % giảm
                    if (_rate != 0)//% giảm nhỏ hơn 100%
                    {
                        if (p1 != 0)
                            p1_Old = Math.round(p1 / _rate * 100);
                        if (p2 != 0)
                            p2_Old = Math.round(p2 / _rate * 100);
                        if (p3 != 0)
                            p3_Old = Math.round(p3 / _rate * 100);
                    }
                    // cập nhật lại giá cho bảng tổng hợp vật tư của dự án
                    dataMaterialOfProject.forEach(function (item) {
                        if (item.BomDetailFK == _bomdetailfk) {
                            item.NameBomdetail = _NameBomdetail;
                            item.priceB1 = p1 + p2 + p3;
                            item.Product_Type = _ProductType;
                            //break;
                        }
                    });
                    // sửa giá của các mặt hàng trong ProjectBomdetail có chưa mã bomdetail
                    gtblListOrderDetail.forEach(function (temp) {
                        if (temp.BoqOrderDetailBoms != null)
                            if (temp.BoqOrderDetailBoms.length != 0) {
                                var data = temp.BoqOrderDetailBoms;
                                data.forEach(function (item) {
                                    if (item.BomDetailFK == _bomdetailfk) {
                                        item.NameBomdetail = _NameBomdetail;
                                        if (item.BomCodeFK == gbomcodeFK)
                                            item.Qty = qty;
                                        //thực hiện sửa giá
                                        item.PriceB1 = p1 + p2 + p3;//giá này đã giảm rồi
                                        item.SubTotalB1 = item.PriceB1 * item.Qty;//tổng này giảm rồi
                                        item.PriceB1_Old = (p1_Old + p2_Old + p3_Old);
                                        item.Product_Type = _ProductType;
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
                });
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
                                UpdateSUMBoq1(item, function (abc, p1, p2, p3) {
                                    gtblListOrderDetail[k].Material_Price = p1;
                                    gtblListOrderDetail[k].Labor_Price = p2;
                                    gtblListOrderDetail[k].Equipment_Price = p3;
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
        //material sysnthetic
        $('body').on('focusin', '#frmMaintainance-Material input', function () {
            var id = $(this).attr('id');
            if (id == 'txtProductNameMS' || id == 'txtNoteMS')
                gStr_old = $(this).val();
            else
                if (id == 'txtUnitPrice')
                    gPrice_old = general.toFloat($(this).val());
        });
        $('body').on('focusout', '#frmMaintainance-Material input', function () {
            var kt = false;
            var rowindex = $(this).parent('td').parent('tr').index() + 1;
            var id = $(this).attr('id');
            if (id == 'txtUnitPrice') {
                var price1_new = general.toFloat($(this).val());
                if (price1_new < 0)
                    price1_new = 0;
                $(this).val(price1_new);
                if (gPrice_old != price1_new) {
                    var unitid = general.toInt($(this).parent('#price').siblings("#unit").data('unitid'));
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
        $('#btn-apply-material').on('click', function () {
            if (ListIndexRowUpdateMS.length != 0) {
                var row = $('#material tr');
                var listUpdateSO = [];
                for (var i = 0; i < ListIndexRowUpdateMS.length; i++) {
                    var indexrow = ListIndexRowUpdateMS[i];
                    var priceB1 = general.toFloat(row.eq(indexrow).children('#price').children('#txtUnitPrice').val());
                    var priceB1_Old = 0;
                    var _rate = 100 - general.toFloat($('#txtK1').val());
                    //lấy giá mới theo hàng đã được chỉnh sửa !
                    // giá này là giá có % giảm rồi !
                    var _bomdetailfk = row.eq(indexrow).children('.col-productCode').text();
                    var _NameBomdetail = row.eq(indexrow).children('.productName').children('#txtProductNameMS').val();
                    var _note = row.eq(indexrow).children('#note').children('#txtNoteMS').val();
                    var _ProductType = general.toInt(row.eq(indexrow).children('#type').text());
                    // cập nhật lại giá gốc theo giá mới chỉnh sữa và % giảm
                    if (_rate != 0)  // % giảm nhỏ hơn 100%
                    {
                        if (priceB1 != 0) //nếu !0
                            priceB1_Old = Math.round(priceB1 / _rate * 100);
                    }
                    // cập nhật lại giá cho bảng tổng hợp vật tư của dự án
                    for (var j = 0; j < dataMaterialOfProject.length; j++) {
                        if (dataMaterialOfProject[j].BomDetailFK == _bomdetailfk) {
                            dataMaterialOfProject[j].NameBomdetail = _NameBomdetail;
                            dataMaterialOfProject[j].priceB1 = priceB1;
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
                                        item.PriceB1 = priceB1;
                                        item.SubTotalB1 = priceB1 * item.Qty;
                                        item.PriceB1_Old = priceB1_Old;
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
                                UpdateSUMBoq1(item, function (abc, p1, p2, p3) {
                                    temp.Material_Price = p1;
                                    temp.Labor_Price = p2;
                                    temp.Equipment_Price = p3;
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
            }
        });
        //Report
        $('body').on('click', '#btnPrintReport', function () {
            console.log("Hello world");
            var getCheck = document.querySelector('input[name="rdPtvt"]:checked').value;
            var that = $('#txtId').val();

            if (getCheck == 'Bảng phân tích vật tư') {
                $.ajax({
                    type: "POST",
                    url: "/SalesOrderBOQ1/ExportMaterialAnalyze",
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
                    url: "/SalesOrderBOQ1/ExportMaterialDetail",
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
                    url: "SalesOrderBOQ1/ExportExcel",
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
                    url: "SalesOrderBOQ1/ExportMaterialUnitPriceDetail",
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
    }

    $('body').on('click', '#material-dropdown', function (e) {
        dropdown = $('#material-dropdown');
        $('#material-dropdown').select2();
        dropdown.empty();
        // Populate dropdown with list of provinces
        for (var i = 0; i < dataMaterialOfProject.length; i++) {
            dropdown.append($('<option></option>').attr('value', dataMaterialOfProject[i].BomDetailFK).text(dataMaterialOfProject[i].BomDetailFK + ' ' + dataMaterialOfProject[i].NameBomdetail));
        }
    });

    function loadDataRow(data, IsRowNew, temp, indexrow, callback) {
        var _DiscountRate = $('#txtK1').val() == "" ? 0 : general.toFloat($('#txtK1').val());
        var _IdRow = gIdRow;
        var Keyid_orderdetail = 0;
        var tblOrderDetail = {
            stt: '',
            SO_FK: '',
            IdRow: 0,
            Parent_Id: '',
            LevelHeader: '',
            NameHeader: '',
            BOMCode_FK: data.ProductCode,
            Product_Name: data.ProductName,
            Description: '',
            UnitFK: data.ProductUnit,
            UnitName: data.ProductUnitNavigation.UnitName,
            QtyOrdered: 0,
            TotalQtyOrdered: 0,
            Material_Price: 0,
            Labor_Price: 0,
            Equipment_Price: 0,
            TotalPrice: 0,
            Subtotal: 0
        };
        if (IsRowNew)//tạo hàng mới thì tăng mã
        {
            gIdRow++;
            temp.siblings('#orderDetailOld').text('0');
        }
        else//là row cũ chỉnh sửa
        {
            _IdRow = temp.siblings('#idrow').text();
            Keyid_orderdetail = temp.siblings('#orderDetailOld').text();
            // khoi tao lai qty =0, update subtotal
            temp.siblings('#productqty').children('#txtProductQty').val(0);
            var _indexrow = temp.parent('tr').index() + 1;
            updateQty(indexrow, null);

            for (var i = 0; i < gtblListOrderDetail.length; i++) {
                if (gtblListOrderDetail[i].IdRow == _IdRow)
                    gtblListOrderDetail.splice(i, 1);
            }
        }
        tblOrderDetail.IdRow = _IdRow;
        // có 4 trường hợp : 1.là header và  chú thích ( không phải load con của nó lên
        //2. là Tạm tính (TTA,TTB,TTC) 4. là thành phẩm bình thường
        // 1. header
        var productCode = data.ProductCode;
        if (productCode == general.bomId.Header) {
            var _idParent, _levelHeader;// update trên và dưới ( cho chính xác lại (phòng cho trường hợp Chèn giữa)
            // xây dựng lại hàm này cập nhật thông tin cho những bảng dưới
            UpdateUpAndDownOfHeader(indexrow, _IdRow, function (total, _idParent, _levelHeader) {
                temp.siblings('#btn').html("<button class='btn btn-xs btn-default btn-plus' data-btn=1 type='button'><i class='fa fa-minus'></i></button>");
                temp.siblings('#levelHeader').children('#txtLevelHeader').val(_levelHeader);
                temp.siblings('#parentId').text(_idParent);

                tblOrderDetail.BoqOrderDetailBoms = null;
                temp.siblings('#levelName').children('#txtLevelName').val('');
                temp.siblings('.productName').children('.tooltip-name').children('#txtProductName').val(tblOrderDetail.Product_Name);
                temp.siblings('.productName').children('.tooltip-name').children('span').text(tblOrderDetail.Product_Name);
                temp.siblings('#description').children('#txtDescription').val(tblOrderDetail.Description);
                temp.siblings('#productunit').text(tblOrderDetail.UnitName);
                //cập nhật giá
                //temp.siblings('#materialPrice').text(general.commaSeparateNumber(Math.round(pr1)));
                //temp.siblings('#laborPrice').text(general.commaSeparateNumber(Math.round(pr2)));
                //temp.siblings('#equipmentPrice').text(general.commaSeparateNumber(Math.round(pr3)));
                //tblOrderDetail.MaterialPrice = pr1;
                //tblOrderDetail.LaborPrice = pr2;
                //tblOrderDetail.EquipmentPrice = pr3;
                //temp.siblings('#totalPrice').text(general.commaSeparateNumber(Math.round(pr1 + pr2 + pr3)));
                temp.siblings('#idrow').text(_IdRow);
                temp.siblings('#unitReal').text(tblOrderDetail.UnitFK);
                gtblListOrderDetail.push(tblOrderDetail);
            });
        }
        else {
            temp.siblings('#btn').text('');
            temp.siblings('#subTotal').text('0');
            temp.siblings('#subTotal').data('value', '0');
            temp.siblings('#levelHeader').children('#txtLevelHeader').val('');
            temp.siblings('#parentId').text('');
        }
        //unit: có 2 TH
        //Unitid là chú thích thì lấy unitname của cha
        //còn lại lấy unit của nó luôn
        var unitName = '';
        var codeBom = data.ProductCode;
        var pr1 = 0;
        var pr2 = 0;
        var pr3 = 0;
        if (productCode == general.bomId.Note) {
            //lấy unit theo cha nó
            getUnitNameByParent(indexrow, function (unitname) {
                unitName = unitname;
                tblOrderDetail.BoqOrderDetailBoms = null;
                temp.siblings('#levelName').children('#txtLevelName').val('');
                temp.siblings('.productName').children('.tooltip-name').children('#txtProductName').val(tblOrderDetail.Product_Name);
                temp.siblings('.productName').children('.tooltip-name').children('span').text(tblOrderDetail.Product_Name);
                temp.siblings('#description').children('#txtDescription').val(tblOrderDetail.Description);
                temp.siblings('#productunit').text(unitName);
                temp.siblings('#unitReal').text(tblOrderDetail.UnitFK);
                temp.siblings('#idrow').text(_IdRow);
                gtblListOrderDetail.push(tblOrderDetail);
            });
        }
        if (productCode == general.bomId.TTA || productCode == general.bomId.TTB || productCode == general.bomId.TTC)//là tạm tính
        {
            // giá bằng 0
            create_ProjectBOMDetail(Keyid_orderdetail, _IdRow, codeBom, _DiscountRate, tblOrderDetail.UnitFK, tblOrderDetail.UnitName, function (bomdetailFk, listPBD) {
                tblOrderDetail.BoqOrderDetailBoms = listPBD;
                codeBom = bomdetailFk;
                temp.children('.autocomplete').children('.code').val(codeBom);
                temp.siblings('#levelName').children('#txtLevelName').val('');
                temp.siblings('.productName').children('.tooltip-name').children('#txtProductName').val(tblOrderDetail.Product_Name);
                temp.siblings('.productName').children('.tooltip-name').children('span').text(tblOrderDetail.Product_Name);
                temp.siblings('#description').children('#txtDescription').val(tblOrderDetail.Description);
                temp.siblings('#productunit').text(tblOrderDetail.UnitName);
                temp.siblings('#unitReal').text(tblOrderDetail.UnitFK);
                temp.siblings('#idrow').text(_IdRow);
                gtblListOrderDetail.push(tblOrderDetail);
            });
        }
        else if (productCode != general.bomId.Header && productCode != general.bomId.Note) {
            FindByOrderDetailFK(data.KeyId, gtblListOrderDetail, Keyid_orderdetail, productCode, _DiscountRate, function (abc, p1, p2, p3) {
                tblOrderDetail.BoqOrderDetailBoms = abc;
                pr1 = p1;
                pr2 = p2;
                pr3 = p3;
                temp.siblings('#levelName').children('#txtLevelName').val('');
                temp.siblings('.productName').children('.tooltip-name').children('#txtProductName').val(tblOrderDetail.Product_Name);
                temp.siblings('.productName').children('.tooltip-name').children('span').text(tblOrderDetail.Product_Name);
                temp.siblings('#description').children('#txtDescription').val(tblOrderDetail.Description);
                temp.siblings('#productunit').text(tblOrderDetail.UnitName);
                //cập nhật giá
                temp.siblings('#materialPrice').text(general.commaSeparateNumber(Math.round(pr1)));
                temp.siblings('#laborPrice').text(general.commaSeparateNumber(Math.round(pr2)));
                temp.siblings('#equipmentPrice').text(general.commaSeparateNumber(Math.round(pr3)));
                tblOrderDetail.Material_Price = pr1;
                tblOrderDetail.Labor_Price = pr2;
                tblOrderDetail.Equipment_Price = pr3;
                var sum = pr1 + pr2 + pr3;
                temp.siblings('#totalPrice').data('value', sum)
                temp.siblings('#totalPrice').text(general.commaSeparateNumber(Math.round(sum)));
                temp.siblings('#idrow').text(_IdRow);
                temp.siblings('#unitReal').text(tblOrderDetail.UnitFK);
                gtblListOrderDetail.push(tblOrderDetail);
            });
            var _rate = 100 - _DiscountRate;
        }
        else {
            var listpbd = [];
            tblOrderDetail.BoqOrderDetailBoms = listpbd;
        }
        callback(tblOrderDetail);
    }
    var delay = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearTimeout(timer);
            timer = setTimeout(callback, ms);
        };
    })();
    function FindByOrderDetailFK(productFk, listorderDetails, orderdetailFK, bomfk, DiscountRate, callback) {
        var Sum_p1 = 0;
        var Sum_p2 = 0;
        var Sum_p3 = 0;
        var _Rate = 100 - DiscountRate;
        var bHavePercent = false;
        var listByOrderdetailfk = [];
        var orderdetail = null;
        listorderDetails.forEach(function (item) {
            if (item.BOMCode_FK == bomfk)
                orderdetail = item;
        });
        if (orderdetail == null)//nếu chưa tồn tại
        {
            var listBomdetail;
            loadBomByProductFk(productFk, function (res) {
                listBomdetail = res;
                listBomdetail.forEach(function (lBomDetail) {
                    var data = {
                        KeyId: 0,
                        OrderDetailFK: 0,
                        BOMofTotal_FK: '',
                        BomCodeFK: 0,
                        BomDetailFK: 0,
                        NameBomdetail: '',
                        UnitFK: '',
                        Qty: 0,
                        Product_Type: '',
                        PriceB1: 0,
                        SubTotalB1: 0,
                        Discount_Rate: 0,
                        PriceB1_Old: 0,
                        PriceB2: 0,
                        SubTotalPriceB2: 0,
                        Note: '',
                        UnitName: ''
                    };
                    data.OrderDetailFK = orderdetailFK;
                    data.BomCodeFK = bomfk;
                    data.BOMofTotal_FK = lBomDetail.BOMofTotalRef_FK;
                    data.BomDetailFK = lBomDetail.ProductFkNavigation.ProductCode;
                    data.UnitFK = lBomDetail.ProductFkNavigation.ProductUnit;
                    if (data.UnitFK == general.unit.Percent)
                        bHavePercent = true;
                    data.Qty = lBomDetail.ProductQty;
                    data.Discount_Rate = _Rate;
                    data.Product_Type = lBomDetail.ProductFkNavigation.ProductTypeFK;
                    data.NameBomdetail = lBomDetail.ProductFkNavigation.ProductName;
                    data.UnitName = lBomDetail.ProductFkNavigation.ProductUnitNavigation.UnitName;
                    //Tìm và điền giá
                    var i = 0;
                    for (; i < dataMaterialOfProject.length; i++) {
                        if (dataMaterialOfProject[i].BomDetailFK == lBomDetail.ProductFk)
                            break;
                    }
                    //lưu giá thực
                    var price = 0;
                    //lưu giá gốc
                    var priceOld = 0;
                    if (i < dataMaterialOfProject.length) {
                        //giá thực theo % giảm
                        price = dataMaterialOfProject[i].priceB1;
                        //tính giá gốc
                        if (_Rate != 0)//nếu =0 thì là giảm 100% tức giá =0
                        {
                            priceOld = Math.round(price * 100 / _Rate);
                        }
                    }
                    else//lấy giá ở product
                    {
                        //giá gốc
                        priceOld = lBomDetail.ProductFkNavigation.UnitPrice;
                        //tính giá thực theo % giảm
                        //giá này là giá khi * với % giảm rồi giá sử dụng thực tế nên phải làm tròn.
                        price = Math.round(priceOld * _Rate / 100);
                        //thêm 1 hàng vào ds ở tổng hợp vật tư cho dư án
                        var ttvl = {
                            BomDetailFK: lBomDetail.ProductFkNavigation.ProductCode,
                            NameBomdetail: lBomDetail.ProductFkNavigation.ProductName,
                            Product_Unit: lBomDetail.ProductFkNavigation.ProductUnit,
                            UnitName: lBomDetail.ProductFkNavigation.ProductUnitNavigation.UnitName,
                            Product_Type: lBomDetail.ProductFkNavigation.ProductTypeFK,
                            ProductTypeName: lBomDetail.ProductFkNavigation.ProductTypeNavigation.ProductTypeName,
                            priceB1: price,
                            Note: ""
                        };
                        dataMaterialOfProject.push(ttvl);
                    }
                    data.PriceB1 = price;
                    data.PriceB1_Old = priceOld;
                    data.SubTotalB1 = price * lBomDetail.ProductQty;
                    if (!bHavePercent) {
                        //thêm vào giá tổng của bom: giá này ko làm tròn (cộng tổng lại hết mới làm tròn)
                        var type = data.Product_Type;
                        if (type == general.productType.Material)
                            Sum_p1 += data.SubTotalB1;
                        else
                            if (type == general.productType.Labor)
                                Sum_p2 += data.SubTotalB1;
                            else
                                Sum_p3 += data.SubTotalB1;
                    }
                    listByOrderdetailfk.push(data);
                });
                if (bHavePercent) {
                    UpdateSUMBoq1(listByOrderdetailfk, function (abc, p1, p2, p3) {
                        callback(abc, p1, p2, p3);
                    });
                }
                else
                    callback(listByOrderdetailfk, Sum_p1, Sum_p2, Sum_p3);
            });
        }
        else//OrderDetail đã tồn tại rồi
        {
            var _detail = [];
            orderdetail.BoqOrderDetailBoms.forEach(function (temp) {
                _detail.push(temp);
            });
            callback(_detail, orderdetail.Material_Price, orderdetail.Labor_Price, orderdetail.Equipment_Price);
        }
    }
    function EditPriceAfterImportEx(listOrderDetail, callback) {
        var _Rate = 100 - general.toFloat($('#txtK1').val());
        var i = 0;
        for (; i < listOrderDetail.length; i++) {
            var bomfk = listOrderDetail[i].BOMCode_FK;
            var _listBomDetail = listOrderDetail[i].BoqOrderDetailBoms;
            var Sum_p1 = 0;
            var Sum_p2 = 0;
            var Sum_p3 = 0;
            var bHavePercent = false;
            var orderdetail = null;
            for (var k = 0; k < gtblListOrderDetail.length; k++)
                if (gtblListOrderDetail[k].BOMCode_FK == bomfk) {
                    orderdetail = gtblListOrderDetail[k];
                    break;
                }

            listOrderDetail[i].IdRow = gIdRow;
            gIdRow++;
            if (orderdetail == null)//nếu chưa tồn tại
            {
                var listBomdetail = _listBomDetail;
                listBomdetail.forEach(function (lBomDetail) {
                    //Tìm và điền giá
                    var i = 0;
                    for (; i < dataMaterialOfProject.length; i++) {
                        if (dataMaterialOfProject[i].BomDetailFK == lBomDetail.BomDetailFK)
                            break;
                    }
                    if (lBomDetail.UnitFK == general.unit.Percent)
                        bHavePercent = true;
                    //lưu giá thực
                    var price = 0;
                    //lưu giá gốc
                    var priceOld = 0;
                    if (i < dataMaterialOfProject.length) {
                        //giá thực theo % giảm
                        price = dataMaterialOfProject[i].priceB1;
                        //tính giá gốc
                        if (_Rate != 0)//nếu =0 thì là giảm 100% tức giá =0
                        {
                            priceOld = Math.round(price * 100 / _Rate);
                        }
                    }
                    else//lấy giá ở product
                    {
                        //giá gốc
                        priceOld = lBomDetail.PriceB1_Old;
                        //tính giá thực theo % giảm
                        //giá này là giá khi * với % giảm rồi giá sử dụng thực tế nên phải làm tròn.
                        price = Math.round(priceOld * _Rate / 100);
                        //thêm 1 hàng vào ds ở tổng hợp vật tư cho dư án
                        var ttvl = {
                            BomDetailFK: lBomDetail.BomDetailFK,
                            NameBomdetail: lBomDetail.NameBomdetail,
                            Product_Unit: lBomDetail.UnitFK,
                            UnitName: lBomDetail.UnitName,
                            Product_Type: lBomDetail.Product_Type,
                            ProductTypeName: lBomDetail.tblProductType.ProductTypeName,
                            priceB1: price,
                            Note: ""
                        };
                        dataMaterialOfProject.push(ttvl);
                    }
                    lBomDetail.PriceB1 = price;
                    lBomDetail.PriceB1_Old = priceOld;
                    lBomDetail.SubTotalB1 = price * lBomDetail.Qty;
                    if (!bHavePercent) {
                        //thêm vào giá tổng của bom: giá này ko làm tròn (cộng tổng lại hết mới làm tròn)
                        var type = lBomDetail.Product_Type;
                        if (type == general.productType.Material)
                            Sum_p1 += lBomDetail.SubTotalB1;
                        else
                            if (type == general.productType.Labor)
                                Sum_p2 += lBomDetail.SubTotalB1;
                            else
                                Sum_p3 += lBomDetail.SubTotalB1;
                    }
                });
                if (bHavePercent) {
                    UpdateSUMBoq1(listBomdetail, function (abc, p1, p2, p3) {
                        listOrderDetail[i].BoqOrderDetailBoms = abc;
                        listOrderDetail[i].Material_Price = p1;
                        listOrderDetail[i].Labor_Price = p2;
                        listOrderDetail[i].Equipment_Price = p3;
                        var totalprice = p1 + p2 + p3;
                        listOrderDetail[i].TotalPrice = totalprice;
                        listOrderDetail[i].Subtotal = totalprice * listOrderDetail[i].QtyOrdered;
                    });
                }
                else {
                    listOrderDetail[i].BoqOrderDetailBoms = listBomdetail;
                    listOrderDetail[i].Material_Price = Sum_p1;
                    listOrderDetail[i].Labor_Price = Sum_p2;
                    listOrderDetail[i].Equipment_Price = Sum_p3;
                    var totalprice = Sum_p1 + Sum_p2 + Sum_p3;
                    listOrderDetail[i].TotalPrice = totalprice;
                    listOrderDetail[i].Subtotal = totalprice * listOrderDetail[i].QtyOrdered;
                }
            }
            else//OrderDetail đã tồn tại rồi
            {
                var _detail = [];
                orderdetail.BoqOrderDetailBoms.forEach(function (temp) {
                    _detail.push(temp);
                });

                listOrderDetail[i].BoqOrderDetailBoms = _detail;
                listOrderDetail[i].Material_Price = orderdetail.Material_Price;
                listOrderDetail[i].Labor_Price = orderdetail.Labor_Price;
                listOrderDetail[i].Equipment_Price = orderdetail.Equipment_Price;
                var totalprice = orderdetail.Material_Price + orderdetail.Labor_Price + orderdetail.Equipment_Price;
                listOrderDetail[i].TotalPrice = totalprice;
                //listOrderDetail[i].QtyOrdered = unitround.RoundByUnit(listOrderDetail[i].QtyOrdered, listOrderDetail[i].UnitName);
                listOrderDetail[i].Subtotal = totalprice * listOrderDetail[i].QtyOrdered;
            }
            if (listOrderDetail[i].UnitFK == general.unit.Header)
                listOrderDetail[i].LevelHeader = 1;
            gtblListOrderDetail.push(listOrderDetail[i]);
        }
        if (i == listOrderDetail.length)
            callback(listOrderDetail);
    }
    function UpdateSUMBoq1(abc, callback) {
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
                            sum += abc[j].SubTotalB1;
                }
                else//là tổng hợp của các thành phẩm
                {
                    for (var j = 0; j < abc.length; j++)
                        if (abc[j].UnitFK != general.unit.Percent && abc[j].Product_Type == abc[i].Product_Type && bomtotalfk == abc[j].BOMofTotal_FK)
                            sum += abc[j].SubTotalB1;
                }
                //có tổng rồi thì tính
                var percent = abc[i].Qty;
                abc[i].SubTotalB1 = percent * sum / 100;
            }
            if (abc[i].Product_Type == general.productType.Material)
                Sp1 += abc[i].SubTotalB1;
            else if (abc[i].Product_Type == general.productType.Labor)
                Sp2 += abc[i].SubTotalB1;
            else
                Sp3 += abc[i].SubTotalB1;
        }
        callback(abc, Sp1, Sp2, Sp3);
    }
    function UpdateUpAndDownOfHeader(indexrow, _id_in, callback) {
        //gọi là header A
        var _Total_A;
        var _ParentId_out;
        var _LevelHeader_out;
        UpdateHeaderToBom(indexrow, _id_in, function (total) {
            _Total_A = total;
            var i = indexrow - 1;
            while (i >= 0) {
                var _unit = general.toInt($('#table-orderDetail tr').eq(i).children('#unitReal').text());
                if (_unit != general.unit.Header)
                    i--;
                else
                    break;
            }

            //là header hoặc hết table
            if (i >= 0) {
                //là header B
                var _id_B = $('#table-orderDetail tr').eq(i).children('#idrow').text();
                var _levelHeader_B = general.toInt($('#table-orderDetail tr').eq(i).children('#levelHeader').children('#txtLevelHeader').val());
                var _TotalB_Old = general.toFloat($('#table-orderDetail tr').eq(i).children('#subTotal').data('value'));
                //gán ref cho header A
                _ParentId_out = _id_B;
                _LevelHeader_out = _levelHeader_B + 1;
                //Tính tổng lại cho header B
                var _totalB = _Total_A;
                var index_B = i;
                i++;
                if (i == indexrow) i++;
                var rowcount = $('#table-orderDetail tr').length;
                var row = $('#table-orderDetail tr');
                while (i < rowcount) {
                    var _id_Parent = row.eq(i).children('#parentId').text();
                    if (_id_Parent == _id_B) {
                        //cộng vào tổng
                        _totalB += general.toFloat(row.eq(i).children('#subTotal').data('value'));
                    }
                    else {
                        //không phải là con
                        var _unit = general.toInt(row.eq(i).children('#unitReal').text());
                        if (_unit == general.unit.Header)//là header
                        {
                            var _levelHeader = general.toInt(row.eq(i).children('#levelHeader').children('#txtLevelHeader').val());
                            if (_levelHeader <= _levelHeader_B)//hết con của B rồi
                                break;
                        }
                    }
                    i++;
                    if (i == indexrow) i++;
                }

                //Tính độ lệch và update lên level cao hơn
                UpdateHeaderToHeaderParent(index_B, _totalB - _TotalB_Old);
            }
            else {
                _ParentId_out = _id_in;
                _LevelHeader_out = 1;
            }
            callback(_Total_A, _ParentId_out, _LevelHeader_out);
        });
    }
    function UpdateHeaderToBom(indexrow, _id, callback) {
        var i = indexrow + 1;
        var total = 0;
        var rowcount = $('#table-orderDetail tr').length;
        var row = $('#table-orderDetail tr');
        while (i < rowcount) {
            var _unit = general.toInt(row.eq(i).children('#unitReal').text());
            if (_unit != general.unit.Header) {
                if (_unit != general.unit.Note) {
                    total += general.toFloat(row.eq(i).children('#subTotal').data('value'));
                    row.eq(i).children('#parentId').text(_id);
                }
            }
            else
                break;
            i++
        }

        callback(total);
    }
    function UpdateHeaderToHeaderParent(indexrowParent, _effect) {
        var i = indexrowParent;
        var _idParent = $('#table-orderDetail tr').eq(i).children('#idrow').text();//id con
        var row = $('#table-orderDetail tr');
        var _id;
        while (i >= 0) {
            var total_Old = general.toFloat(row.eq(i).children('#subTotal').data('value'));
            var _sum = total_Old + _effect;
            row.eq(i).children('#subTotal').text(Math.round(_sum));
            row.eq(i).children('#subTotal').data('value', _sum);
            _id = _idParent;
            _idParent = row.eq(i).children('#parentId').text();
            if (_id != _idParent) {
                //tìm index của cha nó (có id là idparen)
                while (i > 0)
                    if (row.eq(i).children('#idrow').text() != _idParent)
                        i--;
                    else
                        break;
            }
            else
                break;
        }
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
                    ret += general.toFloat(row.eq(i).children('#subTotal').data('value'));
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
                        ret += general.toFloat(row.eq(i).children('#subTotal').data('value'));
                    else if (tt == general.unit.Header) {
                        //tính tổng các header con của nó nếu có
                        while (i < rowcount) {
                            tt = general.toInt(row.eq(i).children('#unitReal').text());
                            if (tt == general.unit.Header) {
                                var lvHeader_Child = general.toInt(row.eq(i).children('#levelHeader').children('#txtLevelHeader').val());
                                if (lvHeader_Child == lvHeader + 1)//là con
                                    ret += general.toFloat(row.eq(i).children('#subTotal').data('value'));
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
            var totalParent_old = general.toFloat(row.eq(parent).children('#subTotal').data('value'));
            UpdateHeaderToHeaderParent(parent, ret - totalParent_old);
        }
    }
    function UpdateHeaderToHeaderParent(indexrowParent, _effect) {
        var row = $('#table-orderDetail tr');
        var i = indexrowParent;
        var _idParent = general.toInt(row.eq(i).children('#idrow').text());
        var _id;
        while (i >= 0) {
            var total_old = general.toFloat(row.eq(i).children('#subTotal').data('value'));
            var _sum = total_old + _effect;
            row.eq(i).children('#subTotal').data('value', _sum);
            row.eq(i).children('#subTotal').text(general.commaSeparateNumber(Math.round(_sum)));
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
    function getUnitNameByParent(indexrow, callback) {
        var i = indexrow - 1;
        while (i > 0) {
            var temp = general.toInt($('#table-orderDetail tr').eq(i).children('#unitReal').text());
            if (temp != general.unit.Note) {
                var unitName = $('#table-orderDetail tr').eq(i).children('#productunit').text();
                callback(unitName);
                break;
            }
            i--;
        }
    }
    //Tạo list thông tin add vào tạm tính
    function create_ProjectBOMDetail(orderDetailFk, idRow, bomfk, DiscountRate, unitId, unitName, callback) {
        var listPBD = [];
        var _bomdetaiFK = "";
        var _after = idRow;//không vượt quá số này
        _bomdetaiFK = bomfk + _after.toString();
        if (bomfk == general.bomId.TTA) {
            //tạo 1 obj
            Add_ProjectBomDetail(orderDetailFk, bomfk, _bomdetaiFK, unitId, unitName, DiscountRate, general.productType.Material,
                "Nhập Tên của vật liệu hoặc Nhân công hoặc máy thi công, đơn vị mặc định là gói", function (_rowDetail, data) {
                    listPBD.push(_rowDetail);
                    dataMaterialOfProject.push(data);
                });
        }
        else if (bomfk == general.bomId.TTB) {
            // tạo 2 obj
            // đảm bảo không bị trùng mã
            //  tạo  obj 1
            Add_ProjectBomDetail(orderDetailFk, bomfk, _bomdetaiFK + 'A', unitId, unitName, DiscountRate
                , general.productType.Material  // mắc định là vật liệu
                , "Nhập Tên của vật liệu hoặc Nhân công, đơn vị mặc định là gói", function (_rowDetail, data) {
                    listPBD.push(_rowDetail);
                    dataMaterialOfProject.push(data);
                });
            //  tạo  obj 2
            Add_ProjectBomDetail(orderDetailFk, bomfk, _bomdetaiFK + 'B', unitId, unitName, DiscountRate
                , general.productType.Material  // mắc định là vật liệu
                , "Nhập Tên của Nhân công hoặc máy thi công, đơn vị mặc định là gói", function (_rowDetail, data) {
                    listPBD.push(_rowDetail);
                    dataMaterialOfProject.push(data);
                });
        }
        else {
            //  tạo  obj 1
            Add_ProjectBomDetail(orderDetailFk, bomfk, _bomdetaiFK + 'A', unitId, unitName, DiscountRate
                , general.productType.Material  // mắc định là vật liệu
                , "Nhập Tên của vật liệu hoặc Nhân công, đơn vị mặc định là gói", function (_rowDetail, data) {
                    listPBD.push(_rowDetail);
                    dataMaterialOfProject.push(data);
                });
            //  tạo  obj 2
            Add_ProjectBomDetail(orderDetailFk, bomfk, _bomdetaiFK + 'B', unitId, unitName, DiscountRate
                , general.productType.Material  // mắc định là vật liệu
                , "Nhập Tên của Nhân công hoặc máy thi công, đơn vị mặc định là gói", function (_rowDetail, data) {
                    listPBD.push(_rowDetail);
                    dataMaterialOfProject.push(data);
                });
            Add_ProjectBomDetail(orderDetailFk, bomfk, _bomdetaiFK + 'C', unitId, unitName, DiscountRate
                , general.productType.Material  // mắc định là vật liệu
                , "Nhập Tên của máy thi công và giá máy, đơn vị mặc định là gói", function (_rowDetail, data) {
                    listPBD.push(_rowDetail);
                    dataMaterialOfProject.push(data);
                });
        }
        callback(_bomdetaiFK, listPBD);
    }
    function Add_ProjectBomDetail(orderdetailfk, bomfk, bomdetailfk, unitId, unitName, DiscountRate, productType, NamePBD, callback) {
        var _rowDetail = {
            Product_Type: productType,
            BomCodeFK: bomfk,
            OrderDetailFK: orderdetailfk,
            //kết hợp giữa order detail và bomFK để tạo mã của bom detail(sao cho không bị trùng)
            BomDetailFK: bomdetailfk, // đặt dại mã này
            NameBomdetail: NamePBD, // nếu đổi tên của tạm tin
            UnitFK: unitId,//gói, tạm tính thì unit con bằng unit cha
            Qty: 1, // mặc đinh là 1
            Discount_Rate: 100 - DiscountRate,
            PriceB1: 0,
            PriceB1_Old: 0,
            PriceB2: 0,
            SubTotalB2: 0,
            UnitName: unitName,
            tblUnit: { UnitName: unitName }
        };
        var data = {
            BomDetailFK: bomdetailfk, // tạm tính thì mã bom detail = mã bom
            NameBomdetail: NamePBD,
            Product_Unit: unitId,
            UnitName: unitName,
            Product_Type: productType,
            BOMofTotal_FK: '',
            priceB1: 0,
            priceB2: 0,
            SubTotalB2: 0,
            Note: ''
        };
        callback(_rowDetail, data);
    }
    function updateQty(indexrow, _qty) {
        var row = $('#table-orderDetail tr');
        var tt = general.toInt(row.eq(indexrow).children('#unitReal').text());
        if (tt != general.unit.Header) {
            var value = general.toFloat(row.eq(indexrow).children('#productqty').children('#txtProductQty').val());
            if (value == 0) {
                row.eq(indexrow).children('#productqty').children('#txtProductQty').css('color', 'Red');
            }
            else {
                row.eq(indexrow).children('#productqty').children('#txtProductQty').css('color', 'Black');
            }
            //update theo unitId
            if (tt == general.unit.Note) {
                SumQtyNoteOfBom(indexrow);
            }
            else //không phải note củng không phải header thì là bom --> công tổng lên header
            {
                UpdateQtyByRow(indexrow, _qty, false);
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
                        ret = general.addFloat(ret, general.toFloat(row.eq(i).children('#productqty').children('#txtProductQty').val()), 2);
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
                            ret = general.addFloat(ret, general.toFloat(row.eq(i).children('#productqty').children('#txtProductQty').val()), 2);
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
            UpdateQtyByRow(parent, ret, true);
            // update thay đổi  lên header của cha nó
            SumBomOfHeader(parent);
        }
    }
    //thay đổi khối lượng cập nhật giá
    function UpdateQtyByRow(indexrow, _qty_order, isNote) {
        var row = $('#table-orderDetail tr');
        var _qtyOrder;
        if (_qty_order != null)
            _qtyOrder = _qty_order;
        else
            _qtyOrder = general.toFloat(row.eq(indexrow).children('#productqty').children('#txtProductQty').val());
        var _price = general.toFloat(row.eq(indexrow).children('#totalPrice').data('value'));
        var _subtotal = _qtyOrder * _price;
        if (isNote)
            row.eq(indexrow).children('#productqty').children('#txtProductQty').prop('readonly', true);
        else
            row.eq(indexrow).children('#productqty').children('#txtProductQty').prop('readonly', false);
        row.eq(indexrow).children('#productqty').children('#txtProductQty').val(_qtyOrder);
        row.eq(indexrow).children('#subTotal').data('value', _subtotal);
        row.eq(indexrow).children('#subTotal').text(general.commaSeparateNumber(Math.round(_subtotal)));//thành tiền đã bao gồm % giảm giá
    }
    //cập nhật giá lên table order detail
    function UpdatePriceOnDgv(listOrderDetail, roundPriceDefault, callback) {
        //truyền lên tabl
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
                        var _Qty = general.toFloat(row.eq(i).children('#productqty').children('#txtProductQty').val());
                        // cập nhật lại giá và làm tròn giá
                        // xem xet lại việc làm tròn giá nếu ....
                        var p1 = temp.Material_Price;
                        var p2 = temp.Labor_Price;
                        var p3 = temp.Equipment_Price;
                        row.eq(i).children('#materialPrice').text(general.commaSeparateNumber(Math.round(p1)));
                        row.eq(i).children('#laborPrice').text(general.commaSeparateNumber(Math.round(p2)));
                        row.eq(i).children('#equipmentPrice').text(general.commaSeparateNumber(Math.round(p3)));
                        var _sum = p1 + p2 + p3;
                        row.eq(i).children('#totalPrice').data('value', _sum);
                        row.eq(i).children('#totalPrice').text(general.commaSeparateNumber(Math.round(_sum)));
                        var _sub = _sum * _Qty;
                        row.eq(i).children('#subTotal').data('value', _sub);
                        row.eq(i).children('#subTotal').text(general.commaSeparateNumber(Math.round(_sub)));
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
                                    var _total = general.toFloat(row.eq(j).children('#subTotal').text());
                                    listTotal[last_Stack] += _total;
                                }
                            }
                            else //j không còn là con của i nữa (chắc chắn j phải là một header)
                            {
                                //update giá trị cuả i vào bảng
                                row.eq(i).children('#subTotal').data('value', listTotal[last_Stack]);
                                row.eq(i).children('#subTotal').text(general.commaSeparateNumber(Math.round(listTotal[last_Stack])));
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
                    row.eq(i).children('#subTotal').data('value', listTotal[last_Stack]);
                    row.eq(i).children('#subTotal').text(general.commaSeparateNumber(Math.round(listTotal[last_Stack])));
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
    function loadProduct(key, temp, callback) {
        general.notify('dang search', 'success');
        var template = $('#productCode-template').html();
        var render = "";
        var a = temp.siblings('.contain-productCode');
        $.ajax({
            type: 'GET',
            url: '/product/GetAllBom',
            data: { keyword: key },
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
                });
                if (render != '') {
                    a.html(render);
                }
                callback(render);
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    function loadBomByProductFk(pro, callback) {
        $.ajax({
            type: 'GET',
            url: '/bom/GetByProductFk',
            data: { productFk: pro },
            success: function (res) {
                console.log(res);
                callback(res);
            },
            error: function (error) {
            }
        });
    }
    function Zoom(indexrow, IsZoomIn) {
        //kt nếu là header thì làm
        var row = $('#table-orderDetail tr');
        var rowcount = row.length;
        var _unit = general.toInt(row.eq(indexrow).children('#unitReal').text());
        if (_unit == general.unit.Header) {
            var max = rowcount - 1;
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
    function Delete_Row(rowindex, row) {
        //var row = $('#table-orderDetail tr');
        var idRow = general.toInt(row.eq(rowindex).children('#idrow').text());
        if (idRow != 0) {
            var idSave = general.toInt(row.eq(rowindex).children('#orderDetailOld').text());
            if (idSave != 0)//Đã lưu xuống csdk
                ListCodeOrderDetailDel.push(idSave);
            for (var i = 0; i < gtblListOrderDetail.length; i++) {
                if (gtblListOrderDetail[i].IdRow == idRow) {
                    gtblListOrderDetail.splice(i, 1);
                    break;
                }
            }
        }
        row.eq(rowindex).remove();
    }
    // cập nhật lại toàn bộ ref theo level header
    function UpdateParentIdByLevelHeader(callback) {
        //đi từ trên xuống dưới
        var i = 0;
        //bắt buộc thằng đầu tiên luôn là header và luôn là lv1
        var row = $('#table-orderDetail tr');
        var _Unit = general.toInt(row.eq(1).children('#unitReal').text());
        if (_Unit != general.unit.Header) {
            alert("Hàng đầu tiên của bảng luôn phải là header. Vui lòng kiểm tra lại ");
            $('#levelHeader-th').hide();
            $('#table-quantity-content tr').each(function () {
                $(this).children('#levelHeader').hide();
                var lv = general.toInt($(this).children('#levelHeader').children('#txtLevelHeader').val());
                if (lv == 0)
                    $(this).show();
                else {
                    $(this).children('#btn').children('.btn-plus').children('i').addClass('fa-minus').removeClass('fa-plus');
                    $(this).children('#btn').children('.btn-plus').data('btn', 1);
                }
            });
            general.stopLoad();
            return;
        }
        else {
            while (i < row.length) {
                //lấy lv của header hiện tại
                var Leveli = general.toInt(row.eq(i).children('#levelHeader').children('#txtLevelHeader').val());
                var _id = general.toInt(row.eq(i).children('#idrow').text());
                var _indexrowNext = -1;// lưu index của header tiếp theo
                // nếu lv=1 thì chỉnh parentid của nó thành id của nó
                if (Leveli == 1)
                    row.eq(i).children('#parentId').text(_id);
                // tìm những header có lv= Lv+1 để gán ref
                var j = i + 1;
                while (j < row.length) {
                    //tìm index header tiếp theo
                    while (j < row.length) {
                        var _unit_j = general.toInt(row.eq(j).children('#unitReal').text());
                        if (_unit_j != general.unit.Header) {
                            if (_indexrowNext == -1 && _unit_j != general.unit.Note)//khi chưa gặp header kế tiếp
                            {
                                //cập nhật cha cho bom
                                row.eq(j).children('#parentId').text(_id);
                            }
                            // chú thích thì luôn đươc cập nhật và xóa theo cha nó. nên không set đến
                            j++;
                        }
                        else
                            break;
                    }
                    if (j < row.length)//j là index của 1 header
                    {
                        //lưu lại index của header kế tiếp
                        if (_indexrowNext == -1)
                            _indexrowNext = j;
                        //lấy level của header để kiểm tra
                        var LevelJ = general.toInt(row.eq(j).children('#levelHeader').children('#txtLevelHeader').val());
                        if (LevelJ <= Leveli)
                            break;
                        if (LevelJ == Leveli + 1)
                            row.eq(j).children('#parentId').text(_id);
                        j++;
                    }
                }
                if (_indexrowNext == -1)
                    break;
                i = _indexrowNext;
            }
        }
        callback();
    }
    function UpdateBy_K1(_discount, listOrderDetail, _MaterialOfProject) {
        var _discount_RateNew = 100 - _discount;
        // cập nhật lại toàn bộ giá cho chi tiết ! rồi load lại vào datagridview
        for (var i = 0; i < listOrderDetail.length; i++) {
            if (listOrderDetail[i].BoqOrderDetailBoms != null)
                if (listOrderDetail[i].BoqOrderDetailBoms.length != 0) {
                    var HaveUnitPercent = false;
                    var Sump1 = 0;
                    var Sump2 = 0;
                    var Sump3 = 0;
                    var temp = listOrderDetail[i].BoqOrderDetailBoms;
                    temp.forEach(function (item) {
                        if (item.UnitFK == general.unit.Percent) {
                            HaveUnitPercent = true;
                        }
                        //lấy giá gốc
                        item.Discount_Rate = _discount_RateNew;
                        //tính giá giảm
                        var priceB1_Old = item.PriceB1_Old;
                        var priceB1 = Math.round(priceB1_Old * _discount_RateNew / 100);
                        for (var j = 0; j < _MaterialOfProject.length; j++)
                            if (_MaterialOfProject[j].BomDetailFK == item.BomDetailFK) {
                                _MaterialOfProject[j].priceB1 = priceB1;
                                break;
                            }
                        //cập nhật lại giá giảm
                        var qty = item.Qty;
                        item.PriceB1 = priceB1;
                        item.PriceB1_Old = priceB1_Old;
                        item.SubTotalB1 = priceB1 * qty;

                        if (!HaveUnitPercent) {
                            //thêm vào giá tổng của bom: giá này ko làm tròn (cộng tổng lại hết mới làm tròn)
                            var type = item.Product_Type;
                            if (type == general.productType.Material)
                                Sump1 += item.SubTotalB1;
                            else if (type == general.productType.Labor)
                                Sump2 += item.SubTotalB1;
                            else
                                Sump3 += item.SubTotalB1;
                        }
                    });
                    if (HaveUnitPercent)
                        UpdateSUMBoq1(temp, function (abc, p1, p2, p3) {
                            listOrderDetail[i].Material_Price = p1;
                            listOrderDetail[i].Labor_Price = p2;
                            listOrderDetail[i].Equipment_Price = p3;
                        });
                    else {
                        listOrderDetail[i].Material_Price = Sump1;
                        listOrderDetail[i].Labor_Price = Sump2;
                        listOrderDetail[i].Equipment_Price = Sump3;
                    }
                }
        }
    }
    function UpdateSumTXT() {
        var _subtotal = general.toFloat($('#txtSubTotal').text());
        var _GeneralExpensesRate = general.toFloat($('#txtGeneralExpensesRate').val());
        if (_GeneralExpensesRate >= 100 || _GeneralExpensesRate < 0)
            _GeneralExpensesRate = 0;
        _GeneralExpensesRate = general.toRound(_GeneralExpensesRate, 2);
        $('#txtGeneralExpensesRate').val(_GeneralExpensesRate);
        var _GeneralExpensesAmount = Math.round(_subtotal * _GeneralExpensesRate / 100);
        $('#txtGeneralExpensesAmount').text(general.commaSeparateNumber(_GeneralExpensesAmount));

        var _ProfitRate = general.toFloat($('#txtprofitRate').val());
        _ProfitRate = general.toRound(_ProfitRate, 2);
        if (_ProfitRate >= 100 || _ProfitRate < 0)
            _ProfitRate = 0;
        $('#txtprofitRate').val(_ProfitRate);
        var _ProfitAmount = Math.round((_subtotal + _GeneralExpensesAmount) * _ProfitRate / 100);
        $('#txtProfitAmount').text(general.commaSeparateNumber(_ProfitAmount));

        var _vatRate = general.toFloat($('#txtVATRate').val());
        _vatRate = general.toRound(_vatRate, 2);
        if (_vatRate >= 100 || _ProfitRate < 0)
            _vatRate = 0;
        $('#txtVATRate').val(_vatRate);
        var _tax = Math.round((_subtotal + _GeneralExpensesAmount + _ProfitAmount) * _vatRate / 100);

        $('#txtTax_Amount').text(general.commaSeparateNumber(_tax));
        $('#txtTotal_Order').text(general.commaSeparateNumber(_subtotal + _GeneralExpensesAmount + _ProfitAmount + _tax));
    }

    //load project by user
    function LoadInitProject(callback) {
        $.ajax({
            type: "GET",
            url: "/Project/GetListProjectAndCustomer",
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                callback(response);
                general.stopLoading();
            },
            error: function () {
                general.notify('Có lỗi trong khi load thông tin công trình !', 'error');
                general.stopLoading();
            }
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
    //get value order detail
    function getOrderDetail(listOrderDetail, callback) {
        var count = 0;
        $('#table-quantity-content tr').each(function (istt) {
            var idrow = general.toInt($(this).children('#idrow').text());
            for (var i = 0; i < gtblListOrderDetail.length; i++) {
                var temp = gtblListOrderDetail[i];
                if (temp.IdRow == idrow) {
                    var qty = general.toFloat($(this).children('#productqty').children('#txtProductQty').val());
                    var totalPrice = temp.Material_Price + temp.Labor_Price + temp.Equipment_Price;
                    var subtotal = qty * totalPrice;
                    var levelHeader = $(this).children('#levelHeader').children('#txtLevelHeader').val();
                    var levelName = $(this).children('#levelName').children('#txtLevelName').val();
                    var parentId = general.toInt($(this).children('#parentId').text());
                    var productName = $(this).children('.productName').children('.tooltip-name').children('#txtProductName').val();
                    var note = $(this).children('.productName').children('#txtNote').val();
                    var orderDetailId = general.toInt($(this).children('#orderDetailOld').text());
                    var description = $(this).children('#description').children('#txtDescription').val();
                    var totalHeader = general.toFloat($(this).children('#subTotal').data('value'));
                    var unitName = $(this).children('#productunit').text();
                    temp.QtyOrdered = qty;
                    temp.TotalPrice = totalPrice;
                    if (temp.UnitFK == general.unit.Header)
                        temp.Subtotal = totalHeader;
                    else
                        temp.Subtotal = subtotal;
                    temp.stt = istt + 1;
                    temp.Product_Name = productName;
                    temp.UnitName = unitName;
                    temp.Description = description;
                    temp.Parent_Id = parentId;
                    temp.LevelHeader = levelHeader;
                    temp.NameHeader = levelName;
                    temp.KeyId = orderDetailId;
                    temp.Note = note;
                    temp.SO_FK = general.toInt($('#txtId').val());
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
    //get parameter
    var getUrlParameter = function getUrlParameter(sParam) {
        var sPageURL = decodeURIComponent(window.location.search.substring(1)),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : sParameterName[1];
            }
        }
    };
    function getValue(id, Isview, callback) {
        if (id != 0) {
            $.ajax({
                type: 'GET',
                url: '/SalesOrderBOQ1/GetById',
                data: { id: id },
                async: false,
                success: function (response) {
                    console.log(response);
                    if (response != null) {
                        var data = response;
                        callback();
                        if (data.KeyId > 0) {
                            $('#txtId').val(data.KeyId);
                            setSoStatusFk(data.SOStatus_FK, function () {
                                controlButtonByStatusKF(id, gstatusid, Isview);
                            });

                            document.getElementById('txtProjectid').setAttribute('data-id', data.ProjectFK);
                            $('#txtCustomerName').text('');
                            for (var i = 0; i < glistProject.length; i++) {
                                if (glistProject[i].Id == data.ProjectFK) {
                                    $('#txtProjectid').val(glistProject[i].Name);
                                    $('#txtCustomerName').text(glistProject[i].Des);
                                    break;
                                }
                            }
                            $('#dtDateCreated').val(general.dateFormatJson(data.DateCreated, true));
                            $('#txtVersionName').val(data.Version_name);
                            $('#lblSO_ref').text(data.SO_ref);
                            $('#txtDescription').val(data.Description);
                            $('#txtSubTotal').text(general.commaSeparateNumber(data.Subtotal));
                            $('#txtGeneralExpensesRate').val(data.GeneralExpensesRate);
                            $('#txtGeneralExpensesAmount').text(general.commaSeparateNumber(data.GeneralExpensesAmount));
                            $('#txtprofitRate').val(data.ProfitRate);
                            $('#txtProfitAmount').text(general.commaSeparateNumber(data.ProfitAmount));
                            $('#txtVATRate').val(data.TaxRate);
                            $('#txtTax_Amount').text(general.commaSeparateNumber(data.Tax_Amount));
                            $('#txtTotal_Order').text(general.commaSeparateNumber(data.Total_Order));
                            $('#txtK1').val(data.Discount_K1);
                            gIdRow = data.maxIdOrderdetail;
                        }
                    }
                    else {
                        general.notify('Lỗi ! Đã gửi phản hồi về lỗi !', 'error');
                    }
                },
                error: function (error) {
                    $('#Page-searchBoq1').show();
                    $('#Page-mainBoq1').hide();
                    return false;
                }
            });
        }
    }
    function getDetail(id, callback) {
        if (id != 0) {
            $.ajax({
                type: 'GET',
                url: '/SalesOrderBOQ1/GetListDetailWithSoid',
                data: { Soid: id },
                success: function (response) {
                    console.log(response);
                    callback(response);
                },
                error: function (error) {
                }
            });
        }
    }
    function loadDetail(data) {
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
                ProductQty: general.toMoney(item.QtyOrdered),
                MaterialPrice: general.toMoney(Math.round(item.Material_Price)),
                LaborPrice: general.toMoney(Math.round(item.Labor_Price)),
                EquipmentPrice: general.toMoney(Math.round(item.Equipment_Price)),
                totalPriceValue: item.TotalPrice,
                TotalPrice: general.toMoney(Math.round(item.TotalPrice)),
                SubTotalValue: item.Subtotal,
                SubTotal: general.toMoney(Math.round(item.Subtotal)),
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
                        //if (tt.tblUnit == null) {
                        //    console.log('lỗi unitName: ');
                        //    console.log(tt);
                        //}
                        var ttvl = {
                            BomDetailFK: tt.BomDetailFK,
                            NameBomdetail: tt.NameBomdetail,
                            Product_Unit: tt.UnitFK,
                            UnitName: tt.UnitName,
                            Product_Type: tt.Product_Type,
                            ProductTypeName: tt.tblProductType != null ? tt.tblProductType.ProductTypeName : '',
                            priceB1: tt.PriceB1,
                            Note: tt.Note
                        };
                        dataMaterialOfProject.push(ttvl);
                    }
                }
            }
        });
        if (render != '')
            $('#table-quantity-content').html(render);
        gtblListOrderDetail = data;
        //thiết lập loại row
        var row = $('#table-quantity-content tr');
        $('#table-quantity-content tr').each(function () {
            var unitid = general.toInt($(this).children('#unitReal').text());
            if (unitid == general.unit.Header) {
                $(this).addClass('header-style');
            }
            else if (unitid == general.unit.Note) {
                $(this).addClass('note-style');
            }
            else {
                $(this).addClass('bom-style');
                var index = $(this).index();
                var unitidAfter = general.toInt(row.eq(index + 1).children('#unitReal').text());
                if (unitidAfter == general.unit.Note)
                    $(this).children('#productqty').children('#txtProductQty').prop('readonly', true);
            }
        });
    }
    function loadODImport(data) {
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
                MaterialPrice: general.toMoney(Math.round(item.Material_Price)),
                LaborPrice: general.toMoney(Math.round(item.Labor_Price)),
                EquipmentPrice: general.toMoney(Math.round(item.Equipment_Price)),
                TotalPrice: general.toMoney(Math.round(item.TotalPrice)),
                totalPriceValue: item.TotalPrice,
                SubTotalValue: item.Subtotal,
                SubTotal: general.toMoney(Math.round(item.Subtotal)),
                Note: item.Note,
                IdRow: item.IdRow,
                OrderDetailOld: item.KeyId,
                UnitReal: item.UnitFK,
                ParentId: item.Parent_Id
            });
        });
        if (render != '')
            $('#table-quantity-content').append(render);
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
        general.stopLoad();
        $('body').css({ 'opacity': 1 });
        $('#btn-Save-rev').hide();
        $('#btn-Save-boq1').hide();
    }
    function deleteEmptyRow(len, callback) {
        var S = 0;
        var i = 0;
        $('#table-quantity-content tr').each(function () {
            var _idRow = general.toInt($(this).children('#idrow').text());
            if (_idRow == 0)
                $(this).remove();
            else {
                var unitid = general.toInt($(this).children('#unitReal').text());
                if (unitid == general.unit.Header) {
                    var level = general.toInt($(this).children('#levelHeader').children('#txtLevelHeader').val());
                    if (level == 1) {
                        var subtotal = general.toFloat($(this).children('#subTotal').data('value'));
                        S += subtotal;
                    }
                }
            }
            i++;
        });
        if (i == len)
            callback(S, i + 1);
        else {
            general.notify('Lỗi ở xóa và cập nhật tổng !', 'error');
        }
    }
    function resetForm() {
        $('#txtProjectid').val('');
        document.getElementById('txtProjectid').setAttribute('data-id', '');
        $('#txtId').val(0);
        $('#txtCustomerName').text('');
        var now = new Date();
        $('#dtDateCreated').val(general.dateFormatJson(now), true);
        $('#txtVersionName').val('');
        $('#lblSO_ref').text('');
        $('#txtDiscription').val('');
        $('#txtDescription').val('');
        $('#txtSubTotal').text(0);
        $('#txtGeneralExpensesRate').val(0);
        $('#txtGeneralExpensesAmount').text(0);
        $('#txtprofitRate').val(0);
        $('#txtProfitAmount').text(0);
        $('#txtVATRate').val(0);
        $('#txtTax_Amount').text(0);
        $('#txtTotal_Order').text(0);
        $('#txtK1').val(0);
        $('#table-quantity-content tr').each(function () {
            $(this).remove();
        });

        $('#btn-Save-rev').hide();
        $('#btn-Save-boq1').hide();
        $('#btn-cancel-boq1').hide();
        $('#btnConfirm').hide();
        $('#btnUnConfirm').hide();
        $('#btnRequiteConfirm').hide();
        $('#btnConfirmTT').hide();
        $('#btn-import').hide();
        $('#btn-export').hide();
        $('#btn-print').hide();
        $('#chkEditHeader').prop('disabled', false);
        $('#chkEditHeader').prop('checked', false);
        gIdRow = 1;
        //   glistOrderStatus = [];
        dataMaterialOfProject = [];
        gtblListOrderDetail = [];
        // glistProject = [];
        ListCodeOrderDetailDel = [];
        gstatusid = general.soStatus.create;
        gstatusName = '';
    }
    function setSoStatusFk(statusid, callback) {
        var kt = false;
        for (var i = 0; i < glistOrderStatus.length; i++)
            if (glistOrderStatus[i].Id == statusid) {
                gstatusid = statusid;
                gstatusName = glistOrderStatus[i].Name;
                kt = true;
                callback();
                break;
            }
        if (kt == false) {
            general.notify('Chưa tìm thấy trạng thái !');
        }
        return false;
    }
    function hideColumn(check) {
        if (check.is(':checked')) {
            $('#materialPrice-th').hide();
            $('#laborPrice-th').hide();
            $('#equipmentPrice-th').hide();
            $('#table-quantity-content tr').each(function () {
                $(this).children('#materialPrice').hide();
                $(this).children('#laborPrice').hide();
                $(this).children('#equipmentPrice').hide();
            });
        }
        else {
            $('#materialPrice-th').show();
            $('#laborPrice-th').show();
            $('#equipmentPrice-th').show();
            $('#table-quantity-content tr').each(function () {
                $(this).children('#materialPrice').show();
                $(this).children('#laborPrice').show();
                $(this).children('#equipmentPrice').show();
            });
        }
    }
    function hideLevelColumn(check) {
        if (check.is(':checked')) {
            $('#levelHeader-th').show();
            $('#table-quantity-content tr').each(function () {
                $(this).children('#levelHeader').show();
                var lv = general.toInt($(this).children('#levelHeader').children('#txtLevelHeader').val());
                if (lv == 0)
                    $(this).hide();
                else {
                    $(this).children('#btn').children('.btn-plus').children('i').addClass('fa-plus').removeClass('fa-minus');
                    $(this).children('#btn').children('.btn-plus').data('btn', 0);
                }
            });
            check.prop('disabled', true);
            $('#btn-Save-boq1').hide();
            $('#btn-Save-rev').hide();
            $('#btn-update-header').prop('disabled', false);
        }
        else {
            $('#levelHeader-th').hide();
            $('#table-quantity-content tr').each(function () {
                $(this).children('#levelHeader').hide();
                var lv = general.toInt($(this).children('#levelHeader').children('#txtLevelHeader').val());
                if (lv == 0)
                    $(this).show();
                else {
                    $(this).children('#btn').children('.btn-plus').children('i').addClass('fa-minus').removeClass('fa-plus');
                    $(this).children('#btn').children('.btn-plus').data('btn', 1);
                }
            });
            $('#btn-update-header').prop('disabled', true);
            $('#btn-Save-boq1').show();
            if (gId > 0)
                $('#btn-Save-rev').show();
        }
    }
    function showHideCol(newrow) {
        if (!$('#chkEditHeader').is(':checked')) {
            newrow.children('#levelHeader').hide();
        }
        else
            newrow.children('#levelHeader').show();
        if ($('#chkHidePrice').is(':checked')) {
            newrow.children('#materialPrice').hide();
            newrow.children('#laborPrice').hide();
            newrow.children('#equipmentPrice').hide();
        }
    }
    function controlButtonByStatusKF(id, SOstatus, IsView) {
        if (id > 0) {
            var btn = $('#btnRequiteConfirm');
            if (IsView) {
                $('#btn-export').show();
                $('#btn-print').show();
                if (btn.length == 0) {
                    // có quyền xác nhận
                    if (SOstatus != general.soStatus.CompletedBoq1) {
                        $('#btnConfirmTT').show();
                        if (SOstatus != general.soStatus.ConfirmedBoq1) {
                            $('#btnConfirm').show();
                        }
                        if (SOstatus != general.soStatus.NoConfirmBoq1)
                            $('#btnUnConfirm').show();
                    }
                }
                else {
                    if (SOstatus != general.soStatus.RequestedConfirmBoq1 || SOstatus != general.soStatus.ConfirmedBoq1) {
                        $('#btnRequiteConfirm').show();
                    }
                }
            }
            else {
                if (btn.length == 0) {
                    //có quyền xác nhận
                    if (SOstatus != general.soStatus.CompletedBoq1) {
                        $('#btn-import').show();

                        $('#btn-Save-rev').show();
                        $('#btn-Save-boq1').show();
                        $('#btn-cancel-boq1').show();
                    }
                }
                else {
                    // chỉ có quyền trình lên
                    if (SOstatus != general.soStatus.CompletedBoq1 || SOstatus != general.soStatus.ConfirmedBoq1) {
                        $('#btn-import').show();

                        $('#btn-Save-rev').show();
                        $('#btn-Save-boq1').show();
                        $('#btn-cancel-boq1').show();
                    }
                }
            }
        }
        else //tạo mới
        {
            //
            var btn = $('#btn-import');
            if (btn.length != 0) {
                btn.show();
                $('#btn-Save-boq1').show();
                $('#btn-cancel-boq1').show();
            }
        }
    }
    function DeleteOrderDetail(data, callback) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ1/DeleteListOrderdetail',
            data: { listid: data },
            success: function (res) {
                callback(res);
            },
            error: function (error) {
            }
        });
    }
    function Save(boq1) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ1/SaveEntity',
            data: boq1,
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (res) {
                console.log(res);
                general.notify('Ghi thành công!', 'success');
                //ghi mới
                if (boq1.KeyId == 0) {
                    gupdatestatus.projectstatus(boq1.ProjectFK, general.projectStatus.buildingBoq1);
                    for (var i = 0; i < glistProject.length; i++)
                        if (glistProject[i].Id == boq1.ProjectFK) {
                            glistProject[i].SoStatusKf = general.projectStatus.buildingBoq1;
                            break;
                        }
                    if (gstatusid == general.soStatus.NoConfirmBoq1) {
                        setSoStatusFk(general.soStatus.create, function () {
                            gupdatestatus.boqSoStatusFk(boq1.KeyId, gstatusid);//update lại trạng thái đang tạo
                        })
                    }
                }
                $('#btn-cancel-boq1').click();
                general.stopLoad();
            },
            error: function (error) {
                general.notify('Có lỗi trong khi ghi!', 'error');
                general.stopLoad();
            }
        });
    }
    function getLevelHeaderNear(indexrow, callback) {
        var row = $('#table-quantity-content tr');
        if (indexrow == 0)//hàng đầu tiên thì level luôn phải là 1
            callback(0);
        var i = indexrow - 1;
        var _unit;
        do {
            _unit = general.toInt(row.eq(i).children('#unitReal').text());
            if ((_unit == general.unit.Header))//là header thì thoát
                break;
            i--;
        } while (i >= 0);
        if (i >= 0)
            callback(general.toInt(row.eq(i).children('#levelHeader').children('#txtLevelHeader').val()));
        else
            callback(0);//bắt buộc phải nhập level bằng 1
    }

    function RequireConfirm(keyid) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ1/RequireConfirm',
            data: { KeyId: keyid },
            success: function (res) {
                general.notify('Yêu cầu xác nhận BOQ1 thành công!', 'success');
            },
            error: function (err) {
                general.notify('Có lỗi khi ghi yêu cầu xác nhận BOQ1', 'error');
            }
        });
    }
    function Confirm(keyid) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ1/Confirm',
            data: { KeyId: keyid },
            success: function (res) {
                general.notify('Xác nhận BOQ1 thành công!', 'success');
            },
            error: function (err) {
                general.notify('Có lỗi khi xác nhận BOQ1', 'error');
            }
        });
    }
    function ConfirmTT(keyid) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ1/ConfirmTT',
            data: { KeyId: keyid },
            success: function (res) {
                general.stopLoad();
                general.notify('Xác nhận trúng thầu thành công!', 'success');
            },
            error: function (err) {
                general.stopLoad();
                general.notify('Có lỗi khi xác nhận trúng thầu', 'error');
            }
        });
    }
    function UnConfirm(keyId) {
        $.ajax({
            type: 'POST',
            url: '/SalesOrderBOQ1/UnConfirm',
            data: { KeyId: keyid },
            success: function (res) {
                general.stopLoad();
                general.notify('Không xác nhận trúng thầu!', 'success');
            },
            error: function (err) {
                general.stopLoad();
                general.notify('Có lỗi khi không xác nhận trúng thầu', 'error');
            }
        });
    }
}