import api from '~/servicos/api';

const AbrangenciaServico = {
  buscarDres(url = '', consideraHistorico = false) {
    if (url) return api.get(url, (consideraHistorico = false));
    return api.get(`/v1/abrangencias/${consideraHistorico}/dres`);
  },
  buscarUes(
    dreId,
    url = '',
    temParametros = false,
    modalidade,
    consideraHistorico = false
  ) {
    if (url && !temParametros)
      return api.get(`${url}/${dreId}/ues/atribuicoes`);
    if (temParametros) return api.get(url);
    return api.get(
      `/v1/abrangencias/${consideraHistorico}/dres/${dreId}/ues?modalidade=${modalidade ||
        ''}`
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
  buscarTurmas(
    ue,
    modalidade = 0,
    periodo = '',
    anoLetivo = '',
    consideraHistorico = false,
    turmasRegulares = false,
    tipos = undefined
  ) {
    let params = {};
    if (modalidade) {
      params = { modalidade };
    }
    const action = turmasRegulares ? 'turmas-regulares' : 'turmas';
    const adicionaTipos = tipos
      ? `&${tipos.map(item => `tipos=${item}`).join('&')}`
      : '';

    if (periodo) {
      params = { ...params, periodo };
    }

    return api.get(
      `v1/abrangencias/${consideraHistorico}/dres/ues/${ue}/${action}${
        anoLetivo ? `?anoLetivo=${anoLetivo}${adicionaTipos}` : ''
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
  obterSemestres(consideraHistorico, anoLetivo, modalidade) {
    const url = `v1/abrangencias/${consideraHistorico}/semestres?anoLetivo=${anoLetivo}&modalidade=${modalidade ||
      0}`;

    return api.get(url);
  },
};

export default AbrangenciaServico;
