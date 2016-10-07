$(document).ready(function () {

    // COUNT TOTAL PARTICIPANTS
    paticipantCount();


    // PARTICIPANT ADDING AND REMOVING
    $(".js-add-sessionParticipant").click(function (e) {
        var link = $(e.target);
        var personId = $("#participantDropdown").val();

        $.ajax({
            url: hr_urlHrKompetensUtveckling + "/Activity/AddPersonToSessionFromActivitySummary/",
            type: "POST",
            data: { sessionId: link.attr("data-sessionId"), personId: personId },
            success: function (result) {
                if (result.Success) {
                    var html = '<tr><td><a href="' + hr_urlHrKompetensUtveckling + '"/Participant/Details/' +
                        result.PersonId + '">' + result.PersonFullName +
                        '</a><span> </span><span class="label label-warning listedParticipantRemove js-delete-sessionParticipant" data-sessionid="' +
                        result.SessionId + '" data-personId="' + result.PersonId + '">Ta bort</span></tr></td>';
                    $(html).hide().appendTo("#ParticipantsForSession").fadeIn(100);

                    paticipantCount();
                }

                else {
                    hr_messageFadingOut(link, "Personen redan tillagd!", "danger");
                }
            }
        });
    });

    $("body").on("click", ".js-delete-sessionParticipant", function (e) {
        var link = $(e.target);


        bootbox.confirm({
            title: "Vänligen bekräfta",
            message: "Vill du verkligen ta bort denna person för detta tillfälle?",
            buttons: {
                cancel: {
                    label: '<i class="glyphicon glyphicon-remove"></i> Avbryt'
                },
                confirm: {
                    label: '<i class="glyphicon glyphicon-ok"></i> Ta bort',
                    className: "btn-danger"
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        url: hr_urlHrKompetensUtveckling + "/Activity/RemovePersonFromSession/",
                        type: "POST",
                        data: { sessionId: link.attr("data-sessionId"), personId: link.attr("data-personId") },
                        success: function () {
                            link.parents("tr")
                                .fadeOut(function () {
                                    $(this).remove();

                                    paticipantCount();
                                });
                        },
                        error: function () {
                            alert("Anropet misslyckades, prova gärna igen.");
                        }
                    });
                }
            }
        });

    });


    // SAVE SESSION COMMENTS AND EVALUATION
    $("body").on("click", ".js-save-sessionComment", function (e) {
        var link = $(e.target);
        var comments = $("#Comments").val();

        $.ajax({
            url: hr_urlHrKompetensUtveckling + "/ActivitySummary/SaveSessionComments/",
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


    $("body").on("click", ".js-save-sessionEvaluation", function (e) {
        var link = $(e.target);
        var evaluation = $("#Evaluation").val();

        $.ajax({
            url: hr_urlHrKompetensUtveckling + "/ActivitySummary/SaveSessionEvaluation/",
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
    $("body").on("click", ".js-delete-session", function (e) {
        var link = $(e.target);

        bootbox.confirm({
            title: "Vänligen bekräfta",
            message: "Vill du verkligen ta bort hela detta tillfälle?",
            buttons: {
                cancel: {
                    label: '<i class="glyphicon glyphicon-remove"></i> Avbryt'
                },
                confirm: {
                    label: '<i class="glyphicon glyphicon-ok"></i> Ta bort',
                    className: "btn-danger"
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        url: hr_urlHrKompetensUtveckling + "/Activity/RemoveSession/" + link.attr("data-sessionId"),
                        type: "POST",
                        success: function () {
                            alert("Kurstillfället är borttaget.");
                            window.location.href = hr_urlHrKompetensUtveckling + "/Activity/Index/";
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