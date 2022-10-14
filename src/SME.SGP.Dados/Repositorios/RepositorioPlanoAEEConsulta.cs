using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEEConsulta : RepositorioBase<PlanoAEE>, IRepositorioPlanoAEEConsulta
    {
        public RepositorioPlanoAEEConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, string[] turmasCodigos, Paginacao paginacao)
        {          
            var query = MontaQueryCompleta(paginacao, dreId, ueId, turmaId, alunoCodigo, situacao, turmasCodigos);

            var parametros = new { dreId, ueId, turmaId, alunoCodigo, situacao, turmasCodigos};
            var retorno = new PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<PlanoAEEAlunoTurmaDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;

        }

        private string MontaQueryCompleta(Paginacao paginacao, long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, string[] turmasCodigos)
        {                        
            
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, ueId, turmaId, alunoCodigo, situacao, turmasCodigos);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, ueId, turmaId, alunoCodigo, situacao, turmasCodigos);

            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long ueId, long turmaId, string alunoCodigo, int? situacao, string[] turmasCodigos)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, ueId, turmaId, alunoCodigo, situacao, turmasCodigos);

            if (!contador)
            {
                sql.AppendLine("group by pa.id ");
                sql.AppendLine("       , pa.aluno_codigo");
                sql.AppendLine("       , pa.aluno_numero");
                sql.AppendLine("       , pa.aluno_nome");
                sql.AppendLine("       , t.turma_id");
                sql.AppendLine("       , t.nome");
                sql.AppendLine("       , t.modalidade_codigo");
                sql.AppendLine("       , t.ano_letivo");
                sql.AppendLine("       , ea.id ");
                sql.AppendLine("       , pa.situacao ");
                sql.AppendLine("       , pa.criado_em");
                sql.AppendLine("       , usu_responsavel.rf_codigo");
                sql.AppendLine("       , usu_responsavel.nome");
                sql.AppendLine("       , usu_paai_responsavel.rf_codigo");
                sql.AppendLine("       , usu_paai_responsavel.nome");
                sql.AppendLine("        order by pa.aluno_nome ");
            }

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
            if (contador)
                sql.AppendLine(" count(distinct pa.id) ");
            else
            {
                sql.AppendLine("  pa.id ");
                sql.AppendLine(", pa.aluno_codigo as AlunoCodigo ");
                sql.AppendLine(", pa.aluno_numero as AlunoNumero ");
                sql.AppendLine(", pa.aluno_nome as AlunoNome ");
                sql.AppendLine(", t.turma_id as TurmaCodigo ");
                sql.AppendLine(", t.nome as TurmaNome ");
                sql.AppendLine(", t.modalidade_codigo as TurmaModalidade ");
                sql.AppendLine(", t.ano_letivo as TurmaAno ");
                sql.AppendLine(", CASE ");
                sql.AppendLine("    WHEN ea.id = 0 THEN 0 ");
                sql.AppendLine("    WHEN ea.id > 0  THEN 1 ");
                sql.AppendLine("  END as PossuiEncaminhamentoAEE ");
                sql.AppendLine(", pa.situacao ");
                sql.AppendLine(", pa.criado_em as CriadoEm ");
                sql.AppendLine(", max(pav.numero) as Versao ");
                sql.AppendLine(", max(pav.criado_em) as DataVersao ");
                sql.AppendLine(", usu_responsavel.rf_codigo RfReponsavel ");
                sql.AppendLine(", usu_responsavel.nome NomeReponsavel ");
                sql.AppendLine(", usu_paai_responsavel.rf_codigo RfPaaiReponsavel ");
                sql.AppendLine(", usu_paai_responsavel.nome NomePaaiReponsavel ");
            }

            sql.AppendLine(" from plano_aee pa ");
            sql.AppendLine(" left join encaminhamento_aee ea on ea.aluno_codigo = pa.aluno_codigo and not ea.excluido and ea.situacao not in(4,5,7,8) ");
            sql.AppendLine(" inner join turma t on t.id = pa.turma_id");
            sql.AppendLine(" inner join ue on t.ue_id = ue.id");
            sql.AppendLine(" inner join plano_aee_versao pav on pa.id = pav.plano_aee_id");
            sql.AppendLine(" left join usuario usu_responsavel on usu_responsavel.id = pa.responsavel_id");
            sql.AppendLine(" left join usuario usu_paai_responsavel on usu_paai_responsavel.id = pa.responsavel_paai_id");
        }

        private void ObtenhaFiltro(StringBuilder sql, long ueId, long turmaId, string alunoCodigo, int? situacao, string[] turmasCodigos)
        {
            sql.AppendLine(" where ue.dre_id = @dreId and not pa.excluido ");

            if (ueId > 0)
                sql.AppendLine(" and ue.id = @ueId ");
            if (turmaId > 0)
                sql.AppendLine(" and t.id = @turmaId ");
            if (!string.IsNullOrEmpty(alunoCodigo))
                sql.AppendLine(" and pa.aluno_codigo = @alunoCodigo ");
            if (situacao.HasValue && situacao > 0)
                sql.AppendLine(" and pa.situacao = @situacao ");
            if (turmasCodigos.Length > 0)
                sql.AppendLine(" and t.turma_id = ANY(@turmasCodigos) ");
        }

        public async Task<PlanoAEEResumoDto> ObterPlanoPorEstudante(string codigoEstudante)
        {
            var query = @"select distinct   pa.Id,
	                                        pa.aluno_numero as numero,
	                                        pa.aluno_nome as nome,
	                                        tu.nome as turma,
	                                        pa.situacao 
                                        from plano_aee pa
                                        inner join turma tu on tu.id = pa.turma_id 
                                        where pa.aluno_codigo = @codigoEstudante 
                                        and not pa.situacao = any(@situacoesDesconsideradas)
                                        and not pa.excluido
                                        limit 1";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAEEResumoDto>(query, new
            {
                codigoEstudante,
                situacoesDesconsideradas = new int[] { (int)SituacaoPlanoAEE.Encerrado, (int)SituacaoPlanoAEE.EncerradoAutomaticamente }
            });
        }

        public async Task<PlanoAEEResumoDto> ObterPlanoPorEstudanteEAno(string codigoEstudante, int ano)
        {
            var query = @"select distinct   pa.Id,
	                                        pa.aluno_numero as numero,
	                                        pa.aluno_nome as nome,
	                                        tu.nome as turma,
	                                        pa.situacao 
                                        from plano_aee pa
                                        inner join turma tu on tu.id = pa.turma_id 
                                        where pa.aluno_codigo = @codigoEstudante 
                                        and pa.situacao not in (3,7)
                                        and EXTRACT(ISOYEAR from pa.criado_em) = @ano 
                                        limit 1";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAEEResumoDto>(query, new { codigoEstudante, ano });
        }
        public async Task<IEnumerable<PlanoAEEResumoDto>> ObterPlanosPorAlunosEAno(string[] codigoEstudante, int ano)
        {
            var query = @"select distinct   pa.Id,
	                                        pa.aluno_numero as numero,
	                                        pa.aluno_nome as nome,
	                                        tu.nome as turma,
	                                        pa.situacao,
	                                        pa.aluno_codigo as CodigoAluno 
                                        from plano_aee pa
                                        inner join turma tu on tu.id = pa.turma_id 
                                        where pa.aluno_codigo = any(@codigoEstudante) 
                                        and pa.situacao not in (3,7)
                                        and EXTRACT(ISOYEAR from pa.criado_em) = @ano 
                                        limit 1";

            return await database.Conexao.QueryAsync<PlanoAEEResumoDto>(query, new { codigoEstudante, ano });
        }

        public async Task<PlanoAEE> ObterPlanoComTurmaPorId(long planoId)
        {
            var query = @" select pa.*, t.*, ue.*
                            from plano_aee pa
                           inner join turma t on t.id = pa.turma_id
                           inner join ue on ue.id = t.ue_id
                           where pa.id = @planoId";

            return (await database.Conexao.QueryAsync<PlanoAEE, Turma, Ue, PlanoAEE>(query,
                (planoAEEDto, turma, ue) =>
                {
                    turma.Ue = ue;
                    planoAEEDto.Turma = turma;

                    return planoAEEDto;
                }, new { planoId })).FirstOrDefault();
        }

        public async Task<IEnumerable<PlanoAEE>> ObterPlanosAtivos()
        {
            var query = @"select * from plano_aee where not excluido and situacao not in (3,7)";

            return await database.Conexao.QueryAsync<PlanoAEE>(query);
        }

        public async Task<IEnumerable<PlanoAEE>> ObterPorDataFinalVigencia(DateTime dataFim, bool desconsiderarPendencias = true, bool desconsiderarNotificados = false, NotificacaoPlanoAEETipo tipoNotificacao = NotificacaoPlanoAEETipo.PlanoCriado)
        {
            var joinPendecias = desconsiderarPendencias ? @"left join pendencia_plano_aee ppa on ppa.plano_aee_id = pa.id
                                                            left join pendencia p on ppa.pendencia_id = p.id and not p.excluido" : string.Empty;
            var joinNotificacoes = desconsiderarNotificados ? "left join notificacao_plano_aee npa on npa.plano_aee_id = pa.id and npa.tipo = @tipoNotificacao" : string.Empty;

            var condicaoPendencias = desconsiderarPendencias ? $"and (ppa.id is null or p.id is null or p.situacao = {(int)SituacaoPendencia.Resolvida})" : string.Empty;
            var condicaoNotificacoes = desconsiderarNotificados ? "and npa.id is null" : string.Empty;

            var query = $@"select pa.* 
                          from plano_aee pa
                         inner join plano_aee_versao pav on pav.id in (select max(id) from plano_aee_versao where plano_aee_id = pa.id)
                         inner join plano_aee_questao paq on paq.plano_aee_versao_id = pav.id
                         inner join questao q on q.id = paq.questao_id and q.ordem = 1 and q.tipo = @tipoQuestao
                         inner join plano_aee_resposta par on par.plano_questao_id = paq.id
                         inner join periodo_escolar pe on pe.id = par.texto::bigint
                          {joinPendecias}
                          {joinNotificacoes}
                         where pe.periodo_fim <= @dataFim
                           and pa.situacao = @situacaoPlano
                           {condicaoPendencias}
                           {condicaoNotificacoes}";

            return await database.Conexao.QueryAsync<PlanoAEE>(query, new { dataFim, tipoNotificacao, tipoQuestao = (int)TipoQuestao.PeriodoEscolar, situacaoPlano = (int)SituacaoPlanoAEE.Validado });
        }

        public async Task<IEnumerable<PlanoAEEReduzidoDto>> ObterPlanosAEEAtivosComTurmaEVigencia()
        {
            var query = @"select
	                    pa.id as Id,
	                    pa.aluno_nome as EstudanteNome,
	                    pa.aluno_codigo as EstudanteCodigo,
	                    t.nome as TurmaNome,
	                    dre.dre_id as DRECodigo,
                        dre.abreviacao as DREAbreviacao,
	                    t.modalidade_codigo as TurmaModalidade,
	                    ue.ue_id as UECodigo,
	                    ue.nome as UENome,
	                    ue.tipo_escola as UETipo,
	                    pa.situacao as Situacao,
	                    pav.numero as VersaoNumero,
	                    par.periodo_inicio as VigenciaInicio,
                        par.periodo_fim as VigenciaFim
                    from
	                    plano_aee pa
                    inner join turma t on
	                    pa.turma_id = t.id
                    inner join ue on
	                    t.ue_id = ue.id
                    inner join dre on 
	                    dre.id = ue.dre_id 
                    inner join plano_aee_versao pav on
	                    pav.id in (select max(id) from plano_aee_versao where plano_aee_id = pa.id)
                    inner join plano_aee_questao paq on
	                    pav.id = paq.plano_aee_versao_id
                    inner join plano_aee_resposta par on
	                    paq.id = par.plano_questao_id
                    where
	                    par.periodo_inicio is not null
	                    and pa.situacao not in (3,
	                    7)
                    order by dre.dre_id, ue.nome, t.nome ";

            return await database.Conexao.QueryAsync<PlanoAEEReduzidoDto>(query);
        }

        public async Task<PlanoAEE> ObterPorReestruturacaoId(long reestruturacaoId)
        {
            var query = @"select pa.*, t.*, ue.*, dre.*
                         from plano_aee pa
                        inner join plano_aee_versao pav on pav.plano_aee_id = pa.id 
                        inner join plano_aee_reestruturacao par on par.plano_aee_versao_id = pav.id
                        inner join turma t on t.id = pa.turma_id 
                        inner join ue on ue.id = t.ue_id 
                        inner join dre on dre.id = ue.dre_id 
                        where par.id = @reestruturacaoId";

            return (await database.Conexao.QueryAsync<PlanoAEE, Turma, Ue, Dre, PlanoAEE>(query,
                (planoAEE, turma, ue, dre) =>
                {
                    ue.Dre = dre;
                    turma.Ue = ue;
                    planoAEE.Turma = turma;

                    return planoAEE;
                }, new { reestruturacaoId })).FirstOrDefault();
        }

        public async Task<PlanoAEE> ObterPlanoComTurmaUeDrePorId(long planoId)
        {
            var query = @" select pa.*, t.*, ue.*, dre.*
                            from plano_aee pa
                           inner join turma t on t.id = pa.turma_id
                           inner join ue on ue.id = t.ue_id
                           inner join dre on dre.id = ue.dre_id
                           where pa.id = @planoId";

            return (await database.Conexao.QueryAsync<PlanoAEE, Turma, Ue, Dre, PlanoAEE>(query,
                (planoAEEDto, turma, ue, dre) =>
                {
                    ue.Dre = dre;
                    turma.Ue = ue;
                    planoAEEDto.Turma = turma;
                    return planoAEEDto;
                }, new { planoId })).FirstOrDefault();
        }

        public async Task<IEnumerable<AEESituacaoPlanoDto>> ObterQuantidadeSituacoes(int ano, long dreId, long ueId)
        {
            var sql = new StringBuilder(@"select situacao, count(pa.id) as Quantidade from plano_aee pa ");
            sql.Append(" inner join turma t on pa.turma_id = t.id ");
            sql.Append(" inner join ue on t.ue_id = ue.id ");

            var where = new StringBuilder(@" where t.ano_letivo = @ano ");

            if (dreId > 0)
            {
                sql.Append(" inner join dre on ue.dre_id = dre.id ");
                where.Append(" and dre.id = @dreId");
            }

            if (ueId > 0)
            {
                where.Append(" and ue.id = @ueId");
            }

            sql.Append(where.ToString());

            sql.Append(" group by pa.situacao ");

            return await database.Conexao.QueryAsync<AEESituacaoPlanoDto>(sql.ToString(), new { ano, dreId, ueId });
        }

        public async Task<IEnumerable<AEETurmaDto>> ObterQuantidadeVigentes(int ano, long dreId, long ueId)
        {
            var sql = new StringBuilder(@"select t.nome, t.modalidade_codigo as Modalidade, count(pa.id) as Quantidade, t.ano as AnoTurma ");
            sql.Append(" from plano_aee pa ");

            sql.Append(" inner join turma t on pa.turma_id = t.id ");
            sql.Append(" inner join ue on t.ue_id = ue.id ");


            var where = new StringBuilder(@" where t.ano_letivo = @ano and pa.situacao in (1,2,8)");

            if (dreId > 0)
            {
                sql.Append(" inner join dre on ue.dre_id = dre.id ");
                where.Append(" and dre.id = @dreId");
            }

            if (ueId > 0)
            {
                where.Append(" and ue.id = @ueId");
            }

            sql.Append(where.ToString());
            sql.Append(" group by t.modalidade_codigo, t.nome, t.ano  ");

            return (await database.Conexao.QueryAsync<AEETurmaDto>(sql.ToString(), new { ano, dreId, ueId }))
                .OrderBy(a => a.Ordem).ThenBy(a => a.Descricao);
        }

        public async Task<IEnumerable<AEEAcessibilidateDto>> ObterQuantidadeAcessibilidades(int ano, long dreId, long ueId)
        {
            var sql = new StringBuilder(@"select 
                case when q.ordem = 7 then 'Regular' else 'SRM' end as Descricao,
                or2.nome as opcao, count(par.id) as quantidade from plano_aee pa ");
            sql.Append(" inner join turma t on pa.turma_id = t.id ");
            sql.Append(" inner join ue on t.ue_id = ue.id ");
            sql.Append(" inner join plano_aee_questao paq on paq.plano_aee_versao_id = (select max(id) from plano_aee_versao where plano_aee_id = pa.id) ");
            sql.Append(" inner join questao q on paq.questao_id = q.id ");
            sql.Append(" inner join plano_aee_resposta par on paq.id = par.plano_questao_id  ");
            sql.Append(" inner join opcao_resposta or2 on par.resposta_id = or2.id ");

            var where = new StringBuilder(@" where t.ano_letivo = @ano and q.ordem in (7,8)");

            if (dreId > 0)
            {
                sql.Append(" inner join dre on ue.dre_id = dre.id ");
                where.Append(" and dre.id = @dreId");
            }

            if (ueId > 0)
            {
                where.Append(" and ue.id = @ueId");
            }

            sql.Append(where.ToString());

            sql.Append(" group by or2.nome, q.ordem ");

            return await database.Conexao.QueryAsync<AEEAcessibilidateDto>(sql.ToString(), new { ano, dreId, ueId });
        }

        public async Task<IEnumerable<PlanoAEE>> ObterPlanosEncerradosAutomaticamente(int pagina, int quantidadeRegistrosPagina)
        {
            var query = @"select * from plano_aee 
                          where not excluido and 
                                situacao = @situacao
                          limit @quantidadeRegistrosPagina
                          offset(@pagina - 1) * @quantidadeRegistrosPagina;";

            return await database.Conexao.QueryAsync<PlanoAEE>(query, new { situacao = (int)SituacaoPlanoAEE.EncerradoAutomaticamente, pagina, quantidadeRegistrosPagina });
        }

        public async Task<Pendencia> ObterUltimaPendenciaPlano(long planoId)
        {
            var query = @"select p.*
	                        from pendencia_plano_aee ppa
		                        inner join pendencia p 
			                        on ppa.pendencia_id = p.id
                          where ppa.plano_aee_id = @planoId and
	                        not p.excluido
                          order by ppa.id desc
                          limit 1;";

            return (await database.Conexao.QueryAsync<Pendencia>(query, new { planoId })).SingleOrDefault();
        }
    }
}
