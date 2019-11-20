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

  listarDres = async () => {
    return api
      .get('v1/abrangencias/dres')
      .then(res => {
        return { sucesso: true, conteudo: res.data };
      })
      .catch(err => {
        return {
          sucesso: false,
          erro: 'ocorreu uma falha ao consultar as dres',
        };
      });
  };

  listarUes = async dre => {
    return api
      .get(`v1/abrangencias/dres/${dre}/ues`)
      .then(res => {
        return { sucesso: true, conteudo: res.data };
      })
      .catch(err => {
        return {
          sucesso: false,
          erro: 'ocorreu uma falha ao consultar as unidades escolares',
        };
      });
  };
}

export default new ServicoEvento();
