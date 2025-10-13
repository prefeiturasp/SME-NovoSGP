using Polly.Registry;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalAbandono : IRepositorioPainelEducacionalAbandono
    {
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalAbandono(ISgpContext database, IReadOnlyPolicyRegistry<string> registry)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalAbandono>> ObterAbandonoVisaoSmeDre(int anoLetivo, string codigoDre)
        {
            string query = MontarQuery(codigoDre);

            var parametros = new { anoLetivo, codigoDre };

            return await database.Conexao.QueryAsync<PainelEducacionalAbandono>(query.ToString(), parametros);
        }

        private static string MontarQuery(string codigoDre)
        {
            var query = new StringBuilder();
            query.AppendLine("SELECT *");
            query.AppendLine("FROM painel_educacional_consolidacao_abandono peci");
            query.AppendLine("WHERE peci.ano_letivo = @anoLetivo");

            if (!string.IsNullOrEmpty(codigoDre))
                query.AppendLine("AND peci.codigo_dre = @codigoDre");

            return query.ToString();
        }
    }
}
