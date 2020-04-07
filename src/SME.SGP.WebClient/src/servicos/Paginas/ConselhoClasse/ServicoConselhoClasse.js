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

  // TODO
  obterAnotacaoAluno = (codigoTurma, codigoAluno, numeroBimestre = 0) => {
    const url = `v1/conselhos-classe/anotacoes-aluno/turmas/${codigoTurma}/alunos/${codigoAluno}/bimestres/${numeroBimestre}`;
    return api.get(url);
  };

  // TODO
  obterRecomendacoesAluno = (codigoTurma, codigoAluno, numeroBimestre = 0) => {
    const url = `v1/conselhos-classe/recomendacoes/turmas/${codigoTurma}/alunos/${codigoAluno}/bimestres/${numeroBimestre}`;
    return api.get(url);
  };
}

export default new ServicoConselhoClasse();
