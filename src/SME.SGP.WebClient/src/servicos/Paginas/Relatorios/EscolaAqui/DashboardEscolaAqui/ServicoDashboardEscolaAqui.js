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

  obterUltimaAtualizacaoPorProcesso = nomeProcesso => {
    const url = `${urlPadrao}/ultimoProcessamento?nomeProcesso=${nomeProcesso}`;
    return api.get(url);
  };

  obterDadosGraficoTotalComunicadosEnviados = (
    codigoDre,
    codigoUe,
    anoLetivo
  ) => {
    console.log(`DRE: ${codigoDre}`);
    console.log(`UE: ${codigoUe}`);
    console.log(`ANO LETIVO: ${anoLetivo}`);

    const mock = { totalExpirados: 200, totalVigentes: 500 };

    return new Promise(resolve => {
      return setTimeout(() => {
        return resolve({ status: 200, data: mock });
      }, 2000);
    });
  };

  obterDadosGraficoTotalComunicadosPorDRE = (
    codigoDre,
    codigoUe,
    anoLetivo
  ) => {
    console.log(`DRE: ${codigoDre}`);
    console.log(`UE: ${codigoUe}`);
    console.log(`ANO LETIVO: ${anoLetivo}`);

    const mock = [
      {
        nomeDre: 'DRE 01',
        comunicadosvigentes: 20,
        comunicadosExpirados: 30,
      },
      {
        nomeDre: 'DRE 02',
        comunicadosvigentes: 10,
        comunicadosExpirados: 50,
      },
      {
        nomeDre: 'DRE 03',
        comunicadosvigentes: 60,
        comunicadosExpirados: 70,
      },
      {
        nomeDre: 'DRE 04',
        comunicadosvigentes: 50,
        comunicadosExpirados: 70,
      },
      {
        nomeDre: 'DRE 05',
        comunicadosvigentes: 80,
        comunicadosExpirados: 90,
      },
      {
        nomeDre: 'DRE 06',
        comunicadosvigentes: 80,
        comunicadosExpirados: 100,
      },
      {
        nomeDre: 'DRE 07',
        comunicadosvigentes: 500,
        comunicadosExpirados: 300,
      },
      {
        nomeDre: 'DRE 08',
        comunicadosvigentes: 60,
        comunicadosExpirados: 70,
      },
      {
        nomeDre: 'DRE 09',
        comunicadosvigentes: 80,
        comunicadosExpirados: 400,
      },
      {
        nomeDre: 'DRE 10',
        comunicadosvigentes: 500,
        comunicadosExpirados: 700,
      },
      {
        nomeDre: 'DRE 11',
        comunicadosvigentes: 600,
        comunicadosExpirados: 400,
      },
      {
        nomeDre: 'DRE 12',
        comunicadosvigentes: 120,
        comunicadosExpirados: 158,
      },
      {
        nomeDre: 'DRE 13',
        comunicadosvigentes: 167,
        comunicadosExpirados: 70,
      },
    ];
    return new Promise(resolve => {
      return setTimeout(() => {
        return resolve({ status: 200, data: mock });
      }, 2000);
    });
  };
}

export default new ServicoDashboardEscolaAqui();
