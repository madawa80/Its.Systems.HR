//SessionForActivity.js
$(document)
    .ready(function() {

        // COUNT TOTAL PARTICIPANTS
        paticipantCount();
        // INIT AUTOCOMPLETE
        hr_createAutocomplete();
        // HANDLE ENTER-BUTTON WHEN ADDING PARTICIPANTS
        hr_addEventListenerForEnter(".js-add-sessionParticipant", "#nameOfParticipant");


        // PARTICIPANT ADDING AND REMOVING
        $(".js-add-sessionParticipant")
            .click(function() {
                var link = $(this);
                var personName = $("#nameOfParticipant").val();

                $.ajax({
                    url: hr_urlPrefix + "/Session/AddPersonToSessionFromActivitySummary/",
                    type: "POST",
                    data: { sessionId: link.attr("data-sessionId"), personName: personName },
                    success: function(result) {
                        if (result.Success) {
                            var html = '<tr><td><a href="' +
                                hr_urlPrefix +
                                "/Participant/Details/" +
                                result.PersonId +
                                '">' +
                                result.PersonFullName +
                                '</a><span> </span><span class="label label-warning listedParticipantRemove js-delete-sessionParticipant" data-sessionid="' +
                                result.SessionId +
                                '" data-personId="' +
                                result.PersonId +
                                '">Ta bort</span></tr></td>';
                            $(html).hide().appendTo("#ParticipantsForSession").fadeIn(hr_fadeInSpeed);

                            paticipantCount();
                            $("#nameOfParticipant").val("");
                        } else {
                            hr_messageFadingOut(link, "Personen redan tillagd!", "danger");
                        }
                    }
                });
            });

        $("body")
            .on("click",
                ".js-delete-sessionParticipant",
                function() {
                    var link = $(this);


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
                        callback: function(result) {
                            if (result) {
                                $.ajax({
                                    url: hr_urlPrefix + "/Session/RemovePersonFromSession/",
                                    type: "POST",
                                    data: {
                                        sessionId: link.attr("data-sessionId"),
                                        personId: link.attr("data-personId")
                                    },
                                    success: function() {
                                        link.parents("tr")
                                            .fadeOut(hr_fadeOutSpeed,
                                                function() {
                                                    $(this).remove();

                                                    paticipantCount();
                                                });
                                    },
                                    error: function() {
                                        alert("Anropet misslyckades, prova gärna igen.");
                                    }
                                });
                            }
                        }
                    });

                });


        // SAVE SESSION COMMENTS AND EVALUATION
        $("body")
            .on("click",
                ".js-save-sessionComment",
                function() {
                    var link = $(this);
                    var comments = $("#Comments").val();

                    $.ajax({
                        url: hr_urlPrefix + "/ActivitySummary/SaveSessionComments/",
                        type: "POST",
                        data: { sessionId: link.attr("data-sessionId"), comments: comments },
                        success: function() {
                            hr_messageFadingOut(link, "Sparat!", "success");
                        },
                        error: function() {
                            alert("Anropet misslyckades, prova gärna igen.");
                        }
                    });
                });


        $("body")
            .on("click",
                ".js-save-sessionEvaluation",
                function() {
                    var link = $(this);
                    var evaluation = $("#Evaluation").val();

                    $.ajax({
                        url: hr_urlPrefix + "/ActivitySummary/SaveSessionEvaluation/",
                        type: "POST",
                        data: { sessionId: link.attr("data-sessionId"), evaluation: evaluation },
                        success: function() {
                            hr_messageFadingOut(link, "Sparat!", "success");
                        },
                        error: function() {
                            alert("Anropet misslyckades, prova gärna igen.");
                        }
                    });
                });


        // DELETE THE WHOLE SESSION
        $("body")
            .on("click",
                ".js-delete-session",
                function() {
                    var link = $(this);

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
                        callback: function(result) {
                            if (result) {
                                $.ajax({
                                    url: hr_urlPrefix + "/Session/RemoveSession/" + link.attr("data-sessionId"),
                                    type: "POST",
                                    success: function() {
                                        alert("Tillfället är borttaget.");
                                        window.location.href = hr_urlPrefix + "/Activity/Index/";
                                    },
                                    error: function() {
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