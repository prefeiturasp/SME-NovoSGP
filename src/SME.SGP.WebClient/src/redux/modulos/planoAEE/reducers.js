import produce from 'immer';

const inicial = {
  dadosIniciaisPlanoAEE: {},
  planoAEEDados: {},
  desabilitarCamposPlanoAEE: false,
  planoAEEEmEdicao: false,
  exibirLoaderPlanoAEE: false,
  planoAEESituacaoEncaminhamentoAEE: {},
  planoAEEDadosSecoesPorEtapa: [],
};

export default function PlanoAEE(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@planoAEE/setDadosIniciaisPlanoAEE': {
        return {
          ...draft,
          dadosIniciaisPlanoAEE: action.payload,
        };
      }
      case '@planoAEE/setExibirLoaderPlanoAEE': {
        return {
          ...draft,
          exibirLoaderPlanoAEE: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEEDados': {
        return {
          ...draft,
          planoAEEDados: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEESituacaoEncaminhamentoAEE': {
        return {
          ...draft,
          planoAEESituacaoEncaminhamentoAEE: action.payload,
        };
      }
      case '@planoAEE/setDesabilitarCamposPlanoAEE': {
        return {
          ...draft,
          desabilitarCamposPlanoAEE: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEEEmEdicao': {
        return {
          ...draft,
          planoAEEEmEdicao: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEEDadosSecoesPorEtapa': {
        return {
          ...draft,
          planoAEEDadosSecoesPorEtapa: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEELimparDados': {
        return {
          ...draft,
          dadosIniciaisPlanoAEE: {},
          planoAEESituacaoEncaminhamentoAEE: {},
          planoAEEDados: null,
          exibirLoaderPlanoAEE: false,
          desabilitarCamposPlanoAEE: false,
          planoAEEDadosSecoesPorEtapa: [],
        };
      }
      default:
        return draft;
    }
  });
}
