import api from '~/servicos/api';

class ServicoFiltro {
  listarAnosLetivos = async () => {
    await api.get('v1/abrangencia/anos-letivos').then(resposta => {
      return resposta;
    });
  };
}

export default new ServicoFiltro();
