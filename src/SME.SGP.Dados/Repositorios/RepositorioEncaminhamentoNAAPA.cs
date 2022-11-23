using Dapper;
using SME.SGP.Dominio;
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
        public RepositorioEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>> ListarPaginado(bool exibirHistorico, 
            int anoLetivo, long dreId, string codigoUe, string nomeAluno, DateTime? dataAberturaQueixaInicio, 
            DateTime? dataAberturaQueixaFim, int situacao, long prioridade, long[] turmasIds, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, exibirHistorico, anoLetivo, dreId, codigoUe, 
                nomeAluno, dataAberturaQueixaInicio, dataAberturaQueixaFim, situacao,prioridade , turmasIds);

            var parametros = new { anoLetivo, codigoUe, dreId, nomeAluno,
                turmasIds, situacao, prioridade, dataAberturaQueixaInicio, dataAberturaQueixaFim };

            var encaminhamentosNAAPA = await database.Conexao.QueryAsync<EncaminhamentoNAAPAResumoDto>(query, parametros);
            
            var acompanhamentosAgrupados = encaminhamentosNAAPA.GroupBy(x => new 
                {
                    x.Id,  
                    x.UeNome,
                    x.TipoEscola,
                    x.CodigoAluno,
                    x.NomeAluno,
                    x.Situacao,
                }).ToList()
                .Select(x => new EncaminhamentoNAAPAResumoDto
                {
                    Id = x.Key.Id,  
                    UeNome = x.Key.UeNome,
                    TipoEscola = x.Key.TipoEscola,
                    CodigoAluno = x.Key.CodigoAluno,
                    NomeAluno = x.Key.NomeAluno,
                    Situacao = x.Key.Situacao,
                    Prioridade = x.Any() && x.Any(a=> !string.IsNullOrEmpty(a.Prioridade)) 
                        ? x.FirstOrDefault(a=> !string.IsNullOrEmpty(a.Prioridade)).Prioridade : null,
                    DataAberturaQueixaInicio = x.Any() && x.Any(a=> a.DataAberturaQueixaInicio.HasValue) 
                        ? x.FirstOrDefault(b=> b.DataAberturaQueixaInicio.HasValue).DataAberturaQueixaInicio.Value  : null,
                })
                .ToList();
            
            var retorno = new PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>()
            {
                Items = acompanhamentosAgrupados.ToList(),
                TotalRegistros = acompanhamentosAgrupados.Count
            };

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }
        private string MontaQueryCompleta(Paginacao paginacao, bool exibirHistorico, int anoLetivo, long dreId, 
            string codigoUe, string nomeAluno, DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, 
            int situacao, long prioridade, long[] turmasIds)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, nomeAluno,dataAberturaQueixaInicio,
                dataAberturaQueixaFim,situacao, prioridade, turmasIds);
            
            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, nomeAluno,dataAberturaQueixaInicio,
                dataAberturaQueixaFim,situacao, prioridade, turmasIds);

            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, string nomeAluno, 
            DateTime? dataAberturaQueixaInicio, DateTime? dataAberturaQueixaFim, int situacao, long prioridade, 
            long[] turmasIds)
        {
            ObterCabecalho(sql, contador);

            ObterFiltro(sql, nomeAluno, dataAberturaQueixaInicio, dataAberturaQueixaFim,situacao, prioridade, turmasIds);
            
            if (!contador && (dataAberturaQueixaInicio.HasValue || dataAberturaQueixaFim.HasValue))
                sql.AppendLine(" order by to_date(enr.texto,'dd/MM/YYYY') desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObterCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
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
                                ,case when q.nome = 'Prioridade' then enr.resposta_id else null end Prioridade 
                                ,case when q.nome = 'Data de entrada da queixa' then to_date(enr.texto,'dd/MM/YYYY') else null end DataAberturaQueixaInicio 
                ");
            }

            sql.AppendLine(@" from encaminhamento_naapa np              
                                join turma t on t.id = np.turma_id
                                join ue on t.ue_id = ue.id
                                join encaminhamento_naapa_secao ens on np.id = ens.encaminhamento_naapa_id  
                                join encaminhamento_naapa_questao enq on ens.id = enq.encaminhamento_naapa_secao_id 
                                join questao q on enq.questao_id = q.id 
                                join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id 
            ");
        }

        private void ObterFiltro(StringBuilder sql, string nomeAluno, DateTime? dataAberturaQueixaInicio, 
            DateTime? dataAberturaQueixaFim, int situacao, long prioridade, long[] turmasIds)
        {
            sql.AppendLine(@" where not np.excluido 
                                    and t.ano_letivo = @anoLetivo 
                                    and ue.ue_id = @codigoUe 
                                    and ue.dre_Id = @dreId");

            if (!string.IsNullOrEmpty(nomeAluno))
                sql.AppendLine(" and enp.aluno_nome = @nomeAluno ");
            
            if (turmasIds.Any())
                sql.AppendLine(" and t.id = ANY(@turmasIds) ");
            
            if (situacao > 0)
                sql.AppendLine(" and np.situacao = @situacao ");
            
            if (prioridade > 0)
                sql.AppendLine(" and q.nome = 'Prioridade' and enr.resposta_id = @prioridade ");

            if (dataAberturaQueixaInicio.HasValue || dataAberturaQueixaFim.HasValue)
            {
                sql.AppendLine(" and q.nome = 'Data de entrada da queixa' ");
               
                if (dataAberturaQueixaInicio.HasValue)
                    sql.AppendLine(" and to_date(enr.texto,'dd/MM/YYYY') >= @dataAberturaQueixaInicio ");
                
                if (dataAberturaQueixaFim.HasValue)
                    sql.AppendLine(" and to_date(enr.texto,'dd/MM/YYYY') <= @dataAberturaQueixaFim  ");
            }
        }
        
        public async Task<EncaminhamentoNAAPA> ObterEncaminhamentoPorId(long id)
        {
            var query = @"select ea.*, eas.*, qea.*, rea.*, sea.*, q.*, op.*
                        from encaminhamento_naapa ea
                        inner join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = ea.id
                        inner join secao_encaminhamento_naapa sea on sea.id = eas.secao_encaminhamento_id 
                        inner join encaminhamento_naapa_questao qea on qea.encaminhamento_naapa_secao_id = eas.id
                        inner join questao q on q.id = qea.questao_id
                        inner join encaminhamento_naapa_resposta rea on rea.questao_encaminhamento_id = qea.id
                         left join opcao_resposta op on op.id = rea.resposta_id
                        where ea.id = @id";

            var encaminhamento = new EncaminhamentoNAAPA();

            await database.Conexao.QueryAsync<EncaminhamentoNAAPA, EncaminhamentoNAAPASecao, QuestaoEncaminhamentoNAAPA, RespostaEncaminhamentoNAAPA, SecaoEncaminhamentoNAAPA, Questao, OpcaoResposta, EncaminhamentoNAAPA>(query
                , (encaminhamentoNAAPA, encaminhamentoSecao, questaoEncaminhamentoNAAPA, respostaEncaminhamento, secaoEncaminhamento, questao, opcaoResposta) =>
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
    }
}
