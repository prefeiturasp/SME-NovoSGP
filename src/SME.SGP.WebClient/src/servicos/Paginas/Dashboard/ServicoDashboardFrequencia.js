import api from '~/servicos/api';
import { store } from '~/redux';
import { setDadosDashboardFrequencia } from '~/redux/modulos/dashboardFrequencia/actions';

const urlPadrao = 'v1/dashboard/frequencias';

class ServicoDashboardFrequencia {
  montarConsultaPadraoGraficos = params => {
    const {
      rota,
      anoLetivo,
      dreId,
      ueId,
      modalidade,
      semestre,
      anoEscolar,
      turmaId,
    } = params;

    let url = `${urlPadrao}/${rota}?anoLetivo=${anoLetivo}`;

    if (dreId) {
      url += `&dreId=${dreId}`;
    }
    if (ueId) {
      url += `&ueId=${ueId}`;
    }
    if (modalidade) {
      url += `&modalidade=${modalidade}`;
    }
    if (semestre) {
      url += `&semestre=${semestre}`;
    }
    if (anoEscolar) {
      url += `&ano=${anoEscolar}`;
    }
    if (turmaId) {
      url += `&turmaId=${turmaId}`;
    }
    return api.get(url);
  };

  obterFrequenciaGlobalPorAno = (
    anoLetivo,
    dreId,
    ueId,
    modalidade,
    semestre
  ) => {
    return this.montarConsultaPadraoGraficos({
      rota: 'global/por-ano',
      anoLetivo,
      dreId,
      ueId,
      modalidade,
      semestre,
    });
  };

  obterFrequenciaGlobalPorDRE = (
    anoLetivo,
    modalidade,
    anoEscolar,
    semestre
  ) => {
    return this.montarConsultaPadraoGraficos({
      rota: 'global/dre',
      anoLetivo,
      modalidade,
      anoEscolar,
      semestre,
    });
  };

  obterQuantidadeAusenciasPossuemJustificativa = (
    anoLetivo,
    dreId,
    ueId,
    modalidade,
    semestre
  ) => {
    return this.montarConsultaPadraoGraficos({
      rota: 'ausencias/justificativas',
      anoLetivo,
      dreId,
      ueId,
      modalidade,
      semestre,
    });
  };

  obterQuantidadeJustificativasMotivo = (
    anoLetivo,
    dreId,
    ueId,
    modalidade,
    semestre,
    anoEscolar,
    turmaId
  ) => {
    return this.montarConsultaPadraoGraficos({
      rota: 'ausencias/motivo',
      anoLetivo,
      dreId,
      ueId,
      modalidade,
      semestre,
      anoEscolar,
      turmaId,
    });
  };

  obterAnosEscolaresPorModalidade = (
    anoLetivo,
    dreId,
    ueId,
    modalidade,
    semestre
  ) => {
    return this.montarConsultaPadraoGraficos({
      rota: 'modalidades/ano',
      anoLetivo,
      dreId,
      ueId,
      modalidade,
      semestre,
    });
  };

  obterUltimaConsolidacao = anoLetivo => {
    return api.get(`${urlPadrao}/consolidacao?anoLetivo=${anoLetivo}`);
  };

  atualizarFiltros = (nomeParametro, valor) => {
    const { dispatch } = store;
    const state = store.getState();

    const { dashboardFrequencia } = state;
    const { dadosDashboardFrequencia } = dashboardFrequencia;

    const novoMap = { ...dadosDashboardFrequencia };
    novoMap[nomeParametro] = valor;

    dispatch(setDadosDashboardFrequencia(novoMap));
  };
}

export default new ServicoDashboardFrequencia();
