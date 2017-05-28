//Initialize function
init();
function init() {
    ShowData(); 
    LoadRoundInTable(); //load list round in process

    $(document).ready(function () {
        $('.detailRoundInProcess').click(function () {
            var url = "/InterviewProcess/RoundDetail/"; // the url to the controller
            var id = $(this).attr('data-id'); // the id that's given to each button in the list

            $.ajax({
                type: "GET",
                url: url + id,
                success: function (data) {
                    $('#interviewRoundDetailInProcessContainer').html(data);
                    $('#interviewRoundDetailInProcessModal').modal('show');
                },
                error: function () {
                    location.reload(false);
                }
            });
        });
    });
}


// show data to select position and check box active
function ShowData() {
    document.getElementById("Status").checked = interviewProcess.Active;
}

//Load list round in process
function LoadRoundInTable() {
    if (listRoundIn != undefined) {
        $.each(listRoundIn, function (i, el) {
            var data = this;
            //show list round
            $('#interviewRoundTable').append(createAppendString(data));
        });
    }
}