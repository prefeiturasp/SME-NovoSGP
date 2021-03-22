import axios from 'axios';
import moment from 'moment';
import { urlBase } from './variaveis';
import { store } from '~/redux';
import { deslogarPorSessaoInvalida } from '~/servicos/ServicoUsuarioDeslogar';
import { DeslogarSessaoExpirou } from '~/redux/modulos/usuario/actions';
import { TOKEN_EXPIRADO } from '~/constantes';

let url = '';

let CancelToken = axios.CancelToken.source();

urlBase().then(resposta => {
  url = resposta?.data;
});

const api = axios.create({
  baseURL: url,
});

const renovaCancelToken = () => {
  CancelToken = axios.CancelToken.source();
};

api.interceptors.request.use(
  async config => {
    const { token, dataHoraExpiracao } = store.getState().usuario;
    const diff = moment().diff(dataHoraExpiracao, 'seconds');

    if (!url) url = await urlBase();
    if (token) config.headers.Authorization = `Bearer ${token}`;

    config.cancelToken = CancelToken.token;

    config.baseURL = url;

    if (diff >= 0 && dataHoraExpiracao) {
      deslogarPorSessaoInvalida();
      store.dispatch(DeslogarSessaoExpirou());
      CancelToken.cancel(TOKEN_EXPIRADO);
      renovaCancelToken();
    }

    return config;
  },
  error => Promise.reject(error)
);

api.interceptors.response.use(
  response => response,
  error => {
    if (axios.isCancel(error)) return Promise.reject(error);
    if (error.response?.status === 401) {
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
