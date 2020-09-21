import api from '~/servicos/api';

const anoAtual = window.moment().format('YYYY');

const AtribuicaoCJServico = {
  buscarLista(params) {
    return api.get(`/v1/atribuicoes/cjs`, { params });
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
    return api.get(`/v1/ues/${ue}/modalidades?ano=${anoAtual}`);
  },
  buscarTurmas(ue, modalidade, anoLetivo) {
    const anoCorrente = anoLetivo || anoAtual;
    return api.get(
      `/v1/ues/${ue}/modalidades/${modalidade}?ano=${anoCorrente}`
    );
  },
};

export default AtribuicaoCJServico;
