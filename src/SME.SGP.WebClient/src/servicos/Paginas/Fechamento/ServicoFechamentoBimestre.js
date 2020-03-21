import api from '~/servicos/api';

const ServicoFechamentoBimestre = {
  buscarDados(turmaCodigo, disciplinaCodigo, bimestre) {
    return api.get(
      `/v1/fechamentos/turmas?turmaCodigo=${turmaCodigo}&disciplinaCodigo=${disciplinaCodigo}&bimestre=${bimestre}`
    );
  },

  // TODO Alteraçõs serao realizadas no back!
  processarReprocessar(fechamentoId) {
    return api.post(`/v1/fechamentos/turmas/reprocessar/${fechamentoId}`);
  },
};

export default ServicoFechamentoBimestre;
