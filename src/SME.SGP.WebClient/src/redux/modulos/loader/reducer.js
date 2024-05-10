import produce from 'immer';

const inicial = {
  loaderGeral: false,
  loaderSecao: false,
  loaderTabela: false,
  loaderModal: false,
  loaderBotao: false,
};

export default function Loader(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@loader/setLoaderGeral': {
        return {
          ...draft,
          loaderGeral: action.payload,
        };
      }
      case '@loader/setLoaderSecao': {
        return {
          ...draft,
          loaderSecao: action.payload,
        };
      }
      case '@loader/setLoaderTabela': {
        return {
          ...draft,
          loaderTabela: action.payload,
        };
      }
      case '@loader/setLoaderModal': {
        return {
          ...draft,
          loaderModal: action.payload,
        };
      }
      case '@loader/setLoaderBotao': {
        return {
          ...draft,
          loaderBotao: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
