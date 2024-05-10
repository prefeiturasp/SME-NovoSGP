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
  setarEventosDiaHandler: (estado, payload) => {
    const { diaSelecionado, dados } = payload;

    const dia = diaSelecionado.getDate();
    const mes = diaSelecionado.getMonth() + 1;

    return {
      ...estado,
      eventos: {
        ...estado.eventos,
        meses: estado.eventos.meses.map(mesAtual =>
          mesAtual.numeroMes === mes
            ? {
                ...mesAtual,
                dias: mesAtual.dias.map(diaAtual =>
                  diaAtual.dia === dia
                    ? {
                        ...diaAtual,
                        dados,
                      }
                    : {
                        ...diaAtual,
                      }
                ),
              }
            : {
                ...mesAtual,
              }
        ),
      },
    };
  },
};
