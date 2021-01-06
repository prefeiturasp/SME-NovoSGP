import api from '~/servicos/api';

class ServicoRegistroIndividual {
  obterListaAlunos = ({ componenteCurricularId, turmaId }) => {
    return api.get(
      `/v1/registros-individuais/turmas/${turmaId}/componentes-curriculares/` +
        `${componenteCurricularId}/alunos`
    );
  };

  obterRegistroIndividualPorData = ({
    alunoCodigo,
    componenteCurricular,
    data,
    turmaId,
  }) => {
    return api.get(
      `/v1/registros-individuais/turmas/${turmaId}/alunos/${alunoCodigo}/` +
        `componentes-curriculares/${componenteCurricular}/data/${data}`
    );
  };

  obterRegistroIndividualPorPeriodo = ({
    alunoCodigo,
    componenteCurricular,
    dataInicio,
    dataFim,
    turmaCodigo,
  }) => {
    return api.get(
      `/v1/registros-individuais/turmas/${turmaCodigo}/alunos/${alunoCodigo}/componentes-curriculares/` +
        `${componenteCurricular}/dataInicio/${dataInicio}/dataFim/${dataFim}`
    );
  };

  obterRegistroIndividualPorId = ({ id }) => {
    return api.get(`/v1/registros-individuais/${id}`);
  };

  salvarRegistroIndividual = params => {
    return api.post('/v1/registros-individuais', params);
  };

  editarRegistroIndividual = params => {
    return api.put(`/v1/registros-individuais/${params.id}`, params);
  };

  deletarRegistroIndividual = ({ id }) => {
    return api.delete(`/v1/registros-individuais/${id}`);
  };
}

export default new ServicoRegistroIndividual();
