import produce from 'immer';

const inicial = {
  expandirLinhaAusenciaEstudante: [],
  dadosModalAnotacao: null,
  exibirModalAnotacao: false,
  exibirModalAviso: false,
  dadosEstudanteObjectCardEncaminhamento: {},
  dadosSecaoLocalizarEstudante: {
    // anoLetivo: '2021',
    // codigoAluno: 4766480,
    // codigoDre: '108100',
    // codigoUe: '092819',
    // codigoTurma: '',
    // turmaId: 614004,
  },
  dadosSecoesPorEtapaDeEncaminhamentoAEE: [],
  formsSecoesEncaminhamentoAEE: null,
  encaminhamentoAEEEmEdicao: false,
  exibirLoaderEncaminhamentoAEE: false,
  dadosEncaminhamento: null,
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
      case '@encaminhamentoAEE/setDadosEstudanteObjectCardEncaminhamento': {
        return {
          ...draft,
          dadosEstudanteObjectCardEncaminhamento: action.payload,
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
      case '@encaminhamentoAEE/setDadosSecaoLocalizarEstudante': {
        return {
          ...draft,
          dadosSecaoLocalizarEstudante: action.payload,
        };
      }
      case '@encaminhamentoAEE/setDadosSecoesPorEtapaDeEncaminhamentoAEE': {
        return {
          ...draft,
          dadosSecoesPorEtapaDeEncaminhamentoAEE: action.payload,
        };
      }
      case '@encaminhamentoAEE/setFormsSecoesEncaminhamentoAEE': {
        return {
          ...draft,
          formsSecoesEncaminhamentoAEE: action.payload,
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

      default:
        return draft;
    }
  });
}
