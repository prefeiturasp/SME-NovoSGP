using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
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
    }
}
