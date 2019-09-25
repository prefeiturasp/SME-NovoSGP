import produce from 'immer';

const inicial = {
  perfilSelecionado: {
    id: "2",
    descricao: 'Professor'
  },
  perfis: []
};

export default function perfil(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@perfil/perfilSelecionado':
        draft.perfilSelecionado = action.payload;
        break;
      case '@perfil/perfis':
        draft.perfis = action.payload;
        break;
      default:
        break;
    }
  });
}
