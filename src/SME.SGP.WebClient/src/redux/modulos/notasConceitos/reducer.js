import produce from 'immer';

const inicial = {
  modoEdicaoGeral: false,
  expandirLinha: [],
};

export default function NotasConceitos(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@notasConceitos/setModoEdicaoGeral':
        draft.modoEdicaoGeral = action.payload;
        break;

      case '@notasConceitos/setModoEdicaoGeralNotaFinal':
        draft.modoEdicaoGeralNotaFinal = action.payload;
        break;

      case '@notasConceitos/setExpandirLinha':
        draft.expandirLinha = action.payload;
        break;

      default:
        break;
    }
  });
}
