import Servico from '../../../servicos/Paginas/PlanoAnualServices';

export function Salvar(indice, bimestre) {
    return {
        type: '@bimestres/SalvarBimestre',
        payload: {
            indice, bimestre
        },
    };
}

export function SalvarMaterias(indice, materias) {
    return {
        type: '@bimestres/SalvarMateria',
        payload: {
            indice, materias
        }
    }
}

export function SalvarEhExpandido(indice, ehExpandido) {
    return {
        type: '@bimestres/SalvarEhExpandido',
        payload: {
            indice, ehExpandido
        }
    }
}

export function SalvarObjetivos(indice, objetivos) {
    return {
        type: '@bimestres/SalvarObjetivos',
        payload: {
            indice, objetivos
        }
    }
}

export function SetarDescricaoFunction(indice, setarObjetivo) {
    return {
        type: '@bimestres/SetarDescricaoFunction',
        payload: {
            indice, setarObjetivo
        }
    }
}

export function SelecionarMateria(indice, indiceMateria, selecionarMateria) {
    return {
        type: '@bimestres/SelecionarMateria',
        payload: {
            indice, indiceMateria, selecionarMateria
        }
    }
}

export function SelecionarObjetivo(indice, indiceObjetivo, selecionarObjetivo) {
    return {
        type: '@bimestres/SelecionarObjetivo',
        payload: {
            indice, indiceObjetivo, selecionarObjetivo
        }
    }
}

export function DefinirObjetivoFocado(indice, codigoObjetivo) {
    return {
        type: '@bimestres/DefinirObjetivoFocado',
        payload: {
            indice, codigoObjetivo
        }
    }
}

export function SetarDescricao(indice, descricao) {
    return {
        type: '@bimestres/SetarDescricao',
        payload: {
            indice, descricao
        }
    }
}

export function ObterObjetivosCall(bimestre) {

    return dispatch => {

        if (!bimestre.materias || bimestre.materias.length === 0) {
            dispatch(SalvarObjetivos(bimestre.indice, []));
            return;
        }

        const materiasSelecionadas = bimestre.materias.filter(materia => materia.selected).map(x => x.codigo);

        if (!materiasSelecionadas || materiasSelecionadas.length === 0) {
            dispatch(SalvarObjetivos(bimestre.indice, []));
            return;
        }

        dispatch(SalvarEhExpandido(bimestre.indice, true));

        Servico.getObjetivoseByDisciplinas(bimestre.anoEscolar, materiasSelecionadas)
            .then(res => {

                if (!bimestre.objetivosAprendizagem || bimestre.objetivosAprendizagem.length === 0) {
                    dispatch(SalvarObjetivos(bimestre.indice, res));
                    return;
                }

                dispatch(SalvarObjetivos(bimestre.indice, res));

                const concatenados = bimestre.objetivosAprendizagem.concat(res.filter(item => {

                    const index = bimestre.objetivosAprendizagem.findIndex(x => x.codigo === item.codigo);

                    return index < 0;

                }));

                dispatch(SalvarObjetivos(bimestre.indice, concatenados));
            });
    }
}


export function PrePost() {
    return {
        type: '@bimestres/PrePostBimestre'
    };
}