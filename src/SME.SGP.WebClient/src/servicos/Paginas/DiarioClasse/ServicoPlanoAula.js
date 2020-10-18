import { store } from '~/redux';
import {
  setDadosOriginaisPlanoAula,
  setDadosPlanoAula,
  setExibirCardCollapsePlanoAula,
  setExibirLoaderFrequenciaPlanoAula,
  setListaComponentesCurricularesPlanejamento,
  setListaObjetivosComponenteCurricular,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';
import ServicoComponentesCurriculares from '~/servicos/Paginas/ComponentesCurriculares/ServicoComponentesCurriculares';

class ServicoPlanoAula {
  gerarRelatorioPlanoAula = planoAulaId => {
    const url = `v1/relatorios/plano-aula`;
    return api.post(url, { planoAulaId });
  };

  obterPlanoAula = async () => {
    const { dispatch } = store;

    const state = store.getState();
    const { frequenciaPlanoAula, usuario } = state;
    const { turmaSelecionada } = usuario;
    const {
      aulaId,
      dadosPlanoAula,
      componenteCurricular,
    } = frequenciaPlanoAula;

    // Seta o componente curricular selecionado no SelectComponent quando não é REGENCIA!
    const montarListaComponenteCurricularesPlanejamento = () => {
      dispatch(
        setListaComponentesCurricularesPlanejamento([componenteCurricular])
      );
    };

    // Carrega lista de componentes para montar as TABS!
    const obterListaComponentesCurricularesPlanejamento = () => {
      dispatch(setExibirLoaderFrequenciaPlanoAula(true));
      ServicoComponentesCurriculares.obterComponetensCuricularesRegencia(
        turmaSelecionada.id
      )
        .then(resposta => {
          dispatch(setListaComponentesCurricularesPlanejamento(resposta.data));
        })
        .catch(e => {
          dispatch(setListaComponentesCurricularesPlanejamento([]));
          erros(e);
        })
        .finally(() => {
          dispatch(setExibirLoaderFrequenciaPlanoAula(false));
        });
    };

    if (!dadosPlanoAula) {
      dispatch(setExibirLoaderFrequenciaPlanoAula(true));

      const plano = await api
        .get(`v1/planos/aulas/${aulaId}?turmaId=${turmaSelecionada.id}`)
        .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
        .catch(e => erros(e));

      if (plano && plano.data) {
        dispatch(setDadosPlanoAula({ ...plano.data }));
        dispatch(setDadosOriginaisPlanoAula({ ...plano.data }));

        const ehMigrado = plano.data.migrado;

        // Quando for MIGRADO mostrar somente um tab com o componente curricular já selecionado!
        if (ehMigrado) {
          montarListaComponenteCurricularesPlanejamento();
        } else if (componenteCurricular.regencia) {
          // Quando for REGENCIA carregar a lista de componentes curriculares!
          obterListaComponentesCurricularesPlanejamento();
        } else {
          montarListaComponenteCurricularesPlanejamento();
        }
      } else {
        dispatch(setDadosPlanoAula());
        dispatch(setExibirCardCollapsePlanoAula({ exibir: false }));
      }
    }
  };

  salvarPlanoAula = dadosPlanoAula => {
    return api.post('v1/planos/aulas', dadosPlanoAula);
  };

  atualizarDadosPlanoAula = (nomeCampo, valorNovo) => {
    const { dispatch } = store;
    const state = store.getState();

    const { frequenciaPlanoAula } = state;
    const { dadosPlanoAula } = frequenciaPlanoAula;

    dadosPlanoAula[nomeCampo] = valorNovo;
    dispatch(setDadosPlanoAula(dadosPlanoAula));
  };

  obterListaObjetivosPorAnoEComponenteCurricular = async () => {
    const { dispatch } = store;

    const state = store.getState();

    const { frequenciaPlanoAula, usuario } = state;
    const { turmaSelecionada } = usuario;

    const {
      dataSelecionada,
      componenteCurricular,
      listaObjetivosComponenteCurricular,
    } = frequenciaPlanoAula;

    const { codigoComponenteCurricular } = componenteCurricular;

    // Caso já tenha sido realizado a consulta vai retornar os dados que estão no redux!
    if (
      listaObjetivosComponenteCurricular &&
      listaObjetivosComponenteCurricular.length
    ) {
      return listaObjetivosComponenteCurricular;
    }

    const objetivos = await api.get(
      `v1/objetivos-aprendizagem/objetivos/turmas/${
        turmaSelecionada.id
      }/componentes/${codigoComponenteCurricular}/disciplinas/0?dataReferencia=${dataSelecionada.format(
        'YYYY-MM-DD'
      )}&regencia=${false}`
    );

    if (objetivos && objetivos.data && objetivos.data.length) {
      dispatch(setListaObjetivosComponenteCurricular(objetivos.data));
      return objetivos.data;
    }
    dispatch(setListaObjetivosComponenteCurricular([]));
    return [];
  };

  atualizarDadosAposCancelarEdicao = () => {
    const { dispatch } = store;

    const state = store.getState();

    const { frequenciaPlanoAula } = state;

    const { dadosOriginaisPlanoAula } = frequenciaPlanoAula;

    dispatch(setDadosOriginaisPlanoAula({ ...dadosOriginaisPlanoAula }));
    dispatch(setDadosPlanoAula({ ...dadosOriginaisPlanoAula }));
  };

  temObjetivosSelecionadosTabComponenteCurricular = codigoComponenteCurricular => {
    const state = store.getState();
    const { frequenciaPlanoAula } = state;
    const { dadosPlanoAula } = frequenciaPlanoAula;

    if (
      dadosPlanoAula &&
      dadosPlanoAula.objetivosAprendizagemComponente &&
      dadosPlanoAula.objetivosAprendizagemComponente.length
    ) {
      const tabAtual = dadosPlanoAula.objetivosAprendizagemComponente.find(
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
}

export default new ServicoPlanoAula();
