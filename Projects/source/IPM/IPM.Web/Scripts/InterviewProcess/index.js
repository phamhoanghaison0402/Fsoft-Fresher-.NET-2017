function search() {
    //Get search value
    var processName = document.getElementById('ProcessName').value;
    var postitionId = $("#selectPosition :selected").val();
    //Create search object
    var process = {
        ProcessName: processName,
        PositionID: postitionId
    };
    //Send search object to controller and get response
    $.ajax({
        type: "POST",
        url: "/InterviewProcess/Index",
        data: {
            "process": process
        },
        success: function (data) {
            $('body').html(data);
        },
        error: function () {
            location.reload(true);
        }
    });
}

