using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
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

        public async Task<IEnumerable<DocumentoDto>> ObterPorUeTipoEClassificacao(long ueId, long tipoDocumentoId, long classificacaoId)
        {
            var query = @"select 
	                        td.descricao as tipoDocumento,
                            cd.descricao as classificacao,
                            usuario_id as usuarioId,
                            u.nome || ' (' || u.rf_codigo || ')' as usuario,
                            case when d.alterado_em is not null then d.alterado_em else d.criado_em end as dataUpload
                        from documento d
                        inner join
	                        classificacao_documento cd on d.classificacao_documento_id = cd.id
                        inner join
	                        tipo_documento td on cd.tipo_documento_id = td.id
                        inner join usuario u on 
	                        d.usuario_id = u.id
                        where d.ue_id = @ueId and td.id = @tipoDocumentoId and cd.id = @classificacaoId";
            return await database.Conexao.QueryAsync<DocumentoDto>(query, new { ueId, tipoDocumentoId, classificacaoId });
        }

        public async Task<bool> RemoverReferenciaArquivo(long documentoId, long arquivoId)
        {
            var query = @"update documento set arquivo_id = null where id = @documentoId and arquivo_id = @arquivoId";

            await database.Conexao.ExecuteAsync(query, new { documentoId, arquivoId });

            return true;
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