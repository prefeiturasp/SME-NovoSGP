import api from '~/servicos/api';

const ResumosGraficosPAPServico = {
  ListarTotalEstudantes(params) {
    return api.get(
      `https://demo7314211.mockable.io/api/v1/recuperacao-paralela/resumos/total-estudantes`,
      {
        params,
      }
    );
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
    return api.get(
      `http://demo7314211.mockable.io/api/v1/recuperacao-paralela/resumos/resultado`,
      {
        params,
      }
    );
  },
};

export default ResumosGraficosPAPServico;
