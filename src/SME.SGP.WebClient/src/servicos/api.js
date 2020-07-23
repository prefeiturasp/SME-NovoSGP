import axios from 'axios';
import { urlBase } from './variaveis';
import { store } from '~/redux';

let url = '';

let CancelToken = axios.CancelToken.source();

urlBase().then(resposta => {
  url = resposta.data;
});

const api = axios.create({
  baseURL: url,
});

const renovaCancelToken = () => {
  CancelToken = axios.CancelToken.source();
};

api.interceptors.request.use(async config => {
  const { token } = store.getState().usuario;

  if (!url) url = await urlBase();
  if (token) config.headers.Authorization = `Bearer ${token}`;

  config.cancelToken = CancelToken.token;

  config.baseURL = url;

  return config;
});

api.interceptors.response.use(
  response => response,
  error => {
    if (axios.isCancel(error)) return Promise.reject(error);

    const autenticacao =
      error.response &&
      error.response.config.url.includes('/api/v1/autenticacao');

    if (error.response && error.response.status === 401 && !autenticacao)
      return Promise.reject(error);

    return autenticacao;
  }
);

api.CancelarRequisicoes = mensagem => {
  CancelToken.cancel(mensagem);

  renovaCancelToken();
};

export default api;
