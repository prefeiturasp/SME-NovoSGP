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

export function SalvarObjetivos(indice, objetivos){
    return {
        type: '@bimestres/SalvarObjetivos',
        payload: {
            indice, objetivos
        }
    }
}

export function SetarDescricaoFunction(indice, setarObjetivo){
    return {
        type: '@bimestres/SetarDescricaoFunction',
        payload: {
            indice, setarObjetivo
        }
    }
}

export function SelecionarMateria(indice, indiceMateria, selecionarMateria){
    return {
        type: '@bimestres/SelecionarMateria',
        payload: {
            indice, indiceMateria, selecionarMateria
        }
    }
}

export function SelecionarObjetivo(indice, indiceObjetivo, selecionarObjetivo){
    return {
        type: '@bimestres/SelecionarObjetivo',
        payload: {
            indice, indiceObjetivo, selecionarObjetivo
        }
    }
}

export function DefinirObjetivoFocado(indice, codigoObjetivo){
    return {
        type: '@bimestres/DefinirObjetivoFocado',
        payload: {
            indice, codigoObjetivo
        }
    }
}

export function SetarDescricao(indice, descricao){
    return {
        type: '@bimestres/SetarDescricao',
        payload: {
            indice, descricao
        }
    }
}


export function PrePost() {
    return {
        type: '@bimestres/PrePostBimestre'
    };
}