var usuarioCadastrar = {
    Model: {
        CreateModel: function () {
            var usuario = {
                NomeCompleto: $("#txtNome").val(),
                Telefone: ($("#txtDDD").val() + $("#txtTelefone").val()).trim(),
                Email: $("#txtEmail").val().trim(),
                Senha: $("#txtSenha").val().trim(),
                SexoId: $("#ddlSexo").val(),
                isAtivo: true
            }
            return usuario;
        }
    },

    View: {
        Init: function () {
            $("#btnSalvar").click(function () {
                usuarioCadastrar.Controller.SubmitForm();
            });

            $("#txtEmail").change(function () {
                usuarioCadastrar.Controller.isEmailRepetido();
            });

            $("txtConfSenha").change(function () {
                usuarioCadastrar.View.isSenhaIgual();
            });

            usuarioCadastrar.View.carregaDrop();
        },

        carregaDrop: function () {
            $.ajax({
                url: util.Dados.baseURL + "api/WebUsuario/GetTB_Sexo",
                type: "GET",
                success: function (data) {
                    $.each(data, function (index, sexo) {
                        $("#ddlSexo").append("<option value=\"" + sexo.Id + "\">" + sexo.Descricao + "</option>");
                    });
                },
                error: function () {
                    toastr.error("Erro ao carregar dropdown");
                }
            });
        },

        isSenhaIgual: function () {
            var senha = $("#txtSenha").val(),
                senhaConf = $("#txtConfSenha").val();

            if (senha != senhaConf) {
                toastr.error("As senhas diferem");
                return false;
            }
            return true;
        },

        isEmailVazio: function () {
            var email = $("#txtEmail").val().trim();

            if (!email) {
                $("#icoOK").slideUp("slow");
                $("#MensagemErro").html("");
                $("#txtEmail").attr("class", "form-control");
                return true;
            }
            return false;
        },

        isEmailValido: function () {

            if (!usuarioCadastrar.View.isEmailVazio()) {
                if (!util.Metodos.isEmail("txtEmail")) {
                    $("#icoOK").slideDown("slow");
                    $("#icoOK").attr("class", "glyphicon glyphicon-remove vermelho");
                    $("#MensagemErro").html("Email inválido!");
                    $("#txtEmail").attr("class", "form-control sombraVermelho");
                    $("#btnSalvar").attr("disabled", "");
                    return false;
                }
                return true;
            }
            return false;
        },

        isTelefoneValido: function () {
            if (!($("#txtDDD").val().length < 2 || $("#txtTelefone").val().length < 8)) {
                if (!(util.Metodos.isDigito("txtDDD") && util.Metodos.isDigito("txtTelefone"))) {
                    toastr.error("Válidos somentes dígitos, campeão!", "Telefone/DDD");
                    return false;
                }
                return true;
            }
            toastr.error("Telefone ié-ié não rola", "Telefone/DDD");
            return false;
        }
    },

    Controller: {
        ValidaForm: function () {
            $.each($(".vldvl"), function (index, elemento) {
                if ($(elemento).val().trim() == "") {
                    toastr.error("Faltou esse campo aqui ó... ", elemento.attributes[1].value);
                    return false;
                }
            });

            if (usuarioCadastrar.View.isSenhaIgual() && usuarioCadastrar.View.isTelefoneValido())
                return true;
            return false;
        },

        SubmitForm: function () {
            var model = usuarioCadastrar.Model.CreateModel();

            if (usuarioCadastrar.Controller.ValidaForm()) {
                $("#btnSalvar").val("Salvando...");
                util.Metodos.BloqueioBotoes("#btnSalvar", "#btnCancelar");

                $.ajax({
                    url: util.Dados.baseURL + "api/WebUsuario/PostTB_Usuario",
                    type: "POST",
                    data: model,
                    success: function () {
                        toastr.success("Usuario cadastrado com sucesso");
                        debugger;
                        setTimeout(function () { window.location.replace(util.Dados.baseURL + "Login"); }, 2000);
                    },
                    error: function (a, b, c) {
                        $("#btnSalvar").removeAttr("disabled");
                        $("#btnCancelar").removeAttr("disabled");
                        toastr.error(a + " " + b + " " + c);
                    }
                });
            }
        },

        isEmailRepetido: function () {
            var email = { "": $("#txtEmail").val().trim() };

            $.ajax({
                url: util.Dados.baseURL + "api/WebUsuario/PostEmail",
                type: "POST",
                data: email,
                success: function (r) {
                    $("#icoOK").slideDown("slow");
                    if (r == true) {
                        $("#icoOK").attr("class", "glyphicon glyphicon-remove vermelho");
                        $("#MensagemErro").html("Email existente!");
                        $("#txtEmail").attr("class", "form-control sombraVermelho");
                        $("#btnSalvar").attr("disabled", "");
                        return r;
                    }
                    else {
                        $("#icoOK").attr("class", "glyphicon glyphicon-ok verde");
                        $("#MensagemErro").html("Email disponível!");
                        $("#txtEmail").attr("class", "form-control sombraVerde");
                        $("#btnSalvar").removeAttr("disabled");
                        return r;
                    }
                },
                error: function (a, b, c) {
                    toastr.error(a + " " + b + " " + c);
                }
            });
        }
    }
}

$(document).ready(function () {
    usuarioCadastrar.View.Init();
});