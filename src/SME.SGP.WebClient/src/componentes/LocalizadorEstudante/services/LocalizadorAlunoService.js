import api from '~/servicos/api';

class LocalizadorAlunoService {
  buscarPorNomeAluno = async params => {
    return api.post('/v1/estudante/pesquisa', params);
  };

  buscarPorCodigoAluno = async params => {
    return api.post('/v1/estudante/pesquisa', params);
  };

  buscarPorCodigoTurma = async params => {
    return api.post('/v1/estudante/pesquisa', params);
  };
}

export default new LocalizadorAlunoService();
