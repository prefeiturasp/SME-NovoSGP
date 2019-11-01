import api from '~/servicos/api';

const urlPadrao = `v1/calendarios/eventos`;
class ServicoEvento {
  salvar = async (id, evento) => {
    let url = urlPadrao;
    if (id) {
      url = `${url}/${id}`;
    }
    const metodo = id ? 'put' : 'post';
    return api[metodo](url, evento);
  };

  obterPorId = async id => {
    return api.get(`${urlPadrao}/${id}`);
  };

  deletar = async ids => {
    const parametros = { data: ids };
    return api.delete(urlPadrao, parametros);
  };
}

export default new ServicoEvento();
