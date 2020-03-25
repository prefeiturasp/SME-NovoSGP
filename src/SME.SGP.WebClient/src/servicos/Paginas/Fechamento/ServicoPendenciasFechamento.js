import api from '~/servicos/api';

const urlPadrao = `/v1/fechamentos/pendencias`;

class ServicoPendenciasFechamento {

  obterPorId = id => {
    return api.get(`${urlPadrao}/${id}`);
  };

  aprovar = pendenciaId => {    
    return api.post(`${urlPadrao}/${pendenciaId}`);
  };
}

export default new ServicoPendenciasFechamento();
