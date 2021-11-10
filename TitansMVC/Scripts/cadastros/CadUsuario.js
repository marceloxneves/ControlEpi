function InicializaComponentesUsuario() {
    $("#btnAddPermissao").click(function () {
        $("#inputPermissao").toggle();
    });
}

function getPermissao() {
    var permissao = new Object();

    permissao.IdUsuario = $("#txtId").val();
    permissao.IdPermissao = $("#ddlRoles").val();
    permissao.DescricaoPermissao = $("#ddlRoles option:selected").text();

    return $.toJSON(permissao);
}