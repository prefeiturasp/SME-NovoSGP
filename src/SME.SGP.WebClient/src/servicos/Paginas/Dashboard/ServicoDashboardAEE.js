import api from '~/servicos/api';

const urlPadrao = 'v1/dashboard/aee';
class ServicoDashboardAEE {
  montarConsultaPadraoGraficos = (
    rota,
    anoLetivo,
    dreId,
    ueId,
    dreCodigo,
    ueCodigo
  ) => {
    let url = `${urlPadrao}/${rota}?anoLetivo=${anoLetivo}`;
    if (dreId) {
      url += `&dreId=${dreId}`;
    }
    if (ueId) {
      url += `&ueId=${ueId}`;
    }
    if (dreCodigo) {
      url += `&dreCodigo=${dreCodigo}`;
    }
    if (ueCodigo) {
      url += `&ueCodigo=${ueCodigo}`;
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

  obterSituacoesPlanos = (anoLetivo, dreId, ueId) => {
    return this.montarConsultaPadraoGraficos(
      'planos/situacoes',
      anoLetivo,
      dreId,
      ueId
    );
  };

  obterPlanosAcessibilidades = (anoLetivo, dreId, ueId) => {
    return this.montarConsultaPadraoGraficos(
      'planos/acessibilidades',
      anoLetivo,
      dreId,
      ueId
    );
  };

  obterPlanosVigentes = (anoLetivo, dreId, ueId) => {
    return this.montarConsultaPadraoGraficos(
      'planos/vigentes',
      anoLetivo,
      dreId,
      ueId
    );
  };

  obterQuantidadeEstudantesMatriculados = (
    anoLetivo,
    dreId,
    ueId,
    dreCodigo,
    ueCodigo
  ) => {
    return this.montarConsultaPadraoGraficos(
      'encaminhamentos/matriculados-srm-paee',
      anoLetivo,
      dreId,
      ueId,
      dreCodigo,
      ueCodigo
    );
  };
}

export default new ServicoDashboardAEE();
