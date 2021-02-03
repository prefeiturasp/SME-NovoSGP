import api from '~/servicos/api';

const urlPadrao = '';

class ServicoRegistroItineranciaAEE {
  obterObjetivos = () => {
    return api.get(`${urlPadrao}/objetivos`);
  };
}

export default new ServicoRegistroItineranciaAEE();
