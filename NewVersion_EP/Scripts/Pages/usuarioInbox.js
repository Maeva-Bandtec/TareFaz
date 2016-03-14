var usuarioInbox = {
    Model: {
    },

    View: {
        Init: function () {
            usuarioInbox.Controller.BuscaUsuarios(sessionStorage["sessaoUser"]);

        },

        ListaMensagens: function (mensagensRecebidas) {
            var resultado,
                dataFormatada;

            if (mensagensRecebidas.length == 0) {
                resultado = "<br><br><div style=\"text-align:center;\"> Que pena, você não tem nenhuma pergunta. Mas relaxa, logo menos aparece alguma :)</div>";
            }
            else {
                resultado = "<br><br><br><table class=\"tabela\"><th><span class=\"glyphicon glyphicon-sort-by-order\" style=\"cursor:pointer;\"></span>"
                    + "</th><th>Servico</th> <th>Pergunta</th> " +
                   "<th>Comprador</th> <th>Data Pergunta</th> <th>Responder</th><th>Excluir</th>";

                $.each(mensagensRecebidas, function (index, mensagem) {
                    dataFormatada = new Date(mensagem.DataPergunta).toLocaleDateString("pt-BR", util.Dados.opcoesData());

                    resultado += "<tr><td>" + (index + 1) + "</td>"
                        + "<td>" + mensagem.Servico + "</td>" + "<td>" + mensagem.Pergunta + "</td>" + "<td>" + mensagem.Comprador + "</td>" +
                        "<td>" + dataFormatada + "</td><td>" +
                       "<a data-toggle=\"modal\" id=\"" + mensagem.IdPergunta + "\" data-target=\"#respostaModal\" href=\"\">Abrir</a></td>"
                    + "<td><a class=\"inativaPergunta\" data-index=\"" + (index + 1) + "\" id=\"" + mensagem.IdPergunta + "\"><span class=\"glyphicon glyphicon-remove\" style=\"cursor:pointer; color:red;\"></span><\a></td>";
                });

            }
            resultado += "</table>";
            $("#box").html(resultado);


            $("a").each(function (index) {
                $(this).click(function () {
                    $("#hdnVal").val($(this).attr("id"));
                    $("#hdnIndex").val($(this).attr("data-index"));
                });
            });

            $(".inativaPergunta").click(function () {
                $(this).confirm({
                    text: "Tem certeza absouluta, mas assim... Absoluta mesmo, que deseja excluir a pergunta " + $("#hdnIndex").val() + " ?",
                    title: "Excluir Pergunta",
                    confirm: function (button) {
                        usuarioInbox.Controller.ExcluirPergunta($("#hdnVal").val());
                    },
                    cancel: function (button) {
                    },
                    confirmButton: "Absolutamente sim",
                    cancelButton: "Não, mudei de ideia",
                    post: true
                });
            });


        }
    },

    Controller: {
        BuscaUsuarios: function (sessao) {
            $.ajax({
                url: util.Dados.baseURL + "api/WebUsuario/MensagensRecebidas?userGUID=" + sessionStorage["sessaoUser"],
                type: "GET",
                dataType: "JSON",
                success: function (data) {
                    if (sessionStorage["sessaoUser"] != "") {
                        usuarioInbox.View.ListaMensagens(data);
                        $(".tabela tr:odd").css("background-color", "rgba(0,0,0,0.08)");
                    }
                    else {
                        toastr.options = { "positionClass": "toast-top-full-width", "showDuration": "500", "timeOut": "90000", "extendedTimeOut": "80000" }
                        toastr.info("", "Você precisa estar logado para acessar está página");
                        window.location.replace(util.Dados.baseURL + "Login");

                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert(xhr, textStatus, errorThrown);
                }
            });
        }
        ,
        ExcluirPergunta: function (idPergunta) {
            $.ajax({
                url: util.Dados.baseURL + "api/WebUsuario/ExcluirPergunta?idPergunta=" + idPergunta,
                type: "GET",
                dataType: "JSON",
                success: function (data) {
                    toastr.info("", "Pergunta excluída com sucesso");
                    window.location.reload();
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert(xhr, textStatus, errorThrown);
                }
            });
        }
    }
}

$(document).ready(function () {
    usuarioInbox.View.Init();
});