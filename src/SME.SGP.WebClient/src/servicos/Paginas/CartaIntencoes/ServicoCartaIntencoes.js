// import * as moment from 'moment';
import api from '~/servicos/api';

const urlPadrao = '/v1/carta-intencoes';

class ServicoCartaIntencoes {
  obterBimestres = (turmaCodigo, componenteCurricularId) => {
    const url = `${urlPadrao}/turmas/${turmaCodigo}/componente-curricular/${componenteCurricularId}`;
    return api.get(url);
  };

  salvarEditarCartaIntencoes = dados => {
    return api.post(urlPadrao, dados);
  };
}

export default new ServicoCartaIntencoes();
