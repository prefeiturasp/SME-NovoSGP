import axios from 'axios';
import api from '~/servicos/api';

const MockAPI = axios.create({
  baseURL: 'http://demo8322243.mockable.io/api',
});

const ResumosGraficosPAPServico = {
  ListarFrequencia(params) {
    return api.get(`/v1/recuperacao-paralela/grafico/frequencia`, {
      params,
    });
  },
  ListarTotalEstudantes(params) {
    return api.get(`/v1/recuperacao-paralela/total-estudantes`, {
      params,
    });
  },
  ListarResultados(params) {
    return api.get(`/v1/recuperacao-paralela/resultado`, {
      params,
    });
  },
};

export default ResumosGraficosPAPServico;
