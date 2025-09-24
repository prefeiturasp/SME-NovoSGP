using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioIdeb : RepositorioBase<Ideb>, IRepositorioIdeb
    {
        public RepositorioIdeb(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<Ideb> ObterRegistroIdebAsync(int anoLetivo, int serieAno, string codigoEOLEscola)
        {
            string query = @"select * from ideb
                            where ano_letivo = @anoLetivo
                            and serie_ano = @serieAno
                            and codigo_eol_escola = @codigoEOLEscola;";

            return database.Conexao.QueryFirstOrDefault<Ideb>(query, new
            {
                anoLetivo,
                serieAno,
                codigoEOLEscola
            });
        }
    }
}
