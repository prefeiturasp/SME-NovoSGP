import ServicoFiltro from '~/servicos/Componentes/ServicoFiltro';

export default class FiltroHelper {
  static async ObtenhaAnosLetivos() {
    const anosLetivos = [];

    return await ServicoFiltro.listarAnosLetivos()
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(ano => {
            anosLetivos.push({ desc: ano, valor: ano });
          });
        }

        return anosLetivos;
      })
      .catch(() => anosLetivos);
  }

  static async ObtenhaModalidades() {
    const modalidadesLista = [];

    return await ServicoFiltro.listarModalidades()
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(modalidade => {
            modalidadesLista.push({
              desc: modalidade.descricao,
              valor: modalidade.id,
            });
          });
        }

        return modalidadesLista;
      })
      .catch(() => modalidadesLista);
  }

  static async ObtenhaPeriodos(modalidade) {
    const periodos = [];

    return await ServicoFiltro.listarPeriodos(modalidade)
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(periodo => {
            periodos.push({ desc: periodo, valor: periodo });
          });
        }

        return periodos;
      })
      .catch(() => periodos);
  }

  static async ObtenhaDres(modalidade) {
    const dres = [];

    return await ServicoFiltro.listarDres(modalidade)
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(dre => {
            dres.push({
              desc: dre.nome,
              valor: dre.codigo,
              abrev: dre.abreviacao,
            });
          });
        }
        return dres;
      })
      .catch(() => dres);
  }

  static async ObtenhaUnidadesEscolares(modalidade, dre) {
    const unidadesEscolares = [];

    return await ServicoFiltro.listarUnidadesEscolares(dre, modalidade)
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(unidade => {
            unidadesEscolares.push({
              desc: unidade.nome,
              valor: unidade.codigo,
            });
          });
        }
        return unidadesEscolares;
      })
      .catch(() => unidadesEscolares);
  }

  static async ObtenhaTurmas(modalidade, unidadeEscolar) {
    const turmas = [];

    return await ServicoFiltro.listarTurmas(unidadeEscolar, modalidade)
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(turma => {
            turmas.push({
              desc: turma.nome,
              valor: turma.codigo,
              ano: turma.ano,
            });
          });
        }
        return turmas;
      })
      .catch(() => turmas);
  }
}
