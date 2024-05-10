export const estadoInicial = {
  Periodo: {},
  Alunos: [],
  Eixos: [],
  Objetivos: [],
  Respostas: [],
  ObjetivoAtivo: {},
};

const actionTypes = {
  carregarAlunos: '@pap/carregarAlunos',
  carregarPeriodo: '@pap/carregarPeriodo',
  carregarEixos: '@pap/carregarEixos',
  carregarObjetivos: '@pap/carregarObjetivos',
  carregarRespostas: '@pap/carregarRespostas',
  setarObjetivoAtivo: '@pap/setarObjetivoAtivo',
};

export const carregarAlunos = payload => ({
  type: actionTypes.carregarAlunos,
  payload,
});

export const carregarPeriodo = payload => ({
  type: actionTypes.carregarPeriodo,
  payload,
});

export const carregarEixos = payload => ({
  type: actionTypes.carregarEixos,
  payload,
});

export const carregarObjetivos = payload => ({
  type: actionTypes.carregarObjetivos,
  payload,
});

export const carregarRespostas = payload => ({
  type: actionTypes.carregarRespostas,
  payload,
});

export const setarObjetivoAtivo = payload => ({
  type: actionTypes.setarObjetivoAtivo,
  payload,
});

export default (state, action) => {
  const { payload, type } = action;
  switch (type) {
    case actionTypes.carregarAlunos:
      return {
        ...state,
        Alunos: payload,
      };
    case actionTypes.carregarPeriodo:
      return {
        ...state,
        Periodo: {
          ...payload,
          alunos: null,
        },
      };
    case actionTypes.carregarEixos:
      return {
        ...state,
        Eixos: payload,
      };
    case actionTypes.carregarObjetivos:
      return {
        ...state,
        Objetivos: payload,
      };
    case actionTypes.carregarRespostas:
      return {
        ...state,
        Respostas: payload,
      };
    case actionTypes.setarObjetivoAtivo:
      return {
        ...state,
        ObjetivoAtivo:
          state.Objetivos.filter(x => x.id === payload)[0] ||
          state.Objetivos[0],
      };
    default:
      return state;
  }
};
