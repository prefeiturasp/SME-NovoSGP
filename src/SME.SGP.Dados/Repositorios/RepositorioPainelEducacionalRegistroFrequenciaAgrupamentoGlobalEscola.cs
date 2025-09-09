using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola : RepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>, IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola
    {
        public RepositorioPainelEducacionalRegistroFrequenciaAgrupamentoGlobalEscola(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExcluirFrequenciaGlobal()
        {
            const string comando = @"delete from public.painel_educacional_registro_frequencia_agrupamento_escola";

            await database.Conexao.ExecuteAsync(comando);
        }

        public async Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>> ObterFrequenciaGlobal(string codigoDre, string codigoUe)
        {
            var sql = $@"SELECT *, d.abreviacao as Dre FROM painel_educacional_registro_frequencia_agrupamento_escola p
                         INNER JOIN dre d on d.dre_id = p.codigo_dre 
                         WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                sql += " AND p.codigo_dre = @codigoDre";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND p.codigo_ue = @codigoUe";

            return await database.QueryAsync<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>(sql, new { codigoDre, codigoUe });
        }

        public async Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoEscola entidade)
        {
            var retorno = await SalvarAsync(entidade);
            return retorno != 0;
        }
    }
}
