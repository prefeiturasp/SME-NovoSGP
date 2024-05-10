import api from '~/servicos/api';

class ServicoRelatorioPlanejamentoDiario {
  gerar = dados => {
    return api.post(`v1/relatorios/diario-classe/planejamento-diario`, dados);
  };
}

export default new ServicoRelatorioPlanejamentoDiario();
