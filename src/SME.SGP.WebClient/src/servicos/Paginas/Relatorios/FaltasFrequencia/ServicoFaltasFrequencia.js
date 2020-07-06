// import api from '~/servicos/api';

class ServicoFaltasFrequencia {
  gerar = dados => {
    console.log(dados);
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ data: { sucesso: true }, status: 200 });
      }, 3000);
    });
  };
}

export default new ServicoFaltasFrequencia();
