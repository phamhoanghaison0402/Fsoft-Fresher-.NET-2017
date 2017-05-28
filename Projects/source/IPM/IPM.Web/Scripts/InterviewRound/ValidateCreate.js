function validate() {
    var validated = true;
    
    var roundName = document.getElementById("roundName").value;

    if ((roundName.length <= 0) || (roundName.length >= 50)) {
        $('#roundNameForm').addClass("has-error");
        $('#roundNameRequired').removeClass("hidden");
        document.getElementById("roundName").value = "";
        validated = false;
    } else {        
        removeValidate();
    }

    if (validated) {        
        $('button[name="submitButton"]').attr('type', 'submit');
        $('[type="submit"]').submit();
    }
    
}
function removeValidate() {
    $('#roundNameForm').removeClass("has-error");
    $('#roundNameRequired').addClass("hidden");
}