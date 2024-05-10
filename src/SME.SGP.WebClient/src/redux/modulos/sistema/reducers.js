import produce from 'immer';
import { types } from './actions';

const inicial = {
  versao: undefined,
};

export default function usuario(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case types.salvarVersao:
        return {
          ...draft,
          versao: action.payload,
        };
      default:
        return state;
    }
  });
}
