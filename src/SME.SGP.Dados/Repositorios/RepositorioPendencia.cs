using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuario(long usuarioId, int[] tiposPendencias, string tituloPendencia, string turmaCodigo, Paginacao paginacao, int? tipoGrupo)
        {
            var query = @"from pendencia p
                          left join pendencia_perfil pp on pp.pendencia_id = p.id
                          left join pendencia_perfil_usuario ppu on ppu.pendencia_perfil_id = pp.id 
                          left join pendencia_usuario pu on pu.pendencia_id = p.id ";

            if (!string.IsNullOrEmpty(turmaCodigo) && tiposPendencias.Count() == 0)
                query += RetornaQueryTurmaFiltrada(turmaCodigo);

            if (!string.IsNullOrEmpty(turmaCodigo) && tiposPendencias.Count() > 0)
                query += RetornaQueryTurmaParaUnicoTipo((TipoPendenciaGrupo)tipoGrupo.Value, turmaCodigo);

            query += @"where not p.excluido 
                           and (ppu.usuario_id = @usuarioId or pu.usuario_id = @usuarioId)
                           and p.situacao = @situacao";

            if (tiposPendencias.Length > 0)
                query = $"{query} and p.tipo = any(@tiposPendencias) ";

            if (!string.IsNullOrEmpty(tituloPendencia))
                query = $"{query} and UPPER(p.titulo) like UPPER('%" + tituloPendencia + "%')";

            if(!string.IsNullOrEmpty(turmaCodigo))
                query += $" AND t.turma_id = '{turmaCodigo}'";

            var orderBy = "order by coalesce(p.alterado_em, p.criado_em) desc";

            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var situacao = SituacaoPendencia.Pendente;

            var retornoPaginado = new PaginacaoResultadoDto<Pendencia>();
            var queryTotalRegistros = $"select count(distinct p.id) {query}";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(queryTotalRegistros, new { usuarioId, situacao, tiposPendencias, tituloPendencia });

            var queryPendencias = $@"select distinct coalesce(p.alterado_em, p.criado_em), p.* {query} {orderBy}
                    offset @qtde_registros_ignorados rows fetch next @qtde_registros rows only;";

            var parametros = new
            {
                usuarioId,
                qtde_registros_ignorados = paginacao.QuantidadeRegistrosIgnorados,
                qtde_registros = paginacao.QuantidadeRegistros,
                situacao,
                tiposPendencias,
                tituloPendencia
            };

            retornoPaginado.Items = await database.Conexao.QueryAsync<Pendencia>(queryPendencias, parametros);
            retornoPaginado.TotalRegistros = totalRegistrosDaQuery;
            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
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
                    query = @"  LEFT JOIN pendencia_fechamento pf ON pf.pendencia_id = p.id
	                            LEFT JOIN fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
	                            LEFT JOIN fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
                                LEFT JOIN pendencia_professor ppf ON ppf.pendencia_id = p.id
                                LEFT JOIN turma t ON t.id = coalesce(ft.turma_id, ppf.turma_id) ";
                    break;

                case TipoPendenciaGrupo.Calendario:
                    query = @"  LEFT JOIN pendencia_aula pa ON pa.pendencia_id = p.id
                                LEFT JOIN pendencia_calendario_ue pcu on pcu.pendencia_id = p.id
                                LEFT JOIN tipo_calendario tc on tc.id = pcu.tipo_calendario_id
                                LEFT JOIN aula a on a.tipo_calendario_id = tc.id
                                LEFT JOIN turma t on t.turma_id = a.turma_id ";
                    break;

                case TipoPendenciaGrupo.DiarioClasse:
                    query = @"  LEFT JOIN pendencia_registro_individual pri ON pri.pendencia_id = p.id
                                LEFT JOIN turma t ON t.id = pri.turma_id ";
                    break;
            }

            return query;
        }

        public string RetornaQueryTurmaFiltrada(string turmaCodigo)
        {
            string query = string.Empty;

            if (!string.IsNullOrEmpty(turmaCodigo))
            {
                query = @"  LEFT JOIN pendencia_fechamento pf ON pf.pendencia_id = p.id
	                        LEFT JOIN fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
	                        LEFT JOIN fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
                            LEFT JOIN pendencia_professor ppf ON ppf.pendencia_id = p.id
                            LEFT JOIN pendencia_aula pa ON pa.pendencia_id = p.id
                            LEFT JOIN pendencia_calendario_ue pcu on pcu.pendencia_id = p.id
                            LEFT JOIN tipo_calendario tc on tc.id = pcu.tipo_calendario_id
                            LEFT JOIN aula a on a.tipo_calendario_id = tc.id
                            LEFT JOIN pendencia_registro_individual pri ON pri.pendencia_id = p.id
                            INNER JOIN turma t ON t.id = coalesce(ft.turma_id, ppf.turma_id, pri.turma_id) or t.turma_id = a.turma_id ";
            }

            return query;
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