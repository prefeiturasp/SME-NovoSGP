import LoginService from '~/servicos/Paginas/LoginServices';
import { salvarDadosLogin } from '~/redux/modulos/usuario/actions';
import history from '~/servicos/history';
import { URL_HOME, URL_REDEFINIRSENHA } from '~/constantes/url';
import { obterMeusDados } from '~/servicos/Paginas/ServicoUsuario';
import { setMenusPermissoes } from '~/servicos/servico-navegacao';

class LoginHelper {
  constructor(dispatch, redirect) {
    this.dispatch = dispatch;
    this.redirect = redirect;
  }

  acessar = async login => {
    const autenticacao = await LoginService.autenticar(login);

    if (!autenticacao.sucesso) return autenticacao;

    const rf = Number.isInteger(login.usuario * 1) ? login.usuario : '';

    this.dispatch(
      salvarDadosLogin({
        token: autenticacao.dados.token,
        rf,
        usuario: login.UsuarioLogin,
        modificarSenha: autenticacao.dados.modificarSenha,
        perfisUsuario: autenticacao.dados.perfisUsuario,
        possuiPerfilSmeOuDre:
          autenticacao.dados.perfisUsuario.possuiPerfilSmeOuDre,
        possuiPerfilDre: autenticacao.dados.perfisUsuario.possuiPerfilDre,
        possuiPerfilSme: autenticacao.dados.perfisUsuario.possuiPerfilSme,
        ehProfessor: autenticacao.dados.perfisUsuario.ehProfessor,
        ehProfessorCj: autenticacao.dados.perfisUsuario.ehProfessorCj,
        ehProfessorInfantil:
          autenticacao.dados.perfisUsuario.ehProfessorInfantil,
        ehProfessorCjInfantil:
          autenticacao.dados.perfisUsuario.ehProfessorCjInfantil,
        ehProfessorPoa: autenticacao.dados.perfisUsuario.ehProfessorPoa,
        dataHoraExpiracao: autenticacao.dados.dataHoraExpiracao,
      })
    );

    if (autenticacao.dados.modificarSenha) {
      history.push({
        pathname: URL_REDEFINIRSENHA,
        state: {
          rf,
        },
      });
      return { sucesso: false, erroGeral: '' };
    }
    obterMeusDados();
    setMenusPermissoes();

    if (this.redirect) history.push(atob(this.redirect));
    else history.push(URL_HOME);

    return autenticacao;
  };
}

export default LoginHelper;
