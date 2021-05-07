import produce from 'immer';

const inicial = {
  turmasAcompanhamentoFechamento: [],
  carregandoAcompanhamentoFechamento: false,
};

export default function AcompanhamentoFechamento(state = inicial, action) {
  console.log('state', state);
  return produce(state, draft => {
    switch (action.type) {
      case '@acompanhamentoFechamento/setTurmasAcompanhamentoFechamento': {
        return {
          ...draft,
          turmasAcompanhamentoFechamento: action.payload,
        };
      }
      case '@acompanhamentoFechamento/setCarregandoAcompanhamentoFechamento': {
        return {
          ...draft,
          carregandoAcompanhamentoFechamento: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
