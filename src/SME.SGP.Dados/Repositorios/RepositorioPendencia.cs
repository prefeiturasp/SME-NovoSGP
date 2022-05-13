﻿using Dapper;
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
    public class RepositorioPendencia : RepositorioBase<Pendencia>, IRepositorioPendencia
    {
        public RepositorioPendencia(ISgpContext database) : base(database)
        {
        }

        public void AtualizarPendencias(long fechamentoId, SituacaoPendencia situacaoPendencia, TipoPendencia tipoPendencia)
        {
            var query = @"update pendencia p
                           set situacao = @situacaoPendencia
                         from pendencia_fechamento f
                        where f.pendencia_id = p.id
	                        and p.tipo = @tipoPendencia
                            and f.fechamento_turma_disciplina_id = @fechamentoId                            
                            and not p.excluido ";

            database.Conexao.Execute(query, new { fechamentoId, situacaoPendencia, tipoPendencia });
        }

        public void ExcluirPendenciasFechamento(long fechamentoId, TipoPendencia tipoPendencia)
        {
            var query = @"update pendencia p
                             set excluido = true
                         from pendencia_fechamento f
                        where not p.excluido
                          and p.situacao = 1
                          and p.id = f.pendencia_id
                          and p.tipo = @tipoPendencia
	                      and f.fechamento_turma_disciplina_id = @fechamentoId";

            database.Conexao.Execute(query, new { fechamentoId, tipoPendencia });
        }

        public async Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioSemFiltro(long usuarioId, Paginacao paginacao)
        {
            var situacao = SituacaoPendencia.Pendente;

            var query = @" select * from ( select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em as CriadoEm
		                            from pendencia p 
		                            inner join pendencia_perfil pp on pp.pendencia_id  = p.id 
		                            inner join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id
		                            where not p.excluido 
		                            and ppu.usuario_id = @usuarioId 
		                            and situacao = @situacao
                            union all 
                            select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em as CriadoEm
		                            from pendencia p 
		                            inner join pendencia_usuario pu on pu.pendencia_id = p.id
		                            where not p.excluido 
		                            and pu.usuario_id = @usuarioId 
		                            and situacao = @situacao 
                            union all
                            select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em as CriadoEm
                                                      from pendencia p                                                      
                                                      inner join pendencia_encaminhamento_aee eaee ON eaee.pendencia_id = p.id
                                                      inner join encaminhamento_aee aee on eaee.encaminhamento_aee_id = aee.id
                                                      where not p.excluido
                                                       and aee.responsavel_id = @usuarioId
                                                       and p.situacao = @situacao) t order by CriadoEm desc"

;

            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var retornoPaginado = new PaginacaoResultadoDto<Pendencia>();
            var queryPendencias = await database.Conexao.QueryAsync<Pendencia>(query, new { usuarioId, situacao });
            var totalRegistrosDaQuery = queryPendencias.Count();

            var queryPendenciasPaginado = $@"{query} offset @qtde_registros_ignorados rows fetch next @qtde_registros rows only;";

            var parametros = new
            {
                usuarioId,
                qtde_registros_ignorados = paginacao.QuantidadeRegistrosIgnorados,
                qtde_registros = paginacao.QuantidadeRegistros,
                situacao
            };

            retornoPaginado.Items = await database.Conexao.QueryAsync<Pendencia>(queryPendenciasPaginado, parametros);
            retornoPaginado.TotalRegistros = totalRegistrosDaQuery;
            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
        }

        public async Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioComFiltro(long usuarioId, int[] tiposPendencias, string tituloPendencia, string turmaCodigo, Paginacao paginacao, int? tipoGrupo)
        {
            var situacao = SituacaoPendencia.Pendente;
            bool queryMontadaParaTurma = false;
            int totalRegistrosDaQuery = 0;
            var queryPendencias = new StringBuilder();        
            var queryUsuario = new StringBuilder();
            var where = new StringBuilder();
            var queryTurmaFiltrada = new StringBuilder();

            var queryPerfil = new StringBuilder( @"from pendencia p
                          inner join pendencia_perfil pp on pp.pendencia_id = p.id
                          inner join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id ");
           

            if (!string.IsNullOrEmpty(turmaCodigo) && tiposPendencias.Count() == 0)
            {
                string whereTitulo = string.Empty;
                if (!string.IsNullOrEmpty(tituloPendencia))
                    whereTitulo = $" and UPPER(p.titulo) like UPPER('%" + tituloPendencia + "%')";

                queryTurmaFiltrada.Append(MontaQueryTurmaFiltrada(turmaCodigo, usuarioId, situacao, whereTitulo));
                queryMontadaParaTurma = true;
            }
                
            if (!string.IsNullOrEmpty(turmaCodigo) && tiposPendencias.Count() > 0)
            {
                queryPerfil.AppendLine(RetornaQueryTurmaParaUnicoTipo((TipoPendenciaGrupo)tipoGrupo.Value));
                queryUsuario.AppendLine(RetornaQueryTurmaParaUnicoTipo((TipoPendenciaGrupo)tipoGrupo.Value));
            }

            if (!queryMontadaParaTurma)
            {
                var orderBy = "order by Datas desc";

                where.AppendLine(@"where not p.excluido   
                                        and p.situacao = @situacao");

                if (tiposPendencias.Length > 0)
                    where.AppendLine($" and p.tipo = any(@tiposPendencias) ");

                if (!string.IsNullOrEmpty(tituloPendencia))
                    where.AppendLine( $" and UPPER(p.titulo) like UPPER('%" + tituloPendencia + "%')");

                if (!string.IsNullOrEmpty(turmaCodigo) && (TipoPendenciaGrupo)tipoGrupo.Value != TipoPendenciaGrupo.AEE)
                    where.AppendLine($" AND t.turma_id = '{turmaCodigo}'");

                queryPerfil.AppendLine($" {where} and ppu.usuario_id = @usuarioId");

                queryPendencias.AppendLine(" select * from (");
                queryPendencias.AppendLine($@"select distinct coalesce(p.alterado_em, p.criado_em) Datas, p.* {queryPerfil}");
                queryPendencias.AppendLine(" union all");
                queryPendencias.AppendLine($@"select distinct coalesce(p.alterado_em, p.criado_em) Datas, p.* from pendencia p ");
                queryPendencias.AppendLine($" inner join pendencia_usuario pu on pu.pendencia_id = p.id {queryUsuario} {where} and pu.usuario_id = @usuarioId ) t");    
                queryPendencias.AppendLine($" {orderBy} offset @qtde_registros_ignorados rows fetch next @qtde_registros rows only;");

                var queryTotalRegistros = RetornaQueryTotalRegistros(queryPerfil, where, queryUsuario);

                totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(queryTotalRegistros.ToString(), new { usuarioId, situacao, tiposPendencias, tituloPendencia });
            }

            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var retornoPaginado = new PaginacaoResultadoDto<Pendencia>();
            

            var parametros = new
            {
                usuarioId,
                qtde_registros_ignorados = paginacao.QuantidadeRegistrosIgnorados,
                qtde_registros = paginacao.QuantidadeRegistros,
                situacao,
                tiposPendencias,
                tituloPendencia
            };

            if (queryMontadaParaTurma)
            {        
                var dadosRelatorio = await database.Conexao.QueryAsync<Pendencia>(queryTurmaFiltrada.ToString(), parametros);
                totalRegistrosDaQuery = dadosRelatorio.Count();

                queryTurmaFiltrada.AppendLine(" offset @qtde_registros_ignorados rows fetch next @qtde_registros rows only");
                retornoPaginado.Items = await database.Conexao.QueryAsync<Pendencia>(queryTurmaFiltrada.ToString(), parametros);
            } 
            else
                retornoPaginado.Items = await database.Conexao.QueryAsync<Pendencia>(queryPendencias.ToString(), parametros);

            retornoPaginado.TotalRegistros = totalRegistrosDaQuery;
            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            retornoPaginado.Items = retornoPaginado.Items.Count() > 0 ? retornoPaginado.Items.OrderByDescending(rp => rp.CriadoEm) : retornoPaginado.Items;
            return retornoPaginado;
        }

        public string RetornaQueryTotalRegistros(StringBuilder queryPerfil, StringBuilder where, StringBuilder queryUsuario)
        {
            var queryTotalRegistros = new StringBuilder();
            queryTotalRegistros.AppendLine("select sum (total) TotalPendencias");
            queryTotalRegistros.AppendLine(" from ( ");
            queryTotalRegistros.AppendLine($" select count(distinct p.id) total {queryPerfil}");
            queryTotalRegistros.AppendLine(" union all ");
            queryTotalRegistros.AppendLine($" select count(distinct p.id) total from pendencia p {queryUsuario}");
            queryTotalRegistros.AppendLine($" inner join pendencia_usuario pu on pu.pendencia_id = p.id {where} and pu.usuario_id = @usuarioId ");
            queryTotalRegistros.AppendLine(" ) t ");

            return queryTotalRegistros.ToString();
        }

        public async Task<long[]> ObterIdsPendenciasPorPlanoAEEId(long planoAeeId)
        {
            var query = @"select p.id 
                            from pendencia_plano_aee paee
                            inner join pendencia p on paee.pendencia_id = p.id and p.situacao != 3
                          where paee.plano_aee_id = @planoAeeId";
            var idsPendencias = await database.Conexao.QueryAsync<long>(query, new { planoAeeId });

            return idsPendencias.AsList().ToArray();
        }

        public async Task AtualizarStatusPendenciasPorIds(long[] ids, SituacaoPendencia situacaoPendencia)
        {
            var query = @"update pendencia set situacao = @situacaoPendencia where id =ANY(@ids)";

            await database.Conexao.ExecuteAsync(query, new { ids, situacaoPendencia });
        }

        public async Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasPendentes()
        {
            var query = @"select p.id as PendenciaId, p.ue_id as UeId
                          from pendencia p
                          where p.situacao = @situacao and p.tipo in (18,11,12,13,15)
                   and excluido is false";

            return await database.Conexao.QueryAsync<PendenciaPendenteDto>(query, new { situacao = SituacaoPendencia.Pendente });
        }

        public async Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasSemPendenciaPerfilUsuario()
        {
            var query = @"select distinct pp.pendencia_id as PendenciaId, p.ue_id as UeId from pendencia_perfil pp 
                            inner join pendencia p on p.id = pp.pendencia_id 
                            left join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id 
                            where ppu.id is null and not p.excluido and p.situacao = @situacao and p.ue_id is not null";

            return await database.Conexao.QueryAsync<PendenciaPendenteDto>(query, new { situacao = SituacaoPendencia.Pendente });
        }

        public async Task<int> ObterModalidadePorPendenciaETurmaId(long pendenciaId, long turmaId)
        {
            var query = @"select t.modalidade_codigo from pendencia p
                            inner join ue ue on ue.id = p.ue_id 
                            inner join turma t on t.ue_id = ue.id 
                            where p.id = @pendenciaId and t.id = @turmaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { pendenciaId, turmaId });
        }

        public string RetornaQueryTurmaParaUnicoTipo(TipoPendenciaGrupo tipoGrupo)
        {
            string query = string.Empty;

            switch (tipoGrupo)
            {
                case TipoPendenciaGrupo.Fechamento:
                    query = @"  LEFT JOIN pendencia_fechamento pf ON pf.pendencia_id = p.id
	                            LEFT JOIN fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
	                            LEFT JOIN fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
                                LEFT JOIN pendencia_professor ppf ON ppf.pendencia_id = p.id
                                INNER JOIN turma t ON t.id = coalesce(ft.turma_id, ppf.turma_id) ";
                    break;

                case TipoPendenciaGrupo.Calendario:
                    query = @"  LEFT JOIN pendencia_aula pa ON pa.pendencia_id = p.id
                                LEFT JOIN pendencia_calendario_ue pcu on pcu.pendencia_id = p.id
                                LEFT JOIN tipo_calendario tc on tc.id = pcu.tipo_calendario_id
                                INNER JOIN aula a on a.tipo_calendario_id = tc.id or a.id = pa.aula_id
                                INNER JOIN turma t on t.turma_id = a.turma_id ";
                    break;

                case TipoPendenciaGrupo.DiarioClasse:
                    query = @"  LEFT JOIN pendencia_registro_individual pri ON pri.pendencia_id = p.id
                                INNER JOIN turma t ON t.id = pri.turma_id ";
                    break;
            }

            return query;
        }

        public string MontaQueryTurmaFiltrada(string turmaCodigo, long usuarioId, SituacaoPendencia situacao, string tituloPendencia)
        {
            var query = new StringBuilder();

            if (!string.IsNullOrEmpty(turmaCodigo))
            {
                query.AppendLine($@"select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo 
						                              from pendencia p 
					                                  inner join pendencia_perfil pp on pp.pendencia_id = p.id
                                                      inner join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id   
                                                      inner join pendencia_fechamento pf ON pf.pendencia_id = p.id
	                                                  inner join fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
	                                                  inner join fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
	                                                  inner join turma t ON t.id = ft.turma_id 
	                                                  where not p.excluido 
                                                       and ppu.usuario_id = @usuarioId
                                                       and p.situacao = @situacao AND t.turma_id = '{turmaCodigo}' {tituloPendencia}
                            union all 
                            select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo
                                                      from pendencia p 
					                                  inner join pendencia_perfil pp on pp.pendencia_id = p.id
                                                      inner join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id 
	                                                  inner join pendencia_professor ppf ON ppf.pendencia_id = p.id
                                                      inner join pendencia_aula pa ON pa.pendencia_id = p.id
	                                                  inner join turma t ON t.id = ppf.turma_id 
	                                                  where not p.excluido 
                                                       and ppu.usuario_id = @usuarioId
                                                       and p.situacao = @situacao AND t.turma_id = '{turmaCodigo}' {tituloPendencia}
                            union all 
                            select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo
                                                      from pendencia p 
					                                  inner join pendencia_perfil pp on pp.pendencia_id = p.id
                                                      inner join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id 
						                              inner join pendencia_aula pa ON pa.pendencia_id = p.id
                                                      inner join aula a on a.id = pa.aula_id 
                                                      inner join turma t ON t.turma_id = a.turma_id 
                                                      where not p.excluido 
                                                      and ppu.usuario_id = @usuarioId
                                                      and p.situacao = @situacao AND t.turma_id = '{turmaCodigo}' {tituloPendencia}
                            union all 
                            select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo 
                                                      from pendencia p  
					                                  inner join pendencia_perfil pp on pp.pendencia_id = p.id
                                                      inner join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id 
						                              inner join pendencia_registro_individual pri ON pri.pendencia_id = p.id
                                                      inner join turma t ON t.id = pri.turma_id 
                                                      where not p.excluido 
                                                       and ppu.usuario_id = @usuarioId
                                                       and p.situacao = @situacao AND t.turma_id = '{turmaCodigo}' {tituloPendencia}");
                query.AppendLine(" union all ");
                query.AppendLine($@"select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo 
						                              from pendencia p 
					                                  inner join pendencia_usuario pu on pu.pendencia_id = p.id 
                                                      inner join pendencia_fechamento pf ON pf.pendencia_id = p.id
	                                                  inner join fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
	                                                  inner join fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
	                                                  inner join turma t ON t.id = ft.turma_id 
	                                                  where not p.excluido 
                                                       and pu.usuario_id = @usuarioId
                                                       and p.situacao = @situacao AND t.turma_id = '{turmaCodigo}' {tituloPendencia}
                            union all 
                            select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo
                                                      from pendencia p 
					                                  inner join pendencia_usuario pu on pu.pendencia_id = p.id 
	                                                  inner join pendencia_professor ppf ON ppf.pendencia_id = p.id
                                                      inner join pendencia_aula pa ON pa.pendencia_id = p.id
	                                                  inner join turma t ON t.id = ppf.turma_id 
	                                                  where not p.excluido 
                                                       and pu.usuario_id = @usuarioId
                                                       and p.situacao = @situacao AND t.turma_id = '{turmaCodigo}' {tituloPendencia}
                            union all 
                            select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo
                                                      from pendencia p 
					                                  inner join pendencia_usuario pu on pu.pendencia_id = p.id  
						                              inner join pendencia_aula pa ON pa.pendencia_id = p.id
                                                      inner join aula a on a.id = pa.aula_id 
                                                      inner join turma t ON t.turma_id = a.turma_id 
                                                      where not p.excluido 
                                                       and pu.usuario_id = @usuarioId
                                                       and p.situacao = @situacao AND t.turma_id = '{turmaCodigo}' {tituloPendencia}
                            union all 
                            select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo 
                                                      from pendencia p  
					                                  inner join pendencia_usuario pu on pu.pendencia_id = p.id 
						                              inner join pendencia_registro_individual pri ON pri.pendencia_id = p.id
                                                      inner join turma t ON t.id = pri.turma_id 
                                                      where not p.excluido 
                                                       and pu.usuario_id = @usuarioId
                                                       and p.situacao = @situacao AND t.turma_id = '{turmaCodigo}' {tituloPendencia}
                            union all
                            select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo
                                                      from pendencia p 
                                                      inner join pendencia_encaminhamento_aee eaee ON eaee.pendencia_id = p.id 
                                                      inner join encaminhamento_aee aee on eaee.encaminhamento_aee_id = aee.id
                                                      where not p.excluido 
                                                       and aee.responsavel_id = @usuarioId 
                                                       and p.situacao = @situacao
                                                       and aee.turma_id = '{turmaCodigo}' {tituloPendencia}");
            }

            return query.ToString();
        }

        public async Task<Pendencia> FiltrarListaPendenciasUsuario(string turmaCodigo, Pendencia pendencia)
        {
            var query = @"select p.* from pendencia p";

            bool tipoPendenciaAceito = true;

            switch (pendencia.Tipo)
            {
                case TipoPendencia.AvaliacaoSemNotaParaNenhumAluno:
                case TipoPendencia.AulasReposicaoPendenteAprovacao:
                case TipoPendencia.AulasSemFrequenciaNaDataDoFechamento:
                case TipoPendencia.ResultadosFinaisAbaixoDaMedia:
                    query += @" LEFT JOIN pendencia_fechamento pf ON pf.pendencia_id = p.id
	                            LEFT JOIN fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
	                            LEFT JOIN fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
                                LEFT JOIN turma t ON t.id = ft.turma_id ";

                    break;

                case TipoPendencia.AulaNaoLetivo:

                    query += @" LEFT JOIN pendencia_aula pa ON pa.pendencia_id = p.id
                                LEFT JOIN aula a ON a.id = pa.aula_id
                                LEFT JOIN turma t ON t.turma_id = a.turma_id ";

                    break;

                case TipoPendencia.CalendarioLetivoInsuficiente:
                case TipoPendencia.CadastroEventoPendente:
                    query += @" LEFT JOIN pendencia_calendario_ue pcu on pcu.pendencia_id = p.id
                                LEFT JOIN tipo_calendario tc on tc.id = pcu.tipo_calendario_id
                                LEFT JOIN aula a on a.tipo_calendario_id = tc.id
                                LEFT JOIN turma t on t.turma_id = a.turma_id ";

                    break;

                case TipoPendencia.AusenciaDeAvaliacaoProfessor:
                case TipoPendencia.AusenciaDeAvaliacaoCP:
                case TipoPendencia.AusenciaFechamento:

                    query += @" LEFT JOIN pendencia_professor pp ON pp.pendencia_id = p.id
                                LEFT JOIN turma t on t.id = pp.turma_id ";

                    break;

                case TipoPendencia.AusenciaDeRegistroIndividual:

                    query += @" LEFT JOIN pendencia_registro_individual pri ON pri.pendencia_id = p.id
                                LEFT JOIN turma t ON t.id = pri.turma_id ";

                    break;

                default:

                    tipoPendenciaAceito = false;

                    break;
            }

            if (tipoPendenciaAceito)
                query += $@" WHERE p.id = {pendencia.Id} AND t.turma_id = '{turmaCodigo}'";
            else
                return null;

            return await database.Conexao.QueryFirstOrDefaultAsync<Pendencia>(query);
        }
    }
}