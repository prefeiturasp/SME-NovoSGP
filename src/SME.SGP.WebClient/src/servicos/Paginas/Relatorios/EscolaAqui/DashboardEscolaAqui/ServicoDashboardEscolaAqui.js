import api from '~/servicos/api';

const urlPadrao = `v1/ea/dashboard`;

class ServicoDashboardEscolaAqui {
  obterDadosGraficoAdesao = (codigoDre, codigoUe) => {
    let url = `${urlPadrao}/adesao`;
    if (codigoDre && !codigoUe) {
      url = `${url}?codigoDre=${codigoDre}`;
    }
    if (codigoDre && codigoUe) {
      url = `${url}?codigoDre=${codigoDre}&codigoUe=${codigoUe}`;
    }
    return api.get(url);
  };

  obterDadosGraficoAdesaoAgrupados = () => {
    const url = `${urlPadrao}/adesao/agrupados`;
    return api.get(url);
  };

  obterUltimaAtualizacaoPorProcesso = nomeProcesso => {
    const url = `${urlPadrao}/ultimoProcessamento?nomeProcesso=${nomeProcesso}`;
    return api.get(url);
  };
}

export default new ServicoDashboardEscolaAqui();
