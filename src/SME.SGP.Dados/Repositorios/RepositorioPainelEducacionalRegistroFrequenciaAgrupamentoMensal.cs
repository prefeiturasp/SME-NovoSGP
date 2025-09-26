using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal : RepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>, IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal
    {
        public RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExcluirFrequenciaMensal(PainelEducacionalRegistroFrequenciaAgrupamentoMensal entidade)
        {
            await RemoverAsync(entidade);
        }

        public async Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>> ObterFrequenciaMensal(string codigoDre, string codigoUe)
        {
            var sql = $@"SELECT * FROM painel_educacional_registro_frequencia_agrupamento_mensal
                        WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                sql += " AND codigo_dre = codigoDre";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND codigo_ue = codigoUe";

            sql += " ORDER BY modalidade";

            return await database.QueryAsync<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>(sql, new { anoLetivo, codigoDre, codigoUe });
        }

        public async Task<bool> SalvarFrequenciaMensal(PainelEducacionalRegistroFrequenciaAgrupamentoMensal entidade)
        {
            var retorno = await SalvarAsync(entidade);
            return retorno != 0;
        }
    }
}
