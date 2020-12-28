import api from '~/servicos/api';

class ServicoRegistroIndividual {
  obterListaAlunos = ({ componenteCurricularId, turmaId }) => {
    return api.get(
      `/v1/registros-individuais/turmas/${turmaId}/componentes-curriculares/${componenteCurricularId}/alunos`
    );
  };
}

export default new ServicoRegistroIndividual();
