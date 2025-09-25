using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAlfabetizacao : RepositorioBase<TaxaAlfabetizacao>, IRepositorioAlfabetizacao
    {
        public RepositorioAlfabetizacao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<TaxaAlfabetizacao> ObterRegistroAlfabetizacaoAsync(int anoLetivo, string codigoEOLEscola)
        {
            string query = @"select * from taxa_alfabetizacao
                    where ano_letivo = @anoLetivo
                    and codigo_eol_escola = @codigoEOLEscola;";

            return database.Conexao.QueryFirstOrDefault<TaxaAlfabetizacao>(query, new
            {
                anoLetivo,
                codigoEOLEscola
            });
        }
    }
}
