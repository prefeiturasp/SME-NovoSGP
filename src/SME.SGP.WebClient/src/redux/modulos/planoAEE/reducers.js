import produce from 'immer';

const inicial = {
  dadosIniciaisPlanoAEE: {},
  dadosSecaoLocalizarEstudante: {},
  dadosPlanoAEE: null,
  desabilitarCamposPlanoAEE: false,
  planoAEEEmEdicao: false,
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
      case '@encaminhamentoAEE/setDadosSecaoLocalizarEstudante': {
        return {
          ...draft,
          dadosSecaoLocalizarEstudante: action.payload,
        };
      }
      case '@planoAEE/setDadosPlanoAEE': {
        return {
          ...draft,
          dadosPlanoAEE: action.payload,
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
      case '@planoAEE/setLimparDadosPlanoAEE': {
        return {
          ...draft,
          dadosIniciaisPlanoAEE: {},
          dadosSecaoLocalizarEstudante: {},
          dadosPlanoAEE: null,
          desabilitarCamposPlanoAEE: false,
        };
      }
      default:
        return draft;
    }
  });
}
