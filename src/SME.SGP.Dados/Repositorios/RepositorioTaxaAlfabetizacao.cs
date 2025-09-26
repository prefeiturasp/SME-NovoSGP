using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.TaxaAlfabetizacao;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTaxaAlfabetizacao : RepositorioBase<TaxaAlfabetizacao>, IRepositorioTaxaAlfabetizacao
    {
        public RepositorioTaxaAlfabetizacao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        { }

        public async Task<TaxaAlfabetizacao> ObterRegistroAlfabetizacaoAsync(int anoLetivo, string codigoEOLEscola)
        {
            string query = @"select * from taxa_alfabetizacao
                             where ano_letivo = @anoLetivo
                             and codigo_eol_escola = @codigoEOLEscola;";

            return await database.Conexao.QueryFirstOrDefaultAsync<TaxaAlfabetizacao>(query, new
            {
                anoLetivo,
                codigoEOLEscola
            });
        }

        public async Task<IEnumerable<TaxaAlfabetizacaoDto>> ObterTaxaAlfabetizacaoAsync()
        {
            string query = @"select 
                                    d.dre_id as CodigoDre,  
                                    u.ue_id as CodigoUe,
                                    tx.ano_letivo as AnoLetivo,      
                                    tx.codigo_eol_escola as CodigoEOLEscola,
                                    tx.taxa as Taxa 
                                from Taxa_alfabetizacao tx
                                inner join ue u on tx.codigo_eol_escola = u.ue_id 
                                inner join dre d on d.id = u.dre_id
                            where tx.ano_letivo > 2019";

            return await database.Conexao.QueryAsync<TaxaAlfabetizacaoDto>(query);
        }
    }
}
