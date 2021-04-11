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

  obterQuantidadeRegistrosPAAI = (anoLetivo, dreId, ueId, mes) => {
    return this.montarConsultaPadraoGraficos(
      'visitas-paais',
      anoLetivo,
      dreId,
      ueId,
      mes
    );
  };

  obterQuantidadeRegistrosPorObjetivo = (anoLetivo, dreId, ueId, mes, rf) => {
    return this.montarConsultaPadraoGraficos(
      'objetivos',
      anoLetivo,
      dreId,
      ueId,
      mes,
      rf
    );
  };
}

export default new ServicoDashboardRegistroItinerancia();
