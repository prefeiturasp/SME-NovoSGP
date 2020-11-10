using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDocumento : RepositorioBase<Documento>, IRepositorioDocumento
    {
        public RepositorioDocumento(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<bool> ValidarUsuarioPossuiDocumento(long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId)
        {
            var query = @"select distinct 1 from documento 
                   inner join classificacao_documento cd on documento.classificacao_documento_id = cd.id
                where 
                documento.classificacao_documento_id = @classificacaoId and 
                documento.usuario_id = @usuarioId and 
                documento.ue_id = @ueId and
                cd.tipo_documento_id = @tipoDocumentoId and
                cd.tipo_documento_id = @tipoDocumentoId";
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { tipoDocumentoId, classificacaoId, usuarioId, ueId });
        }
    }
}