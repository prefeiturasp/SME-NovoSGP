import api from '~/servicos/api';

class ServicoOcorrencias {
  excluir = parametros => {
    return api.delete('v1/ocorrencias', parametros);
  };

  buscarTiposOcorrencias = () => {
    return api.get('v1/ocorrencias/tipos');
  };
}

export default new ServicoOcorrencias();
