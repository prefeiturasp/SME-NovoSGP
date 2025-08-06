using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PendenciaPendente;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TipoPendencia = SME.SGP.Dominio.TipoPendencia;


namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaConsulta : RepositorioBase<Pendencia>, IRepositorioPendenciaConsulta
    {
        public RepositorioPendenciaConsulta(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
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

        public async Task<IEnumerable<long>> FiltrarListaPendenciasUsuario(string turmaCodigo, List<Pendencia> pendencias)
        {
            if (!pendencias.Any())
                return pendencias.Select(s => s.Id).ToList();

            var tiposPendencias = pendencias.Select(c => c.Tipo).Distinct().ToList();

            if (!tiposPendencias.Any())
                return pendencias.Select(s => s.Id).ToList();

            var pendenciasIds = pendencias.Select(c => new { c.Id, c.PendenciaAssunto })
                .Where(c => c.PendenciaAssunto.Equals(TipoPendenciaAssunto.Pendencia))
                .Select(c => c.Id).Distinct().ToArray();

            var pendenciasIdsFechamento = pendencias.Select(c => new { c.Id, c.PendenciaAssunto })
                .Where(c => c.PendenciaAssunto.Equals(TipoPendenciaAssunto.PendenciaFechamento))
                .Select(c => c.Id).Distinct().ToArray();

            var pendenciasIdsAula = pendencias.Select(c => new { c.Id, c.PendenciaAssunto })
                .Where(c => c.PendenciaAssunto.Equals(TipoPendenciaAssunto.PendenciaAula))
                .Select(c => c.Id).Distinct().ToArray();

            var pendenciasIdsCalendario = pendencias.Select(c => new { c.Id, c.PendenciaAssunto })
                .Where(c => c.PendenciaAssunto.Equals(TipoPendenciaAssunto.PendenciaCalendario))
                .Select(c => c.Id).Distinct().ToArray();

            var pendenciasIdsProfessor = pendencias.Select(c => new { c.Id, c.PendenciaAssunto })
                .Where(c => c.PendenciaAssunto.Equals(TipoPendenciaAssunto.PendenciaProfessor))
                .Select(c => c.Id).Distinct().ToArray();

            var pendenciasIdsRegistroIndividual = pendencias.Select(c => new { c.Id, c.PendenciaAssunto })
                .Where(c => c.PendenciaAssunto.Equals(TipoPendenciaAssunto.PendenciaRegistroIndividual))
                .Select(c => c.Id).Distinct().ToArray();

            var pendenciasIdsDevolutiva = pendencias.Select(c => new { c.Id, c.PendenciaAssunto })
                .Where(c => c.PendenciaAssunto.Equals(TipoPendenciaAssunto.PendenciaDevolutiva))
                .Select(c => c.Id).Distinct().ToArray();

            const string selectBase = "select p.id from pendencia p";

            var query = new StringBuilder();

            //-> Montando a query
            foreach (var pendenciaAssunto in pendencias.GroupBy(c => c.PendenciaAssunto))
            {
                if (query.Length > 0)
                    query.Append(" union ");

                query.Append(selectBase);

                switch (pendenciaAssunto.Key)
                {
                    case TipoPendenciaAssunto.PendenciaFechamento:
                        query.Append(@" INNER JOIN pendencia_fechamento pf ON pf.pendencia_id = p.id
                            LEFT JOIN fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
                            LEFT JOIN fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
                            LEFT JOIN turma t ON t.id = ft.turma_id ");

                        query.Append(@" WHERE p.id = any(@pendenciasIdsFechamento) ");
                        break;
                    case TipoPendenciaAssunto.PendenciaAula:
                        query.Append(@" INNER JOIN pendencia_aula pa ON pa.pendencia_id = p.id
                            LEFT JOIN aula a ON a.id = pa.aula_id
                            LEFT JOIN turma t ON t.turma_id = a.turma_id ");

                        query.Append(@" WHERE p.id = any(@pendenciasIdsAula) ");
                        break;
                    case TipoPendenciaAssunto.PendenciaCalendario:
                        query.Append(@" INNER JOIN pendencia_calendario_ue pcu on pcu.pendencia_id = p.id
                            LEFT JOIN tipo_calendario tc on tc.id = pcu.tipo_calendario_id
                            LEFT JOIN aula a on a.tipo_calendario_id = tc.id
                            LEFT JOIN turma t on t.turma_id = a.turma_id ");

                        query.Append(@" WHERE p.id = any(@pendenciasIdsCalendario) ");
                        break;
                    case TipoPendenciaAssunto.PendenciaProfessor:
                        query.Append(@" INNER JOIN pendencia_professor pp ON pp.pendencia_id = p.id
                            LEFT JOIN turma t on t.id = pp.turma_id ");

                        query.Append(@" WHERE p.id = any(@pendenciasIdsProfessor) ");
                        break;
                    case TipoPendenciaAssunto.PendenciaRegistroIndividual:
                        query.Append(@" INNER JOIN pendencia_registro_individual pri ON pri.pendencia_id = p.id
                            LEFT JOIN turma t ON t.id = pri.turma_id ");

                        query.Append(@" WHERE p.id = any(@pendenciasIdsRegistroIndividual) ");
                        break;
                    case TipoPendenciaAssunto.PendenciaDevolutiva:
                        query.Append(@" INNER JOIN pendencia_devolutiva pd ON pd.pendencia_id = p.id
                            LEFT JOIN turma t on t.id = pd.turma_id ");

                        query.Append(@" WHERE p.id = any(@pendenciasIdsDevolutiva) ");
                        break;
                    case TipoPendenciaAssunto.Pendencia:
                    default:
                        if (!string.IsNullOrEmpty(turmaCodigo))
                            query.Append(@" INNER JOIN turma t on t.id = p.turma_id");

                        query.Append(@" WHERE p.id = any(@pendenciasIds) AND p.Criado_em > @dataPendencia ");
                        break;
                }

                if (!string.IsNullOrEmpty(turmaCodigo))
                    query.Append(" AND t.turma_id = @turmaCodigo ");
            }

            return await database.Conexao.QueryAsync<long>(query.ToString(), new
            {
                pendenciasIdsFechamento,
                pendenciasIdsAula,
                pendenciasIdsCalendario,
                pendenciasIdsProfessor,
                pendenciasIdsRegistroIndividual,
                pendenciasIdsDevolutiva,
                pendenciasIds,
                turmaCodigo,
                dataPendencia = new DateTime(DateTime.Now.Year - 1, 1, 1)
            });
        }

        public async Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioComFiltro(long usuarioId, int[] tiposPendenciasGrupos,
       string tituloPendencia, string turmaCodigo, Paginacao paginacao)
        {
            const SituacaoPendencia situacao = SituacaoPendencia.Pendente;

            IEnumerable<long> pendenciasFiltradas;

            var pendenciasRetorno = new List<Pendencia>();

            const string query = @" select distinct * from (select distinct p.id, p.titulo, p.descricao, p.situacao,
                                         p.tipo, p.criado_em as CriadoEm
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

            var pendenciasPerfilUsuario = (await database.Conexao
                .QueryAsync<PendenciaPerfilUsuarioDashboardDto>(query, new { usuarioId, situacao },
                    commandTimeout: 300)).ToList();

            //-> Havendo turma para filtro, porém, sem tipo de grupo de pendência definida para filtro:
            if (!string.IsNullOrEmpty(turmaCodigo) && !tiposPendenciasGrupos.Any())
            {
                var queryFiltrada = MontaQueryTurmaFiltrada();
                var idsPendencias = pendenciasPerfilUsuario.Select(c => c.Id).Distinct().ToArray();

                pendenciasFiltradas = await database.Conexao
                    .QueryAsync<long>(queryFiltrada,
                        new { pendencias = idsPendencias, turmaCodigo, usuarioId, situacao }, commandTimeout: 300);

                pendenciasRetorno = (await ObterPendenciasPorIds(pendenciasFiltradas.Distinct().ToArray(), tituloPendencia)).ToList();

            }

            //-> Havendo tipo de pendência definido no filtro com ou sem turma:
            if (tiposPendenciasGrupos.Any() && tiposPendenciasGrupos.Length == 1)
            {
                var queryFiltrada = RetornaQueryParaUnicoTipoPendenciaGrupo((TipoPendenciaGrupo)tiposPendenciasGrupos.FirstOrDefault(), turmaCodigo);
                var idsPendencias = pendenciasPerfilUsuario.Select(c => c.Id).Distinct().ToArray();

                pendenciasFiltradas = await database.Conexao
                    .QueryAsync<long>(queryFiltrada,
                        new { pendencias = idsPendencias, turmaCodigo }, commandTimeout: 300);

                pendenciasRetorno = (await ObterPendenciasPorIds(pendenciasFiltradas.Distinct().ToArray(), tituloPendencia)).ToList();

            }

            //-> Não havendo turma e nem grupos de pendência, porém, com filtro por título
            if (!string.IsNullOrEmpty(tituloPendencia) && !tiposPendenciasGrupos.Any() &&
                string.IsNullOrEmpty(turmaCodigo))
            {
                pendenciasRetorno =
                    (await ObterPendenciasPorIds(pendenciasPerfilUsuario.Select(c => c.Id).Distinct().ToArray(),
                        tituloPendencia)).ToList();
            }

            //-> Retorno paginado
            if (paginacao.EhNulo() || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var retornoPaginado = new PaginacaoResultadoDto<Pendencia>
            {
                TotalRegistros = pendenciasRetorno.Any() ? pendenciasRetorno.Count : 0
            };

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            retornoPaginado.Items = paginacao.QuantidadeRegistros > 0
                ? pendenciasRetorno
                    .Skip(paginacao.QuantidadeRegistrosIgnorados)
                    .Take(paginacao.QuantidadeRegistros)
                : pendenciasRetorno;

            return retornoPaginado;
        }

        private async Task<IEnumerable<Pendencia>> ObterPendenciasPorIds(long[] pendenciasFiltradas, string tituloPendencia)
        {
            var query = "select distinct coalesce(p.alterado_em, p.criado_em), p.* from pendencia p where id = any(@pendenciasFiltradas)";

            if (!string.IsNullOrEmpty(tituloPendencia))
                query = $"{query} and UPPER(p.titulo) like UPPER('%" + tituloPendencia + "%')";

            query = $"{query} order by coalesce(p.alterado_em, p.criado_em) desc ";

            return await database.Conexao.QueryAsync<Pendencia>(query, new { pendenciasFiltradas }, commandTimeout: 120);
        }

        private static string RetornaQueryParaUnicoTipoPendenciaGrupo(TipoPendenciaGrupo tipoPendenciaGrupo, string turmaCodigo)
        {
            var query = tipoPendenciaGrupo switch
            {
                TipoPendenciaGrupo.Fechamento => $@"
                            select distinct pf.pendencia_id 
                            from pendencia_fechamento pf
                                inner join pendencia p on p.id = pf.pendencia_id
                                LEFT JOIN fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
                                LEFT JOIN fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
                                LEFT JOIN pendencia_professor ppf ON ppf.pendencia_id = pf.pendencia_id
                                {(!string.IsNullOrEmpty(turmaCodigo) ? " INNER JOIN turma t ON t.id = coalesce(ft.turma_id, ppf.turma_id) " : string.Empty)} 
                            where pf.pendencia_id = any(@pendencias)
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " AND t.turma_id = @turmaCodigo" : string.Empty)}
                            and p.tipo in (1,2,3,4,5,6,14,15,16)
                            and not p.excluido",
                TipoPendenciaGrupo.Calendario => $@"
                            select distinct pa.pendencia_id 
                            from pendencia_aula pa
                                inner join pendencia p on p.id = pa.pendencia_id 
                                LEFT JOIN pendencia_calendario_ue pcu on pcu.pendencia_id = pa.pendencia_id
                                LEFT JOIN tipo_calendario tc on tc.id = pcu.tipo_calendario_id
                                INNER JOIN aula a on a.tipo_calendario_id = tc.id or a.id = pa.aula_id
                                {(!string.IsNullOrEmpty(turmaCodigo) ? " JOIN turma t ON t.turma_id = a.turma_id " : string.Empty)}
                            where pa.pendencia_id = any(@pendencias)
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " AND t.turma_id = @turmaCodigo" : string.Empty)}
                            and p.tipo in (11,12,13)
                            and not p.excluido",
                TipoPendenciaGrupo.DiarioClasse => $@"
                            select distinct p.id
                            from pendencia p 
                                inner join pendencia_usuario pu on pu.pendencia_id = p.id
                                inner join pendencia_aula pa on p.id = pa.pendencia_id
                                inner join aula a on pa.aula_id = a.id
                                {(!string.IsNullOrEmpty(turmaCodigo) ? " join turma t ON t.turma_id = a.turma_id " : string.Empty)}
                            WHERE pu.pendencia_id = any(@pendencias) and p.turma_id is null
                            and situacao = 1
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " and t.turma_id = @turmaCodigo" : string.Empty)}
                            and p.tipo in (7,8,9,10,17,19,20)
                            and not p.excluido
                            UNION ALL
                            select distinct p.id
                            from pendencia p 
                                inner join pendencia_usuario pu on pu.pendencia_id = p.id
                                {(!string.IsNullOrEmpty(turmaCodigo) ? " join turma t ON t.id = p.turma_id " : string.Empty)}
                            WHERE pu.pendencia_id = any(@pendencias) and p.turma_id is not null
                            and situacao = 1
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " and t.turma_id = @turmaCodigo" : string.Empty)}
                            and p.tipo in (7,8,9,10,17,19,20)
                            and not p.excluido ",
                TipoPendenciaGrupo.AEE => $@"
                            select distinct p.id
                            from pendencia p 
                                inner join pendencia_plano_aee ppa on ppa.pendencia_id = p.id
                                {(!string.IsNullOrEmpty(turmaCodigo) ? " inner join plano_aee pa on pa.id = ppa.plano_aee_id inner join turma t on t.id = pa.turma_id " : string.Empty)}
                            WHERE ppa.pendencia_id = any(@pendencias)
                            {(!string.IsNullOrEmpty(turmaCodigo) ? " AND t.turma_id = @turmaCodigo" : string.Empty)}
                            and p.situacao = 1
                            and p.tipo in (18)
                            and not p.excluido",
                _ => string.Empty
            };

            return query;
        }

        private static string MontaQueryTurmaFiltrada()
        {
            const string query = @" select distinct pf.pendencia_id  
                                    from pendencia_fechamento pf
                                    inner join fechamento_turma_disciplina ftd ON ftd.id = pf.fechamento_turma_disciplina_id
                                    inner join fechamento_turma ft ON ft.id = ftd.fechamento_turma_id
                                    inner join turma t ON t.id = ft.turma_id
                                    where pf.pendencia_id = any(@pendencias)
                                    and t.turma_id = @turmaCodigo

                                    union all 
                                    select distinct ppf.pendencia_id
                                    from pendencia_professor ppf
                                    left join pendencia_aula pa ON pa.pendencia_id = ppf.pendencia_id
                                    inner join turma t ON t.id = ppf.turma_id 
                                    where ppf.pendencia_id = any(@pendencias) 
                                    AND t.turma_id = @turmaCodigo
                                    
                                    union all 
                                    select distinct pa.pendencia_id
                                    from pendencia_aula pa 
                                    inner join aula a on a.id = pa.aula_id 
                                    inner join turma t ON t.turma_id = a.turma_id 
                                    WHERE pa.pendencia_id = any(@pendencias)
                                    and t.turma_id = @turmaCodigo
                                    
                                    union all 
                                    select distinct pdb.pendencia_id
                                    from pendencia_diario_bordo pdb 
                                    inner join aula a on a.id = pdb.aula_id 
                                    inner join turma t ON t.turma_id = a.turma_id 
                                    WHERE pdb.pendencia_id = any(@pendencias)
                                    and t.turma_id = @turmaCodigo
                                    
                                    union all 
                                    select distinct pri.pendencia_id 
                                    from pendencia_registro_individual pri  
                                    inner join turma t ON t.id = pri.turma_id 
                                    WHERE pri.pendencia_id = any(@pendencias)
                                    and t.turma_id = @turmaCodigo
                                    
                                    union all
                                    select distinct p.id
                                    from pendencia p 
                                    inner join pendencia_encaminhamento_aee eaee ON eaee.pendencia_id = p.id 
                                    inner join encaminhamento_aee aee on eaee.encaminhamento_aee_id = aee.id
                                    inner join turma t on t.id = aee.turma_id
                                    where not p.excluido 
                                    and aee.responsavel_id = @usuarioId
                                    and p.situacao = @situacao
                                    and t.turma_id = @turmaCodigo
                                    and p.id = any(@pendencias) ";

            return query;
        }
    }
}
