import api from '../api';
import { store } from '~/redux';
import { perfilSelecionado, setarPerfis } from '~/redux/modulos/perfil/actions';

class LoginService {
  autenticar = async Login => {
    return await api
      .post(this.obtenhaUrlAutenticacao(), {
        login: Login.usuario,
        senha: Login.senha,
      })
      .then(res => {
        if (res.data && res.data.perfisUsuario) {
          const perfis = res.data.perfisUsuario.perfis;
          const selecionado = perfis.find(
            perfil =>
              perfil.codigoPerfil === res.data.perfisUsuario.perfilSelecionado
          );
          store.dispatch(setarPerfis(perfis));
          store.dispatch(perfilSelecionado(selecionado));
        }
        return {
          sucesso: true,
          mensagem: 'Usuario Logado com sucesso',
          dados: res.data,
        };
      })
      .catch(err => {
        const status = err.response ? err.response.status : null;

        if (status && status === 401)
          return { sucesso: false, erroGeral: 'Usuário e/ou senha invalida' };

        return {
          sucesso: false,
          erroGeral:
            err.data && err.data.mensagens
              ? err.data.mensagens.join(',')
              : 'Falha ao tentar autenticar o servidor',
        };
      });
  };

  obtenhaUrlAutenticacao = () => {
    return 'v1/autenticacao';
  };
}

export default new LoginService();
