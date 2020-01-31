import axios from 'axios';
import api from '~/servicos/api';

const MockAPI = axios.create({
  baseURL: 'http://demo8322243.mockable.io/api',
});

const AcompanhamentoPAPServico = {
  ListarAlunos(params) {
    return api.get(`/v1/recuperacao-paralela/listar`, { params });
  },
  ListarEixos() {
    return MockAPI.get(`/v1/recuperacao-paralela/eixos`);
  },
  ListarObjetivos() {
    return MockAPI.get(`/v1/recuperacao-paralela/objetivos`);
  },
  ListarRespostas() {
    return MockAPI.get(`/v1/recuperacao-paralela/respostas`);
  },
  Salvar(dados) {
    return api.post(`/v1/recuperacao-paralela`, dados);
  },
};

export default AcompanhamentoPAPServico;
