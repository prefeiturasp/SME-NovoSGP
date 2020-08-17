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
}

export default new ServicoDiarioBordo();
