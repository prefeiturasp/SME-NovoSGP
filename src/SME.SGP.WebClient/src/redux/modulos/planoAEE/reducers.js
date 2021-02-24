import produce from 'immer';

const inicial = {
  planoAEEDados: {},
  desabilitarCamposPlanoAEE: false,
  planoAEEEmEdicao: false,
  exibirLoaderPlanoAEE: false,
  planoAEESituacaoEncaminhamentoAEE: {},
  planoAEEDadosSecoesPorEtapa: [],
  exibirModalErrosPlano: false,
  dadosDevolutiva: {},
  parecerCoordenacao: '',
  parecerPAAI: '',
  devolutivaEmEdicao: false,
};

export default function PlanoAEE(state = inicial, action) {
  return produce(state, draft => {
    console.log('state', state);
    switch (action.type) {
      case '@planoAEE/setExibirModalErrosPlano': {
        return {
          ...draft,
          exibirModalErrosPlano: action.payload,
        };
      }

      case '@planoAEE/setExibirLoaderPlanoAEE': {
        return {
          ...draft,
          exibirLoaderPlanoAEE: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEEDados': {
        return {
          ...draft,
          planoAEEDados: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEESituacaoEncaminhamentoAEE': {
        return {
          ...draft,
          planoAEESituacaoEncaminhamentoAEE: action.payload,
        };
      }
      case '@planoAEE/setDesabilitarCamposPlanoAEE': {
        return {
          ...draft,
          desabilitarCamposPlanoAEE: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEEEmEdicao': {
        return {
          ...draft,
          planoAEEEmEdicao: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEEDadosSecoesPorEtapa': {
        return {
          ...draft,
          planoAEEDadosSecoesPorEtapa: action.payload,
        };
      }
      case '@planoAEE/setPlanoAEELimparDados': {
        return {
          ...draft,
          planoAEESituacaoEncaminhamentoAEE: {},
          planoAEEDados: null,
          exibirLoaderPlanoAEE: false,
          desabilitarCamposPlanoAEE: false,
          planoAEEDadosSecoesPorEtapa: [],
          exibirModalErrosPlano: false,
        };
      }
      case '@planoAEE/setAtualizarPlanoAEEDados': {
        return {
          ...draft,
          planoAEEDados: {
            ...draft.planoAEEDados,
            ...action.payload,
          },
        };
      }
      case '@planoAEE/setDadosDevolutiva': {
        return {
          ...draft,
          dadosDevolutiva: action.payload,
        };
      }
      case '@planoAEE/setParecerCoordenacao': {
        return {
          ...draft,
          parecerCoordenacao: action.payload,
        };
      }
      case '@planoAEE/setParecerPAAI': {
        return {
          ...draft,
          parecerPAAI: action.payload,
        };
      }
      case '@planoAEE/setDevolutivaEmEdicao': {
        return {
          ...draft,
          devolutivaEmEdicao: action.payload,
        };
      }
      case '@planoAEE/limparDadosDevolutiva': {
        return {
          ...draft,
          parecerCoordenacao: '',
          parecerPAAI: '',
        };
      }
      default:
        return draft;
    }
  });
}
