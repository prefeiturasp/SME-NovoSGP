const createHost = require('cross-domain-storage/host');

export default createHost([
  {
    origin: 'http://localhost:61540',
    allowedMethods: ['get'],
  },
]);
