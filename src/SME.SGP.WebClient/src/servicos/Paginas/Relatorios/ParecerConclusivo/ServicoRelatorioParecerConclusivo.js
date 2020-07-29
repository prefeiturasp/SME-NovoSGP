import api from '~/servicos/api';

class ServicoRelatorioParecerConclusivo {
  buscarCiclos = () => {
    return Promise.resolve({
      data: [
        { valor: '1', desc: 'Alfabetização' },
        { valor: '2', desc: 'Teste' },
      ],
    });
  };

  buscarPareceresConclusivos = () => {
    return Promise.resolve({
      data: [
        { valor: '1', desc: 'Plenamente satisfatório' },
        { valor: '2', desc: 'Satisfatório' },
        { valor: '3', desc: 'Insuficiente' },
      ],
    });
  };

  gerar = async params => {
    const url = '/v1/relatorios/fechamentos/pendencias';
    return await api.post(url, params);
  };
}

export default new ServicoRelatorioParecerConclusivo();
