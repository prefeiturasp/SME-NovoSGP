import api from '~/servicos/api';

class ServicoRelatorioCompensacaoAusencia {
  gerar = async params => {
    const url = 'v1/relatorios/frequencia/compensacao-ausencia';
    return api.post(url, params);
  };
}

export default new ServicoRelatorioCompensacaoAusencia();
