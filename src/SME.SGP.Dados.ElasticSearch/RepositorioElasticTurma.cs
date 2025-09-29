using Microsoft.Extensions.Options;
using Nest;
using SME.SGP.Infra;
using SME.SGP.Infra.ElasticSearch;
using SME.SGP.Infra.ElasticSearch.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.ElasticSearch
{
    public class RepositorioElasticTurma : RepositorioElasticBase<DocumentoElasticTurma>, IRepositorioElasticTurma
    {
        public RepositorioElasticTurma(IElasticClient elasticClient, IServicoTelemetria servicoTelemetria, IOptions<ElasticOptions> elasticOptions)
            : base(elasticClient, servicoTelemetria, elasticOptions, IndicesElastic.INDICE_ALUNO_TURMA_DRE)
        {

        }

        public async Task<IEnumerable<AlunoNaTurmaElasticDTO>> ObterDadosAlunosDisciplinaPapPeloAnoLetivo(int anoLetivo)
        {
            QueryContainer query = new QueryContainerDescriptor<AlunoNaTurmaElasticDTO>()
                .Term(t => t
                    .Field(f => f.Ano)
                    .Value(anoLetivo));

            var resultado =
                await ObterListaAsync<AlunoNaTurmaElasticDTO>(
                    IndicesElastic.INDICE_ALUNO_TURMA_DRE,
                    _ => query, "Buscar alunos ativos na turma",
                    new { anoLetivo });

            return resultado;
        }
    }
}