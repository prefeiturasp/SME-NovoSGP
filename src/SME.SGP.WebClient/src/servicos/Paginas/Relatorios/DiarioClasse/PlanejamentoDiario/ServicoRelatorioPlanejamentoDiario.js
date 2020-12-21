import api from '~/servicos/api';

class ServicoRelatorioPlanejamentoDiario {
  gerar = dados => {
    return api.post(`v1/relatorios/planejamento-diario/impressao`, dados);
  };
}

export default new ServicoRelatorioPlanejamentoDiario();
