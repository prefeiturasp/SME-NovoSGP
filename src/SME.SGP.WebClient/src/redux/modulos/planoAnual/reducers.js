import produce from 'immer';

const INICIAL = {
  disciplinasPlanoAnual: [],
  bimestres: [],
  bimestresErro: {
    type: '',
    content: [],
    title: '',
    onClose: null,
    visible: false,
  },
};

export default function bimestres(state = INICIAL, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@bimestres/SalvarBimestre':
        draft.bimestres[action.payload.indice] = action.payload.bimestre;
        draft.bimestresErro = state.bimestresErro;

        break;
      case '@bimestres/SalvarTodosBimestres':
        draft.bimestres = action.payload;
        draft.bimestresErro = state.bimestresErro;

        break;
      case '@bimestres/SalvarDisciplinasPlanoAnual':
        draft.disciplinasPlanoAnual = action.payload;
        break;
      case '@bimestres/SelecionarDisciplinaPlanoAnual':
        draft.disciplinasPlanoAnual.forEach(disciplina => {
          disciplina.selecionada = false;
        });
        draft.disciplinasPlanoAnual.find(
          disciplina =>
            disciplina.codigo.toString() === action.payload.codigo.toString()
        ).selecionada = true;
        break;
      case '@bimestres/LimparDisciplinaPlanoAnual':
        if (
          state.disciplinasPlanoAnual &&
          state.disciplinasPlanoAnual.length > 0
        )
          draft.disciplinasPlanoAnual.map(disciplina => {
            disciplina.selecionada = false;
            return disciplina;
          });
        break;
      case '@bimestres/PrePostBimestre':
        const paraEnvio = state.bimestres.filter(x => x.ehEdicao);
        paraEnvio.forEach(elem => {
          if (state.bimestres[elem.indice].setarObjetivo)
            draft.bimestres[elem.indice].objetivo = state.bimestres[
              elem.indice
            ].setarObjetivo();

          draft.bimestres[elem.indice].paraEnviar = true;
        });
        draft.bimestresErro = state.bimestresErro;

        break;
      case '@bimestres/SalvarMateria':
        draft.bimestres[action.payload.indice].materias =
          action.payload.materias;
        draft.bimestresErro = state.bimestresErro;

        break;
      case '@bimestres/SalvarEhExpandido':
        if (!state.bimestres[action.payload.indice]) return;

        draft.bimestres[action.payload.indice].ehExpandido =
          action.payload.ehExpandido;
        draft.bimestres[action.payload.indice].ehEdicao =
          action.payload.ehExpandido;
        draft.bimestresErro = state.bimestresErro;

        break;
      case '@bimestres/SelecionarMateria':
        draft.bimestres[action.payload.indice].materias[
          action.payload.indiceMateria
        ].selecionada = action.payload.selecionarMateria;
        draft.bimestresErro = state.bimestresErro;

        const setarObjetivoFunc =
          state.bimestres[action.payload.indice].setarObjetivo;

        if (state.bimestres[action.payload.indice] && setarObjetivoFunc)
          draft.bimestres[action.payload.indice].objetivo = state.bimestres[
            action.payload.indice
          ].setarObjetivo();

        break;
      case '@bimestres/SalvarObjetivos':
        if (state.bimestres[action.payload.indice])
          draft.bimestres[action.payload.indice].objetivosAprendizagem =
            action.payload.objetivos;

        draft.bimestresErro = state.bimestresErro;

        break;
      case '@bimestres/removerSelecaoTodosObjetivos':
        draft.bimestres[
          action.payload.indice
        ].objetivosAprendizagem = draft.bimestres[
          action.payload.indice
        ].objetivosAprendizagem.map(objetivo => {
          objetivo.selected = false;
          return objetivo;
        });
        draft.bimestres[action.payload.indice].ehEdicao = true;

        break;

      case '@bimestres/SetarDescricaoFunction':
        const bimestre = state.bimestres[action.payload.indice];

        if (bimestre && action.payload.setarObjetivo)
          draft.bimestres[action.payload.indice].setarObjetivo =
            action.payload.setarObjetivo;

        draft.bimestresErro = state.bimestresErro;

        break;

      case '@bimestres/SelecionarObjetivo':
        draft.bimestres[action.payload.indice].objetivosAprendizagem[
          action.payload.indiceObjetivo
        ].selected = action.payload.selecionarObjetivo;
        draft.bimestresErro = state.bimestresErro;

        draft.bimestres[action.payload.indice].ehEdicao = true;

        if (state.bimestres[action.payload.indice].setarObjetivo)
          draft.bimestres[action.payload.indice].objetivo = state.bimestres[
            action.payload.indice
          ].setarObjetivo();

        break;

      case '@bimestres/SetarDescricao':
        if (!state.bimestres[action.payload.indice]) return;

        draft.bimestres[action.payload.indice].objetivo =
          action.payload.descricao;
        draft.bimestresErro = state.bimestresErro;

        break;

      case '@bimestres/LimparBimestres':
        draft.bimestres = [];

        break;

      case '@bimestres/BimestresErro':
        draft.bimestres = state.bimestres;
        draft.bimestresErro = action.payload;

        break;

      case '@bimestres/RemoverFocado':
        draft.bimestres = draft.bimestres.map(x => {
          if (!x) return x;

          x.focado = false;

          return x;
        });
        break;
      case '@bimestres/LimparBimestresErro':
        draft.bimestres = state.bimestres;
        draft.bimestresErro = action.payload;

        break;

      case '@bimestres/PosPostBimestre':
        draft.bimestres = draft.bimestres.map(x => {
          x.paraEnviar = false;
          x.recarregarPlanoAnual = action.payload;
          return x;
        });

        break;
      case '@bimestres/setEdicaoFalse':
        draft.bimestres = draft.bimestres.map(x => {
          x.ehEdicao = false;
          return x;
        });
        break;

      case '@bimestres/jaSincronizou':
        draft.bimestres[action.payload].jaSincronizou = true;
        break;
      default:
        break;
    }
  });
}
