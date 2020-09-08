import api from '~/servicos/api';
import { store } from '~/redux';
import {
  setDadosCardsDashboard,
  setCarregandoDadosCardsDashboard,
} from '~/redux/modulos/dashboard/actions';

class ServicoDashboard {
  obterDadosDashboard = async () => {
    const { dispatch } = store;

    dispatch(setDadosCardsDashboard([]));
    dispatch(setCarregandoDadosCardsDashboard(true));

    const retorno = await api.get('v1/dashboard');

    if (retorno && retorno.data && retorno.data.length) {
      dispatch(setDadosCardsDashboard(retorno.data));
    }
    dispatch(setCarregandoDadosCardsDashboard(false));
  };
}

export default new ServicoDashboard();
