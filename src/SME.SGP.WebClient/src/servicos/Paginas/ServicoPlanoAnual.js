import api from '~/servicos/api';
import { store } from '~/redux';
import { erros } from '../alertas';
import {
  setDadosBimestresPlanoAnual,
  setExibirLoaderPlanoAnual,
  setListaObjetivosAprendizagemPorComponente,
  setPlanejamentoAnualId,
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

  obterTurmasParaCopia = (turmaId, componenteCurricularId, ensinoEspecial, consideraHistorico = false) => {
    return api.get(
      `v1/planejamento/anual/turmas/copia?turmaId=${turmaId}&componenteCurricular=${componenteCurricularId}&ensinoEspecial=${ensinoEspecial}&consideraHistorico=${consideraHistorico}`
    );
  };

  copiarConteudo = params => {
    return api.post(`v1/planejamento/anual/migrar`, params);
  };

  obterBimestresPlanoAnual = turmaId => {
    return api.get(`v1/periodo-escolar/turmas/${turmaId}`);
  };

  obterObjetivosPorAnoEComponenteCurricularNovo = (
    ano,
    componenteCurricularId,
    ensinoEspecial
  ) => {
    const url = `v1/objetivos-aprendizagem/${ano}/${componenteCurricularId}?ensinoEspecial=${ensinoEspecial}`;
    return api.get(url);
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
    const objetivos = await this.obterObjetivosPorAnoEComponenteCurricularNovo(
      ano,
      codigoComponenteCurricular,
      ensinoEspecial
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

  obterPlanejamentoId = (turmaId, codigoComponenteCurricular) => {
    const { dispatch } = store;

    const url = `v1/planejamento/anual/turmas/${turmaId}/componentes-curriculares/${codigoComponenteCurricular}`;
    api
      .get(url)
      .then(resposta => {
        dispatch(setPlanejamentoAnualId(resposta.data));
      })
      .catch(e => {
        dispatch(setPlanejamentoAnualId(0));
        erros(e);
      });
  };

  obterPeriodosEscolaresParaCopia = planejamentoAnualId => {
    const url = `v1/planejamento/anual/${planejamentoAnualId}/periodos-escolares/copia`;
    return api.get(url);
  };

  temObjetivosSelecionadosTabComponenteCurricular = codigoComponenteCurricular => {
    const { planoAnual } = store.getState();

    if (planoAnual?.objetivosAprendizagemComponente?.length) {
      const tabAtual = planoAnual.objetivosAprendizagemComponente.find(
        item =>
          String(item.componenteCurricularId) ===
          String(codigoComponenteCurricular)
      );

      if (
        tabAtual &&
        tabAtual.objetivosAprendizagem &&
        tabAtual.objetivosAprendizagem.length
      ) {
        return true;
      }
    }
    return false;
  };

  verificarDadosPlanoPorComponenteCurricular = async(
    turmaId,
    componenteCurricularId,
    periodoEscolarId    
  )=>{
    const obterDados = () => {
      const url = `v1/planejamento/anual/turmas/${turmaId}/componentes-curriculares/${componenteCurricularId}/periodos-escolares/${periodoEscolarId}`;
      return api.get(url);
    };

    const resultado = await obterDados()
        .catch(e => erros(e))
        .finally(() => {});
    
    return resultado?.data;    
  };
}

export default new ServicoPlanoAnual();
