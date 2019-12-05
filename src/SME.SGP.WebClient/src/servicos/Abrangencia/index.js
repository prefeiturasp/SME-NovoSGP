import api from '~/servicos/api';

const AbrangenciaServico = {
  buscarDres() {
    return api.get('/v1/abrangencias/dres');
  },
  buscarUes(dreId) {
    return api.get(`/v1/abrangencias/dres/${dreId}/ues`);
  },
  buscarModalidades() {
    return api.get(`v1/abrangencias/modalidades`);
  },
  /**
   * @param {String} ue Ue selecionada
   * @param {String} modalidade Modalidade Selecionada
   * @param {String} periodo Periodo (opcional)
   */
  buscarTurmas(ue, modalidade = 0, periodo = '') {
    let params = { modalidade };
    if (periodo) {
      params = { ...params, periodo };
    }

    return api.get(`v1/abrangencias/dres/ues/${ue}/turmas`, {
      ...params,
    });
  },
};

export default AbrangenciaServico;
