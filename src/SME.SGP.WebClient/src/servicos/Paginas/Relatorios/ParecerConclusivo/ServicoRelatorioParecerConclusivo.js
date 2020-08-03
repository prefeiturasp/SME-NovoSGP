import api from '~/servicos/api';

class ServicoRelatorioParecerConclusivo {
  buscarCiclos = params => {
    const url = `/v1/relatorios/filtros/ciclos`;
    return api.post(url, params);
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
