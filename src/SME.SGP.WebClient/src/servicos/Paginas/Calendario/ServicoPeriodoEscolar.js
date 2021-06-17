import moment from 'moment';
import api from '~/servicos/api';

class ServicoPeriodoEscolar {
  obterPeriodosAbertos = async (turma, dataReferencia = null) => {
    let url = `v1/periodo-escolar/turmas/${turma}/bimestres/aberto`;
    if (dataReferencia) {
      dataReferencia = moment(dataReferencia).format('YYYY-MM-DD');
      url = `${url}?dataReferencia=${dataReferencia}`;
    }
    return api.get(url);
  };

  obterPeriodosPorAnoLetivoModalidade = async (modalidade, anoLetivo) => {
    const url = `v1/periodo-escolar/modalidades/${modalidade}/ano-letivo/${anoLetivo}/bimestres`;
    return api.get(url);
  };

  obterBimestresPorTurmaId = turmaId => {
    return api.get(`v1/periodo-escolar/turmas/${turmaId}`);
  };

  obterBimestreAtualPorTurmaId = turmaId => {
    return api.get(`v1/periodo-escolar/turmas/${turmaId}/bimestres/atual`);
  };

  obterPeriodoLetivoTurma = async (codigoTurma) => {
    const url = `v1/periodo-escolar/turmas/${codigoTurma}/periodo-letivo`;
    return api.get(url);
  };
}

export default new ServicoPeriodoEscolar();
