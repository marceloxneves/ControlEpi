function InicializarComponentesSetores() {
    //$("#cbSetorAtivo").bootstrapSwitch();

    $("#btnAddEpi").click(function () {
        $("#inputEpi").toggle();
    });

    $("#btnAddUniforme").click(function () {
        $("#inputUniforme").toggle();
    });
}

function getEpi() {
    var epi = new Object();

    epi.SetorId = $("#txtId").val();
    epi.EpiId = $("#ddlEpis").val();
    epi.NomeEpi = $("#ddlEpis option:selected").text();
    epi.ValidadeEmDias = $("#txtValDias").val();

    return $.toJSON(epi);
}

function getUniforme() {
    var uniforme = new Object();

    uniforme.SetorId = $("#txtId").val();
    uniforme.UniformeId = $("#ddlUniformes").val();
    uniforme.NomeUniforme = $("#ddlUniformes option:selected").text();
    uniforme.ValidadeEmDias = $("#txtValDiasUniforme").val();

    return $.toJSON(uniforme);
}