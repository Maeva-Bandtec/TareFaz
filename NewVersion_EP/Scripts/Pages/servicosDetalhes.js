var servicosDetalhes = {
    Model: {
        CreateModelPergunta: function () {
            var modelPergunta = {
                ServicoId: $("#hdnIdServico").val(),
                VendedorId: $("#hdnIdVendedor").val(),
                CompradorId: $("#hdnId").val(),
                Pergunta: $("textarea").val(),
                DtPergunta: null,
                Resposta: null,
                DtResposta: null,
                isAtivo: true
            }
            return modelPergunta;
        },

        CreateModelCompra: function () {
            var servicoComprado = {
                ServicoId: $("#hdnRoute").val(),
                CompradorId: $("#hdnId").val(),
                DtPedido: null,
                DtEntrega: null,
                CaminhoArquivo: null,
                isAtivo: true
            }
            return servicoComprado;
        }
    },

    View: {
        Init: function () {
            servicosDetalhes.Controller.BuscaServico();
        },

        ListaDetalhes: function (servico) {
            servico.Tag = servico.Tag != null ? servico.Tag : "Não há tags para este serviço",
            servico.Video = servico.Video != null ? servico.Video : "Não há video para este serviço";

            $(".page-header").append(servico.Titulo);
            $(".col-md-7").append("<img src=\"" + util.Dados.baseURL + "Content/Uploads/" + servico.Foto +
                "\" style=\"width: 600px; height: 300px\" class=\"thumbnail\" />");
            $(".desc").append(servico.Descricao);
            $("#ulInfos").append("<li><b>Categoria:</b> " + servico.Categoria.NomeCategoria + "</li>" +
                "<li><b>Entrega em:</b> " + servico.TempoEntrega + " dias</li>" +
                "<li><b>Instruções:</b> " + servico.Instrucoes + "</li>" +
                "<li><b>Vendedor:</b> " + servico.Usuario.NomeCompleto + "</li>" +
                "<li><b>Video:</b> " + servico.Video + "</li>" +
                "<li><b>Tags:</b> " + servico.Tag + "</li>" +
                "<input id=\"hdnIdServico\" type=\"hidden\" value=\"" + servico.Id + "\"</input>" +
                "<input id=\"hdnIdVendedor\" type=\"hidden\" value=\"" + servico.UsuarioId + "\"</input>");

            servicosDetalhes.View.MontaPagina(servico.PerguntasVendedor);
        },

        MontaPagina: function (arrayPerguntas) {
            $(".col-md-5").append("<br><button id=\"btnEscrever\" type=\"button\" class=\"btn btn-warning btndinamico\">Perguntar <span class=\"glyphicon glyphicon-pencil\"></span></input>");
            $(".col-md-5").append(" <button id=\"btnCarrinho\" type=\"button\" class=\"btn btn-primary btndinamico\">Adicionar <span class=\"glyphicon glyphicon-shopping-cart\"></span></input>");
            $(".col-md-5").append(" <button id=\"btnComprar\" type=\"button\" class=\"btn btn-success btndinamico\">Comprar <span class=\"glyphicon glyphicon-usd\"></span></input>");

            servicosDetalhes.View.MontaPerguntas(arrayPerguntas);

            if (sessionStorage["sessaoUser"] != null)
                servicosDetalhes.View.MontaFuncoesBtn();
            else
                $(".btndinamico").attr("disabled", "");
        },

        MontaPerguntas: function (arrayPerguntas) {
            var isAberto = false;

            $("#fldPerguntas").append("<div id=\"divPerguntas\"></div>");

            if (arrayPerguntas.length > 0) {
                $.each(arrayPerguntas, function (index, pergunta) {
                    if (pergunta.DtResposta != null && pergunta.isAtivo != false) {
                        var dataResposta = new Date(pergunta.DtResposta).toLocaleDateString("pt-BR", util.Dados.opcoesData());
                        var dataPergunta = new Date(pergunta.DtPergunta).toLocaleDateString("pt-BR", util.Dados.opcoesData());

                        $("#divPerguntas").append(
                        //container
                        "<div style=\"background-color: rgba(0,0,0,0.08);" +
                        "border-radius: 6px 6px 6px 6px; border: 1.8px dashed rgba(0,0,0,0.1); padding: 0.5em\">" +

                        //header
                        "<div id=\"titulobox" + (index + 1) + "\">" +
                        "<p style=\"border-bottom: 1.5px dashed rgba(0,0,0,0.1)\">" +
                        "<span class=\"glyphicon glyphicon-question-sign\"></span> Pergunta #" + (index + 1) +
                        "&nbsp;&nbsp;<span class=\"glyphicon glyphicon-chevron-down\"></span></p></div>" +

                        //conteudo
                        "<div id=\"conteudobox" + (index + 1) + "\" class=\"expandivel\"><span style=\"font-size: 18pt\">" +
                        pergunta.Pergunta + "</span>" + "<p><span style=\"font-size: 10pt\">" +
                        pergunta.Usuario.NomeCompleto + "</span> - <span style=\"font-size: 10pt\">" + dataPergunta +
                        "</span></p><div style=\"background-color: rgba(0,255,0,0.25);" +
                        "border-radius: 6px 6px 6px 6px; padding: 0.2em\"><p style=\"border-bottom: 1.5px dashed rgba(0,0,0,0.1)\">" +
                        "<span class=\"glyphicon glyphicon-ok\">" + "</span> Resposta</p>" + pergunta.Resposta +
                        "<br><span style=\"font-size: 10pt\">" + dataResposta + "</span></div></div></div><br>");

                        //função expand-collapse
                        $("#titulobox" + (index + 1)).click(function () {
                            $("#conteudobox" + (index + 1)).slideToggle();
                        });
                    }
                });

                $(".expandivel").hide();
            }

            else
                $("#divPerguntas").append("Ainda não há perguntas para este serviço! Faça a sua!");

            $("#icoPerguntas").click(function () {
                if (isAberto) {
                    $("#icoPerguntas").removeClass("glyphicon-chevron-down");
                    $("#icoPerguntas").addClass("glyphicon-chevron-up");
                    isAberto = false;
                }
                else {
                    $("#icoPerguntas").removeClass("glyphicon-chevron-up");
                    $("#icoPerguntas").addClass("glyphicon-chevron-down");
                    isAberto = true;
                }

                $("#divPerguntas").slideToggle();
            });

            $("#divPerguntas").hide();

            $("#fldPerguntas").after("<div id=\"divEscrever\"></div>");
        },

        MontaFuncoesBtn: function () {
            $("#divEscrever").html("<textarea style=\"display:inline-block; width: 100%\" class=\"form-control\" placeholder=\"Escreva aqui a sua pergunta...\" />" +
       "<br><button id=\"btnEnviar\" class=\"btn btn-default\" input=\"button\">Enviar <span class=\"glyphicon glyphicon-share-alt\"></span></button>");

            $("#btnEnviar").click(function () {
                if ($("textarea").val().trim() != "") {
                    servicosDetalhes.Controller.EnviaPergunta();
                    $("textarea").val("");
                    $("#divEscrever").hide("slow");
                }
            });

            $("#divEscrever").hide();

            $("#btnEscrever").click(function () {
                $("#divEscrever").toggle("slow");
            });

            var count = 0;
            $("#btnCarrinho").click(function () {
                if (count == 0) {
                    toastr.info("Serviço adicionado ao carrinho");
                    count++;
                }
                else
                    toastr.warning("Serviço já está no carrinho");
            });

            $("#btnComprar").click(function () {
                servicosDetalhes.Controller.SalvarCompra();
            });
        }
    },

    Controller: {
        BuscaServico: function () {
            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/GetTB_Servicos/" + $("#hdnRoute").val(),
                type: "GET",
                success: function (servico) {
                    servicosDetalhes.View.ListaDetalhes(servico);
                },
                error: function (xml, obj, error) {
                    toastr.error(error);
                }
            });
        },

        EnviaPergunta: function () {
            util.Metodos.getUsuarioIdDoGUID(sessionStorage["sessaoUser"]);
            var model = servicosDetalhes.Model.CreateModelPergunta();

            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/PostTB_PerguntasVendedor",
                type: "POST",
                data: model,
                success: function () {
                    toastr.success("Pergunta enviada, aguarde pela resposta");
                },
                error: function () {
                    toastr.error("Ocorreu um erro ao enviar a pergunta");
                }
            });
        },

        SalvarCompra: function () {
            util.Metodos.getUsuarioIdDoGUID(sessionStorage["sessaoUser"]);
            var model = servicosDetalhes.Model.CreateModelCompra();
            util.Metodos.BloqueioBotoes("#btnEscrever", "#btnComprar", "#btnCarrinho", "#btnEnviar");

            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/PostTB_ServicosVendidos",
                type: "POST",
                data: model,
                success: function () {
                    toastr.success("Serviço comprado");
                },
                error: function () {
                    toastr.error("Erro");
                }
            });
        }
    }
}

$(document).ready(function () {
    servicosDetalhes.View.Init();
});