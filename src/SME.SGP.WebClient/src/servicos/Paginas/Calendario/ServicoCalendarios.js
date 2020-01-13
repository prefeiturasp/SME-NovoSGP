import api from '~/servicos/api';

class ServicoCalendarios {
  obterTiposCalendario = async () => {
    return api
      .get('v1/calendarios/tipos')
      .then(resposta => resposta)
      .catch(() => []);
  };
}

export default new ServicoCalendarios();
