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

  obterComunicadosTotaisSme = (codigoDre, codigoUe, anoLetivo) => {
    const url = `${urlPadrao}/comunicados/totais?anoLetivo=${anoLetivo}&codigoDre=${codigoDre}&codigoUe=${codigoUe}`;
    return api.get(url);
  };

  obterComunicadosTotaisAgrupadosPorDre = anoLetivo => {
    const url = `${urlPadrao}/comunicados/totais/agrupados?anoLetivo=${anoLetivo}`;
    return api.get(url);
  };

  obterComunicadosAutoComplete = descricao => {
    // TODO Alterar rota!
    return api.get(
      `${urlPadrao}/comunicados/auto-complete?descricao=${descricao}`
    );
  };
}

export default new ServicoDashboardEscolaAqui();
