import api from '~/servicos/api';

// TODO Alterar quando tiver o back pronto
const urlPadrao = `/v1/compensacoes/ausencia`;

class ServicoCompensacaoAusencia {
  buscarLista = params => {
    return api.get(urlPadrao, { params });
  };

  salvar = async (id, compensacao) => {
    let url = urlPadrao;
    if (id) {
      url = `${url}/${id}`;
    }
    const metodo = id ? 'put' : 'post';
    return api[metodo](url, compensacao);
  };

  obterPorId = async id => {
    return api.get(`${urlPadrao}/${id}`);
  };

  deletar = async ids => {
    const parametros = { data: ids };
    return api.delete(urlPadrao, parametros);
  };
}

export default new ServicoCompensacaoAusencia();
