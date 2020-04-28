import produce from 'immer';

const inicial = {
  dadosAlunoObjectCard: {},
  alunosRelatorioSemestral: [],
  relatorioSemestralEmEdicao: false,
};

export default function RelatorioSemestral(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@relatorioSemestral/setDadosAlunoObjectCard': {
        return {
          ...draft,
          dadosAlunoObjectCard: action.payload,
        };
      }
      case '@relatorioSemestral/setAlunosRelatorioSemestral': {
        return {
          ...draft,
          alunosRelatorioSemestral: action.payload,
        };
      }
      case '@relatorioSemestral/limparDadosRelatorioSemestral': {
        return {
          ...draft,
          dadosAlunoObjectCard: {},
          relatorioSemestralEmEdicao: false,
        };
      }
      case '@relatorioSemestral/setRelatorioSemestralEmEdicao': {
        return {
          ...draft,
          relatorioSemestralEmEdicao: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
