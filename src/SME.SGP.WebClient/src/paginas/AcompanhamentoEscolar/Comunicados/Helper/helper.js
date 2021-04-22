import AbrangenciaServico from '~/servicos/Abrangencia';
import { erros } from '~/servicos/alertas';
import ServicoFiltroRelatorio from '~/servicos/Paginas/FiltroRelatorio/ServicoFiltroRelatorio';
import ServicoComunicados from '~/servicos/Paginas/AcompanhamentoEscolar/Comunicados/ServicoComunicados';

const ID_TODOS = '-99';

class FiltroHelper {
  async mapearParaSelect(array, todas, ue) {
    return array.map(x => {
      var id = x.codigo == ID_TODOS ? todas : x.codigo;
      var nome = x.codigo == ID_TODOS ? x.nomeSimples : x.nome;
      return { id, nome: ue ? nome : x.nome, tipoEscola : x.tipoEscola };
    });
  }

  async ObterAnoLetivo(modalidade) {
    try {
      const anosLetivos = await AbrangenciaServico.buscarTodosAnosLetivos();

      const dados = anosLetivos.data.map(x => {
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

      const dados = await this.mapearParaSelect(retorno.data, ID_TODOS);

      return dados;
    } catch (error) {
      erros('Não foi possivel obter as Dre');
      return [];
    }
  }

  async ObterUes(dre) {
    try {
      const retorno = await ServicoFiltroRelatorio.obterUes(dre, true);

      const dados = await this.mapearParaSelect(retorno.data, ID_TODOS, true);

      return dados;
    } catch (error) {
      erros('Não foi possivel obter as Ues');
      return [];
    }
  }

  async ObterModalidades(ue, anoLetivo, consideraHistorico) {
    try {
      const retorno = await ServicoFiltroRelatorio.obterModalidades(
        ue,
        anoLetivo,
        consideraHistorico
      );

      const dados = retorno.data.map(x => {
        return { id: x.valor, nome: x.descricao };
      });

      if (dados.length > 1) dados.unshift({ id: ID_TODOS, nome: 'Todas' });

      return dados;
    } catch (error) {
      erros('Não foi possivel obter as Modalidades');
      return [];
    }
  }

  async obterModalidadesAnoLetivo(ue, anoLetivo, consideraNovasModalidades) {
    try {
      const retorno = await ServicoFiltroRelatorio.obterModalidadesAnoLetivo(
        ue,
        anoLetivo,
        consideraNovasModalidades
      );

      const dados = retorno.data.map(x => {
        return { id: x.valor, nome: x.descricao };
      });

      if (dados.length > 1) dados.unshift({ id: ID_TODOS, nome: 'Todas' });

      return dados;
    } catch (error) {
      erros('Não foi possivel obter as Modalidades');
      return [];
    }
  }

  async obterModalidadesAnoLetivo(ue, anoLetivo) {
    try {
      const retorno = await ServicoFiltroRelatorio.obterModalidadesAnoLetivo(
        ue,
        anoLetivo,
        true
      );

      const dados = retorno.data.map(x => {
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
      const retorno = await ServicoFiltroRelatorio.obterTurmasPorCodigoUeModalidadeSemestre(
        anoLetivo,
        codigoUe,
        modalidade,
        semestre
      );

      const dados = retorno.data.map(x => {
        return { id: x.valor, nome: x.descricao };
      });

      dados.unshift({ id: ID_TODOS, nome: 'Todas' });

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

  async obterAnosPorModalidade(modalidade, codigoUe) {
    try {
      const response = await ServicoComunicados.buscarAnosPorModalidade(
        modalidade,
        codigoUe
      );
      const dados = response.data;

      if (dados && dados.length == 0) {
        dados.unshift({
          modalidade: +modalidade,
          ano: 'Todos',
        });
      }

      return dados;
    } catch (error) {
      erros('Não foi possivel obter anos de modalidade');
      return [];
    }
  }

  async obterTurmasEspecificas(
    codigoUe,
    anoLetivo,
    semestre,
    modalidade,
    anosEscolares
  ) {
    try {
      const response = await ServicoFiltroRelatorio.obterTurmasEspecificas(
        codigoUe,
        anoLetivo,
        semestre,
        modalidade,
        anosEscolares
      ).catch(e => {
        erros(e);
      });
      return response.data;
    } catch (error) {
      erros('Não foi possivel obter as obterTurmasEspecificas');
      return [];
    }
  }
}

export default new FiltroHelper();
