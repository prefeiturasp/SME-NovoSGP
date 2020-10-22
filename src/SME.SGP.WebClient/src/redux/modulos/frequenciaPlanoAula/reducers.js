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
  tabAtualComponenteCurricular: undefined,
  listaComponentesCurricularesPlanejamento: [],
  exibirCardCollapsePlanoAula: { exibir: false },
  exibirModalCopiarConteudoPlanoAula: false,
  temPeriodoAberto: true,
  listaObjetivosComponenteCurricular: [],
  errosPlanoAula: false,
  exibirModalErrosPlanoAula: false,
  checkedExibirEscolhaObjetivos: false,
  exibirSwitchEscolhaObjetivos: false,
  dadosOriginaisPlanoAula: [],
  exibirCardCollapseFrequencia: false,
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
          tabAtualComponenteCurricular: undefined,
          listaComponentesCurricularesPlanejamento: [],
          exibirCardCollapsePlanoAula: { exibir: false },
          exibirModalCopiarConteudoPlanoAula: false,
          temPeriodoAberto: true,
          listaObjetivosComponenteCurricular: [],
          errosPlanoAula: false,
          exibirModalErrosPlanoAula: false,
          checkedExibirEscolhaObjetivos: false,
          exibirSwitchEscolhaObjetivos: false,
          dadosOriginaisPlanoAula: [],
          exibirCardCollapseFrequencia: false,
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
      case '@frequenciaPlanoAula/setTemPeriodoAbertoFrequenciaPlanoAula': {
        return {
          ...draft,
          temPeriodoAberto: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setListaObjetivosComponenteCurricular': {
        return {
          ...draft,
          listaObjetivosComponenteCurricular: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setLimparDadosPlanoAula': {
        return {
          ...draft,
          dadosPlanoAula: undefined,
          modoEdicaoPlanoAula: false,
          tabAtualComponenteCurricular: undefined,
          listaComponentesCurricularesPlanejamento: [],
          exibirCardCollapsePlanoAula: { exibir: false },
          exibirModalCopiarConteudoPlanoAula: false,
          listaObjetivosComponenteCurricular: [],
          checkedExibirEscolhaObjetivos: false,
          exibirSwitchEscolhaObjetivos: false,
          dadosOriginaisPlanoAula: [],
        };
      }
      case '@planoAnual/setErrosPlanoAula': {
        return {
          ...draft,
          errosPlanoAula: action.payload,
        };
      }
      case '@planoAnual/setExibirModalErrosPlanoAula': {
        return {
          ...draft,
          exibirModalErrosPlanoAula: action.payload,
        };
      }
      case '@planoAnual/setCheckedExibirEscolhaObjetivos': {
        return {
          ...draft,
          checkedExibirEscolhaObjetivos: action.payload,
        };
      }
      case '@planoAnual/setExibirSwitchEscolhaObjetivos': {
        return {
          ...draft,
          exibirSwitchEscolhaObjetivos: action.payload,
        };
      }
      case '@planoAnual/setDadosOriginaisPlanoAula': {
        return {
          ...draft,
          dadosOriginaisPlanoAula: action.payload,
        };
      }
      case '@planoAnual/setExibirCardCollapseFrequencia': {
        return {
          ...draft,
          exibirCardCollapseFrequencia: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
