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
    return Promise.resolve({
      status: 200,
      data: {
        questoesItinerancia: [
          {
            id: 0,
            questaoId: 1,
            descricao: 'Acompanhamento da situação',
          },
          {
            id: 0,
            questaoId: 2,
            descricao: 'Encaminhamentos',
          },
        ],
        questoesItineranciaAluno: [
          {
            id: 0,
            questaoId: 1,
            descricao: 'Acompanhamento da situação',
          },
          {
            id: 0,
            questaoId: 2,
            descricao: 'Encaminhamentos',
          },
        ],
      },
    });
  };
}

export default new ServicoRegistroItineranciaAEE();
