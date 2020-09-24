import api from '~/servicos/api';

const urlPadrao = `/v1/devolutivas`;

class ServicoDevolutivas {
  obterSugestaoDataInicio = (turmaCodigo, componenteCurricularId) => {
    const url = `${urlPadrao}/turmas/${turmaCodigo}/componentes-curriculares/${componenteCurricularId}/sugestao`;
    return api.get(url);
  };

  salvarAlterarDevolutiva = (params, id) => {
    if (id) {
      return api.put(`${urlPadrao}/${id}`, params);
    }
    return api.post(urlPadrao, params);
  };

  deletarDevolutiva = id => {
    return api.delete(`${urlPadrao}/${id}`);
  };

  obterDevolutiva = id => {
    return api.get(`${urlPadrao}/${id}`);
  };
}

export default new ServicoDevolutivas();
