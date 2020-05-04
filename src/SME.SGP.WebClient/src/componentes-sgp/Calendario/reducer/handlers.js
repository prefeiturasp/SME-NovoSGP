export default {
  selecionarMesHandler: (state, payload) => {
    return {
      ...state,
      meses: state.meses
        .map(item =>
          item.numeroMes === payload.numeroMes
            ? {
                ...payload,
                estaAberto: !state.meses.filter(
                  x => x.numeroMes === payload.numeroMes
                )[0].estaAberto,
              }
            : {
                ...item,
                estaAberto: false,
              }
        )
        .sort((a, b) => a.numeroMes < b.numeroMes),
    };
  },
  selecionarDiaHandler: (state, payload) => {
    const { diaSelecionado } = payload;
    return {
      ...state,
      diaSelecionado:
        state.diaSelecionado &&
        state.diaSelecionado.toString() === diaSelecionado.toString()
          ? undefined
          : diaSelecionado,
    };
  },
};
