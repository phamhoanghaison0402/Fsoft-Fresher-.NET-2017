function PopupDetail(id, name, active) {
    $('#detailGuidelineName').html(name);
    if (active == "True") {
        $('#detailGuidelineStatus').html("Active");
    } else {
        $('#detailGuidelineStatus').html("Inactive");
    }

    $('#DetailModal').modal('show');

}

function PopupUpdate(id, Name) {
    $('#EditModal input#editGuidelineName').val(Name);
    $('#EditModal input#editGuidelineID').val(id);

    $('#EditModal').modal('show');
}

$('#btneditguideline').click(function (e) {
    var guidelineName = $('#editGuidelineName').val();
    var guidelineid = $('#editGuidelineID').val();
    var object = {
        ID: guidelineid,
        Name: guidelineName
    }
    console.log(guidelineid, guidelineName);
    // write ajax

    $.ajax({
        url: "Guideline/Edit",
        type: "POST",
        data: {
            Guideline: object,
            List: [1, 2, 3]

        },
        success: function (result) {
            window.location = '/Guideline';
        },
        error: function (result) {
            console.log("Name bi trong");
            location.reload();
        }

    })
});

function PopupCreate() {
    $('#AddModal').modal('show');
}
$('#btnaddguideline').click(function (e) {

    var guidelineName = $('#addGuidelineName').val();

    console.log(guidelineName);
    //write  content object
    $.ajax({
        url: "/Guideline/Create",
        type: "POST",
        data: { Name: guidelineName },
        success: function (result) {
            window.location = '/guideline';
        },
        error: function (result) {
            console.log("Name bi trong");
            location.reload();
        }
    });
});