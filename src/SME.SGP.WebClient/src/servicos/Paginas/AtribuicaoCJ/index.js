import api from '~/servicos/api';

const AtribuicaoCJServico = {
  buscarLista(params) {
    return api.get(`/v1/atribuicoes/cjs`, params);
  },
  buscarAtribuicoes(ue, modalidade, turma, professorRf) {
    return api.get(
      `/v1/atribuicoes/cjs/ues/${ue}/modalidades/${modalidade}/turmas/${turma}/professores/${professorRf}`
    );
  },
  salvarAtribuicoes(data) {
    return api.post(`/v1/atribuicoes/cjs`, data);
  },
  buscarModalidades(ue) {
    return api.get(`/v1/ues/${ue}/modalidades?ano=2019`);
  },
  buscarTurmas(ue, modalidade) {
    return api.get(`/v1/ues/${ue}/modalidades/${modalidade}?ano=2019`);
  },
};

export default AtribuicaoCJServico;
