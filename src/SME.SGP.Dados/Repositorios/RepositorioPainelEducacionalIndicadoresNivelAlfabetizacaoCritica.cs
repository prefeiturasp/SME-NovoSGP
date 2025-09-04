using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica : RepositorioBase<PainelEducacionalIndicadoresNivelAlfabetizacaoCritica>, IRepositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica
    {
        public RepositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<PainelEducacionalIndicadoresNivelAlfabetizacaoCritica>> ObterNumeroEstudantes(string codigoDre, string codigoUe)
        {
            var sql = $@"SELECT * FROM painel_educacional_registro_frequencia_agrupamento_mensal
                        WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                sql += " AND ano = @anoLetivo";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND periodo = @periodo";

            sql += " LIMIT 10";

            return await database.QueryAsync<PainelEducacionalIndicadoresNivelAlfabetizacaoCritica>(sql, new { codigoDre, codigoUe });
        }
    }
}
