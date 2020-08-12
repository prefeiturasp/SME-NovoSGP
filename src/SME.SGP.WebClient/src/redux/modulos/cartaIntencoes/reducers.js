import produce from 'immer';

const inicial = {
  cartaIntencoesEmEdicao: false,
  dadosCartaIntencoes: [],
  dadosParaSalvarCartaIntencoes: [],
  desabilitarCampos: false,
  exibirModalErrosCartaIntencoes: false,
  errosCartaIntencoes: [],
  carregandoGeral: false,
};

export default function cartaIntencoes(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@cartaIntencoes/limparDadosCartaIntencoes': {
        return {
          ...draft,
          cartaIntencoesEmEdicao: false,
          dadosCartaIntencoes: [],
          dadosParaSalvarCartaIntencoes: [],
          desabilitarCampos: false,
          exibirModalErrosCartaIntencoes: false,
          errosCartaIntencoes: [],
          carregandoGeral: false,
        };
      }
      case '@cartaIntencoes/setCartaIntencoesEmEdicao': {
        return {
          ...draft,
          cartaIntencoesEmEdicao: action.payload,
        };
      }
      case '@cartaIntencoes/setDadosCartaIntencoes': {
        return {
          ...draft,
          dadosCartaIntencoes: action.payload,
        };
      }
      case '@cartaIntencoes/setDadosParaSalvarCartaIntencoes': {
        const dados = state.dadosParaSalvarCartaIntencoes;
        if (dados.length > 0) {
          const valor = dados.find(
            item => item.bimestre === action.payload.bimestre
          );
          if (valor) {
            const indexItem = dados.findIndex(
              item => item.bimestre === action.payload.bimestre
            );
            draft.dadosParaSalvarCartaIntencoes[indexItem] = action.payload;
          } else {
            draft.dadosParaSalvarCartaIntencoes.push(action.payload);
          }
        } else {
          draft.dadosParaSalvarCartaIntencoes.push(action.payload);
        }
        break;
      }

      case '@cartaIntencoes/limparDadosParaSalvarCartaIntencoes': {
        return {
          ...draft,
          dadosParaSalvarCartaIntencoes: [],
        };
      }
      case '@cartaIntencoes/setDesabilitarCampos': {
        return {
          ...draft,
          desabilitarCampos: action.payload,
        };
      }
      case '@cartaIntencoes/setExibirModalErrosCartaIntencoes': {
        return {
          ...draft,
          exibirModalErrosCartaIntencoes: action.payload,
        };
      }
      case '@cartaIntencoes/setErrosCartaIntencoes': {
        return {
          ...draft,
          errosCartaIntencoes: action.payload,
        };
      }
      case '@cartaIntencoes/setCarregandoCartaIntencoes': {
        return {
          ...draft,
          carregandoCartaIntencoes: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
