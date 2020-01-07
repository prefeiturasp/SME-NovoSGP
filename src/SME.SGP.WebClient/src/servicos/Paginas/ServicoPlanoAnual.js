import api from '~/servicos/api';

class ServicoPlanoAnual {
  obter = (anoLetivo, componenteCurricularEolId, ueId, turmaId) => {
    const url = `v1/planos/anual?turmaId=${turmaId}&ueId=${ueId}&anoLetivo=${anoLetivo}&componenteCurricularEolId=${componenteCurricularEolId}`;
    return api.get(url);
  };

  obterObjetivosPorAnoEComponenteCurricular = (anoLetivo, disciplinas) => {
    return api.post('v1/objetivos-aprendizagem', {
      ano: anoLetivo,
      ComponentesCurricularesIds: disciplinas,
    });
  };

  salvar = planoAnual => {
    return api.post('v1/planos/anual', planoAnual);
  };
}

export default new ServicoPlanoAnual();
