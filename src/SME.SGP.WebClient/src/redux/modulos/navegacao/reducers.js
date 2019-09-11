import produce from 'immer';

const rotas = new Map();
const inicial = {
  collapsed: false,
  activeRoute: '/',
  rotas,
};

export default function navegacao(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@navegacao/collapsed':
        draft.collapsed = action.payload;
        break;
      case '@navegacao/activeRoute':
        draft.activeRoute = action.payload;
        break;
      case '@navegacao/rotas':
        draft.rotas.set(action.payload.path, action.payload);
        break;
      default:
        break;
    }
  });
}
