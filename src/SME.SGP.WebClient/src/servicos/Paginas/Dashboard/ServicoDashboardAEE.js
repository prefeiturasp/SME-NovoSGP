class ServicoDashboardAEE {
  obterQuantidadeEncaminhamentosPorSituacao = () => {
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ data: { valor: 123 }, status: 200 });
      }, 2000);
    });
  };
}

export default new ServicoDashboardAEE();
