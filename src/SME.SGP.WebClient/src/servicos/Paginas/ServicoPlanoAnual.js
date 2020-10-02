import api from '~/servicos/api';
import { store } from '~/redux';
import { erros } from '../alertas';
import {
  setDadosBimestresPlanoAnual,
  setExibirLoaderPlanoAnual,
  setListaObjetivosAprendizagemPorComponente,
} from '~/redux/modulos/anual/actions';

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

  obterBimestresPlanoAnual = turmaId => {
    return api.get(`v1/periodo-escolar/turmas/${turmaId}`);
  };

  carregarDadosPlanoAnualPorComponenteCurricular = async (
    turmaId,
    componenteCurricularId,
    periodoEscolarId,
    bimestre
  ) => {
    const obterDados = () => {
      const url = `v1/planejamento/anual/turmas/${turmaId}/componentes-curriculares/${componenteCurricularId}/periodos-escolares/${periodoEscolarId}`;
      return api.get(url);
    };

    const { dispatch } = store;
    const state = store.getState();
    const { planoAnual } = state;

    const { dadosBimestresPlanoAnual } = planoAnual;

    const dadosBimestre = { ...dadosBimestresPlanoAnual[bimestre] };

    let temDadosEmEdicao = false;

    if (
      dadosBimestre &&
      dadosBimestre.componentes &&
      dadosBimestre.componentes.length
    ) {
      const componente = dadosBimestre.componentes.find(
        item =>
          String(item.componenteCurricularId) === String(componenteCurricularId)
      );
      if (componente && componente.emEdicao) {
        temDadosEmEdicao = true;
      }
    }
    if (!temDadosEmEdicao) {
      const resultado = await obterDados()
        .catch(e => erros(e))
        .finally(() => {});
      if (resultado && resultado.status === 200) {
        const paramsIniciaisComponente = {
          bimestre,
          componentes: [
            {
              auditoria: null,
              componenteCurricularId,
              descricao: '',
              objetivosAprendizagemId: [],
            },
          ],
          periodoEscolarId,
        };

        if (
          dadosBimestre &&
          dadosBimestre.componentes &&
          dadosBimestre.componentes.length
        ) {
          const componenteCurricularJaPesquisado = dadosBimestre.componentes.find(
            item =>
              String(item.componenteCurricularId) ===
              String(componenteCurricularId)
          );

          if (componenteCurricularJaPesquisado) {
            const index = dadosBimestre.componentes.indexOf(
              componenteCurricularJaPesquisado
            );
            if (resultado.data.periodoEscolarId) {
              dadosBimestre.componentes[index] = resultado.data.componentes[0];
            } else {
              dadosBimestre.componentes[index] =
                paramsIniciaisComponente.componentes[0];
            }
            dispatch(setDadosBimestresPlanoAnual(dadosBimestre));
          } else if (resultado.data.periodoEscolarId) {
            dadosBimestre.componentes.push(resultado.data.componentes[0]);
            dispatch(setDadosBimestresPlanoAnual(dadosBimestre));
          } else {
            dadosBimestre.componentes.push(
              paramsIniciaisComponente.componentes[0]
            );
            dispatch(setDadosBimestresPlanoAnual(dadosBimestre));
          }
        } else if (resultado.data.periodoEscolarId) {
          dispatch(setDadosBimestresPlanoAnual(resultado.data));
        } else {
          dispatch(setDadosBimestresPlanoAnual(paramsIniciaisComponente));
        }
      }
    }
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
    dispatch(setExibirLoaderPlanoAnual(true));
    const objetivos = await this.obterObjetivosPorAnoEComponenteCurricular(
      ano,
      ensinoEspecial,
      [codigoComponenteCurricular]
    )
      .catch(e => erros(e))
      .finally(() => {
        dispatch(setExibirLoaderPlanoAnual(false));
      });

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

  salvarEditarPlanoAnual = (turmaId, componenteCurricularId, params) => {
    const url = `v1/planejamento/anual/turmas/${turmaId}/componentes-curriculares/${componenteCurricularId}`;
    return api.post(url, params);
  };
}

export default new ServicoPlanoAnual();
