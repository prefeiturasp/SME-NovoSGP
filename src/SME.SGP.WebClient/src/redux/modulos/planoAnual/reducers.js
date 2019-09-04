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
            default:
                break;
        }

    });
}