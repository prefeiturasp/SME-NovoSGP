using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAluno : RepositorioBase<ConselhoClasseAluno>, IRepositorioConselhoClasseAluno
    {
        public RepositorioConselhoClasseAluno(ISgpContext database) : base(database)
        {
        }

        public async Task<ConselhoClasseAluno> ObterPorFiltrosAsync(string codigoTurma, string codigoAluno, int bimestre, bool EhFinal)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select cca.* from fechamento_turma ft");
            query.AppendLine("inner");
            query.AppendLine("join conselho_classe cc");
            query.AppendLine("on cc.fechamento_turma_id = ft.id");
            query.AppendLine("inner");
            query.AppendLine("join conselho_classe_aluno cca");
            query.AppendLine("on cca.conselho_classe_id = cc.id");
            query.AppendLine("inner");
            query.AppendLine("join turma t");
            query.AppendLine("on t.id = ft.turma_id");

            if (!EhFinal)
            {
                query.AppendLine("inner join periodo_escolar p");
                query.AppendLine("on ft.periodo_escolar_id = p.id");
            }

            query.AppendLine("where t.turma_id = @codigoTurma");
            query.AppendLine("and cca.aluno_codigo = @codigoAluno");

            if (EhFinal)
                query.AppendLine("and ft.periodo_escolar_id is null");
            else
                query.AppendLine("and p.bimestre = @bimestre");

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseAluno>(query.ToString(), new { codigoTurma, codigoAluno, bimestre });
        }
    }
}