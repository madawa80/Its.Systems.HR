$(document)
    .ready(function () {

        // COUNT TOTAL PARTICIPANTS, is done with a partial now...
        //paticipantCount();
        // HANDLE ENTER-BUTTON WHEN ADDING PARTICIPANTS
        hr_addEventListenerForEnter(".js-add-sessionParticipant", "#nameOfParticipant");
        // ADD CLICK EVENT HANDLER FOR delete-sessionParticipant
        $(".js-delete-sessionParticipant").click(deleteSessionParticipant);

        //TODO: Test the new AddParticipantToSession-system rigorously
        $("#nameOfParticipant").autocomplete({
            source: hr_urlPrefix + "/Activity/AutoCompleteLocationsParticipants/",
            minLength: 2,
            select: function (event, ui) {
                $("#selectedParticipantId").val(ui.item.id);
            }
        });


        // PARTICIPANT ADDING AND REMOVING
        $(".js-add-sessionParticipant").click(function () {
            var link = $(this);
            var participantId = $("#selectedParticipantId").val(); //TODO: participantId from hidden field

            $.ajax({
                url: hr_urlPrefix + "/Session/AddPersonToSessionFromActivitySummary/",
                type: "POST",
                data: { sessionId: link.attr("data-sessionId"), participantId: participantId },
                success: function (result) {
                    if (result.Success) {

                        var $newA = $("<a>")
                                        .attr("href", hr_urlPrefix + "/Participant/Details/" + result.PersonId)
                                        .attr("target", "_blank")
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

                        //paticipantCount();
                        $("#sessionStatisticCount")
                            .load(hr_urlPrefix + "/ActivitySummary/SessionStatisticCount/" + link.attr("data-sessionId"));

                        $("#nameOfParticipant").val("");
                        $("#selectedParticipantId").val("");
                    }

                    else {
                        $("#selectedParticipantId").val("");
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

                                        //paticipantCount();
                                        $("#sessionStatisticCount")
                                            .load(hr_urlPrefix + "/ActivitySummary/SessionStatisticCount/" + link.attr("data-sessionId"));
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
                                window.location.href = hr_urlPrefix + "/Activity/ActivityIndex/";
                            },
                            error: function () {
                                alert("Anropet misslyckades, prova gärna igen.");
                            }
                        });
                    }
                }
            });

        });

        //function paticipantCount() {
        //    var rowCount = $("#ParticipantsForSession").find("tr").length;
        //    $("#totDeltagare").html(rowCount);
        //}

    });