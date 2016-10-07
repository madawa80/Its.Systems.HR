$(document).ready(function () {

    // DELETE SESSION PARTICIPANT
    $("body").on("click", ".js-delete-sessionParticipant", function (e) {
        var link = $(e.target);

        bootbox.confirm({
            title: "Vänligen bekräfta",
            message: "Vill du verkligen ta bort detta tillfälle för personen?",
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
                        url: hr_urlPrefix + "/Activity/RemovePersonFromSession/",
                        type: "POST",
                        data: { sessionId: link.attr("data-sessionId"), personId: link.attr("data-personId") },
                        success: function () {
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
                        },
                        error: function () {
                            alert("Anropet misslyckades, prova gärna igen.");
                        }
                    });
                }
            }
        });

    });

    // ADD SESSION PARTICIPANT
    $(".js-add-sessionParticipant").click(function (e) {
        var link = $(e.target);
        var sessionId = $("#Id").val();

        $.ajax({
            url: hr_urlPrefix + "/Activity/AddPersonToSession",
            type: "POST",
            data: { sessionId: sessionId, personId: link.attr("data-personId") },
            success: function (result) {
                if (result.Success) {
                    var html = '<tr><td><a href="' + hr_urlPrefix + '/ActivitySummary/GetParticipants/' + sessionId + '">' + result.SessionName + '</a><span> (' + result.StartDate + ') </span><span class="label label-warning listedParticipantRemove js-delete-sessionParticipant" data-sessionId="' + result.SessionId + '" data-personId="' + result.PersonId + '">Ta bort</span></tr></td>';
                    $(html).hide().appendTo("#allSessionsForParticipant").fadeIn(hr_fadeInSpeed);

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

                } else {
                    hr_messageFadingOut(link, "Tillfället redan tillagt!", "danger");
                }
            }
        });
    });

    // SAVE COMMENTS FOR PARTICIPANT
    $(".js-save-participantComment").click(function (e) {
        var link = $(e.target);
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
    $(".js-save-participantWish").click(function (e) {
        var link = $(e.target);
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