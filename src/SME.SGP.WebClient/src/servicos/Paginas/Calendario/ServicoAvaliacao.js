import api from '~/servicos/api';

class ServicoAvaliacao {
  listarDisciplinas = async (rf, turma) => {
    return api.get(`v1/professores/${rf}/turmas/${turma}/disciplinas`);
  };

  listarTipos = async () => {
    return api.get('v1/atividade-avaliativa/tipos/listar');
  };

  buscar = async id => {
    return api.get(`v1/atividade-avaliativa/${id}`);
  };

  validar = async dados => {
    return api
      .post('v1/atividade-avaliativa/validar', dados)
      .then(resposta => resposta)
      .catch(err => err.response.data.mensagens[0]);
  };

  salvar = async (id, dados) => {
    const url = `v1/atividade-avaliativa/${id}`;
    const metodo = id ? 'put' : 'post';
    return api[metodo](url, dados);
  };

  excluir = async id => {
    return api
      .delete(`v1/atividade-avaliativa?id=${id}`)
      .then(resposta => resposta)
      .catch(err => err.response.data.mensagens[0]);
  };
}

export default new ServicoAvaliacao();
