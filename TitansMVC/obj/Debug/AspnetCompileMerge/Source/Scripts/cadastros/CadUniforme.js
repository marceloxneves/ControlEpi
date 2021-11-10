function InicializarComponentesUniforme() {
    //$("#dtp-data-val-ca").mask("99/99/9999");
    $("#txtCustoUniforme").maskMoney({ symbol: "R$", showSymbol: false, decimal: ",", thousands: "", decimalplaces:"2" });
    
}

$(function () {
    $('#dtp-data-val-ca').datetimepicker({
        locale: 'pt-BR',
        format: 'L'
    });
});

$(function () {
    $('#datepicker-rev').datetimepicker({
        locale: 'pt-BR',
        format: 'L'
    });
});

//LER IMAGEM DO INPUT
function readFotoUniforme(input) {
    if (input.files && input.files[0]) {
        var fr = new FileReader();
        fr.onload = function (e) {
            $("#foto-uniforme").removeClass("oculto");
            $("#foto-uniforme").attr("src", e.target.result);
        };

        fr.readAsDataURL(input.files[0]);
    }
}

function removeFotoUniforme() {    
    $("#foto-uniforme").removeClass("oculto");
    $("#foto-uniforme").attr("src", "#");    
}

function CarregarFichaCompleta(resp) {
    var obj = JSON.parse(resp);

    $("#hdId").val(obj.Id);
    $("#hdIdUniforme").val(obj.IdUniforme);
    $("#txtNomeUniformeDial").val(obj.UniformeNome);
    
    $("#txtDescrEquip").val(obj.DescrDetEquip);

    if (obj.RevisadoEm !== undefined && obj.RevisadoEm !== null && obj.RevisadoEm !== "") {
        dataFormat = obj.RevisadoEm.substr(8, 2) + "/" + obj.RevisadoEm.substr(5, 2) + "/" + obj.RevisadoEm.substr(0, 4);
        $("#datepicker-rev").val(dataFormat);
    }    

    
    if (img !== undefined && img !== null) {
        var c = document.getElementById("canvas-foto-ficha");
        var ctx = c.getContext("2d");
        var img = document.getElementById("foto-uniforme");
        ctx.drawImage(img, 0, 0, c.width, c.height);
    }    

    $("#txtNumIdent").val(obj.NumIdentificacao);
    $("#txtFabrForn").val(obj.FornFabr);
    $("#txtPcsRepo").val(obj.PecasReposicao);
    $("#txtFormHig").val(obj.FormaHigienizacao);
    $("#txtFinalidade").val(obj.FinalidadeAreaAplic);
    $("#txtObs").val(obj.Obs);
    $("#txtRegistro").val(obj.Registro);
    $("#txtArea").val(obj.Area);
    $("#txtAprovadoPor").val(obj.AprovadoPor);

    $("#btnImprimir").show();
}

function CarregarFichaParcial() {
    $("#hdIdUniforme").val($("#hd-id-uniforme").val());
    $("#txtNomeUniformeDial").val($("#txtUniformeNome").val());
    
    var c = document.getElementById("canvas-foto-ficha");
    var ctx = c.getContext("2d");
    var img = document.getElementById("foto-uniforme");
    c.setAttribute('width', '200');
    c.setAttribute('height', '200');
    ctx.drawImage(img, 0, 0, c.width, c.height);
    
    $("#btnImprimir").hide();

}

function getFichaUniforme() {
    var ficha = new Object();

    ficha.Id = $("#hdId").val() === "" ? "0" : $("#hdId").val();
    ficha.IdUniforme = $("#hdIdUniforme").val();
    ficha.UniformeNome = $("#txtNomeUniformeDial").val();
    ficha.RevisadoEm = $("#datepicker-rev").val();
    ficha.DescrDetEquip = $("#txtDescrEquip").val();
    ficha.FornFabr = $("#txtFabrForn").val();
    ficha.FormaHigienizacao = $("#txtFormHig").val();
    ficha.AreaAplicacao = $("#").val();
    ficha.FinalidadeAreaAplic = $("#txtFinalidade").val();
    ficha.Obs = $("#txtObs").val();
    ficha.AprovadoPor = $("#txtAprovadoPor").val();
    ficha.Registro = $("#txtRegistro").val();
    ficha.Area = $("#txtArea").val();
    ficha.PecasReposicao = $("#txtPcsRepo").val();
    ficha.NumIdentificacao = $("#txtNumIdent").val();

    return $.toJSON(ficha);
}

function getFotoFichaUniforme() {
    var base64 = document.getElementById("canvas-foto-ficha").toDataURL("image/png");
    base64 = base64.substr(base64.indexOf(",") + 1, base64.length);

    return base64;
}

function btnVisualizar() {
    $("#btnCriarVisualFicha").text("Visualizar Ficha");
    $("#btnCriarVisualFicha").attr("title", "Visualizar a Ficha deste Uniforme");
    $("#btnCriarVisualFicha").attr("onclick", "CarregarFicha()");
    $("#btnImprimir").show();
    $("#btnExcluirFicha").show();
}

function limparExclusaoFicha() {
    $("#btnCriarVisualFicha").text("Criar Ficha");
    $("#btnCriarVisualFicha").attr("title", "Criar Ficha para este Uniforme");
    $("#btnCriarVisualFicha").attr("onclick", "CarregarFichaParcial()");
    $("#btnImprimir").hide();
    $("#btnExcluirFicha").hide();
}

function limparComponentesFicha() {
    $("#hdId").val("");
    $("#txtDescrEquip").val("");
    $("#datepicker-rev").val("");
    $("#txtNumIdent").val("");
    $("#txtFabrForn").val("");
    $("#txtPcsRepo").val("");
    $("#txtFormHig").val("");
    $("#txtFinalidade").val("");
    $("#txtObs").val("");
    $("#txtAprovadoPor").val("");
    $("#txtRegistro").val("");
    $("#txtArea").val("");
}

function carrCompDialogAltEstoque(itemEst) {
    $("#hdIdEst").val(itemEst.idEst);
    $("#hdIdProd").val(itemEst.idProd);
    $("#txtQtde").val(itemEst.qtde);
    $("#txtQtdeMin").val(itemEst.qtdeMin);
    $("#txtQtdeMax").val(itemEst.qtdeMax);
}

function carrCompDialogAddEstoque(itemEst) {
    $("#hdIdEstAdd").val(itemEst.idEst);
    $("#hdIdProdAdd").val(itemEst.idProd);
    $("#txtAddEst").val(0);
}

function getEstoque() {
    var estoque = {
        IdEst: $("#hdIdEst").val(),
        IdProd: $("#hdIdProd").val(),
        Qtde: $("#txtQtde").val() != "" ? $("#txtQtde").val() : 0,
        QtdeMin: $("#txtQtdeMin").val() != "" ? $("#txtQtdeMin").val() : 0,
        QtdeMax: $("#txtQtdeMax").val() != "" ? $("#txtQtdeMax").val() : 0
    };

    return $.toJSON(estoque);
}

function getIdEstoque() {
    return $("#hdIdEst").val();
}

function getQtde() {
    return $("#txtAddEst").val();
}