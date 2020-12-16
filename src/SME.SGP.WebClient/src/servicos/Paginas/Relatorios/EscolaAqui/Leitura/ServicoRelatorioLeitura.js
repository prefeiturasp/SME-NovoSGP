import api from '~/servicos/api';

class ServicoRelatorioLeitura {
  gerar = dados => {
    return api.post(`v1/relatorios/escola-aqui/dados-leitura`, dados);
  };
}

export default new ServicoRelatorioLeitura();
