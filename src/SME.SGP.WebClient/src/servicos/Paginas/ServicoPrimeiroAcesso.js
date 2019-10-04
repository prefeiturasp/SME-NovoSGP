import api from '~/servicos/api';

class ServicoPrimeiroAcesso {
  alterarSenha = async dados => {
    api.put('/api/v1/autenticacao/PrimeiroAcesso', dados).then(resposta => {
      return resposta;
    });
  };
}

export default new ServicoPrimeiroAcesso();
