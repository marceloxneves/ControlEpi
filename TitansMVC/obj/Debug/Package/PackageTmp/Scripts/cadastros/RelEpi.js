function InicializarComponentesRelEpiColaborador() {
    $(function () {
        $("#dp-inicio").datepicker({
            locale: "pt-BR",
            onSelect: function () {
                //$('#dp-fim').datepicker('setDate', $('#dp-inicio').datepicker('getDate'));
                $('#dp-fim').datepicker('option', 'minDate', $('#dp-inicio').datepicker('getDate'));
            }
        });
    });

    $(function () {
        $("#dp-fim").datepicker({
            locale: "pt-BR",
            onClose: function () {
                //check to prevent a user from entering a date below date of dtInicio
                if ($('#dp-fim').datepicker('getDate') <= $('#dp-inicio').datepicker('getDate')) {
                    var minDate = $('#dp-fim').datepicker('option', 'minDate');
                    $('#dp-fim').datepicker('setDate', minDate);
                }
            }
        });
    });
}

function ColEditGridRow(id) {
    $("#txtIdColaborador").val(id);
    $("#pesqColaborador").hide();
    //$("#it-todos").attr("checked", true);
    $("#search").val("");
    $("#txtIdColaborador").blur();
}

function EpiEditGridRow(id) {
    $("#txtIdEpi").val(id);
    $("#pesqEpi").hide();
    //$("#it-todos").attr("checked", true);
    $("#search").val("");
    $("#txtIdEpi").blur();
}

function getEpiColFilter() {
    var epiColFiltr = new Object();

    epiColFiltr.UnidadeNegocioId = $("#ddlUnidadeNegocio").val() === "" ? "0" : $("#ddlUnidadeNegocio").val();
    epiColFiltr.ColaboradorId = $("#ddlColaboradores").val() === "" ? "0" : $("#ddlColaboradores").val();
    epiColFiltr.TipoEpiId = $("#ddlTiposEpis").val() === "" ? "0" : $("#ddlTiposEpis").val();
    epiColFiltr.EpiId = $("#ddlEpis").val() === "" ? "0" : $("#ddlEpis").val();
    epiColFiltr.Marca = $("#txtMarca").val();
    epiColFiltr.SetorId = $("#ddlSetores").val() === "" ? "0" : $("#ddlSetores").val();
    epiColFiltr.CentroCustoId = $("#ddlCCustos").val() === "" ? "0" : $("#ddlCCustos").val();
    epiColFiltr.Ativos = $("#hdAtivos").val();
    //epiColFiltr.EstadoEpis = $("#rb-todos").is(":checked") ? "0" : $("#rb-em-uso").is(":checked") ? "1" : "2";
    if (($("#dp-inicio").val() !== "") && ($("#dp-fim").val() !== "")) {
        epiColFiltr.DataInicial = $("#dp-inicio").val();
        epiColFiltr.DataFinal = $("#dp-fim").val();
    }
    
    //epiColFiltr.TipoRelatorio = $("#rb-impr").is(":checked") ? "impr" : "tela";
    epiColFiltr.TipoRelatorio = "não utilizado ainda";

    return $.toJSON(epiColFiltr);
}

function getEpiColFilterImpressao() {
    var epiColFiltr = new Object();
    epiColFiltr.UnidadeNegocioId = $("#hdUnidadeNegocio").val();
    epiColFiltr.ColaboradorId = $("#hdColaborador").val();
    epiColFiltr.TipoEpiId = $("#hdTipoEpi").val();
    epiColFiltr.EpiId = $("#hdEpi").val();
    epiColFiltr.Marca = $("#hdMarca").val();
    epiColFiltr.SetorId = $("#hdSetor").val();
    epiColFiltr.CentroCustoId = $("#hdCentroCusto").val();
    epiColFiltr.EstadoEpis = $("#hdEstado").val();
    epiColFiltr.Ativos = $("#hdAtivos").val();
    if (($("#hdDataInicial").val() !== "") && ($("#hdDataFinal").val() !== "")) {
        var dataInicial = $("#hdDataInicial").val();
        var dataFinal = $("#hdDataFinal").val();
        epiColFiltr.DataInicial = dataInicial.split(" ")[0];
        epiColFiltr.DataFinal = dataFinal.split(" ")[0];
    }
    
    return $.toJSON(epiColFiltr);
}

function getEpiColFilterImpressaoEpiFalta() {
    var epiColFiltr = new Object();

    epiColFiltr.EpiId = $("#hdEpi").val();
    epiColFiltr.DescrEpi = $("#hdDescrEpi").val();
    epiColFiltr.Marca = $("#hdMarca").val();
    epiColFiltr.FabricanteId = "";

    return $.toJSON(epiColFiltr);
}