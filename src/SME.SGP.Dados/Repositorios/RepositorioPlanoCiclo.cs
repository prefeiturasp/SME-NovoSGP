using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoCiclo : RepositorioBase<PlanoCiclo>, IRepositorioPlanoCiclo
    {
        public RepositorioPlanoCiclo(ISgpContext conexao) : base(conexao)
        {
        }

        public PlanoCicloCompletoDto ObterPlanoCicloComMatrizesEObjetivos(int ano, long cicloId, string escolaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("	pc.id,");
            query.AppendLine("	pc.descricao,");
            query.AppendLine("	pc.alterado_em as alteradoem,");
            query.AppendLine("	pc.alterado_por as alteradopor,");
            query.AppendLine("	pc.criado_por as criadopor,");
            query.AppendLine("	pc.criado_em as criadoem,");
            query.AppendLine("	pc.ciclo_id as CicloId,");
            query.AppendLine("	pc.ciclo_id as CicloId,");
            query.AppendLine("	pc.migrado as Migrado,");
            query.AppendLine("	string_agg(distinct cast(msp.matriz_id as text), ',') as MatrizesSaber,");
            query.AppendLine("	string_agg(distinct cast(odp.objetivo_desenvolvimento_id as text), ',') as ObjetivosDesenvolvimento");
            query.AppendLine("from");
            query.AppendLine("	plano_ciclo pc");
            query.AppendLine("left join matriz_saber_plano msp on");
            query.AppendLine("  pc.id = msp.plano_id");
            query.AppendLine("left join recuperacao_paralela_objetivo_desenvolvimento_plano odp on");
            query.AppendLine("  odp.plano_id = pc.id");
            query.AppendLine("where");
            query.AppendLine("  pc.ciclo_id = @cicloId and pc.ano = @ano and pc.escola_id = @escolaId");
            query.AppendLine("group by");
            query.AppendLine("  pc.id");

            return database.Conexao.Query<PlanoCicloCompletoDto>(query.ToString(), new { cicloId, ano, escolaId }).SingleOrDefault();
        }

        public bool ObterPlanoCicloPorAnoCicloEEscola(int ano, long cicloId, string escolaId)
        {
            return database.Conexao.Query<bool>("select 1 from plano_ciclo where ano = @ano and ciclo_id = @cicloId and escola_id = @escolaId", new { ano, cicloId, escolaId }).SingleOrDefault();
        }
    }
}