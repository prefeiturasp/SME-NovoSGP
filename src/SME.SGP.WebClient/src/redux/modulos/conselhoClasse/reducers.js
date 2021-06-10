import produce from 'immer';

const inicial = {
  listaTiposConceitos: [],
  dadosAlunoObjectCard: {},
  alunosConselhoClasse: [],
  recomendacaoAluno: '',
  recomendacaoFamilia: '',
  anotacoesPedagogicas: '',
  anotacoesAluno: [],
  bimestreAtual: { valor: '' },
  conselhoClasseEmEdicao: false,
  dadosPrincipaisConselhoClasse: {},
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
  exibirModalImpressaoConselhoClasse: false,
  dadosBimestresConselhoClasse: [],
  exibirLoaderGeralConselhoClasse: false,
};

export default function ConselhoClasse(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@conselhoClasse/setDadosAlunoObjectCard': {
        return {
          ...draft,
          dadosAlunoObjectCard: action.payload,
        };
      }
      case '@conselhoClasse/setAlunosConselhoClasse': {
        return {
          ...draft,
          alunosConselhoClasse: action.payload,
        };
      }
      case '@conselhoClasse/setRecomendacaoAluno': {
        return {
          ...draft,
          recomendacaoAluno: action.payload,
        };
      }
      case '@conselhoClasse/setRecomendacaoFamilia': {
        return {
          ...draft,
          recomendacaoFamilia: action.payload,
        };
      }
      case '@conselhoClasse/setAnotacoesPedagogicas': {
        return {
          ...draft,
          anotacoesPedagogicas: action.payload,
        };
      }
      case '@conselhoClasse/setAnotacoesAluno': {
        return {
          ...draft,
          anotacoesAluno: action.payload,
        };
      }
      case '@conselhoClasse/setBimestreAtual': {
        return {
          ...draft,
          bimestreAtual: { valor: action.payload },
        };
      }
      case '@conselhoClasse/limparDadosConselhoClasse': {
        return {
          ...draft,
          dadosAlunoObjectCard: {},
          recomendacaoAluno: '',
          recomendacaoFamilia: '',
          anotacoesPedagogicas: '',
          anotacoesAluno: [],
          bimestreAtual: { valor: draft.bimestreAtual.valor },
          conselhoClasseEmEdicao: false,
          dadosPrincipaisConselhoClasse: {},
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
          exibirModalImpressaoConselhoClasse: false,
          exibirLoaderGeralConselhoClasse: false,
        };
      }
      case '@conselhoClasse/setConselhoClasseEmEdicao': {
        return {
          ...draft,
          conselhoClasseEmEdicao: action.payload,
        };
      }
      case '@conselhoClasse/setDadosPrincipaisConselhoClasse': {
        return {
          ...draft,
          dadosPrincipaisConselhoClasse: action.payload,
        };
      }
      case '@conselhoClasse/setAuditoriaAnotacaoRecomendacao': {
        return {
          ...draft,
          auditoriaAnotacaoRecomendacao: action.payload,
        };
      }
      case '@conselhoClasse/setDentroPeriodo': {
        return {
          ...draft,
          dentroPeriodo: action.payload,
        };
      }
      case '@conselhoClasse/setFechamentoPeriodoInicioFim': {
        return {
          ...draft,
          fechamentoPeriodoInicioFim: action.payload,
        };
      }
      case '@conselhoClasse/setListaTiposConceitos': {
        return {
          ...draft,
          listaTiposConceitos: action.payload,
        };
      }

      case '@conselhoClasse/setNotasJustificativas':
        draft.notasJustificativas.componentes = [
          ...(action.payload.componentes || []),
        ];
        draft.notasJustificativas.componentesRegencia = [
          ...(action.payload.componentesRegencia || []),
        ];
        break;

      case '@conselhoClasse/setDadosListasNotasConceitos': {
        return {
          ...draft,
          dadosListasNotasConceitos: action.payload,
        };
      }

      case '@conselhoClasse/setExpandirLinha': {
        return {
          ...draft,
          expandirLinha: action.payload,
        };
      }
      case '@conselhoClasse/setNotaConceitoPosConselhoAtual': {
        return {
          ...draft,
          notaConceitoPosConselhoAtual: action.payload,
        };
      }
      case '@conselhoClasse/setIdCamposNotasPosConselho': {
        return {
          ...draft,
          idCamposNotasPosConselho: action.payload,
        };
      }
      case '@conselhoClasse/setMarcadorParecerConclusivo': {
        return {
          ...draft,
          marcadorParecerConclusivo: action.payload,
        };
      }
      case '@conselhoClasse/setGerandoParecerConclusivo': {
        return {
          ...draft,
          gerandoParecerConclusivo: action.payload,
        };
      }
      case '@conselhoClasse/setDesabilitarCampos': {
        return {
          ...draft,
          desabilitarCampos: action.payload,
        };
      }
      case '@conselhoClasse/setPodeEditarNota': {
        return {
          ...draft,
          podeEditarNota: action.payload,
        };
      }
      case '@conselhoClasse/setSalvouJustificativa': {
        return {
          ...draft,
          salvouJustificativa: action.payload,
        };
      }
      case '@conselhoClasse/setExibirModalImpressaoConselhoClasse': {
        return {
          ...draft,
          exibirModalImpressaoConselhoClasse: action.payload,
        };
      }
      case '@conselhoClasse/setDadosBimestresConselhoClasse': {
        return {
          ...draft,
          dadosBimestresConselhoClasse: action.payload,
        };
      }
      case '@conselhoClasse/setJustificativaAtual': {
        draft.notaConceitoPosConselhoAtual.justificativa = action.payload;
        break;
      }

      default:
        return draft;
    }
  });
}
