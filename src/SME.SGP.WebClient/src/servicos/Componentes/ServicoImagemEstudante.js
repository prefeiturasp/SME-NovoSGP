import api from '../api';

class ServicoImagemEstudante {
  obterImagemEstudante = codigoAluno => {
    return api.get(`v1/estudante/${codigoAluno}/foto`);
  };

  uploadImagemEstudante = (formData, configuracaoHeader) => {
    const codigoAluno = formData.get('codigoAluno');
    return api.post(
      `v1/estudante/${codigoAluno}/foto`,
      formData,
      configuracaoHeader
    );
  };

  excluirImagemEstudante = codigoAluno => {
    return api.delete(`v1/estudante/${codigoAluno}/foto`);
  };
}

export default new ServicoImagemEstudante();
