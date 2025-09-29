using Dapper;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
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

        public async Task<IEnumerable<PainelEducacionalFluenciaLeitoraDto>> ObterFluenciaLeitora(int periodo, int anoLetivo, string codigoDre)
        {
            var query = new StringBuilder(@"
                                           SELECT fluencia as NomeFluencia,
                                           descricao_fluencia as DescricaoFluencia,
                                           dre_codigo as DreCodigo,
                                           percentual, 
                                           quantidade_alunos as QuantidadeAlunos,
                                           ano,
                                           periodo
                                           FROM consolidacao_painel_educacional_fluencia_leitora");

            var condicoes = new List<string>();
            var parametros = new DynamicParameters();

            if (periodo != 0)
            {
                condicoes.Add("periodo = @periodo");
                parametros.Add("periodo", periodo);
            }

            if (anoLetivo != 0 && anoLetivo != 0)
            {
                condicoes.Add("ano = @anoLetivo");
                parametros.Add("anoLetivo", anoLetivo);
            }

            if (!string.IsNullOrWhiteSpace(codigoDre) && codigoDre != "0")
            {
                condicoes.Add("dre_codigo = @codigoDre");
                parametros.Add("codigoDre", codigoDre);
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