import axios from 'axios';
import { urlBase } from './variaveis';
import { store } from '~/redux';
import { deslogarPorSessaoInvalida } from '~/servicos/ServicoUsuarioDeslogar';

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
    if (error.response.status === 401) {
      deslogarPorSessaoInvalida();
    }
    return Promise.reject(error);
  }
);

api.CancelarRequisicoes = mensagem => {
  CancelToken.cancel(mensagem);

  renovaCancelToken();
};

export default api;
