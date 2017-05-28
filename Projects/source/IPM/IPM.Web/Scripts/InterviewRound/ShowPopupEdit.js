$(document).ready(function () {
    $('.editRound').click(function () {
        var url = "/InterviewRound/Edit";
        var id = $(this).attr('data-id');
        $.get(url + '/' + id, function (data) {
            $('#interviewRoundEditContainer').html(data);
            $('#interviewRoundEditModal').modal('show');
        });
    });
});