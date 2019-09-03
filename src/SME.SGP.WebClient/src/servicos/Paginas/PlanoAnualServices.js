import API from '../api';

const Service = {

    _getBaseUrlMateriasProfessor: (RF, CodigoTurma) => {
        return `v1/professores/${RF}/turmas/${CodigoTurma}/disciplinas/`;
    },

    _getBaseUrlObjetivosFiltro: () => {
        return `v1/objetivos-aprendizagem`;
    },

    getMateriasProfessor: async (RF, CodigoTurma) => {

        const requisicao = await API.get(Service._getBaseUrlMateriasProfessor(RF, CodigoTurma));

        return requisicao.data.map(req => {

            return {
                codigo: req.codigoComponenteCurricular,
                materia: req.nome
            };

        });

    },

    getObjetivoseByDisciplinas: async (Ano, disciplinas) => {

        const corpoRequisicao = {
            "Ano": Ano,
            "ComponentesCurricularesIds": disciplinas
        };

        const requisicao =  await API.post(Service._getBaseUrlObjetivosFiltro(), corpoRequisicao);

        return requisicao.data;
    }

}

export default Service;