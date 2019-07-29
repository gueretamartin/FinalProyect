$(document).ready(function () {

    $('#VentaL4').hide();
    $('#VentaR4').hide();
    $('#VEN_CLI_CF_ID').hide();
    $('#VEN_CLI_RI_ID').hide();
});

$(document).ready(function () {

    $('#VenOptionIVA').change(function () {
        var ddlValue = $(this).val();

        if (ddlValue == "1") {
            // show time div, hide fromTo div
            $('#VEN_CLI_CF_ID').show();
            $('#VEN_CLI_RI_ID').hide();
        }
        else if (ddlValue == "2") {
            // show fromTo div, hide time div
            $('#VEN_CLI_CF_ID').hide();
            $('#VEN_CLI_RI_ID').show();
        }
        else {
            $('#VEN_CLI_RI_ID').hide();
            $('#VEN_CLI_CF_ID').hide();
        }
    });

});

function EntUsado(element) {
    if (element.checked) {
        $('#VentaL4').show()
        $('#VentaR4').show()
    }
    else {
        $('#VentaL4').hide()
        $('#VentaR4').hide()
    }
}