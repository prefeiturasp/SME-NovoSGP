import api from '~/servicos/api';

const urlPadrao = 'v1/pendencias';

class ServicoPendencias {
  obterPendenciasListaPaginada = (numeroPagina, numeroRegistros) => {
    return api.get(
      `${urlPadrao}?numeroPagina=${numeroPagina ||
        1}&NumeroRegistros=${numeroRegistros || 5}`
    );
  };
}

export default new ServicoPendencias();
