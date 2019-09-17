using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAnual : RepositorioBase<PlanoAnual>, IRepositorioPlanoAnual
    {
        public RepositorioPlanoAnual(ISgpContext conexao) : base(conexao)
        {
        }

        public PlanoAnualCompletoDto ObterPlanoAnualCompletoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select");
            query.AppendLine("	pa.ano as AnoLetivo, pa.*,");
            query.AppendLine("	string_agg(distinct cast(oap.objetivo_aprendizagem_jurema_id as text), ',') as ObjetivosAprendizagemPlano");
            query.AppendLine("from");
            query.AppendLine("	plano_anual pa");
            query.AppendLine("left join objetivo_aprendizagem_plano oap on");
            query.AppendLine("	pa.id = oap.plano_id");
            query.AppendLine("where");
            query.AppendLine("	pa.ano = @ano");
            query.AppendLine("	and pa.bimestre = @bimestre");
            query.AppendLine("	and pa.escola_id = @escolaId");
            query.AppendLine("	and pa.turma_id = @turmaId");
            query.AppendLine("group by");
            query.AppendLine("	pa.id");

            return database.Conexao.Query<PlanoAnualCompletoDto>(query.ToString(), new { ano, escolaId, turmaId, bimestre }).SingleOrDefault();
        }

        public PlanoAnual ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre)
        {
            return database.Conexao.Query<PlanoAnual>("select id, escola_id, turma_id, ano, bimestre, descricao, criado_em, alterado_em, criado_por, alterado_por, criado_rf, alterado_rf from plano_anual where ano = @ano and escola_id = @escolaId and bimestre = @bimestre and turma_id = @turmaId", new { ano, escolaId, turmaId, bimestre }).SingleOrDefault();
        }

        public bool ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(int ano, string escolaId, long turmaId, int bimestre)
        {
            return database.Conexao.Query<bool>("select 1 from plano_anual where ano = @ano and escola_id = @escolaId and bimestre = @bimestre and turma_id = @turmaId", new { ano, escolaId, turmaId, bimestre }).SingleOrDefault();
        }
    }
}