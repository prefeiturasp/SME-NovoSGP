import produce from 'immer';

const inicial = {
  collapsed: false
};

export default function menu(state = inicial, action) {
  return produce(state, draft => {
    if (action.type === '@menu/collapsed') {
      draft.collapsed = action.payload;
    }
  });
}
