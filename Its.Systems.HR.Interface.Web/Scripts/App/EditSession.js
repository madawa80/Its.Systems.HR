$(document).ready(function () {

    // INIT BOOTSTRAP 3 DATEPICKERS
    hr_initBootstrap3DatePickers();
    // INIT AUTOCOMPLETE
    hr_createAutocomplete();
    // HANDLE ENTER-BUTTON WHEN ADDING TAGS
    hr_addEventListenerForEnter(".js-add-tag-edit-session", "#tagsInput");
    // INIT .js-remove-tag-edit-session
    $(".js-remove-tag-edit-session").click(removeTag);
    // INIT CHECKBOX FOR EXPRESSIONOFINTEREST
    hr_initCheckboxForExpressionOfInterest("#IsOpenForExpressionOfInterest");

    // TAGS
    $(".js-add-tag-edit-session").on("click", function() {
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
                    success: function(data) {
                        if (data.Success) {

                            var $tagSpan = $("<span>")
                                .attr("data-tagId", data.TagId)
                                .attr("data-sessionId", link.attr("data-sessionId"))
                                .addClass("label label-primary js-remove-tag-edit-session")
                                .text(addedTag);

                            var $removeGlyph = $("<span>")
                                .addClass("glyphicon glyphicon-remove");

                            var $html = $tagSpan.append($removeGlyph);

                            $($html).hide().appendTo("#selectedTags").fadeIn(hr_fadeInSpeed);
                            $html.click(removeTag);

                            $("#tagsInput").val("");
                        } else {
                            hr_messageFadingOut(link, "Redan tillagd!", "danger");
                        }
                    },
                    error: function() {
                        alert("Anropet misslyckades, prova gärna igen.");
                    }
                });

    });

    function removeTag() {
                var link = $(this);

                $.ajax({
                    url: hr_urlPrefix + "/Session/RemoveTagFromSession",
                    type: "POST",
                    data: { sessionId: link.attr("data-sessionId"), tagId: link.attr("data-tagId") },
                    success: function() {
                        hr_fadeOutObject(link);
                    },
                    error: function() {
                        alert("Anropet misslyckades, prova gärna igen.");
                    }
                });

    }

});