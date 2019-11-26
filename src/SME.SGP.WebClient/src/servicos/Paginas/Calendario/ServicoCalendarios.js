import api from '~/servicos/api';

class ServicoCalendarios {
  static obterTiposCalendario = async () => {
    return api
      .get('v1/calendarios/tipos')
      .then(resposta => resposta)
      .catch(() => []);
  };
}

export default ServicoCalendarios;
