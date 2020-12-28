import produce from 'immer';

const inicial = {
  listaTiposConceitos: [],
  dadosAlunoObjectCard: {},
  alunosRegistroIndividual: [],
  recomendacaoAluno: '',
  recomendacaoFamilia: '',
  anotacoesPedagogicas: '',
  anotacoesAluno: [],
  bimestreAtual: { valor: '' },
  registroIndividualEmEdicao: false,
  dadosPrincipaisRegistroIndividual: {},
  auditoriaAnotacaoRecomendacao: null,
  fechamentoPeriodoInicioFim: {},
  notasJustificativas: { componentes: [], componentesRegencia: [] },
  expandirLinha: [],
  dadosListasNotasConceitos: {},
  notaConceitoPosConselhoAtual: {},
  idCamposNotasPosConselho: {},
  marcadorParecerConclusivo: {},
  gerandoParecerConclusivo: false,
  desabilitarCampos: false,
  podeEditarNota: false,
  salvouJustificativa: false,
  exibirModalImpressaoRegistroIndividual: false,
  dadosBimestresRegistroIndividual: [],
  exibirLoaderGeralRegistroIndividual: false,
};

export default function RegistroIndividual(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@registroIndividual/setDadosAlunoObjectCard': {
        return {
          ...draft,
          dadosAlunoObjectCard: action.payload,
        };
      }
      case '@registroIndividual/setAlunosRegistroIndividual': {
        return {
          ...draft,
          alunosRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/setRecomendacaoAluno': {
        return {
          ...draft,
          recomendacaoAluno: action.payload,
        };
      }
      case '@registroIndividual/setRecomendacaoFamilia': {
        return {
          ...draft,
          recomendacaoFamilia: action.payload,
        };
      }
      case '@registroIndividual/setAnotacoesPedagogicas': {
        return {
          ...draft,
          anotacoesPedagogicas: action.payload,
        };
      }
      case '@registroIndividual/setAnotacoesAluno': {
        return {
          ...draft,
          anotacoesAluno: action.payload,
        };
      }
      case '@registroIndividual/setBimestreAtual': {
        return {
          ...draft,
          bimestreAtual: { valor: action.payload },
        };
      }
      case '@registroIndividual/limparDadosRegistroIndividual': {
        return {
          ...draft,
          dadosAlunoObjectCard: {},
          recomendacaoAluno: '',
          recomendacaoFamilia: '',
          anotacoesPedagogicas: '',
          anotacoesAluno: [],
          bimestreAtual: { valor: draft.bimestreAtual.valor },
          registroIndividualEmEdicao: false,
          dadosPrincipaisRegistroIndividual: {},
          auditoriaAnotacaoRecomendacao: null,
          dentroPeriodo: true,
          fechamentoPeriodoInicioFim: {},
          notasJustificativas: { componentes: [], componentesRegencia: [] },
          expandirLinha: [],
          dadosListasNotasConceitos: [],
          notaConceitoPosConselhoAtual: {},
          idCamposNotasPosConselho: {},
          marcadorParecerConclusivo: {},
          gerandoParecerConclusivo: false,
          desabilitarCampos: false,
          exibirModalImpressaoRegistroIndividual: false,
          exibirLoaderGeralRegistroIndividual: false,
        };
      }
      case '@registroIndividual/setRegistroIndividualEmEdicao': {
        return {
          ...draft,
          registroIndividualEmEdicao: action.payload,
        };
      }
      case '@registroIndividual/setDadosPrincipaisRegistroIndividual': {
        return {
          ...draft,
          dadosPrincipaisRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/setAuditoriaAnotacaoRecomendacao': {
        return {
          ...draft,
          auditoriaAnotacaoRecomendacao: action.payload,
        };
      }
      case '@registroIndividual/setDentroPeriodo': {
        return {
          ...draft,
          dentroPeriodo: action.payload,
        };
      }
      case '@registroIndividual/setFechamentoPeriodoInicioFim': {
        return {
          ...draft,
          fechamentoPeriodoInicioFim: action.payload,
        };
      }
      case '@registroIndividual/setListaTiposConceitos': {
        return {
          ...draft,
          listaTiposConceitos: action.payload,
        };
      }

      case '@registroIndividual/setNotasJustificativas':
        draft.notasJustificativas.componentes = [
          ...(action.payload.componentes || []),
        ];
        draft.notasJustificativas.componentesRegencia = [
          ...(action.payload.componentesRegencia || []),
        ];
        break;

      case '@registroIndividual/setDadosListasNotasConceitos': {
        return {
          ...draft,
          dadosListasNotasConceitos: action.payload,
        };
      }

      case '@registroIndividual/setExpandirLinha': {
        return {
          ...draft,
          expandirLinha: action.payload,
        };
      }
      case '@registroIndividual/setNotaConceitoPosConselhoAtual': {
        return {
          ...draft,
          notaConceitoPosConselhoAtual: action.payload,
        };
      }
      case '@registroIndividual/setIdCamposNotasPosConselho': {
        return {
          ...draft,
          idCamposNotasPosConselho: action.payload,
        };
      }
      case '@registroIndividual/setMarcadorParecerConclusivo': {
        return {
          ...draft,
          marcadorParecerConclusivo: action.payload,
        };
      }
      case '@registroIndividual/setGerandoParecerConclusivo': {
        return {
          ...draft,
          gerandoParecerConclusivo: action.payload,
        };
      }
      case '@registroIndividual/setDesabilitarCampos': {
        return {
          ...draft,
          desabilitarCampos: action.payload,
        };
      }
      case '@registroIndividual/setPodeEditarNota': {
        return {
          ...draft,
          podeEditarNota: action.payload,
        };
      }
      case '@registroIndividual/setSalvouJustificativa': {
        return {
          ...draft,
          salvouJustificativa: action.payload,
        };
      }
      case '@registroIndividual/setExibirModalImpressaoRegistroIndividual': {
        return {
          ...draft,
          exibirModalImpressaoRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/setDadosBimestresRegistroIndividual': {
        return {
          ...draft,
          dadosBimestresRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/setExibirLoaderGeralRegistroIndividual': {
        return {
          ...draft,
          exibirLoaderGeralRegistroIndividual: action.payload,
        };
      }

      default:
        return draft;
    }
  });
}
