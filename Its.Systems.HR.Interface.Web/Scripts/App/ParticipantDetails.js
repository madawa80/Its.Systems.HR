$(document).ready(function () {

    // FADEOUT ERROR MESSAGE IF EXISTING
    if ($("#errorMessage").length) {
        $("#errorMessage").fadeOut(hr_messageFadingOutSpeed);
    }

    // ADD CLICK EVENT HANDLERS
    $(".js-delete-sessionParticipant").click(deleteSessionParticipant);
    $(".js-remove-expressionOfInterest").click(removeExpressionOfInterest);

    // REMOVE EXPRESSION OF INTEREST
    function removeExpressionOfInterest() {
        var link = $(this);

        bootbox.confirm({
            title: "Vänligen bekräfta",
            message: "Vill du verkligen ta bort intresseanmälan för detta tillfälle?",
            buttons: {
                cancel: {
                    label: "<i class=\"glyphicon glyphicon-remove\"></i> Avbryt"
                },
                confirm: {
                    label: "<i class=\"glyphicon glyphicon-ok\"></i> Ta bort",
                    className: "btn-danger"
                }
            },
            callback: function(result) {
                if (result) {
                    alert("TODO...");
                    //$.ajax({
                    //    url: hr_urlPrefix + "/Session/RemoveExpressionOfInterest/",
                    //    type: "POST",
                    //    data: { sessionId: link.attr("data-sessionId"), personId: link.attr("data-personId") },
                    //    success: handleRemoveExpressionOfInterestResult,
                    //    error: function () {
                    //        alert("Anropet misslyckades, prova gärna igen.");
                    //    }
                    //});
                }
            }
        });
    }

    // DELETE SESSION PARTICIPANT
    function deleteSessionParticipant() {
        var link = $(this);

        bootbox.confirm({
            title: "Vänligen bekräfta",
            message: "Vill du verkligen ta bort medverkan på detta tillfälle?",
            buttons: {
                cancel: {
                    label: "<i class=\"glyphicon glyphicon-remove\"></i> Avbryt"
                },
                confirm: {
                    label: "<i class=\"glyphicon glyphicon-ok\"></i> Ta bort",
                    className: "btn-danger"
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        url: hr_urlPrefix + "/Session/RemovePersonFromSession/",
                        type: "POST",
                        data: { sessionId: link.attr("data-sessionId"), personId: link.attr("data-personId") },
                        success: handleRemoveResult,
                        error: function () {
                            alert("Anropet misslyckades, prova gärna igen.");
                        }
                    });
                }
            }
        });

        function handleRemoveResult() {
            link.parents("tr")
                .fadeOut(hr_fadeOutSpeed, function () {
                    $(this).remove();

                    $.ajax({
                        url: hr_urlPrefix + "/Participant/ParticipantStatisticSummary/",
                        type: "GET",
                        data: { personId: link.attr("data-personId") },
                        success: function (result2) {
                            $("#statisticsSummary").html(result2);
                        },
                        error: function () {
                            alert("Anropet misslyckades, prova gärna igen.");
                        }
                    });
                });
        }

    }

    // ADD SESSION PARTICIPANT
    // Handled by a form postback to the controller

    // SAVE COMMENTS FOR PARTICIPANT
    $(".js-save-participantComment").click(function () {
        var link = $(this);
        var comments = $("#Comments").val();

        $.ajax({
            url: hr_urlPrefix + "/Participant/SaveComments/",
            type: "POST",
            data: { personId: link.attr("data-personId"), comments: comments },
            success: function () {
                hr_messageFadingOut(link, "Sparat!", "success");
            },
            error: function () {
                alert("Anropet misslyckades, prova gärna igen.");
            }
        });
    });

    // SAVE WISHES FOR PARTICIPANT
    $(".js-save-participantWish").click(function () {
        var link = $(this);
        var wishes = $("#Wishes").val();

        $.ajax({
            url: hr_urlPrefix + "/Participant/SaveWishes/",
            type: "POST",
            data: { personId: link.attr("data-personId"), wishes: wishes },
            success: function () {
                hr_messageFadingOut(link, "Sparat!", "success");
            },
            error: function () {
                alert("Anropet misslyckades, prova gärna igen.");
            }
        });
    });

});