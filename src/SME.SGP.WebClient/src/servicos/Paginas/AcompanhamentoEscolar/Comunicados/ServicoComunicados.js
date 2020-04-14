import api from '../../../api';

class ServicoComunicados {
  listarGrupos = async () => {
    const lista = [];

    try {
      const requisicao = await api.get('v1/comunicacao/grupos/listar');
      if (requisicao.data) {
        requisicao.data.forEach(grupo => {
          lista.push({
            id: grupo.id,
            nome: grupo.nome,
          });
        });
      }
    } catch {
      return lista;
    }

    return lista;
  };

  consultarPorId = async id => {
    let comunicado = {};

    try {
      const requisicao = await api.get(`v1/comunicado/${id}`);
      if (requisicao.data) comunicado = requisicao.data;
    } catch {
      return comunicado;
    }

    return comunicado;
  };

  salvar = async dados => {
    let salvou = {};

    let metodo = 'post';
    let url = 'v1/comunicado';

    if (dados.id && dados.id > 0) {
      metodo = 'put';
      url = `${url}/${dados.id}`;
    }

    try {
      const requisicao = await api[metodo](url, dados);
      if (requisicao.data) salvou = requisicao;
    } catch (erro) {
      salvou = [...erro.response.data.mensagens];
    }

    return salvou;
  };

  excluir = async ids => {
    let exclusao = {};
    const parametros = { data: ids };

    try {
      const requisicao = await api.delete('v1/comunicado', parametros);
      if (requisicao && requisicao.status === 200) exclusao = requisicao;
    } catch (erro) {
      exclusao = [...erro.response.data.mensagens];
    }

    return exclusao;
  };
}

export default new ServicoComunicados();
