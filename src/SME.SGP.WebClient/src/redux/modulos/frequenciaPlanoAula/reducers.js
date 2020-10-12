import produce from 'immer';

const inicial = {
  componenteCurricular: undefined,
  listaDadosFrequencia: {},
  modoEdicaoFrequencia: false,
  modoEdicaoPlanoAula: false,
  aulaId: 0,
  dataSelecionada: undefined,
  exibirCardFrequencia: false,
  exibirLoaderFrequenciaPlanoAula: false,
  somenteConsulta: false,
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
          modoEdicaoFrequencia: false,
          modoEdicaoPlanoAula: false,
          aulaId: 0,
          dataSelecionada: undefined,
          exibirCardFrequencia: false,
        };
      }
      case '@frequenciaPlanoAula/setListaDadosFrequencia': {
        return {
          ...draft,
          listaDadosFrequencia: action.payload,
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
      case '@frequenciaPlanoAula/setExibirCardFrequencia': {
        return {
          ...draft,
          exibirCardFrequencia: action.payload,
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

      default:
        return draft;
    }
  });
}
