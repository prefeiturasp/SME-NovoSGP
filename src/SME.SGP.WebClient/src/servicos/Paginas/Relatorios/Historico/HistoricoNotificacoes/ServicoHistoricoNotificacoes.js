import api from '~/servicos/api';

class ServicoHistoricoNotificacoes {
  gerar = dados => {
    return api.post(`v1/relatorios/notificacoes/impressao`, dados);
  };
}

export default new ServicoHistoricoNotificacoes();
