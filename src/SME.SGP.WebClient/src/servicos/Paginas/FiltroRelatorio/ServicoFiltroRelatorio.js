import api from '~/servicos/api';
import { erros } from '~/servicos/alertas';

const urlPadrao = `v1/relatorios/filtros`;

class ServicoFiltroRelatorio {
  obterDres = () => {
    return api.get(`${urlPadrao}/dres`);
  };

  obterUes = (codigoDre, consideraNovasUEs = false) => {
    const url = `${urlPadrao}/dres/${codigoDre}/ues?consideraNovasUEs=${consideraNovasUEs}`;
    return api.get(url);
  };

  obterModalidades = (codigoUe, anoLetivo, consideraHistorico) => {
    const url = `${urlPadrao}/ues/${codigoUe}/modalidades?anoLetivo=${anoLetivo}&consideraHistorico=${consideraHistorico}`;
    return api.get(url);
  };

  obterModalidadesAnoLetivo = (
    codigoUe,
    anoLetivo,
    consideraNovasModalidades = false
  ) => {
    const url = `${urlPadrao}/ues/${codigoUe}/${anoLetivo}/modalidades?consideraNovasModalidades=${consideraNovasModalidades}`;
    return api.get(url);
  };

  obterModalidadesPorAbrangencia = (
    codigoUe,
    consideraNovasModalidades = false
  ) => {
    const url = `${urlPadrao}/ues/${codigoUe}/modalidades/abrangencias?consideraNovasModalidades=${consideraNovasModalidades}`;
    return api.get(url);
  };

  obterAnosEscolares = (codigoUe, modalidade) => {
    const url = `${urlPadrao}/ues/${codigoUe}/modalidades/${modalidade}/anos-escolares`;
    return api.get(url);
  };

  obterAnosEscolaresPorAbrangencia = (modalidade, cicloId) => {
    const url = `${urlPadrao}/modalidades/${modalidade}/ciclos/${cicloId}/anos-escolares`;
    return api.get(url);
  };

  buscarCiclos = (codigoUe, modalidade) => {
    const url = `/v1/relatorios/filtros/ues/${codigoUe}/modalidades/${modalidade}/ciclos?consideraAbrangencia=true`;
    return api.get(url);
  };

  obterTurmasPorCodigoUeModalidadeSemestre = async (
    anoLetivo,
    codigoUe,
    modalidade,
    semestre
  ) => {
    try {
      let url = `${urlPadrao}/ues/${codigoUe}/anoletivo/${anoLetivo}/turmas?`;

      if (semestre && semestre !== 0) url += `semestre=${semestre}&`;

      if (modalidade && modalidade !== 0) url += `modalidade=${modalidade}`;

      const dados = await api.get(url);

      return dados;
    } catch (error) {
      erros(error);
      return [];
    }
  };

  obterComponetensCuriculares = (
    codigoUe,
    modalidade,
    anoLetivo,
    anosEscolares
  ) => {
    const url = `${urlPadrao}/componentes-curriculares/anos-letivos/${anoLetivo}/ues/${codigoUe}/modalidades/${modalidade}/?anos=${anosEscolares.join(
      '&anos=',
      anosEscolares
    )}`;
    return api.get(url);
  };

  obterTurmasEspecificas = (
    codigoUe,
    anoLetivo,
    semestre,
    modalidade,
    anosEscolares
  ) => {
    const codigoUeAndAnoLetivo = `${urlPadrao}/turmas/ues/${codigoUe}/anoletivo/${anoLetivo}?`;
    const semestreParams = semestre && `semestre=${semestre}`;
    const modalidadePrams = modalidade && `&modalidade=${modalidade}`;
    const anosParams =
      anosEscolares && `&anos=${anosEscolares.join('&anos=', anosEscolares)}`;

    const url =
      codigoUeAndAnoLetivo + semestreParams + modalidadePrams + anosParams;

    return api.get(url);
  };
}

export default new ServicoFiltroRelatorio();
