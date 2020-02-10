import api from '~/servicos/api';

class ServicoFechamentoFinal {
  obter = (turmaCodigo, disciplinaCodigo) => {
    return api.get(
      `v1/fechamentos/finais?DisciplinaCodigo=${disciplinaCodigo}&TurmaCodigo=${turmaCodigo}`
    );
  };
}

export default new ServicoFechamentoFinal();
