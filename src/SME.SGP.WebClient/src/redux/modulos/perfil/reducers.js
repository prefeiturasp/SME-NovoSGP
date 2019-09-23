import produce from 'immer';

const inicial = {
    perfilSelecionado : {}
};

export default function perfil(state = inicial, action) {
    return produce(state, draft => {
        if (action.type === '@perfil/perfilSelecionado') {
            draft.perfilSelecionado = action.payload;
        }
    });
}
