import * as moment from 'moment';

const dados = [
  {
    id: 1,
    texto:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore',
    proprietario: true,
    codigoRf: '999999',
    auditoria: {
      criadoEm: moment(),
      criadoPor: 'keanu reeves',
      criadoRF: '999999',
      alteradoEm: moment(),
      alteradoPor: 'keanu reeves',
      alteradoRF: '999999',
    },
  },
  {
    id: 2,
    texto:
      'texto do comentário 2 texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2texto do comentário 2',
    proprietario: false,
    codigoRf: '7944560',
    auditoria: {
      criadoEm: moment(),
      criadoPor: 'HELOISA MARIA DE MORAIS GIANNICHI',
      criadoRF: '7944560',
      alteradoEm: moment(),
      alteradoPor: 'HELOISA MARIA DE MORAIS GIANNICHI',
      alteradoRF: '7944560',
    },
  },
  {
    id: 3,
    texto:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore',
    proprietario: true,
    codigoRf: '3135535',
    auditoria: {
      criadoEm: moment(),
      criadoPor: 'SADAKO MORITA',
      criadoRF: '3135535',
      alteradoEm: moment(),
      alteradoPor: 'SADAKO MORITA',
      alteradoRF: '3135535',
    },
  },
  {
    id: 4,
    texto:
      'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore reprehenderit in voluptate velit esse cillum dolore',
    proprietario: false,
    codigoRf: '1111222',
    auditoria: {
      criadoEm: moment(),
      criadoPor: 'MARIA',
      criadoRF: '1111222',
      alteradoEm: moment(),
      alteradoPor: 'MARIA',
      alteradoRF: '1111222',
    },
  },
  {
    id: 5,
    texto:
      'teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste',
    proprietario: false,
    codigoRf: '43434343',
    auditoria: {
      criadoEm: '19/07/2020 18:55',
      criadoPor: 'NOG',
      criadoRF: '43434343',
      alteradoEm: moment(),
      alteradoPor: 'NOG',
      alteradoRF: '43434343',
    },
  },
];

export { dados };
