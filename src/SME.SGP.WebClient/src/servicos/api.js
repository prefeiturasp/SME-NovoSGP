import axios from 'axios';
import { urlBase } from './variaveis';
import { store } from '~/redux';
import history from '~/servicos/history';
import { URL_LOGIN } from '~/constantes/url';
import { Deslogar } from '~/redux/modulos/usuario/actions';
import { limparDadosFiltro } from '~/redux/modulos/filtro/actions';
import { LimparSessao } from '~/redux/modulos/sessao/actions';
import shortid from 'shortid';

let url = '';
let CancelToken = axios.CancelToken.source();
CancelToken.id = shortid.generate();

const renovaCancelToken = () => {
  CancelToken = axios.CancelToken.source();
  CancelToken.id = shortid.generate();
};

urlBase().then(resposta => (url = resposta.data));

const api = axios.create({
  baseURL: url,
});

const fila = [];

api.interceptors.request.use(async config => {
  const token = store.getState().usuario.token;

  if (!url) url = await urlBase();
  if (token) config.headers.Authorization = `Bearer ${token}`;

  config.cancelToken = CancelToken.token;

  config.baseURL = url;
  config.headers.requestId = CancelToken.id;
  fila.push(config);

  return config;
});

api.interceptors.response.use(
  response => {
    let requestId = response.config.headers.requestId;
    if (requestId) {
      let requestIndex = fila.findIndex(c => c.headers.requestId == requestId);
      if (requestIndex > -1) {
        fila.splice(requestIndex, 1);
      }
    }
    return response;
  },
  error => {
    if (axios.isCancel(error)) {
      const requests = fila.filter(
        c => c.url.indexOf('v1/autenticacao/revalidar') < 0
      );
      if (requests && requests.length > 0) {
        const request = requests.find(
          c => c.headers.requestId == error.message
        );
        if (request) return api.request(request);
      }
      return Promise.reject(error);
    }

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
  CancelToken.cancel(CancelToken.id);

  renovaCancelToken();
};

export default api;
