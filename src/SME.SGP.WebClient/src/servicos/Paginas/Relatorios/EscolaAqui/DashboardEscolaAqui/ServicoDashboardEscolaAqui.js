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

  obterComunicadosAutoComplete = (
    anoLetivo,
    codigoDre,
    codigoUe,
    gruposIds,
    modalidade,
    semestre,
    anoEscolar,
    codigoTurma,
    dataEnvioInicial,
    dataEnvioFinal,
    descricao
  ) => {
    let url = `${urlPadrao}/comunicados/filtro`;
    url += `?anoLetivo=${anoLetivo}`;

    if (codigoDre) {
      url += `&codigoDre=${codigoDre}`;
    }

    if (codigoUe) {
      url += `&codigoUe=${codigoUe}`;
    }

    if (descricao) {
      url += `&descricao=${descricao}`;
    }

    if (modalidade) {
      url += `&modalidade=${modalidade}`;
    }

    if (semestre) {
      url += `&semestre=${semestre}`;
    }

    if (anoEscolar) {
      url += `&anoEscolar=${anoEscolar}`;
    }

    if (codigoTurma) {
      url += `&codigoTurma=${codigoTurma}`;
    }

    if (dataEnvioInicial) {
      url += `&dataEnvioInicial=${dataEnvioInicial.format('YYYY-MM-DD')}`;
    }

    if (dataEnvioFinal) {
      url += `&dataEnvioFinal=${dataEnvioFinal.format('YYYY-MM-DD')}`;
    }

    if (gruposIds?.length) {
      url += `&gruposIds=${gruposIds.join('&gruposIds=', gruposIds)}`;
    }

    return api.get(url);
  };

  obterDadosDeLeituraDeComunicados = (
    codigoDre,
    codigoUe,
    notificacaoId,
    modoVisualizacao
  ) => {
    const url = `${urlPadrao}/comunicados/leitura?codigoDre=${codigoDre}&codigoUe=${codigoUe}&notificacaoId=${notificacaoId}&modoVisualizacao=${modoVisualizacao}`;
    return api.get(url);
  };

  obterDadosDeLeituraDeComunicadosAgrupadosPorDre = (
    notificacaoId,
    modoVisualizacao
  ) => {
    const url = `${urlPadrao}/comunicados/leitura/agrupados?notificacaoId=${notificacaoId}&modoVisualizacao=${modoVisualizacao}`;
    return api.get(url);
  };

  obterDadosDeLeituraDeComunicadosPorModalidades = (
    codigoDre,
    codigoUe,
    notificacaoId,
    modoVisualizacao
  ) => {
    let url = `${urlPadrao}/comunicados/leitura/modalidades?notificacaoId=${notificacaoId}&modoVisualizacao=${modoVisualizacao}`;
    if (codigoDre) {
      url += `&codigoDre=${codigoDre}`;
    }

    if (codigoUe) {
      url += `&codigoUe=${codigoUe}`;
    }
    return api.get(url);
  };

  obterDadosDeLeituraDeComunicadosPorModalidadeETurmas = (
    codigoDre,
    codigoUe,
    notificacaoId,
    modoVisualizacao,
    modalidades,
    codigosTurmas
  ) => {
    let url = `${urlPadrao}/comunicados/leitura/turmas?codigoDre=${codigoDre}&codigoUe=${codigoUe}&notificacaoId=${notificacaoId}&modoVisualizacao=${modoVisualizacao}`;
    if (modalidades?.length) {
      url += `&modalidades=${modalidades.join('&modalidades=', modalidades)}`;
    }
    if (codigosTurmas?.length) {
      url += `&codigosTurmas=${codigosTurmas.join(
        '&codigosTurmas=',
        codigosTurmas
      )}`;
    }
    return api.get(url);
  };

  obterDadosLeituraDeComunicadosPorAlunos = codigosTurmas => {
    // TODO Remover mock!
    // const url = `${urlPadrao}/comunicados/leitura/alunos?codigosTurmas=${codigosTurmas.join(
    //   '&codigosTurmas=',
    //   codigosTurmas
    // )}`;

    // return api.get(url);

    const mock = [
      {
        id: 1,
        numeroChamada: 1,
        nome: 'Alvaro Ramos Grassi (1257845)',
        responsavel: 'Maria Joaquina (Filiação 1)',
        contatoResponsavel: '(11) 94455-4455',
        possuiAplicativo: 'Sim',
        leuMensagem: 'Não',
        dataLeitura: new Date(),
        marcador: { descricao: 'Estudante transferido em xx/xx/xxxx.' },
      },
      {
        id: 2,
        numeroChamada: 2,
        nome: 'Aline Grassi (1257845)',
        responsavel: 'João Grassi (Filiação 1)',
        contatoResponsavel: '(11) 94455-4455',
        possuiAplicativo: 'Sim',
        leuMensagem: 'Sim',
        dataLeitura: new Date(),
        marcador: null,
      },
    ];
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ status: 200, data: mock });
      }, 2000);
    });
  };
}

export default new ServicoDashboardEscolaAqui();
