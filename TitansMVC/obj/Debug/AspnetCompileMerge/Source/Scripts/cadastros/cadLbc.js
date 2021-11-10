function InicializarComponentesLbc() {
    //$("#cbLbcAtivo").bootstrapSwitch();
}

//LER IMAGEM DO INPUT
function readLogoLbcImage(input) {
    if (input.files && input.files[0]) {
        var fr = new FileReader();
        fr.onload = function (e) {
            $("#logo-lbc").removeClass("oculto");
            $("#logo-lbc").attr("src", e.target.result);
        };

        fr.readAsDataURL(input.files[0]);
    }
}

function removeLogoLbc() {
    $("#logo-lbc").addClass("oculto");
    $("#logo-lbc").attr("src", "#");
}
