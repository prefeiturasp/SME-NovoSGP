import api from '~/servicos/api';

class ServicoHistoricoAlteracoesNotas {
  gerar = dados => {
    return api.post(`v1/relatorios/historico-alteracao-notasa`, dados);
  };
}

export default new ServicoHistoricoAlteracoesNotas();
