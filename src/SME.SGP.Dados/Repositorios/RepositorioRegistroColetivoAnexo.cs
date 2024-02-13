using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroColetivoAnexo : RepositorioBase<RegistroColetivoAnexo>, IRepositorioRegistroColetivoAnexo
    {
        public RepositorioRegistroColetivoAnexo(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> RemoverPorRegistroColetivoId(long registroColetivoId)
        {
            var query = "delete from registrocoletivo_anexo where registrocoletivo_id = @registroColetivoId";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { registroColetivoId });
        }
    }
}
