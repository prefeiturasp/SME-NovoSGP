import { obterUrlSondagem } from '~/servicos/variaveis';

const createHost = require('cross-domain-storage/host');

obterUrlSondagem().then(resposta => {
  debugger;
  return resposta;
});

export default createHost([
  {
    origin: obterUrlSondagem(),
    allowedMethods: ['get'],
  },
]);
