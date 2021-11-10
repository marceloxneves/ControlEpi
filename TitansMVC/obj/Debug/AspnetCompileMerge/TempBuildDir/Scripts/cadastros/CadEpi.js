function InicializarComponentesEpi() {
    //$("#dtp-data-val-ca").mask("99/99/9999");
    $("#txtCustoEpi").maskMoney({ symbol: "R$", showSymbol: false, decimal: ",", thousands: "", decimalplaces:"2" });
    //$(function () {
    //    $("#datepicker-rev").datepicker({
    //        locale: "pt-BR"
    //    });
    //});

    //$("#cbEpiAtivo").bootstrapSwitch();

    //$(function () {
    //    $("#txtEpiValCaDial").datepicker({
    //        locale: "pt-BR"
    //    });
    //});
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
function readFotoEpi(input) {
    if (input.files && input.files[0]) {
        var fr = new FileReader();
        fr.onload = function (e) {
            $("#foto-epi").removeClass("oculto");
            $("#foto-epi").attr("src", e.target.result);
        };

        fr.readAsDataURL(input.files[0]);
    }
}

function removeFotoEpi() {    
    $("#foto-epi").removeClass("oculto");
    $("#foto-epi").attr("src", "#");    
}

function CarregarFichaCompleta(resp) {
    var obj = JSON.parse(resp);

    $("#hdId").val(obj.Id);
    $("#hdIdEpi").val(obj.IdEpi);
    $("#txtNomeEpiDial").val(obj.EpiNome);
    $("#txtEpiCaDial").val(obj.EpiCa);
    if (obj.EpiCaValidade !== undefined && obj.EpiCaValidade !== null && obj.EpiCaValidade !== "") {
        var dataFormat = obj.EpiCaValidade.substr(8, 2) + "/" + obj.EpiCaValidade.substr(5, 2) + "/" + obj.EpiCaValidade.substr(0, 4);
        $("#txtEpiValCaDial").val(dataFormat);
    }

    $("#txtDescrEquip").val(obj.DescrDetEquip);

    if (obj.RevisadoEm !== undefined && obj.RevisadoEm !== null && obj.RevisadoEm !== "") {
        dataFormat = obj.RevisadoEm.substr(8, 2) + "/" + obj.RevisadoEm.substr(5, 2) + "/" + obj.RevisadoEm.substr(0, 4);
        $("#datepicker-rev").val(dataFormat);
    }    

    //usado para recuperar imagem do BD e inseri-la em um canvas
    // neste caso aqui nao mais necessario
    //var c = document.getElementById("canvas-foto-ficha");
    //var imgObj = new Image();
    //imgObj.src = "data:image/png;base64," + obj.Foto;
    //var ctx = c.getContext("2d");
    //c.setAttribute('width', '200');
    //c.setAttribute('height', '200');
    //ctx.drawImage(imgObj, 0, 0);

    if (img !== undefined && img !== null) {
        var c = document.getElementById("canvas-foto-ficha");
        var ctx = c.getContext("2d");
        var img = document.getElementById("foto-epi");
        //c.setAttribute('width', '200');
        //c.setAttribute('height', '200');
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
    $("#hdIdEpi").val($("#hd-id-epi").val());
    $("#txtNomeEpiDial").val($("#txtEpiNome").val());
    $("#txtEpiCaDial").val($("#txtEpiCa").val());
    if ($("#dtp-data-val-ca").val() !== undefined && $("#dtp-data-val-ca").val() !== null && $("#dtp-data-val-ca").val() !== "") {
        //var dataFormat = $("#dtp-data-val-ca").val().substr(6, 4) + "/" + $("#dtp-data-val-ca").val().substr(3, 2) + "/" + $("#dtp-data-val-ca").val().substr(0, 2);
        //$("#txtEpiValCaDial").val(dataFormat);
        $("#txtEpiValCaDial").val($("#dtp-data-val-ca").val());
    }

    // find eh uma opcao, mas neste caso nao eh necessario
    //$("#foto-epi-dialog").attr("src", $('#div-foto').find("#foto-epi").attr('src'));
    //$("#foto-epi-dialog").attr("src", $("#foto-epi").attr("src"));

    var c = document.getElementById("canvas-foto-ficha");
    var ctx = c.getContext("2d");
    var img = document.getElementById("foto-epi");
    c.setAttribute('width', '200');
    c.setAttribute('height', '200');
    ctx.drawImage(img, 0, 0, c.width, c.height);
    //var imgData = ctx.getImageData(10,10,10,10);

    $("#btnImprimir").hide();

}

function getFichaEpi() {
    var ficha = new Object();

    ficha.Id = $("#hdId").val() === "" ? "0" : $("#hdId").val();
    ficha.IdEpi = $("#hdIdEpi").val();
    ficha.EpiNome = $("#txtNomeEpiDial").val();
    ficha.EpiCa = $("#txtEpiCaDial").val();
    ficha.EpiCaValidade = $("#txtEpiValCaDial").val();
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

//function getFotoFichaEpi() {
//    var base64 = img_find();
//    base64 = base64.substr(base64.indexOf(",") + 1, base64.length);

//    return base64;
//}

function getFotoFichaEpi() {
    var base64 = document.getElementById("canvas-foto-ficha").toDataURL("image/png");
    base64 = base64.substr(base64.indexOf(",") + 1, base64.length);

    return base64;
}

function btnVisualizar() {
    $("#btnCriarVisualFicha").text("Visualizar Ficha");
    $("#btnCriarVisualFicha").attr("title", "Visualizar a Ficha deste EPI");
    $("#btnCriarVisualFicha").attr("onclick", "CarregarFicha()");
    $("#btnImprimir").show();
    $("#btnExcluirFicha").show();
}

function limparExclusaoFicha() {
    $("#btnCriarVisualFicha").text("Criar Ficha");
    $("#btnCriarVisualFicha").attr("title", "Criar Ficha para este EPI");
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

// pega a imagem da tag img
//function img_find() {
//    var c = document.getElementById("canvas-foto-ficha");
//        var ctx = c.getContext("2d");
//        var img = document.getElementById("foto-epi");
//        ctx.drawImage(img, 10, 10);
//        var imgData = ctx.getImageData(10,10,10,10);

//    return imgData;

//}

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