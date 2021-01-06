import api from '~/servicos/api';

class ServicoOcorrencias {
  excluir = parametros => {
    return api.delete('v1/ocorrencias', parametros);
  };

  buscarTiposOcorrencias = () => {
    return api.get('v1/ocorrencias/tipos');
  };

  buscarCriancas = turmaId => {
    return api.get(
      `v1/registros-individuais/turmas/${turmaId}/componentes-curriculares/0/alunos`
    );
  };

  incluir = parametros => {
    return api.post('v1/ocorrencias', parametros);
  };

  alterar = parametros => {
    return api.put('v1/ocorrencias', parametros);
  };

  buscarOcorrencia = id => {
    return api.get(`v1/ocorrencias/${id}`);
  };
}

export default new ServicoOcorrencias();
