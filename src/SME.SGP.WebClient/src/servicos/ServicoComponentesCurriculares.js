import api from '~/servicos/api';

class ServicoDisciplina {
  obterComponentesPorUeTurmas = (ueId, turmasId) => {
    const url = `v1/componentes-curriculares/ues/${ueId}/turmas`;
    return api.post(url, turmasId);
  };
}

export default new ServicoDisciplina();
