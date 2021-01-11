import produce from 'immer';

const inicial = {
  dadosEstudanteObjectCardEncaminhamento: {
    nome: 'Estudante mock',
    numeroChamada: 1,
    dataNascimento: new Date(),
    codigoEOL: 123456,
    situacao: 'Ativo',
    dataSituacao: new Date(),
    frequencia: '88%',
  },
  expandirLinhaAusenciaEstudante: [],
  dadosModalAnotacao: null,
  exibirModalAnotacao: false,
  exibirModalAviso: false,
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

      default:
        return draft;
    }
  });
}
