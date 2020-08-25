import { store } from '~/redux';
import { setDadosObservacoesUsuario } from '~/redux/modulos/observacoesUsuario/actions';
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

  atualizarSalvarEditarDadosObservacao = (dados, dadosAposSalvar) => {
    const { dispatch } = store;
    const state = store.getState();

    const { observacoesUsuario } = state;
    const { dadosObservacoes } = observacoesUsuario;

    const observacaoId = dados.id;

    if (observacaoId) {
      const item = dadosObservacoes.find(e => e.id === dados.id);
      const index = dadosObservacoes.indexOf(item);
      dados.auditoria = dadosAposSalvar;
      dadosObservacoes[index] = { ...dados };
      dispatch(setDadosObservacoesUsuario([...dadosObservacoes]));
    } else {
      const dadosObs = dadosObservacoes;
      const params = {
        proprietario: true,
        observacao: dados.observacao,
        id: dadosAposSalvar.id,
        auditoria: dadosAposSalvar,
      };
      dadosObs.unshift(params);
      dispatch(setDadosObservacoesUsuario([...dadosObs]));
    }
  };

  excluirObservacao = dados => {
    const observacaoId = dados.id;
    return api.delete(`${urlPadrao}/observacoes/${observacaoId}`);
  };

  atualizarExcluirDadosObservacao = dados => {
    const { dispatch } = store;
    const state = store.getState();

    const { observacoesUsuario } = state;
    const { dadosObservacoes } = observacoesUsuario;

    const item = dadosObservacoes.find(e => e.id === dados.id);
    const index = dadosObservacoes.indexOf(item);
    dadosObservacoes.splice(index, 1);

    dispatch(setDadosObservacoesUsuario([...dadosObservacoes]));
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

  obterPlanejamentosPorIntervalo = (
    turmaCodigo,
    componenteCurricularId,
    dataInicio,
    dataFim,
    numeroPagina
  ) => {
    const url = `${urlPadrao}/turmas/${turmaCodigo}/componentes-curriculares/${componenteCurricularId}/inicio/${dataInicio}/fim/${dataFim}?numeroPagina=${numeroPagina ||
      1}&NumeroRegistros=4`;
    return api.get(url);
  };

  obterPlanejamentosPorDevolutiva = (idDevolutiva, numeroPagina) => {
    const url = `${urlPadrao}/devolutivas/${idDevolutiva}?numeroPagina=${numeroPagina ||
      1}`;
    return api.get(url);
  };
}

export default new ServicoDiarioBordo();
