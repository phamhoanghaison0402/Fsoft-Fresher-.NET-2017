$(document).ready(function () {
    $('.detailRound').click(function () {
        var url = "/InterviewRound/Detail";
        var id = $(this).attr('data-id');
        $.get(url + '/' + id, function (data) {
            $('#interviewRoundDetailContainer').html(data);
            $('#interviewRoundDetailModal').modal('show');
        });
    });
});