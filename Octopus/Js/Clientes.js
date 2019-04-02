
//var condicion = sessionStorage.getItem("CONDICIONIVA");

$(document).ready(function (e) {
   
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

        if (ddlValue == "CONSUMIDOR_FINAL") {
            // show time div, hide fromTo div
            $('#PersonalesL').show();
            $('#PersonalesR').show();
            $('#EmpresaLU').hide();
            $('#EmpresaLD').hide();
            $('#EmpresaRU').hide();
            $('#EmpresaRD').hide();
            $('#Buttons').show();
        }
        else if (ddlValue == "RESPONSABLE_INSCRIPTO") {
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

    if (condicion == "CONSUMIDOR_FINAL")
    {
        $('#PersonalesL').show();
        $('#PersonalesR').show();
        $('#EmpresaLU').hide();
        $('#EmpresaLD').hide();
        $('#EmpresaRU').hide();
        $('#EmpresaRD').hide();
        $('#Buttons').show();
    }
    else if (condicion == "RESPONSABLE_INSCRIPTO")
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



