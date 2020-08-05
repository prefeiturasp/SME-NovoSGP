import * as moment from 'moment';

class ServicoCartaIntencoes {
  obterBimestres = () => {
    const mockBimestres = [
      {
        auditoria: {
          alteradoEm: null,
          alteradoPor: null,
          alteradoRF: null,
          criadoEm: '0001-01-01T00:00:00',
          criadoPor: null,
          criadoRF: null,
        },
        bimestre: 1,
        descricao: null,
        id: 0,
      },
      {
        auditoria: {
          alteradoEm: moment(),
          alteradoPor: 'TESTE MOCK',
          alteradoRF: 9999999,
          criadoEm: moment(),
          criadoPor: 'TESTE MOCK',
          criadoRF: 9999999,
        },
        bimestre: 2,
        descricao: 'teste mock teste mock',
        id: 123,
      },
    ];
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({
          data: mockBimestres,
        });
      }, 2000);
    });
  };

  salvarCartaIntencoes = dados => {
    console.log(dados);
    console.log('Salvar');
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({
          status: 200,
        });
      }, 2000);
    });
  };
}

export default new ServicoCartaIntencoes();
