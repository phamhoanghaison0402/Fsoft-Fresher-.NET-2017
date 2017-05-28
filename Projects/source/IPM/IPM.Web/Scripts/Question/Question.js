//hien modal khi chon 
function PopupCreate() {
    $('#AddQuestionModal').modal('show');
    $('#formAddQuestion').validate({
        rules: {
            Content: {
                required: true
            },
            Answer: { required: true }
        },
        messages: {
            Content: {
                required: $('#Contentvalid').val()
},
            Answer: { required: $('#Answervalid').val() }
        }
    });
    $('#formAddQuestion').validate().resetForm();

}


$("#btnAddQuestion1").click(function () {
    if ($('#formAddQuestion').valid()) {
        //khoong chuyeern trang/ko submit form
        // viet noi dung ajax
        // lấy giấ trị trong thẻ html theo id
        var addskillid = $('#selectSkill1 :selected').val();
        var addcontent = $('#inputContent').val();
        var addanswer = $('#inputAnswer').val();
        var addactive = false;
        if ($('#inputActive').is(":checked")) {
            // it is checked
            addactive = true;
        }
        //gan gia tri cho Object tu model
        var object = {
            SkillID: addskillid,
            Content: addcontent,
            Answer: addanswer,
            Active: addactive
        }
        console.log(object);
        $.ajax({
            url: "/Questions/Create",
            type: "POST",
            data: {
                Question: object

            },
            success: function (result) {
                window.location = '/questions';
            },
            error: function (result) {
                console.log("Không thêm được!");
                location.reload();
            }
        });
    }
   
});
//Update question
function PopupUpdate(id, skillID, Content, Answer, Active, _SkillID) {
    console.log(id, skillID, Content, Answer, Active);
    $('#_skillID').val(_skillID);
    $('#EditQuestionModal input#Editupdateid').val(id);
    $('#EditQuestionModal input#updatecontent').val(Content);
    $('#EditQuestionModal input#updateanswer').val(Answer);
    $('#selectSkill2').select2("val", _SkillID);
    // Checkbox  khi gia tri bang true
    if (Active == 'True') {

        $('#updateactive').prop('checked', true);
    }
    else {

        $('#updateactive').prop('checked', false);
    }

    $('#EditQuestionModal').modal('show');
    
}

//Khi chon nut Update Question 
$("#btnUpdateQuestion").click(function (e) {
    //khoong chuyeern trang/ko submit form
    // viet noi dung ajax
    console.log(updateid);
    // gan gia tri hien thi len modal
    var updateid = $('#Editupdateid ').val();
    var _skillID = $('#selectSkill2 :selected').val();
    var updatecontent = $('#updatecontent').val();
    var updateanswer = $('#updateanswer').val();
    var updateactive = false;
    if ($('#updateactive').is(":checked")) {
        // it is checked
        updateactive = true;
    }

    //gan gia tri cho Object tu model
    var object = {
        ID: updateid,
        SkillID: _skillID,
        Content: updatecontent,
        Answer: updateanswer,
        Active: updateactive
    }
    console.log(object);
    $.ajax({
        url: "/Questions/Update",
        type: "POST",
        data: {
            Question: object,

        },
        success: function (result) {
            window.location = '/questions';
        },
        error: function (result) {
            console.log("Không cập nhật được!");
            location.reload();
        }
    });
});


//Delete Question
function PopupDelete(id) {
    $('#deleteid').val(id);
    $('#DeleteQuestionModal').modal('show');

}
$("#btnDeleteQuestion").click(function (e) {
    var id = $('#deleteid').val();
    console.log(id);

    $.ajax({
        url: "/Questions/Delete",
        type: "POST",
        data: {
            id: id,
        },
        success: function (result) {
            window.location = '/questions';
        },
        error: function (result) {
            console.log("Không xóa được!");
            location.reload();
        }
    });
});


function PopupDetail(id, skillID, Content, Answer, Active) {
    console.log(id, skillID, Content, Answer, Active);
    //Gan gia tri hien thi len cac label tren model 
    $('#DetailQuestionModal #DetailID').html(id);
    $('#DetailQuestionModal #detailskillid').html(skillID);
    $('#DetailQuestionModal #detailcontent').html(Content);
    $('#DetailQuestionModal #detailanswer').html(Answer);
    // Checkbox  khi gia tri bang true
    if (Active == 'True') {

        $('#detailactive').prop('checked', true);
    } else {

        $('#detailactive').prop('checked', false);
    }
    $('#DetailQuestionModal').modal('show');
}



