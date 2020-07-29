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

  gerar = params => {
    const url = '/v1/relatorios/pareceres-conclusivos';
    return api.post(url, params);
  };
}

export default new ServicoRelatorioParecerConclusivo();
