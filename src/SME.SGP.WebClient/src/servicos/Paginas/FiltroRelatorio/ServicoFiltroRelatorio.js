import api from '~/servicos/api';

const urlPadrao = `v1/relatorios/filtros`;

class ServicoFiltroRelatorio {
  obterDres = () => {
    return api.get(`${urlPadrao}/dres`);
  };

  obterUes = codigoDre => {
    const url = `${urlPadrao}/dres/${codigoDre}/ues`;
    return api.get(url);
  };

  obterModalidades = codigoUe => {
    const url = `${urlPadrao}/ues/${codigoUe}/modalidades`;
    return api.get(url);
  };

  obterAnosEscolares = (codigoUe, modalidade) => {
    const url = `${urlPadrao}/ues/${codigoUe}/modalidades/${modalidade}/anos-escolares`;
    return api.get(url);
  };
}

export default new ServicoFiltroRelatorio();
