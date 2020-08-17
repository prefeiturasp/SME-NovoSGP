import api from '~/servicos/api';

class ServicoRelatorioNotasConceitos {
  gerar = dados => {
    return api.post(`v1/relatorios/notas-conceitos-finais`, dados);
  };
}

export default new ServicoRelatorioNotasConceitos();
