// Global namespace...


// CHECKS URL TO ADD PROPER URL-PREFIX
// TODO: WATCH OUT FOR THIS ONE!!!
if (window.location.href.indexOf("localhost") > -1) {
    hr_urlPrefix = "";
} else {
    hr_urlPrefix = "/HrKompetensutveckling";
}

// FADEIN/OUT-SPEEDS
hr_fadeInSpeed = 100;
hr_fadeOutSpeed = 100;
hr_messageFadingOutSpeed = 4000;

// INIT BOOTSTRAP 3 DATEPICKERS (ADDON)
function hr_initBootstrap3DatePickers() {
    $("#datetimepicker1").datetimepicker(
    {
        locale: "sv",
        format: "YYYY-MM-DD" //REMOVE IF YOU WANT TIME-PICKER AS WELL
    });
    $("#datetimepicker2").datetimepicker({
        locale: "sv",
        format: "YYYY-MM-DD", //REMOVE IF YOU WANT TIME-PICKER AS WELL
        useCurrent: false //Important! See issue #1075
    });
    $("#datetimepicker1").on("dp.change", function (e) {
        $("#datetimepicker2").data("DateTimePicker").minDate(e.date);
    });
    $("#datetimepicker2").on("dp.change", function (e) {
        $("#datetimepicker1").data("DateTimePicker").maxDate(e.date);
    });
};

// MESSAGE TO THE USER IN ADDED IN A SPAN AFTER <source> THEN FADES OUT
function hr_messageFadingOut(source, message, type) {
    var span = $("<span />")
                .addClass("alert")
                .addClass("alert-" + type)
                .attr("role", "alert")
                .html(message);

    source.after(span);

    span.fadeOut(hr_messageFadingOutSpeed, function () {
        $(this).remove();
    });
};

// FADE OUT AN OBJECT
function hr_fadeOutObject(source) {
    source.fadeOut(hr_fadeOutSpeed, function () {
        $(this).remove();
    });
}


// AUTOCOMPLETE
// Targets input elements with the 'data-' attributes and each time the input changes
// it calls the 'createAutocomplete' function
function hr_createAutocomplete() {
    $("input[data-autocomplete]").each(createAutocompletes);
};
function createAutocompletes() {
    var $input = $(this); // the HTML element (Textbox)

    var options = {
        // selecting the source by finding elements with the 'data-' attribute
        source: $input.attr("data-autocomplete") // Required
    };

    // apply options
    $input.autocomplete(options);
}

// Init the TableSorter plugin with initial sorting on the first column.
function hr_createTableSorter(tableId) {
    $(tableId).tablesorter(
    {
        sortList: [[0, 0]],
        emptyTo: "bottom"
    });
}

// HANDLE ENTER-BUTTON WHEN ADDING TAGS
function hr_addEventListenerForEnter(selector, inputField) {
    $(document).keypress(inputField, function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $(selector).trigger("click");
        }
    });
}

function hr_initCheckboxForExpressionOfInterest(selector) {

    var $checkbox = $(selector);

    updateSpanFromCheckboxValue($checkbox);

    $($checkbox).change(function () {
        updateSpanFromCheckboxValue($checkbox);
    });
}
function updateSpanFromCheckboxValue($checkbox) {

    if ($checkbox.is(":checked")) {
        $checkbox.siblings("label").html("Öppen");
    } else {
        $checkbox.siblings("label").html("Stängd");
    }
}