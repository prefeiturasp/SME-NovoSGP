import api from '~/servicos/api';

class ServicoDisciplina {
  obterDisciplinasPorTurma = turmaId => {
    const url = `v1/professores/0/turmas/${turmaId}/disciplinas`;
    return api.get(url);
  };
}

export default new ServicoDisciplina();
