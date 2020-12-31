import api from '~/servicos/api';

class ServicoOcorrencias {
  excluir = parametros => {
    return api.delete('v1/ocorrencias', parametros);
  };
}

export default new ServicoOcorrencias();
