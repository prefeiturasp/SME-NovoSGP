import api from '~/servicos/api';

const urlPadrao = `/v1/diarios-bordo`;

class ServicoDiarioBordo {
  obterDadosObservacoes = diarioBordoId => {
    return api.get(`${urlPadrao}/${diarioBordoId}/observacoes`);
  };

  salvarEditarObservacao = (diarioBordoId, dados) => {
    const observacaoId = dados.id;
    if (observacaoId) {
      const url = `${urlPadrao}/observacoes/${observacaoId}`;
      return api.put(url, { observacao: dados.observacao });
    }

    const url = `${urlPadrao}/${diarioBordoId}/observacoes`;
    return api.post(url, { observacao: dados.observacao });
  };

  excluirObservacao = dados => {
    const observacaoId = dados.id;
    return api.delete(`${urlPadrao}/observacoes/${observacaoId}`);
  };

  obterDiarioBordo = aulaId => {
    return api.get(`${urlPadrao}/${aulaId}`);
  };

  salvarDiarioBordo = (params, idDiarioBordo) => {
    if (idDiarioBordo) {
      params.id = idDiarioBordo;
      return api.put(urlPadrao, params);
    }
    return api.post(urlPadrao, params);
  };
}

export default new ServicoDiarioBordo();
