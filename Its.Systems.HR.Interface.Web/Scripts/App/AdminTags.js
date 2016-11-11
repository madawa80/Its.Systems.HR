$(document).ready(function () {

    //INIT TABLE SORTER
    $("#listTagsTable").tablesorter(
    {
        sortList: [[0, 0]],
        headers: {
            2: {
                sorter: false
            }
        }
    });

    // ADD CLICK EVENT HANDLERS
    $(".js-delete-tag").click(deleteTagCallback);

    // DELETE A TAG
    function deleteTagCallback(e) {
        e.preventDefault();
        var link = $(this);

        bootbox.confirm({
            title: "Vänligen bekräfta",
            message: "Vill du verkligen ta bort denna etikett från databasen?",
            buttons: {
                cancel: {
                    label: "<i class=\"glyphicon glyphicon-remove\"></i> Avbryt"
                },
                confirm: {
                    label: "<i class=\"glyphicon glyphicon-ok\"></i> Ta bort",
                    className: "btn-danger"
                }
            },
            callback: deleteTagConfirmedCallback(link)
        });
    }

    function deleteTagConfirmedCallback(link) {
        return function wrapped(result) {
            if (result) {
                $.ajax({
                    url: hr_urlPrefix + "/Admin/DeleteTag/",
                    type: "POST",
                    data: { tagId: link.attr("data-tagId") },
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