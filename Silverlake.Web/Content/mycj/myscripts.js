function myFunction() {
    // Get the snackbar DIV
    var x = $("#snackbar");
    // Add the "show" class to DIV
    $(x).addClass('show');
    $(x).html('This is sample notification :)');
    // After 3 seconds, remove the show class from DIV
    setTimeout(function () { $(x).removeClass('show') }, 3000);
}

$(document).ready(function () {
    $(document).on("mousedown", ".tabs li", function (e) {
        switch (e.which) {
            case 1:
                //left Click
                break;
            case 2:
                $(this).find('.tabs-close').click();// Middle click
                break;
            case 3:
                //right Click
                break;
        }
        return true;// to allow the browser to know that we handled it. 
    });
    setTimeout(function () {
        $('[data-title="Dashboard"]').click();
    }, 100);
});