import api from '~/servicos/api';

class ServicoTipoEvento {
  salvar = async (id, evento) => {
    const url = `v1/calendarios/eventos/tipos/${id}`;
    const metodo = id ? 'put' : 'post';
    return api[metodo](url, evento);
  };
}

export default new ServicoTipoEvento();
