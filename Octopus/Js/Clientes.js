
//var condicion = sessionStorage.getItem("CONDICIONIVA");

$(document).ready(function (e) {
    var condicion = $('#myHiddenVar').val();
    $('#RESPONSABLEINSCRIPTOL').hide();
    $('#RESPONSABLEINSCRIPTOR').hide();
    $('#CONSUMIDORFINALCUILLT').hide();
    $('#CONSUMIDORFINALCUILRT').hide();
    $('#CONSUMIDORFINALCUILL').hide();
    $('#CONSUMIDORFINALCUILR').hide();
    $('#CONSUMIDORFINALL').hide();
    $('#CONSUMIDORFINALR').hide();
    $('#BUTTONS').hide();

    if (condicion == 1) {
        $('#RESPONSABLEINSCRIPTOL').hide();
        $('#RESPONSABLEINSCRIPTOR').hide();
        $('#CONSUMIDORFINALCUILLT').show();
        $('#CONSUMIDORFINALCUILRT').show();
        $('#CONSUMIDORFINALCUILL').show();
        $('#CONSUMIDORFINALCUILR').show();
        $('#CONSUMIDORFINALL').show();
        $('#CONSUMIDORFINALR').show();
        $('#BUTTONS').show();
    }

    if (condicion == 2) {
        $('#RESPONSABLEINSCRIPTOL').show();
        $('#RESPONSABLEINSCRIPTOR').show();
        $('#CONSUMIDORFINALCUILLT').show();
        $('#CONSUMIDORFINALCUILRT').show();
        $('#CONSUMIDORFINALCIULL').hide();
        $('#CONSUMIDORFINALCUILR').hide();
        $('#CONSUMIDORFINALL').show();
        $('#CONSUMIDORFINALR').show();
        $('#BUTTONS').show();
    }

    $('#OptionIVA').change(function () {
        var ddlValue = $(this).val();

        if (ddlValue == 1) {
            // show time div, hide fromTo div
            $('#RESPONSABLEINSCRIPTOL').hide();
            $('#RESPONSABLEINSCRIPTOR').hide();
            $('#CONSUMIDORFINALCUILLT').show();
            $('#CONSUMIDORFINALCUILRT').show();
            $('#CONSUMIDORFINALCUILL').show();
            $('#CONSUMIDORFINALCUILR').show();
            $('#CONSUMIDORFINALL').show();
            $('#CONSUMIDORFINALR').show();
            $('#BUTTONS').show();
        }
        else if (ddlValue == 2) {
            // show fromTo div, hide time div
            $('#RESPONSABLEINSCRIPTOL').show();
            $('#RESPONSABLEINSCRIPTOR').show();
            $('#CONSUMIDORFINALCUILLT').show();
            $('#CONSUMIDORFINALCUILRT').show();
            $('#CONSUMIDORFINALCUILL').hide();
            $('#CONSUMIDORFINALCUILR').hide();
            $('#CONSUMIDORFINALL').show();
            $('#CONSUMIDORFINALR').show();
            $('#BUTTONS').show();
        }
        else {
            $('#RESPONSABLEINSCRIPTOL').hide();
            $('#RESPONSABLEINSCRIPTOR').hide();
            $('#CONSUMIDORFINALCUILLT').hide();
            $('#CONSUMIDORFINALCUILRT').hide();
            $('#CONSUMIDORFINALCUILL').hide();
            $('#CONSUMIDORFINALCUILR').hide();
            $('#CONSUMIDORFINALL').hide();
            $('#CONSUMIDORFINALR').hide();
            $('#BUTTONS').hide();
        }
    });
});
$(window).on("load", function () {

    //var condicion = sessionStorage.getItem("CONDICIONIVA");
    //var condicion = '@Session["CONDICIONIVA"]';
    var condicion = $('#myHiddenVar').val();

    if (condicion == 1) {
        $('#RESPONSABLEINSCRIPTOL').hide();
        $('#RESPONSABLEINSCRIPTOR').hide();
        $('#CONSUMIDORFINALCUILLT').show();
        $('#CONSUMIDORFINALCUILRT').show();
        $('#CONSUMIDORFINALCUILL').show();
        $('#CONSUMIDORFINALCUILR').show();
        $('#CONSUMIDORFINALL').show();
        $('#CONSUMIDORFINALR').show();
        $('#BUTTONS').show();
    }
    else if (condicion == 2) {
        $('#RESPONSABLEINSCRIPTOL').show();
        $('#RESPONSABLEINSCRIPTOR').show();
        $('#CONSUMIDORFINALCUILLT').show();
        $('#CONSUMIDORFINALCUILRT').show();
        $('#CONSUMIDORFINALCUILL').hide();
        $('#CONSUMIDORFINALCUILR').hide();
        $('#CONSUMIDORFINALL').show();
        $('#CONSUMIDORFINALR').show();
        $('#BUTTONS').show();
    }
    else {
        $('#RESPONSABLEINSCRIPTOL').hide();
        $('#RESPONSABLEINSCRIPTOR').hide();
        $('#CONSUMIDORFINALCUILLT').hide();
        $('#CONSUMIDORFINALCUILRT').hide();
        $('#CONSUMIDORFINALCUILL').hide();
        $('#CONSUMIDORFINALCUILR').hide();
        $('#CONSUMIDORFINALL').hide();
        $('#CONSUMIDORFINALR').hide();
        $('#BUTTONS').hide();
    }
}
);



