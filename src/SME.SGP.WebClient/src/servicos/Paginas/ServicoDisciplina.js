import api from '~/servicos/api';

class ServicoDisciplina {
  obterDisciplinasPorTurma = turmaId => {
    const url = `v1/professores/turmas/${turmaId}/disciplinas`;
    return api.get(url);
  };

  obterDisciplinasPlanejamento = (
    codigoDisciplina,
    turmaId,
    turmaPrograma,
    regencia
  ) => {
    const url = `v1/professores/turmas/${turmaId}/disciplinas/planejamento?codigoDisciplina=${codigoDisciplina}&turmaPrograma=${!!turmaPrograma}&regencia=${!!regencia}`;
    return api.get(url);
  };
}

export default new ServicoDisciplina();
