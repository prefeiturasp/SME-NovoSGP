import api from '~/servicos/api';

class ServicoFaltasFrequencia {
  gerar = dados => {
    return api.post(`v1/relatorios/faltas-frequencia`, dados);
  };
}

export default new ServicoFaltasFrequencia();
