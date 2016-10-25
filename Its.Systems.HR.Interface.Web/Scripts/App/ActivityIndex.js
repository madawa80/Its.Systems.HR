//ActivityIndex.js
$(document)
    .ready(function() {

        // INIT TABLESORTER
        $("#listActivitiesTable")
            .tablesorter(
            {
                sortList: [[0, 0]],
                headers: {
                    1: {
                        sorter: false
                    }
                }
            });


        // DELETE A ACTIVITY, WARNING: CASCADE DELETES SESSIONS AND SESSIONPARTICIPANTS FOR THAT ACTIVITY
        $("body")
            .on("click",
                ".js-delete-activity",
                function(e) {
                    e.preventDefault();
                    var link = $(this);

                    bootbox.confirm({
                        title: "Vänligen bekräfta",
                        message: "Vill du verkligen ta bort den här aktiviteten och alla dess tillfällen?",
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
                                    url: hr_urlPrefix + "/Activity/DeleteActivity/",
                                    type: "POST",
                                    data: { activityId: link.attr("data-activityId") },
                                    success: function() {
                                        hr_fadeOutObject(link.parents("tr"));
                                    },
                                    error: function() {
                                        alert("Anropet misslyckades, prova gärna igen.");
                                    }
                                });
                            }
                        }
                    });

                });

    });