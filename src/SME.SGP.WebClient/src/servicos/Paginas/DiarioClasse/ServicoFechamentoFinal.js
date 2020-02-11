import api from '~/servicos/api';

class ServicoFechamentoFinal {
  obter = (turmaCodigo, disciplinaCodigo, ehRegencia) => {
    return api.get(
      `v1/fechamentos/finais?DisciplinaCodigo=${disciplinaCodigo}&TurmaCodigo=${turmaCodigo}&ehRegencia=${ehRegencia}`
    );
  };
}

export default new ServicoFechamentoFinal();
