var contractController = function () {
    var dataMaterialOfProject = [];
    var glistOrderStatus = [];
    var gstatusid = 1;
    var gstatusName = 'Test thử';
    var gboq1;
    var glistod = [];
    this.initialize = function () {
        registerEvents();
        loadOrderStatus();
        delayStatus(function () {
            var status = $('#lblSoStatus').text();
            if (status == "") {
                $('#lblSoStatus').text(gstatusName);
            }
            else
                $('#lblSoStatus').text('');
        }, 500)
    }
    this.loadbyid = function (id,IsView) {
        if (id != 0) {
            general.startLoad();
            $('#txtCustomerPONumber').prop('disabled', IsView);
            $('#txtDescription').prop('disabled', IsView);
            $('#dtCustomerPODate').prop('disabled', IsView);

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
                }
            }
        });
        $('#btn-info').on('click', function () {
            $('.contain-info').toggle();
        });
        $('#btn-Save-Contract').on('click', function () {
            var id = general.toInt($('#txtId').text());
            var customerPONumber = $('#txtCustomerPONumber').val();
            var dtCustomerPODate = $('#dtCustomerPODate');
            var customerPODate = '';
            if (dtCustomerPODate.data('date') != '') // co chon tg
                customerPODate = general.dateFormatJson(dtCustomerPODate.data("datetimepicker").getDate(), false);
            var note = $('#txtDescription').val();
            if (customerPONumber == '' || customerPODate == '') {
                general.notify('Vui lòng điền đầy đủ thông tin trước !', 'error');
                return false;
            }
            $.ajax({
                type: 'POST',
                url: 'contract/UpdateContract',
                data: { soid: id, shd: customerPONumber, date: customerPODate, note: note },
                beforeSend: function () {
                    general.startLoad();
                },
                success: function (res) {
                    general.stopLoad();
                    $('#btnRefresh-search').click();
                    $('#Page-searchContract').show();
                    $('#Page-mainContract').hide();
                    general.notify('Ghi thành công!', 'success');
                    resetForm();
                    general.stopLoad();
                },
                error: function (err) {
                    general.stopLoad();
                    general.notify('Có lỗi trong khi ghi!', 'error');
                }
            });
        });
        $('#btnConfirm').on('click', function () {
            general.startLoading();
            if (gstatusid == general.soStatus.CompletedBoq1) {
                
                //1. ghi vào nhật ký chứng từ
                //2. ghi nợ phải thu
                var accountReceivable;
                var generalJournal;
                var customerLiability;
                var projectDetail;
                var data;
                var des = "Nguồn từ " + gboq1.KeyId + " của công trình có mã căn là " + gboq1.Gnproject.ConstructionId;
                loadAccountDefault(function (reponse) {
                    accountReceivable = {
                        TransactNo: gboq1.KeyId,
                        Transaction_Date: general.dateFormatJson(new Date(),false),
                        ArNo: reponse.DebitAccount,
                        CustomerFk: gboq1.Gnproject.CustomerFk,
                        ProjectId: gboq1.ProjectFK,
                        Amount: gboq1.Total_Order,
                        Type: general.code.SalesOrder,
                        Description: des
                    };
                    generalJournal = {
                        DebitAccount: reponse.DebitAccount,
                        CreditAccount: reponse.CreditAccount,
                        Description: des,
                        Reference: gboq1.KeyId,
                        ReferenceDate: general.dateFormatJson(new Date(), false),
                        Status: 0,
                        Amount: gboq1.Total_Order,
                        Type: general.code.SalesOrder
                    };
                    
                    // 3.Cập nhật thông tin cho bảng customerliabilities
                    // khác nhau giữa hợp đồng với phục lúc là hợp đồng thì tạo mới ở customerliabilities còn phụ lục thì cộng thêm vào
                    if ($('#lblSO_ref').text() == '') {//là hợp đồng
                        customerLiability = {
                            ProjectFk: gboq1.ProjectFK,
                            ContractAmount: gboq1.Total_Order,
                            IncurredAmount: 0,
                            SettlementAmount: gboq1.Total_Order,
                            PaidAmount: 0,
                            ReceivableAmount: gboq1.Total_Order
                        };
                    }
                    else {//là phụ lục
                        // cộng tiền lên cho công trình đó ở phần phát sinh, giá trị quyết toán, giá trị phải thu.
                        UpdateTotal();
                        // ghi thêm vào project detail 1 cột phụ lục
                        projectDetail = {
                            KeyId: 0,
                            ProjectFk: gboq1.ProjectFK,
                            SoFk: gboq1.KeyId,
                            Amount: gboq1.Total_Order
                        };
                    }
                    // 4. Tạo ra tổng hợp vật tự cho dự án (gồm cả đơn giá và khối lượng tổng)
                    
                    //Tạo acceptance
                    getAcceptanceDetail(function (listDetail) {
                        data = {
                            KeyId: 0,
                            SO_FK: gboq1.KeyId,
                            PaymentPeriod: 1,
                            AcceptanceStatus_FK: general.acceptanceStatus.draft,
                            Total: gboq1.Subtotal,
                            tblBoqAcceptanceDetails: listDetail
                        };
                    });
                    var list = {
                        Acceptance: data,
                        accountReceivable: accountReceivable,
                        generalJournal: generalJournal,
                        customerLiabilities: customerLiability,
                        projectDetail: projectDetail,
                        SyntheticMaterials: dataMaterialOfProject,
                        projectId: gboq1.ProjectFK,
                        SOId: gboq1.KeyId
                    };
                    $.ajax({
                        type: 'POST',
                        url: '/Contract/ConfirmContract',
                        data: list,
                        beforeSend: function () {
                            general.startLoad();
                        },
                        success: function (res) {
                            general.notify('Đưa vào nội bộ thành công!', 'success');
                            $('#btnConfirm').hide();
                            $('#btnUnConfirm').hide();
                            $('#btnRefresh-search').click();
                            // chuyển trạng thái so sang đã kết thúc boq1
                            setSoStatusFk(general.soStatus.FinishBOQ1, function () {
                            });
                            $('#btn-cancel-Contract').click();
                            general.stopLoad();
                        },
                        error: function (err) {
                            general.stopLoad();
                            general.notify('Có lỗi khi xác nhận đưa vào nội bộ', 'error');
                        }
                    });
                    for (var i = 0; i < glistOrderStatus.length; i++) {
                        if (glistOrderStatus[i] == general.soStatus.Finish) {
                            gstatusName = glistOrderStatus[i].SOStatusName;
                        }
                    }
                    
                });
            }
        });
        $('#btnCreate').on('click', function () {
            window.location.replace('/SalesOrderBOQ1?soref=' + gboq1.KeyId + '&projectId=' + gboq1.ProjectFK);
        });
    }

    function UpdateTotal() {
        $.ajax({
            type: 'POST',
            url: '/customerliabilities/UpdateBySORef',
            data: { projectId: gboq1.ProjectFK, val: gboq1.Total_Order },
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                console.log(response);
                general.stopLoad();
            },
            error: function (error) {
                general.stopLoad();

            }
        });
    }
    
    function getValue(id, Isview, callback) {
        if (id != 0) {
            $.ajax({
                type: 'GET',
                url: '/SalesOrderBOQ1/GetById',
                data: { id: id },
                async: false,
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    console.log(response);
                    if (response != null) {
                        gboq1 = response;
                        var data = response;
                        callback();
                        if (data.KeyId > 0) {
                            $('#txtId').text(data.KeyId);
                            $('#txtProjectid').data('id', data.ProjectFK);
                            $('#txtCustomerName').text(data.Gnproject.CustomerFkNavigation.UserBy.FullName);
                            $('#txtProjectid').text(data.Gnproject.ConstructionName);
                            $('#dtDateCreated').text(general.dateFormatJson(data.DateCreated, true));
                            $('#txtVersionName').text(data.Version_name);
                            $('#lblSO_ref').text(data.SO_ref);
                            $('#txtDescription').val(data.Description);
                            $('#txtSubTotal').text(general.commaSeparateNumber(data.Subtotal));
                            $('#txtGeneralExpensesRate').text(data.GeneralExpensesRate);
                            $('#txtGeneralExpensesAmount').text(general.commaSeparateNumber(data.GeneralExpensesAmount));
                            $('#txtprofitRate').text(data.ProfitRate);
                            $('#txtProfitAmount').text(general.commaSeparateNumber(data.ProfitAmount));
                            $('#txtVATRate').text(data.TaxRate);
                            $('#txtTax_Amount').text(general.commaSeparateNumber(data.Tax_Amount));
                            $('#txtTotal_Order').text(general.commaSeparateNumber(data.Total_Order));
                            $('#txtK1').text(data.Discount_K1);
                            $('#txtCustomerPONumber').val(data.CustomerPONumber);
                            if (data.CustomerPODate != null)
                                $('#dtCustomerPODate').data('datetimepicker').setValue(data.CustomerPODate);
                            setSoStatusFk(data.SOStatus_FK, function () {
                                controlButtonByStatusKF(id, gstatusid, Isview);
                            });
                        }
                    }
                    else {
                        general.notify('Lỗi ! Đã gửi phản hồi về lỗi !', 'error');
                    }
                    general.stopLoad();
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
                url: '/SalesOrderBOQ1/GetListDetailWithSoid',
                data: { Soid: id },
                success: function (response) {
                    console.log(response);
                    glistod = response;
                    callback(response);
                },
                error: function (error) {

                }
            });
        }
    }
    function loadDetail(data) {
        general.startLoading();
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
                    for (; j < dataMaterialOfProject.length; j++) {
                        if (tt.BomDetailFK == dataMaterialOfProject[j].BomDetailFK) {
                            flag = 1;
                            break;
                        }
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
                            PriceB2: 0,
                            TotalQtyB1: item.QtyOrdered * tt.Qty,
                            TotalQtyB2: 0,
                            TotalQtyOrdered: 0,
                            Note: tt.Note
                        };
                        dataMaterialOfProject.push(ttvl);
                    }
                    else
                        dataMaterialOfProject[j].TotalQtyB1 += item.QtyOrdered * tt.Qty;
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
        general.stopLoad();
    }
    function getAcceptanceDetail(callback) {
        var listDetail = [];
        for (var i = 0; i < glistod.length; i++) {
            var item = glistod[i];
            var temp = {
                OrderDetail_FK: item.KeyId,
                Unit_FK: item.UnitFK,
                Unit_Name: item.UnitName,
                QtyOrdered: item.QtyOrdered,
                AccumulatedLastQtyAcc: 0,
                UnitPrice: item.TotalPrice,
                AccumulatedNowQtyAcc: 0
            }
            listDetail.push(temp);
        }
        if (i == glistod.length)
            callback(listDetail);

    }
    //load status
    function loadOrderStatus() {
        glistOrderStatus = [];
        $.ajax({
            type: 'GET',
            url: '/SalesOrderBOQ1/GetListStatus',
            dataType: 'json',
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                console.log(response);
                if (response.length > 0) {
                    glistOrderStatus = response;
                }
                general.stopLoad();
            },
            error: function (status) {
                console.log(status);
                general.stopLoad();
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
                var customerPON = $('#txtCustomerPONumber').val();
                var customerPOD = $('#dtCustomerPODate').data('date');
                if (SOstatus != general.soStatus.CompletedBoq1 || customerPOD == '' || customerPON == '') {
                    $('#btnConfirm').hide();
                    $('#btnUnConfirm').hide();
                }
                else {
                    $('#btnConfirm').show();
                    $('#btnUnConfirm').show();
                }
                $('#btn-Save-Contract').hide();
                $('#btn-cancel-Contract').hide();
                $('#btnCreate').show();
            }
            else {
                $('#btnConfirm').hide();
                $('#btnUnConfirm').hide();
                $('#btn-export').hide();
                $('#btn-print').hide();
                $('#btn-Save-Contract').show();
                $('#btn-cancel-Contract').show();
                $('#btnCreate').hide();
            }
        }
    }
    function loadAccountDefault(callback) {
        $.ajax({
            type: 'GET',
            url: '/SalesOrderBOQ1/GetAccountDefault',
            data: { codeSelect: general.accountDefault.AccountForContract },
            beforeSend: function () {
                general.startLoad();
            },
            success: function (response) {
                callback(response);

                general.stopLoad();

            },
            error: function (err) {
                general.stopLoad();
            }
        });
    }
    function resetForm() {
        $('#txtProjectid').text('');
        $('#txtId').text(0);
        $('#txtCustomerName').text('');
        $('#dtDateCreated').text();
        $('#txtVersionName').text('');
        $('#lblSO_ref').text('');
        $('#txtDescription').val('');
        $('#txtSubTotal').text(0);
        $('#txtGeneralExpensesRate').text(0);
        $('#txtGeneralExpensesAmount').text(0);
        $('#txtprofitRate').text(0);
        $('#txtProfitAmount').text(0);
        $('#txtVATRate').text(0);
        $('#txtTax_Amount').text(0);
        $('#txtTotal_Order').text(0);
        $('#txtK1').text(0);
        $('#txtCustomerPONumber').val('');
        $('#dtCustomerPODate').data('datetimepicker').reset();
        $('#table-quantity-content tr').each(function () {
            $(this).remove();
        });

        $('#btn-Save-Contract').hide();
        $('#btnConfirm').hide();
        $('#btnUnConfirm').hide();
        $('#btnCreate').hide();
        $('#btn-export').hide();
        $('#btn-print').hide();

        dataMaterialOfProject = [];
        gstatusid = 0;
        gstatusName = '';
        gboq1='';
        glistod = [];
    }
}