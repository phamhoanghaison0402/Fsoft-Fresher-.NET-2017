$(document).ready(function () {
    $('.deleteRound').click(function () {
        var url = "/InterviewRound/Delete";
        var id = $(this).attr('data-id');
        $.get(url + '/' + id, function (data) {
            $('#interviewRoundDeleteContainer').html(data);
            $('#interviewRoundDeleteModal').modal('show');
        });
    });
});