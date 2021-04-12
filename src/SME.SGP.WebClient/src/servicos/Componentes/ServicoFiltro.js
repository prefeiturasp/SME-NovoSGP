import api from '~/servicos/api';

class ServicoFiltro {
  static listarAnosLetivos = async ({ consideraHistorico, anoMinimo }) => {
    return api
      .get(
        `v1/abrangencias/${consideraHistorico}/anos-letivos?anoMinimo=${anoMinimo}`
      )
      .then(resposta => resposta);
  };

  static listarModalidades = async ({
    consideraHistorico,
    anoLetivoSelecionado,
  }) => {
    return api
      .get(
        `v1/abrangencias/${consideraHistorico}/modalidades/?anoLetivo=${anoLetivoSelecionado}`
      )
      .then(resposta => resposta);
  };

  static listarPeriodos = async ({
    consideraHistorico,
    modalidadeSelecionada,
    anoLetivoSelecionado,
  }) => {
    return api
      .get(
        `v1/abrangencias/${consideraHistorico}/semestres?anoLetivo=${anoLetivoSelecionado}&modalidade=${modalidadeSelecionada ||
          0}`
      )
      .then(resposta => resposta);
  };

  static listarDres = async ({
    consideraHistorico,
    modalidadeSelecionada,
    periodoSelecionado,
    anoLetivoSelecionado,
  }) => {
    const periodoQuery = periodoSelecionado
      ? `&periodo=${periodoSelecionado}`
      : '';

    return api
      .get(
        `v1/abrangencias/${consideraHistorico}/dres?anoLetivo=${anoLetivoSelecionado}&modalidade=${modalidadeSelecionada ||
          0}${periodoQuery}`
      )
      .then(resposta => resposta);
  };

  static listarUnidadesEscolares = async ({
    consideraHistorico,
    modalidadeSelecionada,
    dreSelecionada,
    periodoSelecionado,
    anoLetivoSelecionado,
  }) => {
    const periodoQuery = periodoSelecionado
      ? `&periodo=${periodoSelecionado}`
      : '';

    return api
      .get(
        `v1/abrangencias/${consideraHistorico}/dres/${dreSelecionada}/ues?anoLetivo=${anoLetivoSelecionado}&modalidade=${modalidadeSelecionada ||
          0}${periodoQuery}`
      )
      .then(resposta => resposta);
  };

  static listarTurmas = async ({
    consideraHistorico,
    modalidadeSelecionada,
    unidadeEscolarSelecionada,
    periodoSelecionado,
    anoLetivoSelecionado,
  }) => {
    const periodoQuery = periodoSelecionado
      ? `&periodo=${periodoSelecionado}`
      : '';

    return api
      .get(
        `v1/abrangencias/${consideraHistorico}/dres/ues/${unidadeEscolarSelecionada}/turmas?anoLetivo=${anoLetivoSelecionado}&modalidade=${modalidadeSelecionada ||
          0}${periodoQuery}`
      )
      .then(resposta => resposta);
  };
}

export default ServicoFiltro;
