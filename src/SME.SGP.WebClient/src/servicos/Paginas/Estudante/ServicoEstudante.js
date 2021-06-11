import api from '~/servicos/api';

const urlPadrao = 'v1/estudante';

class ServicoEstudante {
  obterDadosEstudante = (codigoAluno, anoLetivo) => {
    const url = `${urlPadrao}/${codigoAluno}/anosLetivos/${anoLetivo}`;
    return api.get(url);
  };

  obterInformacoesEscolaresDoAluno = (codigoAluno, codigoTurma) => {
    const url = `${urlPadrao}/informacoes-escolares?codigoAluno=${codigoAluno}&codigoTurma=${codigoTurma}`;
    return api.get(url);
  };
}

export default new ServicoEstudante();
