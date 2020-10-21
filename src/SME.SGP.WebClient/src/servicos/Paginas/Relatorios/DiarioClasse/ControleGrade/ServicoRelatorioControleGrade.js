import api from '~/servicos/api';

class ServicoRelatorioControleGrade {
  gerar = dados => {
    return api.post(`v1/relatorios/controle-grade/impressao`, dados);
  };
}

export default new ServicoRelatorioControleGrade();
