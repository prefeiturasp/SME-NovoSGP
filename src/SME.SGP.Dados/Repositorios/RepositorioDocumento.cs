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

        public async Task<bool> ExcluirDocumentoPorId(long id)
        {
            var query = "delete from documento where id = @id";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { id });
        }

        public async Task<PaginacaoResultadoDto<DocumentoDto>> ObterPorUeTipoEClassificacaoPaginada(long ueId, long tipoDocumentoId, 
            long classificacaoId, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<DocumentoDto>();

            var sql = MontaQueryCompleta(paginacao, tipoDocumentoId, classificacaoId);

            var parametros = new { ueId, tipoDocumentoId, classificacaoId };

            using (var multi = await database.Conexao.QueryMultipleAsync(sql, parametros))
            {
                retorno.Items = multi.Read<DocumentoDto>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;

        }

        private static string MontaQueryCompleta(Paginacao paginacao, long tipoDocumentoId, long classificacaoId)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, tipoDocumentoId, classificacaoId);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, tipoDocumentoId, classificacaoId);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador = false, long tipoDocumentoId = 0, long classificacaoId = 0)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, tipoDocumentoId, classificacaoId);

            if (!contador)
                sql.AppendLine("order by d.id");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY");
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
            if(contador)
            {
                sql.AppendLine("count(*) ");
            } 
            else
            {
                sql.AppendLine("d.id as DocumentoId, ");
                sql.AppendLine("a.nome as NomeArquivo, ");
                sql.AppendLine("td.descricao as tipoDocumento, ");
                sql.AppendLine("cd.descricao as classificacao, ");
                sql.AppendLine("usuario_id as usuarioId, ");
                sql.AppendLine("u.nome || ' (' || u.rf_codigo || ')' as usuario, ");
                sql.AppendLine("case when d.alterado_em is not null then d.alterado_em else d.criado_em end as dataUpload, ");
                sql.AppendLine("a.codigo as CodigoArquivo ");
            }
            sql.AppendLine("from documento d ");
            sql.AppendLine("inner join ");
            sql.AppendLine("classificacao_documento cd on d.classificacao_documento_id = cd.id ");
            sql.AppendLine("inner join ");
            sql.AppendLine("tipo_documento td on cd.tipo_documento_id = td.id ");
            sql.AppendLine("inner join ");
            sql.AppendLine("arquivo a on d.arquivo_id = a.id ");
            sql.AppendLine("inner join usuario u on ");
            sql.AppendLine("d.usuario_id = u.id ");
        }

        private static void ObtenhaFiltro(StringBuilder sql, long tipoDocumentoId, long classificacaoId)
        {
            sql.AppendLine("where d.ue_id = @ueId ");

            if (tipoDocumentoId > 0)
                sql.AppendLine("and td.id = @tipoDocumentoId ");
            if (classificacaoId > 0)
                sql.AppendLine("and cd.id = @classificacaoId ");
        }

        public async Task<bool> RemoverReferenciaArquivo(long documentoId, long arquivoId)
        {
            var query = @"update documento set arquivo_id = null where id = @documentoId and arquivo_id = @arquivoId";

            await database.Conexao.ExecuteAsync(query, new { documentoId, arquivoId });

            return true;
        }

        public async Task<bool> ValidarUsuarioPossuiDocumento(long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId, long documentoId)
        {
            var query = @"select distinct 1 from documento 
                   inner join classificacao_documento cd on documento.classificacao_documento_id = cd.id
                where documento.id <> @documentoId and
                documento.classificacao_documento_id = @classificacaoId and 
                documento.usuario_id = @usuarioId and 
                documento.ue_id = @ueId and
                cd.tipo_documento_id = @tipoDocumentoId and
                cd.tipo_documento_id = @tipoDocumentoId";
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { tipoDocumentoId, classificacaoId, usuarioId, ueId, documentoId});
        }

        public async Task<ObterDocumentoDto> ObterPorIdCompleto(long documentoId)
        {
            var query = @"select 
                            d.id as Id,
                            d.arquivo_id as arquivoId,
                            a2.nome as NomeArquivo,
	                        d.alterado_em,
	                        d.alterado_por ,
	                        d.alterado_rf ,
	                        d.criado_em ,
	                        d.criado_por ,
	                        d.criado_rf ,
                            d.ano_letivo as AnoLetivo,
	                        d.classificacao_documento_id as ClassificacaoId,
	                        cd.tipo_documento_id as TipoDocumentoId,
	                        u.rf_codigo as ProfessorRf,
	                        ue.ue_id as UeId,
	                        dre.dre_id as DreId,
	                        a2.codigo  as CodigoArquivo
	                        from documento d
                        inner join usuario u on d.usuario_id = u.id
                        inner join ue on d.ue_id = ue.id
                        inner join arquivo a2 on d.arquivo_id = a2.id 
                        inner join classificacao_documento cd on d.classificacao_documento_id = cd.id 
                        inner join dre on ue.dre_id = dre.id 
                        WHERE d.id = @documentoId";
            return await database.Conexao.QueryFirstOrDefaultAsync<ObterDocumentoDto>(query, new { documentoId });
        }
    }
}