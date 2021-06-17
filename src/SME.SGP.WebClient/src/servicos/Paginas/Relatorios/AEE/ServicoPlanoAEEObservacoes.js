import api from '~/servicos/api';

const urlPadrao = 'v1/plano-aee/observacoes';

class ServicoPlanoAEEObservacoes {
  obterDadosObservacoes = planoAEEId => {
    return api.get(`${urlPadrao}?planoAEEId=${planoAEEId}`);
  };

  salvarEditarObservacao = dados => {
    if (dados?.id) {
      return api.put(urlPadrao, dados);
    }

    return api.post(urlPadrao, dados);
  };

  excluirObservacao = id => {
    return api.delete(`${urlPadrao}/${id}`);
  };
}

export default new ServicoPlanoAEEObservacoes();
