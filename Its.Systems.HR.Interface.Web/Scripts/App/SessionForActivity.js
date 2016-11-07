$(document).ready(function () {

    // COUNT TOTAL PARTICIPANTS
    paticipantCount();
    // INIT AUTOCOMPLETE
    hr_createAutocomplete();
    // HANDLE ENTER-BUTTON WHEN ADDING PARTICIPANTS
    hr_addEventListenerForEnter(".js-add-sessionParticipant", "#nameOfParticipant");
    // ADD CLICK EVENT HANDLER FOR delete-sessionParticipant
    $(".js-delete-sessionParticipant").click(deleteSessionParticipant);



    // PARTICIPANT ADDING AND REMOVING
    $(".js-add-sessionParticipant").click(function () {
        var link = $(this);
        var personName = $("#nameOfParticipant").val();

        $.ajax({
            url: hr_urlPrefix + "/Session/AddPersonToSessionFromActivitySummary/",
            type: "POST",
            data: { sessionId: link.attr("data-sessionId"), personName: personName },
            success: function (result) {
                if (result.Success) {

                    // TODO: STRATEGI FÖR ATT BYGGA HTML MED JQUERY INTERFACET
                    // BÖRJA INIFRÅN och ut
                    // bryt ut a-taggen
                    // bryt ut button-taggen
                    // wrappa i varsin td
                    // wrappa i TR

                    var $newA = $("<a>")
                                    .attr("href", hr_urlPrefix + "/Participant/Details/" + result.PersonId)
                                    .text(result.PersonFullName);

                    var $newButton = $("<button>")
                                        .attr("type", "button")
                                        .addClass("btn btn-warning btn-xs js-delete-sessionParticipant")
                                        .attr("data-sessionid", result.SessionId)
                                        .attr("data-personid", result.PersonId)
                                        .text("Ta bort");

                    var $html = $("<tr>").append($("<td>").append($newA)).append($("<td>").append($newButton));
                    
                    $($html).hide().appendTo("#ParticipantsForSession").fadeIn(hr_fadeInSpeed);
                    $newButton.click(deleteSessionParticipant);

                    paticipantCount();
                    $("#nameOfParticipant").val("");
                }

                else {
                    hr_messageFadingOut(link, result.ErrorMessage, "danger");
                }
            }
        });
    });

    function deleteSessionParticipant() {
        var link = $(this);
        
        bootbox.confirm({
            title: "Vänligen bekräfta",
            message: "Vill du verkligen ta bort denna person för detta tillfälle?",
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
                        success: function () {
                            link.parents("tr")
                                .fadeOut(hr_fadeOutSpeed, function () {
                                    $(this).remove();

                                    paticipantCount();
                                });
                        },
                        error: function () {
                            alert(link.attr("data-sessionId"));
                            alert("Anropet misslyckades, prova gärna igen.");
                        }
                    });
                }
            }
        });
    }


    // SAVE SESSION COMMENTS AND EVALUATION
    $(".js-save-sessionComment").click(function () {
        var link = $(this);
        var comments = $("#Comments").val();

        $.ajax({
            url: hr_urlPrefix + "/ActivitySummary/SaveSessionComments/",
            type: "POST",
            data: { sessionId: link.attr("data-sessionId"), comments: comments },
            success: function () {
                hr_messageFadingOut(link, "Sparat!", "success");
            },
            error: function () {
                alert("Anropet misslyckades, prova gärna igen.");
            }
        });
    });
    
    $(".js-save-sessionEvaluation").click(function () {
        var link = $(this);
        var evaluation = $("#Evaluation").val();

        $.ajax({
            url: hr_urlPrefix + "/ActivitySummary/SaveSessionEvaluation/",
            type: "POST",
            data: { sessionId: link.attr("data-sessionId"), evaluation: evaluation },
            success: function () {
                hr_messageFadingOut(link, "Sparat!", "success");
            },
            error: function () {
                alert("Anropet misslyckades, prova gärna igen.");
            }
        });
    });
    
    // DELETE THE WHOLE SESSION
    $(".js-delete-session").click(function () {
        var link = $(this);

        bootbox.confirm({
            title: "Vänligen bekräfta",
            message: "Vill du verkligen ta bort hela detta tillfälle?",
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
                        url: hr_urlPrefix + "/Session/RemoveSession/" + link.attr("data-sessionId"),
                        type: "POST",
                        success: function () {
                            alert("Tillfället är borttaget.");
                            window.location.href = hr_urlPrefix + "/Activity/Index/";
                        },
                        error: function () {
                            alert("Anropet misslyckades, prova gärna igen.");
                        }
                    });
                }
            }
        });

    });

    function paticipantCount() {
        var rowCount = $("#ParticipantsForSession").find("tr").length;
        $("#totDeltagare").html(rowCount);
    }

});