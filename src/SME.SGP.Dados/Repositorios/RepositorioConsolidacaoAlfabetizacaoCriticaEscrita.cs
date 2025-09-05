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
    public class RepositorioConsolidacaoAlfabetizacaoCriticaEscrita : IRepositorioConsolidacaoAlfabetizacaoCriticaEscrita
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoAlfabetizacaoCriticaEscrita(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }
        public async Task ExcluirConsolidacaoAlfabetizacaoCriticaEscrita()
        {
            const string comando = @"truncate table public.consolidacao_alfabetizacao_critica_escrita";
            await database.Conexao.ExecuteAsync(comando);
        }

        public async Task<bool> SalvarConsolidacaoAlfabetizacaoCriticaEscrita(ConsolidacaoAlfabetizacaoCriticaEscrita entidade)
        {
            return (long)(await database.Conexao.InsertAsync(entidade)) > 0;
        }
        public async Task<IEnumerable<ConsolidacaoAlfabetizacaoCriticaEscrita>> ObterNumeroEstudantes(string codigoDre, string codigoUe)
        {
            var query = new StringBuilder(@"
                                           SELECT * 
                                           FROM consolidacao_alfabetizacao_critica_escrita");

            var condicoes = new List<string>();
            var parametros = new DynamicParameters();

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

            return await database.QueryAsync<ConsolidacaoAlfabetizacaoCriticaEscrita>(
                query.ToString(),
                parametros
            );
        }
    }
}
