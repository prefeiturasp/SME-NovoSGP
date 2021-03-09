import api from '~/servicos/api';

class ServicoRelatorioDevolutivas {
  gerar = params => {
    return api.post('/v1/relatorios/devolutivas', params);
  };
}

export default new ServicoRelatorioDevolutivas();
