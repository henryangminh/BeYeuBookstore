var ProjectController = function () {
    var self = this;
    var glistCustomer = [];
    var glistStatus = [];
    var glistMenuDocument = [];
    var gStatus = "";
    var gStatusFk=0;
    var gid;
    this.initialize = function () {
        loadData();
        loadCustomer(function (arr) {
            if (arr.length > 0)
                autocomplete(document.getElementById("txtCustomer"), arr);
        });
        delay(function () {
            var status = $('#lblStatus').text();
            if (status == "") {
                $('#lblStatus').text(gStatus);
            }
            else
                $('#lblStatus').text('');
        }, 500) 
        loadStatus();
        loadMenuDocument();
        registerEvents();
        initUserList();
    }

    function registerEvents() {
        //Init validation
        $("#txtLastUpdatedByFk").prop('disabled', true);
        $("#dtDateModified").prop('disabled', true);

        $("#txtConstructionId").attr('maxlength', '25');
        $("#txtConstructionName").attr('maxlength', '500');
        $("#txtProjectName").attr('maxlength', '500');
        $("#txtCustomer").attr('maxlength', '200');

        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtConstructionId: { required: true },
                txtConstructionName: { required: true },
                selCustomer: { required: true }
            }
        });

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                loadData(true);
            }
        });
        $("#btn-search").on('click', function () {
            loadData(true);
        });

       
        $("#ddl-show-page").on('change', function () {
            general.configs.pageSize = $(this).val();
            general.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            gStatusFk = general.projectStatus.create;
            if (glistStatus.length > 0) {
                for (var i = 0; i < glistStatus.length; i++)
                    if (glistStatus[i].Id == gStatusFk) {
                        gStatus = glistStatus[i].Name;
                        break;
                    }
            }

            // load menu document lên
            if (glistMenuDocument.length > 0) {
                var template = $('#template-table-detail').html();
                for (var i = 0; i < glistMenuDocument.length; i++) {
                    var render = Mustache.render(template, {
                        KeyId: 0,
                        Link:"",
                        stt: i + 1,
                        Name: glistMenuDocument[i].Name,
                        Note: "",
                        linkupload:""
                    });
                    $('#table-document-content').append(render);
                }
            }
            $('#modal-add-edit').modal('show');

        });
        $('#btn-add-document').on('click', function () {
            var len = $('#table-document-content tr').length;
            var template = $('#template-table-detail').html();
            var render = Mustache.render(template, { stt: len+1});
            $('#table-document-content').append(render);
        });

        //Grant permission
        $('body').on('click', '.btn-view', function () {
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            resetFormMaintainance();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Project/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    if (response != null) {
                        var data = response;
                        gid = that;
                        $('#txtConstructionId').val(data.ConstructionId);
                        $('#txtConstructionName').val(data.ConstructionName);
                        $('#txtProjectName').val(data.ProjectName);

                        if (glistCustomer.length > 0) {
                            for (var i = 0; i < glistCustomer.length;i++)
                                if (glistCustomer[i].Id == data.CustomerFK) {
                                    $('#txtCustomer').val(glistCustomer[i].Name);
                                    document.getElementById("txtCustomer").setAttribute('data-id', glistCustomer[i].Id);
                                    break;
                                }
                        }
                        gStatusFk = data.Status;
                        if (glistStatus.length > 0) {
                            for (var i = 0; i < glistStatus.length; i++)
                                if (glistStatus[i].Id == data.Status) {

                                    gStatus = glistStatus[i].Name;
                                    break;
                                }
                        }
                        if (data.LastUpdateByFkNavigation != null)
                            $('#txtLastUpdatedByFk').val(data.LastUpdateByFkNavigation.FullName);
                        $('#dtDateModified').val(general.dateTimeFormatJson(data.DateModified,true));

                        $('#modal-add-edit').modal('show');

                        general.stopLoading();
                    }
                },
                error: function (status) {
                    general.notify('Có lỗi xảy ra', 'error');
                    general.stopLoading();
                }
            });
        });
        $('body').on('click', '.btn-userproject', function (e) {
            var ProjectId = $(this).data('id');
            $('#hidProjectId').val(ProjectId);
            loadUserForProject(ProjectId);
            $('#lblConstructionName').text($(this).parent('td').siblings('#constructionid').text() + ' | ' + $(this).parent('td').siblings('#constructionname').text());
            $('#modal-user-project').modal('show');
        });

        $('body').on('click','#btnSelectImg',function () {
            $(this).parent('.input-group-btn').siblings('#fileInputImage').click();
        });

        $('body').on('change','#fileInputImage', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            if (files.length > 0) {
                var path = files[0].name;
                for (var i = 1; i < files.length; i++) {
                    path += ', ' + files[i].name ;
                }
                $(this).parent('.autocomplete').parent('#col-image').siblings('#linkupload').text(path);
            }
            //var data = new FormData();
            //for (var i = 0; i < files.length; i++) {
            //    data.append(files[i].name, files[i]);
            //}
            //$.ajax({
            //    type: "POST",
            //    url: "/Upload/UploadImage",
            //    contentType: false,
            //    processData: false,
            //    data: data,
            //    success: function (path) {
            //        $('#txtImage').val(path);
            //        general.notify('Upload image succesful!', 'success');

            //    },
            //    error: function () {
            //        general.notify('There was error uploading files!', 'error');
            //    }
            //});
        });
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var constructionId = $('#txtConstructionId').val();
                var constructionName = $('#txtConstructionName').val();
                var projectName = $('#txtProjectName').val();
                var customerFK = document.getElementById("txtCustomer").getAttribute('data-id');
                if (customerFK == "") {
                    general.notify("Vui lòng điền thông tin chủ đầu tư",'error')
                    return;
                }
                    
                //var LastUpdatedByFk = $('#user').data('userid');
                $.ajax({
                    type: "POST",
                    url: "/Project/SaveEntity",
                    data: {
                        KeyId: gid,
                        ConstructionId: constructionId,
                        ConstructionName: constructionName,
                        ProjectName: projectName,
                        CustomerFK: customerFK,
                        //LastUpdatedByFk: LastUpdatedByFk,
                        Status:gStatusFk
                    },
                    dataType: "json",
                    beforeSend: function () {
                        general.startLoading();
                    },
                    success: function (response) {
                        general.notify('Lưu thành công', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
                        general.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        general.notify('Ghi lỗi !', 'error');
                        general.stopLoading();
                    }
                });
                return false;
            }
        });
        $('#btnSaveUserProject').on('click', function (e) {
            var ProjectId = $('#hidProjectId').val();
            var listUserProject = [];
            $.each($('input[name="ckUsers"]'), function (i, item) {
                if ($(item).prop('checked') === true)
                    listUserProject.push($(item).prop('value'));               
            });
            $.ajax({
                type: "POST",
                url: "/Project/SaveUserProject",
                data: {
                    projectid: ProjectId,
                    listusers: listUserProject,
                },
                dataType: "json",
                beforeSend: function () {
                    general.startLoading();
                },
                success: function (response) {
                    general.notify('Lưu thành công', 'success');
                    $('#modal-user-project').modal('hide');
                    general.stopLoading();
                },
                error: function () {
                    general.notify('Ghi lỗi !', 'error');
                    general.stopLoading();
                }
            });
            return false;
        });
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            general.confirm('Bạn có chắc chắn xóa ?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Project/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        general.startLoading();
                    },
                    success: function (response) {
                        general.notify('Xóa thành công !', 'success');
                        general.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        general.notify('Lỗi xóa ! Vui lòng kiểm tra lại !', 'error');
                        general.stopLoading();
                    }
                });
            });
        });
        $('body').on('click', '.btn-delete-document', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });
    };
    function resetFormMaintainance() {
        $('#txtConstructionId').val('');
        $('#txtConstructionName').val('');
        $('#txtProjectName').val('');
        $('#txtCustomer').val('');
        $('#txtLastUpdatedByFk').val('');
        $('#dtDateModified').val('');
        $('#lblStatus').val('');
        $('#table-document-content tr').each(function () { $(this).remove(); });
      
    }

    var delay = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearInterval(timer)
            timer = setInterval(callback, ms);
        };
    })();
    function loadMenuDocument() {
        glistMenuDocument = [];
        $.ajax({
            type: 'GET',
            url: '/Project/GetListMenuDocument',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                if (response.length > 0) {
                    glistMenuDocument = response;
                }
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    function loadStatus() {
        glistStatus = [];
        $.ajax({
            type: 'GET',
            url: '/Project/GetListStatus',
            dataType: 'json',
            success: function (response) {
                console.log(response);
                if (response.length > 0) {
                    glistStatus = response;
                }
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
            }
        });
    }
    function loadCustomer(callBack) {
        glistCustomer = [];
        $.ajax({
            type: 'GET',
            data: {
                status: general.status.Active
            },
            url: '/Customer/GetListFullName',
            dataType: 'json',
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                if (response.length > 0) {
                    glistCustomer = response;
                    callBack(response);
                }
                general.stopLoading();
            },
            error: function (status) {
                console.log(status);
                general.notify('Không thể load dữ liệu', 'error');
                general.stopLoading();
            }
        });
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/Project/GetAllPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: general.configs.pageIndex,
                pageSize: general.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                general.startLoading();
            },
            success: function (response) {
                console.log(response);
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            stt: i + 1,
                            ConstructionId: item.ConstructionId,
                            ConstructionName: item.ConstructionName,
                            ProjectName: item.ProjectName,
                            CustomerName: item.CustomerFkNavigation.UserBy.FullName,
                            Status: item.StatusNavigation.SOStatusName,
                            KeyId: item.KeyId
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render != undefined) {
                        $('#tbl-content').html(render);

                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);


                }
                else {
                    $('#tbl-content').html('');
                }
                general.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

   
    function initUserList(selectedUsers) {
        $.ajax({
            url: "/Employee/GetListFullNameOfUser",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var template = $('#user-template').html();
                var data = response;
                var render = '';
                $.each(data, function (i, item) {
                    var checked = '';
                    if (selectedUsers !== undefined && selectedUsers.indexOf(item.Name) !== -1)
                        checked = 'checked';
                    render = Mustache.render(template,
                        {
                            id: item.Id,
                            Name: item.Name,
                            checked: false
                        });
                    if (i % 2 == 0) {
                        $('#list-user').append(render);
                    }
                    else {
                        $('#list-user1').append(render);
                    }
                 
                });
                //$('#list-user').html(render);
            }
        });
    }
    function loadUserForProject(that) {
        $.ajax({
            url: "/Project/GetUserByProjectFk",
            type: 'GET',
            data: { projectid: that },
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = response;
                $.each($('input[name="ckUsers"]'), function (i, item) {
                    var iduser = $(item).prop('value');
                    var i = 0;
                    for (; i < data.length; i++)
                        if (data[i].UserId == iduser) {
                            $(item).prop('checked', true);
                            break;
                        }
                    if (i == data.length) {
                        $(item).prop('checked', false);
                    }
                });
            }
        });
    }
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