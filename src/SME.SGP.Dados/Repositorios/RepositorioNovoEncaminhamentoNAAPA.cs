using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using SME.SGP.Infra.Interface;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNovoEncaminhamentoNAAPA : RepositorioBase<EncaminhamentoEscolar>, IRepositorioNovoEncaminhamentoNAAPA
    {
        public const int QUESTAO_DATA_QUEIXA_ORDEM = 0;
        public const int QUESTAO_PRIORIDADE_ORDEM = 1;
        public const int SECAO_ETAPA_1 = 1;
        public const int SECAO_INFORMACOES_ALUNO_ORDEM = 1;
        public const string QUESTAO_DATA_ENTRADA_QUEIXA = "DATA_ENTRADA_QUEIXA";
        public const string QUESTAO_DATA_DO_ATENDIMENTO = "DATA_DO_ATENDIMENTO";
        public const string QUESTAO_SUSPEITA_VIOLENCIA = "SUSPEITA_VIOLENCIA";

        public RepositorioNovoEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria)
            : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>> ListarPaginado(
            int anoLetivo,
            long dreId,
            string codigoUe,
            string codigoNomeAluno,
            DateTime? dataAberturaQueixaInicio,
            DateTime? dataAberturaQueixaFim,
            int situacao,
            long prioridade,
            long[] turmasIds,
            Paginacao paginacao,
            bool exibirEncerrados,
            OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao)
        {
            var query = MontaQueryCompleta(paginacao, codigoUe, codigoNomeAluno, dataAberturaQueixaInicio,
                dataAberturaQueixaFim, situacao, prioridade, turmasIds, exibirEncerrados, ordenacao);

            var situacoesEncerrado = (int)SituacaoNAAPA.Encerrado;

            if (!string.IsNullOrWhiteSpace(codigoNomeAluno))
                codigoNomeAluno = $"%{codigoNomeAluno.ToLower()}%";

            var parametros = new
            {
                anoLetivo,
                codigoUe,
                dreId,
                codigoNomeAluno,
                turmasIds,
                situacao,
                prioridade,
                dataAberturaQueixaInicio,
                dataAberturaQueixaFim,
                situacoesEncerrado
            };

            var retorno = new PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>();

            using (var encaminhamentosNAAPA = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = encaminhamentosNAAPA.Read<NovoEncaminhamentoNAAPAResumoDto>();
                retorno.TotalRegistros = encaminhamentosNAAPA.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQueryCompleta(
            Paginacao paginacao,
            string codigoUe,
            string codigoNomeAluno,
            DateTime? dataAberturaQueixaInicio,
            DateTime? dataAberturaQueixaFim,
            int situacao,
            long prioridade,
            long[] turmasIds,
            bool exibirEncerrados,
            OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, codigoNomeAluno, dataAberturaQueixaInicio,
                dataAberturaQueixaFim, situacao, prioridade, turmasIds, codigoUe, exibirEncerrados, ordenacao);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, codigoNomeAluno, dataAberturaQueixaInicio,
                dataAberturaQueixaFim, situacao, prioridade, turmasIds, codigoUe, exibirEncerrados);

            return sql.ToString();
        }

        private void MontaQueryConsulta(
            Paginacao paginacao,
            StringBuilder sql,
            bool contador,
            string codigoNomeAluno,
            DateTime? dataAberturaQueixaInicio,
            DateTime? dataAberturaQueixaFim,
            int situacao,
            long prioridade,
            long[] turmasIds,
            string codigoUe,
            bool exibirEncerrados,
            OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao = null)
        {
            ObterCabecalho(sql, contador);
            ObterFiltro(sql, codigoNomeAluno, dataAberturaQueixaInicio, dataAberturaQueixaFim,
                situacao, prioridade, turmasIds, codigoUe, exibirEncerrados);
            ObterOrdenacaoConsulta(sql, ordenacao);

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObterOrdenacaoConsulta(StringBuilder sql, OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao)
        {
            StringBuilder sqlAux = new StringBuilder();
            if (ordenacao != null && ordenacao.Any())
            {
                foreach (var order in ordenacao)
                {
                    if (sqlAux.Length == 0)
                        sqlAux.AppendLine("order by");
                    else
                        sqlAux.Append(", ");

                    switch (order)
                    {
                        case OrdenacaoListagemPaginadaAtendimentoNAAPA.UE:
                            sqlAux.AppendLine($" {EnumExtensao.ObterCaseWhenSQL<TipoEscola>("ue.tipo_escola")}||' '||ue.nome");
                            break;
                        case OrdenacaoListagemPaginadaAtendimentoNAAPA.Estudante:
                            sqlAux.AppendLine(" np.aluno_nome, np.aluno_codigo");
                            break;
                        case OrdenacaoListagemPaginadaAtendimentoNAAPA.DataEntradaQueixa:
                            sqlAux.AppendLine(" to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd')");
                            break;
                        case OrdenacaoListagemPaginadaAtendimentoNAAPA.UEDesc:
                            sqlAux.AppendLine($" {EnumExtensao.ObterCaseWhenSQL<TipoEscola>("ue.tipo_escola")}||' '||ue.nome desc");
                            break;
                        case OrdenacaoListagemPaginadaAtendimentoNAAPA.EstudanteDesc:
                            sqlAux.AppendLine(" np.aluno_nome desc, np.aluno_codigo desc");
                            break;
                        case OrdenacaoListagemPaginadaAtendimentoNAAPA.DataEntradaQueixaDesc:
                            sqlAux.AppendLine(" to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd') desc ");
                            break;
                        default:
                            break;
                    }
                }
                sql.AppendLine(sqlAux.ToString());
            }
        }

        private static void ObterCabecalho(StringBuilder sql, bool contador)
        {
            var sqlSelect = $@"with cte_tipo_questionario as (
                                    select encaminhamento_escolar_id, tipo
                                    from (
                                        select ens.encaminhamento_escolar_id, q2.tipo,
                                               ROW_NUMBER() OVER(PARTITION BY ens.encaminhamento_escolar_id ORDER BY q2.tipo DESC) as rn
                                        from encaminhamento_naapa_secao ens
                                        join secao_encaminhamento_naapa sec on sec.id = ens.secao_encaminhamento_id
                                        join questionario q2 on q2.id = sec.questionario_id
                                        where not ens.excluido
                                          and q2.tipo in ({string.Join(",", TipoQuestionarioNaapa())})
                                    ) t
                                    where rn = 1
                                ),
                                vw_resposta_data as (
                                    select ens.encaminhamento_escolar_id, enr.texto DataAberturaQueixaInicio
                                    from encaminhamento_naapa_secao ens
                                    join encaminhamento_escolar_questao enq on ens.id = enq.encaminhamento_escolar_secao_id
                                    join questao q on enq.questao_id = q.id
                                    join encaminhamento_escolar_resposta enr on enr.questao_encaminhamento_id = enq.id
                                    join secao_encaminhamento_naapa secao on secao.id = ens.secao_encaminhamento_id
                                    where q.nome_componente = 'DATA_ENTRADA_QUEIXA'
                                      and not ens.excluido
                                      and not enq.excluido
                                      and not enr.excluido
                                      and ens.encaminhamento_escolar_id is not null
                                ),
                                vw_resposta_data_ultimo_atendimento as (
                                    select ens.encaminhamento_escolar_id, max(to_date(enr.texto,'yyyy-mm-dd')) DataUltimoAtendimento
                                    from encaminhamento_naapa_secao ens
                                    join encaminhamento_escolar_questao enq on ens.id = enq.encaminhamento_escolar_secao_id
                                    join questao q on enq.questao_id = q.id
                                    join encaminhamento_escolar_resposta enr on enr.questao_encaminhamento_id = enq.id
                                    join secao_encaminhamento_naapa secao on secao.id = ens.secao_encaminhamento_id
                                    join questionario q2 on q2.id = secao.questionario_id
                                    where length(enr.texto) > 0
                                      and not ens.excluido
                                      and not enq.excluido
                                      and not enr.excluido
                                      and (secao.nome_componente = 'QUESTOES_ITINERACIA' or secao.nome_componente = 'QUESTOES_ITINERANCIA')
                                      and q.nome_componente = 'DATA_DO_ATENDIMENTO'
                                    group by ens.encaminhamento_escolar_id
                                ),
                                cte_opcao_sim_violencia as (
                                    select opr.id as resposta_sim_id
                                    from questao q
                                    join opcao_resposta opr on opr.questao_id = q.id
                                    where q.nome_componente = 'SUSPEITA_VIOLENCIA'
                                      and UPPER(opr.nome) = 'SIM'
                                      and not q.excluido
                                      and not opr.excluido
                                    limit 1
                                ),
                                vw_suspeita_violencia as (
                                    select distinct ens.encaminhamento_escolar_id, true as SuspeitaViolencia
                                    from encaminhamento_naapa_secao ens
                                    join encaminhamento_escolar_questao enq on enq.encaminhamento_escolar_secao_id = ens.id
                                    join questao q on q.id = enq.questao_id
                                    join encaminhamento_escolar_resposta enr on enr.questao_encaminhamento_id = enq.id
                                    join cte_opcao_sim_violencia csv on csv.resposta_sim_id = enr.resposta_id
                                    where q.nome_componente = 'SUSPEITA_VIOLENCIA'
                                      and not ens.excluido
                                      and not enq.excluido
                                      and not enr.excluido
                                )
                                select ";

            sql.AppendLine(sqlSelect);

            if (contador)
                sql.AppendLine("count(np.id) ");
            else
            {
                sql.AppendLine(@"np.id
                                ,ctq.tipo as TipoQuestionario
                                ,ue.nome as UeNome 
                                ,ue.tipo_escola as TipoEscola 
                                ,np.aluno_nome || ' (' || np.aluno_codigo || ')' as NomeAluno 
                                ,t.nome as TurmaNome
                                ,case when length(qdata.DataAberturaQueixaInicio) > 0 then to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd') else null end DataAberturaQueixaInicio
                                ,qdataultimoatendimento.DataUltimoAtendimento
                                ,np.situacao
                                ,COALESCE(vsv.SuspeitaViolencia, false) as SuspeitaViolencia
                ");
            }

            sql.AppendLine(@$" from encaminhamento_escolar np              
                                left join turma t on t.id = np.turma_id
                                left join ue on ue.id = COALESCE(t.ue_id, np.ue_id)
                                join dre on dre.id = ue.dre_id
                                left join cte_tipo_questionario ctq on ctq.encaminhamento_escolar_id = np.id
                                left join vw_resposta_data qdata on qdata.encaminhamento_escolar_id = np.id
                                left join vw_resposta_data_ultimo_atendimento qdataultimoatendimento on qdataultimoatendimento.encaminhamento_escolar_id = np.id
                                left join vw_suspeita_violencia vsv on vsv.encaminhamento_escolar_id = np.id
            ");
        }

        private static int[] TipoQuestionarioNaapa()
        {
            int[] tipoQuestionario = { (int)TipoQuestionario.EncaminhamentoNAAPAIndividual,
                                       (int)TipoQuestionario.EncaminhamentoNAAPAInstitucional };

            return tipoQuestionario;
        }

        private void ObterFiltro(
            StringBuilder sql,
            string codigoNomeAluno,
            DateTime? dataAberturaQueixaInicio,
            DateTime? dataAberturaQueixaFim,
            int situacao,
            long prioridade,
            long[] turmasIds,
            string codigoUe,
            bool exibirEncerrados)
        {
            var dataTexto = "^\\d{4}-\\d{2}-\\d{2}";

            sql.AppendLine($@" where not np.excluido 
                                    and dre.id = @dreId
                                    and qdata.DataAberturaQueixaInicio ~ '{dataTexto}'
                                    and EXTRACT(YEAR FROM to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd')) = @anoLetivo");

            if (!string.IsNullOrEmpty(codigoUe))
                sql.AppendLine(@" and ue.ue_id = @codigoUe ");

            if (!string.IsNullOrEmpty(codigoNomeAluno))
                sql.AppendLine(" and (lower(np.aluno_nome) like @codigoNomeAluno or np.aluno_codigo like @codigoNomeAluno)");

            if (turmasIds != null && turmasIds.Any())
                sql.AppendLine(" and t.id = ANY(@turmasIds) ");

            if (situacao > 0)
                sql.AppendLine(" and np.situacao = @situacao ");

            if (prioridade > 0)
                sql.AppendLine(" and qPrioridade.PrioridadeId = @prioridade ");

            if (!exibirEncerrados)
                sql.AppendLine(" and np.situacao <> @situacoesEncerrado ");

            if (dataAberturaQueixaInicio.HasValue || dataAberturaQueixaFim.HasValue)
            {
                if (dataAberturaQueixaInicio.HasValue)
                    sql.AppendLine(" and to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd') >= @dataAberturaQueixaInicio ");

                if (dataAberturaQueixaFim.HasValue)
                    sql.AppendLine(" and to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd') <= @dataAberturaQueixaFim");
            }
        }

        public async Task<EncaminhamentoEscolar> ObterEncaminhamentoPorId(long id)
        {
            const string query = @"select ea.*, eas.*, qea.*, rea.*, sea.*, q.*, op.*
                                    from encaminhamento_escolar ea
                                    inner join encaminhamento_naapa_secao eas on eas.encaminhamento_escolar_id = ea.id
                                        and not eas.excluido
                                    inner join secao_encaminhamento_naapa sea on sea.id = eas.secao_encaminhamento_id
                                        and not sea.excluido
                                    inner join encaminhamento_escolar_questao qea on qea.encaminhamento_escolar_secao_id = eas.id
                                        and not qea.excluido
                                    inner join questao q on q.id = qea.questao_id
                                        and not q.excluido
                                    inner join encaminhamento_escolar_resposta rea on rea.questao_encaminhamento_id = qea.id
                                        and not rea.excluido
                                    left join opcao_resposta op on op.id = rea.resposta_id
                                        and not op.excluido
                                    where ea.id = @id
                                    and not ea.excluido";

            var encaminhamento = new EncaminhamentoEscolar();

            await database.Conexao
                .QueryAsync<EncaminhamentoEscolar, EncaminhamentoNAAPASecao, QuestaoEncaminhamentoEscolar,
                    RespostaEncaminhamentoEscolar, SecaoEncaminhamentoNAAPA, Questao, OpcaoResposta, EncaminhamentoEscolar>(
                    query, (encaminhamentoNAAPA, encaminhamentoSecao, questaoEncaminhamentoNAAPA, respostaEncaminhamento,
                        secaoEncaminhamento, questao, opcaoResposta) =>
                    {
                        if (encaminhamento.Id == 0)
                            encaminhamento = encaminhamentoNAAPA;

                        var secao = encaminhamento.Secoes.FirstOrDefault(c => c.Id == encaminhamentoSecao.Id);

                        if (secao.EhNulo())
                        {
                            encaminhamentoSecao.SecaoEncaminhamentoNAAPA = secaoEncaminhamento;
                            secao = encaminhamentoSecao;
                            encaminhamento.Secoes.Add(secao);
                        }

                        var questaoEncaminhamento = secao.QuestoesEscolar.FirstOrDefault(c => c.Id == questaoEncaminhamentoNAAPA.Id);

                        if (questaoEncaminhamento.EhNulo())
                        {
                            questaoEncaminhamento = questaoEncaminhamentoNAAPA;
                            questaoEncaminhamento.Questao = questao;
                            secao.QuestoesEscolar.Add(questaoEncaminhamento);
                        }

                        var resposta = questaoEncaminhamento.Respostas.FirstOrDefault(c => c.Id == respostaEncaminhamento.Id);

                        if (resposta.EhNulo())
                        {
                            resposta = respostaEncaminhamento;
                            resposta.Resposta = opcaoResposta;
                            questaoEncaminhamento.Respostas.Add(resposta);
                        }

                        return encaminhamento;
                    }, new { id });

            return encaminhamento;
        }

        public async Task<EncaminhamentoEscolar> ObterEncaminhamentoComTurmaPorId(long encaminhamentoId)
        {
            var query = @"select ea.*, 
                         t.*, 
                         ue.*, 
                         dre.*,
                         ue2.* as UeDireta,
                         dre2.* as DreDireta
                    from encaminhamento_escolar ea
                    left join turma t on t.id = ea.turma_id
                    left join ue on ue.id = t.ue_id
                    left join dre on dre.id = ue.dre_id
                    left join ue ue2 on ue2.id = ea.ue_id
                    left join dre dre2 on dre2.id = ea.dre_id
                    where ea.id = @encaminhamentoId";

            return (await database.Conexao.QueryAsync<EncaminhamentoEscolar, Turma, Ue, Dre, Ue, Dre, EncaminhamentoEscolar>(
                query,
                (encaminhamento, turma, ueTurma, dreTurma, ueEncaminhamento, dreEncaminhamento) =>
                {
                    if (turma != null)
                    {
                        turma.Ue = ueTurma;
                        turma.Ue.Dre = dreTurma;
                        encaminhamento.Turma = turma;
                    }

                    if (ueEncaminhamento != null)
                    {
                        ueEncaminhamento.Dre = dreEncaminhamento;
                        encaminhamento.Ue = ueEncaminhamento;
                        encaminhamento.Dre = dreEncaminhamento;
                    }

                    return encaminhamento;
                },
                new { encaminhamentoId },
                splitOn: "id,id,id,id,id"
            )).FirstOrDefault();
        }

        public async Task<bool> VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente(long encaminhamentoId)
        {
            var query = @"select
	                            *
                            from
	                            encaminhamento_escolar ee
                            left join encaminhamento_naapa_secao ens 
                                                     on
	                            ens.encaminhamento_naapa_id = ee.id
                            left join secao_encaminhamento_naapa sen 
                                                     on
	                            sen.id = ens.secao_encaminhamento_id
                            where
	                            not ee.excluido
	                            and ee.situacao = @situacao
	                            and sen.nome_componente =  @secaoNome
	                            and ens.concluido
	                            and not ens.excluido
	                            and ee.id = @encaminhamentoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { situacao = (int)SituacaoNAAPA.AguardandoAtendimento, encaminhamentoId, secaoNome = NovoEncaminhamentoNAAPAConstants.QUESTOES_FLUXO_ENC_INDIVIDUAL });
            }
    }
}