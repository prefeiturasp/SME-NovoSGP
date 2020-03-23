export const parseScreenObject = recurrence => {
  if (recurrence) {
    return {
      dataInicio: recurrence.dataInicio,
      dataFim: recurrence.dataTermino,
      diaDeOcorrencia: recurrence.diaNumero,
      diasDaSemana:
        recurrence.diasSemana.length > 0
          ? recurrence.diasSemana.map(item => item.valor)
          : recurrence.diaSemana
          ? [recurrence.diaSemana]
          : [],
      padrao: recurrence.tipoRecorrencia.value,
      padraoRecorrenciaMensal: recurrence.padraoRecorrencia,
      repeteACada: recurrence.quantidadeRecorrencia,
    };
  }
  return null;
};

export const parseDataObject = recurrence => {
  if (recurrence) {
    return {
      dataInicio: recurrence.dataInicio,
      dataTermino: recurrence.dataFim,
      diaNumero: recurrence.diaDeOcorrencia,
      diasDaSemana: recurrence.diasSemana.map(item => item.valor),
      padrao: recurrence.tipoRecorrencia.value,
      padraoRecorrencia: recurrence.padraoRecorrenciaMensal,
      quantidadeRecorrencia: recurrence.repeteACada,
    };
  }
  return null;
};
