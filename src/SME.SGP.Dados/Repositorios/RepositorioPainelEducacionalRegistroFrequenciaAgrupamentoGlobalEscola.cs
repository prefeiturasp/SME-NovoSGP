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
            var sql = $@"SELECT * FROM painel_educacional_registro_frequencia_agrupamento_escola
                        WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                sql += " AND codigo_dre = codigoDre";
            
            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND codigo_ue = codigoUe";

            return await database.QueryAsync<PainelEducacionalRegistroFrequenciaAgrupamentoEscola>(sql, new { codigoDre, codigoUe });
        }

        public async Task<bool> SalvarFrequenciaGlobal(PainelEducacionalRegistroFrequenciaAgrupamentoEscola entidade)
        {
            var retorno = await SalvarAsync(entidade);
            return retorno != 0;
        }
    }
}
