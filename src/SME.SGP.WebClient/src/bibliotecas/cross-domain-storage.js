import { obterUrlSondagem } from '../servicos/variaveis';

const createHost = require('cross-domain-storage/host');

export default createHost([
  {
    origin: obterUrlSondagem.obterUrlSondagem,
    allowedMethods: ['get'],
  },
]);
