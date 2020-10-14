import { store } from '~/redux';
import {
  setDadosParaSalvarPlanoAula,
  setDadosPlanoAula,
  setExibirLoaderFrequenciaPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';

class ServicoPlanoAula {
  gerarRelatorioPlanoAula = planoAulaId => {
    const url = `v1/relatorios/plano-aula`;
    return api.post(url, { planoAulaId });
  };

  obterPlanoAula = async () => {
    const { dispatch } = store;
    const state = store.getState();

    const { frequenciaPlanoAula } = state;
    const { aulaId, dadosPlanoAula } = frequenciaPlanoAula;

    if (!dadosPlanoAula) {
      dispatch(setExibirLoaderFrequenciaPlanoAula(true));

      const plano = await api
        .get(`v1/planos/aulas/${aulaId}`)
        .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
        .catch(e => erros(e));

      if (plano && plano.data) {
        dispatch(setDadosPlanoAula(plano.data));
        dispatch(setDadosParaSalvarPlanoAula(plano.data));
      } else {
        dispatch(setDadosPlanoAula({}));
      }
    }
  };

  salvarPlanoAula = dadosPlanoAula => {
    return api.post('v1/planos/aulas', dadosPlanoAula);
  };

  atualizarDadosParaSalvarPlanoAula = (nomeCampo, valorNovo) => {
    const { dispatch } = store;
    const state = store.getState();

    const { frequenciaPlanoAula } = state;
    const { dadosParaSalvarPlanoAula } = frequenciaPlanoAula;

    dadosParaSalvarPlanoAula[nomeCampo] = valorNovo;
    dispatch(setDadosParaSalvarPlanoAula(dadosParaSalvarPlanoAula));
  };
}

export default new ServicoPlanoAula();
