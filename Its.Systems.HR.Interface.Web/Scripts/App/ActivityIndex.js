$(document).ready(function () {

    // INIT TABLESORTER
    $("#listActivitiesTable").tablesorter(
    {
        sortList: [[0, 0]],
        headers: {
            2: {
                sorter: false
            }
        }
    });

    // DELETE A ACTIVITY, WARNING: CASCADE DELETES SESSIONS AND SESSIONPARTICIPANTS FOR THAT ACTIVITY
    $(".js-delete-activity").click(deleteActivityCallback);


    function deleteActivityCallback(e) {
        e.preventDefault();
        var link = $(this);

        bootbox.confirm({
            title: "Vänligen bekräfta",
            message: "Vill du verkligen ta bort den här aktiviteten och alla dess tillfällen?",
            buttons: {
                cancel: {
                    label: "<i class=\"glyphicon glyphicon-remove\"></i> Avbryt"
                },
                confirm: {
                    label: "<i class=\"glyphicon glyphicon-ok\"></i> Ta bort",
                    className: "btn-danger"
                }
            },
            callback: deleteActivityConfirmedCallback(link) // TODO: Läs på closures för att förstå hur denna funkar
        });
    }

    function deleteActivityConfirmedCallback(link) {
        return function wrapped(result) {
            if (result) {
                $.ajax({
                    url: hr_urlPrefix + "/Activity/DeleteActivity/",
                    type: "POST",
                    data: { activityId: link.attr("data-activityId") },
                    success: function () {
                        hr_fadeOutObject(link.parents("tr"));
                    },
                    error: function () {
                        alert("Anropet misslyckades, prova gärna igen.");
                    }
                });
            }
        }
    }
    


});