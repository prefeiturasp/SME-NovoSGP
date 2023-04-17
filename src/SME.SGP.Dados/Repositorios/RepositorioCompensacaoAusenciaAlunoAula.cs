using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusenciaAlunoAula : RepositorioBase<CompensacaoAusenciaAlunoAula>, IRepositorioCompensacaoAusenciaAlunoAula
    {
        public RepositorioCompensacaoAusenciaAlunoAula(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
        
        public async Task<bool> ExclusaoLogicaCompensacaoAusenciaAlunoAulaPorIds(long[] ids)
        {
            var sql = $@"update compensacao_ausencia_aluno_aula set excluido = true where id = ANY(@ids)";

            return await database.Conexao.ExecuteScalarAsync<bool>(sql, new { ids});
        }
    }
}
