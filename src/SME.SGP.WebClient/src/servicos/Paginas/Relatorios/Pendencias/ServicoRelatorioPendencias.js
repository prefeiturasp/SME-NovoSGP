import api from '~/servicos/api';

class ServicoRelatorioPendencias {
  gerar = async params => {
    const url = '/v1/relatorio/pendencias-fechamento/gerar';
    return api.post(url, params);
  };
}

export default new ServicoRelatorioPendencias();
