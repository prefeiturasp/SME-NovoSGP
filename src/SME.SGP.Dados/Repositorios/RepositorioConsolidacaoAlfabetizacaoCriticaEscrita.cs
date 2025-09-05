using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
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
    }
}
