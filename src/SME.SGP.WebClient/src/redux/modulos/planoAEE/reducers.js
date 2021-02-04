import produce from 'immer';

const inicial = {
  dadosIniciaisPlanoAEE: {},
  planoAEEDados: {
    encaminhamento: {
      encaminhamentoId: 1,
      situacao: 'Aguardando validação CP',
    },
  },
  desabilitarCamposPlanoAEE: false,
  planoAEEEmEdicao: false,
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
      case '@planoAEE/setPlanoAEEDados': {
        return {
          ...draft,
          planoAEEDados: action.payload,
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
          planoAEEDados: null,
          desabilitarCamposPlanoAEE: false,
          planoAEEDadosSecoesPorEtapa: [],
        };
      }
      default:
        return draft;
    }
  });
}
