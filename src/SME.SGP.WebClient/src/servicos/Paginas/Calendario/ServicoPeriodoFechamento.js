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
}

export default new ServicoPeriodoFechamento();
