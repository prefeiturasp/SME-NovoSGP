import api from '~/servicos/api';

class ServicoFiltro {
  static listarAnosLetivos = async () => {
    return api.get('v1/abrangencias/anos-letivos').then(resposta => resposta);
  };

  static listarModalidades = async () => {
    return api.get('v1/abrangencias/modalidades').then(resposta => resposta);
  };

  static listarPeriodos = async () => {
    return api.get('v1/abrangencias/semestres').then(resposta => resposta);
  };

  static listarDres = async () => {
    return api.get('v1/abrangencias/dres').then(resposta => resposta);
  };

  static listarUnidadesEscolares = async dre => {
    return api.get(`v1/abrangencias/dres/${dre}/ues`).then(resposta => resposta);
  };

  static listarTurmas = async ue => {
    return api
      .get(`v1/abrangencias/dres/ues/${ue}/turmas`)
      .then(resposta => resposta);
  };
}

export default ServicoFiltro;
