export default {
  setarEventosMesHandler: (estado, payload) => {
    let { meses } = estado.eventos;

    if (meses.filter(x => x.numeroMes === payload.numeroMes).length > 0) {
      meses = meses.map(mesAtual =>
        mesAtual.numeroMes === payload.numeroMes
          ? { ...mesAtual, dias: payload.dias }
          : mesAtual
      );
    } else {
      meses = meses.concat([payload]);
    }

    return {
      ...estado,
      eventos: {
        ...estado.eventos,
        meses,
      },
    };
  },
};
