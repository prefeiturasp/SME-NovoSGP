import { store } from '~/redux';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import {
  setAuditoriaAnotacaoRecomendacao,
  setDadosAnotacoesRecomendacoes,
} from '~/redux/modulos/conselhoClasse/actions';

class ServicoSalvarConselhoClasse {
  salvarRecomendacoesAlunoFamilia = async () => {
    const { dispatch } = store;
    const state = store.getState();

    const { conselhoClasse } = state;

    const {
      dadosAnotacoesRecomendacoes,
      dadosAlunoObjectCard,
      anotacoesPedagogicas,
      recomendacaoAluno,
      recomendacaoFamilia,
    } = conselhoClasse;

    const params = {
      conselhoClasseId: dadosAnotacoesRecomendacoes.conselhoClasseId,
      fechamentoTurmaId: dadosAnotacoesRecomendacoes.fechamentoTurmaId,
      alunoCodigo: dadosAlunoObjectCard.codigoEOL,
      anotacoesPedagogicas,
      recomendacaoAluno,
      recomendacaoFamilia,
    };

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
      return true;
    }
    return false;
  };
}

export default new ServicoSalvarConselhoClasse();
