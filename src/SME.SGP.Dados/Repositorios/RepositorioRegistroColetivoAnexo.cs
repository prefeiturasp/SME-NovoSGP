using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroColetivoAnexo : RepositorioBase<RegistroColetivoAnexo>, IRepositorioRegistroColetivoAnexo
    {
        public RepositorioRegistroColetivoAnexo(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<AnexoDto>> ObterAnexoPorRegistroColetivoId(long registroColetivoId)
        {
            var query = @"select ra.arquivo_id as ArquivoId, a.codigo as AnexoId
                from registrocoletivo_anexo ra
                inner join arquivo a on a.id = ra.arquivo_id
                where registrocoletivo_id = @registroColetivoId";

            return await database.Conexao.QueryAsync<AnexoDto>(query, new { registroColetivoId });
        }

        public async Task<bool> RemoverPorRegistroColetivoId(long registroColetivoId)
        {
            var query = "delete from registrocoletivo_anexo where registrocoletivo_id = @registroColetivoId";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { registroColetivoId });
        }
    }
}
