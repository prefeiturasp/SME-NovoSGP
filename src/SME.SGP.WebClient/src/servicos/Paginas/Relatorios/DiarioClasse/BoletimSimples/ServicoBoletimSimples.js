import api from '../../../../api';

class ServicoBoletimSimples {
  imprimirBoletim = async dados => {
    let retorno = {};

    const metodo = 'post';
    const url = 'v1/boletim/imprimir';

    try {
      const requisicao = await api[metodo](url, dados);
      if (requisicao.data) retorno = requisicao;
    } catch (erro) {
      retorno = {
        erro: true,
      };
    }

    return retorno;
  };
}

export default new ServicoBoletimSimples();
