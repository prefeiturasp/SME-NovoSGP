using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseNota : RepositorioBase<ConselhoClasseNota>, IRepositorioConselhoClasseNota
    {
        public RepositorioConselhoClasseNota(ISgpContext database) : base(database)
        {
        }

        public async Task<ConselhoClasseNota> ObterPorFiltrosAsync(string alunoCodigo, int bimestre, string componenteCurricularCodigo, string turmaCodigo)
        {
            var query = new StringBuilder();

            query.AppendLine("select * from conselho_classe_nota ccn");
            query.AppendLine("inner join conselho_classe_aluno cca");
            query.AppendLine("on ccn.conselho_classe_aluno_id = cca.id");
            query.AppendLine("inner join conselho_classe cc");
            query.AppendLine("on cca.conselho_classe_id = cc.id");
            query.AppendLine("inner join fechamento_turma ft");
            query.AppendLine("on cc.fechamento_turma_id = ft.id");

            if (bimestre > 0)
            {
                query.AppendLine("inner join periodo_escolar pe");
                query.AppendLine("on ft.periodo_escolar_id = pe.id");
            }

            query.AppendLine("where");
            query.AppendLine("ccn.componente_curricular_codigo = @componenteCurricularCodigo");
            query.AppendLine("and cca.aluno_codigo = @alunoCodigo");
            query.AppendLine("and ft.turma_id = @turmaCodigo");
            if (bimestre > 0)
                query.AppendLine("and pe.bimestre = @bimestre");
            else query.AppendLine("and ft.periodo_escolar_id is null");

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseNota>(query.ToString(), new { alunoCodigo, bimestre, componenteCurricularCodigo, turmaCodigo });
        }
    }
}