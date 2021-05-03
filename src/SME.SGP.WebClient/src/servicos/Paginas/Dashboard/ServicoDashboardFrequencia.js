const mockQuantidadeJustificativasMotivo = [
  {
    quantidade: 1500,
    descricao: 'Atestado Médico do Aluno',
  },
  {
    quantidade: 1500,
    descricao: 'Atestado Médico de pessoa da Família',
  },
  {
    quantidade: 2000,
    descricao: 'Doença na Família, sem atestado',
  },
  {
    quantidade: 1901,
    descricao: 'Óbito de pessoa da Família',
  },
  {
    quantidade: 2000,
    descricao: 'Inexistência de pessoa para levar à escola',
  },
  {
    quantidade: 1702,
    descricao: 'Enchente',
  },
  {
    quantidade: 1501,
    descricao: 'Falta de transporte',
  },
];

const mockoFrequenciaGlobalPorAno = [
  {
    quantidade: 1000,
    descricao: 'Qtd. acima do minímo de frequência',
    turma: 'EI - 5',
  },
  {
    quantidade: 1501,
    descricao: 'Qtd. abaixo do mínimo de frequência',
    turma: 'EI - 5',
  },
  {
    quantidade: 500,
    descricao: 'Qtd. acima do minímo de frequência',
    turma: 'EI - 6',
  },
  {
    quantidade: 20000,
    descricao: 'Qtd. abaixo do mínimo de frequência',
    turma: 'EI - 6',
  },
];

const mockFrequenciaGlobalPorDRE = [
  {
    quantidade: 1000,
    descricao: 'Qtd. acima do minímo de frequência',
    dre: 'BT',
  },
  {
    quantidade: 1501,
    descricao: 'Qtd. abaixo do mínimo de frequência',
    dre: 'BT',
  },
  {
    quantidade: 5003,
    descricao: 'Qtd. acima do minímo de frequência',
    dre: 'CL',
  },
  {
    quantidade: 2000,
    descricao: 'Qtd. abaixo do mínimo de frequência',
    dre: 'CL',
  },
  {
    quantidade: 4003,
    descricao: 'Qtd. acima do minímo de frequência',
    dre: 'CS',
  },
  {
    quantidade: 1000,
    descricao: 'Qtd. abaixo do mínimo de frequência',
    dre: 'CS',
  },
  {
    quantidade: 500,
    descricao: 'Qtd. acima do minímo de frequência',
    dre: 'FB',
  },
  {
    quantidade: 900,
    descricao: 'Qtd. abaixo do mínimo de frequência',
    dre: 'FB',
  },
  {
    quantidade: 3500,
    descricao: 'Qtd. acima do minímo de frequência',
    dre: 'GA',
  },
  {
    quantidade: 4900,
    descricao: 'Qtd. abaixo do mínimo de frequência',
    dre: 'GA',
  },
];

const mockQuantidadeAusenciasPossuemJustificativa = [
  {
    quantidade: 1500,
    descricao: 'EI - 5',
  },
  {
    quantidade: 1500,
    descricao: 'EI - 6',
  },
];

class ServicoDashboardFrequencia {
  obterFrequenciaGlobalPorAno = () => {
    return new Promise(resolve => {
      const retorno = { data: mockoFrequenciaGlobalPorAno };
      setTimeout(() => {
        resolve(retorno);
      }, 2000);
    });
  };

  obterFrequenciaGlobalPorDRE = () => {
    return new Promise(resolve => {
      const retorno = { data: mockFrequenciaGlobalPorDRE };
      setTimeout(() => {
        resolve(retorno);
      }, 2000);
    });
  };

  obterQuantidadeAusenciasPossuemJustificativa = () => {
    return new Promise(resolve => {
      const retorno = { data: mockQuantidadeAusenciasPossuemJustificativa };
      setTimeout(() => {
        resolve(retorno);
      }, 2000);
    });
  };

  obterQuantidadeJustificativasMotivo = () => {
    return new Promise(resolve => {
      const retorno = { data: mockQuantidadeJustificativasMotivo };
      setTimeout(() => {
        resolve(retorno);
      }, 2000);
    });
  };
}

export default new ServicoDashboardFrequencia();
