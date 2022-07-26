using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PendenciaPendente;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendencia : RepositorioBase<Pendencia>, IRepositorioPendencia
    {
        public RepositorioPendencia(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public void ExclusaoLogicaPendencia(long pendenciaId)
        {
            database.Conexao.Execute("update pendencia set excluido = true where id = @pendenciaId", new { pendenciaId });
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

            var query = @" select distinct * from ( select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em as CriadoEm
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
                                                      inner join pendencia_usuario pu2 on p.id = pu2.pendencia_id and pu2.usuario_id = :usuarioId
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
            string queryPendencias = string.Empty;
            IEnumerable<long> pendenciasFiltradas;
            var pendenciasRetorno = Enumerable.Empty<Pendencia>();
            var query = @" select distinct * from ( select distinct p.id, p.titulo, p.descricao, p.situacao, p.tipo, p.criado_em as CriadoEm
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
                                                       and p.situacao = @situacao) t order by CriadoEm desc";

           var pendenciasPerfilUsuarioDto = await database.Conexao
                                                            .QueryAsync<PendenciaPerfilUsuarioDashboardDto>(query, new { situacao, usuarioId, tiposPendencias }, commandTimeout: 300);


            if (!string.IsNullOrEmpty(turmaCodigo) && tiposPendencias.Count() == 0)
            {
                var queryFiltrada = MontaQueryTurmaFiltrada(turmaCodigo, usuarioId, situacao, pendenciasPerfilUsuarioDto);
                try
                {
                    pendenciasFiltradas = await database.Conexao
                                                    .QueryAsync<long>(queryFiltrada, new { pendencias = pendenciasPerfilUsuarioDto.Select(x => x.Id).ToArray(), usuarioId, situacao}, commandTimeout: 300);
                    pendenciasRetorno = await ObterPendenciasPorIds(pendenciasFiltradas, tituloPendencia);
                }
                catch(Exception ex)
                {
                    throw new NegocioException(ex.Message);
                }
            }

            if ((!string.IsNullOrEmpty(turmaCodigo) && tiposPendencias.Any()) || (string.IsNullOrEmpty(turmaCodigo) && tiposPendencias.Any() && tipoGrupo.Value > 0))
            {
                var queryTurma = RetornaQueryTurmaParaUnicoTipo((TipoPendenciaGrupo)tipoGrupo.Value, turmaCodigo);

                pendenciasFiltradas = await database.Conexao
                                                    .QueryAsync<long>(queryTurma, new { pendencias = pendenciasPerfilUsuarioDto.Select(x => x.Id).ToArray(), turmaCodigo }, commandTimeout: 300);

                pendenciasRetorno = await ObterPendenciasPorIds(pendenciasFiltradas, tituloPendencia);
            }

            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var retornoPaginado = new PaginacaoResultadoDto<Pendencia>();

            retornoPaginado.TotalRegistros = pendenciasRetorno.Any() ? pendenciasRetorno.Count() : 0;
            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);
            retornoPaginado.Items = paginacao.QuantidadeRegistros > 0 ? pendenciasRetorno.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros) : pendenciasRetorno;

            return retornoPaginado;
        }

        private async Task<IEnumerable<Pendencia>> ObterPendenciasPorIds(IEnumerable<long> pendenciasFiltradas, string tituloPendencia)
        {
            var query = "select distinct coalesce(p.alterado_em, p.criado_em), p.* from pendencia p where id = any(@pendenciasFiltradas)";

            if (!string.IsNullOrEmpty(tituloPendencia))
                query = $"{query} and UPPER(p.titulo) like UPPER('%" + tituloPendencia + "%')";

            query = $"{query} order by coalesce(p.alterado_em, p.criado_em) desc ";

            return await database.Conexao.QueryAsync<Pendencia>(query, new { pendenciasFiltradas }, commandTimeout: 120);
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

        public string RetornaQueryTurmaParaUnicoTipo(TipoPendenciaGrupo tipoGrupo, string turmaCodigo)
        {
            string query = string.Empty;

            switch (tipoGrupo)
            {
                case TipoPendenciaGrupo.Fechamento:
                    query = $@"
                            select distinct pf.pendencia_id 
                            from pendencia_fechamento pf
                            inner join pendencia p on p.id = pf.pendencia_id
                            LEFT JOIN fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
                            LEFT JOIN fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
                            LEFT JOIN pendencia_professor ppf ON ppf.pendencia_id = pf.pendencia_id
                            INNER JOIN turma t ON t.id = coalesce(ft.turma_id, ppf.turma_id) 
                            where pf.pendencia_id = any(@pendencias)
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " AND t.turma_id = @turmaCodigo" : string.Empty)}
                            and p.tipo in (1,2,3,4,5,6,14,15,16)";
                    break;

                case TipoPendenciaGrupo.Calendario:
                    query = $@"  
                            select distinct pa.pendencia_id 
                            from pendencia_aula pa
                            inner join pendencia p on p.id = pa.pendencia_id 
                            LEFT JOIN pendencia_calendario_ue pcu on pcu.pendencia_id = pa.pendencia_id
                            LEFT JOIN tipo_calendario tc on tc.id = pcu.tipo_calendario_id
                            INNER JOIN aula a on a.tipo_calendario_id = tc.id or a.id = pa.aula_id
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " JOIN turma t ON t.turma_id = a.turma_id " : string.Empty)}
                            where pa.pendencia_id = any(@pendencias)
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " AND t.turma_id = @turmaCodigo" : string.Empty)}
                            and p.tipo in (11,12,13)";
                    break;

                case TipoPendenciaGrupo.DiarioClasse:
                    query = $@"
                            select distinct p.id
							from pendencia p 
                            inner join pendencia_usuario pu on pu.pendencia_id = p.id
                            inner join pendencia_aula pa on p.id = pa.pendencia_id
	                        inner join aula a on pa.aula_id = a.id
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " join turma t ON t.turma_id = a.turma_id " : string.Empty)}
                            WHERE pu.pendencia_id = any(@pendencias)
                            and situacao = 1
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " and t.turma_id = @turmaCodigo" : string.Empty)}
                            and p.tipo in (7,8,9,10,17,19) and not p.excluido";
                    break;
                
                case TipoPendenciaGrupo.AEE:
                    query = $@"
                            select distinct p.id
							from pendencia p 
                            inner join pendencia_plano_aee ppa on ppa.pendencia_id = p.id
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " inner join plano_aee pa on pa.id = ppa.plano_aee_id inner join turma t on t.turma_id = pa.turma_id " : string.Empty)}
                            WHERE ppa.pendencia_id = any(@pendencias)
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " AND t.turma_id = @turmaCodigo" : string.Empty)}                            
                            and situacao = 1
                            and p.tipo in (18)";
                    break;
            }

            return query;
        }

        public string MontaQueryTurmaFiltrada(string turmaCodigo, long usuarioId, SituacaoPendencia situacao, IEnumerable<PendenciaPerfilUsuarioDashboardDto> pendencias)
        {
            string query = string.Empty;
            if (!string.IsNullOrEmpty(turmaCodigo))
            {
                query = $@" select distinct pf.pendencia_id  
                            from pendencia_fechamento pf
                            inner join fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
                            inner join fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
                            inner join turma t ON t.id = ft.turma_id
                            where pf.pendencia_id = any(@pendencias)
                            and t.turma_id = '{turmaCodigo}'

                            union all 
                            select distinct ppf.pendencia_id
                            from pendencia_professor ppf
                            inner join pendencia_aula pa ON pa.pendencia_id = ppf.pendencia_id
	                        inner join turma t ON t.id = ppf.turma_id 
	                        where ppf.pendencia_id = any(@pendencias) 
                            AND t.turma_id = '{turmaCodigo}'

                            union all 
                            select distinct pa.pendencia_id
                            from pendencia_aula pa 
                            inner join aula a on a.id = pa.aula_id 
                            inner join turma t ON t.turma_id = a.turma_id 
                            WHERE t.turma_id = '{turmaCodigo}'
                            AND pa.pendencia_id = any(@pendencias)

                            union all 
                            select distinct pdb.pendencia_id
                            from pendencia_diario_bordo pdb 
                            inner join aula a on a.id = pdb.aula_id 
                            inner join turma t ON t.turma_id = a.turma_id 
                            WHERE t.turma_id = '{turmaCodigo}'
                            AND pdb.pendencia_id = any(@pendencias)

                            union all 
                            select distinct pri.pendencia_id 
                            from pendencia_registro_individual pri  
                            inner join turma t ON t.id = pri.turma_id 
                            WHERE t.turma_id = '{turmaCodigo}' 
                            AND pri.pendencia_id = any(@pendencias) 

                            union all
                            select distinct p.id
                                    from pendencia p 
                                    inner join pendencia_encaminhamento_aee eaee ON eaee.pendencia_id = p.id 
                                    inner join encaminhamento_aee aee on eaee.encaminhamento_aee_id = aee.id
                                    where not p.excluido 
                                     and aee.responsavel_id = @usuarioId 
                                     and p.situacao = @situacao
                                     and aee.turma_id = '{turmaCodigo}'
                                     and p.id = any(@pendencias) ";
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
                case TipoPendencia.Avaliacao:
                    if (turmaCodigo == null)
                    {
                        tipoPendenciaAceito = false;
                        break;
                    }
                    else
                    {
                        query += @" LEFT JOIN pendencia_aula pa ON pa.pendencia_id = p.id
                                LEFT JOIN aula a ON a.id = pa.aula_id
                                LEFT JOIN turma t ON t.turma_id = a.turma_id ";
                        break;
                    }

                case TipoPendencia.Devolutiva:
                    query += @" LEFT JOIN pendencia_devolutiva pd ON pd.pendencia_id = p.id
                                LEFT JOIN turma t on t.id = pd.turma_id";
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