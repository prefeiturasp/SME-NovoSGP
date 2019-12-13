import produce from 'immer';

const inicial = {
  modoEdicaoGeral: false,
};

export default function NotasConceitos(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@notasConceitos/setModoEdicaoGeral': {
        return {
          ...draft,
          modoEdicaoGeral: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
