import produce from 'immer';

const inicial = {
  dadosBimestresPlanoAnual: [],
  listaComponentesCurricularesPlanejamento: [],
  planoAnualEmEdicao: false,
  componenteCurricular: undefined,
};

export default function planoAnual(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@planoAnual/setDadosBimestresPlanoAnual': {
        return {
          ...draft,
          dadosBimestresPlanoAnual: action.payload,
        };
      }
      case '@planoAnual/setListaComponentesCurricularesPlanejamento': {
        return {
          ...draft,
          listaComponentesCurricularesPlanejamento: action.payload,
        };
      }
      case '@planoAnual/setPlanoAnualEmEdicao': {
        return {
          ...draft,
          planoAnualEmEdicao: action.payload,
        };
      }
      case '@planoAnual/setComponenteCurricularPlanoAnual': {
        return {
          ...draft,
          componenteCurricular: action.payload,
        };
      }
      case '@planoAnual/limparDadosPlanoAnual': {
        return {
          ...draft,
          planoAnualEmEdicao: false,
          componenteCurricular: undefined,
          dadosBimestresPlanoAnual: [],
        };
      }

      default:
        return draft;
    }
  });
}
