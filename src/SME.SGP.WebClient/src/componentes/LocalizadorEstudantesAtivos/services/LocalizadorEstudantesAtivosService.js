import api from '~/servicos/api';

class LocalizadorEstudantesAtivosService {
  buscarPorNome = async params => {
    return api.post('/v1/estudantes/autocomplete/ativos', params);
  };

  buscarPorCodigo = async params => {
    return api.post('/v1/estudantes/autocomplete/ativos', params);
  };
}

export default new LocalizadorEstudantesAtivosService();
