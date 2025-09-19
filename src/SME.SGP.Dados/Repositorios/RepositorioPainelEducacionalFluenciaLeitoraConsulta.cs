using Dapper;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalFluenciaLeitoraConsulta : IRepositorioPainelEducacionalFluenciaLeitoraConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioPainelEducacionalFluenciaLeitoraConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalFluenciaLeitoraDto>> ObterFluenciaLeitora(string periodo, string anoLetivo, string codigoDre, string codigoUe)
        {
            var query = new StringBuilder(@"
                                           SELECT * 
                                           FROM consolidacao_painel_educacional_fluencia-leitora");

            var condicoes = new List<string>();
            var parametros = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(periodo) && periodo != "-99")
            {
                condicoes.Add("periodo = @periodo");
                parametros.Add("periodo", periodo);
            }

            if (!string.IsNullOrWhiteSpace(anoLetivo) && anoLetivo != "-99" && anoLetivo != "0")
            {
                condicoes.Add("ano_letivo = @anoLetivo");
                parametros.Add("anoLetivo", anoLetivo);
            }

            if (!string.IsNullOrWhiteSpace(codigoDre) && codigoDre != "-99" && codigoDre != "0")
            {
                condicoes.Add("dre_codigo = @codigoDre");
                parametros.Add("codigoDre", codigoDre);
            }

            if (!string.IsNullOrWhiteSpace(codigoUe) && codigoUe != "-99" && codigoUe != "0")
            {
                condicoes.Add("ue_codigo = @codigoUe");
                parametros.Add("codigoUe", codigoUe);
            }

            if (condicoes.Any())
            {
                query.Append(" WHERE ");
                query.Append(string.Join(" AND ", condicoes));
            }

            return await database.QueryAsync<PainelEducacionalFluenciaLeitoraDto>(
                query.ToString(),
                parametros
            );
        }
    }
}