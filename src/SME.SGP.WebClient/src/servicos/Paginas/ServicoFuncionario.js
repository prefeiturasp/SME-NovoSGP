import api from '~/servicos/api';

const urlPadrao = 'v1/funcionarios';

class ServicoFuncionario {
  obterFuncionariosPAAIs = dreId => {
    const url = `${urlPadrao}/dres/${dreId}/paais`;
    return api.get(url);
  };
}

export default new ServicoFuncionario();
