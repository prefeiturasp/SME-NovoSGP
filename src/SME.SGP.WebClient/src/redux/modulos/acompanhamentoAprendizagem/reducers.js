import produce from 'immer';

const inicial = {
  alunosAcompanhamentoAprendizagem: [],
  dadosAlunoObjectCard: {},
  codigoAlunoSelecionado: null,
  exibirLoaderGeralAcompanhamentoAprendizagem: false,
  dadosAcompanhamentoAprendizagem: {},
  acompanhamentoAprendizagemEmEdicao: false,
  desabilitarCamposAcompanhamentoAprendizagem: false,
  dadosApanhadoGeral: {},
  apanhadoGeralEmEdicao: false,
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
          dadosAcompanhamentoAprendizagem: null,
          acompanhamentoAprendizagemEmEdicao: false,
        };
      }
      case '@acompanhamentoAprendizagem/setExibirLoaderGeralAcompanhamentoAprendizagem': {
        return {
          ...draft,
          exibirLoaderGeralAcompanhamentoAprendizagem: action.payload,
        };
      }
      case '@acompanhamentoAprendizagem/setDadosAcompanhamentoAprendizagem': {
        return {
          ...draft,
          dadosAcompanhamentoAprendizagem: action.payload,
        };
      }
      case '@acompanhamentoAprendizagem/setAcompanhamentoAprendizagemEmEdicao': {
        return {
          ...draft,
          acompanhamentoAprendizagemEmEdicao: action.payload,
        };
      }
      case '@acompanhamentoAprendizagem/setDesabilitarCamposAcompanhamentoAprendizagem': {
        return {
          ...draft,
          desabilitarCamposAcompanhamentoAprendizagem: action.payload,
        };
      }
      case '@acompanhamentoAprendizagem/setDadosApanhadoGeral': {
        return {
          ...draft,
          dadosApanhadoGeral: action.payload,
        };
      }
      case '@acompanhamentoAprendizagem/setApanhadoGeralEmEdicao': {
        return {
          ...draft,
          apanhadoGeralEmEdicao: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
