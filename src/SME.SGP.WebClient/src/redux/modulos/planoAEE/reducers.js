import produce from 'immer';

const inicial = {
  planoAEEDados: {},
  desabilitarCamposPlanoAEE: false,
  planoAEEEmEdicao: false,
  exibirLoaderPlanoAEE: false,
  planoAEESituacaoEncaminhamentoAEE: {},
  planoAEEDadosSecoesPorEtapa: [],
  exibirModalErrosPlano: false,
  reestruturacaoDados: [],
};

export default function PlanoAEE(state = inicial, action) {
  return produce(state, draft => {
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
      case '@planoAEE/setReestruturacaoDados': {
        return {
          ...draft,
          reestruturacaoDados: action.payload,
        };
      }
      case '@planoAEE/setAlteracaoDados': {
        let dadosParaSalvar = draft.reestruturacaoDados.map(item => {
          if (item.id === action.payload?.id) {
            return {
              ...item,
              ...action.payload,
              data: item.data,
            };
          }
          return item;
        });

        if (action.payload.adicionando) {
          dadosParaSalvar = [...draft.reestruturacaoDados, action.payload];
        }
        return {
          ...draft,
          reestruturacaoDados: dadosParaSalvar,
        };
      }
      default:
        return draft;
    }
  });
}
