function validateEdit() {
    var validated = true;

    var roundName = document.getElementById("roundNameEdit").value;

    if ((roundName.length <= 0) || (roundName.length >= 50)) {
        $('#roundNameFormEdit').addClass("has-error");
        $('#roundNameRequiredEdit').removeClass("hidden");
        document.getElementById("roundNameEdit").value = "";
        validated = false;
    } else {
        removeValidateEdit()        
    }


    if (validated) {
        $('button[name="submitButtonEdit"]').attr('type', 'submit');
        $('[type="submit"]').submit();
    }

  
}

function removeValidateEdit() {
    $('#roundNameFormEdit').removeClass("has-error");
    $('#roundNameRequiredEdit').addClass("hidden");

}