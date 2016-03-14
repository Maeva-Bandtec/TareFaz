var usuarioIndex = {
    Model: {
    },

    View: {
        Init: function () {
            usuarioIndex.Controller.BuscaUsuarios();
        },

        ListaUsuarios: function (TB_Usuarios) {
            var resultado = "<table class=\"tabela\"><th>ID</th> <th>GUID</th> <th>NOME</th>" +
                "<th>TELEFONE</th> <th>EMAIL</th> <th>SENHA</th> <th>DATA CRIAÇÃO</th>" +
                "<th>DATA ALTERAÇÃO</th> <th>DATA ÚLTIMO ACESSO</th> <th>QTD ERROS</th>" +
                "<th>BLOQUEADO?</th> <th>ATIVO?</th>";

            $.each(TB_Usuarios, function (index, TB_Usuario) {
                resultado += "<tr><td>" + TB_Usuario.Id + "</td>" +
                    "<td>" + TB_Usuario.UserGuid + "</td>" +
                    "<td>" + TB_Usuario.NomeCompleto + "</td>" +
                    "<td>" + TB_Usuario.Telefone + "</td>" +
                    "<td>" + TB_Usuario.Email + "</td>" +
                    "<td>" + TB_Usuario.Senha + "</td>" +
                    "<td>" + TB_Usuario.DtCriacao + "</td>" +
                    "<td>" + TB_Usuario.DtAlteracao + "</td>" +
                    "<td>" + TB_Usuario.DtUltimoAcesso + "</td>" +
                    "<td>" + TB_Usuario.ContaErro + "</td>" +
                    "<td>" + TB_Usuario.isLocked + "</td>" +
                    "<td>" + TB_Usuario.isAtivo + "</td></tr>";
            });

            resultado += "</table>";
            $("#box").html(resultado);
        }
    },

    Controller: {
        BuscaUsuarios: function () {
            $.ajax({
                url: util.Dados.baseURL + "api/WebUsuario/GetTB_Usuario",
                type: "GET",
                dataType: "JSON",
                success: function (data) {
                    usuarioIndex.View.ListaUsuarios(data);
                    $(".tabela tr:odd").css("background-color", "rgba(0,0,0,0.08)");
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert(xhr, textStatus, errorThrown);
                }
            });
        }
    }
}

$(document).ready(function () {
    usuarioIndex.View.Init();
});