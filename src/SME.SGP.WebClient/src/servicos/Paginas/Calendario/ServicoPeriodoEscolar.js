import api from '~/servicos/api';
import moment from 'moment';

class ServicoPeriodoEscolar {
    obterPeriodosAbertos = async (turma, dataReferencia = null) => {
        let url = `v1/periodo-escolar/turmas/${turma}/bimestres/aberto`;
        if (dataReferencia) {
            dataReferencia = moment(dataReferencia).format('YYYY-MM-DD');
            url = `${url}?dataReferencia=${dataReferencia}`;
        }
        return api.get(url);
    };
};

export default new ServicoPeriodoEscolar();
