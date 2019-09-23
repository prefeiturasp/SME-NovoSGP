import LoginService from '~/servicos/Paginas/LoginServices';
import history from '~/servicos/history';
import { URL_HOME, URL_MODIFICARSENHA } from '~/constantes/url';

class LoginHelper {
  validarDados = Login => {
    const Validacao = {
      falha: false,
    };

    const UsuarioNaoInformado =
      !Login.usuario ||
      Login.usuario === '' ||
      typeof Login.usuario === 'undefined';
    const SenhaNaoInformada =
      !Login.senha || Login.senha === '' || typeof Login.senha === 'undefined';

    if (UsuarioNaoInformado) {
      Validacao.erroUsuario = 'Digite seu Usuário';
      Validacao.falha = true;
    }

    if (!UsuarioNaoInformado && Login.usuario.length < 5) {
      Validacao.erroUsuario = 'O usuário deve conter no mínimo 5 caracteres.';
      Validacao.falha = true;
    }

    if (SenhaNaoInformada) {
      Validacao.erroSenha = 'Digite sua Senha';
      Validacao.falha = true;
    }

    if (!SenhaNaoInformada && Login.senha.length < 4) {
      Validacao.erroSenha = 'A senha deve conter no mínimo 4 caracteres.';
      Validacao.falha = true;
    }

    return Validacao;
  };

  acessar = async Login => {
    const validacao = this.validarDados(Login);

    if (validacao.falha)
      return {
        sucesso: false,
        erroGeral:
          'Você precisa informar um usuário e senha para acessar o sistema.',
        erroUsuario: validacao.erroUsuario && validacao.erroUsuario,
        erroSenha: validacao.erroSenha && validacao.erroSenha,
      };

    const Autenticacao = await LoginService.autenticar(Login);

    if (!Autenticacao.sucesso) {
      return Autenticacao;
    }

    //Salvar Dados no Redux

    //Salvar Token na storage

    if (Autenticacao.dados.modificarSenha) history.push(URL_MODIFICARSENHA);
    else history.push(URL_HOME);
  };
}

export default new LoginHelper();
