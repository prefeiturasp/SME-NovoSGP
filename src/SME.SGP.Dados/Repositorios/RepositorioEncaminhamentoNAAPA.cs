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
    public class RepositorioEncaminhamentoNaapa : RepositorioBase<EncaminhamentoNAAPA>, IRepositorioEncaminhamentoNAAPA
    {
        public const int QUESTAO_DATA_QUEIXA_ORDEM = 0;
        public const int QUESTAO_PRIORIDADE_ORDEM = 1;
        public const int SECAO_ETAPA_1 = 1;
        public const int SECAO_INFORMACOES_ALUNO_ORDEM = 1;
        public const string QUESTAO_DATA_DO_ATENDIMENTO = "DATA_DO_ATENDIMENTO";

        public RepositorioEncaminhamentoNaapa(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>> ListarPaginado(int anoLetivo, long dreId,
            string codigoUe, string codigoNomeAluno, DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim,
            int situacao, long prioridade, long[] turmasIds, Paginacao paginacao, bool exibirEncerrados, OrdenacaoListagemPaginadaEncaminhamentoNAAPA[] ordenacao)
        {
            var parametrosListagem = new ListarPaginadoParametrosEncaminhamentoNaapaDto
            {
                AnoLetivo = anoLetivo,
                DreId = dreId,
                CodigoUe = codigoUe,
                CodigoNomeAluno = codigoNomeAluno,
                DataAberturaQueixaInicio = dataAberturaQueixaInicio,
                DataAberturaQueixaFim = dataAberturaQueixaFim,
                Situacao = situacao,
                Prioridade = prioridade,
                TurmasIds = turmasIds,
                Paginacao = paginacao,
                ExibirEncerrados = exibirEncerrados,
                Ordenacao = ordenacao
            };

            var query = MontaQueryCompleta(parametrosListagem.Paginacao, parametrosListagem.CodigoUe, parametrosListagem.CodigoNomeAluno,
                parametrosListagem.DataAberturaQueixaInicio, parametrosListagem.DataAberturaQueixaFim, parametrosListagem.Situacao,
                parametrosListagem.Prioridade, parametrosListagem.TurmasIds, parametrosListagem.ExibirEncerrados, parametrosListagem.Ordenacao);

            var situacoesEncerrado = (int)SituacaoNAAPA.Encerrado;

            if (!string.IsNullOrWhiteSpace(parametrosListagem.CodigoNomeAluno))
                parametrosListagem.CodigoNomeAluno = $"%{parametrosListagem.CodigoNomeAluno.ToLower()}%";

            var parametrosDapper = new
            {
                parametrosListagem.AnoLetivo,
                parametrosListagem.CodigoUe,
                parametrosListagem.DreId,
                parametrosListagem.CodigoNomeAluno,
                parametrosListagem.TurmasIds,
                parametrosListagem.Situacao,
                parametrosListagem.Prioridade,
                parametrosListagem.DataAberturaQueixaInicio,
                parametrosListagem.DataAberturaQueixaFim,
                situacoesEncerrado
            };

            var retorno = new PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>();

            using (var encaminhamentosNAAPA = await database.Conexao.QueryMultipleAsync(query, parametrosDapper))
            {
                retorno.Items = await encaminhamentosNAAPA.ReadAsync<EncaminhamentoNAAPAResumoDto>();
                retorno.TotalRegistros = await encaminhamentosNAAPA.ReadFirstAsync<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / parametrosListagem.Paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQueryCompleta(Paginacao paginacao, string codigoUe, string codigoNomeAluno,
            DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, int situacao, long prioridade, long[] turmasIds, bool exibirEncerrados, OrdenacaoListagemPaginadaEncaminhamentoNAAPA[] ordenacao)
        {
            var parametrosQueryCompleta = new MontaQueryCompletaParametrosEncaminhamentoNaapaDto
            {
                Paginacao = paginacao,
                CodigoUe = codigoUe,
                CodigoNomeAluno = codigoNomeAluno,
                DataAberturaQueixaInicio = dataAberturaQueixaInicio,
                DataAberturaQueixaFim = dataAberturaQueixaFim,
                Situacao = situacao,
                Prioridade = prioridade,
                TurmasIds = turmasIds,
                ExibirEncerrados = exibirEncerrados,
                Ordenacao = ordenacao
            };

            var sql = new StringBuilder();

            MontaQueryConsulta(parametrosQueryCompleta.Paginacao, sql, contador: false, parametrosQueryCompleta.CodigoNomeAluno,
                parametrosQueryCompleta.DataAberturaQueixaInicio, parametrosQueryCompleta.DataAberturaQueixaFim,
                parametrosQueryCompleta.Situacao, parametrosQueryCompleta.Prioridade, parametrosQueryCompleta.TurmasIds,
                parametrosQueryCompleta.CodigoUe, parametrosQueryCompleta.ExibirEncerrados, parametrosQueryCompleta.Ordenacao);

            sql.AppendLine(";");

            MontaQueryConsulta(parametrosQueryCompleta.Paginacao, sql, contador: true, parametrosQueryCompleta.CodigoNomeAluno,
                parametrosQueryCompleta.DataAberturaQueixaInicio, parametrosQueryCompleta.DataAberturaQueixaFim,
                parametrosQueryCompleta.Situacao, parametrosQueryCompleta.Prioridade, parametrosQueryCompleta.TurmasIds,
                parametrosQueryCompleta.CodigoUe, parametrosQueryCompleta.ExibirEncerrados);

            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, string codigoNomeAluno,
            DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, int situacao, long prioridade,
            long[] turmasIds, string codigoUe, bool exibirEncerrados, OrdenacaoListagemPaginadaEncaminhamentoNAAPA[] ordenacao = null)
        {
            var parametrosQueryConsulta = new MontaQueryConsultaParametrosEncaminhamentoNaapaDto
            {
                Paginacao = paginacao,
                Contador = contador,
                CodigoNomeAluno = codigoNomeAluno,
                DataAberturaQueixaInicio = dataAberturaQueixaInicio,
                DataAberturaQueixaFim = dataAberturaQueixaFim,
                Situacao = situacao,
                Prioridade = prioridade,
                TurmasIds = turmasIds,
                CodigoUe = codigoUe,
                ExibirEncerrados = exibirEncerrados,
                Ordenacao = ordenacao
            };

            ObterCabecalho(sql, parametrosQueryConsulta.Contador);

            var parametrosFiltro = new ObterFiltroParametrosEncaminhamentoNaapaDto
            {
                CodigoNomeAluno = parametrosQueryConsulta.CodigoNomeAluno,
                DataAberturaQueixaInicio = parametrosQueryConsulta.DataAberturaQueixaInicio,
                DataAberturaQueixaFim = parametrosQueryConsulta.DataAberturaQueixaFim,
                Situacao = parametrosQueryConsulta.Situacao,
                Prioridade = parametrosQueryConsulta.Prioridade,
                TurmasIds = parametrosQueryConsulta.TurmasIds,
                CodigoUe = parametrosQueryConsulta.CodigoUe,
                ExibirEncerrados = parametrosQueryConsulta.ExibirEncerrados
            };
            ObterFiltro(sql, parametrosFiltro);

            ObterOrdenacaoConsulta(sql, parametrosQueryConsulta.Ordenacao);

            if (parametrosQueryConsulta.Paginacao.QuantidadeRegistros > 0 && !parametrosQueryConsulta.Contador)
                sql.AppendLine($" OFFSET {parametrosQueryConsulta.Paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {parametrosQueryConsulta.Paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObterOrdenacaoConsulta(StringBuilder sql, OrdenacaoListagemPaginadaEncaminhamentoNAAPA[] ordenacao)
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
                        case OrdenacaoListagemPaginadaEncaminhamentoNAAPA.UE:
                            sqlAux.AppendLine($" {EnumExtensao.ObterCaseWhenSQL<TipoEscola>("ue.tipo_escola")}||' '||ue.nome");
                            break;
                        case OrdenacaoListagemPaginadaEncaminhamentoNAAPA.Estudante:
                            sqlAux.AppendLine(" np.aluno_nome, np.aluno_codigo");
                            break;
                        case OrdenacaoListagemPaginadaEncaminhamentoNAAPA.DataEntradaQueixa:
                            sqlAux.AppendLine(" to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd')");
                            break;
                        case OrdenacaoListagemPaginadaEncaminhamentoNAAPA.UEDesc:
                            sqlAux.AppendLine($" {EnumExtensao.ObterCaseWhenSQL<TipoEscola>("ue.tipo_escola")}||' '||ue.nome desc");
                            break;
                        case OrdenacaoListagemPaginadaEncaminhamentoNAAPA.EstudanteDesc:
                            sqlAux.AppendLine(" np.aluno_nome desc, np.aluno_codigo desc");
                            break;
                        case OrdenacaoListagemPaginadaEncaminhamentoNAAPA.DataEntradaQueixaDesc:
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

        private static void ObterFiltro(StringBuilder sql, ObterFiltroParametrosEncaminhamentoNaapaDto parametros)
        {
            sql.AppendLine(@" where not np.excluido
                                    and t.ano_letivo = @anoLetivo
                                    and ue.dre_Id = @dreId");

            if (!string.IsNullOrEmpty(parametros.CodigoUe))
                sql.AppendLine(@" and ue.ue_id = @codigoUe ");

            if (!string.IsNullOrEmpty(parametros.CodigoNomeAluno))
                sql.AppendLine(" and (lower(np.aluno_nome) like @codigoNomeAluno or np.aluno_codigo like @codigoNomeAluno)");

            if (parametros.TurmasIds.Any())
                sql.AppendLine(" and t.id = ANY(@turmasIds) ");

            if (parametros.Situacao > 0)
                sql.AppendLine(" and np.situacao = @situacao ");

            if (parametros.Prioridade > 0)
                sql.AppendLine(" and qPrioridade.PrioridadeId = @prioridade ");

            if (!parametros.ExibirEncerrados)
                sql.AppendLine(" and np.situacao <> @situacoesEncerrado ");

            if (parametros.DataAberturaQueixaInicio.HasValue || parametros.DataAberturaQueixaFim.HasValue)
            {
                if (parametros.DataAberturaQueixaInicio.HasValue)
                    sql.AppendLine(" and to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd') >= @dataAberturaQueixaInicio ");

                if (parametros.DataAberturaQueixaFim.HasValue)
                    sql.AppendLine(" and to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd') <= @dataAberturaQueixaFim");
            }
        }

        private const string SELECT_ENCAMINHAMENTO_NAAPA = @"
            ea.id AS Id, 
            ea.criado_em AS CriadoEm, 
            ea.criado_por as CriadoPor, 
            ea.criado_rf as CriadoRf,
            ea.alterado_em AS AlteradoEm, 
            ea.alterado_por as AlteradoPor, 
            ea.alterado_rf as AlteradoRf,
            ea.turma_id AS TurmaId, 
            ea.aluno_codigo AS AlunoCodigo, 
            ea.aluno_nome AS AlunoNome,
            ea.situacao AS Situacao, 
            ea.excluido AS Excluido,
            ea.situacao_matricula_aluno AS SituacaoMatriculaAluno, 
            ea.motivo_encerramento AS MotivoEncerramento,
            ea.data_ultima_notificacao_sem_atendimento AS DataUltimaNotificacaoSemAtendimento";

        private const string SELECT_ENCAMINHAMENTO_NAAPA_SECAO = @"
            eas.id AS Id, 
            eas.encaminhamento_naapa_id AS EncaminhamentoNAAPAId,
            eas.secao_encaminhamento_id AS SecaoEncaminhamentoNAAPAId, 
            eas.concluido AS Concluido,
            eas.excluido AS Excluido, 
            eas.criado_em AS CriadoEm, 
            eas.alterado_em AS AlteradoEm,
            eas.alterado_por as AlteradoPor, 
            eas.alterado_rf as AlteradoRf, 
            eas.criado_rf as CriadoRf,
            eas.criado_por as CriadoPor";

        private const string SELECT_QUESTAO_ENCAMINHAMENTO = @"
            qea.id AS Id, 
            qea.encaminhamento_naapa_secao_id AS EncaminhamentoNAAPASecaoId,
            qea.questao_id AS QuestaoId, 
            qea.excluido AS Excluido, 
            qea.criado_em AS CriadoEm,
            qea.alterado_em AS AlteradoEm, 
            qea.alterado_por as AlteradoPor, 
            qea.alterado_rf as AlteradoRf,
            qea.criado_rf as CriadoRf, 
            qea.criado_por as CriadoPor";

        private const string SELECT_RESPOSTA = @"
            rea.id AS Id, 
            rea.questao_encaminhamento_id AS QuestaoEncaminhamentoId,
            rea.resposta_id AS RespostaId, 
            rea.texto AS Texto, 
            rea.arquivo_id AS ArquivoId,
            rea.excluido AS Excluido, 
            rea.criado_em AS CriadoEm, 
            rea.alterado_em AS AlteradoEm,
            rea.alterado_por as AlteradoPor, 
            rea.alterado_rf as AlteradoRf, 
            rea.criado_rf as CriadoRf,
            rea.criado_por as CriadoPor";

        private const string SELECT_SECAO_CONFIGURADA = @"
            sea.id AS Id, 
            sea.questionario_id AS QuestionarioId, 
            sea.nome AS Nome, 
            sea.ordem AS Ordem,
            sea.etapa AS Etapa, 
            sea.excluido AS Excluido, 
            sea.nome_componente AS NomeComponente,
            sea.criado_em AS CriadoEm, 
            sea.alterado_em AS AlteradoEm,
            sea.alterado_por as AlteradoPor, 
            sea.alterado_rf as AlteradoRf, 
            sea.criado_rf as CriadoRf,
            sea.criado_por as CriadoPor";

        private const string SELECT_QUESTAO = @"
            q.id AS Id, 
            q.nome AS Nome, 
            q.ordem AS Ordem, 
            q.nome_componente AS NomeComponente,
            q.excluido AS Excluido, 
            q.criado_em AS CriadoEm, 
            q.alterado_em AS AlteradoEm,
            q.alterado_por as AlteradoPor, 
            q.alterado_rf as AlteradoRf, 
            q.criado_rf as CriadoRf,
            q.criado_por as CriadoPor";

        private const string SELECT_OPCAO_RESPOSTA = @"
            op.id AS Id, 
            op.questao_id AS QuestaoId, 
            op.ordem AS Ordem, 
            op.nome AS Nome,
            op.observacao as Observacao, 
            op.excluido AS Excluido, 
            op.criado_em AS CriadoEm,
            op.alterado_em AS AlteradoEm, 
            op.alterado_por as AlteradoPor, 
            op.alterado_rf as AlteradoRf,
            op.criado_rf as CriadoRf, 
            op.criado_por as CriadoPor";

        private const string CAMPOSSELECT = $@"
            SELECT
                -- Encaminhamento NAAPA
                ea.id AS EncaminhamentoNAAPAInicio,
                {SELECT_ENCAMINHAMENTO_NAAPA},
                -- início da seção
                eas.id AS SecaoInicio,
                -- Encaminhamento NAAPA - Seção
                {SELECT_ENCAMINHAMENTO_NAAPA_SECAO},
                -- início da questão
                qea.id AS QuestaoEncaminhamentoInicio,
                -- Questão do encaminhamento
                {SELECT_QUESTAO_ENCAMINHAMENTO},
                -- início da resposta
                rea.id AS RespostaInicio,
                -- Resposta
                {SELECT_RESPOSTA},
                -- início da seção configurada
                sea.id AS SecaoConfiguradaInicio,
                -- Seção configurada
                {SELECT_SECAO_CONFIGURADA},
                -- início da questão
                q.id AS QuestaoInicio,
                -- Questão
                {SELECT_QUESTAO},
                -- início da opção
                op.id AS OpcaoRespostaInicio,
                -- Opção de resposta
                {SELECT_OPCAO_RESPOSTA}
           ";

        private EncaminhamentoNAAPA MapearEncaminhamento(
            EncaminhamentoNAAPA encaminhamentoAtual,
            EncaminhamentoNAAPA encaminhamentoNAAPA,
            EncaminhamentoNAAPASecao encaminhamentoSecao,
            QuestaoEncaminhamentoNAAPA questaoEncaminhamentoNAAPA,
            RespostaEncaminhamentoNAAPA respostaEncaminhamento,
            SecaoEncaminhamentoNAAPA secaoEncaminhamento,
            Questao questao,
            OpcaoResposta opcaoResposta)
        {
            if (encaminhamentoAtual.Id == 0)
            {
                encaminhamentoAtual = encaminhamentoNAAPA;
            }

            var secao = encaminhamentoAtual.Secoes.FirstOrDefault(x => x.Id == encaminhamentoSecao.Id);

            if (secao.EhNulo())
            {
                encaminhamentoSecao.SecaoEncaminhamentoNAAPA = secaoEncaminhamento;
                secao = encaminhamentoSecao;
                encaminhamentoAtual.Secoes.Add(secao);
            }

            var questaoEncaminhamento = secao.Questoes.FirstOrDefault(x => x.Id == questaoEncaminhamentoNAAPA.Id);

            if (questaoEncaminhamento.EhNulo())
            {
                questaoEncaminhamento = questaoEncaminhamentoNAAPA;
                questaoEncaminhamento.Questao = questao;
                secao.Questoes.Add(questaoEncaminhamento);
            }

            var resposta = questaoEncaminhamento.Respostas.FirstOrDefault(x => x.Id == respostaEncaminhamento.Id);

            if (resposta.EhNulo())
            {
                resposta = respostaEncaminhamento;
                resposta.Resposta = opcaoResposta;
                questaoEncaminhamento.Respostas.Add(resposta);
            }

            return encaminhamentoAtual;
        }

        public async Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorId(long id)
        {
            const string query = $@"
                {CAMPOSSELECT}
                FROM encaminhamento_naapa ea
                INNER JOIN encaminhamento_naapa_secao eas
                    ON eas.encaminhamento_naapa_id = ea.id
                   AND NOT eas.excluido
                INNER JOIN secao_encaminhamento_naapa sea
                    ON sea.id = eas.secao_encaminhamento_id
                   AND NOT sea.excluido
                INNER JOIN encaminhamento_naapa_questao qea
                    ON qea.encaminhamento_naapa_secao_id = eas.id
                   AND NOT qea.excluido
                INNER JOIN questao q
                    ON q.id = qea.questao_id
                   AND NOT q.excluido
                INNER JOIN encaminhamento_naapa_resposta rea
                    ON rea.questao_encaminhamento_id = qea.id
                   AND NOT rea.excluido
                LEFT JOIN opcao_resposta op
                    ON op.id = rea.resposta_id
                   AND NOT op.excluido
                WHERE ea.id = @id
                  AND NOT ea.excluido;";

            var encaminhamento = new EncaminhamentoNAAPA();

           var consulta =  await database.Conexao.QueryAsync<
                EncaminhamentoNAAPA,
                EncaminhamentoNAAPASecao,
                QuestaoEncaminhamentoNAAPA,
                RespostaEncaminhamentoNAAPA,
                SecaoEncaminhamentoNAAPA,
                Questao,
                OpcaoResposta,
                EncaminhamentoNAAPA>(
                    query,
                    (encaminhamentoNAAPA, encaminhamentoSecao, questaoEncaminhamentoNAAPA, respostaEncaminhamento, secaoEncaminhamento, questao, opcaoResposta) =>
                        MapearEncaminhamento(encaminhamento, encaminhamentoNAAPA, encaminhamentoSecao, questaoEncaminhamentoNAAPA, respostaEncaminhamento, secaoEncaminhamento, questao, opcaoResposta),
                    new { id },
                    splitOn:
                       "EncaminhamentoNAAPAInicio," +
                        "SecaoInicio," +
                        "QuestaoEncaminhamentoInicio," +
                        "RespostaInicio," +
                        "SecaoConfiguradaInicio," +
                        "QuestaoInicio," +
                        "OpcaoRespostaInicio");
           encaminhamento = consulta?.FirstOrDefault() ?? new EncaminhamentoNAAPA();
            return encaminhamento;
        }

        public async Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorIdESecao(long id, long secaoId)
        {
            const string query = $@"
                    {CAMPOSSELECT}
                    FROM encaminhamento_naapa ea
                    INNER JOIN encaminhamento_naapa_secao eas
                        ON eas.encaminhamento_naapa_id = ea.id
                       AND NOT eas.excluido
                    INNER JOIN secao_encaminhamento_naapa sea
                        ON sea.id = eas.secao_encaminhamento_id
                       AND NOT sea.excluido
                    INNER JOIN encaminhamento_naapa_questao qea
                        ON qea.encaminhamento_naapa_secao_id = eas.id
                       AND NOT qea.excluido
                    INNER JOIN questao q
                        ON q.id = qea.questao_id
                       AND NOT q.excluido
                    INNER JOIN encaminhamento_naapa_resposta rea
                        ON rea.questao_encaminhamento_id = qea.id
                       AND NOT rea.excluido
                    LEFT JOIN opcao_resposta op
                        ON op.id = rea.resposta_id
                       AND NOT op.excluido
                    WHERE ea.id = @id
                      AND qea.encaminhamento_naapa_secao_id = @secaoId
                      AND NOT ea.excluido;";

            var encaminhamento = new EncaminhamentoNAAPA();

        var consulta = await database.Conexao.QueryAsync<
                EncaminhamentoNAAPA,
                EncaminhamentoNAAPASecao,
                QuestaoEncaminhamentoNAAPA,
                RespostaEncaminhamentoNAAPA,
                SecaoEncaminhamentoNAAPA,
                Questao,
                OpcaoResposta,
                EncaminhamentoNAAPA>(
                    query,
                    (encaminhamentoNAAPA, encaminhamentoSecao, questaoEncaminhamentoNAAPA, respostaEncaminhamento, secaoEncaminhamento, questao, opcaoResposta) =>
                        MapearEncaminhamento(encaminhamento, encaminhamentoNAAPA, encaminhamentoSecao, questaoEncaminhamentoNAAPA, respostaEncaminhamento, secaoEncaminhamento, questao, opcaoResposta),
                    new { id, secaoId },
                    splitOn:
                        "EncaminhamentoNAAPAInicio, " +
                        "SecaoInicio," +
                        "QuestaoEncaminhamentoInicio," +
                        "RespostaInicio," +
                        "SecaoConfiguradaInicio," +
                        "QuestaoInicio," +
                        "OpcaoRespostaInicio");

        encaminhamento = consulta?.FirstOrDefault() ?? new EncaminhamentoNAAPA();
        return encaminhamento;
        }

        public async Task<IEnumerable<EncaminhamentoNAAPACodigoArquivoDto>> ObterCodigoArquivoPorEncaminhamentoNAAPAId(long encaminhamentoId)
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

            return await database.Conexao.QueryAsync<EncaminhamentoNAAPACodigoArquivoDto>(sql.ToString(), new { encaminhamentoId });
        }

        public async Task<EncaminhamentoNAAPA> ObterEncaminhamentoComTurmaPorId(long encaminhamentoId)
        {
            const string query = @"
                SELECT
                    -- EncaminhamentoNAAPA
                    ea.id AS Id,
                    ea.criado_em AS CriadoEm,
                    ea.criado_por AS CriadoPor,
                    ea.alterado_em AS AlteradoEm,
                    ea.alterado_por AS AlteradoPor,
                    ea.alterado_rf AS AlteradoRF,
                    ea.criado_rf AS CriadoRF,
                    ea.turma_id AS TurmaId,
                    ea.aluno_codigo AS AlunoCodigo,
                    ea.aluno_nome AS AlunoNome,
                    ea.situacao AS Situacao,
                    ea.excluido AS Excluido,
                    ea.situacao_matricula_aluno
                        AS SituacaoMatriculaAluno,
                    ea.motivo_encerramento
                        AS MotivoEncerramento,
                    ea.data_ultima_notificacao_sem_atendimento
                        AS DataUltimaNotificacaoSemAtendimento,

                    -- marcador de início da Turma
                    t.id AS TurmaInicio,

                    -- Turma
                    t.id AS Id,
                    t.ano AS Ano,
                    t.ano_letivo AS AnoLetivo,
                    t.turma_id AS CodigoTurma,
                    t.tipo_turma AS TipoTurma,
                    t.data_atualizacao AS DataAtualizacao,
                    t.modalidade_codigo AS ModalidadeCodigo,
                    t.nome AS Nome,
                    t.qt_duracao_aula AS QuantidadeDuracaoAula,
                    t.semestre AS Semestre,
                    t.tipo_turno AS TipoTurno,
                    t.serie_ensino AS SerieEnsino,
                    t.ue_id AS UeId,
                    t.nome_filtro AS NomeFiltro,
                    t.historica AS Historica,
                    t.ensino_especial AS EnsinoEspecial,
                    t.data_inicio AS DataInicio,
                    t.dt_fim_eol AS DataFim,
                    t.etapa_eja AS EtapaEJA,

                    -- marcador de início da UE
                    u.id AS UeInicio,

                    -- UE
                    u.id AS Id,
                    u.ue_id AS CodigoUe,
                    u.data_atualizacao AS DataAtualizacao,
                    u.dre_id AS DreId,
                    u.nome AS Nome,
                    u.tipo_escola AS TipoEscola,

                    -- marcador de início da DRE
                    d.id AS DreInicio,

                    -- DRE
                    d.id AS Id,
                    d.dre_id AS CodigoDre,
                    d.abreviacao AS Abreviacao,
                    d.nome AS Nome,
                    d.data_atualizacao AS DataAtualizacao

                FROM encaminhamento_naapa ea
                INNER JOIN turma t
                    ON t.id = ea.turma_id
                INNER JOIN ue u
                    ON u.id = t.ue_id
                INNER JOIN dre d
                    ON d.id = u.dre_id
                WHERE ea.id = @encaminhamentoId;";

            var resultado =
                await database.Conexao.QueryAsync<
                    EncaminhamentoNAAPA,
                    Turma,
                    Ue,
                    Dre,
                    EncaminhamentoNAAPA>(
                        query,
                        (
                            encaminhamentoNAAPA,
                            turma,
                            ue,
                            dre) =>
                        {
                            ue.AdicionarDre(dre);
                            turma.AdicionarUe(ue);
                            encaminhamentoNAAPA.Turma = turma;

                            return encaminhamentoNAAPA;
                        },
                        new { encaminhamentoId },
                        splitOn: "TurmaInicio,UeInicio,DreInicio");

            return resultado.FirstOrDefault();
        }

        public async Task<bool> EncaminhamentoContemAtendimentosItinerancia(long encaminhamentoId)
        {
            var query = $@"select ens.id
                        from encaminhamento_naapa_secao ens
                        INNER JOIN secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id
                        WHERE NOT ens.excluido and sen.nome_componente = @secaoNome
                                and ens.encaminhamento_naapa_id = @id";

            return (await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { id = encaminhamentoId, secaoNome = EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA }));
        }

        public async Task<IEnumerable<EncaminhamentosNAAPAConsolidadoDto>> ObterQuantidadeSituacaoEncaminhamentosPorUeAnoLetivo(long ueId, int anoLetivo)
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

            return await database.Conexao.QueryAsync<EncaminhamentosNAAPAConsolidadoDto>(query, new { ueId, anoLetivo });
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

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { situacao = (int)SituacaoNAAPA.AguardandoAtendimento, encaminhamentoId, secaoNome = EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA });
        }

        public async Task<IEnumerable<EncaminhamentoNAAPADto>> ObterEncaminhamentosComSituacaoDiferenteDeEncerrado()
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

            return await database.Conexao.QueryAsync<EncaminhamentoNAAPADto>(query, new { situacao = (int)SituacaoNAAPA.Encerrado });
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

        public async Task<IEnumerable<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>> ObterInformacoesDeNotificacaoDeInatividadeDeAtendimento(long ueId)
        {
            var situacoes = new int[] { (int)SituacaoNAAPA.AguardandoAtendimento, (int)SituacaoNAAPA.EmAtendimento };
            var query = new StringBuilder();

            query.AppendLine("WITH inatividade_atendimento AS(");
            query.AppendLine(@$"select en.aluno_codigo AlunoCodigo, en.aluno_nome AlunoNome, en.turma_id TurmaId, en.id EncaminhamentoId
                from encaminhamento_naapa en
                inner join encaminhamento_naapa_secao ens on ens.encaminhamento_naapa_id = en.id
                where not exists(select 1
                                 from secao_encaminhamento_naapa sen
                                 where sen.nome_componente = '{EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA}'
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
                inner join secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id and sen.nome_componente = '{EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA}'
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

            return await database.Conexao.QueryAsync<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>(query.ToString(), new { ueId, situacoes });
        }
    }
}