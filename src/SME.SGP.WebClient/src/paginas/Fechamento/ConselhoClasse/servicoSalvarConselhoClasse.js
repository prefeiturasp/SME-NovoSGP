import { store } from '~/redux';
import { erros, sucesso, confirmar, erro } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import {
  setAuditoriaAnotacaoRecomendacao,
  setDadosAnotacoesRecomendacoes,
  setConselhoClasseEmEdicao,
} from '~/redux/modulos/conselhoClasse/actions';

class ServicoSalvarConselhoClasse {
  validarSalvarRecomendacoesAlunoFamilia = async (salvarSemValidar = false) => {
    const { dispatch } = store;
    const state = store.getState();

    const { conselhoClasse } = state;

    const {
      dadosAnotacoesRecomendacoes,
      dadosAlunoObjectCard,
      anotacoesPedagogicas,
      recomendacaoAluno,
      recomendacaoFamilia,
      conselhoClasseEmEdicao,
    } = conselhoClasse;

    const perguntaDescartarRegistros = async () => {
      return confirmar(
        'Atenção',
        '',
        'Os registros deste aluno ainda não foram salvos, deseja descartar os registros?'
      );
    };

    const salvar = async () => {
      const params = {
        conselhoClasseId: dadosAnotacoesRecomendacoes.conselhoClasseId,
        fechamentoTurmaId: dadosAnotacoesRecomendacoes.fechamentoTurmaId,
        alunoCodigo: dadosAlunoObjectCard.codigoEOL,
        anotacoesPedagogicas,
        recomendacaoAluno,
        recomendacaoFamilia,
      };

      if (!recomendacaoAluno) {
        erro('É obrigatório informar Recomendações ao aluno');
        return false;
      }

      if (!recomendacaoFamilia) {
        erro('É obrigatório informar Recomendações a família ');
        return false;
      }

      const retorno = await ServicoConselhoClasse.salvarRecomendacoesAlunoFamilia(
        params
      ).catch(e => erros(e));

      if (retorno && retorno.status === 200) {
        if (!dadosAnotacoesRecomendacoes.conselhoClasseId) {
          dadosAnotacoesRecomendacoes.conselhoClasseId =
            retorno.data.conselhoClasseId;
          dispatch(setDadosAnotacoesRecomendacoes(dadosAnotacoesRecomendacoes));
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
        sucesso('Suas informações foram salvas com sucesso.');
        return true;
      }
      return false;
    };

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

      // Tenta salvar os registros se estão válidos e continuar para executação a ação!
      return salvar();
    }
    return true;
  };
}

export default new ServicoSalvarConselhoClasse();
