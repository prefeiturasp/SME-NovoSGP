import api from '~/servicos/api';

const urlPadrao = 'v1/itinerancias';

class ServicoRegistroItineranciaAEE {
  obterObjetivos = () => {
    return api.get(`${urlPadrao}/objetivos`);
  };

  obterItineranciaPorId = id => {
    return api.get(`${urlPadrao}/${id}`);
  };

  obterQuestoesItinerancia = () => {
    return api.get(`${urlPadrao}/questoes`);
  };
}

export default new ServicoRegistroItineranciaAEE();
