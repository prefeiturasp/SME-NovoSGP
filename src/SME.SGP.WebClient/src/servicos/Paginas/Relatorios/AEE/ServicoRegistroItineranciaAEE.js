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

  obterQuestoesItineranciaPorId = id => {
    return api.get(`${urlPadrao}/alunos/questoes/${id}`);
  };

  salvarItinerancia = itinerancia => {
    return api.post(urlPadrao, itinerancia);
  }
}

export default new ServicoRegistroItineranciaAEE();
