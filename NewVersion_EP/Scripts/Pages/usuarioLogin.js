var usuarioLogin = {

    Model: {
        createModel: function () {
            var loginModel = {
                Email: $("#Email").val(),
                Senha: $("#Senha").val()
            }
            return loginModel;
        }
    },
    View: {
        init: function () {
            $("#Cancelar").click(function () { usuarioLogin.Controller.Voltar() });
            $("#Entrar").click(function () { usuarioLogin.Controller.Entrar() });
            $("#CriarConta").click(function () { usuarioLogin.Controller.CriarConta() });
            $("#EsqueciSenha").click(function () { usuarioLogin.Controller.EsqueciSenha() });

            $(".keyp").keypress(function (e) {
                if (e.which == 13) {
                    $("#Entrar").trigger("click");
                }
            });
        }

    },

    Controller: {

        EsqueciSenha: function () {

            var model = usuarioLogin.Model.createModel();
            if (model.Email == "") {
                toastr.warning("Digite seu e-mail");
                $("#Email").focus();
                return false;
            } else {
                $.ajax({
                    url: util.Dados.baseURL + 'api/WebUsuario/EsqueciSenha',
                    type: 'POST',
                    dataType: 'JSON',
                    data: model,
                    success: function (data) {
                        if (data) {
                            toastr.info("Verifique seu e-mail", "Senha enviada com sucesso");
                        }
                    },
                    error: function (data) {
                        toastr.error(data.error);
                    }
                })
            }
        },

        Voltar: function () {
            window.history.back();
        },

        CriarConta: function () {
            window.location.replace(util.Dados.baseURL + "usuario/Cadastrar");
        },

        ValidaCampos: function (model) {
            if (model.Email == "" || model.Senha == "") {
                $("#Email").focus();
                toastr.error("Preencha os Campos");
                toastr.warning("Os campos são obrigatórios");
                return false;
            }
            else
                return true;
        },

        Entrar: function () {

            var model = usuarioLogin.Model.createModel();
            if (usuarioLogin.Controller.ValidaCampos(model)) {

                $.ajax({
                    url: util.Dados.baseURL + 'api/WebUsuario/ValidaLogin',
                    type: 'POST',
                    dataType: 'JSON',
                    data: model,
                    success: function (data) {
                        //var numericReg = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
                        //if (!numericReg.test(data)) {
                        if (data.length == 2) {
                            switch (data) {
                                case "NE":
                                    toastr.error('E-mail Inválido');
                                    break;
                                case "SI":
                                    toastr.error('Senha Inválida');
                                    break;
                                case "IN":
                                    toastr.error('Sua conta foi inativada');
                                    toastr.warning('Entre em contato pelo Fale Conosco');
                                    break;
                                case "BQ":
                                    toastr.error('Sua conta foi bloqueada', 'Você errou a senha mais de 5 vezes.')
                                    toastr.warning('Entre em contato pelo Fale Conosco');
                            }
                        }
                        else {
                            window.location.replace(util.Dados.baseURL + "Usuario/Opcoes");

                            var dados = data.split(",");

                            sessionStorage.setItem('sessaoUser', dados[0]);
                            sessionStorage.setItem('sexoUser', dados[1]);
                        }

                    },
                    error: function (data) {
                        toastr.error(data.error);
                    }
                })
            }

        }
    }

};

$(document).ready(function () { usuarioLogin.View.init() });