import api from '~/servicos/api';
import * as moment from 'moment';

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

  verificarSePodeAlterarNoPeriodo = async (
    turmaCodigo,
    bimestre,
    dataReferencia = ''
  ) => {
    let url = `/v1/periodo-escolar/bimestres/${bimestre}/turmas/${turmaCodigo}/aberto`;
    if (dataReferencia) {
      dataReferencia = moment(dataReferencia).format('YYYY-MM-DD');
      url = `${url}?dataReferencia=${dataReferencia}`;
    }
    return api.get(url);
  };
}

export default new ServicoPeriodoFechamento();
