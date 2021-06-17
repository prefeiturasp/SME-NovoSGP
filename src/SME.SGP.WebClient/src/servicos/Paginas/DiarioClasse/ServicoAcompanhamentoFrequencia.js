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
    alunoCodigo,
    bimestre
  ) => {
    return api.get(
      `${urlPadrao}/turmas/${turmaId}/componentes-curriculares/${componenteCurricularId}/alunos/${alunoCodigo}/bimestres/${bimestre}/justificativas`
    );
  };

  obterJustificativaAcompanhamentoFrequenciaPaginacaoManual = (
    turmaId,
    componenteCurricularId,
    alunoCodigo,
    bimestre,
    numeroPagina,
    numeroRegistros
  ) => {
    const url = `${urlPadrao}/turmas/${turmaId}/componentes-curriculares/${componenteCurricularId ||
      0}/alunos/${alunoCodigo}/bimestres/${bimestre}/justificativas?numeroPagina=${numeroPagina ||
      1}&numeroRegistros=${numeroRegistros}`;

    return api.get(url);
  };

  obterInformacoesDeFrequenciaAlunoPorSemestre = (
    turmaId,
    semestre,
    alunoCodigo
  ) => {
    const url = `${urlPadrao}/turmas/${turmaId}/semestres/${semestre}/alunos/${alunoCodigo}`;
    return api.get(url);
  };
}

export default new ServicoAcompanhamentoFrequencia();
