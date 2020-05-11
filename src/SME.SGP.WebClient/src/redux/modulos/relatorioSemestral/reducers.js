import produce from 'immer';

const inicial = {
  dadosAlunoObjectCard: {},
  alunosRelatorioSemestral: [],
  relatorioSemestralEmEdicao: false,
  dadosRelatorioSemestral: {},
  dadosParaSalvarRelatorioSemestral: [],
  auditoriaRelatorioSemestral: null,
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
          dadosRelatorioSemestral: {},
          dadosParaSalvarRelatorioSemestral: [],
          auditoriaRelatorioSemestral: null,
        };
      }
      case '@relatorioSemestral/setRelatorioSemestralEmEdicao': {
        return {
          ...draft,
          relatorioSemestralEmEdicao: action.payload,
        };
      }
      case '@relatorioSemestral/setDadosRelatorioSemestral': {
        return {
          ...draft,
          dadosRelatorioSemestral: action.payload,
        };
      }
      case '@relatorioSemestral/setDadosParaSalvarRelatorioSemestral': {
        const dados = state.dadosParaSalvarRelatorioSemestral;
        if (dados.length > 0) {
          const valor = dados.find(item => item.id == action.payload.id);
          if (valor) {
            const indexItem = dados.findIndex(
              item => item.id == action.payload.id
            );
            draft.dadosParaSalvarRelatorioSemestral[indexItem] = action.payload;
          } else {
            draft.dadosParaSalvarRelatorioSemestral.push(action.payload);
          }
        } else {
          draft.dadosParaSalvarRelatorioSemestral.push(action.payload);
        }
        break;
      }
      case '@relatorioSemestral/limparDadosParaSalvarRelatorioSemestral': {
        return {
          ...draft,
          dadosParaSalvarRelatorioSemestral: [],
        };
      }
      case '@relatorioSemestral/setAuditoriaRelatorioSemestral': {
        return {
          ...draft,
          auditoriaRelatorioSemestral: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
