import api from '~/servicos/api';

const AcompanhamentoPAPServico = {
  ListarAlunos(params) {
    return api.get(`/v1/recuperacao-paralela/listar`, { params });
  },
  Salvar(dados) {
    return api.post(`/v1/recuperacao-paralela`, dados);
  },
};

export default AcompanhamentoPAPServico;
