import produce from 'immer';

const inicial = {
  expandirLinhaAusenciaEstudante: [],
  dadosModalAnotacao: null,
  exibirModalAnotacao: false,
  dadosEstudanteObjectCardEncaminhamento: {
    nome: 'Estudante mock',
    numeroChamada: 1,
    dataNascimento: new Date(),
    codigoEOL: 123456,
    situacao: 'Ativo',
    dataSituacao: new Date(),
    frequencia: '88%',
  },
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

      default:
        return draft;
    }
  });
}
