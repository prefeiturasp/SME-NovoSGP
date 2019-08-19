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

        public PlanoAnualCompletoDto ObterPlanoAnualComObjetivosAprendizagem(int ano, long cicloId, long escolaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("	pa.id,");
            query.AppendLine("	pa.descricao,");
            query.AppendLine("	string_agg(distinct cast(odp.objetivo_aprendizagem_id as text), ',') as ObjetivosDesenvolvimento");
            query.AppendLine("from");
            query.AppendLine("	plano_anual pa");
            query.AppendLine("inner join objetivo_aprendizagem_plano oa on");
            query.AppendLine("  pa.id = oa.plano_id");
            query.AppendLine("where");
            query.AppendLine("  pa.ciclo_id = @cicloId and pc.ano = @ano and pc.escola_id = @escolaId");
            query.AppendLine("group by");
            query.AppendLine("  pa.id");

            return database.Conexao.Query<PlanoAnualCompletoDto>(query.ToString(), new { cicloId, ano, escolaId }).SingleOrDefault();
        }
    }
}