import LoginService from '~/servicos/Paginas/LoginServices';
import { SalvarDadosLogin } from '~/redux/modulos/usuario/actions';
import { setarPerfis, perfilSelecionado} from '~/redux/modulos/perfil/actions';
import history from '~/servicos/history';
import { URL_HOME, URL_MODIFICARSENHA } from '~/constantes/url';

class LoginHelper {
  constructor(dispatch, redirect) {
    this.dispatch = dispatch;
    this.redirect = redirect;
  }

  validarDados = login => {
    const validacao = {
      falha: false,
    };

    const UsuarioNaoInformado =
      !login.usuario ||
      login.usuario === '' ||
      typeof login.usuario === 'undefined';

    const SenhaNaoInformada =
      !login.senha || login.senha === '' || typeof login.senha === 'undefined';

    if (UsuarioNaoInformado) {
      validacao.erroUsuario = 'Digite seu Usuário';
      validacao.falha = true;
    }

    if (!UsuarioNaoInformado && login.usuario.length < 5) {
      validacao.erroUsuario = 'O usuário deve conter no mínimo 5 caracteres.';
      validacao.falha = true;
    }

    if (SenhaNaoInformada) {
      validacao.erroSenha = 'Digite sua Senha';
      validacao.falha = true;
    }

    if (!SenhaNaoInformada && login.senha.length < 4) {
      validacao.erroSenha = 'A senha deve conter no mínimo 4 caracteres.';
      validacao.falha = true;
    }

    return validacao;
  };

  acessar = async login => {
    const validacao = this.validarDados(login);

    if (validacao.falha)
      return {
        sucesso: false,
        erroGeral:
          'Você precisa informar um usuário e senha para acessar o sistema.',
        erroUsuario: validacao.erroUsuario && validacao.erroUsuario,
        erroSenha: validacao.erroSenha && validacao.erroSenha,
      };
    const autenticacao = await LoginService.autenticar(login);

    if (!autenticacao.sucesso) {
      if(autenticacao.dados && autenticacao.dados.perfisUsuario){
        const perfis = autenticacao.dados.perfisUsuario.perfis;
        const selecionado = perfis.find(perfil => perfil.codigoPerfil === autenticacao.dados.perfisUsuario.perfilSelecionado);
        this.dispatch(setarPerfis(perfis));
        this.dispatch(perfilSelecionado(selecionado));
      }
      return autenticacao;
    }
    
    this.dispatch(SalvarDadosLogin({ token: '7777710', rf: '7777710' }));

    if (autenticacao.dados.modificarSenha) {
      history.push(URL_MODIFICARSENHA);
      return;
    }

    console.log(this.redirect);
    if (this.redirect) history.push(atob(this.redirect));
    else history.push(URL_HOME);

    return autenticacao;
  };
}

export default LoginHelper;
