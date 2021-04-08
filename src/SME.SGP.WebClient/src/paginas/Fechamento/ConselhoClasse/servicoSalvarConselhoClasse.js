import { store } from '~/redux';
import { erros, sucesso, confirmar, erro } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import {
  setAuditoriaAnotacaoRecomendacao,
  setDadosPrincipaisConselhoClasse,
  setConselhoClasseEmEdicao,
  setExpandirLinha,
  setIdCamposNotasPosConselho,
  setNotaConceitoPosConselhoAtual,
  setGerandoParecerConclusivo,
  setExibirLoaderGeralConselhoClasse,
} from '~/redux/modulos/conselhoClasse/actions';
import notasConceitos from '~/dtos/notasConceitos';

class ServicoSalvarConselhoClasse {
  validarSalvarRecomendacoesAlunoFamilia = async (salvarSemValidar = false) => {
    const { dispatch } = store;
    const state = store.getState();

    const { conselhoClasse } = state;

    const {
      dadosPrincipaisConselhoClasse,
      dadosAlunoObjectCard,
      anotacoesPedagogicas,
      recomendacaoAluno,
      recomendacaoFamilia,
      conselhoClasseEmEdicao,
      desabilitarCampos,
    } = conselhoClasse;

    const perguntaDescartarRegistros = async () => {
      return confirmar(
        'Atenção',
        '',
        'Anotações e recomendações ainda não foram salvas, deseja descartar?'
      );
    };

    const salvar = async () => {
      const params = {
        conselhoClasseId: dadosPrincipaisConselhoClasse.conselhoClasseId,
        fechamentoTurmaId: dadosPrincipaisConselhoClasse.fechamentoTurmaId || 0,
        alunoCodigo: dadosAlunoObjectCard.codigoEOL,
        anotacoesPedagogicas,
        recomendacaoAluno,
        recomendacaoFamilia,
      };

      if (!recomendacaoAluno) {
        erro('É obrigatório informar Recomendações ao estudante');
        return false;
      }

      if (!recomendacaoFamilia) {
        erro('É obrigatório informar Recomendações a família ');
        return false;
      }
      dispatch(setExibirLoaderGeralConselhoClasse(true));
      const retorno = await ServicoConselhoClasse.salvarRecomendacoesAlunoFamilia(
        params
      )
        .finally(() => {
          dispatch(setExibirLoaderGeralConselhoClasse(false));
        })
        .catch(e => {
          dispatch(setExibirLoaderGeralConselhoClasse(false));
          erros(e);
        });

      if (retorno && retorno.status === 200) {
        if (!dadosPrincipaisConselhoClasse.conselhoClasseId) {
          dadosPrincipaisConselhoClasse.conselhoClasseId =
            retorno.data.conselhoClasseId;
          dispatch(
            setDadosPrincipaisConselhoClasse(dadosPrincipaisConselhoClasse)
          );
        }

        const auditoria = {
          criadoEm: retorno.data.criadoEm,
          criadoPor: retorno.data.criadoPor,
          criadoRF: retorno.data.criadoRF,
          alteradoEm: retorno.data.alteradoEm,
          alteradoPor: retorno.data.alteradoPor,
          alteradoRF: retorno.data.alteradoRF,
        };
        dispatch(setAuditoriaAnotacaoRecomendacao(auditoria));
        dispatch(setConselhoClasseEmEdicao(false));
        sucesso('Anotações e recomendações salvas com sucesso.');
        return true;
      }
      return false;
    };

    if (desabilitarCampos) {
      return true;
    }

    if (salvarSemValidar && conselhoClasseEmEdicao) {
      return salvar();
    }

    if (conselhoClasseEmEdicao) {
      const temRegistrosInvalidos = !recomendacaoAluno || !recomendacaoFamilia;

      let descartarRegistros = false;
      if (temRegistrosInvalidos) {
        descartarRegistros = await perguntaDescartarRegistros();
      }

      // Voltar para a tela continua e executa a ação!
      if (descartarRegistros) {
        dispatch(setConselhoClasseEmEdicao(false));
        return true;
      }

      // Voltar para a tela e não executa a ação!
      if (!descartarRegistros && temRegistrosInvalidos) {
        return false;
      }

      const perguntarParaSalvar = async () => {
        return confirmar(
          'Atenção',
          '',
          'Suas alterações não foram salvas, deseja salvar agora?'
        );
      };

      // Tenta salvar os registros se estão válidos e continuar para executação a ação!
      const perguntaAantesSalvar = await perguntarParaSalvar();
      if (perguntaAantesSalvar) return salvar();
    }
    return true;
  };

  salvarNotaPosConselho = async codigoTurma => {
    const { dispatch } = store;

    const state = store.getState();

    const { conselhoClasse } = state;

    const {
      dadosPrincipaisConselhoClasse,
      notaConceitoPosConselhoAtual,
      idCamposNotasPosConselho,
      desabilitarCampos,
      bimestreAtual,
    } = conselhoClasse;

    const {
      conselhoClasseId,
      fechamentoTurmaId,
      alunoCodigo,
      tipoNota,
    } = dadosPrincipaisConselhoClasse;

    const {
      justificativa,
      nota,
      conceito,
      codigoComponenteCurricular,
      idCampo,
    } = notaConceitoPosConselhoAtual;

    const ehNota = Number(tipoNota) === notasConceitos.Notas;

    const limparDadosNotaPosConselhoJustificativa = () => {
      dispatch(setExpandirLinha([]));
      dispatch(setNotaConceitoPosConselhoAtual({}));
    };

    const gerarParecerConclusivo = async () => {
      dispatch(setGerandoParecerConclusivo(true));
      const retorno = await ServicoConselhoClasse.gerarParecerConclusivo(
        conselhoClasseId,
        fechamentoTurmaId,
        alunoCodigo
      ).catch(e => erros(e));
      if (retorno && retorno.data) {
        ServicoConselhoClasse.setarParecerConclusivo(retorno.data);
      }
      dispatch(setGerandoParecerConclusivo(false));
    };

    if (desabilitarCampos) {
      return false;
    }

    if (!justificativa) {
      erro(
        `É obrigatório informar justificativa de ${
          ehNota ? 'nota' : 'conceito'
        } pós-conselho`
      );
      return false;
    }

    if ((nota === null || typeof nota === 'undefined') && !conceito) {
      erro(
        `É obrigatório informar ${ehNota ? 'nota' : 'conceito'} pós-conselho`
      );
      return false;
    }

    const notaDto = {
      justificativa,
      nota: ehNota ? nota : '',
      conceito: !ehNota ? conceito : '',
      codigoComponenteCurricular,
    };

    const retorno = await ServicoConselhoClasse.salvarNotaPosConselho(
      conselhoClasseId,
      fechamentoTurmaId,
      alunoCodigo,
      notaDto,
      codigoTurma,
      bimestreAtual.valor
    ).catch(e => erros(e));

    if (retorno && retorno.status === 200) {
      dadosPrincipaisConselhoClasse.conselhoClasseId =
        retorno.data.conselhoClasseId;
      dadosPrincipaisConselhoClasse.fechamentoTurmaId =
        retorno.data.fechamentoTurmaId;
      dispatch(setDadosPrincipaisConselhoClasse(dadosPrincipaisConselhoClasse));

      const { auditoria } = retorno.data;

      const temJustificativasDto = idCamposNotasPosConselho;
      temJustificativasDto[idCampo] = auditoria.id;
      dispatch(setIdCamposNotasPosConselho(temJustificativasDto));

      limparDadosNotaPosConselhoJustificativa();

      sucesso(
        `${ehNota ? 'Nota' : 'Conceito'} pós-conselho ${
          ehNota ? 'salva' : 'salvo'
        } com sucesso`
      );

      if (bimestreAtual && bimestreAtual.valor === 'final') {
        gerarParecerConclusivo();
      }

      return true;
    }
    return false;
  };

  validarNotaPosConselho = async () => {
    const { dispatch } = store;

    const limparDadosNotaPosConselhoJustificativa = () => {
      dispatch(setExpandirLinha([]));
      dispatch(setNotaConceitoPosConselhoAtual({}));
    };

    const state = store.getState();

    const { conselhoClasse } = state;

    const {
      notaConceitoPosConselhoAtual,
      dadosPrincipaisConselhoClasse,
      desabilitarCampos,
    } = conselhoClasse;

    if (desabilitarCampos) {
      return true;
    }

    const { tipoNota } = dadosPrincipaisConselhoClasse;

    const { ehEdicao } = notaConceitoPosConselhoAtual;

    const ehNota = Number(tipoNota) === notasConceitos.Notas;

    const perguntaDescartarRegistros = async () => {
      return confirmar(
        'Atenção',
        '',
        `${ehNota ? 'Nota' : 'Conceito'} pós-conselho não foi ${
          ehNota ? 'salva' : 'salvo'
        }, deseja descartar?`
      );
    };

    if (ehEdicao) {
      const descartarRegistros = await perguntaDescartarRegistros();

      // Voltar para a tela continua e executa a ação!
      if (descartarRegistros) {
        limparDadosNotaPosConselhoJustificativa();
        return true;
      }

      // Voltar para a tela e não executa a ação!
      return false;
    }

    return true;
  };
}

export default new ServicoSalvarConselhoClasse();
