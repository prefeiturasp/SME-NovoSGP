import Api from '../api';
import { salvarDadosLogin } from '~/redux/modulos/usuario/actions';
import history from '~/servicos/history';
import { URL_HOME } from '~/constantes/url';
import { obterMeusDados } from '~/servicos/Paginas/ServicoUsuario';
import { setMenusPermissoes } from '~/servicos/servico-navegacao';
import { perfilSelecionado, setarPerfis } from '~/redux/modulos/perfil/actions';
import { store } from '~/redux';
class ServicoRedefinirSenha {
  redefinirSenha = async (redefinirSenhaDto, dispatch) => {
    let formData = new FormData();

    formData.set('token', redefinirSenhaDto.token);
    formData.set('novaSenha', redefinirSenhaDto.novaSenha);

    return Api.post(this._obtenhaUrlSolicitarRecuperacao(), formData)
      .then(res => {
        dispatch(
          salvarDadosLogin({
            token: res.data.token,
            rf: res.data.usuarioRf,
            usuario: res.data.usuarioLogin,
            modificarSenha: res.data.modificarSenha,
            perfisUsuario: res.data.perfisUsuario,
            possuiPerfilSmeOuDre: res.data.perfisUsuario.possuiPerfilSmeOuDre,
            possuiPerfilDre: res.data.perfisUsuario.possuiPerfilDre,
            possuiPerfilSme: res.data.perfisUsuario.possuiPerfilSme,
            ehProfessor: res.data.perfisUsuario.ehProfessor,
            ehProfessorCj: res.data.perfisUsuario.ehProfessorCj,
            ehProfessorPoa: res.data.perfisUsuario.ehProfessorPoa,
            dataHoraExpiracao: res.data.dataHoraExpiracao,
          })
        );

        const { perfis } = res.data.perfisUsuario;
        const selecionado = perfis.find(
          perfil =>
            perfil.codigoPerfil === res.data.perfisUsuario.perfilSelecionado
        );
        store.dispatch(setarPerfis(perfis));
        store.dispatch(perfilSelecionado(selecionado));
        obterMeusDados();
        setMenusPermissoes();

        history.push(URL_HOME);

        return { sucesso: false, erroGeral: '' };
      })
      .catch(err => {
        const response = err.response && err.response;

        if (!response)
          return {
            sucesso: false,
            erro:
              'Ocorreu uma falha na comunicação com o servidor, por favor contate o suporte',
          };

        if (response.status === 403)
          return {
            sucesso: false,
            tokenExpirado: true,
            erro: response.data.mensagens.join(','),
          };

        if (response.data)
          return {
            sucesso: false,
            erro: response.data.mensagens.join(','),
          };

        return {
          sucesso: false,
          erro:
            'Ocorreu uma falha na comunicação com o servidor, por favor contate o suporte',
        };
      });
  };

  validarToken = async token => {
    const requisicao = await Api.get(this._obtenhaUrlValidarToken(token));

    return requisicao.data;
  };

  _obtenhaUrlSolicitarRecuperacao = () => {
    return 'v1/autenticacao/recuperar-senha';
  };

  _obtenhaUrlValidarToken = token => {
    return `v1/autenticacao/valida-token-recuperacao-senha/${token}`;
  };
}

export default new ServicoRedefinirSenha();
