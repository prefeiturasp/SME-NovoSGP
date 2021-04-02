import api from '~/servicos/api';

const urlPadrao = 'v1/dashboard/aee';
class ServicoDashboardAEE {
  obterQuantidadeEncaminhamentosPorSituacao = (anoLetivo, dreId, ueId) => {
    let url = `${urlPadrao}/encaminhamentos/situacoes?ano=${anoLetivo}`;
    if (dreId) {
      url += `&dreId=${dreId}`;
    }
    if (ueId) {
      url += `&ueId=${ueId}`;
    }
    return api.get(url);
  };
}

export default new ServicoDashboardAEE();
