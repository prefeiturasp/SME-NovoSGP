import api from '../api';

class LoginService {
  autenticar = async Login => {
    return await api
      .post(this.obtenhaUrlAutenticacao(), {
        login: Login.usuario,
        senha: Login.senha,
      })
      .then(res => {
        console.log(res.data);
        return {
          sucesso: true,
          mensagem: 'Usuario Logado com sucesso',
          dados: res.data
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
