using Dapper;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios
{
    public class RepositorioSGP : IRepositorioSGP
    {
        private readonly ISgpContext database;

        public RepositorioSGP(ISgpContext database)
        {
            this.database = database;
        }

        public Task<int> ObterQuantidadeAcessosDia(DateTime data)
            => database.Conexao.QueryFirstOrDefaultAsync<int>("select count(u.id) from usuario u where date(u.ultimo_login) = @data", new { data });
    }
}
