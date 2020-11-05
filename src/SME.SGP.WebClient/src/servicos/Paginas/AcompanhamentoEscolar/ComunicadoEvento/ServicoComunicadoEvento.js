import api from '../../../api';


class ServicoComunicadoEvento {
    listarPor = async (options) => {
        let result = [];
        const endpoint = 'v1/comunicadoevento/ListarPorCalendario';

        try {
            const response = await api.post( endpoint, options);
            result = response && response.data ? response.data : [];
        } catch(ex) {
            console.error(`[ ERROR ] Failed to perform api call ${endpoint}: `, ex);
            result = [];
        } finally {
            return result;
        }
    };
}


export default new ServicoComunicadoEvento();
