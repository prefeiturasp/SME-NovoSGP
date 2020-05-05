import axios from 'axios';
import { urlBase } from './variaveis';
import { store } from '~/redux';
import history from '~/servicos/history';
import { URL_LOGIN } from '~/constantes/url';
import { Deslogar } from '~/redux/modulos/usuario/actions';
import { limparDadosFiltro } from '~/redux/modulos/filtro/actions';
import { LimparSessao } from '~/redux/modulos/sessao/actions';

let url = '';

let CancelToken = axios.CancelToken.source();

urlBase().then(resposta => (url = resposta.data));

const api = axios.create({
  baseURL: url,
});

const renovaCancelToken = () => {
  CancelToken = axios.CancelToken.source();
};

api.interceptors.request.use(async config => {
  const token = store.getState().usuario.token;

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

    if (error.response && error.response.status === 401 && !autenticacao) {
      const path = window.location.pathname;
      store.dispatch(limparDadosFiltro());
      store.dispatch(Deslogar());
      store.dispatch(LimparSessao());
      history.push(`${URL_LOGIN}/${btoa(path)}`);

      return Promise.reject(error);
    }

    return Promise.reject(error);
  }
);

api.CancelarRequisicoes = mensagem => {
  CancelToken.cancel(mensagem);

  renovaCancelToken();
};

export default api;
