class ServicoDashboardAEE {
  obterQuantidadeEncaminhamentosPorSituacao = (
    anoLetivo,
    codigoDre,
    codigoUe
  ) => {
    console.log('obterQuantidadeEncaminhamentosPorSituacao');
    console.log(`anoLetivo: ${anoLetivo}`);
    console.log(`codigoDre: ${codigoDre}`);
    console.log(`codigoUe: ${codigoUe}`);
    const mock = [
      {
        descricaoSituacao: 'Aguardando validação do CP',
        quantidade: 264,
      },
      {
        descricaoSituacao: 'Aguardando análise PAEE/PAAI',
        quantidade: 332,
      },
      {
        descricaoSituacao: 'Indeferido',
        quantidade: 363,
      },
      {
        descricaoSituacao: 'Deferido',
        quantidade: 348,
      },
    ];

    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ data: mock, status: 200 });
      }, 2000);
    });
  };
}

export default new ServicoDashboardAEE();
