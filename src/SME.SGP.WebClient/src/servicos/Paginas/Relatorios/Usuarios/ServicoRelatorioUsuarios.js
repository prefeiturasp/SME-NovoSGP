import api from '~/servicos/api';

class ServicoRelatorioUsuarios {
  gerar = params => {
    return api.post('v1/relatorios/usuarios/impressao', params);
  };
}

export default new ServicoRelatorioUsuarios();
