import api from '~/servicos/api';

const AtribuicaoEsporadicaServico = {
  buscarDres() {
    return api.get('/v1/abrangencias/dres');
  },
  buscarUes(dreId) {
    return api.get(`/v1/abrangencias/dres/${dreId}/ues`);
  },
};

export default AtribuicaoEsporadicaServico;
