import api from '~/servicos/api';

const urlPadrao = 'v1/estudante';

class ServicoEstudante {
  obterDadosEstudante = (codigoAluno, anoLetivo) => {
    const url = `${urlPadrao}/${codigoAluno}/anosLetivos/${anoLetivo}`;
    return api.get(url);
  };
}

export default new ServicoEstudante();
