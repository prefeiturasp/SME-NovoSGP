using Dapper;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioIdep : RepositorioBase<Idep>, IRepositorioIdep
    {
        public RepositorioIdep(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<Idep> ObterRegistroIdepAsync(int anoLetivo, int serieAno, string codigoEOLEscola)
        {
            string query = @"select * from idep
                            where ano_letivo = @anoLetivo
                            and serie_ano = @serieAno
                            and codigo_eol_escola = @codigoEOLEscola;";

            return database.Conexao.QueryFirstOrDefault<Idep>(query, new
            {
                anoLetivo,
                serieAno,
                codigoEOLEscola
            });
        }
    }
}
