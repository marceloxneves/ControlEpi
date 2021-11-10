function InicializarComponentesColaboradores() {
    $("#txtCpf").mask("999.999.999-99");
    $("#txtCpf").blur(function () {
        if ($("#txtCpf").val() !== "") {
            if (!valida_cpf($("#txtCpf").val())) {
                alert("CPF Inválido.");
                $("#txtCpf").focus();
            } 
        } 
    });

    verificarAdvertencia();

    $("#cbRecebeuTreinamento").bootstrapSwitch();
    $("#cbRecebeuAdvertencia").bootstrapSwitch({
        onSwitchChange: function () {
            verificarAdvertencia();
        }
    });
}

$(function () {
    $('#dtp-data-admissao').datetimepicker({
        locale: 'pt-BR',
        format: 'L'
    });
});

$(function () {
    $('#dtp-data-nascimento').datetimepicker({
        locale: 'pt-BR',
        format: 'L'
    });
});

function liberarEpi() {
    $("#inputEpi").show();
    alert("Registro Biométrico Válido!");
}

function liberarUniforme() {
    $("#inputUniforme").show();
    alert("Registro Biométrico Válido!");
}

function bloquearEpi() {
    $("#inputEpi").hide();
    alert("Registro Biométrico Inválido ou Não Cadastrado!");
}

function bloquearUniforme() {
    $("#inputUniforme").hide();
    alert("Registro Biométrico Inválido ou Não Cadastrado!");
}


function verificarAdvertencia() {
    if ($("#cbRecebeuAdvertencia").is(":checked")) {
        $("#div-motivo-advertencia").show();
    } else {
        $("#div-motivo-advertencia").hide();
        $("#taMotAdvertencia").val("");
    }
}

function InicializarEditComponentesColaboradores() {
    //$("#divBiometria").hide();
    $("#divAssinatura").hide();

    $('.sigPad').signaturePad({ drawOnly: true });

    //$("#btnCreateEpi").click(function () {
    //    $("#txtQtde").val("");
    //});

    $("#ddlEpisOutrSetores").change(function () {
        $("#ddlEpis option").each(function () {
            if ($(this).html() === "Selecione") {
                $(this).attr("selected", "selected");
                $("#txtJustificativaEpiOutroSetor").show();
                $("#lblJustificativa").show();
                return;
            }
        });
    });

    $("#ddlUniformesOutrSetores").change(function () {
        $("#ddlUniformes option").each(function () {
            if ($(this).html() === "Selecione") {
                $(this).attr("selected", "selected");
                $("#txtJustificativaUniformeOutroSetor").show();
                $("#lblJustificativa").show();
                return;
            }
        });
    });

    $("#ddlEpis").change(function () {
        $("#ddlEpisOutrSetores option").each(function () {
            if ($(this).html() === "Selecione") {
                $(this).attr("selected", "selected");
                $("#txtJustificativa").hide();
                $("#lblJustificativa").hide();
                return;
            }
        });
    });

    $("#ddlUniformes").change(function () {
        $("#ddlUniformesOutrSetores option").each(function () {
            if ($(this).html() === "Selecione") {
                $(this).attr("selected", "selected");
                $("#txtJustificativa").hide();
                $("#lblJustificativa").hide();
                return;
            }
        });
    });

    $("#cbTodos").change(function () {
        if ($("#cbTodos").is(":checked")) {
            $(".marcar").each(
                function () {
                    $(this).prop("checked", true); //"#cb-baixado"
                });
        } else {
            $(".marcar").each(
                function () {
                    $(this).prop("checked", false); //"#cb-baixado"
                });
        }
    });

    $('input[type=radio][name=TipoAutenticacao]').change(function () {
        if (this.value == 'Assinatura') {
            $("#divBiometria").hide();           
            $("#divAssinatura").show();
            $("#txtTipoValidacao").val(true);
            console.log($("#txtTipoValidacao").val());
        }
        else if (this.value == 'Biometria') {
            $("#divAssinatura").hide();
            $("#divBiometria").show();
            $("#txtTipoValidacao").val(false);
        }
    });
}

//LER IMAGEM DO INPUT
function readImage(input) {
    if (input.files && input.files[0]) {
        var fr = new FileReader();

        fr.onload = function (e) {
            $("#foto-col").removeClass("oculto");
            $("#foto-col").attr("src", e.target.result);
        };

        fr.readAsDataURL(input.files[0]);
    }
}

function getEpi() {
    var epi = new Object();

    epi.ColaboradorId = $("txtId").val();
    if ($("#ddlEpis").val() !== "" && $("#ddlEpis").val() !== null) {
        //epi.NomeEpi = $("#ddlEpis option:selected").text();
        epi.EpiSetorId = $("#ddlEpis").val();
    } else {
        //epi.NomeEpi = $("#ddlEpisOutrSetores option:selected").text();
        epi.EpiSetorId = $("#ddlEpisOutrSetores").val();
    }
    epi.CentroCustoId = $("#ddlCentroCusto").val();

    epi.Quantidade = $("#txtQtde").val();

    if ($("#txtTipoValidacao").val() != undefined && $("#txtTipoValidacao").val() != null && $("#txtTipoValidacao").val() != "")
    {
        epi.TipoValidacao = $("#txtTipoValidacao").val();
    }
    else
    {
        epi.TipoValidacao = false;
    }

    epi.JustificativaEpiOutroSetor = $("#txtJustificativaEpiOutroSetor").val();

    return $.toJSON(epi);
}

function getUniforme() {
    var uniforme = new Object();

    uniforme.ColaboradorId = $("txtId").val();
    if ($("#ddlUniformes").val() !== "" && $("#ddlUniformes").val() !== null) {
        uniforme.UniformeSetorId = $("#ddlUniformes").val();
    } else {
        uniforme.UniformeSetorId = $("#ddlUniformesOutrSetores").val();
    }
    uniforme.CentroCustoId = $("#ddlCentroCusto").val();

    uniforme.Quantidade = $("#txtQtde").val();

    if ($("#txtTipoValidacao").val() != undefined && $("#txtTipoValidacao").val() != null && $("#txtTipoValidacao").val() != "") {
        uniforme.TipoValidacao = $("#txtTipoValidacao").val();
    }
    else {
        uniforme.TipoValidacao = false;
    }

    uniforme.JustificativaUniformeOutroSetor = $("#txtJustificativaUniformeOutroSetor").val();

    return $.toJSON(uniforme);
}

function getAssinaturaAtual() {
    //Utilizar o toDataURL para converter em Base64
    //var base64 = $("#imgAssinaturaAtual").toDataURL("image/png");
    //var base64 = getBase64Image(img_find());
    //var base64 = convertImageToCanvas(img_find());
    var base64 = img_find();
    base64 = base64.substr(base64.indexOf(',') + 1, base64.length);

    return base64;
}

// pega a imagem da tag img
function img_find() {
    var img = $("#imgAssinaturaAtual");
    var imgSrcs = [];

    for (var i = 0; i < img.length; i++) {
        imgSrcs.push(img[i].src);
    }

    //return imgSrcs;
    return img[0].src;
}

function liberarEpi() {
    if ($("#ddlSetor").val() !== "") {
        var image = document.getElementById("cassinaturaAtual").toDataURL("image/png");
        if (image !== undefined && image !== null && image !== "") {
            image = image.replace('data:image/png;base64,', '');
            $("#imageData").val(image);
            $("#assinOption").val(2);
        }
        $("#inputEpi").toggle();
    } else {
        alert("Este colaborador não está vinculado a nenhum setor. Será necessário vinculá-lo a um setor antes de efetuar a entrega do EPI");
    }
}

function liberarUniforme() {
    if ($("#ddlSetor").val() !== "") {
        var image = document.getElementById("cassinaturaAtual").toDataURL("image/png");
        if (image !== undefined && image !== null && image !== "") {
            image = image.replace('data:image/png;base64,', '');
            $("#imageData").val(image);
            $("#assinOption").val(2);
        }
        $("#inputUniforme").toggle();
    } else {
        alert("Este colaborador não está vinculado a nenhum setor. Será necessário vinculá-lo a um setor antes de efetuar a entrega do Uniforme");
    }
}

function carregarAssinaturas() {
    $("#imgAssinaturaOriginal").attr("src", $("#assinaturaColaborador")[0].src);

    var assinaturaAtual = document.getElementById("cassinaturaAtual").toDataURL("image/png");
    $("#imgAssinaturaAtual").attr("src", assinaturaAtual);
}

function LimparCombosEpiSetores() {
    $("#ddlEpis").empty();
    $("#ddlEpis").append('<option value="">Selecione</option>');
    $("#ddlEpisOutrSetores").empty();
    $("#ddlEpisOutrSetores").append('<option value="">Selecione</option>');
}

function LimparCombosUniformeSetores() {
    $("#ddlUniformes").empty();
    $("#ddlUniformes").append('<option value="">Selecione</option>');
    $("#ddlUniformesOutrSetores").empty();
    $("#ddlUniformesOutrSetores").append('<option value="">Selecione</option>');
}

function ResetarCombosEpiSetores() {
    $("#ddlEpis option").each(function () {
        if ($(this).html() === "Selecione") {
            $(this).attr("selected", "selected");
            return;
        }
    });

    $("#ddlEpisOutrSetores option").each(function () {
        if ($(this).html() === "Selecione") {
            $(this).attr("selected", "selected");
            return;
        }
    });
}

function ResetarCombosUniformeSetores() {
    $("#ddlUniformes option").each(function () {
        if ($(this).html() === "Selecione") {
            $(this).attr("selected", "selected");
            return;
        }
    });

    $("#ddlUniformesOutrSetores option").each(function () {
        if ($(this).html() === "Selecione") {
            $(this).attr("selected", "selected");
            return;
        }
    });
}

function ResetarCombosEpis() {
    $("#ddlTipoEpi option").each(function () {
        if ($(this).html() === "Selecione") {
            $(this).attr("selected", "selected");
            return;
        }
    });

    ResetarCombosEpiSetores();

    $("#ddlCentroCusto option").each(function () {
        if ($(this).html() === "Selecione") {
            $(this).attr("selected", "selected");
            return;
        }
    });
}

function ResetarCombosUniformes() {
    $("#ddlTipoUniforme option").each(function () {
        if ($(this).html() === "Selecione") {
            $(this).attr("selected", "selected");
            return;
        }
    });

    ResetarCombosUniformeSetores();

    $("#ddlCentroCusto option").each(function () {
        if ($(this).html() === "Selecione") {
            $(this).attr("selected", "selected");
            return;
        }
    });
}


function guardarIdTemp(id) {
    $("#txtIdEpiEntregue").val(id);
}

function guardarIdTempUniforme(id) {
    $("#txtIdUniformeEntregue").val(id);
}

function limparAssinCad() {
    $('#limparAssinCad').click();
}

function limparAssinPegarEpi() {
    $('#limparAssin').click();
}

function limparAssinPegarUniforme() {
    $('#limparAssin').click();
}


