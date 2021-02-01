import api from '~/servicos/api';

const urlPadrao = 'v1/plano-aee';

class ServicoPlanoAEE {
  obterSituacoes = () => {
    return api.get(`${urlPadrao}/situacoes`);
  };
}

export default new ServicoPlanoAEE();
