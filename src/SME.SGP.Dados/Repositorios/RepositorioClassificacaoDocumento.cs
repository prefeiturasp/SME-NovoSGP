using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using ClassificacaoDocumento = SME.SGP.Dominio.ClassificacaoDocumento;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioClassificacaoDocumento : IRepositorioClassificacaoDocumento
    {
        protected readonly ISgpContext database;

        public RepositorioClassificacaoDocumento(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<bool> ValidarTipoDocumento(long classificacaoDocumentoId, int tipoDocumento)
        {
            const string query = @"select 1 
                                    from classificacao_documento 
                                    where id = @classificacaoDocumentoId 
                                      and tipo_documento_id = @tipoDocumento";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { classificacaoDocumentoId, tipoDocumento });
        }

        public async Task<ClassificacaoDocumento> ObterPorIdAsync(long id)
        {
            return await database.Conexao.GetAsync<ClassificacaoDocumento>(id);
        }
    }
}