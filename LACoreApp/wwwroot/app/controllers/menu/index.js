var menuController = function () {
    this.initialize = function () {
        loadGroups();
        loadTypes();
        loadData();
        registerEvents();
    }
    function registerEvents() {
        var type = $("#ddlType").val();
        $("#frmMaintainance").validate({
            errorClass: "red",
            ignore: [],
            lang: "en",
            focusInvalid: false,
            invalidHandler: function () {
                $(this).find(":input.error:first").focus();
            },
            rules: {
                txtNameM: { required: true },
                txtOrderM: { number: true },
                txtURL: {
                    required: function () {
                        return type == 3;
                    }
                },
                ddlCategory: {
                    required: function () {
                        return type == 1 || type == 2;
                    }
                }
            }
        });

        $('#btnCreate').off('click').on('click', function () {
            resetFormMaintainance();
            initTreeDropDownParent();
            $('#modal-add-edit').modal('show');
        });
        $('#ddlType').on('change', function () {
            var type = $('#ddlType').val();
            if (type == 1 || type == 2) {
                $('#ddlCategory').parents('.form-group').removeClass('hidden');
                $('#txtURL').parents('.form-group').addClass('hidden');
                initTreeDropDownCatagory(type)
            }
            else if (type == 3) {
                $('#ddlCategory').parents('.form-group').addClass('hidden');
                $('#txtURL').parents('.form-group').removeClass('hidden');
                $('#txtURL').val('');
            }
            else {
                $('#ddlCategory').parents('.form-group').addClass('hidden');
                $('#txtURL').parents('.form-group').addClass('hidden');
            }
        });

        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            $.ajax({
                type: "GET",
                url: "/Admin/Menu/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    tedu.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownParent(data.ParentId);

                    $('#txtURL').val(data.URL);

                    $('#ddlGroup').val(data.Group);
                    initTreeDropDownCatagory(data.Type, data.CategoryId)
                    $('#ddlType').val(data.Type);

                    $('#txtOrderM').val(data.SortOrder);

                    $('#ckStatusM').prop('checked', data.Status === 1);

                    $('#modal-add-edit').modal('show');
                    tedu.stopLoading();

                },
                error: function (status) {
                    tedu.notify('Có lỗi xảy ra', 'error');
                    tedu.stopLoading();
                }
            });
        });

        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            tedu.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Menu/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        tedu.startLoading();
                    },
                    success: function (response) {
                        tedu.notify('Deleted success', 'success');
                        tedu.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        tedu.notify('Has an error in deleting progress', 'error');
                        tedu.stopLoading();
                    }
                });
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var parentId = $('#ddlParentIdM').combotree('getValue');
                var group = $('#ddlGroup').val();
                var type = $('#ddlType').val();
                var order = parseInt($('#txtOrderM').val());
                var status = $('#ckStatusM').prop('checked') == true ? 1 : 0;
                var url = $('#txtURL').val();
                if (type != 3) {
                    url = $('#ddlCategory').combotree('getValue');
                }

                $.ajax({
                    type: "POST",
                    url: "/Admin/Menu/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        ParentId: parentId,
                        URL: url,
                        SortOrder: order,
                        Group: group,
                        Type: type,
                        Status: status
                    },
                    dataType: "json",
                    beforeSend: function () {
                        tedu.startLoading();
                    },
                    success: function (response) {
                        tedu.notify('Update success', 'success');
                        $('#modal-add-edit').modal('hide');

                        resetFormMaintainance();

                        tedu.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        tedu.notify('Has an error in update progress', 'error');
                        tedu.stopLoading();
                    }
                });
            }
            return false;

        });

        if ($('#ddlType').val() != 3) {
            $('#ddlCategory').parents('.form-group').removeClass('hidden');
            $('#txtURL').parents('.form-group').addClass('hidden');
        } else {
            $('#ddlCategory').parents('.form-group').addClass('hidden');
            $('#txtURL').parents('.form-group').removeClass('hidden');
        }
    }
    function resetFormMaintainance() {
        $('#hidIdM').val('00000000-0000-0000-0000-000000000000');
        $('#txtNameM').val('');
        initTreeDropDownParent('');

        $('#txtURL').val('');
        $('#txtOrderM').val(1);
        $('#ddlGroup').val('');
        $('#ddlType').val('');
        $('#ddlCategory').val('');
        $('#ckStatusM').prop('checked', true);
    }
    function initTreeDropDownParent(selectedId) {
        $.ajax({
            url: "/Admin/Menu/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = tedu.unflattern(data);
                $('#ddlParentIdM').combotree({
                    data: arr
                });
                if (selectedId != undefined) {
                    $('#ddlParentIdM').combotree('setValue', selectedId);
                }
            }
        });
    }
    function loadGroups() {
        $.ajax({
            type: 'GET',
            url: '/admin/menu/GetGroups',
            dataType: 'json',
            success: function (response) {
                var render = "<option value=''>--Select group--</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>"
                });
                $('#ddlGroup').html(render);
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading group', 'error');
            }
        });
    }

    function loadTypes() {
        $.ajax({
            type: 'GET',
            url: '/admin/menu/GetTypes',
            dataType: 'json',
            success: function (response) {
                var render = "<option value=''>--Select type--</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>"
                });
                $('#ddlType').html(render);
            },
            error: function (status) {
                console.log(status);
                tedu.notify('Cannot loading type', 'error');
            }
        });
    }

    function initTreeDropDownCatagory(type, selectedCategory) {
        var test = parseInt(type);
        $.ajax({
            url: "/Admin/Menu/GetCategory",
            type: 'GET',
            dataType: 'json',
            data: {
                type: test
            },
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = tedu.unflattern(data);
                $('#ddlCategory').combotree({
                    data: arr
                });
                if (selectedCategory != undefined) {
                    $('#ddlCategory').combotree('setValue', selectedCategory);
                }
            }
        });
    }

    function loadData() {
        $.ajax({
            url: '/Admin/Menu/GetAll',
            dataType: 'json',
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });

                });
                var treeArr = tedu.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                //var $tree = $('#treeProductCategory');

                $('#treeParent').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        //$('#tt').tree('select', node.target);
                        $('#hidIdM').val(node.id);
                        // display context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) {
                        console.log(target);
                        console.log(source);
                        console.log(point);
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });

                            //Update to database
                            $.ajax({
                                url: '/Admin/Menu/UpdateParentId',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function (res) {
                                    loadData();
                                }
                            });
                        }
                        else if (point === 'top' || point === 'bottom') {
                            $.ajax({
                                url: '/Admin/Menu/ReOrder',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function (res) {
                                    loadData();
                                }
                            });
                        }
                    }
                });

            }
        });
    }
}