$(document).ready(function () {

    // INIT AUTOCOMPLETE
    hr_createAutocomplete();

    // INIT BOOTSTRAPSLIDER
    $("#yearSlider").bootstrapSlider({
        tooltip: "always"
    });

    // INIT TABLESORTER
    hr_createTableSorter("#listSessionsPartialTable");
    
});