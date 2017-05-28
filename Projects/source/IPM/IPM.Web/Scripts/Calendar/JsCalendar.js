$(document).ready(function () {
    var check = 0;
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        buttonText: {
            today: 'today',
            month: 'month',
            week: 'week',
            day: 'day'
        },
        defaultView: 'agendaWeek',
        editable: true,
        allDaySlot: false,
        slotMinutes: 15,
        events: function (start, end, timezone, callback) {
            $('#icon_loadingdata').css("display", "block");
            $.ajax({
                url: "/Calendar/GetCalendarEvents/",
                contentType: "application/json",
                type: "Get",
                dataType: "json",
                success: function (doc) {
                    if (doc.status) {
                        var my_events = [];
                        $.map(doc.data,
                            function(r) {
                                my_events.push({
                                    id: r.id,
                                    title: r.title,
                                    start: moment(r.start).format('MM/DD/YYYY hh:mm a'),
                                    end: moment(r.end).format('MM/DD/YYYY hh:mm a'),
                                    body: r.body,
                                    location: r.location,
                                    address: r.address,
                                });
                            });
                        callback(my_events);
                        $('#icon_loadingdata').css("display", "none");
                    } else {
                        $('#icon_loadingdata').css("display", "none");
                        alert(doc.message);
                        if (confirm("Do you want to reload page?")) {
                            $('#calendar').fullCalendar('refetchEvents');
                        }
                    }
                }
            });                   
        },
        
        //eventClick: function (calEvent, jsEvent, view) {
        //    $('#hideOrganizer').css("display","block");
        //    alert('You clicked on event id: ' + calEvent.id
        //        + "\Address : " + calEvent.address 
        //        + "\nAnd the title is: " + calEvent.title); 
        //},        
        droppable: true, // this allows things to be dropped onto the calendar !!!
        eventDrop: function (event, delta, revertFunc) {
            $.ajax({
                dataType: 'Json',
                type: 'POST',
                url: "/Calendar/CalendarDrop/",
                data: {
                    data:{
                        Start: moment(event.start).format('MM/DD/YYYY hh:mm a'),
                        End: moment(event.end).format('MM/DD/YYYY hh:mm a'),
                        ID: event.id,
                        Title: event.title,
                        Location: event.location,
                        Body:event.body,
                    }               
                },
                success: function (data) {
                    if (data.status) {
                        bootbox.alert("Update success!");
                        $('#calendar').fullCalendar('refetchEvents');
                    }
                },
            });
        },
        //event pull
        eventResize: function (event, dayDelta, minuteDelta, revertFunc) {
            bootbox.confirm({
                message: "Confirm change appointment length?",
                buttons: {
                    confirm: {
                        label: 'Yes',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'No',
                        className: 'btn-danger'
                    }
                },
                callback: function (result) {
                    if (result == true) {
                        $.ajax({
                            dataType: 'Json',
                            type: 'POST',
                            url: "/Calendar/CalendarDrop/",
                            data: {
                                data: {
                                    Start: moment(event.start).format('MM/DD/YYYY hh:mm a'),
                                    End: moment(event.end).format('MM/DD/YYYY hh:mm a'),
                                    Title: event.title,
                                    Location: event.location,
                                    ID: event.id,
                                    Boday: event.body,
                                },
                            },
                            success: function (data) {
                                if (data.status) {
                                    bootbox.alert("Update success!");
                                    $('#calendar').fullCalendar('refetchEvents');
                                }
                                else {
                                    bootbox.alert("Update success!");
                                    revertFunc();
                                }
                            },
                        });
                    }                   
                }
            });           
        },
        selectable: true,
        selectHelper: true,
        dayClick: function (date, allDay, jsEvent, view) {
            check = 0;
            $('#hideOrganizer').css("display","none");
            $('#txt_eventTitle').val("");
            $('#txt_Location').val("");
            var start = moment(date).utc().format('YYYY-MM-DDTHH:mm:ss');
            var end = moment(date).add(30, 'm').utc().format('YYYY-MM-DDTHH:mm:ss');
            $('#txt_StartDate').val(start);
            $('#txt_EndDate').val(end)
            $('#txt_Body').val("");
            $('#txt_eventTitle').focus();            
            $('#modalCalendar').modal('show');
        },
        eventRender: function (event, element) {
            element.append("<div style='position:absolute;top:0px;right:0px;z-index:100'><button type='button' id='btnDeleteEvent'   class='btn btn-block btn-primary btn-flat'>X</button></div>");
            element.find("#btnDeleteEvent").click(function () {
                bootbox.confirm({
                    message: "Do you want to delete this event?",
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },
                    callback: function (result) {
                        if (result == true) {
                            $.ajax({
                                dataType: 'Json',
                                type: 'POST',
                                url: '/Calendar/DeleteEvent/',
                                data: {
                                    id: event.id,
                                },
                                success: function (response) {
                                    if (response.status) {
                                        $('#calendar').fullCalendar('removeEvents', event._id);
                                    }
                                    else {
                                        bootbox.alert("Delete fail!");
                                    }
                                },
                            });
                        }                    
                    }
                });                
            });
            element.bind('dblclick', function () {
                check = 1;
                $('#txt_eventTitle').val(event.title);
                $('#txt_Location').val(event.location);
                var start = moment(event.start).utc().format('YYYY-MM-DDTHH:mm:ss');
                var end = moment(event.end).utc().format('YYYY-MM-DDTHH:mm:ss');
                $('#txt_StartDate').val(start);
                $('#txt_EndDate').val(end);
                $('#txt_Body').val(event.body);
                //$('#txt_eventTitle').focus();
                $('#eventID').val(event.id);
                if(event.address!=null)
                {
                    $('#hideOrganizer').css("display", "block");
                    $('#txt_Organizer').val(event.address);
                }
                $('#modalCalendar').modal('show');
            });
            if (event.allDay == true) {
                event.allDay = true;
            } else {
                event.allDay = false;
            }
        },
    });
   
    $('#btnSaveData').click(function () {
        saveFunction();
    });
    function saveFunction() {
        if ($('#EventForm').valid()) {
            var data = {
                'Start': moment($('#txt_StartDate').val()).format('MM/DD/YYYY hh:mm a'),
                'End': moment($('#txt_EndDate').val()).format('MM/DD/YYYY hh:mm a'),
                'Title': $('#txt_eventTitle').val(),
                'Location': $('#txt_Location').val(),
                'Body': $('#txt_Body').val(),
                'ID': $('#eventID').val(),
            }
            var url = "";
            if (check == 1) {
                url = "/Calendar/CalendarDrop/";
            }
            else {
                url = "/Calendar/AddEvent/";
            }
            $.ajax({
                dataType: 'Json',
                type: 'POST',
                data: {
                    data: data,
                },
                url: url,
                success: function (response) {
                    if (response.status) {
                        bootbox.alert(response.message);
                        $('#modalCalendar').modal('hide');
                        $('#calendar').fullCalendar('refetchEvents');
                    }
                    else {
                        bootbox.alert(response.message);
                    }
                },
            });
        }
    };

    var wage = document.getElementById("EventForm");
    wage.addEventListener("keydown", function (e) {
        if (e.keyCode === 13) {
            saveFunction();
        }
    });
    $.validator.addMethod("greaterThan",function (value, element, params) {
        if (!/Invalid|NaN/.test(new Date(value))) {
            return new Date(value) > new Date($(params).val());
        }
        return isNaN(value) && isNaN($(params).val())
            || (Number(value) > Number($(params).val()));
    }, 'Must be greater than {0}.');
    $('#EventForm').validate({
        rules: {
            Title: { required: true },
            StartDate:{
                required:true
            },
            EndDate: {               
                greaterThan: "#txt_StartDate"
            },
            Location:{required:true},
        },
        messages: {
            EndDate: { greaterThan: "EndDate must greater than startDate" },
        },
    });
})

