import produce from 'immer';

const inicial = {
  alunosRegistroIndividual: [],
  auditoriaAnotacaoRecomendacao: null,
  componenteCurricularSelecionado: '',
  dadosAlunoObjectCard: {},
  dadosPrincipaisRegistroIndividual: {},
  desabilitarCampos: false,
  exibirLoaderGeralRegistroIndividual: false,
  expandirLinha: [],
  registroIndividualEmEdicao: false,
  salvouJustificativa: false,
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
          dadosAlunoObjectCard: {},
          registroIndividualEmEdicao: false,
          dadosPrincipaisRegistroIndividual: {},
          auditoriaAnotacaoRecomendacao: null,
          expandirLinha: [],
          desabilitarCampos: false,
          salvouJustificativa: false,
          exibirLoaderGeralRegistroIndividual: false,
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
      case '@registroIndividual/setSalvouJustificativa': {
        return {
          ...draft,
          salvouJustificativa: action.payload,
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
      default:
        return draft;
    }
  });
}
