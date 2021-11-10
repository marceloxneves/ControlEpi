function InicializarComponentesEmpresa() {
    $("#txtCep").mask("99999-999");

    $("#btnAddTel").click(function () {
        $("#inputsTel").toggle();
    });

    $("#btnAddMail").click(function () {
        $("#inputsMail").toggle();
    });

    $("#cbEmprMatriz").bootstrapSwitch();
    $("#cbPossuiBiometria").bootstrapSwitch();
    $("#cbPossuiAssinatura").bootstrapSwitch();
}

//LER IMAGEM DO INPUT
function readLogoEmprImage(input) {
    if (input.files && input.files[0]) {
        var fr = new FileReader();
        fr.onload = function (e) {
            $("#logo-empr").removeClass("oculto");
            $("#logo-empr").attr("src", e.target.result);
        };

        fr.readAsDataURL(input.files[0]);
    }
}

function removeLogoEmpresa() {
    $("#logo-empr").removeClass("oculto");
    $("#logo-empr").attr("src", "#");
}

function getTel() {
    var telefone = new Object();

    telefone.EmpresaId = $("#model-id").val();
    telefone.Descricao = $("#txtTelDescricao").val();
    telefone.Numero = $("#txtTelNumero").val();
    telefone.Ramal = $("#txtTelRamal").val();

    return $.toJSON(telefone);
}

function getMail() {
    var email = new Object();

    email.EmpresaId = $("#model-id").val();
    email.Descricao = $("#txtMailDescricao").val();
    email.Email = $("#txtMailEmail").val();

    return $.toJSON(email);
}

function LimparCompTel() {
    $("#txtTelDescricao").val("");
    $("#txtTelNumero").val("");
    $("#txtTelRamal").val("");
}

function LimparCompMail() {
    $("#txtMailDescricao").val("");
    $("#txtMailEmail").val("");
}