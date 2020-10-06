import { store } from '~/redux';
import {
  limparDadosPlanoAnual,
  setErrosPlanoAnual,
  setExibirLoaderPlanoAnual,
  setExibirModalErrosPlanoAnual,
  setPlanoAnualEmEdicao,
  setTodosDadosBimestresPlanoAnual,
} from '~/redux/modulos/anual/actions';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import ServicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';

class ServicoSalvarPlanoAnual {
  perguntaDescartarRegistros = async () => {
    const { dispatch } = store;
    const state = store.getState();

    const { planoAnual } = state;

    const { planoAnualEmEdicao } = planoAnual;

    let continuar = true;
    if (planoAnualEmEdicao) {
      const descartar = await confirmar(
        'Atenção',
        '',
        'Os registros não foram salvos, deseja descartar os registros?'
      );
      if (descartar) {
        dispatch(limparDadosPlanoAnual());
        continuar = true;
      } else {
        continuar = false;
      }
    }

    return continuar;
  };

  campoInvalido = valorEditado => {
    const state = store.getState();
    const { planoAnual } = state;
    const { planoAnualEmEdicao } = planoAnual;

    let ehInvalido = false;
    if (planoAnualEmEdicao) {
      if (!valorEditado) {
        ehInvalido = true;
      }
    }

    return ehInvalido;
  };

  validarSalvarPlanoAnual = async () => {
    const { dispatch } = store;
    const state = store.getState();

    const { planoAnual, usuario } = state;
    const { turmaSelecionada } = usuario;

    const {
      dadosBimestresPlanoAnual,
      listaComponentesCurricularesPlanejamento,
      planoAnualEmEdicao,
      componenteCurricular,
    } = planoAnual;

    const listaObjetivosSemDados = () => {
      const camposInvalidos = [];

      dadosBimestresPlanoAnual.forEach(bimestreAlterado => {
        if (
          bimestreAlterado.componentes &&
          bimestreAlterado.componentes.length
        ) {
          const semObjetivos = bimestreAlterado.componentes.filter(
            item =>
              item.emEdicao &&
              item.objetivosAprendizagemId &&
              item.objetivosAprendizagemId.length === 0
          );
          if (semObjetivos && semObjetivos.length) {
            semObjetivos.forEach(componente => {
              const c = listaComponentesCurricularesPlanejamento.find(
                item =>
                  String(item.codigoComponenteCurricular) ===
                  String(componente.componenteCurricularId)
              );
              const msg = `${bimestreAlterado.bimestre}º Bimestre - ${c.nome}: Ao menos um objetivo de aprendizagem deve ser selecionado.`;
              camposInvalidos.push(msg);
            });
          }
        }
      });

      if (camposInvalidos.length) {
        dispatch(setErrosPlanoAnual(camposInvalidos));
        dispatch(setExibirModalErrosPlanoAnual(true));
        return false;
      }
      return true;
    };

    const campoDescricaoNaoInformado = () => {
      const camposInvalidos = [];

      dadosBimestresPlanoAnual.forEach(bimestreAlterado => {
        if (
          bimestreAlterado.componentes &&
          bimestreAlterado.componentes.length
        ) {
          const componestesSemDescricao = bimestreAlterado.componentes.filter(
            item => !item.descricao && item.emEdicao
          );
          if (componestesSemDescricao && componestesSemDescricao.length) {
            componestesSemDescricao.forEach(componente => {
              const c = listaComponentesCurricularesPlanejamento.find(
                item =>
                  String(item.codigoComponenteCurricular) ===
                  String(componente.componenteCurricularId)
              );
              const msg = `A descrição do planejamento de ${c.nome} do ${bimestreAlterado.bimestre}º Bimestre é obrigatória`;
              camposInvalidos.push(msg);
            });
          }
        }
      });

      if (camposInvalidos.length) {
        dispatch(setErrosPlanoAnual(camposInvalidos));
        dispatch(setExibirModalErrosPlanoAnual(true));
        return false;
      }
      return true;
    };

    const salvar = async () => {
      const listaNova = [...dadosBimestresPlanoAnual];
      const listaParaSalvar = listaNova
        .filter(a => a)
        .map(item => {
          return {
            periodoEscolarId: item.periodoEscolarId,
            componentes: item.componentes.filter(c => c.emEdicao),
          };
        });
      const params = {
        periodosEscolares: listaParaSalvar,
      };
      dispatch(setExibirLoaderPlanoAnual(true));
      const retorno = await ServicoPlanoAnual.salvarEditarPlanoAnual(
        turmaSelecionada.id,
        componenteCurricular.codigoComponenteCurricular,
        params
      ).catch(e => erros(e));

      if (retorno && retorno.status === 200) {
        dispatch(setPlanoAnualEmEdicao(false));

        const { periodosEscolares } = retorno.data;

        dadosBimestresPlanoAnual.forEach(bimestreParaSetarAuditoria => {
          const bimestreComNovaAuditoria = periodosEscolares.find(
            item =>
              item.periodoEscolarId ===
              bimestreParaSetarAuditoria.periodoEscolarId
          );
          if (bimestreComNovaAuditoria) {
            bimestreParaSetarAuditoria.componentes.forEach(compSetarAudi => {
              const componenteComNovaAuditoria = bimestreComNovaAuditoria.componentes.find(
                comp =>
                  String(comp.componenteCurricularId) ===
                  String(compSetarAudi.componenteCurricularId)
              );
              if (componenteComNovaAuditoria) {
                compSetarAudi.auditoria = componenteComNovaAuditoria.auditoria;
                compSetarAudi.emEdicao = false;
              }
            });
          }
        });

        const dadosParaAtualizarNoRedux = [];
        dadosBimestresPlanoAnual.forEach(a => {
          dadosParaAtualizarNoRedux[a.bimestre] = { ...a };
        });
        dispatch(setTodosDadosBimestresPlanoAnual(dadosParaAtualizarNoRedux));

        sucesso('Suas informações foram salvas com sucesso.');
        dispatch(setExibirLoaderPlanoAnual(false));
        ServicoPlanoAnual.obterPlanejamentoId();
        return true;
      }
      dispatch(setExibirLoaderPlanoAnual(false));
      return false;
    };
    if (planoAnualEmEdicao) {
      // Voltar para a tela e não executa a ação!

      if (componenteCurricular.possuiObjetivos) {
        const temListaObjetivosSemDados = !listaObjetivosSemDados();
        if (temListaObjetivosSemDados) {
          return false;
        }
      }

      const temCampoDescricaoNaoInformado = !campoDescricaoNaoInformado();
      if (temCampoDescricaoNaoInformado) {
        return false;
      }

      // Tenta salvar os registros se estão válidos e continuar para executação a ação!
      return salvar();
    }
    return true;
  };
}

export default new ServicoSalvarPlanoAnual();
