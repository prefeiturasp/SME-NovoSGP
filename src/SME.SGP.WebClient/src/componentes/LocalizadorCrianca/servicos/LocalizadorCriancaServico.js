import api from '~/servicos/api';

class LocalizadorGenericoServico {
  buscarDados = url => {
    return api.get(url);
  };
}

export default new LocalizadorGenericoServico();
