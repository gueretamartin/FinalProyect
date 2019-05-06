$(document).ready(function () {

    $('#RES_CLI_CF_ID').hide();
    $('#RES_CLI_RI_ID').hide();
});

$(document).ready(function () {

    $('#ResOptionIVA').change(function () {
        var ddlValue = $(this).val();

        if (ddlValue == "1") {
            // show time div, hide fromTo div
            $('#RES_CLI_CF_ID').show();
            $('#RES_CLI_RI_ID').hide();
        }
        else if (ddlValue == "2") {
            // show fromTo div, hide time div
            $('#RES_CLI_CF_ID').hide();
            $('#RES_CLI_RI_ID').show();
        }
        else {
            $('#RES_CLI_RI_ID').hide();
            $('#RES_CLI_CF_ID').hide();
        }
    });
});