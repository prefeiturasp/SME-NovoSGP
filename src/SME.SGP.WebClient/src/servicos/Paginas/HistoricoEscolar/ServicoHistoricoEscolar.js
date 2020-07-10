import api from '~/servicos/api';

class ServicoHistoricoEscolar {
  gerar = async params => {
    const url = '/v1/historico-escolar/gerar';
    return api.post(url, params);
  };
}

export default new ServicoHistoricoEscolar();
