var responderPergunta = {
    Model: {
        CreateModel: function () {
            var usuario = {
                Descricao: $("#txtDescricao").val(),
                IdPergunta: $("#hdnVal").val()
            }
            return usuario;
        }
    },

    View: {
        Init: function () {
            $("#btnSalvar").click(function () {
                if (responderPergunta.Model.CreateModel().Descricao.trim() == "")
                    toastr.error("O campo de resposta é obrigatório");
                else
                    responderPergunta.Controller.SubmitForm();
            });

        },
    },

    Controller: {
        SubmitForm: function () {

            util.Metodos.BloqueioBotoes("#btnSalvar");
            var model = responderPergunta.Model.CreateModel();

            $.ajax({
                url: util.Dados.baseURL + "api/WebUsuario/responderPergunta?IdPergunta=" + model.IdPergunta + "&Desc=" + model.Descricao,
                type: "POST",
                data: model,
                success: function () {
                    toastr.success("Resposta enviada com sucesso");
                    setTimeout(function () { window.location.reload(); }, 3000);
                },
                error: function (a, b, c) {
                    toastr.error(a + " " + b + " " + c);
                }
            });

        }
    }
}

$(document).ready(function () {
    responderPergunta.View.Init();
});