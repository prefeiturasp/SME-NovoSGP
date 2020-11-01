// import api from '~/servicos/api';

class ServicoDashboardEscolaAqui {
  obterDadosGraficoAdesao = () => {
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

    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ data: dadosMock });
      }, 3000);
    });
  };
}

export default new ServicoDashboardEscolaAqui();
