import { store } from '~/redux';
import {
  setListaDadosFrequencia,
  setExibirLoaderFrequenciaPlanoAula,
  setTemPeriodoAbertoFrequenciaPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';

const urlPadrao = '/v1/calendarios';

class ServicoFrequencia {
  obterDisciplinas = turmaId => {
    const url = `v1/calendarios/frequencias/turmas/${turmaId}/disciplinas`;
    return api.get(url);
  };

  obterDatasDeAulasPorCalendarioTurmaEComponenteCurricular = (
    turmaId,
    componenteCurricular
  ) => {
    const url = `${urlPadrao}/frequencias/aulas/datas/turmas/${turmaId}/componente/${componenteCurricular}`;
    return api.get(url);
  };

  obterListaFrequencia = async () => {
    const { dispatch } = store;
    const state = store.getState();

    const { frequenciaPlanoAula } = state;

    const { aulaId, componenteCurricular } = frequenciaPlanoAula;

    if (aulaId) {
      dispatch(setExibirLoaderFrequenciaPlanoAula(true));
      const frequenciaAlunos = await api
        .get(`v1/calendarios/frequencias`, {
          params: {
            aulaId,
            componenteCurricularId:
              componenteCurricular.id > 0
                ? componenteCurricular.id
                : componenteCurricular.codigoComponenteCurricular,
          },
        })
        .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
        .catch(e => erros(e));

      if (frequenciaAlunos && frequenciaAlunos.data) {
        dispatch(setListaDadosFrequencia(frequenciaAlunos.data));
        dispatch(
          setTemPeriodoAbertoFrequenciaPlanoAula(
            frequenciaAlunos.data.temPeriodoAberto
          )
        );
      } else {
        dispatch(setListaDadosFrequencia({}));
        dispatch(setTemPeriodoAbertoFrequenciaPlanoAula(true));
      }
    }
  };

  salvarFrequencia = params => {
    return api.post(`v1/calendarios/frequencias`, params);
  };
}

export default new ServicoFrequencia();
