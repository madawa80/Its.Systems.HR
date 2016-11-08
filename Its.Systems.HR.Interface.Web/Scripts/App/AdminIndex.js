$(document).ready(function () {

    //INIT TABLE SORTER
    $("#listParticipantsTable").tablesorter(
    {
        sortList: [[0, 0]]
    });


    $(".js-update-isHrPerson").change(function () {
            var link = $(this);

            
            var result = this.checked;
            updateHr(link, result);

    });

    function updateHr(link, result) {

    
        $.ajax({
            url: hr_urlPrefix + "/Admin/UpdatePersonalHrStatus",
            type: "POST",
            dataType: "json",
            data: { ParticipantId: link.attr("data-ParticipantId"), isChecked: result },
            success: function () {
                alert("framgångsrikt ändrade status");
            },
            error: function () {
                alert("Anropet misslyckades, prova gärna igen.");
            }

        });
    }


    $(".js-update-isDeleted").change(function () {
        var link = $(this);

        var result = this.checked;
        deletePerson(link, result);

    });

    function deletePerson(link, result) {

        $.ajax({
            url: "/Admin/ChangePersonalDeletedStatus",
            type: "POST",
            dataType: "json",
            data: { ParticipantId: link.attr("data-ParticipantId"), isChecked: result },
            success: function () {
                alert("framgångsrikt ändrade status");
            },
            error: function () {
                alert("Anropet misslyckades, prova gärna igen.");
            }

        });
    }


});