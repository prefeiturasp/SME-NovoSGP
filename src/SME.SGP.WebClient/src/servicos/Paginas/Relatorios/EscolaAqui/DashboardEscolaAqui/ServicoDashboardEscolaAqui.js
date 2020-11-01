const urlPadrao = `v1/ea/dashboard`;

class ServicoDashboardEscolaAqui {
  obterDadosGraficoAdesao = (codigoDre, codigoUe) => {
    const dadosMock = [
      {
        id: '1',
        label: 'Responsáveis sem CPF ou com CPF inválido no EOL',
        value: 42,
        color: '#F98F84',
      },
      {
        id: '2',
        label: 'Usuários que não realizaram a instalação',
        value: 120,
        color: '#57CDBC',
      },
      {
        id: '3',
        label: 'Usuários com primeiro acesso incompleto',
        value: 48,
        color: '#EFB971',
      },
      {
        id: '4',
        label: 'Usuários válidos',
        value: 75,
        color: '#3982AC',
      },
    ];

    console.log(urlPadrao);
    console.log(codigoDre);
    console.log(codigoUe);

    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ data: dadosMock });
      }, 3000);
    });
    // TODO
    // const url = `${urlPadrao}/adesao?codigoDre=${codigoDre}&codigoUe=${codigoUe}`;
    // return api.get(url);
  };

  obterUltimaAtualizacaoPorProcesso = () => {
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ data: '2020-11-01T14:11:34' });
      }, 3000);
    });
    // TODO
    // const url = `${urlPadrao}/ultimoProcessamento`;
    // return api.get(url);
  };
}

export default new ServicoDashboardEscolaAqui();
