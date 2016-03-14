var usuarioMeusServicos = {
    Model: {
    },

    View: {
        Init: function () {
            usuarioMeusServicos.Controller.BuscaMeusServicos();

            $("#btnSalvar").click(function () {
                usuarioMeusServicos.Controller.AtualizaMeusServicos();
            });

            $("#btnAdd").click(function () {
                window.location.replace(util.Dados.baseURL + "Servico/Cadastrar");
            });
        },

        ListaMeusServicos: function (dados) {
            var htmlString = "<table class=\"tabela\" style=\"width: 100%\"><tr style=\"border-bottom: 1px solid black\">" +
                "<th>SERVIÇO</th> <th>VENDI</th> <th>ENTREGUEI</th> <th>FALTAM</th> <th>CHECAR</th> <th>ATIVO</th> <th></th></tr>";

            $.each(dados, function (index, meuServico) {
                var dataFormatada = new Date(meuServico.DtPedido).toLocaleDateString("pt-BR", util.Dados.opcoesData),
                    visualizar = meuServico.QtdPendentes > 0 ?
                    "<a href=\"" + util.Dados.baseURL + "Usuario/MeusServicosVendidos/" + meuServico.Id + "?userGUID=" + sessionStorage["sessaoUser"] +
                    "\"><span style=\"cursor: pointer\" class=\"glyphicon glyphicon-eye-open\"></span></a>" :
                    "<span style=\"cursor: not-allowed\" class=\"glyphicon glyphicon-eye-close\"></span>",
                    valCheck = "",
                    valDisabled = "";

                if (meuServico.IsAtivo)
                    valCheck = "checked";
                else
                    valDisabled = "disabled";

                htmlString +=
                    "<td>" + meuServico.Titulo + "</td>" +
                    "<td>" + meuServico.QtdVendidos + "</td>" +
                    "<td>" + meuServico.QtdEntregues + "</td>" +
                    "<td>" + meuServico.QtdPendentes + "</td>" +
                    "<td>" + visualizar + "</td>" +
                "<td><input id=\"chk" + meuServico.Id + "\" type=\"checkbox\"" +
                "data-idServico=\"" + meuServico.Id + "\" data-init=\"" +
                meuServico.IsAtivo + "\" style=\"display: none\" " + valCheck +
                "></td></tr>";
            });

            htmlString += "</table>";
            $("#containerServicos").html(htmlString);

            $("input[type=\"checkbox\"]").each(function () {
                $(this).wrap("<span class=\"chkCustom\"></span>");

                if ($(this).is(":checked"))
                    $(this).parent().addClass("chkTrue");
            });

            $(".chkCustom").click(function () {
                var inputChk = $(this).find("input"),
                    isInputChk = inputChk.is(":checked");

                $(this).toggleClass("chkTrue");

                if (isInputChk)
                    inputChk.prop("checked", false);
                else
                    inputChk.prop("checked", true);
            });
        }
    },

    Controller: {
        BuscaMeusServicos: function () {
            var userGUID = { "": sessionStorage["sessaoUser"] };

            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/Get_MeusServicos",
                type: "POST",
                data: userGUID,
                success: function (dados) {
                    if (dados.length > 0)
                        usuarioMeusServicos.View.ListaMeusServicos(dados);
                    else {
                        $("#btnSalvar").hide();
                        $("#containerServicos").html("Você ainda não tem serviços! bora lá cadastrar uns?");
                    }
                },
                error: function () {
                    toastr.error("Erro");
                }
            });
        },

        UploadArquivo: function () {
            var data = new FormData();

            var files = $("#up1").get(0).files;

            if (files.length > 0) {
                data.append("arquivo", files[0]);

                util.Metodos.BloqueioBotoes("#btnSalvar");

                $.ajax({
                    type: "POST",
                    url: "/api/WebServicos/Post_Foto",
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function () {
                        usuarioMeusServicos.Controller.AtualizaMeusServicos();
                    },
                    error: function () {
                        toastr.error("Ocorreu um erro");
                    }
                });
            }
        },

        AtualizaMeusServicos: function () {
            var vetorIds = new Array();

            $("[type=checkbox]").each(function () {
                if ($(this).is(":checked").toString() != $(this).attr("data-init"))
                    vetorIds[vetorIds.length] = $(this).attr("data-idservico");
            });

            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/AtualizaStatusServicos",
                type: "POST",
                data: { "": vetorIds },
                success: function () {
                    toastr.success("Dados atualizados", "Sr vendedor, taca-lhe pau");
                },
                error: function () {
                    toastr.error("Erro");
                }
            });
        }
    }
}

$(document).ready(function () {
    usuarioMeusServicos.View.Init();
});