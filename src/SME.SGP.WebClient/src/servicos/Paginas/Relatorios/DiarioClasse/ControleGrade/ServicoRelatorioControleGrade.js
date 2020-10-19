import api from '~/servicos/api';

class ServicoRelatorioControleGrade {
  gerar = dados => {
    // TODO Validar endpoint!
    return api.post(`v1/relatorios/controle-grade`, dados);
  };
}

export default new ServicoRelatorioControleGrade();
