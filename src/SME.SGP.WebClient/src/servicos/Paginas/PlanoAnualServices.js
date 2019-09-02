import API from '../api';

const Service = {

    getBaseUrlMateriasProfessor: (RF, CodigoTurma) => {
        return `v1/professores/${RF}/turmas/${CodigoTurma}/disciplinas/`;
    },

    getMateriasProfessor: async (RF, CodigoTurma) => {

        const requisicao = await API.get(Service.getBaseUrlMateriasProfessor(RF, CodigoTurma));

        return requisicao.data.map(req => {

            return {
                codigo: req.codigoComponenteCurricular,
                materia: req.nome
            };

        });

    },

    getObjetivoseByDisciplinas: async (disciplinas, Ano) => {

        console.log(disciplinas);

    }

}

export default Service;