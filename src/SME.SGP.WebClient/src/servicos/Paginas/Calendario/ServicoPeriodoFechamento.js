import api from '~/servicos/api';

class ServicoPeriodoFechamento {
  obterPorTipoCalendarioDreEUe = async (
    tipoCalendarioSelecionado,
    dreId,
    ueId
  ) => {
    return api.get(
      `/v1/periodos/fechamentos/aberturas?tipoCalendarioId=${tipoCalendarioSelecionado}&dreId=${dreId}&ueId=${ueId}`
    );
  };

  salvar = async fechamento => {
    return api.post('/v1/periodos/fechamentos/aberturas', fechamento);
  };

  verificarSePodeAlterarNoPeriodo = async (turmaCodigo, bimestre, dataReferencia?) => {
    if (dataReferencia) {
      dataReferencia = window.moment(dataReferencia).format('YYYY-MM-DD');
    }
    return api.get(
      `/v1/periodos/fechamentos/aberturas/turmas/${turmaCodigo}/bimestres/
      ${bimestre}/aberto?dataReferencia=2015-01-01`);
  };
}

export default new ServicoPeriodoFechamento();
