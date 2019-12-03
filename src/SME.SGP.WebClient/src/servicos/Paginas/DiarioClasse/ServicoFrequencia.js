import api from '~/servicos/api';

class ServicoFrequencia {
  obterDisciplinas = turmaId => {
    const url = `v1/calendarios/frequencias/turmas/${turmaId}/disciplinas`;
    return api.get(url);
  };
}

export default new ServicoFrequencia();
