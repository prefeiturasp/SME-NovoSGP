import Api from '../api';


class RedefinirSenhaService {

    redefinirSenha = async (redefinirSenhaDto) => {

        var formData = new FormData();
        formData.set('token', redefinirSenhaDto.token);
        formData.set('novaSenha', redefinirSenhaDto.novaSenha)

        return await Api.post(this._obtenhaUrlSolicitarRecuperacao(), formData)
            .then(res => {
                return { sucesso: true };
            }).catch(err => {
                const response = err.response && err.response;

                if (!response)
                    return { sucesso: false, erro: "Ocorreu uma falha na comunicação com o servidor, por favor contate o suporte" }

                if (response.status === 403)
                    return {
                        sucesso: false,
                        tokenExpirado: true,
                        erro: response.data.mensagens.join(",")
                    };

                if (response.data)
                    return {
                        sucesso: false,
                        erro: response.data.mensagens.join(",")
                    };

                return { sucesso: false, erro: "Ocorreu uma falha na comunicação com o servidor, por favor contate o suporte" }

            });

    };

    validarToken = async (token) => {

        const requisicao = await Api.get(this._obtenhaUrlValidarToken(token));

        return requisicao.data;
    }

    _obtenhaUrlSolicitarRecuperacao = () => {
        return "v1/autenticacao/recuperar-senha";
    }

    _obtenhaUrlValidarToken = (token) => {
        return `v1/autenticacao/valida-token-recuperacao-senha/${token}`;
    }
}

export default new RedefinirSenhaService();