//date time picker config
$(function () {
    $('#dtBegin').datepicker({
        startView: 0,
        minViewMode: 0,
        format: "dd/mm/yyyy",
        clearBtn: true,
        language: "vi",

    });
    $('#dtEnd').datepicker({
        startView: 0,
        minViewMode: 0,
        format: "dd/mm/yyyy",
        clearBtn: true,
        language: "vi",
    });

    $('#addModal .input-group.date').datepicker({
        startView: 0,
        minViewMode: 0,
        format: "dd/mm/yyyy",
        clearBtn: true,
        language: "vi",
    });
});
