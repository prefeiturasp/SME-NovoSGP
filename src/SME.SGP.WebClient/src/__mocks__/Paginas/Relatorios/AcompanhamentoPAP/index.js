const Eixos = [
  {
    Id: 1,
    Descricao: 'Informações escolares',
  },
];

const Objetivos = [
  {
    Id: 1,
    EixoId: 1,
    Descricao: 'É atendido pelo AEE?',
  },
  {
    Id: 2,
    EixoId: 1,
    Descricao: 'É atendido pelo NAAPA?',
  },
  {
    Id: 3,
    EixoId: 1,
    Descricao: 'Parecer conclusivo do ano anterior',
  },
];

const Respostas = [
  {
    Id: 1,
    ObjetivoId: 1,
    Opcoes: [
      {
        Id: 1,
        valor: 'S',
        descricao: 'Sim',
      },
      {
        Id: 2,
        valor: 'N',
        descricao: 'Não',
      },
    ],
  },
  {
    Id: 2,
    ObjetivoId: 2,
    Opcoes: [
      {
        Id: 1,
        valor: 'S',
        descricao: 'Sim',
      },
      {
        Id: 2,
        valor: 'N',
        descricao: 'Não',
      },
    ],
  },
  {
    Id: 3,
    ObjetivoId: 3,
    Opcoes: [
      {
        Id: 1,
        valor: '0',
        descricao: 'Aprovado',
      },
      {
        Id: 2,
        valor: '1',
        descricao: 'Aprovado pelo conselho',
      },
      {
        Id: 3,
        valor: '2',
        descricao: 'Continuidade dos estudos',
      },
      {
        Id: 4,
        valor: '3',
        descricao: 'Retido',
      },
      {
        Id: 5,
        valor: '4',
        descricao: 'Retido por frequência',
      },
    ],
  },
];

const Alunos = [
  {
    Id: 1,
    NumeroChamada: 1,
    Nome: 'Jéssica Moura Silveira Santos',
    Concluido: Math.floor(Math.random() * (3 - 0) + 0),
    Turma: '5º C',
    Respostas: [
      {
        ObjetivoId: 1,
        RespostaId: 1,
        OpcaoId: 1,
      },
    ],
  },
  {
    Id: 1,
    NumeroChamada: 1,
    Nome: 'Ítalo Gustavo Pereira de Maio',
    Concluido: Math.floor(Math.random() * (3 - 0) + 0),
    Turma: '5º C',
    Respostas: [
      {
        ObjetivoId: 1,
        RespostaId: 1,
        OpcaoId: 2,
      },
      {
        ObjetivoId: 2,
        RespostaId: 2,
        OpcaoId: 1,
      },
      {
        ObjetivoId: 3,
        RespostaId: 3,
        OpcaoId: 4,
      },
    ],
  },
  {
    Id: 2,
    NumeroChamada: 2,
    Nome: 'Jhonny Conrado Dantas',
    Concluido: Math.floor(Math.random() * (3 - 0) + 0),
    Turma: '5º C',
    Respostas: [
      {
        ObjetivoId: 1,
        RespostaId: 1,
        OpcaoId: 1,
      },
    ],
  },
  {
    Id: 3,
    NumeroChamada: 3,
    Nome: 'Maurício Charles Teste',
    Concluido: Math.floor(Math.random() * (3 - 0) + 0),
    Turma: '5º C',
    Respostas: [
      {
        ObjetivoId: 1,
        RespostaId: 1,
        OpcaoId: 2,
      },
    ],
  },
  {
    Id: 4,
    NumeroChamada: 4,
    Nome: 'Roberto Carlos da Silva',
    Concluido: Math.floor(Math.random() * (3 - 0) + 0),
    Turma: '5º C',
    Respostas: [
      {
        ObjetivoId: 1,
        RespostaId: 1,
        OpcaoId: 1,
      },
    ],
  },
  {
    Id: 5,
    NumeroChamada: 5,
    Nome: 'Joana Teste de Oliveira',
    Concluido: Math.floor(Math.random() * (3 - 0) + 0),
    Turma: '5º C',
    Respostas: [
      {
        ObjetivoId: 1,
        RespostaId: 1,
        OpcaoId: 1,
      },
    ],
  },
  {
    Id: 6,
    NumeroChamada: 6,
    Nome: 'Larissa Rodrigues de Souza Silva',
    Concluido: Math.floor(Math.random() * (3 - 0) + 0),
    Turma: '5º C',
    Respostas: [
      {
        ObjetivoId: 1,
        RespostaId: 1,
        OpcaoId: 1,
      },
    ],
  },
  {
    Id: 7,
    NumeroChamada: 7,
    Nome: 'Carla Barbosa de Souza',
    Concluido: Math.floor(Math.random() * (3 - 0) + 0),
    Turma: '5º C',
    Respostas: [
      {
        ObjetivoId: 1,
        RespostaId: 1,
        OpcaoId: 1,
      },
    ],
  },
];

export { Eixos, Objetivos, Respostas, Alunos };
