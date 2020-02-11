import api from '~/servicos/api';

class ServicoFechamentoFinal {
  obter = (turmaCodigo, disciplinaCodigo, ehRegencia) => {
    return api.get(
      `v1/fechamentos/finais?DisciplinaCodigo=${disciplinaCodigo}&TurmaCodigo=${turmaCodigo}&ehRegencia=${ehRegencia}`
    );
  };

  salvar = fechamentoFinal => {
    return api.post('v1/fechamentos/finais', fechamentoFinal);
  };
}

export default new ServicoFechamentoFinal();
