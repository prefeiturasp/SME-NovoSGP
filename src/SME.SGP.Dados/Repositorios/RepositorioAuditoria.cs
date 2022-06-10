using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAuditoria : IRepositorioAuditoria
    {
        protected readonly ISgpContext database;

        public RepositorioAuditoria(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<Auditoria>> ObtenhaAuditoriaDoAdministrador(string login)
        {
            var sql = @"SELECT id, data, entidade, chave, usuario, acao, rf, perfil, administrador  
                        FROM auditoria 
                        WHERE administrador = @login";
  
            return await database.Conexao.QueryAsync<Auditoria>(sql, new { login });
        }
    }
}
