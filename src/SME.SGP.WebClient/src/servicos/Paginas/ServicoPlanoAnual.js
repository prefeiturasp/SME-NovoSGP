import api from '~/servicos/api';
import { store } from '~/redux';
import { erros } from '../alertas';
import { setListaObjetivosAprendizagemPorComponente } from '~/redux/modulos/anual/actions';

class ServicoPlanoAnual {
  obter = (anoLetivo, componenteCurricularEolId, ueId, turmaId) => {
    const url = `v1/planos/anual?turmaId=${turmaId}&ueId=${ueId}&anoLetivo=${anoLetivo}&componenteCurricularEolId=${componenteCurricularEolId}`;
    return api.get(url);
  };

  obterObjetivosPorAnoEComponenteCurricular = (
    anoLetivo,
    ensinoEspecial,
    disciplinas
  ) => {
    return api.post('v1/objetivos-aprendizagem', {
      ano: anoLetivo,
      ensinoEspecial,
      ComponentesCurricularesIds: disciplinas,
    });
  };

  salvar = planoAnual => {
    return api.post('v1/planos/anual', planoAnual);
  };

  obterTurmasParaCopia = (turmaId, componenteCurricularEolId) => {
    return api.get(
      `v1/planos/anual/turmas/copia?turmaId=${turmaId}&componenteCurricular=${componenteCurricularEolId}`
    );
  };

  copiarConteudo = migrarConteudoPlanoAnual => {
    return api.post(`v1/planos/anual/migrar`, migrarConteudoPlanoAnual);
  };

  obterBimestresPlanoAnual = (modalidade, anoLetivo) => {
    const url = `v1/periodo-escolar/modalidades/${modalidade}/anos-letivos/${anoLetivo}`;
    return api.get(url);
  };

  obterDadosPlanoAnualPorComponenteCurricular = (
    turmaId,
    componenteCurricularId,
    periodoEscolarId
  ) => {
    const url = `v1/planejamento/anual/turmas/${turmaId}/componentes-curriculares/${componenteCurricularId}/periodos-escolares/${periodoEscolarId}`;
    return api.get(url);
  };

  obterListaObjetivosPorAnoEComponenteCurricular = async (
    ano,
    ensinoEspecial,
    codigoComponenteCurricular
  ) => {
    const { dispatch } = store;
    const state = store.getState();
    const { planoAnual } = state;

    const { listaObjetivosAprendizagemPorComponente } = planoAnual;

    const listaObjetivos =
      listaObjetivosAprendizagemPorComponente[codigoComponenteCurricular];

    if (listaObjetivos && listaObjetivos.length) {
      return listaObjetivos;
    }

    const objetivos = await this.obterObjetivosPorAnoEComponenteCurricular(
      ano,
      ensinoEspecial,
      [codigoComponenteCurricular]
    ).catch(e => erros(e));

    if (objetivos && objetivos.data && objetivos.data.length) {
      dispatch(
        setListaObjetivosAprendizagemPorComponente({
          objetivos: objetivos.data,
          codigoComponenteCurricular,
        })
      );
      return objetivos.data;
    }
    return [];
  };
}

export default new ServicoPlanoAnual();
