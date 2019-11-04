import api from '~/servicos/api';

class ServicoEvento {
  salvar = async (id, evento) => {
    let url = `v1/calendarios/eventos`;
    if (id) {
      url = `${url}/${id}`;
    }
    const metodo = id ? 'put' : 'post';
    return api[metodo](url, evento);
  };
}

export default new ServicoEvento();
