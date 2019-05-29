$(document).ready(function () {

    $('#VEH_CLI_RI_ID').hide();
    $('#VEH_CLI_CF_ID').hide();
});

$(document).ready(function () {

    $('#OptionIVA').change(function () {
        var ddlValue = $(this).val();

        if (ddlValue == "1") {
            // show time div, hide fromTo div
            $('#VEH_CLI_CF_ID').show();
            $('#VEH_CLI_RI_ID').hide();
        }
        else if (ddlValue == "2") {
            // show fromTo div, hide time div
            $('#VEH_CLI_CF_ID').hide();
            $('#VEH_CLI_RI_ID').show();
        }
        else {
            $('#VEH_CLI_RI_ID').hide();
            $('#VEH_CLI_CF_ID').hide();
        }
    });
});
