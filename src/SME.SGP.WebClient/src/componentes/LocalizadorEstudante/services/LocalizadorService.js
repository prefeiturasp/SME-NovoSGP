import api from '~/servicos/api';

class LocalizadorService {
  buscarPorNome = async params => {
    return api.post('/v1/historico-escolar/alunos', params);
  };

  buscarPorCodigo = async params => {
    return api.post('/v1/historico-escolar/alunos', params);
  };
}

export default new LocalizadorService();
