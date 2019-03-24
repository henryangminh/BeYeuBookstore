// responsive for device has screen width less than 768px
function myFunction(x) {
    var hd = document.getElementById("pgHeader");
    fx = hd.children[1];
    if (x.matches) { // If media query matches
        fx.classList.remove("form-inline");
        fx.classList.remove("pull-right");
        hd.children[0].classList.remove("pull-left");
    } else {
        fx.classList.add("form-inline");
        fx.classList.add("pull-right");
        hd.children[0].classList.add("pull-left");
    }
}
var x = window.matchMedia("(max-width: 768px)")
myFunction(x) // Call listener function at run time
x.addListener(myFunction) // Attach listener function on state changes
// end