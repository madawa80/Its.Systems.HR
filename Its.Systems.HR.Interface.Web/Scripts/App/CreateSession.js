//CreateSession.js
$(document).ready(function () {

    // INIT BOOTSTRAP 3 DATEPICKERS
    hr_initBootstrap3DatePickers();
    // INIT AUTOCOMPLETE
    hr_createAutocomplete();
    // HANDLE ENTER-BUTTON WHEN ADDING TAGS
    hr_addEventListenerForEnter(".js-add-tag-create-session", "#tagsInput");


    // ADDING PARTICIPANTS "ON-THE-FLY"
    // Array holding participants
    var listOfParticiantsThatParticipated = [];

    $(".js-add-participantBeforeSessionExists").on("click", function () {
        var link = $(this);
        var resultName = $("#Participant_Id :selected").text();
        var resultId = $("#Participant_Id").val();

        if ($.inArray(resultId, listOfParticiantsThatParticipated) > -1) {
            hr_messageFadingOut(link, "Redan tillagd!", "danger");
            return;
        }

        listOfParticiantsThatParticipated.push(resultId);
        $("#AddedParticipants").val(listOfParticiantsThatParticipated);

        addParticipantLi(resultId, resultName);
    });

    $("body").on("click", ".js-remove-participantBeforeSessionExists", function () {
        var link = $(this);
        var resultId = link.parents("li").attr("data-personId");

        var index = listOfParticiantsThatParticipated.indexOf(resultId);
        if (index > -1) {
            listOfParticiantsThatParticipated.splice(index, 1);
            $("#AddedParticipants").val(listOfParticiantsThatParticipated);
            
            hr_fadeOutObject(link.parents("li"));
        }
    });

    $("body").on("click", ".listedParticipantLink", function () {
        var link = $(this);
        var resultId = link.parents("li").attr("data-personId");

        var url = hr_urlPrefix + "/Participant/Details/" + resultId;
        window.open(url, "_blank");
    });

    function addParticipantLi(resultId, resultName) {
        var html = '<li data-personId="' +
        resultId +
        '"><span class="listedParticipantLink">' +
        resultName +
        '</span><span> </span><span class="badge js-remove-participantBeforeSessionExists listedParticipantRemove"> x </span></li>';
        $(html).hide().appendTo("#selectedParticipants").fadeIn(hr_fadeInSpeed);
    }

    // ADDING TAGS
    // Array holding tags
    var listOfAddedTags = [];

    $(".js-add-tag-create-session").on("click", function () {
        var link = $(this);
        var addedTag = $("#tagsInput").val();

        if ($.inArray(addedTag, listOfAddedTags) > -1) {
            hr_messageFadingOut(link, "Redan tillagd!", "danger");
            return;
        }
        if (addedTag.length < 1) {
            hr_messageFadingOut(link, "Kan ej vara tom!", "danger");
            return;
        }

        listOfAddedTags.push(addedTag);
        $("#AddedTags").val(listOfAddedTags);

        addTagSpan(addedTag);

        $("#tagsInput").val("");
    });

    $("body").on("click", ".js-remove-tag-create-session", function () {
        var link = $(this);
        var resultTagName = link.attr("data-tagName");

        var index = listOfAddedTags.indexOf(resultTagName);
        if (index > -1) {
            listOfAddedTags.splice(index, 1);
            $("#AddedTags").val(listOfAddedTags);

            hr_fadeOutObject(link);
        }
    });

    function addTagSpan(addedTag) {
        var html = '<span data-tagName="' +
            addedTag +
            '" class="label label-primary js-remove-tag-create-session">' +
            addedTag +
            '&nbsp;<span class="glyphicon glyphicon-remove"></span></span>';
        $(html).hide().appendTo("#selectedTags").fadeIn(hr_fadeInSpeed);
    }

});