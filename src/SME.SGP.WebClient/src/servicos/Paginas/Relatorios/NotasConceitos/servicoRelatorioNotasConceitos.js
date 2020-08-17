import api from '~/servicos/api';

class ServicoRelatorioNotasConceitos {
  gerar = dados => {
    return api.post(``, dados);
  };
}

export default new ServicoRelatorioNotasConceitos();
