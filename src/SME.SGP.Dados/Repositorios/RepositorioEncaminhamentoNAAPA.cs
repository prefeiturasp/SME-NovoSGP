using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoNAAPA : RepositorioBase<EncaminhamentoNAAPA>, IRepositorioEncaminhamentoNAAPA
    {
        public const int QUESTAO_DATA_QUEIXA_ORDEM = 0;
        public const int QUESTAO_PRIORIDADE_ORDEM = 1;
        public const int SECAO_ETAPA_1 = 1;
        public const int SECAO_INFORMACOES_ALUNO_ORDEM = 1;

        public RepositorioEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>> ListarPaginado(int anoLetivo, long dreId, 
            string codigoUe, string nomeAluno, DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, 
            int situacao, long prioridade, long[] turmasIds, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, dreId, codigoUe, nomeAluno, dataAberturaQueixaInicio, 
                dataAberturaQueixaFim, situacao,prioridade , turmasIds);

            if (!string.IsNullOrWhiteSpace(nomeAluno))
                nomeAluno = $"%{nomeAluno.ToLower()}%";
            
            var parametros = new { anoLetivo, codigoUe, dreId, nomeAluno,
                turmasIds, situacao, prioridade, dataAberturaQueixaInicio, dataAberturaQueixaFim };

            var retorno = new PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>();
            
            using (var encaminhamentosNAAPA = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = encaminhamentosNAAPA.Read<EncaminhamentoNAAPAResumoDto>();
                retorno.TotalRegistros = encaminhamentosNAAPA.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }
        private string MontaQueryCompleta(Paginacao paginacao, long dreId, string codigoUe, string nomeAluno, 
            DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, int situacao, long prioridade, long[] turmasIds)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, nomeAluno,dataAberturaQueixaInicio,
                dataAberturaQueixaFim,situacao, prioridade, turmasIds, codigoUe);
            
            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, nomeAluno,dataAberturaQueixaInicio,
                dataAberturaQueixaFim,situacao, prioridade, turmasIds, codigoUe);

            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, string nomeAluno, 
            DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, int situacao, long prioridade, 
            long[] turmasIds, string codigoUe)
        {
            ObterCabecalho(sql, contador);

            ObterFiltro(sql, nomeAluno, dataAberturaQueixaInicio, dataAberturaQueixaFim,situacao, prioridade, turmasIds, codigoUe);
            
            if (!contador)
                sql.AppendLine(" order by to_date(qdata.DataAberturaQueixaInicio,'yyyy-mm-dd') desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
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
                ");
            }

            sql.AppendLine(@" from encaminhamento_naapa np              
                                join turma t on t.id = np.turma_id
                                join ue on t.ue_id = ue.id
                                left join vw_resposta_data qdata on qdata.encaminhamento_naapa_id = np.id
                                left join vw_resposta_prioridade qprioridade on qprioridade.encaminhamento_naapa_id = np.id 
            ");
        }

        private void ObterFiltro(StringBuilder sql, string nomeAluno, DateTime? dataAberturaQueixaInicio, 
            DateTime? dataAberturaQueixaFim, int situacao, long prioridade, long[] turmasIds, string codigoUe)
        {
            sql.AppendLine(@" where not np.excluido 
                                    and t.ano_letivo = @anoLetivo
                                    and ue.dre_Id = @dreId"); 

            if (!string.IsNullOrEmpty(codigoUe))
                sql.AppendLine(@" and ue.ue_id = @codigoUe ");

            if (!string.IsNullOrEmpty(nomeAluno))
                sql.AppendLine(" and lower(np.aluno_nome) like @nomeAluno ");
            
            if (turmasIds.Any())
                sql.AppendLine(" and t.id = ANY(@turmasIds) ");
            
            if (situacao > 0)
                sql.AppendLine(" and np.situacao = @situacao ");
            
            if (prioridade > 0)
                sql.AppendLine(" and qPrioridade.PrioridadeId = @prioridade ");

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
                        
                        if (secao == null)
                        {
                            encaminhamentoSecao.SecaoEncaminhamentoNAAPA = secaoEncaminhamento;
                            secao = encaminhamentoSecao;
                            encaminhamento.Secoes.Add(secao);
                        }

                        var questaoEncaminhamento = secao.Questoes.FirstOrDefault(c => c.Id == questaoEncaminhamentoNAAPA.Id);
                        
                        if (questaoEncaminhamento == null)
                        {
                            questaoEncaminhamento = questaoEncaminhamentoNAAPA;
                            questaoEncaminhamento.Questao = questao;
                            secao.Questoes.Add(questaoEncaminhamento);
                        }

                        var resposta = questaoEncaminhamento.Respostas.FirstOrDefault(c => c.Id == respostaEncaminhamento.Id);
                        
                        if (resposta == null)
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
                        
                        if (secao == null)
                        {
                            encaminhamentoSecao.SecaoEncaminhamentoNAAPA = secaoEncaminhamento;
                            secao = encaminhamentoSecao;
                            encaminhamento.Secoes.Add(secao);
                        }

                        var questaoEncaminhamento = secao.Questoes.FirstOrDefault(c => c.Id == questaoEncaminhamentoNAAPA.Id);
                        
                        if (questaoEncaminhamento == null)
                        {
                            questaoEncaminhamento = questaoEncaminhamentoNAAPA;
                            questaoEncaminhamento.Questao = questao;
                            secao.Questoes.Add(questaoEncaminhamento);
                        }

                        var resposta = questaoEncaminhamento.Respostas.FirstOrDefault(c => c.Id == respostaEncaminhamento.Id);

                        if (resposta != null) 
                            return encaminhamento;
                        
                        resposta = respostaEncaminhamento;
                        resposta.Resposta = opcaoResposta;
                        questaoEncaminhamento.Respostas.Add(resposta);

                        return encaminhamento;
                    }, new { id, secaoId });

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
    }
}
