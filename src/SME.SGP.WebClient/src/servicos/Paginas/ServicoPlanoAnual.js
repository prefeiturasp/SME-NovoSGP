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
}

export default new ServicoPlanoAnual();
