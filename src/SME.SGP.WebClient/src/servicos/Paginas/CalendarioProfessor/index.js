import api from '~/servicos/api';

const CalendarioProfessorServico = {
  buscarEventosAulasMes(params) {
    return api.post(`/v1/calendarios/meses/tipos/eventos-aulas`, params);
  },
};

export default CalendarioProfessorServico;
