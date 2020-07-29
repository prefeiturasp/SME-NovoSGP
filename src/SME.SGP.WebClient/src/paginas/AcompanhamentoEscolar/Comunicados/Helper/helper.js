import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros } from '~/servicos/alertas';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import ServicoComunicados from '~/servicos/Paginas/AcompanhamentoEscolar/Comunicados/ServicoComunicados';

class FiltroHelper {
  async mapearParaSelect(array, todas, ue) {
    return array.map(x => {
      var id = x.codigo == '-99' ? todas : x.codigo;
      var nome = x.codigo == '-99' ? x.nomeSimples : x.nome;
      return { id, nome: ue ? nome : x.nome };
    });
  }

  async ObterAnoLetivo() {
    try {
      var anosLetivos = await AbrangenciaServico.buscarTodosAnosLetivos();

      var dados = anosLetivos.data.map(x => {
        return { id: x, nome: x };
      });

      return dados;
    } catch (error) {
      erros('Não foi possivel obter os anos Letivos');
      return [];
    }
  }

  async ObterDres() {
    try {
      const retorno = await ServicoFiltroRelatorio.obterDres().catch(e => {
        erros(e);
      });

      var dados = await this.mapearParaSelect(retorno.data, 'todas');

      return dados;
    } catch (error) {
      erros('Não foi possivel obter as Dre');
      return [];
    }
  }

  async ObterUes(dre) {
    try {
      const retorno = await ServicoFiltroRelatorio.obterUes(dre);

      var dados = await this.mapearParaSelect(retorno.data, 'todas', true);

      return dados;
    } catch (error) {
      erros('Não foi possivel obter as Ues');
      return [];
    }
  }

  async ObterModalidades(ue) {
    try {
      const retorno = await ServicoFiltroRelatorio.obterModalidades(ue);

      var dados = retorno.data.map(x => {
        return { id: x.valor, nome: x.descricao };
      });

      if (dados.length > 1) dados.unshift({ id: '-99', nome: 'Todas' });

      return dados;
    } catch (error) {
      erros('Não foi possivel obter as Modalidades');
      return [];
    }
  }

  async ObterTurmas(anoLetivo, codigoUe, modalidade, semestre) {
    try {
      var retorno = await ServicoFiltroRelatorio.obterTurmasPorCodigoUeModalidadeSemestre(
        anoLetivo,
        codigoUe,
        modalidade,
        semestre
      );

      var dados = retorno.data.map(x => {
        return { id: x.valor, nome: x.descricao };
      });

      dados.unshift({ id: '-99', nome: 'Todas' });

      return dados;
    } catch (error) {
      erros('Não foi possivel obter as Turmas');
      return [];
    }
  }

  async ObterGruposIdPorModalidade(modalidade) {
    try {
      const dados = await ServicoComunicados.obterGruposPorModalidade(
        modalidade
      );

      return dados.map(x => `${x}`);
    } catch (error) {
      erros(`Não foi possivel obter os grupos da modalidade ${modalidade}`);
    }
  }

  async ObterAlunos(codigoTurma, anoLetivo) {
    try {
      const dados = await ServicoComunicados.obterAlunos(
        codigoTurma,
        anoLetivo
      );

      return dados.map(x => {
        return {
          codigoAluno: x.codigoAluno,
          nomeAluno: x.nomeSocialAluno || x.nomeAluno,
          numeroAlunoChamada: x.numeroAlunoChamada,
          selecionado: false,
        };
      });
    } catch (error) {
      erros(
        `Não foi possivel obter os alunos da turma ${codigoTurma} e ano letivo ${anoLetivo}`
      );
      return [];
    }
  }
}

export default new FiltroHelper();
