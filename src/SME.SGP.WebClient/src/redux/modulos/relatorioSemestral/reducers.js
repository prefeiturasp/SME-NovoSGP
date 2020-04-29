import produce from 'immer';

const inicial = {
  dadosAlunoObjectCard: {},
  alunosRelatorioSemestral: [],
  relatorioSemestralEmEdicao: false,
  dadosRelatorioSemestral: {},
  historicoEstudante: '',
  dificuldades: '',
  encaminhamentos: '',
  avancos: '',
  outros: '',
  auditoriaRelatorioSemestral: null,
  desabilitarCampos: false,
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
          historicoEstudante: '',
          dificuldades: '',
          encaminhamentos: '',
          avancos: '',
          outros: '',
          auditoriaRelatorioSemestral: null,
          desabilitarCampos: false,
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
      case '@relatorioSemestral/setHistoricoEstudante': {        
        return {
          ...draft,
          historicoEstudante: action.payload,
        };
      }
      case '@relatorioSemestral/setDificuldades': {
        return {
          ...draft,
          dificuldades: action.payload,
        };
      }
      case '@relatorioSemestral/setEncaminhamentos': {
        return {
          ...draft,
          encaminhamentos: action.payload,
        };
      }
      case '@relatorioSemestral/setAvancos': {
        return {
          ...draft,
          avancos: action.payload,
        };
      }
      case '@relatorioSemestral/setOutros': {
        return {
          ...draft,
          outros: action.payload,
        };
      }
      case '@relatorioSemestral/setAuditoriaRelatorioSemestral': {
        return {
          ...draft,
          auditoriaRelatorioSemestral: action.payload,
        };
      }
      case '@relatorioSemestral/setDesabilitarCampos': {
        return {
          ...draft,
          desabilitarCampos: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
