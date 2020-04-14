using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasse : RepositorioBase<ConselhoClasse>, IRepositorioConselhoClasse
    {
        public RepositorioConselhoClasse(ISgpContext database) : base(database)
        {
        }

        public async Task<ConselhoClasse> ObterPorTurmaEPeriodoAsync(long turmaId, long? periodoEscolarId = null)
        {
            var query = new StringBuilder(@"select c.*
                            from conselho_classe c
                           where c.turma_id = @turmaId ");

            if (periodoEscolarId.HasValue)
                query.AppendLine(" and c.periodo_escolar_id = @periodoEscolarId");
            else
                query.AppendLine(" and c.periodo_escolar_id is null");

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasse>(query.ToString(), new { turmaId, periodoEscolarId });
        }

        //public async Task<ConselhoClasse> ObterPorTurmaBimestreAsync(int bimestre, long fechamentoTurmaId)
        //{
        //    var query = @"
        //                select
        //                 *
        //                from
        //                 conselho_classe cc
        //                inner join periodo_escolar pe on
        //                 ft.periodo_escolar_id = pe.id
        //                where
        //                 cc.fechamento_turma_id = @fechamentoTurmaId
        //                 and pe.bimestre = @bimestre";

        //    return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasse>(query, new { bimestre, fechamentoTurmaId });
        //}
    }
}