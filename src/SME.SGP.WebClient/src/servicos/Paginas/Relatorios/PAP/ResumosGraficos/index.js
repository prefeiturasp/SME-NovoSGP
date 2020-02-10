import axios from 'axios';
import api from '~/servicos/api';

const MockAPI = axios.create({
  baseURL: 'http://demo8322243.mockable.io/api',
});

const ResumosGraficosPAPServico = {
  ListarFrequencia(params) {
    return MockAPI.get(`/v1/recuperacao-paralela/resumos/frequencia`, {
      params,
    });
  },
  ListarTotalEstudantes(params) {
    return MockAPI.get(`/v1/recuperacao-paralela/resumos/total-estudantes`, {
      params,
    });
  },
};

export default ResumosGraficosPAPServico;
