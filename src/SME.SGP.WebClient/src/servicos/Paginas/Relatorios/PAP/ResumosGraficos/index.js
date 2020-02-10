import api from '~/servicos/api';

const ResumosGraficosPAPServico = {
  ListarTotalEstudantes(params) {
    return api.get('v1/recuperacao-paralela/total-estudantes', {
      params,
    });
  },
  ListarFrequencia(params) {
    return api.get(
      `http://demo8322243.mockable.io/api/v1/recuperacao-paralela/resumos/frequencia`,
      {
        params,
      }
    );
  },
  ListarResultados(params) {
    return api.get('v1/recuperacao-paralela/resultado', {
      params,
    });
  },
};

export default ResumosGraficosPAPServico;
