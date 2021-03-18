import api from '~/servicos/api';

const urlPadrao = '/v1/acompanhamento/alunos';

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

  salvarAcompanhamentoAprendizagem = params => {
    return api.get(`${urlPadrao}/semestres`, params);
  };

  uploadFoto = (formData, configuracaoHeader) => {
    return api.post(
      `${urlPadrao}/semestres/upload`,
      formData,
      configuracaoHeader
    );
  };

  obterFotos = acompanhamentoAlunoSemestreId => {
    return api.get(
      `${urlPadrao}/semestres/${acompanhamentoAlunoSemestreId}/fotos`
    );
  };

  excluirFotos = (acompanhamentoAlunoSemestreId, codigoFoto) => {
    return api.delete(
      `${urlPadrao}/semestres/${acompanhamentoAlunoSemestreId}/fotos/${codigoFoto}`
    );
  };
}

export default new ServicoAcompanhamentoAprendizagem();
