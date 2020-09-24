import api from '~/servicos/api';

const urlPadrao = '/v1/carta-intencoes';

class ServicoCartaIntencoes {
  obterBimestres = (turmaCodigo, componenteCurricularId) => {
    const url = `${urlPadrao}/turmas/${turmaCodigo}/componente-curricular/${componenteCurricularId}`;
    return api.get(url);
  };

  salvarEditarCartaIntencoes = dados => {
    return api.post(urlPadrao, dados);
  };

  obterDadosObservacoes = (turmaCodigo, componenteCurricularId) => {
    return api.get(
      `${urlPadrao}/turmas/${turmaCodigo}/componente-curricular/${componenteCurricularId}/observacoes`
    );
  };

  salvarEditarObservacao = (dados, turmaCodigo, componenteCurricularId) => {
    const observacaoId = dados.id;
    if (observacaoId) {
      const url = `${urlPadrao}/observacoes/${observacaoId}`;
      return api.put(url, { observacao: dados.observacao });
    }

    const url = `${urlPadrao}/turmas/${turmaCodigo}/componente-curricular/${componenteCurricularId}/observacoes`;
    return api.post(url, { observacao: dados.observacao });
  };

  excluirObservacao = dados => {
    const observacaoId = dados.id;
    return api.delete(`${urlPadrao}/observacoes/${observacaoId}`);
  };
}

export default new ServicoCartaIntencoes();
