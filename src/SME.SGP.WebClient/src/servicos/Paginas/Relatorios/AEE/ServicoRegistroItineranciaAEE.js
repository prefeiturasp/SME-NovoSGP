import api from '~/servicos/api';

const urlPadrao = 'v1/encaminhamento-aee';

class ServicoRegistroItineranciaAEE {
  obterSituacoes = () => {
    return api.get(`${urlPadrao}/situacoes`);
  };
}

export default new ServicoRegistroItineranciaAEE();
