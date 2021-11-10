function InicializarComponentesRelUniformeColaborador() {
    $(function () {
        $("#dp-inicio").datepicker({
            locale: "pt-BR",
            onSelect: function () {
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
    $("#search").val("");
    $("#txtIdColaborador").blur();
}

function UniformeEditGridRow(id) {
    $("#txtIdUniforme").val(id);
    $("#pesqUniforme").hide();
    $("#search").val("");
    $("#txtIdUniforme").blur();
}

function getUniformeColFilter() {
    var uniformeColFiltr = new Object();

    uniformeColFiltr.UnidadeNegocioId = $("#ddlUnidadeNegocio").val() === "" ? "0" : $("#ddlUnidadeNegocio").val();
    uniformeColFiltr.ColaboradorId = $("#ddlColaboradores").val() === "" ? "0" : $("#ddlColaboradores").val();
    uniformeColFiltr.TipoUniformeId = $("#ddlTiposUniformes").val() === "" ? "0" : $("#ddlTiposUniformes").val();
    uniformeColFiltr.UniformeId = $("#ddlUniformes").val() === "" ? "0" : $("#ddlUniformes").val();
    uniformeColFiltr.Marca = $("#txtMarca").val();
    uniformeColFiltr.SetorId = $("#ddlSetores").val() === "" ? "0" : $("#ddlSetores").val();
    uniformeColFiltr.CentroCustoId = $("#ddlCCustos").val() === "" ? "0" : $("#ddlCCustos").val();
    uniformeColFiltr.Ativos = $("#hdAtivos").val();
    if (($("#dp-inicio").val() !== "") && ($("#dp-fim").val() !== "")) {
        uniformeColFiltr.DataInicial = $("#dp-inicio").val();
        uniformeColFiltr.DataFinal = $("#dp-fim").val();
    }
    
    uniformeColFiltr.TipoRelatorio = "não utilizado ainda";

    return $.toJSON(uniformeColFiltr);
}

function getUniformeColFilterImpressao() {
    var uniformeColFiltr = new Object();
    uniformeColFiltr.UnidadeNegocioId = $("#hdUnidadeNegocio").val();
    uniformeColFiltr.ColaboradorId = $("#hdColaborador").val();
    uniformeColFiltr.TipoUniformeId = $("#hdTipoUniforme").val();
    uniformeColFiltr.UniformeId = $("#hdUniforme").val();
    uniformeColFiltr.Marca = $("#hdMarca").val();
    uniformeColFiltr.SetorId = $("#hdSetor").val();
    uniformeColFiltr.CentroCustoId = $("#hdCentroCusto").val();
    uniformeColFiltr.EstadoUniformes = $("#hdEstado").val();
    uniformeColFiltr.Ativos = $("#hdAtivos").val();
    if (($("#hdDataInicial").val() !== "") && ($("#hdDataFinal").val() !== "")) {
        var dataInicial = $("#hdDataInicial").val();
        var dataFinal = $("#hdDataFinal").val();
        uniformeColFiltr.DataInicial = dataInicial.split(" ")[0];
        uniformeColFiltr.DataFinal = dataFinal.split(" ")[0];
    }
    
    return $.toJSON(uniformeColFiltr);
}

function getUniformeColFilterImpressaoUniformeFalta() {
    var uniformeColFiltr = new Object();

    uniformeColFiltr.UniformeId = $("#hdUniforme").val();
    uniformeColFiltr.DescrUniforme = $("#hdDescrUniforme").val();
    uniformeColFiltr.Marca = $("#hdMarca").val();
    uniformeColFiltr.FabricanteId = "";

    return $.toJSON(uniformeColFiltr);
}