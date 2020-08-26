import api from '~/servicos/api';

class ServicoConselhoAtaFinal {
  gerar = dados => {
    const url = 'v1/relatorios/conselhos-classe/atas-finais';

    return api.get(url, dados);
  };
}

export default new ServicoConselhoAtaFinal();
