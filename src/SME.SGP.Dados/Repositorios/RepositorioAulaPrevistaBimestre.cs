using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAulaPrevistaBimestre : RepositorioBase<AulaPrevistaBimestre>, IRepositorioAulaPrevistaBimestre
    {
        public RepositorioAulaPrevistaBimestre(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<AulaPrevistaBimestre>> ObterAulaPrevistaBimestrePorAulaPrevistaIdBimestre(long aulaPrevistaId, int[] bimestres)
        {
            var query = @"select * from aula_prevista_bimestre where aula_prevista_id = @aulaPrevistaId and bimestre = ANY(@bimestres)";

            return await database.Conexao.QueryAsync<AulaPrevistaBimestre>(query, new { aulaPrevistaId, bimestres });
        }

    }
}