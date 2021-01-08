// import api from '~/servicos/api';

// const urlPadrao = 'v1/relatorios/aee';

class ServicoEncaminhamentoAEE {
  obterSituacoes = () => {
    // TODO
    // return api.get(`${urlPadrao}/situacao`);

    return new Promise(resolve => {
      setTimeout(() => {
        resolve({
          data: [
            { valor: 1, desc: 'Sit 01' },
            { valor: 2, desc: 'Sit 02' },
          ],
        });
      }, 3000);
    });
  };
}

export default new ServicoEncaminhamentoAEE();
