import { store } from '~/redux';
import {
  setDadosParaSalvarPlanoAula,
  setDadosPlanoAula,
  setExibirCardCollapsePlanoAula,
  setExibirLoaderFrequenciaPlanoAula,
  setListaComponentesCurricularesPlanejamento,
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
        .get(`v1/planos/aulas/${aulaId}`)
        .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
        .catch(e => erros(e));

      if (plano && plano.data) {
        dispatch(setDadosPlanoAula({ ...plano.data }));
        dispatch(setDadosParaSalvarPlanoAula({ ...plano.data }));

        const ehMigrado = plano.data.migrado;
        // TODO Validar se é necessário usar essa prop no redux!
        // dispatch(setEhRegistroMigrado(!!ehMigrado));

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

  atualizarDadosParaSalvarPlanoAula = (nomeCampo, valorNovo) => {
    const { dispatch } = store;
    const state = store.getState();

    const { frequenciaPlanoAula } = state;
    const { dadosParaSalvarPlanoAula } = frequenciaPlanoAula;

    dadosParaSalvarPlanoAula[nomeCampo] = valorNovo;
    dispatch(setDadosParaSalvarPlanoAula(dadosParaSalvarPlanoAula));
  };

  obterListaObjetivosPorAnoEComponenteCurricular = async () => {
    const state = store.getState();

    const { frequenciaPlanoAula, usuario } = state;
    const { turmaSelecionada } = usuario;

    const {
      dataSelecionada,
      componenteCurricular,
      tabAtualComponenteCurricular,
    } = frequenciaPlanoAula;

    const codigoComponenteCurricularTabAtual =
      tabAtualComponenteCurricular.codigoComponenteCurricular;

    const { codigoComponenteCurricular, regencia } = componenteCurricular;

    const objetivos = await api.get(
      `v1/objetivos-aprendizagem/objetivos/turmas/${
        turmaSelecionada.turma
      }/componentes/${codigoComponenteCurricular}/disciplinas/${codigoComponenteCurricularTabAtual}?dataAula=${dataSelecionada.format(
        'YYYY-MM-DD'
      )}&regencia=${regencia}`
    );

    if (objetivos && objetivos.data && objetivos.data.length) {
      // TODO - Adicionar no redux para melhorar a performance!
      return objetivos.data;
    }
    return [];
  };

  atualizarDadosAposCancelarEdicao = () => {
    const { dispatch } = store;

    const state = store.getState();

    const { frequenciaPlanoAula } = state;

    const { dadosPlanoAula } = frequenciaPlanoAula;

    dispatch(setDadosParaSalvarPlanoAula({ ...dadosPlanoAula }));
    dispatch(setDadosPlanoAula({ ...dadosPlanoAula }));
  };
}

export default new ServicoPlanoAula();
