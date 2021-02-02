import produce from 'immer';

const inicial = {
  expandirLinhaAusenciaEstudante: [],
  dadosModalAnotacao: null,
  exibirModalAnotacao: false,
  exibirModalAviso: false,
  dadosSecoesPorEtapaDeEncaminhamentoAEE: [],
  encaminhamentoAEEEmEdicao: false,
  exibirLoaderEncaminhamentoAEE: false,
  dadosEncaminhamento: null,
  exibirModalErrosEncaminhamento: false,
  exibirModalEncerramentoEncaminhamentoAEE: false,
  desabilitarCamposEncaminhamentoAEE: false,
};

export default function EncaminhamentoAEE(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@encaminhamentoAEE/setExpandirLinhaAusenciaEstudante': {
        return {
          ...draft,
          expandirLinhaAusenciaEstudante: action.payload,
        };
      }
      case '@encaminhamentoAEE/setDadosModalAnotacao': {
        return {
          ...draft,
          dadosModalAnotacao: action.payload,
        };
      }
      case '@encaminhamentoAEE/setExibirModalAnotacao': {
        return {
          ...draft,
          exibirModalAnotacao: action.payload,
        };
      }
      case '@encaminhamentoAEE/setExibirModalAviso': {
        return {
          ...draft,
          exibirModalAviso: action.payload,
        };
      }
      case '@encaminhamentoAEE/setDadosModalAviso': {
        return {
          ...draft,
          dadosModalAviso: action.payload,
        };
      }

      case '@encaminhamentoAEE/setDadosSecoesPorEtapaDeEncaminhamentoAEE': {
        return {
          ...draft,
          dadosSecoesPorEtapaDeEncaminhamentoAEE: action.payload,
        };
      }
      case '@encaminhamentoAEE/setEncaminhamentoAEEEmEdicao': {
        return {
          ...draft,
          encaminhamentoAEEEmEdicao: action.payload,
        };
      }
      case '@encaminhamentoAEE/setExibirLoaderEncaminhamentoAEE': {
        return {
          ...draft,
          exibirLoaderEncaminhamentoAEE: action.payload,
        };
      }
      case '@encaminhamentoAEE/setDadosEncaminhamento': {
        return {
          ...draft,
          dadosEncaminhamento: action.payload,
        };
      }
      case '@encaminhamentoAEE/setExibirModalErrosEncaminhamento': {
        return {
          ...draft,
          exibirModalErrosEncaminhamento: action.payload,
        };
      }
      case '@encaminhamentoAEE/setExibirModalEncerramentoEncaminhamentoAEE': {
        return {
          ...draft,
          exibirModalEncerramentoEncaminhamentoAEE: action.payload,
        };
      }
      case '@encaminhamentoAEE/setDesabilitarCamposEncaminhamentoAEE': {
        return {
          ...draft,
          desabilitarCamposEncaminhamentoAEE: action.payload,
        };
      }
      case '@encaminhamentoAEE/setLimparDadosEncaminhamento': {
        return {
          ...draft,
          dadosSecoesPorEtapaDeEncaminhamentoAEE: [],
          encaminhamentoAEEEmEdicao: false,
          exibirLoaderEncaminhamentoAEE: false,
          dadosEncaminhamento: null,
          exibirModalErrosEncaminhamento: false,
          exibirModalEncerramentoEncaminhamentoAEE: false,
        };
      }

      default:
        return draft;
    }
  });
}
