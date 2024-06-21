$(document).ready(function () {

    $('input[title]').tooltip({ 'placement': 'left' });
    $('select[title]').tooltip({ 'placement': 'left' });

    if ($("body").hasClass("layout-top-nav")) {
        $("#main-container").addClass("container");
        $("#buttons-container").addClass("container");
        $("#services-container").addClass("container");
        $("#rate-container").addClass("container");
        $("#footer-container").addClass("container");
    }
});

function loadMap(latitud, longitud) {
    
}