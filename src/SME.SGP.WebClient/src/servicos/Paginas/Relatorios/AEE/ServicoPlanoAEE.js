import api from '~/servicos/api';

const urlPadrao = 'v1/plano-aee';

class ServicoPlanoAEE {
  obterSituacoes = () => {
    return api.get(`${urlPadrao}/situacoes`);
  };

  obterQuestionario = () => {
    return {
      data: [
        {
          id: 2,
          nome: '2 - Organização do AEE',
          obrigatorio: true,
          observacao: '',
          opcaoResposta: [
            {
              id: 3,
              nome: 'Sim',
              ordem: 1,
            },
          ],
          opcionais: '',
          ordem: 2,
          resposta: [],
          tipoQuestao: 3,
        },
      ],
    };
  };
}

export default new ServicoPlanoAEE();
