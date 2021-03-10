import produce from 'immer';

const inicial = {
  alunosAcompanhamentoAprendizagem: [],
  dadosAlunoObjectCard: {},
  codigoAlunoSelecionado: null,
  exibirLoaderGeralAcompanhamentoAprendizagem: false,
};

export default function AcompanhamentoAprendizagem(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@acompanhamentoAprendizagem/setAlunosAcompanhamentoAprendizagem': {
        return {
          ...draft,
          alunosAcompanhamentoAprendizagem: action.payload,
        };
      }
      case '@acompanhamentoAprendizagem/setDadosAlunoObjectCard': {
        return {
          ...draft,
          dadosAlunoObjectCard: action.payload,
        };
      }
      case '@acompanhamentoAprendizagem/setCodigoAlunoSelecionado': {
        return {
          ...draft,
          codigoAlunoSelecionado: action.payload,
        };
      }
      case '@acompanhamentoAprendizagem/limparDadosAcompanhamentoAprendizagem': {
        return {
          ...draft,
          dadosAlunoObjectCard: {},
          codigoAlunoSelecionado: null,
          exibirLoaderGeralAcompanhamentoAprendizagem: false,
        };
      }
      case '@acompanhamentoAprendizagem/setExibirLoaderGeralAcompanhamentoAprendizagem': {
        return {
          ...draft,
          exibirLoaderGeralAcompanhamentoAprendizagem: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
