import api from '~/servicos/api';

const ServicoFechamentoBimestre = {
  buscarDados() {
    return api.get('/v1/fechamento/bimestre');
  },
};

export default ServicoFechamentoBimestre;
