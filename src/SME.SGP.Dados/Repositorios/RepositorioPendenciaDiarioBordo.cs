using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaDiarioBordo : RepositorioBase<PendenciaDiarioBordo>, IRepositorioPendenciaDiarioBordo
    {
        public RepositorioPendenciaDiarioBordo(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task Excluir(long aulaId, long componenteCurricularId)
        {
            var sql = @"delete from pendencia_diario_bordo where aula_id = @aulaId and componente_curricular_id = @componenteCurricularId ";

            await database.Conexao.ExecuteScalarAsync(sql, new { aulaId, componenteCurricularId }, commandTimeout: 60);
        }

        public async Task ExcluirPorAulaId(long aulaId)
        {
            var sql = @"delete from pendencia_diario_bordo where aula_id = @aulaId";

            await database.Conexao.ExecuteScalarAsync(sql, new { aulaId }, commandTimeout: 60);
        }

    }
}
