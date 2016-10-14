//HrHelpers.js
// Global namespace...

// JAVASCRIPT TODOS:
// TODO: Proper error handling



// CHECKS URL TO ADD PROPER URL-PREFIX
// TODO: WATCH OUT FOR THIS ONE!!!
if (window.location.href.indexOf("localhost") > -1) {
    hr_urlPrefix = "";
} else {
    hr_urlPrefix = "/HrKompetensutveckling";
}

// FADEIN/OUT-SPEEDS, NOTE: hr_messageFadingOut speed hardcoded in this file.
hr_fadeInSpeed = 100;
hr_fadeOutSpeed = 100;

// INIT BOOTSTRAP 3 DATEPICKERS (ADDON)
function hr_initBootstrap3DatePickers() {
    $("#datetimepicker1").datetimepicker({
        locale: "sv"
    });
    $("#datetimepicker2").datetimepicker({
        locale: "sv"
    });
};

// MESSAGE TO THE USER IN ADDED IN A SPAN AFTER <source> THEN FADES OUT
function hr_messageFadingOut(source, message, type) {
    source.after(' <span class="alert alert-' + type + ' js-fadeOutThisMessage" role="alert">' + message + '</span>');
    $(".js-fadeOutThisMessage").fadeOut(4000, function () {
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


//function createAutocompletes() {
//    //var $input = $(this); // the HTML element (Textbox)

//    var myTextField = document.getElementById("#nameOfParticipant");

//    //var options = {

//    //    // selecting the source by finding elements with the 'data-' attribute
//    //    source: $input.attr("data-autocomplete")
       
//    //};

//    //// apply options
//    //$input.autocomplete(options);
//    myTextField.autocomplete();
//}

// HANDLE ENTER-BUTTON WHEN ADDING TAGS
function hr_addEventListenerForEnter(selector) {
    $(document).on("keypress", "#tagsInput", function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            $(selector).trigger("click");
        }
    });
}