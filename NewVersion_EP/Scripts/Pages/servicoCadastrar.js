function criarServico() {
    var servico = {
        categoriaId: $("#ddlCategoria").val(),
        usuarioId: $("#hdnId").val(),
        titulo: $("#txtTituloServico").val(),
        descricao: $("#txtDescricao").val(),
        foto: $("#imagem").val(),
        tempoEntrega: $("#tempoEntrega").val(),
        tag: $("#txtTagServico").val(),
        video: $("#video").val(),
        instrucoes: $("#txtInstrucoes").val(),
        temArquivo: $("#temArquivo").val(),
        dataAtivacao: $("#dataAtivacao").val(),
        isAtivo: true
    }
    return servico;
};

function enviarServico() {
    util.Metodos.getUsuarioIdDoGUID(sessionStorage["sessaoUser"]);
    var model = criarServico(),
        extensao = $("#imagem").val().split(".");

    extensao = (extensao[extensao.length - 1]);

    if (extensao.indexOf("jpg") > -1)
        $.ajax({
            url: util.Dados.baseURL + "API/webservicos/PostTB_Servicos",
            type: "POST",
            data: model,
            success: function (dados) {
                upload(dados[0], dados[1]);
            },
            error: function () {
                toastr.error("Erro");
            }
        });

    else
        toastr.error("As extensões devem ser JPG", "Foi mal...");
};

function upload(idUsuario, idServico) {
    var data = new FormData(),
        files = $("#imagem").get(0).files;

    if (files.length > 0) {
        data.append("arquivo", files[0]);

        util.Metodos.BloqueioBotoes("#Salvar");

        $.ajax({
            type: "POST",
            url: "/api/WebServicos/Post_Foto/" + idUsuario + "?idServico=" + idServico,
            contentType: false,
            processData: false,
            data: data,
            success: function () {
                toastr.success("Pronto pessoa, taca-lhe pau nas vendas!", "Serviço cadastrado");
                setTimeout(function () { window.location.replace(util.Dados.baseURL + "usuario/Opcoes"); }, 3000);
            },
            error: function () {
                toastr.error("Ocorreu um erro");
            }
        });
    }
};

function loadDdl() {
    $.ajax({
        url: util.Dados.baseURL + "API/WebServicos/GetTB_Categoria",
        type: "GET",
        success: function (dados) {
            $.each(dados, function (index, categoria) {
                $("#ddlCategoria").append("<option value=\"" + categoria.Id + "\">" + categoria.NomeCategoria + "</option>");
            });
        },
        error: function () {
            toastr.error("ERRO");
        }
    });

}

$(document).ready(function () {
    $("#btnSalvar").click(function () {
        if (util.Metodos.isDigito("tempoEntrega"))
            enviarServico();
        else
            toastr.error("Meu... põe o valor em números, blz?", "Tempo de entrega");
    });

    loadDdl();

    $("#tempoEntrega").keypress(function (e) {
        if (!(e.keyCode >= 48 && e.keyCode <= 57))
            e.preventDefault();
    });
});