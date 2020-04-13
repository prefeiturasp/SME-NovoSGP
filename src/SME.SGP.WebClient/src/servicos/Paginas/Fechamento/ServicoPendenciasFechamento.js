import api from '~/servicos/api';

const urlPadrao = `/v1/fechamentos/pendencias`;

class ServicoPendenciasFechamento {

  obterPorId = id => {
    return api.get(`${urlPadrao}/${id}`);
  };

  aprovar = pendenciasIds => {    
    return api.post(`${urlPadrao}/aprovar`, pendenciasIds);
  };
}

export default new ServicoPendenciasFechamento();
