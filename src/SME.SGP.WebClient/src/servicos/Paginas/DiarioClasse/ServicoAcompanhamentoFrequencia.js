import api from '~/servicos/api';

const urlPadrao = '/v1/frequencias/acompanhamentos';

class ServicoAcompanhamentoFrequencia {
  obterAcompanhamentoFrequenciaPorBimestre = async (
    turmaId,
    componenteCurricularId,
    bimestre
  ) => {
    return api.get(
      `${urlPadrao}?turmaId=${turmaId}&componenteCurricularId=${componenteCurricularId}&bimestre=${bimestre}`
    );
  };

  obterJustificativaAcompanhamentoFrequencia = async (
    turmaId,
    componenteCurricularId,
    alunoCodigo
  ) => {
    return api.get(
      `${urlPadrao}/turmas/${turmaId}/componentes-curriculares/${componenteCurricularId}/alunos/${alunoCodigo}/justificativas`
    );
  };
}

export default new ServicoAcompanhamentoFrequencia();
