import api from '~/servicos/api';

const anoAtual = window.moment().format('YYYY');

const EventosServico = {
  buscarTiposCalendario(ano) {
    return api.get(
      `v1/calendarios/tipos/anos/letivos/${!ano ? anoAtual : ano}`
    );
  },
  buscarTipoEventos() {
    return api.get(`v1/calendarios/eventos/tipos/listar`);
  },
};

export default EventosServico;
