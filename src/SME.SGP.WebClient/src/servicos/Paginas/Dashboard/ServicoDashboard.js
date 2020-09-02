// import api from '~/servicos/api';
import { store } from '~/redux';
import {
  setDadosCardsDashboard,
  setCarregandoDadosCardsDashboard,
} from '~/redux/modulos/dashboard/actions';

class ServicoDashboard {
  obterDadosDashboard = async rf => {
    const { dispatch } = store;

    const obterDadosMock = () => {
      const dados = [
        {
          descricao: 'Carta Intenções',
          rota: '/planejamento/carta-intencoes',
          turmaObrigatoria: true,
          usuarioTemPermissao: true,
          icone: 'fa-check-square',
          pack: 'far',
        },
        {
          descricao: 'Calendário do professor',
          rota: '/calendario-escolar/calendario-professor',
          turmaObrigatoria: true,
          usuarioTemPermissao: true,
          icone: 'fa-calendar-alt',
          pack: 'fas',
        },
      ];

      return new Promise(resolve => {
        setTimeout(() => {
          resolve({ data: dados });
        }, 2000);
      });
    };

    dispatch(setDadosCardsDashboard([]));
    dispatch(setCarregandoDadosCardsDashboard(true));

    const retorno = await obterDadosMock(rf);

    if (retorno && retorno.data && retorno.data.length) {
      dispatch(setDadosCardsDashboard(retorno.data));
    }
    dispatch(setCarregandoDadosCardsDashboard(false));
  };
}

export default new ServicoDashboard();
