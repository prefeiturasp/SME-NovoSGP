import api from '~/servicos/api';

class ServicoFechamentoFinal {
  obter = (turmaCodigo, disciplinaCodigo, ehRegencia, semestre) => {
    return api.get(
      `v1/fechamentos/finais?DisciplinaCodigo=${disciplinaCodigo}&TurmaCodigo=${turmaCodigo}&ehRegencia=${ehRegencia}&semestre=${semestre ||
        0}`
    );
  };

  salvar = fechamentoFinal => {
    return api.post('v1/fechamentos/finais', fechamentoFinal);
  };
}

export default new ServicoFechamentoFinal();
