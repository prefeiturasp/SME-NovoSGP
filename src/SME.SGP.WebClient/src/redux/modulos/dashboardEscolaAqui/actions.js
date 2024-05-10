export const setDadosDeLeituraDeComunicadosAgrupadosPorModalidade = payload => ({
  type:
    '@dashboardEscolaAqui/setDadosDeLeituraDeComunicadosAgrupadosPorModalidade',
  payload,
});

export const setDadosDeLeituraDeComunicadosPorTurmas = payload => ({
  type: '@dashboardEscolaAqui/setDadosDeLeituraDeComunicadosPorTurmas',
  payload,
});

export const limparDadosDashboardEscolaAqui = payload => ({
  type: '@dashboardEscolaAqui/limparDadosDashboardEscolaAqui',
  payload,
});
