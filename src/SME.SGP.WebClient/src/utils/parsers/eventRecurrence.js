export const parseScreenObject = recurrence => ({
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
});

export const parseDataObject = recurrence => ({
  dataInicio: recurrence.dataInicio,
  dataTermino: recurrence.dataFim,
  diaNumero: recurrence.diaDeOcorrencia,
  diasDaSemana: recurrence.diasSemana.map(item => item.valor),
  padrao: recurrence.tipoRecorrencia.value,
  padraoRecorrencia: recurrence.padraoRecorrenciaMensal,
  quantidadeRecorrencia: recurrence.repeteACada,
});
