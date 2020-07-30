import api from '~/servicos/api';
import modalidade from '~/dtos/modalidade';
import { erros } from '~/servicos/alertas';

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

  obterTurmasPorCodigoUeModalidadeSemestre = async (
    anoLetivo,
    codigoUe,
    modalidade,
    semestre
  ) => {
    try {
      let url = `${urlPadrao}/ues/${codigoUe}/anoletivo/${anoLetivo}/turmas?`;

      if (semestre && semestre != 0) url += `semestre=${semestre}&`;

      if (modalidade && modalidade != 0) url += `modalidade=${modalidade}`;

      var dados = await api.get(url);

      return dados;
    } catch (error) {
      erros(error);
      return [];
    }
  };
}

export default new ServicoFiltroRelatorio();
