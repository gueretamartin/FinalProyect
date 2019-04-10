
//var condicion = sessionStorage.getItem("CONDICIONIVA");

$(document).ready(function () {

    $('#PersonalesL').hide();
    $('#PersonalesR').hide();
    $('#EmpresaLU').hide();
    $('#EmpresaLD').hide();
    $('#EmpresaRU').hide();
    $('#EmpresaRD').hide();
    $('#Buttons').hide();

    function ShowCF() {
        $('#PersonalesL').show();
        $('#PersonalesR').show();
        $('#EmpresaLU').hide();
        $('#EmpresaLD').hide();
        $('#EmpresaRU').hide();
        $('#EmpresaRD').hide();
        $('#Buttons').show();
    }

    function ShowRI() {
        $('#PersonalesL').hide();
        $('#PersonalesR').hide();
        $('#EmpresaLU').show();
        $('#EmpresaLD').show();
        $('#EmpresaRU').show();
        $('#EmpresaRD').show();
        $('#Buttons').show();
    }

    $('#OptionIVA').change(function () {
        var ddlValue = $(this).val();

        if (ddlValue == 1) {
            // show time div, hide fromTo div
            $('#PersonalesL').show();
            $('#PersonalesR').show();
            $('#EmpresaLU').hide();
            $('#EmpresaLD').hide();
            $('#EmpresaRU').hide();
            $('#EmpresaRD').hide();
            $('#Buttons').show();
        }
        else if (ddlValue == 2) {
            // show fromTo div, hide time div
            $('#PersonalesL').hide();
            $('#PersonalesR').hide();
            $('#EmpresaLU').show();
            $('#EmpresaLD').show();
            $('#EmpresaRU').show();
            $('#EmpresaRD').show();
            $('#Buttons').show();
        }
        else {
            $('#PersonalesL').hide();
            $('#PersonalesR').hide();
            $('#EmpresaLU').hide();
            $('#EmpresaLD').hide();
            $('#EmpresaRU').hide();
            $('#EmpresaRD').hide();
            $('#Buttons').hide();
        }
    });
});

$(window).load(function () {
    
    //var condicion = sessionStorage.getItem("CONDICIONIVA");
    //var condicion = '@Session["CONDICIONIVA"]';
    var condicion = $('#myHiddenVar').val();

    if (condicion == 1)
    {
        $('#PersonalesL').show();
        $('#PersonalesR').show();
        $('#EmpresaLU').hide();
        $('#EmpresaLD').hide();
        $('#EmpresaRU').hide();
        $('#EmpresaRD').hide();
        $('#Buttons').show();
    }
    else if (condicion == 2)
    {
        $('#PersonalesL').hide();
        $('#PersonalesR').hide();
        $('#EmpresaLU').show();
        $('#EmpresaLD').show();
        $('#EmpresaRU').show();
        $('#EmpresaRD').show();
        $('#Buttons').show();
    }
    else 
    {
        $('#PersonalesL').hide();
        $('#PersonalesR').hide();
        $('#EmpresaLU').hide();
        $('#EmpresaLD').hide();
        $('#EmpresaRU').hide();
        $('#EmpresaRD').hide();
        $('#Buttons').hide();
    }

}
);



