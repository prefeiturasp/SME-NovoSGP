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
  desenvolvimentoDaAulaValidaObrigatoriedade: undefined,
  objetivosEspecificosParaAulaValidarObrigatoriedade: undefined,
  atualizarDatas: false,
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
          desenvolvimentoDaAulaValidaObrigatoriedade: undefined,
          objetivosEspecificosParaAulaValidarObrigatoriedade: undefined,
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
          desenvolvimentoDaAulaValidaObrigatoriedade: undefined,
          objetivosEspecificosParaAulaValidarObrigatoriedade: undefined,
        };
      }
      case '@frequenciaPlanoAula/setErrosPlanoAula': {
        return {
          ...draft,
          errosPlanoAula: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setExibirModalErrosPlanoAula': {
        return {
          ...draft,
          exibirModalErrosPlanoAula: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setCheckedExibirEscolhaObjetivos': {
        return {
          ...draft,
          checkedExibirEscolhaObjetivos: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setExibirSwitchEscolhaObjetivos': {
        return {
          ...draft,
          exibirSwitchEscolhaObjetivos: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setDadosOriginaisPlanoAula': {
        return {
          ...draft,
          dadosOriginaisPlanoAula: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setExibirCardCollapseFrequencia': {
        return {
          ...draft,
          exibirCardCollapseFrequencia: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setDesenvolvimentoDaAulaValidaObrigatoriedade': {
        return {
          ...draft,
          desenvolvimentoDaAulaValidaObrigatoriedade: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setobjetivosEspecificosParaAulaValidarObrigatoriedade': {
        return {
          ...draft,
          objetivosEspecificosParaAulaValidarObrigatoriedade: action.payload,
        };
      }
      case '@frequenciaPlanoAula/setAtualizarDatas': {
        return {
          ...draft,
          atualizarDatas: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
