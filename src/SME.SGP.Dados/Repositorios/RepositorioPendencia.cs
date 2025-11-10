using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TipoPendencia = SME.SGP.Dominio.TipoPendencia;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPendencia : RepositorioBase<Pendencia>, IRepositorioPendencia
    {
        public RepositorioPendencia(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExclusaoLogicaPendencia(long pendenciaId)
        {
            await database.Conexao.ExecuteAsync("update pendencia set excluido = true where id = @pendenciaId", new { pendenciaId });
        }

        public async Task AtualizarQuantidadeDiasAulas(long pendenciaId, long quantidadeAulas, long quantidadeDias)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"update pendencia ");
            sql.AppendLine(@"set qtde_aulas = @qtdeaulas , qtde_dias = @qtdedias");
            sql.AppendLine(@"where id =@pendenciaId ");
            await database.Conexao.ExecuteAsync(sql.ToString(), new { pendenciaId, qtdeaulas = quantidadeAulas, qtdedias = quantidadeDias });
        }
        public async Task ExclusaoLogicaPendenciaIds(long[] pendenciasIds)
        {
            await database.Conexao.ExecuteAsync("update pendencia set excluido = true where id = ANY(@pendenciasIds)", new { pendenciasIds });
        }

        public async Task AtualizarPendencias(long fechamentoId, SituacaoPendencia situacaoPendencia, TipoPendencia tipoPendencia)
        {
            string query = "select fechamento_turma_id, disciplina_id from fechamento_turma_disciplina where id = @fechamentoId";
            (long fechamentoTurmaId, long disciplinaId) = await database.Conexao.QueryFirstOrDefaultAsync<(long fechamentoTurmaId, long disciplinaId)>(query, new { fechamentoId });
            query = @"update pendencia p
                                     set situacao = @situacaoPendencia
                                   from pendencia_fechamento f
                                   inner join fechamento_turma_disciplina ftd on ftd.id = f.fechamento_turma_disciplina_id                                    
                                     where f.pendencia_id = p.id
                                     and p.tipo = @tipoPendencia
                                     and ftd.fechamento_turma_id = @fechamentoTurmaId
                                     and ftd.disciplina_id = @disciplinaId
                                     and not p.excluido  ";

            await database.Conexao.ExecuteAsync(query, new { fechamentoTurmaId, disciplinaId, situacaoPendencia, tipoPendencia });
        }

        public async Task ExcluirPendenciasFechamento(long fechamentoId, TipoPendencia tipoPendencia)
        {
            string query = "select fechamento_turma_id, disciplina_id from fechamento_turma_disciplina where id = @fechamentoId";
            (long fechamentoTurmaId, long disciplinaId) = await database.Conexao.QueryFirstOrDefaultAsync<(long fechamentoTurmaId, long disciplinaId)>(query, new { fechamentoId });

            query = @"update pendencia p
                        set excluido = true
                      from pendencia_fechamento f
                      inner join fechamento_turma_disciplina ftd on ftd.id = f.fechamento_turma_disciplina_id                                    
                        where not p.excluido
                          and p.situacao = 1
                          and p.id = f.pendencia_id
                          and p.tipo = @tipoPendencia
                          and ftd.fechamento_turma_id = @fechamentoTurmaId
                          and ftd.disciplina_id = @disciplinaId";

            await database.Conexao.ExecuteAsync(query, new { fechamentoTurmaId, disciplinaId, tipoPendencia });
        }

        public async Task<long[]> ObterIdsPendenciasPorPlanoAEEId(long planoAeeId)
        {
            const string query = @"select p.id 
                                    from pendencia_plano_aee paee
                                        inner join pendencia p on paee.pendencia_id = p.id and p.situacao != 3
                                    where paee.plano_aee_id = @planoAeeId";

            var idsPendencias = await database.Conexao.QueryAsync<long>(query, new { planoAeeId });

            return idsPendencias.ToArray();
        }

        public async Task AtualizarStatusPendenciasPorIds(long[] ids, SituacaoPendencia situacaoPendencia)
        {
            const string query = @"update pendencia set situacao = @situacaoPendencia where id =ANY(@ids)";
            await database.Conexao.ExecuteAsync(query, new { ids, situacaoPendencia });
        }

        public async Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasPendentes()
        {
            const string query = @"select p.id as PendenciaId, p.ue_id as UeId
                                    from pendencia p
                                    where p.situacao = @situacao
                                    and p.tipo in (18,11,12,13,15)
                                    and excluido is false";

            return await database.Conexao.QueryAsync<PendenciaPendenteDto>(query, new { situacao = SituacaoPendencia.Pendente });
        }

        public async Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasSemPendenciaPerfilUsuario()
        {
            const string query = @"select distinct pp.pendencia_id as PendenciaId, p.ue_id as UeId 
                                    from pendencia_perfil pp 
                                        inner join pendencia p on p.id = pp.pendencia_id 
                                        left join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id 
                                    where ppu.id is null and not p.excluido and p.situacao = @situacao and p.ue_id is not null";

            return await database.Conexao.QueryAsync<PendenciaPendenteDto>(query, new { situacao = SituacaoPendencia.Pendente });
        }

        public async Task<int> ObterModalidadePorPendenciaETurmaId(long pendenciaId, long turmaId)
        {
            var query = @"select t.modalidade_codigo from pendencia p
                            inner join turma t on t.id = p.turma_id 
                            where p.id = @pendenciaId and t.id = @turmaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { pendenciaId, turmaId });
        }

        public async Task<IEnumerable<AulasDiasPendenciaDto>> ObterPendenciasParaCargaDiasAulas(int? anoLetivo, long ueid)
        {
            var situacoesPendencia = new int[] { (int)SituacaoPendencia.Aprovada, (int)SituacaoPendencia.Pendente };
            var tiposPendenciaFechamento = new int[] { (int)TipoPendencia.AulasSemFrequenciaNaDataDoFechamento, (int)TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento };
            var tiposPendenciaAula = new int[] { (int)TipoPendencia.Frequencia, (int)TipoPendencia.PlanoAula };
            var tiposPendenciaDiarioBordo = new int[] { (int)TipoPendencia.DiarioBordo };
            var anoLetivoInformado = anoLetivo ?? DateTime.Now.Year;
            var sql = new StringBuilder();
            sql.AppendLine(@"     select distinct ");
            sql.AppendLine(@"      p.id as PendenciaId,");
            sql.AppendLine(@"      count(a.data_aula) as QuantidadeDias,");
            sql.AppendLine(@"      sum(a.quantidade) as QuantidadeAulas");
            sql.AppendLine(@"     from pendencia_fechamento pf");
            sql.AppendLine(@"      inner join pendencia p on pf.pendencia_id  = p.id");
            sql.AppendLine(@"      inner join pendencia_fechamento_aula pfa on pf.id = pfa.pendencia_fechamento_id ");
            sql.AppendLine(@"      inner join aula a on a.id = pfa.aula_id ");
            sql.AppendLine(@"      inner join turma t on t.turma_id  = a.turma_id     ");
            sql.AppendLine(@"     where  not p.excluido and not a.excluido  ");
            sql.AppendLine(@"         and t.ano_letivo = @anoLetivoInformado ");
            sql.AppendLine(@"         and t.ue_id = @ueid ");
            sql.AppendLine(@"      and p.tipo = any(@tiposPendenciaFechamento)");
            sql.AppendLine(@"       and p.situacao = any(@situacoesPendencia)");
            sql.AppendLine(@"      group by p.id                           ");
            sql.AppendLine(@"    union all                        ");
            sql.AppendLine(@"     select distinct ");
            sql.AppendLine(@"         p.id as PendenciaId,");
            sql.AppendLine(@"      count(a.data_aula) as QuantidadeDias,");
            sql.AppendLine(@"      sum(a.quantidade) as QuantidadeAula");
            sql.AppendLine(@"  from pendencia_aula pa");
            sql.AppendLine(@"      inner join pendencia p on pa.pendencia_id  = p.id");
            sql.AppendLine(@"      inner join aula a on a.id  = pa.aula_id");
            sql.AppendLine(@"         inner join turma t  on t.turma_id = a.turma_id ");
            sql.AppendLine(@"     where not p.excluido and not a.excluido  ");
            sql.AppendLine(@"      and t.ano_letivo = @anoLetivoInformado ");
            sql.AppendLine(@"      and t.ue_id = @ueid ");
            sql.AppendLine(@"      and p.tipo = any(@tiposPendenciaAula)");
            sql.AppendLine(@"       and p.situacao = any(@situacoesPendencia)");
            sql.AppendLine(@"      group by p.id ");
            sql.AppendLine(" union all     ");
            sql.AppendLine(QuantidadeDiasPendenciasDiarioBordo());
            return await database.Conexao.QueryAsync<AulasDiasPendenciaDto>(sql.ToString(), new { anoLetivoInformado, ueid, situacoesPendencia, tiposPendenciaFechamento, tiposPendenciaDiarioBordo, tiposPendenciaAula }, commandTimeout: 60);
        }

        public async Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioSemFiltro(long usuarioId, Paginacao paginacao)
        {
            const SituacaoPendencia situacao = SituacaoPendencia.Pendente;

            if (paginacao.EhNulo() || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            const string query = @" 
                            SELECT DISTINCT p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em, p.criado_por, p.alterado_em, p.alterado_por
                            FROM pendencia p 
                                INNER JOIN pendencia_perfil pp ON pp.pendencia_id = p.id 
                                INNER JOIN pendencia_perfil_usuario ppu ON ppu.pendencia_perfil_id = pp.id
                            WHERE NOT p.excluido 
                            AND ppu.usuario_id = @usuarioId 
                            AND p.situacao = @situacao
                            AND p.Criado_em > @dataPendencia
            
                            UNION ALL 
            
                            SELECT DISTINCT p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em, p.criado_por, p.alterado_em, p.alterado_por
                            FROM pendencia p 
                                INNER JOIN pendencia_usuario pu ON pu.pendencia_id = p.id
                                INNER JOIN pendencia_aula pa ON p.id = pa.pendencia_id
                            WHERE NOT p.excluido 
                            AND pu.usuario_id = @usuarioId
                            AND p.situacao = @situacao
                            AND p.Criado_em > @dataPendencia

                            UNION ALL
            
                            SELECT DISTINCT p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em, p.criado_por, p.alterado_em, p.alterado_por
                            FROM pendencia p 
                                INNER JOIN pendencia_usuario pu ON pu.pendencia_id = p.id
                                INNER JOIN pendencia_professor pp ON p.id = pp.pendencia_id
                            WHERE NOT p.excluido 
                            AND pu.usuario_id = @usuarioId
                            AND p.situacao = @situacao
                            AND p.Criado_em > @dataPendencia  
            
                            UNION ALL
            
                            SELECT DISTINCT p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em, p.criado_por, p.alterado_em, p.alterado_por
                            FROM pendencia p 
                                INNER JOIN pendencia_usuario pu ON pu.pendencia_id = p.id
                                INNER JOIN pendencia_fechamento pf ON p.id = pf.pendencia_id
                            WHERE NOT p.excluido 
                            AND pu.usuario_id = @usuarioId
                            AND p.situacao = @situacao
                            AND p.Criado_em > @dataPendencia
            
                            UNION ALL
            
                            SELECT DISTINCT p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em, p.criado_por, p.alterado_em, p.alterado_por
                            FROM pendencia p 
                                INNER JOIN pendencia_usuario pu ON pu.pendencia_id = p.id
                                INNER JOIN pendencia_diario_bordo pdb ON p.id = pdb.pendencia_id
                                INNER JOIN aula a ON a.id = pdb.aula_id
                            WHERE NOT p.excluido 
                            AND NOT a.excluido
                            AND pu.usuario_id = @usuarioId
                            AND p.situacao = @situacao 
                            AND p.Criado_em > @dataPendencia
            
                            UNION ALL
            
                            SELECT DISTINCT p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em, p.criado_por, p.alterado_em, p.alterado_por
                            FROM pendencia p 
                                INNER JOIN pendencia_usuario pu ON pu.pendencia_id = p.id
                                INNER JOIN pendencia_devolutiva pd ON p.id = pd.pendencia_id
                            WHERE NOT p.excluido 
                            AND pu.usuario_id = @usuarioId
                            AND p.situacao = @situacao 
                            AND p.Criado_em > @dataPendencia
            
                            UNION ALL
            
                            SELECT DISTINCT p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em, p.criado_por, p.alterado_em, p.alterado_por
                            FROM pendencia p                                                      
                                INNER JOIN pendencia_encaminhamento_aee eaee ON eaee.pendencia_id = p.id
                                INNER JOIN encaminhamento_aee aee ON eaee.encaminhamento_aee_id = aee.id
                                INNER JOIN pendencia_usuario pu2 ON p.id = pu2.pendencia_id AND pu2.usuario_id = @usuarioId
                            WHERE NOT p.excluido
                            AND aee.responsavel_id = @usuarioId
                            AND p.situacao = @situacao
                            AND p.Criado_em > @dataPendencia
            
                            UNION ALL
            
                            SELECT DISTINCT p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em, p.criado_por, p.alterado_em, p.alterado_por
                            FROM pendencia p 
                                INNER JOIN pendencia_usuario pu ON pu.pendencia_id = p.id
                                INNER JOIN pendencia_plano_aee ppaee ON p.id = ppaee.pendencia_id
                            WHERE NOT p.excluido 
                            AND pu.usuario_id = @usuarioId
                            AND p.situacao = @situacao
                            AND p.Criado_em > @dataPendencia";

            var parametros = new
            {
                usuarioId,
                situacao,
                dataPendencia = new DateTime(DateTime.Now.Year - 1, 1, 1)
            };

            var resultado = await database.Conexao.QueryAsync(query, parametros);

            var retornoPaginado = new PaginacaoResultadoDto<Pendencia>();

            if (!resultado.Any())
            {
                retornoPaginado.Items = new List<Pendencia>();
                retornoPaginado.TotalRegistros = 0;
                retornoPaginado.TotalPaginas = 0;
                return retornoPaginado;
            }

            // Paginação feita em código porque estava causando timeout no banco
            var paginaPendencias = resultado
                            .OrderByDescending(p => p.CriadoEm)
                            .Skip(paginacao.QuantidadeRegistrosIgnorados)
                            .Take(paginacao.QuantidadeRegistros)
                            .ToList();

            var totalRegistros = resultado.Count();

            retornoPaginado.Items = paginaPendencias.Select(r => new Pendencia
            {
                Id = r?.id,
                Titulo = r?.titulo,
                Descricao = r?.descricao,
                Situacao = (SituacaoPendencia)r?.situacao,
                Tipo = (TipoPendencia)r?.tipo,
                CriadoEm = r?.criado_em,
                CriadoPor = r?.criado_por,
                AlteradoEm = r?.alterado_em,
                AlteradoPor = r?.alterado_por
            }).ToList();

            retornoPaginado.TotalRegistros = totalRegistros;
            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
        }

        public async Task<IEnumerable<Pendencia>> ObterPorIdsAsync(long[] pendenciasId)
        {
            var query = "select * from pendencia where id = any(@pendenciasId);";
            return await database.Conexao.QueryAsync<Pendencia>(query, new { pendenciasId });
        }

        private static string QuantidadeDiasPendenciasDiarioBordo()
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"select distinct ");
            sql.AppendLine(@"       p.id  as PendenciaId,");
            sql.AppendLine(@"       count(a.data_aula) as QuantidadeDias,");
            sql.AppendLine(@"    sum(a.quantidade) as QuantidadeAula");
            sql.AppendLine(@"from pendencia p");
            sql.AppendLine(@"inner join pendencia_diario_bordo pdb on pdb.pendencia_id = p.id ");
            sql.AppendLine(@"join aula a on a.id = pdb.aula_id");
            sql.AppendLine(@"join turma t on t.turma_id = a.turma_id ");
            sql.AppendLine(@"where t.ano_letivo = @anoLetivoInformado ");
            sql.AppendLine(@"    and t.ue_id = @ueid ");
            sql.AppendLine(@"    and p.tipo = any(@tiposPendenciaDiarioBordo)");
            sql.AppendLine(@"    and p.situacao = any(@situacoesPendencia)");
            sql.AppendLine(@"group by p.id  ");
            return sql.ToString();
        }
    }
}