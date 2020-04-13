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
}

export default new ServicoComunicados();
