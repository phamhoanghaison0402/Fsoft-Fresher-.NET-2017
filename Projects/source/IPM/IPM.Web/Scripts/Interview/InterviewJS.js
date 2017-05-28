$(document).ready(function () {
    /* fields use for handling showing record timer*/
    var myVar, seconds = 00, minutes = 00, minuteDisplay, secondDisplay;

    $('#finish').hide();

    /* get data of candidate in table */
    $("tr.item").click(function () {
        var tableData = $(this).children("td").map(function () {
            return $(this).text();
        }).get();
        document.getElementById("fullname").innerHTML = $.trim(tableData[1]);
        document.getElementById("position").innerHTML = $.trim(tableData[2]);
        document.getElementById("round").innerHTML = $.trim(tableData[3]);
    });
    /* editable field : CandidateAnswer, Comment, InterviewAnswer's Mark*/
    $('#guidelinebody').editable({
        container: 'body',
        selector: 'td.candidateanswer',
        title: 'Candidate Answer',
        type: 'POST',
        url: '/Interview/UpdateAnswer'
    });

    $('#guidelinebody').editable({
        container: 'body',
        selector: 'td.note',
        title: 'Note',
        type: 'POST',
        url: '/Interview/UpdateAnswer'
    });

    $('#guidelinebody').editable({
        container: 'body',
        selector: 'td.point',
        title: 'Mark',
        type: 'POST',
        source: function listPoint() {
            var i = 1;
            var list = [];
            /* Get list point form 1 to max point for selected Interview Answer */
            while (i <= $(this).attr("data-maxpoint")) {
                list.push({ value: i, text: i });
                i++;
            }
            return list;
        },
        url: '/Interview/UpdateCatalogMark',
        validate: function (value) {
            if(value > $(this).attr("data-maxpoint"))
            {
                return "Error";
            }
        }
    });

    /* Submit Interview Result */
    $('#finish').click(function () {
        submitResult($(this).attr("data-pk"));
    });

    /* Update Interview Result */
    $('#updateinfo').click(function () {
        submitResult($(this).attr("data-pk"));
    });

    /* Show popup add Question to Catalog */
    $('.addQuestion').click(function () {
        PopupCreate($(this).attr("data-pk"));
    });

    /* Close popup add Question to Catalog */
    $('#closeModal').click(function () {
        PopupClose();
    });

    /* Start record timer */
    $('#startrecord').click(function () {
        myVar = setInterval(myTimer, 1000);
        myTimer();
        $('#finish').show();
        $(this).hide();
    });

    /* Stop record timer */
    $('#fin').click(function () {
        clearInterval(myVar);
    });

    /* Auto select first tab in Interview screen*/
    $('#tab-content .tab-pane').eq(0).addClass('active');

    /* Add question to selected Interview Answer in Guideline*/
    $('#btnAddQuestion').click(function () {
        var question = $("#selectQuestion :selected").val();
        $.ajax({
            type: "POST",
            url: "/Interview/GetQuestion",
            data: { id : question, pk: $(this).attr("data-pk") },
            success: function (data) {
                if (data != "")
                {
                    var appendString = "<tr><td>" + data.content + "</td><td>"
                    + data.answer + "</td><td data-name='CandidateAnswer' data-type='text' data-pk='"
                    + data.id + "' class='candidateanswer'></td><td></td>"
                    + "<td data-name='Comment' data-type='text' data-pk= '" + data.id + "' class='note'></td></tr>";
                    $("tr[id=" + data.pk + "]").after(appendString);
                    var increaseRow = parseInt($("td[id=" + data.pk + "][class='catalogrank']").attr("rowspan")) + 1;
                    $("td[id=" + data.pk + "][class='catalogrank']").attr("rowspan", increaseRow);
                    PopupClose();

                }
                else
                {
                    $("#comboboxQuestion").after("<div class='col-sm-offset-2'><label class='control-label' style='color:red;'>The value of question is not valid or not exist in database</label></div>");
                }
            },
        });
    });

    function PopupCreate(id) {
        listQuestion(id);
        $('#AddQuestionModal').modal('show');
        $('#btnAddQuestion').attr("data-pk", id);
    }

    function PopupClose() {
        $('#AddQuestionModal').modal('hide');
    }

    function submitResult(id) {
        var str = 'input[name="radio ' + id + '"]:checked';
        var result = $(str).val();
        $.ajax({
            type: "POST",
            url: "/Interview/SetResult",
            data: {
                pk: id, value: result
            },
        });
    }

    function myTimer() {
        seconds += 1;
        if (seconds == 60) {
            seconds = 00;
            minutes += 1;
        }
        if (seconds < 10) secondDisplay = "0" + seconds;
        else secondDisplay = seconds;
        if (minutes < 10) minuteDisplay = "0" + minutes;
        else minuteDisplay = minutes;
        //document.getElementById("timerecording").innerHTML = minuteDisplay + ":" + secondDisplay;
    }

    function listQuestion(id) {
        $.ajax({
            type: "POST",
            url: "/Interview/GetListQuestion",
            data: { id: id },
            success: function (data) {
                $('#selectQuestion').html("");
                $('.callout-danger').remove();
                $.each(data.list, function (index, item) {
                    $('#selectQuestion').append("<option value='" + item.ID + "'>" + item.Content + "</option>");
                });
                $.each(data.existList, function (index, item) {
                    $("option[value=" + item + "]").remove();
                });
            },
        });
    }

});