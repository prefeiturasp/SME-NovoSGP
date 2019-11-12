import api from '~/servicos/api';

class ServicoFiltro {
  static listarAnosLetivos = async () => {
    return api.get('v1/abrangencias/anos-letivos').then(resposta => resposta);
  };

  static listarModalidades = async () => {
    return api.get('v1/abrangencias/modalidades').then(resposta => resposta);
  };

  static listarPeriodos = async modalidade => {
    return api
      .get(`v1/abrangencias/semestres?modalidade=${modalidade}`)
      .then(resposta => resposta);
  };

  static listarDres = async modalidade => {
    return api
      .get(`v1/abrangencias/dres?modalidade=${modalidade}`)
      .then(resposta => resposta);
  };

  static listarUnidadesEscolares = async (dre, modalidade) => {
    return api
      .get(`v1/abrangencias/dres/${dre}/ues?modalidade=${modalidade}`)
      .then(resposta => resposta);
  };

  static listarTurmas = async (ue, modalidade) => {
    return api
      .get(`v1/abrangencias/dres/ues/${ue}/turmas?modalidade=${modalidade}`)
      .then(resposta => resposta);
  };
}

export default ServicoFiltro;
