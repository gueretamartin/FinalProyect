$(document).ready(function () {
    $('.help').on('click', function () {
        swal({
            title: "Este será el Menú de Ayuda",
            text: "Cada vez que veas un ícono de Ayuda puedes pulsar sobre el para obtener información adicional!",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: '#5bc0de',
            cancelButtonColor: '#8CEC43',
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Más Ayuda",
            cancelButtonText: "Entendido!",
            closeOnConfirm: false,
            closeOnCancel: true
        }
     ,function (isConfirm) {
         if (isConfirm) {
              // El método que te lleva a la info extra del sistema.
             //swal("Deleted!", "Your imaginary file has been deleted.", "success");
             window.open ( '/Manual/OpenManual');
         }
     });
});
});

$(document).ready(function () {
    $('.help-search').on('click', function () {
        swal({
            title: "Botón de Búsqueda",
            text: "Pulse sobre la lupa y escribe un texto o palabra para buscar en la tabla. Luego presiona la tecla Enter",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: '#5bc0de',
            cancelButtonColor: '#8CEC43',
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Más Ayuda",
            cancelButtonText: "Entendido!",
            closeOnConfirm: false,
            closeOnCancel: true
        }
     , function (isConfirm) {
         if (isConfirm) {
             // El método que te lleva a la info extra del sistema.
             //swal("Deleted!", "Your imaginary file has been deleted.", "success");
             window.open('/Manual/OpenManual');
         }
     });
    });
});

$(document).ready(function () {
    $('.help-detail').on('click', function () {
        swal({
            title: "Botón de Detalle",
            text: "Pulse sobre los 3 puntos (...) para ir al detalle del registro",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: '#5bc0de',
            cancelButtonColor: '#8CEC43',
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Más Ayuda",
            cancelButtonText: "Entendido!",
            closeOnConfirm: false,
            closeOnCancel: true
        }
     , function (isConfirm) {
         if (isConfirm) {
             // El método que te lleva a la info extra del sistema.
             //swal("Deleted!", "Your imaginary file has been deleted.", "success");
             window.open('/Manual/OpenManual');
         }
     });
    });
});