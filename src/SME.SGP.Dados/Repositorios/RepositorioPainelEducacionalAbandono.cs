using Polly.Registry;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
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
            var sql = @"SELECT * FROM painel_educacional_consolidacao_abandono WHERE ano_letivo = @anoLetivo
                        AND (@codigoDre IS NULL OR codigo_dre = @codigoDre)";

            return await database.Conexao.QueryAsync<PainelEducacionalAbandono>(sql, new { anoLetivo, codigoDre });
        }
    }
}
