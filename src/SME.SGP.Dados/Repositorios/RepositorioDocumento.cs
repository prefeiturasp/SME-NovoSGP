using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioDocumento : RepositorioBase<Documento>, IRepositorioDocumento
    {
        public RepositorioDocumento(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<bool> ExcluirDocumentoPorId(long id)
        {
            var query = "delete from documento where id = @id";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { id });
        }

        public async Task<PaginacaoResultadoDto<DocumentoResumidoDto>> ObterPorDreUeTipoEClassificacaoPaginada(long? dreId, long? ueId, long tipoDocumentoId, 
            long classificacaoId, int? anoLetivo, Paginacao paginacao)
        {
            var sql = MontaQueryCompleta(dreId, ueId, tipoDocumentoId, classificacaoId, anoLetivo);

            var parametros = new { dreId, ueId, tipoDocumentoId, classificacaoId, anoLetivo };

            var documentos = await database.Conexao.QueryAsync<DocumentoCompletoDto>(sql, parametros);
            
            var documentosAgrupados = documentos.GroupBy(g => new
            { 
                g.DocumentoId,
                g.Classificacao,
                g.TipoDocumento,
                g.Usuario,
                g.Data,
                g.TurmaNome,
                g.Modalidade,
                g.ComponenteCurricularNome,
                g.CodigoDre,
                g.NomeDre,
                g.AbreviacaoDre,
                g.CodigoUe,
                g.NomeUe,
                g.TipoEscola
            }, (key, group) => 
                new DocumentoResumidoDto { 
                    DocumentoId = key.DocumentoId,
                    Classificacao = key.Classificacao,
                    TipoDocumento = key.TipoDocumento,
                    Usuario = key.Usuario,
                    Data = key.Data,
                    TurmaComponenteCurricular = ObterTurmaComponenteCurricular(key.TurmaNome, key.ComponenteCurricularNome, key.Modalidade),
                    Arquivos = group.Select(s=>
                        new ArquivoResumidoDto
                        {
                            Codigo = s.CodigoArquivo,
                            Nome = s.NomeArquivo
                        }).ToList(),
                    NomeDre = key.NomeDre,
                    AbreviacaoDre = key.AbreviacaoDre,
                    NomeUe = key.NomeUe,
                    TipoEscola = key.TipoEscola,
                    CodigoDre = key.CodigoDre,
                    CodigoUe = key.CodigoUe
                })
                .OrderBy(o => o.CodigoDre)
                .ThenBy(o => o.SiglaNomeUe)
                .ThenBy(o => o.TurmaComponenteCurricular)
                .ThenByDescending(o => o.Data)
                .ToList();

            var retorno = new PaginacaoResultadoDto<DocumentoResumidoDto> { Items = documentosAgrupados };
            retorno.TotalRegistros = retorno.Items.NaoEhNulo() && retorno.Items.Any() ? retorno.Items.Count() : 0;
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);
            retorno.Items = retorno.Items.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string ObterTurmaComponenteCurricular(string turmaNome, string componenteCurricularNome, int modalidade)
        {
            if (!string.IsNullOrEmpty(turmaNome) && !string.IsNullOrEmpty(componenteCurricularNome))
                return $"{((Modalidade)modalidade).ShortName()} - {turmaNome} - {componenteCurricularNome}";
                
            return string.Empty;
        }

        private static string MontaQueryCompleta(long? dreId, long? ueId, long tipoDocumentoId, long classificacaoId, int? anoLetivo)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(sql, dreId, ueId, anoLetivo, tipoDocumentoId, classificacaoId);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(StringBuilder sql, long? dreId, long? ueId, int? anoLetivo, long tipoDocumentoId = 0, long classificacaoId = 0)
        {
            ObterCabecalho(sql);

            ObterFiltro(sql, dreId, ueId, tipoDocumentoId, classificacaoId, anoLetivo);
        }

        private static void ObterCabecalho(StringBuilder sql)
        {
            sql.AppendLine(@"select d.id as DocumentoId,      
                                  cd.descricao as classificacao, 
                                  td.descricao as tipoDocumento,
                                  t.nome as turmanome,
                                  t.modalidade_codigo as modalidade,
                                  coalesce(cc.descricao_infantil,cc.descricao_sgp) as componenteCurricularNome,
                                  a.codigo as CodigoArquivo, 
                                  a.nome as NomeArquivo, 
                                  u.nome || ' (' || u.rf_codigo || ')' as Usuario,
                                  case when d.alterado_em is not null then d.alterado_em else d.criado_em end as Data,
                                  dre.dre_id as CodigoDre, ue.ue_id as CodigoUe,
                                  dre.nome as NomeDre, ue.nome as NomeUe, ue.tipo_escola as TipoEscola,
                                  dre.abreviacao as AbreviacaoDre 
                            from documento d 
                               join  classificacao_documento cd on d.classificacao_documento_id = cd.id 
                               join  tipo_documento td on cd.tipo_documento_id = td.id       
                               join usuario u on d.usuario_id = u.id
                               join ue on ue.id = d.ue_id
                               join dre on dre.id = ue.dre_id
                               left join turma t on t.id = d.turma_id
                               left join componente_curricular cc on cc.id = d.componente_curricular_id
                               left join documento_arquivo da on da.documento_id = d.id 
                               left join arquivo a on a.id = da.arquivo_id  ");
        }

        private static void ObterFiltro(StringBuilder sql, long? dreId, long? ueId, long tipoDocumentoId, long classificacaoId, int? anoLetivo)
        {
            sql.AppendLine("where 1 = 1");
            if (dreId.HasValue && !dreId.Equals(-99))
                sql.AppendLine(" and ue.dre_id = @dreId ");
            if (ueId.HasValue && !ueId.Equals(-99))
                sql.AppendLine(" and ue.id = @ueId ");
            if (tipoDocumentoId > 0)
                sql.AppendLine("and td.id = @tipoDocumentoId ");
            if (classificacaoId > 0)
                sql.AppendLine("and cd.id = @classificacaoId ");
            if (anoLetivo.NaoEhNulo())
                sql.AppendLine("and d.ano_letivo = @anoLetivo");
        }

        public async Task<bool> RemoverReferenciaArquivo(long documentoId, long arquivoId)
        {
            const string query = @"delete from documento_arquivo where documento_id = @documentoId and arquivo_id = @arquivoId";
            await database.Conexao.ExecuteAsync(query, new { documentoId, arquivoId });
            return true;
        }

        public async Task<bool> ValidarUsuarioPossuiDocumento(long tipoDocumentoId, long classificacaoId, long usuarioId, long ueId, long anoLetivo,long documentoId)
        {
            const string query = @"select distinct 1 from documento 
                                        inner join classificacao_documento cd on documento.classificacao_documento_id = cd.id
                                    where documento.id <> @documentoId and
                                    documento.classificacao_documento_id = @classificacaoId and 
                                    documento.usuario_id = @usuarioId and 
                                    documento.ue_id = @ueId and
                                    cd.tipo_documento_id = @tipoDocumentoId and
                                    cd.tipo_documento_id = @tipoDocumentoId and
                                    documento.ano_letivo = @anoLetivo and
                                    not cd.ehregistromultiplo";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query,
                new { tipoDocumentoId, classificacaoId, usuarioId, ueId, anoLetivo,documentoId });
        }

        public async Task<ObterDocumentoResumidoDto> ObterPorIdCompleto(long documentoId)
        {
            const string query = @"select 
                                        d.id as Id,                            
                                        a.nome as NomeArquivo,
                                        a.codigo as CodigoArquivo,
                                        d.alterado_em,
                                        d.alterado_por ,
                                        d.alterado_rf ,
                                        d.criado_em ,
                                        d.criado_por ,
                                        d.criado_rf ,
                                        d.ano_letivo as AnoLetivo,
                                        d.classificacao_documento_id as ClassificacaoId,
                                        cd.descricao as ClassificacaoDescricao,
                                        cd.tipo_documento_id as TipoDocumentoId,
                                        td.descricao as TipoDocumentoDescricao,
                                        u.rf_codigo as ProfessorRf,
                                        ue.ue_id as UeId,
                                        ue.nome as UeNome,
                                        ue.tipo_escola as TipoEscola,
                                        dre.dre_id as DreId,
                                        dre.nome as DreNome,
                                        d.turma_id as TurmaId,
                                        t.turma_id as turmaCodigo,
                                        t.nome_filtro as TurmaNome,
                                        t.semestre,
                                        t.modalidade_codigo as modalidade,
                                        d.componente_curricular_id as ComponenteCurricularId,
                                        cc.descricao as ComponenteCurricularDescricao
                                    from documento d
                                        inner join usuario u on d.usuario_id = u.id
                                        inner join ue on d.ue_id = ue.id 
                                        inner join classificacao_documento cd on d.classificacao_documento_id = cd.id 
                                        inner join dre on ue.dre_id = dre.id 
                                        left join turma t on t.id = d.turma_id 
                                        left join documento_arquivo da on da.documento_id = d.id 
                                        left join arquivo a on a.id = da.arquivo_id 
                                        left join componente_curricular cc on cc.id = d.componente_curricular_id
                                        inner join tipo_documento td on td.id = cd.tipo_documento_id
                                    WHERE d.id = @documentoId";
            
            var documentosCompleto = (await database.Conexao.QueryAsync<ObterDocumentoCompletoDto>(query, new { documentoId })).ToList();

            var documentoCompleto = documentosCompleto.FirstOrDefault();

            if (documentoCompleto.EhNulo())
                return new ObterDocumentoResumidoDto();

            var documentoResumido = new ObterDocumentoResumidoDto
            {
                Id = documentoCompleto.Id,
                AlteradoEm = documentoCompleto.AlteradoEm,
                AlteradoPor = documentoCompleto.AlteradoPor,
                AlteradoRF = documentoCompleto.AlteradoRF,
                CriadoPor = documentoCompleto.CriadoPor,
                CriadoRF = documentoCompleto.CriadoRF,
                CriadoEm = documentoCompleto.CriadoEm,
                AnoLetivo = documentoCompleto.AnoLetivo,
                ClassificacaoId = documentoCompleto.ClassificacaoId,
                ClassificacaoDescricao = documentoCompleto.ClassificacaoDescricao,
                TipoDocumentoId = documentoCompleto.TipoDocumentoId,
                TipoDocumentoDescricao = documentoCompleto.TipoDocumentoDescricao,
                ProfessorRf = documentoCompleto.ProfessorRf,
                UeId = documentoCompleto.UeId,
                UeNome = (TipoEscola)documentoCompleto.TipoEscola == TipoEscola.Nenhum
                    ? documentoCompleto.UeNome
                    : $"{((TipoEscola)documentoCompleto.TipoEscola).ShortName()} {documentoCompleto.UeNome}",
                DreId = documentoCompleto.DreId,
                DreNome = documentoCompleto.DreNome,
                TurmaId = documentoCompleto.TurmaId,
                TurmaCodigo = documentoCompleto.TurmaCodigo,
                TurmaNome = documentoCompleto.TurmaNome,
                Modalidade = documentoCompleto.Modalidade,
                ModalidadeNome = documentoCompleto.Modalidade.EhNulo()
                    ? null
                    : ((Modalidade)documentoCompleto.Modalidade).Name(),
                Semestre = documentoCompleto.Semestre,
                ComponenteCurricularId = documentoCompleto.ComponenteCurricularId,
                ComponenteCurricularDescricao = documentoCompleto.ComponenteCurricularDescricao
            };

            documentoResumido.Arquivos.AddRange(documentosCompleto.Select(s => new ArquivoResumidoDto
            {
                Codigo = s.CodigoArquivo,
                Nome = s.NomeArquivo
            }));

            return documentoResumido;
        }
    }
}