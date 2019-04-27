$(document).ready(function () {
    setTimeout(function () {
        //$('#' + formID + ' *').unbind('click');


        var tabSelectedLi = $('li.tabs-selected');
        var selectedTabIndex = $('#tt ul.tabs li').index(tabSelectedLi);
        var selectedPanel = $('.panel.panel-htop')[selectedTabIndex];
        var formID = $(selectedPanel).find('form').attr('id');
        if ($('body').hasClass('modal-open'))
            formID = $('#commonModalContent').find('form').attr('id');
        var aspxURL = $(tabSelectedLi).find('.data-url').text();
        if ($('body').hasClass('modal-open'))
            aspxURL = $('#commonModalContent').find('form').attr('action');

        //if ($.isFunction(refreshContent)) {
        //    delete refreshContent;
        //    refreshContent = undefined;
        //}

        function refreshContent() {
            var tabSelectedLi = $('li.tabs-selected');
            var selectedTabIndex = $('#tt ul.tabs li').index(tabSelectedLi);
            var selectedPanel = $('.panel.panel-htop')[selectedTabIndex];
            var formID = $(selectedPanel).find('form').attr('id');
            if ($('body').hasClass('modal-open'))
                formID = $('#commonModalContent').find('form').attr('id');
            var aspxURLNew = $(tabSelectedLi).find('.data-url').text();
            var url = aspxURLNew;

            $.get(url, function (content) {
                var selectedTab = $('#tt').tabs('getSelected');
                $('#tt').tabs('update', {
                    tab: selectedTab,
                    options: {
                        content: content
                    }
                });
            }, 'html');
        }

        //if ($.isFunction(loadContent)) {
        //    delete loadContent;
        //    loadContent = undefined;
        //}

        function loadContent() {
            var data = $('#' + formID).serializeArray();
            data = data.filter(function (item) {
                return item.name.indexOf('_') === -1;
            });
            var url = aspxURL + "?" + $.param(data);
            //alert(url);

            if ($('body').hasClass('modal-open')) {
                url = aspxURL + "&" + $.param(data);
                console.log($.param(url))
            }
            $.post(url, function (content) {
                if (!$('body').hasClass('modal-open')) {
                    var selectedTab = $('#tt').tabs('getSelected');
                    $('#tt').tabs('update', {
                        tab: selectedTab,
                        options: {
                            content: content
                        }
                    });
                }
                else {
                    $('#commonModalContent').html(content);
                }
                //$('#' + formID + ' #Search').focus();
                //var tmpStr = $('#' + formID + ' #Search').val();
                //$('#' + formID + ' #Search').val('');
                //$('#' + formID + ' #Search').val(tmpStr);
            }, 'html');
            $('#continueFunction').val('1');
        }

        if ($.fn.DataTable.isDataTable('#' + formID + ' .dataTable')) {
            $('#' + formID + ' .dataTable').DataTable().destroy();
        }
        if (!$('body').hasClass('modal-open')) {
            $('#' + formID + ' .dataTable').DataTable({
                dom: "t<'row'<'col-md-4 infoDiv'><'col-md-8 text-right paginationDiv'>>",
                select: true,
                destroy: true,
                responsive: true,
                order: [],
                ordering: false,
                searching: true,
                length: false,
                autoWidth: false
            });//.rows(':not(.parent)').nodes().to$().find('td:first-child').trigger('click');
        }
        $(window).resize(function () {
            clearTimeout(window.refresh_size);
            window.refresh_size = setTimeout(function () {
                $('#' + formID + ' .dataTable').resize()
            }, 250);
        });
        var pageNo = parseInt($('#' + formID + ' #hdnCurrentPageNo').val());
        var total = parseInt($('#' + formID + ' #hdnTotalRecordsCount').val());
        var numPerPage = parseInt($('#' + formID + ' #hdnNumberPerPage').val());
        var pageNos = parseInt(Math.ceil(total / numPerPage));

        $('#' + formID + ' .infoDiv').html("Showing page " + (pageNo == NaN ? "1" : pageNo) + " of " + pageNos + " (Total records: " + total + ")");

        var ul = $('<ul class="pagination" />');
        var liFirst = $('<li />').addClass('firstPage ' + (pageNo == 1 ? 'active' : ''));
        var aFirst = $('<a />').attr('href', 'javascript:;').addClass('pageNumber ' + (pageNo == 1 ? 'active' : '')).html('First').appendTo(liFirst);
        liFirst.appendTo(ul);
        var liPrev = $('<li />').addClass('prev');
        var aPrev = $('<a />').attr('href', 'javascript:;').addClass('pageNumber').html('Prev').appendTo(liPrev);
        liPrev.appendTo(ul);
        var className = 'pageNumber pageNoDis';
        for (i = 1; i <= pageNos; i++) {
            if (i < 3) {
                if (i == pageNo)
                    className = 'pageNumber pageNoDis active';
                else
                    className = 'pageNumber pageNoDis';
                var li = $('<li />');
                if (i == pageNo)
                    li = $('<li class="active" />');
                var a = $('<a href="javascript:;" class="' + className + '" />').html(i).appendTo(li);
                li.appendTo(ul);
            }
            else if (i > pageNo - 3 && i < pageNo + 3) {
                if (i == pageNo)
                    className = 'pageNumber pageNoDis active';
                else
                    className = 'pageNumber pageNoDis';
                var li = $('<li />');
                if (i == pageNo)
                    li = $('<li class="active" />');
                var a = $('<a href="javascript:;" class="' + className + '" />').html(i).appendTo(li);
                li.appendTo(ul);
            }
            else if (i > pageNos - 2) {
                className = 'pageNumber pageNoDis';
                var li = $('<li />');
                if (i == pageNo)
                    li = $('<li class="active" />');
                var a = $('<a href="javascript:;" class="' + className + '" />').html(i).appendTo(li);
                li.appendTo(ul);
            }
            if (i == 3 && pageNo >= 6 || i == pageNos - 3 && pageNo <= pageNos - 6) {
                className = 'pageNumber pageNoDis';
                var li = $('<li />');
                if (i == pageNo)
                    li = $('<li class="active" />');
                var a = $('<a href="javascript:;" />').html('...').appendTo(li);
                li.appendTo(ul);
            }
        }
        var liNext = $('<li />').addClass('next');
        var aNext = $('<a />').attr('href', 'javascript:;').addClass('pageNumber').html('Next').appendTo(liNext);
        liNext.appendTo(ul);
        var liLast = $('<li />').addClass('lastPage ' + (pageNo == pageNos ? 'active' : ''));
        var aLast = $('<a />').attr('href', 'javascript:;').addClass('pageNumber ' + (pageNo == pageNos ? 'active' : '')).html('Last').appendTo(liLast);
        liLast.appendTo(ul);
        $('#' + formID + ' .paginationDiv').append(ul);

        $('#' + formID + ' li.firstPage').unbind('click');
        $('#' + formID + ' li.firstPage').click(function () {
            var curPageNo = parseInt(1);
            $('#' + formID + ' #hdnCurrentPageNo').val(curPageNo);
            loadContent();
        });

        $('#' + formID + ' li.lastPage').unbind('click');
        $('#' + formID + ' li.lastPage').click(function () {
            var curPageNo = parseInt($('#' + formID + ' .pageNoDis').last().text());
            $('#' + formID + ' #hdnCurrentPageNo').val(curPageNo);
            loadContent();
        });

        $('#' + formID + ' li.next').unbind('click');
        $('#' + formID + ' li.next').click(function () {
            var curPageNo = parseInt($('#' + formID + ' .pageNumber.pageNoDis.active').text());
            if (curPageNo >= parseInt($('.pageNoDis').last().text())) {

            }
            else {
                $('#' + formID + ' #hdnCurrentPageNo').val(curPageNo + 1);
                loadContent();
            }

        });

        $('#' + formID + ' li.prev').unbind('click');
        $('#' + formID + ' li.prev').click(function () {
            var curPageNo = parseInt($('#' + formID + ' .pageNumber.pageNoDis.active').text());
            if (curPageNo <= parseInt($('#' + formID + ' .pageNoDis').first().text())) {

            }
            else {
                $('#' + formID + ' #hdnCurrentPageNo').val(curPageNo - 1);
                loadContent();
            }
        });

        $('#' + formID + ' .pageNumber.pageNoDis').unbind('click');
        $('#' + formID + ' .pageNumber.pageNoDis').click(function () {
            var curPageNo = parseInt($(this).text());
            $('#' + formID + ' #hdnCurrentPageNo').val(curPageNo);
            loadContent();
        });

        $('#' + formID + ' .action-button').unbind('click');
        $('#' + formID + ' .action-button').click(function () {
            var tabSelectedLi = $('li.tabs-selected');
            var selectedTabIndex = $('#tt ul.tabs li').index(tabSelectedLi);
            var selectedPanel = $('.panel.panel-htop')[selectedTabIndex];
            var formID = $(selectedPanel).find('form').attr('id');
            var aspxURLNew = $(tabSelectedLi).find('.data-url').text();
            var checkedRowIds = $.map($('#' + formID + ' td.icheck input.checkRow:checked'), function (obj, idx) {
                return $(obj).val();
            });
            var that = $(this);
            var action = $(this).attr('data-action');
            if (action == 'Activate' || action == 'Deactivate' || action == 'Reject' || action == 'GenerateToken' || action == 'Repost') {
                var actionStatus = $(this).attr('data-status');
                var actionTitle = $(this).attr('data-original-title');
                var actionMsg = $(this).attr('data-message');

                if (checkedRowIds.length == 0) {
                    alert('Please select records.');
                    return false;
                }
                var isConfirm = $(this).attr('data-isconfirm');
                var isExcecute = 0;
                if (isConfirm == 1) {
                    if (confirm('Are you sure to ' + actionTitle + '?')) {
                        isExcecute = 1;
                    }
                    else {
                        isExcecute = 0;
                    }
                }
                else {
                    isExcecute = 1;
                }
                var rejectReason = '';
                if (isExcecute == 1) {
                    if (action == "Reject") {
                        rejectReason = prompt("Reject reason: ", "Enter reject reason");
                    }
                    var checkedRowIds = $.map($('#' + formID + ' td.icheck input.checkRow:checked'), function (obj, idx) {
                        return $(obj).val();
                    });
                    var postData = { ids: JSON.stringify(checkedRowIds), action: JSON.stringify(action), rejectReason: JSON.stringify(rejectReason) };
                    $.ajax({
                        url: aspxURLNew + "/Action",
                        contentType: 'application/json; charset=utf-8',
                        type: "GET",
                        dataType: "JSON",
                        data: postData,
                        success: function (data) {
                            if (action == 'Activate') {
                                showSnackBar(actionMsg);
                                $.each($('#' + formID + ' td.icheck input.checkRow:checked'), function (idx, obj) {
                                    $(obj).parents('td').find('.row-status').html("<span class='label label-success'>" + actionStatus + "</span>");
                                    $(obj).iCheck('uncheck');
                                });
                            }
                            else if (action == 'Deactivate') {
                                debugger;
                                if (data.d != null) {
                                    if (data.d.isSuccess) {
                                        showSnackBar(data.d.message);
                                    }
                                    else {
                                        showSnackBar(data.d.message);
                                    }
                                } else {
                                    showSnackBar(actionMsg);
                                    $.each($('#' + formID + ' td.icheck input.checkRow:checked'), function (idx, obj) {
                                        $(obj).parents('td').find('.row-status').html("<span class='label label-danger'>" + actionStatus + "</span>");
                                        $(obj).iCheck('uncheck');
                                    });
                                }
                            }
                            else if (action == 'Reject') {
                                showSnackBar(actionMsg);
                                $.each($('#' + formID + ' td.icheck input.checkRow:checked'), function (idx, obj) {
                                    $(obj).parents('td').find('.row-status').html("<span class='label label-inverse'>" + actionStatus + "</span>");
                                    $(obj).iCheck('uncheck');
                                });
                                loadContent();
                            }
                            else if (action == 'GenerateToken') {
                                showSnackBar(actionMsg);
                                setTimeout(loadContent, 100);
                            }
                            else if (action == 'Repost') {
                                showSnackBar(actionMsg);
                                setTimeout(loadContent, 100);
                            }
                        }
                    });
                }
            }

            $('#commonModal').find('.modal-footer button').removeClass('hide');
            $('#commonModalContent').html('<div class="text-center">Loading... Please wait</div>');
            if (action == 'Add') {
                $('body').attr('data-action-active', 'add');
                var title = $(this).attr('data-title');
                var url = $(this).attr('data-url');

                $('#commonModal').find('.modal-dialog').removeClass('sm');
                $('#commonModal').find('.modal-dialog').removeClass('md');
                $('#commonModal').find('.modal-dialog').removeClass('lg');
                $('#commonModal').modal('toggle');
                $('#commonModal').find('.modal-dialog').addClass('md');
                $('#commonModalTitle').html(title);

                $('#commonModalContent').load(url);
            }
            if (action == 'Edit') {
                $('body').attr('data-action-active', 'edit');
                if (checkedRowIds.length == 0 || checkedRowIds.length > 1) {
                    alert('Please select one to edit.');
                    return false;
                }
                else {
                    var title = $(this).attr('data-title');
                    var url = $(this).attr('data-url');

                    $('#commonModal').find('.modal-dialog').removeClass('sm');
                    $('#commonModal').find('.modal-dialog').removeClass('md');
                    $('#commonModal').find('.modal-dialog').removeClass('lg');
                    $('#commonModal').modal('toggle');
                    $('#commonModal').find('.modal-dialog').addClass('md');
                    $('#commonModalTitle').html(title);
                    $('#commonModalContent').load(url + "?Id=" + checkedRowIds);
                }
            }
            if (action == 'Refresh') {
                $('#' + formID + ' .action-button[data-action="Search"]').click();
                //refreshContent();
            }
            //Only for Language
            if (action == 'UpdateJSON') {
                console.log('Update JSON');
                $.ajax({
                    url: aspxURLNew + "/UpdateLanguageDirJSON",
                    contentType: 'application/json; charset=utf-8',
                    type: "GET",
                    dataType: "JSON",
                    data: {},
                    success: function (data) {
                        refreshContent();
                        showSnackBar('Successfully updated');
                    }
                });
            }

            if (action == 'Search') {
                $('#' + formID + ' #IsNewSearch').val('1');
                setTimeout(loadContent, 100);
            }

            if (action == 'View') {
                $('body').attr('data-action-active', 'view');
                var title = $(this).attr('data-title');
                var url = $(this).attr('data-url');
                var Id = $(this).attr('data-id');
                var fromdate = $(this).attr('data-fromdate');
                var todate = $(this).attr('data-todate');
                var allDataAttributes = $(this).data();
                var cache = [];
                var jsonString = JSON.stringify(allDataAttributes, function (key, value) {
                    if (typeof value === 'object' && value !== null) {
                        if (cache.indexOf(value) !== -1) {
                            // Duplicate reference found
                            try {
                                // If this value does not reference a parent it can be deduped
                                return JSON.parse(JSON.stringify(value));
                            } catch (error) {
                                // discard key if value cannot be deduped
                                return;
                            }
                        }
                        // Store value in our collection
                        cache.push(value);
                    }
                    if (key == "fromdate")
                        return (fromdate);
                    if (key == "todate")
                        return (todate);
                    else
                        return value;
                });
                cache = null; // Enable garbage collection
                //var jsonString = JSON.stringify(allDataAttributes);
                var jsonAttr = JSON.parse(jsonString);
                Object.keys(jsonAttr).forEach(function (key) {
                    if (key.indexOf('.') > -1)
                        delete jsonAttr[key];
                });
                var param = $.param(jsonAttr);
                $('#commonModal').find('.modal-dialog').removeClass('sm');
                $('#commonModal').find('.modal-dialog').removeClass('md');
                $('#commonModal').find('.modal-dialog').removeClass('lg');
                $('#commonModal').modal('toggle');
                $('#commonModal').find('.modal-dialog').addClass('lg');
                $('#commonModalTitle').html(title);
                $('#commonModalContent').load(url + "?" + param);
                $('#commonModal').find('.modal-footer button').addClass('hide');
            }

            if (action == 'SortActive') {
                $('#continueFunction').val('0');
                $('#IsNewSort').val('1');
                $('#SortBy').val('SortActive');
                if ($('#SortByAsc').val() == "1") {
                    $('#SortByAsc').val('0');
                }
                else {
                    $('#SortByAsc').val('1');
                }
                setTimeout(loadContent, 100);
            }
            if (action == 'SortBranch') {
                $('#continueFunction').val('0');
                $('#IsNewSort').val('1');
                $('#SortBy').val('SortBranch');
                if ($('#SortByAsc').val() == "1") {
                    $('#SortByAsc').val('0');
                }
                else {
                    $('#SortByAsc').val('1');
                }
                setTimeout(loadContent, 100);
            }
            if (action == 'ClearSort') {
                $('#IsNewSort').val('0');
                $('#SortBy').val('');
                $('#SortByAsc').val('1');
                setTimeout(loadContent, 100);
            }

            if (action == 'UpdateDepartments') {
                var allObjs = [];
                $('#' + formID + ' tbody tr').each(function () {
                    var BranchDepartments = {};
                    var Id = $(this).find('td.icheck input.checkRow').val();
                    BranchDepartments.Id = Id;
                    if ($(this).find('label.icheck input.checkRow.selectAll').length) {
                        var isSelectAll = $(this).find('label.icheck input.checkRow.selectAll').is(':checked');
                        var checkedDepartmentIds = $.map($(this).find('label.icheck input.checkRow:not(.selectAll):not(:checked)'), function (obj, idx) {
                            return $(obj).val();
                        });
                        BranchDepartments.DepartmentIds = checkedDepartmentIds;
                        BranchDepartments.isSelectAll = isSelectAll;
                        if (checkedDepartmentIds.length > 0) {

                        }
                        allObjs.push(BranchDepartments);
                    }
                });
                $.ajax({
                    url: aspxURLNew + "/UpdateDepartments",
                    contentType: 'application/json; charset=utf-8',
                    type: "GET",
                    dataType: "JSON",
                    data: { allObjs: JSON.stringify(allObjs) },
                    success: function (data) {
                        loadContent();
                        showSnackBar('Successfully updated');
                    }
                });
            }

        });

        $(function () {
            $('#' + formID + ' .square input').iCheck({
                checkboxClass: 'icheckbox_square',
                radioClass: 'iradio_square',
            });
            $('#' + formID + ' .flat-green input').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_flat-green'
            });
            $('#' + formID + ' .flat-blue input').iCheck({
                checkboxClass: 'icheckbox_flat-blue',
                radioClass: 'iradio_flat-blue'
            });
        });

        $('#' + formID + ' td.icheck').unbind('click');
        $('#' + formID + ' td.icheck').click(function () {
            var checkbox = $(this).find('input:checkbox');
            $(checkbox).iCheck('toggle');
        });

        $('#btnCommonModalSubmit').unbind('click');
        $('#btnCommonModalSubmit').click(function () {
            try {
                debugger;
                var tabSelectedLi = $('li.tabs-selected');
                var selectedTabIndex = $('#tt ul.tabs li').index(tabSelectedLi);
                var selectedPanel = $('.panel.panel-htop')[selectedTabIndex];
                var formID = $(selectedPanel).find('form').attr('id');
                var aspxURLNew = $(tabSelectedLi).find('.data-url').text();

                var dataActionActive = $('body').attr('data-action-active');
                //var modalForm = $('#commonModalContent').find('form');
                var modalForm = $('#commonModalContent').find('#FormId');
                var modalFormId = $(modalForm).attr('data-value');
                var data = $('[data-value=' + modalFormId + '] :input').serializeArray();
                data = data.filter(function (item) {
                    return item.name.indexOf('_') === -1;
                });
                console.log(data);
                var jsonObj = {};
                for (var key in data) {
                    jsonObj[data[key].name] = data[key].value;
                }
                var formData = {
                    obj: JSON.stringify(jsonObj)
                };
                $.ajax({
                    url: aspxURLNew + "/Add",
                    contentType: 'application/json; charset=utf-8',
                    type: "GET",
                    dataType: "JSON",
                    data: formData,
                    success: function (data) {
                        if (modalFormId == "addLanguageDirForm") {
                            $('#' + modalFormId).find('[name="TextId"]').val('0');
                        }
                        var response = data.d;
                        if (response.isSuccess != undefined) {
                            if (response.isSuccess) {
                                refreshContent();
                                var checkbox = $('#' + formID).find('table tbody tr td.icheck .checkRow');
                                $(checkbox).iCheck('uncheck');
                                $('#' + modalFormId).find('input, select, textarea').val('');
                                if ($('#' + modalFormId).find('[name="ID"]').length > 0)
                                    $('#' + modalFormId).find('[name="ID"]').val('0');
                                if ($('#' + modalFormId).find('[name="Status"]').length > 0)
                                    $('#' + modalFormId).find('[name="Status"]').val('1');
                                $('#commonModal').modal('toggle');
                                $('body').removeAttr('data-action-active');
                            }
                            else {
                            }
                            showSnackBar(response.message);
                        }
                        else {
                            if (response) {
                                var checkbox = $('#' + formID).find('table tbody tr td.icheck .checkRow');
                                $(checkbox).iCheck('uncheck');
                                $('#' + modalFormId).find('input, select, textarea').val('');
                                if ($('#' + modalFormId).find('[name="ID"]').length > 0)
                                    $('#' + modalFormId).find('[name="ID"]').val('0');
                                if ($('#' + modalFormId).find('[name="Status"]').length > 0)
                                    $('#' + modalFormId).find('[name="Status"]').val('1');
                                $('#commonModal').modal('toggle');
                                $('body').removeAttr('data-action-active');
                                refreshContent();
                                if (dataActionActive = 'add')
                                    showSnackBar('Successfully added');
                                if (dataActionActive = 'edit')
                                    showSnackBar('Successfully updated');
                            }
                            else {

                            }
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        var response = jqXHR.responseJSON;
                        //alert(response.Message);
                        console.log(response.StackTrace);
                        showSnackBar('Please enter all required fields');
                    }
                });
            }
            catch(err) {
                alert(err);
            }
            return false;
        });

        $('#' + formID + ' .popovers').popover();
        $('#' + formID + ' #IsNewSearch').val('0');

        $('#' + formID).unbind('keydown');
        $('#' + formID).keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                $('#' + formID + ' .action-button[data-action="Search"]').click();
                return false;
            }
        });

        $('#' + formID + ' .search-features select').unbind('change');
        $('#' + formID + ' .search-features select').change(function () {
            $('#' + formID + ' .action-button[data-action="Search"]').click();
        });

        $('#' + formID + ' #btnFormSubmit').unbind('click');
        $('#' + formID + ' #btnFormSubmit').click(function () {
            var tabSelectedLi = $('li.tabs-selected');
            var selectedTabIndex = $('#tt ul.tabs li').index(tabSelectedLi);
            var selectedPanel = $('.panel.panel-htop')[selectedTabIndex];
            var formID = $(selectedPanel).find('form').attr('id');
            var aspxURLNew = $(tabSelectedLi).find('.data-url').text();

            var data = $('#' + formID).serializeArray();
            data = data.filter(function (item) {
                return item.name.indexOf('_') === -1;
            });
            console.log(data);
        });

        var selectedLanguage = Cookies.get("selectedLanguage");
        if (initLanguage == 'en' && selectedLanguage != 'en') {
            $('.loadingDiv').removeClass('hide');
            TranslateLanguage(selectedLanguage);
        }

    }, 100);
});
