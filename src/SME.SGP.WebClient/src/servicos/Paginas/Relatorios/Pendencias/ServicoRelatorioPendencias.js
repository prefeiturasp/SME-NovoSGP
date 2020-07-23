import api from '~/servicos/api';

class ServicoRelatorioPendencias {
  gerar = async params => {
    const url = '/v1/relatorios/fechamentos/pendencias';
    return api.post(url, params);
  };
}

export default new ServicoRelatorioPendencias();
