import api from '~/servicos/api';

const PlanoAulaServico = {
  verificarSeExiste(parametros) {
    return api.post(`/v1/planos/aulas/validar-existente/`, parametros);
  },
  migrarPlano(dados) {
    return api.post(`/v1/planos/aulas/migrar`, dados);
  },
};

export default PlanoAulaServico;
