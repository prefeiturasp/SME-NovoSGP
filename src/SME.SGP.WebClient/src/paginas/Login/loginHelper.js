import LoginService from '~/servicos/Paginas/LoginServices';
import { SalvarDadosLogin } from '~/redux/modulos/usuario/actions';
import { setarPerfis, perfilSelecionado } from '~/redux/modulos/perfil/actions';
import history from '~/servicos/history';
import { URL_HOME, URL_MODIFICARSENHA } from '~/constantes/url';

class LoginHelper {
  constructor(dispatch, redirect) {
    this.dispatch = dispatch;
    this.redirect = redirect;
  }

  acessar = async login => {
    const autenticacao = await LoginService.autenticar(login);

    if (!autenticacao.sucesso) {
      return autenticacao;
    }

    console.log(autenticacao);

    const RF = Number.isInteger(login.usuario * 1) ? login.usuario : '';

    this.dispatch(
      SalvarDadosLogin({
        token: autenticacao.dados.token,
        rf: RF,
        perfisUsuario: autenticacao.dados.PerfisUsuario,
      })
    );

    if (autenticacao.dados.modificarSenha) {
      history.push(URL_MODIFICARSENHA);
      return { sucesso: false, erroGeral: '' };
    }

    if (this.redirect) history.push(atob(this.redirect));
    else history.push(URL_HOME);

    return autenticacao;
  };
}

export default LoginHelper;
