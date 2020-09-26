import { URL_LOGIN } from '~/constantes/url';
import { store } from '~/redux';
import { limparDadosFiltro } from '~/redux/modulos/filtro/actions';
import { Deslogar } from '~/redux/modulos/usuario/actions';
import { LimparSessao } from '~/redux/modulos/sessao/actions';
import history from '~/servicos/history';

const deslogarPorSessaoInvalida = () => {
  store.dispatch(limparDadosFiltro());
  store.dispatch(Deslogar());
  store.dispatch(LimparSessao());
  history.push(URL_LOGIN);
};

export { deslogarPorSessaoInvalida };
