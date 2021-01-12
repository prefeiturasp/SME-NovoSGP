import produce from 'immer';

const inicial = {
  expandirLinhaAusenciaEstudante: [],
  dadosModalAnotacao: null,
  exibirModalAnotacao: false,
  exibirModalAviso: false,
  dadosEstudanteObjectCardEncaminhamento: {},
  dadosSecaoLocalizarEstudante: {
    anoLetivo: '2021',
    codigoAluno: 6160735,
    codigoDre: '108800',
    codigoTurma: undefined,
    codigoUe: '094765',
  },
  dadosSecoesPorEtapaDeEncaminhamentoAEE: [],
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

      default:
        return draft;
    }
  });
}
