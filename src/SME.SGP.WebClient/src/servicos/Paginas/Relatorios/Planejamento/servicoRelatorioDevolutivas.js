import api from '~/servicos/api';

class ServicoRelatorioDevolutivas {
  gerar = params => {
    return api.post('', params);
  };
}

export default new ServicoRelatorioDevolutivas();
