import api from '~/servicos/api';

const ServicoCadastroAula = {
  BuscarGrades(turmaId, disciplinaId, regencia, params) {
    return api.get(
      `v1/grades/aulas/turmas/${turmaId}/disciplinas/${disciplinaId}?ehRegencia=${regencia}`,
      params
    );
  },
  BuscarAula(id) {
    return api.get(`v1/calendarios/professores/aulas/${id}`);
  },
  BuscarRecorrencias(id) {
    return api.get(`v1/calendarios/professores/aulas/${id}/recorrencias/serie`);
  },
};

export default ServicoCadastroAula;
