import api from '~/servicos/api';

class ServicoRelatorioParecerConclusivo {
  buscarCiclos = (codigoUe, modalidade) => {
    const url = `/v1/relatorios/filtros/ues/${codigoUe}/modalidades/${modalidade}/ciclos`;
    return api.get(url);
  };

  buscarPareceresConclusivos = () => {
    const url = `/v1/conselhos-classe/pareceres-conclusivos`;
    return api.get(url);
  };

  gerar = params => {
    const url = '/v1/relatorios/pareceres-conclusivos';
    return api.post(url, params);
  };
}

export default new ServicoRelatorioParecerConclusivo();
