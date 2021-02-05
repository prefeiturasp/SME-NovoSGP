import api from '~/servicos/api';

class ServicoLocalizadorFuncionario {
  buscarPorNome = async params => {
    return api.post('/v1/funcionarios/pesquisa', params);
  };

  buscarPorCodigo = async params => {
    return api.post('/v1/funcionarios/pesquisa', params);
  };
}

export default new ServicoLocalizadorFuncionario();
