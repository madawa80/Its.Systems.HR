$(document).ready(function () {

    // INIT BOOTSTRAP 3 DATEPICKERS
    hr_initBootstrap3DatePickers();
    // INIT AUTOCOMPLETE
    hr_createAutocomplete();
    // HANDLE ENTER-BUTTON WHEN ADDING TAGS
    hr_addEventListenerForEnter(".js-add-tag-edit-session");


    // TAGS
    $(".js-add-tag-edit-session").on("click", function () {
        var link = $(this);
        var addedTag = $("#tagsInput").val();

        if (addedTag.length < 1) {
            hr_messageFadingOut(link, "Kan ej vara tom!", "danger");
            return;
        }

        $.ajax({
            url: hr_urlPrefix + "/Session/AddTagToSession",
            type: "POST",
            data: { sessionId: link.attr("data-sessionId"), tagName: addedTag },
            success: function (data) {
                if (data.Success) {
                    var html = '<span data-tagId="' +
                        data.TagId +
                        '" data-sessionId="' + link.attr("data-sessionId") + '" class="label label-primary js-remove-tag-edit-session">' +
                        addedTag +
                        '&nbsp;<span class="glyphicon glyphicon-remove"></span></span>';
                    $(html).hide().appendTo("#selectedTags").fadeIn(hr_fadeInSpeed);

                    $("#tagsInput").val("");
                } else {
                    hr_messageFadingOut(link, "Redan tillagd!", "danger");
                }
            },
            error: function () {
                alert("Anropet misslyckades, prova gärna igen.");
            }
        });

    });

    $("body").on("click", ".js-remove-tag-edit-session", function () {
        var link = $(this);

        $.ajax({
            url: hr_urlPrefix + "/Session/RemoveTagFromSession",
            type: "POST",
            data: { sessionId: link.attr("data-sessionId"), tagId: link.attr("data-tagId") },
            success: function () {
                hr_fadeOutObject(link);
            },
            error: function () {
                alert("Anropet misslyckades, prova gärna igen.");
            }
        });

    });

});