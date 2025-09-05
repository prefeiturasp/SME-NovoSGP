using Dapper;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoAlfabetizacaoNivelEscrita : IRepositorioConsolidacaoAlfabetizacaoNivelEscrita
    {
        private readonly ISgpContext database;
        public RepositorioConsolidacaoAlfabetizacaoNivelEscrita(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task ExcluirConsolidacaoNivelEscrita()
        {
            const string comando = @"truncate table public.consolidacao_alfabetizacao_nivel_escrita";
            await database.Conexao.ExecuteAsync(comando);
        }

        public async Task<bool> SalvarConsolidacaoNivelEscrita(ConsolidacaoAlfabetizacaoNivelEscrita entidade)
        {
            return (long)(await database.Conexao.InsertAsync(entidade)) > 0;
        }

        public async Task<IEnumerable<ContagemNivelEscritaDto>> ObterNumeroAlunos(int anoLetivo, int periodo, string codigoDre = null, string codigoUe = null)
        {
            var query = new StringBuilder(@"
                SELECT 
                    nivel_escrita AS NivelEscrita, 
                    SUM(quantidade) AS Quantidade 
                FROM consolidacao_alfabetizacao_nivel_escrita");
            var condicoes = new List<string>();
            var parametros = new DynamicParameters();

            if (anoLetivo > 0 && anoLetivo != -99)
            {
                condicoes.Add("ano_letivo = @anoLetivo");
                parametros.Add("anoLetivo", anoLetivo);
            }

            if (periodo > 0 && periodo != -99)
            {
                condicoes.Add("periodo = @periodo");
                parametros.Add("periodo", periodo);
            }

            if (!string.IsNullOrWhiteSpace(codigoDre) && codigoDre != "-99")
            {
                condicoes.Add("dre_codigo = @codigoDre");
                parametros.Add("codigoDre", codigoDre);
            }

            if (!string.IsNullOrWhiteSpace(codigoUe) && codigoUe != "-99")
            {
                condicoes.Add("ue_codigo = @codigoUe");
                parametros.Add("codigoUe", codigoUe);
            }

            if (condicoes.Any())
            {
                query.Append(" WHERE ");
                query.Append(string.Join(" AND ", condicoes));
            }
            query.Append(" GROUP BY nivel_escrita");

            return await database.QueryAsync<ContagemNivelEscritaDto>(query.ToString(), parametros);
        }
    }
}
