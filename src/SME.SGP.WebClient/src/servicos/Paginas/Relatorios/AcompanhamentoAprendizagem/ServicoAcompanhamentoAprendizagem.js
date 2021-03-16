import api from '~/servicos/api';

class ServicoAcompanhamentoAprendizagem {
  obterListaAlunos = (turmaCodigo, anoLetivo, periodo) => {
    // TODO Trocar endpoint!
    const url = `v1/fechamentos/turmas/${turmaCodigo}/alunos/anos/${anoLetivo}/semestres/${periodo}`;
    return api.get(url);
  };

  obterListaSemestres = () => {
    return new Promise(resolve => {
      resolve({
        data: [
          {
            semestre: '1',
            descricao: '1º Semestre',
          },
          {
            semestre: '2',
            descricao: '2º Semestre',
          },
        ],
      });
    });
  };
}

export default new ServicoAcompanhamentoAprendizagem();
