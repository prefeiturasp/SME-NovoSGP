import api from '../../../api';

class ServicoComunicadoEvento {
  listarPor = async parametros => {
    return api.post('v1/comunicadoevento/ListarPorCalendario', parametros);
  };
}

export default new ServicoComunicadoEvento();
