function criarMensagem() {
    var data = new Date();
    var mensagem = {
        NomeRemetente: $("#nome").val(),
        EmailRemetente: $("#email").val(),
        AssuntoID: $("#ddlAssunto").val(),
        Descricao: $("#mensagem").val(),
        isAtivo: true
    }
    return mensagem;
};

function enviarMensagem() {
    var mensagem = criarMensagem();
    $.ajax({
        url: util.Dados.baseURL + "API/webadmin/PostTB_FaleConosco",
        type: "POST",
        data: mensagem,
        success: function () {
            toastr.success("Mensagem enviada com sucesso");
            $("#nome").val(null);
            $("#email").val(null);
            $("#mensagem").val(null);
        },
        error: function () {
            toastr.error("Erro no envio da mensagem");
        }
    })
};

function loadDdl() {
    $.ajax({
        url: util.Dados.baseURL + "API/webadmin/GetTB_FaleConoscoAssunto",
        type: "GET",
        success: function (dados) {
            $.each(dados, function (index, assunto) {
                $("#ddlAssunto").append("<option value=\"" + assunto.Id + "\">" + assunto.Assunto + "</option>");
            });
        },
        error: function () {
            toastr.error("Não foi possível carregar a lista de assuntos");
        }
    });
}

$(document).ready(function () {
    $("#btnEnviarMensagem").click(function () {
        enviarMensagem();
    });
    loadDdl();
});