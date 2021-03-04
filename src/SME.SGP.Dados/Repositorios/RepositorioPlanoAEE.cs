using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEE : RepositorioBase<PlanoAEE>, IRepositorioPlanoAEE
    {
        public RepositorioPlanoAEE(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, dreId, ueId, turmaId, alunoCodigo, situacao);

            var parametros = new { dreId, ueId, turmaId, alunoCodigo, situacao };
            var retorno = new PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<PlanoAEEAlunoTurmaDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private static string MontaQueryCompleta(Paginacao paginacao, long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, ueId, turmaId, alunoCodigo, situacao);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, ueId, turmaId, alunoCodigo, situacao);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long ueId, long turmaId, string alunoCodigo, int? situacao)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, ueId, turmaId, alunoCodigo, situacao);

            if (!contador)
                sql.AppendLine(" order by pa.aluno_nome ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
            if (contador)
                sql.AppendLine(" count(pa.id) ");
            else
            {
                sql.AppendLine(" pa.id ");
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
                sql.AppendLine(", pav.numero as Versao ");
                sql.AppendLine(", pav.criado_em as DataVersao ");
            }

            sql.AppendLine(" from plano_aee pa ");
            sql.AppendLine(" left join encaminhamento_aee ea on ea.aluno_codigo = pa.aluno_codigo and not ea.excluido and ea.situacao not in(4,5,7,8) ");
            sql.AppendLine(" inner join turma t on t.id = pa.turma_id");
            sql.AppendLine(" inner join ue on t.ue_id = ue.id");
            sql.AppendLine(" inner join plano_aee_versao pav on pav.id = (select max(id) from plano_aee_versao where plano_aee_id = pa.id)");
        }

        private static void ObtenhaFiltro(StringBuilder sql, long ueId, long turmaId, string alunoCodigo, int? situacao)
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
                                        and pa.situacao = 1
                                        limit 1";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAEEResumoDto>(query, new { codigoEstudante });
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

        public async Task<PlanoAEE> ObterPlanoComTurmaPorId(long planoId)
        {
            var query = @" select pa.*, t.*
                            from plano_aee pa
                           inner join turma t on t.id = pa.turma_id
                           where pa.id = @planoId";

            return (await database.Conexao.QueryAsync<PlanoAEE, Turma, PlanoAEE>(query,
                (planoAEEDto, turma) =>
                {
                    planoAEEDto.Turma = turma;
                    return planoAEEDto;
                }, new { planoId })).FirstOrDefault();
        }

        public async Task<IEnumerable<PlanoAEE>> ObterPlanosAtivos()
        {
            var query = @"select * from plano_aee where not excluido and situacao not in (3,7)";

            return await database.Conexao.QueryAsync<PlanoAEE>(query);
        }

        public async Task<int> AtualizarSituacaoPlanoPorVersao(long versaoId, int situacao)
        {
            var query = @"update plano_aee
                           set situacao = @situacao
                          where id in (select plano_aee_id from plano_aee_versao where id = @versaoId) ";

            return await database.Conexao.ExecuteAsync(query, new { versaoId, situacao });
        }

        public async Task<IEnumerable<PlanoAEE>> ObterPorDataFinalVigencia(DateTime dataFim, bool desconsiderarPendencias = true, bool desconsiderarNotificados = false, NotificacaoPlanoAEETipo tipo = NotificacaoPlanoAEETipo.PlanoCriado)
        {
            var joinPendecias = desconsiderarPendencias ? "left join pendencia_plano_aee ppa on ppa.plano_aee_id = pa.id" : "";
            var joinNotificacoes = desconsiderarNotificados ? "left join notificacao_plano_aee npa on npa.plano_aee_id = pa.id and npa.tipo = @tipo" : "";

            var condicaoPendencias = desconsiderarPendencias ? "and ppa.id is null" : "";
            var condicaoNotificacoes = desconsiderarNotificados ? "and npa.id is null" : "";

            var query = $@"select pa.* 
                          from plano_aee pa
                         inner join plano_aee_versao pav on pav.id in (select max(id) from plano_aee_versao where plano_aee_id = pa.id)
                         inner join plano_aee_questao paq on paq.plano_aee_versao_id = pav.id
                         inner join questao q on q.id = paq.questao_id and q.ordem = 1 and q.tipo = 10
                         inner join plano_aee_resposta par on par.plano_questao_id = paq.id
                          {joinPendecias}
                          {joinNotificacoes}
                         where par.periodo_fim <= @dataFim
                           and pa.situacao in (1,2)
                           {condicaoPendencias}
                           {condicaoNotificacoes}";

            return await database.Conexao.QueryAsync<PlanoAEE>(query, new { dataFim, tipo });
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
    }
}
