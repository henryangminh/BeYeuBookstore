var PurchaseOrderDetailController = function () {
    var gIsView = false;
    this.initialize = function () {
        loadUnit();
        registerEvents();
    }
    function registerEvents() {
        $('#txtCode').prop('disabled', true);
        $('#txtSubTotalOrder').prop('disabled', true);
        $('#btn-add-detail').on('click', function () {
            if (!gIsView) {
                //reset dữ liệu trước
                $('#txtSelectCode').val('');
                $('#txtSelectName').val('');
                purchaseOrder.resetFormSelectDetail();
                $('#modal-select-bom-order').modal('show');
            }
        })
    }
    function loadUnit() {
        $.ajax({
            type: 'GET',
            url: 'Unit/GetAllUnitNameAndRound',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                if (response.length > 0) {
                    autoUnit(document.getElementById('txtUnit'), response);
                }
                else {
                    general.notify('Chưa có đơn vị tính vui lòng thêm đơn vị tính trước và thử lại !', 'error');
                }
            },
            error: function (status) {
                general.notify('Tải dữ liệu đơn vị tính bị lỗi ! vui lòng kiểm tra lại ', 'error');
            }
        })
    }
}