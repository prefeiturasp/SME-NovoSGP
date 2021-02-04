import api from '~/servicos/api';

class ServicoLocalizadorFuncionario {
  // TODO Trocar endpoint
  buscarPorNome = async params => {
    return api.post('/v1/funcionarios', params);
  };

  buscarPorCodigo = async params => {
    return api.post('/v1/funcionarios', params);
  };
}

export default new ServicoLocalizadorFuncionario();
