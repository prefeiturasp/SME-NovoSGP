import api from '~/servicos/api';

const TipoAvaliacaoServico = {
  buscarTipoAvaliacao() {
    return api.get('/v1/atividade-avaliativa/tipos/listar');
  },
  deletarTipoAvaliacao(id) {
    return api.delete(`/v1/verdepois/${id}`);
  },
  salvarTipoAvaliacao(){
    
  }
};

export default TipoAvaliacaoServico;
