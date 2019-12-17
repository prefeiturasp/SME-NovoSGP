import api from '~/servicos/api';

const RegitroPOAServico = {
  salvarRegistroPOA(registro) {
    return api.post(`/v1/atribuicao/poa`, registro);
  },
  buscarRegistroPOA(id) {
    return api.get(`/v1/atribuicao/poa/${id}`);
  },
  deletarAtribuicaoEsporadica(id) {
    return api.delete(`/v1/atribuicao/esporadica/${id}`);
  },
};

export default RegitroPOAServico;
