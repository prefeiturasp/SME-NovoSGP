import api from '~/servicos/api';

class ServicoAvaliacao {
  buscar = async id => {
    return api.get(`v1/atividade-avaliativa/${id}`);
  };

  salvar = async (id, dados) => {
    const url = `v1/atividade-avaliativa/${id}`;
    const metodo = id ? 'put' : 'post';
    return api[metodo](url, dados);
  };
}

export default new ServicoAvaliacao();
