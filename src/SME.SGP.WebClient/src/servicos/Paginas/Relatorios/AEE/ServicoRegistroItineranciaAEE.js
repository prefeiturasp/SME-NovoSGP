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
    if (itinerancia.id) {
      return api.put(urlPadrao, itinerancia);
    }
    return api.post(urlPadrao, itinerancia);
  };

  obterSituacoes = () => {
    return api.get(`${urlPadrao}/situacoes`);
  };

  obterAnosLetivos = () => {
    return api.get(`${urlPadrao}/anos-letivos`);
  };

  gerarRelatorio = ids => {
    return api.post('/v1/relatorios/itinerancias', ids);
  };

  obterEventos = (tipoCalendarioId, itineranciaId) => {
    let url = `${urlPadrao}/eventos?tipoCalendarioId=${tipoCalendarioId}`;
    if (itineranciaId) {
      url += `&itineranciaId=${itineranciaId}`;
    }
    return api.get(url);
  };
}

export default new ServicoRegistroItineranciaAEE();
