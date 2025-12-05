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

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioAtendimentoNAAPA : RepositorioBase<EncaminhamentoNAAPA>, IRepositorioAtendimentoNAAPA
    {
        public const int QUESTAO_DATA_QUEIXA_ORDEM = 0;
        public const int QUESTAO_PRIORIDADE_ORDEM = 1;
        public const int SECAO_ETAPA_1 = 1;
        public const int SECAO_INFORMACOES_ALUNO_ORDEM = 1;
        public const string QUESTAO_DATA_DO_ATENDIMENTO = "DATA_DO_ATENDIMENTO";

        public RepositorioAtendimentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>> ListarPaginado(int anoLetivo, long dreId, 
            string codigoUe, string codigoNomeAluno, DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, 
            int situacao, long prioridade, long[] turmasIds, Paginacao paginacao, bool exibirEncerrados, OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao)
        {
            var query = MontaQueryCompleta(paginacao, codigoUe, codigoNomeAluno, dataAberturaQueixaInicio, 
                dataAberturaQueixaFim, situacao,prioridade , turmasIds, exibirEncerrados, ordenacao);
            var situacoesEncerrado = (int)SituacaoNAAPA.Encerrado ;

            if (!string.IsNullOrWhiteSpace(codigoNomeAluno))
                codigoNomeAluno = $"%{codigoNomeAluno.ToLower()}%";
            
            var parametros = new { anoLetivo, codigoUe, dreId, codigoNomeAluno,
                turmasIds, situacao, prioridade, dataAberturaQueixaInicio, 
                dataAberturaQueixaFim, situacoesEncerrado };

            var retorno = new PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>();
            
            using (var encaminhamentosNAAPA = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = encaminhamentosNAAPA.Read<AtendimentoNAAPAResumoDto>();
                retorno.TotalRegistros = encaminhamentosNAAPA.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQueryCompleta(Paginacao paginacao, string codigoUe, string codigoNomeAluno, 
            DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, int situacao, long prioridade, long[] turmasIds, bool exibirEncerrados, OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, codigoNomeAluno, dataAberturaQueixaInicio,
                dataAberturaQueixaFim,situacao, prioridade, turmasIds, codigoUe, exibirEncerrados, ordenacao);
            
            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, codigoNomeAluno, dataAberturaQueixaInicio,
                dataAberturaQueixaFim,situacao, prioridade, turmasIds, codigoUe, exibirEncerrados);

            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, string codigoNomeAluno, 
            DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, int situacao, long prioridade, 
            long[] turmasIds, string codigoUe, bool exibirEncerrados, OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao = null)
        {
            ObterCabecalho(sql, contador);

            ObterFiltro(sql, codigoNomeAluno, dataAberturaQueixaInicio, dataAberturaQueixaFim,situacao, prioridade, turmasIds, codigoUe, exibirEncerrados);
            
            ObterOrdenacaoConsulta(sql, ordenacao);

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObterOrdenacaoConsulta(StringBuilder sql, OrdenacaoListagemPaginadaAtendimentoNAAPA[] ordenacao)
        {
            StringBuilder sqlAux = new StringBuilder();
            if (ordenacao.PossuiRegistros())
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
            var sqlSelect = $@"with vw_resposta_data as (
                        select ens.encaminhamento_naapa_id, 
                               enr.texto DataAberturaQueixaInicio    
                        from encaminhamento_naapa_secao ens   
                        join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id  
                        join questao q on enq.questao_id = q.id 
                        join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
                        join secao_encaminhamento_naapa secao on secao.id = ens.secao_encaminhamento_id
                        left join opcao_resposta opr on opr.id = enr.resposta_id
                        where q.ordem = {QUESTAO_DATA_QUEIXA_ORDEM} and secao.etapa = {SECAO_ETAPA_1} and secao.ordem = {SECAO_INFORMACOES_ALUNO_ORDEM}
                              and not ens.excluido and not enq.excluido and not enr.excluido 
                        ),
                        vw_resposta_prioridade as (
                        select ens.encaminhamento_naapa_id, 
                                opr.nome as Prioridade,
                                enr.resposta_id  as PrioridadeId
                        from encaminhamento_naapa_secao ens   
                        join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id  
                        join questao q on enq.questao_id = q.id 
                        join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
                        join secao_encaminhamento_naapa secao on secao.id = ens.secao_encaminhamento_id
                        left join opcao_resposta opr on opr.id = enr.resposta_id
                        where q.ordem = {QUESTAO_PRIORIDADE_ORDEM} and secao.etapa = {SECAO_ETAPA_1} and secao.ordem = {SECAO_INFORMACOES_ALUNO_ORDEM}
                              and not ens.excluido and not enq.excluido and not enr.excluido 
                        ),
                        vw_resposta_data_ultimo_atendimento as (
                        select ens.encaminhamento_naapa_id, 
                               max(to_date(enr.texto,'yyyy-mm-dd')) DataUltimoAtendimento   
                        from encaminhamento_naapa_secao ens   
                        join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id  
                        join questao q on enq.questao_id = q.id 
                        join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
                        join secao_encaminhamento_naapa secao on secao.id = ens.secao_encaminhamento_id
                        join questionario q2 on q2.id = secao.questionario_id 
                        where length(enr.texto) > 0 and not ens.excluido and not enq.excluido and not enr.excluido  
                              and (secao.nome_componente = 'QUESTOES_ITINERACIA' or secao.nome_componente = 'QUESTOES_ITINERANCIA' )
                              and q2.tipo = {(int)TipoQuestionario.EncaminhamentoNAAPA} and q.nome_componente = '{QUESTAO_DATA_DO_ATENDIMENTO}'
                        group by ens.encaminhamento_naapa_id 
                        )
                        select ";
            sql.AppendLine(sqlSelect);
            if (contador)
                sql.AppendLine("count(np.id) ");
            else
            {
                sql.AppendLine(@"np.id
                                ,ue.nome UeNome 
                                ,ue.tipo_escola TipoEscola
                                ,np.aluno_codigo as CodigoAluno
                                ,np.aluno_nome as NomeAluno 
                                ,np.situacao 
                                ,case when length(qdata.DataAberturaQueixaInicio) > 0 then to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd') else null end DataAberturaQueixaInicio
                                ,qprioridade.Prioridade
                                ,t.nome as TurmaNome, t.modalidade_codigo as TurmaModalidade
                                ,qdataultimoatendimento.DataUltimoAtendimento
                ");
            }

            sql.AppendLine(@" from encaminhamento_naapa np              
                                join turma t on t.id = np.turma_id
                                join ue on t.ue_id = ue.id
                                left join vw_resposta_data qdata on qdata.encaminhamento_naapa_id = np.id
                                left join vw_resposta_prioridade qprioridade on qprioridade.encaminhamento_naapa_id = np.id 
                                left join vw_resposta_data_ultimo_atendimento qdataultimoatendimento on qdataultimoatendimento.encaminhamento_naapa_id = np.id 
            ");
        }

        private void ObterFiltro(StringBuilder sql, string codigoNomeAluno, DateTime? dataAberturaQueixaInicio, 
            DateTime? dataAberturaQueixaFim, int situacao, long prioridade, long[] turmasIds, string codigoUe, bool exibirEncerrados)
        {
            sql.AppendLine(@" where not np.excluido 
                                    and t.ano_letivo = @anoLetivo
                                    and ue.dre_Id = @dreId"); 

            if (!string.IsNullOrEmpty(codigoUe))
                sql.AppendLine(@" and ue.ue_id = @codigoUe ");

            if (!string.IsNullOrEmpty(codigoNomeAluno))
                sql.AppendLine(" and (lower(np.aluno_nome) like @codigoNomeAluno or np.aluno_codigo like @codigoNomeAluno)");
            
            if (turmasIds.Any())
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
        
        public async Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorId(long id)
        {
            const string query = @"select ea.*, eas.*, qea.*, rea.*, sea.*, q.*, op.*
                                    from encaminhamento_naapa ea
                                    inner join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = ea.id
                                        and not eas.excluido
                                    inner join secao_encaminhamento_naapa sea on sea.id = eas.secao_encaminhamento_id
                                        and not sea.excluido
                                    inner join encaminhamento_naapa_questao qea on qea.encaminhamento_naapa_secao_id = eas.id
                                        and not qea.excluido
                                    inner join questao q on q.id = qea.questao_id
                                        and not q.excluido
                                    inner join encaminhamento_naapa_resposta rea on rea.questao_encaminhamento_id = qea.id
                                        and not rea.excluido
                                    left join opcao_resposta op on op.id = rea.resposta_id
                                        and not op.excluido
                                    where ea.id = @id
                                    and not ea.excluido";

            var encaminhamento = new EncaminhamentoNAAPA();

            await database.Conexao
                .QueryAsync<EncaminhamentoNAAPA, EncaminhamentoNAAPASecao, QuestaoEncaminhamentoNAAPA,
                    RespostaEncaminhamentoNAAPA, SecaoEncaminhamentoNAAPA, Questao, OpcaoResposta, EncaminhamentoNAAPA>(
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

                        var questaoEncaminhamento = secao.Questoes.FirstOrDefault(c => c.Id == questaoEncaminhamentoNAAPA.Id);
                        
                        if (questaoEncaminhamento.EhNulo())
                        {
                            questaoEncaminhamento = questaoEncaminhamentoNAAPA;
                            questaoEncaminhamento.Questao = questao;
                            secao.Questoes.Add(questaoEncaminhamento);
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

        public async Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorIdESecao(long id, long secaoId)
        {
            const string query = @"select ea.*, eas.*, qea.*, rea.*, sea.*, q.*, op.*
                                    from encaminhamento_naapa ea
                                    inner join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = ea.id
                                        and not eas.excluido
                                    inner join secao_encaminhamento_naapa sea on sea.id = eas.secao_encaminhamento_id
                                        and not sea.excluido
                                    inner join encaminhamento_naapa_questao qea on qea.encaminhamento_naapa_secao_id = eas.id
                                        and not qea.excluido
                                    inner join questao q on q.id = qea.questao_id
                                        and not q.excluido
                                    inner join encaminhamento_naapa_resposta rea on rea.questao_encaminhamento_id = qea.id
                                        and not rea.excluido
                                    left join opcao_resposta op on op.id = rea.resposta_id
                                        and not op.excluido
                                    where ea.id = @id
                                    and qea.encaminhamento_naapa_secao_id = @secaoId
                                    and not ea.excluido";

            var encaminhamento = new EncaminhamentoNAAPA();

            await database.Conexao
                .QueryAsync<EncaminhamentoNAAPA, EncaminhamentoNAAPASecao, QuestaoEncaminhamentoNAAPA,
                    RespostaEncaminhamentoNAAPA, SecaoEncaminhamentoNAAPA, Questao, OpcaoResposta, EncaminhamentoNAAPA>(
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

                        var questaoEncaminhamento = secao.Questoes.FirstOrDefault(c => c.Id == questaoEncaminhamentoNAAPA.Id);
                        
                        if (questaoEncaminhamento.EhNulo())
                        {
                            questaoEncaminhamento = questaoEncaminhamentoNAAPA;
                            questaoEncaminhamento.Questao = questao;
                            secao.Questoes.Add(questaoEncaminhamento);
                        }

                        var resposta = questaoEncaminhamento.Respostas.FirstOrDefault(c => c.Id == respostaEncaminhamento.Id);

                        if (resposta.NaoEhNulo()) 
                            return encaminhamento;
                        
                        resposta = respostaEncaminhamento;
                        resposta.Resposta = opcaoResposta;
                        questaoEncaminhamento.Respostas.Add(resposta);

                        return encaminhamento;
                    }, new { id, secaoId });

            return encaminhamento;
        }

        public async Task<IEnumerable<AtendimentoNAAPACodigoArquivoDto>> ObterCodigoArquivoPorEncaminhamentoNAAPAId(long encaminhamentoId)
        {
            var sql = @"select
                            a.codigo
                        from
                            encaminhamento_naapa ea
                        inner join encaminhamento_naapa_secao eas on
                            ea.id = eas.encaminhamento_naapa_id
                        inner join encaminhamento_naapa_questao qea on
                            eas.id = qea.encaminhamento_naapa_secao_id
                        inner join encaminhamento_naapa_resposta rea on
                            qea.id = rea.questao_encaminhamento_id
                        inner join arquivo a on
                            rea.arquivo_id = a.id
                        where
                            ea.id = @encaminhamentoId";

            return await database.Conexao.QueryAsync<AtendimentoNAAPACodigoArquivoDto>(sql.ToString(), new { encaminhamentoId });
        }   
        
        public async Task<EncaminhamentoNAAPA> ObterEncaminhamentoComTurmaPorId(long encaminhamentoId)
        {
            var query = @" select ea.*, t.*, ue.*, dre.*
                            from encaminhamento_naapa ea
                           inner join turma t on t.id = ea.turma_id
                            join ue on ue.id = t.ue_id
                            join dre on dre.id = ue.dre_id  
                           where ea.id = @encaminhamentoId";

            return (await database.Conexao.QueryAsync<EncaminhamentoNAAPA, Turma, Ue, Dre,EncaminhamentoNAAPA>(query,
                (encaminhamentoNAAPA, turma, ue, dre) =>
                {
                    encaminhamentoNAAPA.Turma = turma;
                    encaminhamentoNAAPA.Turma.Ue = ue;
                    encaminhamentoNAAPA.Turma.Ue.Dre = dre;
                    
                    return encaminhamentoNAAPA;
                }, new { encaminhamentoId })).FirstOrDefault();
        }

        public async Task<bool> EncaminhamentoContemAtendimentosItinerancia(long encaminhamentoId)
        {
            var query = $@"select ens.id
                        from encaminhamento_naapa_secao ens 
                        INNER JOIN secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id 
                        WHERE NOT ens.excluido and sen.nome_componente = @secaoNome
                                and ens.encaminhamento_naapa_id = @id";

            return (await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { id = encaminhamentoId, secaoNome = AtendimentoNAAPAConstants.SECAO_ITINERANCIA }));           
        }

        public async Task<IEnumerable<AtendimentosNAAPAConsolidadoDto>> ObterQuantidadeSituacaoEncaminhamentosPorUeAnoLetivo(long ueId, int anoLetivo)
        {
           var query = @"select 
                            t.ue_id UeId,
                            t.ano_letivo AnoLetivo, 
                            en.situacao,
                            count(en.id)quantidade,
                            t.modalidade_codigo as Modalidade
                        from encaminhamento_naapa en
                        inner join turma t on en.turma_id = t.id 
                        where not en.excluido  
                        and t.ue_id = @ueId
                        and t.ano_letivo = @anoLetivo
                        group by t.ue_id,t.ano_letivo ,en.situacao, t.modalidade_codigo ";
           
           return await database.Conexao.QueryAsync<AtendimentosNAAPAConsolidadoDto>(query, new {ueId, anoLetivo});
        }

        public async Task<SituacaoDto> ObterSituacao(long id)
        {
            var query = @" select situacao
                            from encaminhamento_naapa 
                           where id = @id";

            var situacao = await database.Conexao.QueryFirstAsync<int?>(query, new { id });

            if (situacao.HasValue)
                return new SituacaoDto
                {
                    Codigo = situacao.Value,
                    Descricao = ((SituacaoNAAPA)situacao.Value).ObterNome()
                };

            return new SituacaoDto();
        }

        public async Task<bool> VerificaSituacaoEncaminhamentoNAAPASeEstaAguardandoAtendimentoIndevidamente(long encaminhamentoId)
        {
            var query = @"select 1 from encaminhamento_naapa en 
                        left join encaminhamento_naapa_secao ens 
                         on ens.encaminhamento_naapa_id = en.id 
                        left join secao_encaminhamento_naapa sen 
                         on sen.id = ens.secao_encaminhamento_id 
                        where not en.excluido 
                        and en.situacao = @situacao
                        and sen.nome_componente = @secaoNome
                        and ens.concluido 
                        and not ens.excluido and 
                        en.id = @encaminhamentoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { situacao = (int)SituacaoNAAPA.AguardandoAtendimento, encaminhamentoId, secaoNome = AtendimentoNAAPAConstants.SECAO_ITINERANCIA });
        }

        public async Task<IEnumerable<AtendimentoNAAPADto>> ObterEncaminhamentosComSituacaoDiferenteDeEncerrado()
        {
            var query = @" select 
                        id,
                        turma_id as TurmaId,
                        aluno_codigo as AlunoCodigo,
                        aluno_nome as AlunoNome,
                        situacao,
                        situacao_matricula_aluno as SituacaoMatriculaAluno
                        from encaminhamento_naapa 
                        where situacao <> @situacao and not excluido";

            return await database.Conexao.QueryAsync<AtendimentoNAAPADto>(query, new { situacao = (int)SituacaoNAAPA.Encerrado });
        }

        public Task<EncaminhamentoNAAPA> ObterCabecalhoEncaminhamentoPorId(long id)
        {
            var query = @" select ea.*
                            from encaminhamento_naapa ea
                           where ea.id = @id";

            return (database.Conexao.QueryFirstOrDefaultAsync<EncaminhamentoNAAPA>(query, new { id }));
        }

        public async Task<bool> ExisteEncaminhamentoNAAPAAtivoParaAluno(string codigoAluno)
        {
            var query = @"SELECT 1 
                          FROM encaminhamento_naapa 
                         WHERE aluno_codigo = @codigoAluno 
                           and situacao <> @situacao
                           and not excluido";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { codigoAluno, situacao = (int)SituacaoNAAPA.Encerrado });
        }

        public async Task<IEnumerable<AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>> ObterInformacoesDeNotificacaoDeInatividadeDeAtendimento(long ueId)
        {
            var situacoes = new int[] { (int)SituacaoNAAPA.AguardandoAtendimento, (int)SituacaoNAAPA.EmAtendimento };
            var query = new StringBuilder();

            query.AppendLine("WITH inatividade_atendimento AS(");
            query.AppendLine(@$"select en.aluno_codigo AlunoCodigo, en.aluno_nome AlunoNome, en.turma_id TurmaId, en.id EncaminhamentoId
                from encaminhamento_naapa en
                inner join encaminhamento_naapa_secao ens on ens.encaminhamento_naapa_id = en.id
                where not exists(select 1
                                 from secao_encaminhamento_naapa sen
                                 where sen.nome_componente = '{AtendimentoNAAPAConstants.SECAO_ITINERANCIA}'
                                   and not sen.excluido 
                                   and sen.id = ens.secao_encaminhamento_id)
                and en.situacao = any(@situacoes)
                and not en.excluido
                and not ens.excluido
                and coalesce(en.data_ultima_notificacao_sem_atendimento, en.criado_em) + interval '30 day' <= now()");
            query.AppendLine(" union");
            query.AppendLine($@"select en.aluno_codigo AlunoCodigo, en.aluno_nome AlunoNome, en.turma_id TurmaId, en.id EncaminhamentoId
                from encaminhamento_naapa en
                inner join encaminhamento_naapa_secao ens on ens.encaminhamento_naapa_id = en.id
                inner join secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id and sen.nome_componente = '{AtendimentoNAAPAConstants.SECAO_ITINERANCIA}'
                inner join questionario qto on qto.id = sen.questionario_id
                inner join(select max(texto::date) dataAtendimento, enq.encaminhamento_naapa_secao_id
                           from encaminhamento_naapa_resposta enr
                           inner join encaminhamento_naapa_questao enq on enq.id = enr.questao_encaminhamento_id
                           inner join questao q on q.id = enq.questao_id
                           where q.nome_componente = '{QUESTAO_DATA_DO_ATENDIMENTO}'
                             and not enr.excluido
                             and not enq.excluido
                           group by enq.encaminhamento_naapa_secao_id) tab_dt_atendimento on tab_dt_atendimento.encaminhamento_naapa_secao_id = ens.id
                where en.situacao = any(@situacoes)
                  and not en.excluido
                  and not ens.excluido
                  and not sen.excluido
                  and coalesce(en.data_ultima_notificacao_sem_atendimento, tab_dt_atendimento.dataAtendimento) + interval '30 day' <= now()");
            query.AppendLine(")");
            query.AppendLine($@"select ia.AlunoCodigo, ia.AlunoNome, ia.EncaminhamentoId, ia.TurmaId, t.nome TurmaNome, 
                ue.nome UeNome, ue.ue_id UeCodigo, ue.tipo_escola TipoEscola, dre.abreviacao DreNome, dre.dre_id DreCodigo
                from inatividade_atendimento ia
                inner join turma t on t.id = ia.TurmaId
                inner join ue on ue.id = t.ue_id
                inner join dre on dre.id = ue.dre_id
                where ue.id = @ueId");

            return await database.Conexao.QueryAsync<AtendimentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>(query.ToString(), new { ueId, situacoes });
        }
    }
}
