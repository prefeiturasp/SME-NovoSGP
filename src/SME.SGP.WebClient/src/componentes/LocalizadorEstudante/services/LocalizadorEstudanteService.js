import api from '~/servicos/api';

class LocalizadorEstudanteService {
  buscarPorNome = async params => {
    return api.post('/v1/estudante/pesquisa', params);
  };

  buscarPorCodigo = async params => {
    return api.post('/v1/estudante/pesquisa', params);
  };
}

export default new LocalizadorEstudanteService();
