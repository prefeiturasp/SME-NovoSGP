import api from '~/servicos/api';

const AbrangenciaServico = {
  buscarDres(url = '', consideraHistorico = false) {    
    if (url) return api.get(url, consideraHistorico = false);
    return api.get(`/v1/abrangencias/${consideraHistorico}/dres`);
  },
  buscarUes(dreId, url = '', temParametros = false, modalidade, consideraHistorico = false) {
    if (url && !temParametros)
      return api.get(`${url}/${dreId}/ues/atribuicoes`);
    if (temParametros) return api.get(url);
    return api.get(
      `/v1/abrangencias/${consideraHistorico}/dres/${dreId}/ues?modalidade=${modalidade || ''}`
    );
  },
  buscarModalidades() {
    return api.get(`v1/abrangencias/modalidades`);
  },
  /**
   * @param {String} ue Ue selecionada
   * @param {String} modalidade Modalidade Selecionada
   * @param {String} periodo Periodo (opcional)
   */
  buscarTurmas(ue, modalidade = 0, periodo = '', anoLetivo = '', consideraHistorico = false) {
    let params = { modalidade };
    if (periodo) {
      params = { ...params, periodo };
    }

    return api.get(
      `v1/abrangencias/${consideraHistorico}/dres/ues/${ue}/turmas${
        anoLetivo ? `?anoLetivo=${anoLetivo}` : ''
      }`,
      {
        params,
      }
    );
  },
  buscarDisciplinas(codigoTurma, params) {
    return api.get(`v1/professores/turmas/${codigoTurma}/disciplinas`, {
      params,
    });
  },
  buscarDisciplinasPlanejamento(codigoTurma, params) {
    return api.get(
      `v1/professores/turmas/${codigoTurma}/disciplinas/planejamento`,
      {
        params,
      }
    );
  },
  buscarTodosAnosLetivos(consideraHistorico = false) {
    return api.get(`v1/abrangencias/${consideraHistorico}/anos-letivos-todos`);
  },
  usuarioTemAbrangenciaTodasTurmas(consideraHistorico = false) {
    return api.get(`v1/abrangencias/${consideraHistorico}/adm`);
  },
  buscarAnosEscolares(codigoUe, modalidade, consideraHistorico = false) {
    return api.get(
      `v1/abrangencias/${consideraHistorico}/ues/${codigoUe}/modalidades/${modalidade}/turmas/anos`
    );
  },
};

export default AbrangenciaServico;
