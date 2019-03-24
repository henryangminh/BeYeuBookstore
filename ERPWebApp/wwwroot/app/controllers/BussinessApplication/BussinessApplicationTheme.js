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

// responsive for device has screen width less than 768px
function myFunction(x) {
    var fx = document.getElementById("frmSearch");
    if (x.matches) { // If media query matches
        fx.classList.remove("form-inline");
        fx.classList.remove("pull-left");
    } else {
        fx.classList.add("form-inline");
        fx.classList.add("pull-left");
    }
}
var x = window.matchMedia("(max-width: 768px)")
myFunction(x) // Call listener function at run time
x.addListener(myFunction) // Attach listener function on state changes
// end