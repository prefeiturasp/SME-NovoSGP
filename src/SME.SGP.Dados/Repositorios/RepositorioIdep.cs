using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            return await database.Conexao.QueryFirstOrDefaultAsync<Idep>(query, new
            {
                anoLetivo,
                serieAno,
                codigoEOLEscola
            });
        }

        public async Task<IEnumerable<Idep>> ObterRegistrosIdepsAsync(int anoLetivo, string codigoEOLEscola)
        {
            string query = MontarQuery(anoLetivo, codigoEOLEscola);
            var parametros = new { anoLetivo, codigoEOLEscola };

            return await database.Conexao.QueryAsync<Idep>(query, parametros);
        }

        private static string MontarQuery(int anoLetivo, string codigoEOLEscola)
        {
            var query = new StringBuilder();
            query.AppendLine("SELECT *");
            query.AppendLine("FROM idep i");

            var conditions = new List<string>();

            if (!string.IsNullOrEmpty(codigoEOLEscola))
                conditions.Add("i.codigo_eol_escola = @codigoEOLEscola");

            if (anoLetivo > 0)
                conditions.Add("i.ano_letivo = @anoLetivo");

            if (conditions.Any())
            {
                query.AppendLine("WHERE " + string.Join(" AND ", conditions));
            }

            return query.ToString();
        }
    }
}
