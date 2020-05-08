import api from '~/servicos/api';

const CalendarioProfessorServico = {
  buscarTiposCalendario(turma) {
    return api.get(`/v1/turmas/${turma}/tipo-calendario`);
  },
  buscarEventosAulasMes(params) {
    return api.get(
      `/v1/calendarios/${params.tipoCalendarioId}/meses/${params.numeroMes}/eventos-aulas?ueCodigo=${params.ue}&dreCodigo=${params.dre}&anoLetivo=${params.anoLetivo}&turmaCodigo=${params.turma}`
    );
  },
  buscarEventosAulasDia(params) {
    return api.get(
      `v1/calendarios/${params.tipoCalendarioId}/meses/${params.numeroMes}/dias/${params.dia}/eventos-aulas?ueCodigo=${params.ue}&dreCodigo=${params.dre}&anoLetivo=${params.anoLetivo}&turmaCodigo=${params.turma}`
    );
  },
};

export default CalendarioProfessorServico;
