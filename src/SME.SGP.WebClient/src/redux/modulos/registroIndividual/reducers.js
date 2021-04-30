import produce from 'immer';

const inicial = {
  alunosRegistroIndividual: [],
  auditoriaNovoRegistroIndividual: null,
  componenteCurricularSelecionado: undefined,
  dadosAlunoObjectCard: {},
  dadosParaSalvarNovoRegistro: {},
  dadosPrincipaisRegistroIndividual: {},
  dadosRegistroAtual: {},
  desabilitarCampos: false,
  exibirLoaderConteudoRegistroAnteriores: false,
  exibirLoaderGeralRegistroAnteriores: false,
  exibirLoaderGeralRegistroIndividual: false,
  recolherRegistrosAnteriores: false,
  registroAnteriorEmEdicao: false,
  registroAnteriorId: {},
  registroIndividualEmEdicao: false,
  resetDataNovoRegistroIndividual: false,
  podeRealizarNovoRegistro: false,
  exibirModalImpressaoRegistroIndividual: false,
  dataInicioImpressaoRegistrosAnteriores: '',
  dataFimImpressaoRegistrosAnteriores: '',
  mostrarMensagemSemHistorico: false,
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
      case '@registroIndividual/limparDadosRegistroIndividual': {
        return {
          ...draft,
          auditoriaNovoRegistroIndividual: null,
          dadosParaSalvarNovoRegistro: {},
          dadosPrincipaisRegistroIndividual: {},
          dadosRegistroAtual: {},
          desabilitarCampos: false,
          exibirLoaderGeralRegistroIndividual: false,
          registroIndividualEmEdicao: false,
          resetDataNovoRegistroIndividual: true,
        };
      }
      case '@registroIndividual/setRegistroIndividualEmEdicao': {
        return {
          ...draft,
          registroIndividualEmEdicao: action.payload,
        };
      }
      case '@registroIndividual/setRegistroAnteriorEmEdicao': {
        return {
          ...draft,
          registroAnteriorEmEdicao: action.payload,
        };
      }
      case '@registroIndividual/setDadosPrincipaisRegistroIndividual': {
        return {
          ...draft,
          dadosPrincipaisRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/setExpandirLinha': {
        return {
          ...draft,
          expandirLinha: action.payload,
        };
      }
      case '@registroIndividual/setDesabilitarCampos': {
        return {
          ...draft,
          desabilitarCampos: action.payload,
        };
      }
      case '@registroIndividual/setExibirLoaderGeralRegistroIndividual': {
        return {
          ...draft,
          exibirLoaderGeralRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/setComponenteCurricularSelecionado': {
        return {
          ...draft,
          componenteCurricularSelecionado: action.payload,
        };
      }
      case '@registroIndividual/setDadosParaSalvarNovoRegistro': {
        return {
          ...draft,
          dadosParaSalvarNovoRegistro: action.payload,
        };
      }
      case '@registroIndividual/setAuditoriaNovoRegistro': {
        return {
          ...draft,
          auditoriaNovoRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/resetDataNovoRegistro': {
        return {
          ...draft,
          resetDataNovoRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/excluirRegistroAnteriorId': {
        const items = state.dadosPrincipaisRegistroIndividual.registrosIndividuais.items.filter(
          dados => dados.id !== action.payload
        );
        if (!items?.length) {
          return {
            ...draft,
            dadosPrincipaisRegistroIndividual: {},
            resetDataNovoRegistroIndividual: true,
          };
        }
        return {
          ...draft,
          dadosPrincipaisRegistroIndividual: {
            ...state.dadosPrincipaisRegistroIndividual,
            registrosIndividuais: {
              ...state.dadosPrincipaisRegistroIndividual.registrosIndividuais,
              items,
            },
          },
        };
      }
      case '@registroIndividual/alterarRegistroAnterior': {
        const items = state.dadosPrincipaisRegistroIndividual.registrosIndividuais.items.map(
          dados => {
            if (dados.id === action.payload.id) {
              return {
                ...dados,
                registro: action.payload.registro,
                auditoria: action.payload.auditoria,
              };
            }
            return dados;
          }
        );
        return {
          ...draft,
          dadosPrincipaisRegistroIndividual: {
            ...state.dadosPrincipaisRegistroIndividual,
            registrosIndividuais: {
              ...state.dadosPrincipaisRegistroIndividual.registrosIndividuais,
              items,
            },
          },
        };
      }
      case '@registroIndividual/setRegistroAnteriorId': {
        return {
          ...draft,
          registroAnteriorId: action.payload,
        };
      }
      case '@registroIndividual/atualizaDadosRegistroAtual': {
        return {
          ...draft,
          dadosRegistroAtual: {
            ...state.dadosRegistroAtual,
            ...action.payload,
          },
        };
      }
      case '@registroIndividual/atualizarMarcadorDiasSemRegistroExibir': {
        const alunos = state.alunosRegistroIndividual.map(aluno => {
          if (aluno.codigoEOL === action.payload) {
            return {
              ...aluno,
              marcadorDiasSemRegistroExibir: false,
              marcadorDiasSemRegistroTexto: null,
            };
          }
          return aluno;
        });
        return {
          ...draft,
          alunosRegistroIndividual: alunos,
        };
      }
      case '@registroIndividual/setExibirLoaderGeralRegistroAnteriores': {
        return {
          ...draft,
          exibirLoaderGeralRegistroAnteriores: action.payload,
        };
      }
      case '@registroIndividual/setExibirLoaderConteudoRegistroAnteriores': {
        return {
          ...draft,
          exibirLoaderConteudoRegistroAnteriores: action.payload,
        };
      }
      case '@registroIndividual/setRecolherRegistrosAnteriores': {
        return {
          ...draft,
          recolherRegistrosAnteriores: action.payload,
        };
      }
      case '@registroIndividual/setDadosRegistroAtual': {
        return {
          ...draft,
          dadosRegistroAtual: action.payload,
        };
      }
      case '@registroIndividual/setPodeRealizarNovoRegistro': {
        return {
          ...draft,
          podeRealizarNovoRegistro: action.payload,
        };
      }
      case '@registroIndividual/resetarDadosRegistroIndividual': {
        return {
          ...draft,
          registroIndividualEmEdicao: false,
          auditoriaNovoRegistro: null,
          dadosAlunoObjectCard: {},
          dadosRegistroAtual: {},
          dadosPrincipaisRegistroIndividual: {},
          resetDataNovoRegistroIndividual: true,
        };
      }
      case '@registroIndividual/setExibirModalImpressaoRegistroIndividual': {
        return {
          ...draft,
          exibirModalImpressaoRegistroIndividual: action.payload,
        };
      }
      case '@registroIndividual/setDataInicioImpressaoRegistrosAnteriores': {
        return {
          ...draft,
          dataInicioImpressaoRegistrosAnteriores: action.payload,
        };
      }
      case '@registroIndividual/setDataFimImpressaoRegistrosAnteriores': {
        return {
          ...draft,
          dataFimImpressaoRegistrosAnteriores: action.payload,
        };
      }
      case '@registroIndividual/setMostrarMensagemSemHistorico': {
        return {
          ...draft,
          mostrarMensagemSemHistorico: action.payload,
        };
      }
      default:
        return draft;
    }
  });
}
