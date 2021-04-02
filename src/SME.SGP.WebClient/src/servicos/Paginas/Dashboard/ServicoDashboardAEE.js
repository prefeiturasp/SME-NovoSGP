import api from '~/servicos/api';

const urlPadrao = 'v1/dashboard/aee';
class ServicoDashboardAEE {
  montarConsultaPadraoGraficos = (rota, anoLetivo, dreId, ueId) => {
    let url = `${urlPadrao}/${rota}?ano=${anoLetivo}`;
    if (dreId) {
      url += `&dreId=${dreId}`;
    }
    if (ueId) {
      url += `&ueId=${ueId}`;
    }
    return api.get(url);
  };

  obterQuantidadeEncaminhamentosPorSituacao = (anoLetivo, dreId, ueId) => {
    return this.montarConsultaPadraoGraficos(
      'encaminhamentos/situacoes',
      anoLetivo,
      dreId,
      ueId
    );
  };

  obterEncaminhamentosDeferidos = (anoLetivo, dreId, ueId) => {
    return this.montarConsultaPadraoGraficos(
      'encaminhamentos/deferidos',
      anoLetivo,
      dreId,
      ueId
    );
  };
}

export default new ServicoDashboardAEE();
