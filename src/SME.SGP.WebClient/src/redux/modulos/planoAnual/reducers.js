import produce from 'immer';

const INICIAL = {
    bimestres: [],
};

export default function bimestres(state = INICIAL, action) {
    return produce(state, draft => {

        switch (action.type) {
            case "@bimestres/SalvarBimestre":

                draft.bimestres[action.payload.indice] = action.payload.bimestre;

                break;
            case "@bimestres/PrePostBimestre":

                const paraEnvio = state.bimestres.filter(x => x.ehExpandido);

                paraEnvio.forEach(elem => {
                    draft.bimestres[elem.indice].objetivo = state.bimestres[elem.indice].setarObjetivo();
                    draft.bimestres[elem.indice].paraEnviar = true;
                })

                break;

            case "@bimestres/SalvarMateria":

                draft.bimestres[action.payload.indice].materias = action.payload.materias;

                break;

            case "@bimestres/SalvarEhExpandido":

                draft.bimestres[action.payload.indice].ehExpandido = action.payload.ehExpandido;

                break;

            case '@bimestres/SelecionarMateria':

                draft.bimestres[action.payload.indice].materias[action.payload.indiceMateria].selected = action.payload.selecionarMateria;

                break;

            case '@bimestres/SalvarObjetivos':

                draft.bimestres[action.payload.indice].objetivosAprendizagem = action.payload.objetivos;

                break;

            case '@bimestres/SetarDescricaoFunction':

                draft.bimestres[action.payload.indice].setarObjetivo = action.payload.setarObjetivo;

                break;

            case '@bimestres/SelecionarObjetivo':

                draft.bimestres[action.payload.indice].objetivosAprendizagem[action.payload.indiceObjetivo].selected = action.payload.selecionarObjetivo;

                break;


            case '@bimestres/DefinirObjetivoFocado':

                draft.bimestres[action.payload.indice].objetivoIdFocado = action.payload.codigoObjetivo;

                break;

            case '@bimestres/SetarDescricao':

                draft.bimestres[action.payload.indice].objetivo = action.payload.descricao;

                break;

            default:
                break;
        }

    });
}