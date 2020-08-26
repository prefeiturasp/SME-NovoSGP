import api from '~/servicos/api';

const TerritorioSaberServico = {
  buscarPlanejamento(params) {
    return api.get(`/v1/planos/anual/territorio-saber`, { params });
  },
  salvarPlanejamento(params) {
    return api.post(`/v1/planos/anual/territorio-saber`, params);
  },
};

export default TerritorioSaberServico;
