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
}

export default new ServicoConselhoClasse();
