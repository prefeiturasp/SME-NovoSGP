import produce from 'immer';

const inicial = {
  filtro: {
    anoLetivo: '',
    dreId: '',
    ueId: '',
  },
};

export default function atribuicaoEsporadica(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@atribuicaoEsporadica/selecionarDre':
        draft.filtro.dreId = action.payload;
        break;
      case '@atribuicaoEsporadica/selecionarUe':
        draft.filtro.ueId = action.payload;
        break;
      case '@atribuicaoEsporadica/selecionarAnoLetivo':
        draft.filtro.anoLetivo = action.payload;
        break;
      default:
        break;
    }
  });
}
