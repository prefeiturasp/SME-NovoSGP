import api from '~/servicos/api';

const TipoAvaliacaoServico = {
  buscarTipoAvaliacao() {
    return api.get('/v1/atividade-avaliativa/tipos/listar');
  },
  deletarTipoAvaliacao(id) {
    return api.delete(`/v1/verdepois/${id}`);
  },
  salvarTipoAvaliacao(tipoAvaliacao) {
    return api.post('/v1/atividade-avaliativa/tipos', tipoAvaliacao);
  },
  buscarTipoAvaliacaoPorId(id) {
    return api.get(`/v1/atividade-avaliativa/tipos/${id}`);
  },
  atualizaTipoAvaliacao(id, tipoAvaliacao) {
    return api.put(`/v1/atividade-avaliativa/tipos/${id}`, tipoAvaliacao);
  },
};

export default TipoAvaliacaoServico;
