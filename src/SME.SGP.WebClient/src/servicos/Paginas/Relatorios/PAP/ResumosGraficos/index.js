import api from '~/servicos/api';

const ResumosGraficosPAPServico = {
  ListarTotalEstudantes(params) {
    return api.get('v1/recuperacao-paralela/total-estudantes', {
      params,
    });
  },
  ListarFrequencia(params) {
    return api.get('v1/recuperacao-paralela/grafico/frequencia', {
      params,
    });
  },
  ListarResultados(params) {
    return api.get('v1/recuperacao-paralela/resultado', {
      params,
    });
  },
  ListarInformacoesEscolares(params) {
    return api.get('v1/recuperacao-paralela/resultado/encaminhamento', {
      params,
    });
  },
};

export default ResumosGraficosPAPServico;
