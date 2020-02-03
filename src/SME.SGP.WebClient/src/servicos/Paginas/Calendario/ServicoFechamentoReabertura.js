import api from '~/servicos/api';

const urlPadrao = `v1/fechamentos/reaberturas`;

class ServicoFechamentoReabertura {
  salvar = async (id, parametros, alteracaoHierarquicaConfirmacao) => {
    let url = urlPadrao;
    if (id) {
      url = `${url}/${id}`;
    }
    if (alteracaoHierarquicaConfirmacao) {
      url = `${url}/?alteracaoHierarquicaConfirmacao=${alteracaoHierarquicaConfirmacao}`;
      return api.put(url, parametros);
    }
    const metodo = id ? 'put' : 'post';
    return api[metodo](url, parametros);
  };

  obterPorId = async id => {
    return api.get(`${urlPadrao}/${id}`);
  };

  deletar = async ids => {
    const parametros = { data: ids };
    return api.delete(urlPadrao, parametros);
  };
}

export default new ServicoFechamentoReabertura();
