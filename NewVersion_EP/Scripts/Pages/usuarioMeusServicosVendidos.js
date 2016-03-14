var usuarioMeusServicosVendidos = {
    Model: {
    },

    View: {
        Init: function () {
            usuarioMeusServicosVendidos.Controller.BuscarServicosVendidos();
        },

        ListaServicosVendidos: function (dados) {
            var htmlString = "<table class=\"tabela\" style=\"width: 100%\"><tr style=\"border-bottom: 1px solid black\">" +
                "<th>NUM. PEDIDO</th> <th>NOME SERVIÇO</th> <th>COMPRADOR</th> <th>DATA</th> <th>ARQUIVO</th> <th></th></tr>";

            $.each(dados, function (index, retorno) {
                var dataFormatada = new Date(retorno.DtPedido).toLocaleDateString("pt-BR", util.Dados.opcoesData),
                    temArquivo = retorno.CaminhoArquivo == null ? "<input id=\"arquivo" + retorno.Id + "\" type=\"file\"><td><button class=\"btn btn-success\"" +
                    "data-idVendedor=\"" + retorno.Servico.UsuarioId + "\"" +
                    "data-idComprador=\"" + retorno.Usuario.Id + "\"" +
                    "data-idServico=\"" + retorno.Servico.Id + "\"" +
                    "data-idSV=\"" + retorno.Id + "\"" +
                    "id=\"btnEnviarArq" + retorno.Servico.Id + "\">Enviar</button>" +
                    "</td>" : retorno.CaminhoArquivo;

                htmlString +=
                    "<td>" + retorno.Id + "</td>" +
                    "<td>" + retorno.Servico.Titulo + "</td>" +
                    "<td>" + retorno.Usuario.NomeCompleto + "</td>" +
                    "<td>" + dataFormatada + "</td>" +
                    "<td>" + temArquivo + "</td>" +
                "</tr>";
            });

            htmlString += "</table>";
            $("#container").html(htmlString);

            $(".btn-success").click(function () {
                usuarioMeusServicosVendidos.Controller.EnviarServico(
                    $(this).attr("data-idvendedor"),
                    $(this).attr("data-idcomprador"),
                    $(this).attr("data-idservico"),
                    $(this).attr("data-idsv")
                    );
            });
        }
    },

    Controller: {
        BuscarServicosVendidos: function () {
            var objDados = { id: $("#hdnPagina").val(), userGUID: sessionStorage["sessaoUser"] };

            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/GetTB_ServicosVendidos",
                type: "POST",
                data: objDados,
                success: function (dados) {
                    console.log(dados);
                    usuarioMeusServicosVendidos.View.ListaServicosVendidos(dados);
                },
                error: function () {
                    toastr.error("Error");
                }
            });
        },

        EnviarServico: function (idVendedor, idComprador, idServico, idsv) {
            var data = new FormData();

            var files = $("#arquivo" + idsv).get(0).files;

            if (files.length > 0) {
                data.append("arquivo", files[0]);

                util.Metodos.BloqueioBotoes("#btnEnviarArq");

                $.ajax({
                    type: "POST",
                    url: "/api/WebServicos/Post_Arquivo?idVendedor=" + idVendedor + "&idComprador=" + idComprador + "&idServico=" + idServico,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function () {
                        location.reload();
                    },
                    error: function () {
                        toastr.error("Ocorreu um erro");
                    }
                });
            }
        }
    }
}

$(document).ready(function () {
    usuarioMeusServicosVendidos.View.Init();
});