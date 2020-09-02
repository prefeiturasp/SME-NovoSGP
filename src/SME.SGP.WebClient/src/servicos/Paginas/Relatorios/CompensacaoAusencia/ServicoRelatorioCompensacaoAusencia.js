import api from '~/servicos/api';

class ServicoRelatorioCompensacaoAusencia {
  gerar = async params => {
    const url = 'v1/relatorios/frequencias/compensacoes-ausencias';
    return api.post(url, params);
  };
}

export default new ServicoRelatorioCompensacaoAusencia();
