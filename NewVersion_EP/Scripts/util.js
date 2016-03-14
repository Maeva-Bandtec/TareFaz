var util = {
    Dados: {
        baseURL: window.location.protocol + "//" + window.location.host + "/",
        opcoesData: function () {
            var opcoes = {
                year: "numeric",
                month: "short",
                day: "numeric",
                hour: "2-digit",
                minute: "2-digit"
            }
            return opcoes;
        }
    },

    Metodos: {
        isEmail: function (idInput) {
            var padrao = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
            return padrao.test($("#" + idInput).val());
        },

        isDigito: function (idInput) {
            var padrao = /^\d+$/;
            return padrao.test($("#" + idInput).val());
        },

        getQueryString: function (variable) {
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (pair[0] == variable) { return pair[1]; }
            }
            return (false);
        },

        BloqueioBotoes: function () {
            for (var i = 0; i < arguments.length; i++) {
                $(arguments[i]).attr("disabled", "");
            }

            setTimeout(function () { $(".btndinamico").removeAttr("disabled"); }, 5000);
        },

        getUsuarioIdDoGUID: function (guid) {
            var userGUID = { "": guid };

            $.ajax({
                url: util.Dados.baseURL + "api/WebServicos/GuidRetornaIdUsuario",
                async: false,
                type: "POST",
                data: userGUID,
                success: function (dados) {
                    $("#hdnId").val(dados);
                },
                error: function () {
                    toastr.error("Erro ao buscar ID", "Poxa poxinha...");
                }
            });
        },

    }
}
