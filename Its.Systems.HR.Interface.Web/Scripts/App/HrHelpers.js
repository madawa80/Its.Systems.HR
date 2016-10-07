// Global namespace...


// INIT BOOTSTRAP 3 DATEPICKERS (ADDON)
function hr_initBootstrap3DatePickers() {
    $("#datetimepicker1").datetimepicker({
        locale: "sv"
    });
    $("#datetimepicker2").datetimepicker({
        locale: "sv"
    });
};

// Message to the user in a span
function hr_messageFadingOut(source, message, type) {
    source.after(' <span class="alert alert-' + type + ' js-fadeOutThisMessage" role="alert">' + message + '</span>');
    $(".js-fadeOutThisMessage").fadeOut(4000, function () {
        $(this).remove();
    });
};

// Fade out an object
function hr_fadeOutObject(source) {
    source.fadeOut(100, function () {
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


// HANDLE ENTER-BUTTON WHEN ADDING TAGS
function hr_addEventListenerForEnter(selector) {
    $(document).on("keypress", "#tagsInput", function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $(selector).trigger("click");
        }
    });
}