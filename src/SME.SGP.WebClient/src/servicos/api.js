import axios from 'axios';

const api = axios.create({
  // TODO
  baseURL: 'http://hom-smeintegracaoapi.sme.prefeitura.sp.gov.br/api/v1',
});

export default api;
