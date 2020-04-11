import api from '../../../api';

class ServicoComunicados {
  listarGrupos = async () => {
    const lista = [];

    try {
      const requisicao = await api.get('v1/comunicacao/grupos/listar');
      if (requisicao.data) {
        requisicao.data.forEach(grupo => {
          lista.push({
            Id: grupo.id,
            Nome: grupo.nome.replace('�', 'é'),
          });
        });
      }
    } catch {
      return lista;
    }

    return lista;
  };

  consultarPorId = async id => {
    return id;
  };
}

export default new ServicoComunicados();
