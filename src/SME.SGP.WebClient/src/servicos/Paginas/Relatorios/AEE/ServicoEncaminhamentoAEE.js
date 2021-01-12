import { store } from '~/redux';
import {
  setDadosModalAviso,
  setExibirModalAviso,
} from '~/redux/modulos/encaminhamentoAEE/actions';
import api from '~/servicos/api';

const urlPadrao = 'v1/encaminhamento-aee';

class ServicoEncaminhamentoAEE {
  obterSituacoes = () => {
    return api.get(`${urlPadrao}/situacoes`);
  };

  obterAvisoModal = async () => {
    const { dispatch } = store;

    const state = store.getState();
    const { encaminhamentoAEE } = state;

    const { dadosModalAviso } = encaminhamentoAEE;

    if (!dadosModalAviso) {
      const retorno = await api.get(`${urlPadrao}/instrucoes-modal`);
      if (retorno?.data) {
        dispatch(setDadosModalAviso(retorno.data));
        dispatch(setExibirModalAviso(true));
      } else {
        dispatch(setDadosModalAviso());
        dispatch(setExibirModalAviso(false));
      }
    } else {
      dispatch(setExibirModalAviso(true));
    }
  };

  obterDadosEstudante = (codigoAluno, anoLetivo) => {
    const url = `v1/estudante/${codigoAluno}/anosLetivos/${anoLetivo}`;
    return api.get(url);
  };

  obterSecoesPorEtapaDeEncaminhamentoAEE = etapa => {
    const url = `${urlPadrao}/secoes?etapa=${etapa}`;
    return api.get(url);
  };
}

export default new ServicoEncaminhamentoAEE();
