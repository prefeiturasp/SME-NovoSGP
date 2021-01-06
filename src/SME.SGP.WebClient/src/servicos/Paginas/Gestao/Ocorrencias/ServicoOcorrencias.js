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
}

export default new ServicoOcorrencias();
