function InicializarComponentesRegistro() {
    $("#txtCnpjEmpr").mask("99.999.999/9999-99");
    $("#txtCnpjEmpr").blur(function () {
        if ($("#txtCnpjEmpr").val() !== "") {
            if (!valida_cnpj($("#txtCnpjEmpr").val())) {
                alert("CNPJ Inválido.");
                $("#txtCnpjEmpr").val("");
            }
        }
    });
}