import api from '~/servicos/api';

const urlPadrao = 'v1/dashboard/registro-itinerancia';

class ServicoDashboardRegistroItinerancia {
  montarConsultaPadraoGraficos = (rota, anoLetivo, dreId, ueId, mes, rf) => {
    let url = `${urlPadrao}/${rota}?anoLetivo=${anoLetivo}`;
    if (dreId) {
      url += `&dreId=${dreId}`;
    }
    if (ueId) {
      url += `&ueId=${ueId}`;
    }
    if (mes) {
      url += `&mes=${mes}`;
    }
    if (rf) {
      url += `&rf=${rf}`;
    }

    return api.get(url);
  };

  obterQuantidadeRegistrosPAAI = (
    anoLetivo,
    dreId,
    ueId,
    dreCodigo,
    ueCodigo,
    mes
  ) => {
    return this.montarConsultaPadraoGraficos(
      'visitas-paais',
      anoLetivo,
      dreId,
      ueId,
      mes,
      dreCodigo,
      ueCodigo
    );
  };

  obterQuantidadeRegistrosPorObjetivo = (
    anoLetivo,
    dreId,
    ueId,
    dreCodigo,
    ueCodigo,
    mes,
    rf
  ) => {
    return this.montarConsultaPadraoGraficos(
      'objetivos',
      anoLetivo,
      dreId,
      ueId,
      mes,
      rf,
      dreCodigo,
      ueCodigo
    );
  };
}

export default new ServicoDashboardRegistroItinerancia();
