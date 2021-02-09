import api from '~/servicos/api';

const urlPadrao = 'v1/plano-aee';

class ServicoPlanoAEE {
  obterSituacoes = () => {
    return api.get(`${urlPadrao}/situacoes`);
  };

  obterPlanoPorId = planoId => {
    return api.get(`${urlPadrao}/${planoId}`);
  };

  obterPlanoPorCodigoEstudante = codigoEstudante => {
    return api.get(`${urlPadrao}/estudante/${codigoEstudante}`);
  };

  obterVersaoPlanoPorId = versaoPlanoId => {
    return api.get(`${urlPadrao}/versao/${versaoPlanoId}`);
  };

  obterQuestionario = (questionarioId, planoId, codigoAluno, codigoTurma) => {
    let url = `${urlPadrao}/questionario?questionarioId=${questionarioId}&codigoAluno=${codigoAluno}&codigoTurma=${codigoTurma}`;
    if (planoId) {
      url = `${url}&planoId=${planoId}`;
    }
    return api.get(url);
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
