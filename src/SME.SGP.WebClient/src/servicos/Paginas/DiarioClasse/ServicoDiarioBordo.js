import api from '~/servicos/api';

const urlPadrao = `/v1/diarios-bordo`;

class ServicoDiarioBordo {
  obterDiarioBordo = aulaId => {
    return api.get(`${urlPadrao}/${aulaId}`);
  };

  salvarDiarioBordo = (params, idDiarioBordo) => {
    if (idDiarioBordo) {
      params.id = idDiarioBordo;
      return api.put(urlPadrao, params);
    }
    return api.post(urlPadrao, params);
  };

  obterPlanejamentosPorIntervalo = (
    turmaCodigo,
    componenteCurricularId,
    dataInicio,
    dataFim,
    numeroPagina
  ) => {
    const url = `${urlPadrao}/turmas/${turmaCodigo}/componentes-curriculares/${componenteCurricularId}/inicio/${dataInicio}/fim/${dataFim}?numeroPagina=${numeroPagina ||
      1}&NumeroRegistros=4`;
    return api.get(url);
  };

  obterPlanejamentosPorDevolutiva = (idDevolutiva, numeroPagina) => {
    const url = `${urlPadrao}/devolutivas/${idDevolutiva}?numeroPagina=${numeroPagina ||
      1}`;
    return api.get(url);
  };
}

export default new ServicoDiarioBordo();
