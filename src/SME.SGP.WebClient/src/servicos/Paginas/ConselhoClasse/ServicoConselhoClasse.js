import api from '~/servicos/api';

class ServicoConselhoClasse {
  obterListaAlunos = (turmaCodigo, anoLetivo) => {
    const url = `v1/fechamentos/turmas/${turmaCodigo}/alunos/anos/${anoLetivo}`;
    return api.get(url);
  };

  obterFrequenciaAluno = alunoCodigo => {
    const url = `v1/calendarios/frequencias/alunos/${alunoCodigo}/geral`;
    return api.get(url);
  };

  obterAnotacoesRecomendacoes = (
    codigoTurma,
    codigoAluno,
    numeroBimestre = 0,
    modalidade,
    ehFinal = false
  ) => {
    const url = `v1/conselhos-classe/recomendacoes/turmas/${codigoTurma}/alunos/${codigoAluno}/bimestres/${numeroBimestre}?modalidade=${modalidade}&ehFinal=${ehFinal}`;
    return api.get(url);
  };

  obterBimestreAtual = modalidade => {
    return api.get(
      `v1/periodo-escolar/modalidades/${modalidade}/bimestres/atual`
    );
  };

  salvarRecomendacoesAlunoFamilia = params => {
    return api.post('v1/conselhos-classe/recomendacoes', params);
  };
}

export default new ServicoConselhoClasse();
