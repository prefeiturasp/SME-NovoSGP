using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao : RepositorioBase<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao>, IRepositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao
    {
        public RepositorioPainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
        public async Task<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao>> ObterNumeroAlunos(int anoLetivo, int periodo, string codigoDre = null, string codigoUe = null)
        {

            var sql = $@"SELECT * FROM consolidacao_alfabetizacao_nivel_escrita
                        WHERE 1 = 1";

            if (anoLetivo != -99 && anoLetivo != 0)
                sql += " AND ano_letivo = @anoLetivo";

            if (periodo != -99 && periodo != 0)
                sql += " AND periodo = @periodo";

            if (!string.IsNullOrWhiteSpace(codigoDre) && !codigoDre.Equals("-99"))
                sql += " AND dre_codigo = @codigoDre";

            if (!string.IsNullOrWhiteSpace(codigoUe) && !codigoUe.Equals("-99"))
                sql += " AND ue_codigo = @codigoUe";

            return await database.QueryAsync<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao>(sql, new { anoLetivo, periodo, codigoDre, codigoUe });
        }
    }
}
