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
        };
      }
      case '@registroIndividual/setRegistroIndividualEmEdicao': {
        return {
          ...draft,
          registroIndividualEmEdicao: action.payload,
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
      default:
        return draft;
    }
  });
}
