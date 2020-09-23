import api from '~/servicos/api';

class ServicoPlanoAnual {
  obter = (anoLetivo, componenteCurricularEolId, ueId, turmaId) => {
    const url = `v1/planos/anual?turmaId=${turmaId}&ueId=${ueId}&anoLetivo=${anoLetivo}&componenteCurricularEolId=${componenteCurricularEolId}`;
    return api.get(url);
  };

  obterObjetivosPorAnoEComponenteCurricular = (
    anoLetivo,
    ensinoEspecial,
    disciplinas
  ) => {
    return api.post('v1/objetivos-aprendizagem', {
      ano: anoLetivo,
      ensinoEspecial,
      ComponentesCurricularesIds: disciplinas,
    });
  };

  salvar = planoAnual => {
    return api.post('v1/planos/anual', planoAnual);
  };

  obterTurmasParaCopia = (turmaId, componenteCurricularEolId) => {
    return api.get(
      `v1/planos/anual/turmas/copia?turmaId=${turmaId}&componenteCurricular=${componenteCurricularEolId}`
    );
  };

  copiarConteudo = migrarConteudoPlanoAnual => {
    return api.post(`v1/planos/anual/migrar`, migrarConteudoPlanoAnual);
  };

  obterBimestresPlanoAnual = (modalidade, anoLetivo) => {
    const url = `v1/periodo-escolar/modalidades/${modalidade}/anos-letivos/${anoLetivo}`;
    return api.get(url);
  };

  obterDadosPlanoAnualPorComponenteCurricular = (
    turmaId,
    componenteCurricularId,
    periodoEscolarId
  ) => {
    const url = `v1/planejamento/anual/turmas/${turmaId}/componentes-curriculares/${componenteCurricularId}/periodos-escolares/${periodoEscolarId}`;
    return api.get(url);
  };
}

export default new ServicoPlanoAnual();
