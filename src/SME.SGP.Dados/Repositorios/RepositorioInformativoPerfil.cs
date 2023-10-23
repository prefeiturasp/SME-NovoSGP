using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioInformativoPerfil : RepositorioBase<InformativoPerfil>, IRepositorioInformativoPerfil
    {
        public RepositorioInformativoPerfil(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> RemoverPerfisPorInformesIdAsync(long informesId)
        {
            var query = @"delete from informativo_perfil where informativo_id = @informesId";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { informesId });
        }
    }
}
