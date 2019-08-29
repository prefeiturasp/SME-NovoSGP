import axios from 'axios';
import urlBase from './variaveis';

let url = '';
urlBase().then(resposta => (url = resposta.data));

const api = axios.create({ baseURL: url });

api.interceptors.request.use(async config => {
  if (!url) {
    url = await urlBase();
  }
  config.baseURL = url;
  return config;
});

export default api;
