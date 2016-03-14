var servicosPagina = {
    Model: {
    },

    View: {
        Init: function () {
            servicosPagina.Controller.BuscaCategorias();
            servicosPagina.Controller.BuscaServicos(util.Metodos.getQueryString("categoria"));
            servicosPagina.Controller.TotalServicos(util.Metodos.getQueryString("categoria"));

            $("#ddlCategoria").change(function () {
                var categoria = $("#ddlCategoria").val(),
                    linkDDL = (categoria == 0) ? "Servico/Pagina/1" : "Servico/Pagina/1?categoria=" + categoria;
                window.location.replace(util.Dados.baseURL + linkDDL);
            });
        },

        ListaServicos: function (arrayServicos) {
            var numeroLinha = 0;

            if (arrayServicos.length > 0)
                $.each(arrayServicos, function (index, Servico) {
                    var urlDetalhe = util.Dados.baseURL + "Servico/Detalhes/" + Servico.Id,
                        urlFoto = util.Dados.baseURL + "Content/Uploads/" + Servico.Foto;

                    if (index % 3 == 0 || index == 0) {
                        if (index != 0)
                            numeroLinha++;
                        $(".container-lines").append("<div id='divLin" + numeroLinha + "' class='row'></div>");
                    }

                    $("#divLin" + numeroLinha).append("<div class=\"col-md-4 img-portfolio\"><a href=\"" +
                           urlDetalhe + "\" class=\"thumbnail\">" + "<img class=\"img-responsive img-hover\" style=\"width:360px; height: 247px\"" +
                           "src=\"" + urlFoto + "\"></a><h3>" + "<a href=\"" + urlDetalhe + "\">" +
                           Servico.Titulo + "</a></h3><p>" + Servico.Descricao + "</p></div>");
                });
            else
                $(".container-lines").append("Não há serviços adicionados nesta categoria ;(");
        },

        PaginacaoAtiva: function () {
            $(".pagination li a").each(function (index) {
                if ($(this).html() == $("#hdnPagina").val())
                    $(this).parent().addClass("active");
            });
        },

        ListaCategorias: function (TB_Categoria) {

            $("#ddlCategoria").html("<option value=\"0\">Todas categorias</option>");

            $.each(TB_Categoria, function (index, Categoria) {
                $("#ddlCategoria").append("<option value=\"" + Categoria.Id + "\">" + Categoria.NomeCategoria + "</option>");
            });

            $("option[value=\"" + util.Metodos.getQueryString("categoria") + "\"]").attr("selected", "selected");
        },

        MontaPagination: function (qtdServicos) {
            var pagAtual = parseInt($("#hdnPagina").val()),
                valCategoria = window.location.href.split("?")[1],
                linkCategoria = (valCategoria == undefined) ? "" : ("?" + valCategoria),
                paginas = (qtdServicos % 6 == 0) ? (qtdServicos / 6) : ((qtdServicos / 6) + 1);

            if (pagAtual != 1)
                $(".pagination").append("<li><a id=\"btnVolta\" href=\"" + (pagAtual - 1) + linkCategoria + "\">&laquo;</a></li>");

            for (i = 1; i <= paginas; i++) {
                if (i == pagAtual)
                    $(".pagination").append("<li><a>" + i + "</a></li>");
                else
                    $(".pagination").append("<li><a href=\"" + i + linkCategoria + "\">" + i + "</a></li>");
            }

            if (pagAtual <= paginas - 1)
                $(".pagination").append("<li><a id=\"btnAvanca\" href=\"" + (pagAtual + 1) + linkCategoria + "\">&raquo;</a></li>");

            servicosPagina.View.PaginacaoAtiva();
        }
    },

    Controller: {
        BuscaServicos: function (categoria) {
            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/GetTB_ServicosLista/" +
                    $("#hdnPagina").val() + "?" + "categoria=" + categoria,
                type: "GET",
                dataType: "JSON",
                success: function (data) {
                    servicosPagina.View.ListaServicos(data);
                },
                error: function (a, b, c) {
                    toastr.error(a, b, c);
                }
            });
        },

        BuscaCategorias: function () {
            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/GetTB_Categoria",
                type: "GET",
                dataType: "JSON",
                success: function (data) {
                    servicosPagina.View.ListaCategorias(data);
                },
                error: function (a, b, c) {
                    toastr.error(a, b, c);
                }
            });
        },

        TotalServicos: function (categoria) {
            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/GetTB_ServicosLength/" + categoria,
                type: "GET",
                dataType: "JSON",
                success: function (data) {
                    servicosPagina.View.MontaPagination(data);
                },
                error: function (a, b, c) {
                    toastr.error(a, b, c);
                }
            });
        }
    }
}

$(document).ready(function () {
    servicosPagina.View.Init();
});