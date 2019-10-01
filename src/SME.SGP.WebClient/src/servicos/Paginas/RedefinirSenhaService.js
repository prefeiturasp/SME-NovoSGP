import Api from '../api';


class RedefinirSenhaService {

    redefinirSenha = async () => {

        Api
            .post('/v1/autenticacao/TROCAR PARA RECUPERAR', {
                usuario: 'string',
                rfcpf: 'string',
                usuarioExterno: 'boolean',
                novaSenha: 'string',
                confirmarSenha: 'string',
            })
            .then(res => {
                if (res.data) {
                    return res.data;
                }
                return false;
            });

    };

    validarToken = async (token) => {

        const requisicao = await Api.get(this._obtenhaUrlValidarToken(token));

        return requisicao.data;
    }

    _obtenhaUrlSolicitarRecuperacao = () => {
        return "v1/autenticacao/solicitar-recuperacao-senha";
    }

    _obtenhaUrlValidarToken = (token) => {
        return `v1/autenticacao/valida-token-recuperacao-senha/${token}`;
    }
}

export default new RedefinirSenhaService();