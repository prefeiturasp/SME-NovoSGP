import api from '~/servicos/api';

class ServicoRelatorioAtribuicoes {
  gerar = params => {
    return api.post('v1/relatorios/atribuicoes/cjs', params);
  };
}

export default new ServicoRelatorioAtribuicoes();
