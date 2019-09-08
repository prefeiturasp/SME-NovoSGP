import produce from 'immer';

const inicial = {
  collapsed: false,
  activeRoute: '/'
};

export default function navegacao(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@navegacao/collapsed':
        draft.collapsed = action.payload;
      break
      case '@navegacao/activeRoute':
        draft.activeRoute = action.payload;
      break
      default:
        break;
    }
  });
}
