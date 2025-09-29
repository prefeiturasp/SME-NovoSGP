using SME.Pedagogico.Interface;
using SME.SGP.Infra.ElasticSearch.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.ElasticSearch
{
    public interface IRepositorioElasticTurma : IRepositorioElasticBase<DocumentoElasticTurma>
    {
        Task<IEnumerable<AlunoNaTurmaElasticDTO>> ObterDadosAlunosDisciplinaPapPeloAnoLetivo(int anoLetivoInicial);
    }
}