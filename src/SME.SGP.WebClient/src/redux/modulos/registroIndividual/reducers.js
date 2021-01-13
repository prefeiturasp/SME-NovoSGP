import produce from 'immer';

const inicial = {
  alunosRegistroIndividual: [],
  auditoriaNovoRegistroIndividual: null,
  componenteCurricularSelecionado: '',
  dadosAlunoObjectCard: {},
  dadosParaSalvarNovoRegistro: {},
  dadosPrincipaisRegistroIndividual: {},
  desabilitarCampos: false,
  exibirLoaderGeralRegistroIndividual: false,
  expandirLinha: [],
  registroIndividualEmEdicao: false,
  registroAnteriorEmEdicao: false,
  resetDataNovoRegistroIndividual: false,
};

export default function RegistroIndividual(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@registroIndividual/setDadosAlunoObjectCard': {
        return {
          ...draft,
          dadosAlunoObjectCard: action.payload,
        };
      }
      case '@registroIndividual/setAlunosRegistroIndividual': {
        return {
          ...draft,
          alunosRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/limparDadosRegistroIndividual': {
        return {
          ...draft,
          auditoriaNovoRegistroIndividual: null,
          dadosParaSalvarNovoRegistro: {},
          dadosPrincipaisRegistroIndividual: {},
          desabilitarCampos: false,
          exibirLoaderGeralRegistroIndividual: false,
          expandirLinha: [],
          registroIndividualEmEdicao: false,
          salvouJustificativa: false,
          resetDataNovoRegistroIndividual: true,
        };
      }
      case '@registroIndividual/setRegistroIndividualEmEdicao': {
        return {
          ...draft,
          registroIndividualEmEdicao: action.payload,
        };
      }
      case '@registroIndividual/setRegistroAnteriorEmEdicao': {
        return {
          ...draft,
          registroAnteriorEmEdicao: action.payload,
        };
      }
      case '@registroIndividual/setDadosPrincipaisRegistroIndividual': {
        return {
          ...draft,
          dadosPrincipaisRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/setExpandirLinha': {
        return {
          ...draft,
          expandirLinha: action.payload,
        };
      }
      case '@registroIndividual/setDesabilitarCampos': {
        return {
          ...draft,
          desabilitarCampos: action.payload,
        };
      }
      case '@registroIndividual/setExibirLoaderGeralRegistroIndividual': {
        return {
          ...draft,
          exibirLoaderGeralRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/setComponenteCurricularSelecionado': {
        return {
          ...draft,
          componenteCurricularSelecionado: action.payload,
        };
      }
      case '@registroIndividual/setDadosParaSalvarNovoRegistro': {
        return {
          ...draft,
          dadosParaSalvarNovoRegistro: action.payload,
        };
      }
      case '@registroIndividual/setAuditoriaNovoRegistro': {
        return {
          ...draft,
          auditoriaNovoRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/resetDataNovoRegistro': {
        return {
          ...draft,
          resetDataNovoRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/excluirRegistroAnteriorId': {
        const items = state.dadosPrincipaisRegistroIndividual.registrosIndividuais.items.filter(
          dados => dados.id !== action.payload
        );
        return {
          ...draft,
          dadosPrincipaisRegistroIndividual: {
            ...state.dadosPrincipaisRegistroIndividual,
            registrosIndividuais: {
              ...state.dadosPrincipaisRegistroIndividual.registrosIndividuais,
              items,
            },
          },
        };
      }
      case '@registroIndividual/atualizarMarcadorDiasSemRegistroExibir': {
        const aluno = state.alunosRegistroIndividual.find(
          a => a.codigoEOL === action.payload
        );
        aluno.marcadorDiasSemRegistroExibir = false;
        break;
      }
      default:
        return draft;
    }
  });
}
