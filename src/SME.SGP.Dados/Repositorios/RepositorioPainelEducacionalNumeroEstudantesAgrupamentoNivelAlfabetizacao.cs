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

        public async Task<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao>> ObterNumeroAlunos(string anoLetivo, string periodo)
        {
            var sql = $@"SELECT * FROM painel_educacional_registro_frequencia_agrupamento_mensal
                        WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(anoLetivo))
                sql += " AND ano = @anoLetivo";

            if (!string.IsNullOrWhiteSpace(periodo))
                sql += " AND periodo = @periodo";

            return await database.QueryAsync<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacao>(sql, new { anoLetivo, periodo });
        }
    }
}
