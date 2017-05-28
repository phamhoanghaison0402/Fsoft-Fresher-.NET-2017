const MAXLENGTHPROCESSNAME = 100;
const MAXLENGTHPROCESSSTARTDATE = 10;
//Initialize function
init();
function init() {
    ShowData();
    LoadRoundInTable();
}


//get event press enter
$(document).keypress(function (e) {
    if (e.which == 13) {
        editProcess();
    }
});


// show data to select position and check box active
function ShowData() {
    document.getElementById('selectPosition').value = interviewProcess.PositionID;
    document.getElementById('Status').checked = interviewProcess.Active;
}


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
    hiddenProcListRoundValidateMesEdit();
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
            alert('Failed to get detail round.');
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
    //delete in table in view
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


//edit process
function editProcess() {
    var processName = document.getElementById('ProcessName').value;
    var status = document.getElementById('Status').checked;
    var positionId = $("#selectPosition :selected").val();
    var startDate = document.getElementById('datepicker').value;
    var commit = true;
    //check validate for processName
    if (processName == null || processName == '') {
        $('#ProcessNameForm').addClass("has-error");
        $('#blockProcessName').removeClass("hidden");
        commit = false;
    } else {
        if (processName.length > MAXLENGTHPROCESSNAME) {
            $('#ProcessNameForm').addClass("has-error");
            $('#blockProcessNameTooLong').removeClass("hidden");
        } else {
            hiddenProcNameValidateMesEdit();
        }
    }
    //check validate for startDate
    if (startDate == null || startDate == '') {
        $('#StartDateForm').addClass("has-error");
        $('#blockStartDate').removeClass("hidden");
        commit = false;
    } else {
        if (processName.length > MAXLENGTHPROCESSSTARTDATE) {
            $('#StartDateForm').addClass("has-error");
            $('#blockProcessStartDateTooLong').removeClass("hidden");
        } else {
            hiddenProcStartDateValidateMesEdit();
        }
    }
    //check validate for list interview round in process
    if (listRoundIn.length == 0) {
        $('#listRoundForm').addClass("has-error");
        $('#blockListRound').removeClass("hidden");
        commit = false;
    } else {
        hiddenProcListRoundValidateMesEdit();
    }

    //check is data changed
    var interviewStartDate = getFormattedDate(interviewProcess.StartDate); //change format interview start date to dd/mm/yyyy
    if (processName == interviewProcess.ProcessName && status == interviewProcess.Active
        && startDate == interviewStartDate && positionId == interviewProcess.PositionID) {
        commit = false;
        if (listRoundIn.length == listRoundInPrevious.length) {
            for (i = 0; i < listRoundIn.length; i++) {
                if (listRoundIn[i] != listRoundInPrevious[i]) {
                    commit = true;
                    break;
                }
            }
        } else {
            commit = true;
        }
    }

    //if data is valid, send to data to controller
    if (commit == true) {
        //create edit object
        var process = {
            ID: interviewProcess.ID,
            ProcessName: processName,
            PositionID: positionId,
            Active: status,
            StartDate: startDate
        };
        //Send search object to controller and get response
        $.ajax({
            type: "POST",
            url: "/InterviewProcess/EditProcess",
            data: {
                "listRound": listRoundIn,
                "process": process
            },
            success: function (data) {
                window.location.href = '..\\';
            },
            error: function () {
                location.reload(false);
            }
        });
    }
}


//change format date to dd/mm/yyyy
function getFormattedDate(input) {
    var date = new Date(input);
    var year = date.getFullYear();
    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    return day + '/' + month + '/' + year;
}


//hidden interview process name validate message
function hiddenProcNameValidateMesEdit() {
    $('#ProcessNameForm').removeClass("has-error");
    $('#blockProcessName').addClass("hidden");
    $('#blockProcessNameTooLong').addClass("hidden");
}


//hidden interview process name validate message
function hiddenProcStartDateValidateMesEdit() {
    $('#StartDateForm').removeClass("has-error");
    $('#blockStartDate').addClass("hidden");
    $('#blockProcessStartDateTooLong').addClass("hidden");
}


//hidden interview process name validate message
function hiddenProcListRoundValidateMesEdit() {
    $('#listRoundForm').removeClass("has-error");
    $('#blockListRound').addClass("hidden");
}