import api from '~/servicos/api';

class ServicoAdesaoEscolaAqui {
  gerar = params => {
    return api.post('v1/relatorios/ae/adesao', params);
  };
}

export default new ServicoAdesaoEscolaAqui();
