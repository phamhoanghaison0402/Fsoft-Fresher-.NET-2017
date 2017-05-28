const MAXLENGTHPROCESSNAME = 100;
const MAXLENGTHPROCESSSTARTDATE = 10;
//Initialize function
init();
function init() {
    LoadRoundInTable();
}


//get event press enter
$(document).keypress(function (e) {
    if (e.which == 13) {
        addProcess();
    }
});


//Load list round to combobox
function LoadRoundtoCombox() {
    $.each(listAddRound, function (i, el) {
        var data = this;
        $('#selectedRound').append('<option id="' + data.Value + '" value="' + data.Value + '">' + data.Text + '</option>');
    });
}


//Load list round in process
function LoadRoundInTable() {
    if (listRoundIn != undefined) {
        $.each(listRoundIn, function (i, el) {
            var data = this;
            //show list round
            $('#interviewRoundTable').append(createAppendString(data));

            //delete row was inserted in combobox
            $.each(listAddRound, function (i, el) {
                var round = this;
                if (round.Value == data.ID) {
                    listAddRound.splice(i, 1);
                }
            });
        });
    }
    LoadRoundtoCombox();
}


//add round to process
function addRoundTemp() {
    //hidden list round in process validate message
    hiddenProcessListRoundValidateMessage();
    //Get selected round
    var round = $("#selectedRound :selected").val();
    //Get info round
    $.ajax({
        type: "GET",
        url: "/InterviewProcess/GetDetailRound",
        data: { "roundId": round },
        dataType: "JSON",
        success: function (data) {
            //insert row into table
            listRoundIn.push(data);
            $('#interviewRoundTable').append(createAppendString(data));

            //delete option in combobox
            $('#selectedRound').find('option').each(function () {
                var idr = this.id;
                if (idr == round) {
                    $(this).remove();
                }
            });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr);
        }
    });
}


//user click delete button in round table
function Delete(id, roundName) {
    //delete in list
    $.each(listRoundIn, function (i, el) {
        if (this.ID == id) {
            listRoundIn.splice(i, 1);
        }
    });
    //delete in table
    $('#interviewRoundTable').closest('table').find('tr')
            .each(function () {
                var idr = this.id;
                if (idr == id) {
                    $(this).remove();
                }
            });
    //add option to combobox
    $('#selectedRound').append('<option id="' + id + '" value="' + id + '">' + roundName + '</option>');
}


//add process
function addProcess() {
    //get create process data
    var processName = document.getElementById('ProcessName').value;
    var status = document.getElementById('Status').checked;
    var postitionId = $("#selectPosition :selected").val();
    var startDate = document.getElementById('datepicker').value;

    //validate create process data
    var commit = true;
    //validate for processName
    if (processName == null || processName == '') {
        $('#ProcessNameFormCreate').addClass("has-error");
        $('#blockProcessNameCreate').removeClass("hidden");
        commit = false;
    } else {
        if (processName.length > MAXLENGTHPROCESSNAME) {
            $('#ProcessNameFormCreate').addClass("has-error");
            $('#blockProcessNameTooLongCreate').removeClass("hidden");
        } else {
            hiddenProcessNameValidateMessage();
        }
    }
    //validate for startDate
    if (startDate == null || startDate == '') {
        $('#StartDateFormCreate').addClass("has-error");
        $('#blockStartDateCreate').removeClass("hidden");
        commit = false;
    } else {
        if (processName.length > MAXLENGTHPROCESSSTARTDATE) {
            $('#ProcessNameFormCreate').addClass("has-error");
            $('#blockProcessStartDateTooLongCreate').removeClass("hidden");
        }
        hiddenProcessStartDateValidateMessage();
    }
    //validate for list round in process
    if (listRoundIn.length == 0) {
        $('#listRoundFormCreate').addClass("has-error");
        $('#blockListRoundCreate').removeClass("hidden");
        commit = false;
    } else {
        hiddenProcessListRoundValidateMessage();
    }
    //if data is valid, send to controller
    if (commit == true) {
        var process = {
            ProcessName: processName,
            PositionID: postitionId,
            Active: status,
            StartDate: startDate,
        };

        $.ajax({
            type: "POST",
            url: "/InterviewProcess/AddProcesses",
            data: {
                "listRound": listRoundIn,
                "process": process
            },
            success: function (data) {
                window.location.href = '.\\';
            },
            error: function () {
                location.reload(true);
            }
        });
    }
}


//hidden interview process name validate message
function hiddenProcessNameValidateMessage() {
    $('#ProcessNameFormCreate').removeClass("has-error");
    $('#blockProcessNameCreate').addClass("hidden");
    $('#blockProcessNameTooLongCreate').addClass("hidden");
}


//hidden interview process name validate message
function hiddenProcessStartDateValidateMessage() {
    $('#StartDateFormCreate').removeClass("has-error");
    $('#blockStartDateCreate').addClass("hidden");
    $('#blockProcessStartDateTooLongCreate').addClass("hidden");
}


//hidden interview process name validate message
function hiddenProcessListRoundValidateMessage() {
    $('#listRoundFormCreate').removeClass("has-error");
    $('#blockListRoundCreate').addClass("hidden");
}