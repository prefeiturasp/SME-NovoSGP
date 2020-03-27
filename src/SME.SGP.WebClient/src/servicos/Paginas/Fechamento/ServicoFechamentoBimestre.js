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

  formatarNotaConceito(valor) {
    const novoValor = Number(valor).toFixed(1);
    return isNaN(novoValor) ? valor : novoValor;
  },
};

export default ServicoFechamentoBimestre;
