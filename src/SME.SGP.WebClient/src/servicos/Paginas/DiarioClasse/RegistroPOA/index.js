import api from '~/servicos/api';

const RegitroPOAServico = {
  salvarRegistroPOA(registro, id) {
    const metodo = id ? 'put' : 'post';
    return api[metodo](`/v1/atribuicao/poa/${id || ''}`, registro);
  },
  buscarRegistroPOA(id) {
    return api.get(`/v1/atribuicao/poa/${id}`);
  },
  deletarRegistroPOA(id) {
    return api.delete(`/v1/atribuicao/poa/${id}`);
  },
};

export default RegitroPOAServico;
