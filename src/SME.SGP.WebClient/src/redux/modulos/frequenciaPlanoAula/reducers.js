import produce from 'immer';

const inicial = {
  componenteCurricular: undefined,
  listaDadosFrequencia: {},
  dadosPlanoAula: undefined,
  modoEdicaoFrequencia: false,
  modoEdicaoPlanoAula: false,
  aulaId: 0,
  dataSelecionada: undefined,
  exibirLoaderFrequenciaPlanoAula: false,
  somenteConsulta: false,
  desabilitarCamposPlanoAula: false,
  dadosParaSalvarPlanoAula: undefined,
  tabAtualComponenteCurricular: undefined,
  listaComponentesCurricularesPlanejamento: [],
  exibirCardCollapsePlanoAula: { exibir: false },
  exibirModalCopiarConteudoPlanoAula: false,
};

export default function frequenciaPlanoAula(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@frequenciaPlanoAula/setComponenteCurricularFrequenciaPlanoAula': {
        return {
          ...draft,
          componenteCurricular: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setDataSelecionadaFrequenciaPlanoAula': {
        return {
          ...draft,
          dataSelecionada: action.payload,
        };
      }
      case '@frequenciaPlanoAula/limparDadosFrequenciaPlanoAula': {
        return {
          ...draft,
          listaDadosFrequencia: [],
          dadosPlanoAula: undefined,
          modoEdicaoFrequencia: false,
          modoEdicaoPlanoAula: false,
          aulaId: 0,
          dataSelecionada: undefined,
          desabilitarCamposPlanoAula: false,
          dadosParaSalvarPlanoAula: undefined,
          tabAtualComponenteCurricular: undefined,
          listaComponentesCurricularesPlanejamento: [],
          exibirCardCollapsePlanoAula: { exibir: false },
          exibirModalCopiarConteudoPlanoAula: false,
        };
      }
      case '@frequenciaPlanoAula/setListaDadosFrequencia': {
        return {
          ...draft,
          listaDadosFrequencia: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setDadosPlanoAula': {
        return {
          ...draft,
          dadosPlanoAula: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setModoEdicaoFrequencia': {
        return {
          ...draft,
          modoEdicaoFrequencia: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setModoEdicaoPlanoAula': {
        return {
          ...draft,
          modoEdicaoPlanoAula: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setAulaIdFrequenciaPlanoAula': {
        return {
          ...draft,
          aulaId: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setDesabilitarCamposPlanoAula': {
        return {
          ...draft,
          desabilitarCamposPlanoAula: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setExibirLoaderFrequenciaPlanoAula': {
        return {
          ...draft,
          exibirLoaderFrequenciaPlanoAula: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setSomenteConsultaFrequenciaPlanoAula': {
        return {
          ...draft,
          somenteConsulta: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setDadosParaSalvarPlanoAula': {
        return {
          ...draft,
          dadosParaSalvarPlanoAula: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setTabAtualComponenteCurricular': {
        return {
          ...draft,
          tabAtualComponenteCurricular: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setListaComponentesCurricularesPlanejamento': {
        return {
          ...draft,
          listaComponentesCurricularesPlanejamento: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setExibirCardCollapsePlanoAula': {
        return {
          ...draft,
          exibirCardCollapsePlanoAula: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setExibirModalCopiarConteudoPlanoAula': {
        return {
          ...draft,
          exibirModalCopiarConteudoPlanoAula: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
