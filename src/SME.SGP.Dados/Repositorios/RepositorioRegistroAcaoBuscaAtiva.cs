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
    public class RepositorioRegistroAcaoBuscaAtiva : RepositorioBase<RegistroAcaoBuscaAtiva>, IRepositorioRegistroAcaoBuscaAtiva
    {
        public const string QUESTAO_DATA_REGISTRO_NOME_COMPONENTE = "'DATA_REGISTRO_ACAO'";
        public const string QUESTAO_PROCEDIMENTO_REALIZADO_NOME_COMPONENTE = "'PROCEDIMENTO_REALIZADO'";
        public const string QUESTAO_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP_NOME_COMPONENTE = "'PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP'";
        public const string QUESTAO_CONTATO_COM_RESPONSAVEL_NOME_COMPONENTE = "'CONTATO_COM_RESPONSAVEL'";
        public const string QUESTAO_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA_NOME_COMPONENTE = "'APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA'";
        public const int SECAO_ETAPA_1 = 1;
        public const int SECAO_ORDEM_1 = 1;
        public const int FILTRO_TODOS = -99;

        public RepositorioRegistroAcaoBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {}

        public async Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>> ListarPaginadoCriancasEstudantesAusentes(string codigoAluno, long turmaId, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, codigoAluno, turmaId);
            var parametros = new
            { codigoAluno, turmaId };

            var retorno = new PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>();
            using (var registroAcao = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = registroAcao.Read<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>();
                retorno.TotalRegistros = registroAcao.ReadFirst<int>();
            }
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }
        private string MontaQueryCompleta(Paginacao paginacao, string codigoAluno, long? turmaId)
        {
            var sql = new StringBuilder();
            MontaQueryConsulta(paginacao, sql, contador: false, codigoAluno, turmaId);
            sql.AppendLine(";");
            MontaQueryConsulta(paginacao, sql, contador: true, codigoAluno, turmaId);
            return sql.ToString();
        }

        private string MontaQueryCompleta(Paginacao paginacao, FiltroTurmaRegistrosAcaoDto filtroTurma,
                                          FiltroRespostaRegistrosAcaoDto filtroRespostas)
        {
            var sql = new StringBuilder();
            MontaQueryConsulta(paginacao, sql, contador: false, 
                               filtroTurma,
                               filtroRespostas);
            sql.AppendLine(";");
            MontaQueryConsulta(paginacao, sql, contador: true,
                               filtroTurma,
                               filtroRespostas);
            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, string codigoAluno, long? turmaId)
        {
            ObterCabecalho(sql, contador);
            ObterFiltro(sql, codigoAluno, turmaId);

            if (!contador)
                sql.AppendLine(" order by to_date(qdata.DataRegistro,'yyyy-mm-dd') desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador,
                                        FiltroTurmaRegistrosAcaoDto filtroTurma,
                                        FiltroRespostaRegistrosAcaoDto filtroRespostas)
        {
            ObterCabecalhoRegistrosAcao(sql, contador);
            ObterFiltro(sql, filtroTurma, filtroRespostas);

            if (!contador)
                sql.AppendLine(" order by to_date(qdata.DataRegistro,'yyyy-mm-dd') desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObterCabecalho(StringBuilder sql, bool contador)
        {
            var sqlSelect = $@"with vw_resposta_data as (
                                select rabas.registro_acao_busca_ativa_id, 
                                       rabar.texto DataRegistro
                                from registro_acao_busca_ativa_secao rabas    
                                join registro_acao_busca_ativa_questao rabaq on rabas.id = rabaq.registro_acao_busca_ativa_secao_id  
                                join questao q on rabaq.questao_id = q.id 
                                join registro_acao_busca_ativa_resposta rabar on rabar.questao_registro_acao_id = rabaq.id 
                                join secao_registro_acao_busca_ativa sraba on sraba.id = rabas.secao_registro_acao_id
                                left join opcao_resposta opr on opr.id = rabar.resposta_id
                                where q.nome_componente = {QUESTAO_DATA_REGISTRO_NOME_COMPONENTE} 
                                      and sraba.etapa = {SECAO_ETAPA_1} 
                                      and sraba.ordem = {SECAO_ORDEM_1} 
                                )
                                select ";
            sql.AppendLine(sqlSelect);
            if (contador)
                sql.AppendLine("count(raba.id) ");
            else
            {
                sql.AppendLine(@"raba.id
                                ,case when length(qdata.DataRegistro) > 0 then to_date(qdata.DataRegistro,'yyyy-mm-dd') else null end DataRegistro
                                ,raba.criado_por || ' (' || raba.criado_rf || ')' as usuario
                ");
            }
            sql.AppendLine(@" from registro_acao_busca_ativa raba 
                              left join vw_resposta_data qdata on qdata.registro_acao_busca_ativa_id = raba.id");
        }

        private static void ObterCabecalhoRegistrosAcao(StringBuilder sql, bool contador)
        {
            var sqlSelect = $@"with vw_resposta_data as (
                                select rabas.registro_acao_busca_ativa_id, 
                                       rabar.texto DataRegistro
                                from registro_acao_busca_ativa_secao rabas    
                                join registro_acao_busca_ativa_questao rabaq on rabas.id = rabaq.registro_acao_busca_ativa_secao_id  
                                join questao q on rabaq.questao_id = q.id 
                                join registro_acao_busca_ativa_resposta rabar on rabar.questao_registro_acao_id = rabaq.id 
                                join secao_registro_acao_busca_ativa sraba on sraba.id = rabas.secao_registro_acao_id
                                left join opcao_resposta opr on opr.id = rabar.resposta_id
                                where q.nome_componente = {QUESTAO_DATA_REGISTRO_NOME_COMPONENTE} 
                                      and sraba.etapa = {SECAO_ETAPA_1} 
                                      and sraba.ordem = {SECAO_ORDEM_1} 
                                )
                              , vw_resposta_procedimento_realizado as (
                                select rabas.registro_acao_busca_ativa_id, 
                                       opr.nome as ProcedimentoRealizado,
                                       rabar.resposta_id  as ProcedimentoRealizadoId,
                                       opr.ordem as ProcedimentoRealizadoOrdem
                                from registro_acao_busca_ativa_secao rabas    
                                join registro_acao_busca_ativa_questao rabaq on rabas.id = rabaq.registro_acao_busca_ativa_secao_id  
                                join questao q on rabaq.questao_id = q.id 
                                join registro_acao_busca_ativa_resposta rabar on rabar.questao_registro_acao_id = rabaq.id 
                                join secao_registro_acao_busca_ativa sraba on sraba.id = rabas.secao_registro_acao_id
                                left join opcao_resposta opr on opr.id = rabar.resposta_id
                                where (q.nome_componente = {QUESTAO_PROCEDIMENTO_REALIZADO_NOME_COMPONENTE}
                                       or q.nome_componente = {QUESTAO_PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP_NOME_COMPONENTE})
                                      and sraba.etapa = {SECAO_ETAPA_1} 
                                      and sraba.ordem = {SECAO_ORDEM_1}
                                      and not rabar.excluido 
                                )
                              , vw_resposta_contato_com_responsavel as (
                                select rabas.registro_acao_busca_ativa_id, 
                                       opr.nome as ContatoRealizado,
                                       rabar.resposta_id  as ContatoRealizadoId
                                from registro_acao_busca_ativa_secao rabas    
                                join registro_acao_busca_ativa_questao rabaq on rabas.id = rabaq.registro_acao_busca_ativa_secao_id  
                                join questao q on rabaq.questao_id = q.id 
                                join registro_acao_busca_ativa_resposta rabar on rabar.questao_registro_acao_id = rabaq.id 
                                join secao_registro_acao_busca_ativa sraba on sraba.id = rabas.secao_registro_acao_id
                                left join opcao_resposta opr on opr.id = rabar.resposta_id
                                where q.nome_componente = {QUESTAO_CONTATO_COM_RESPONSAVEL_NOME_COMPONENTE}
                                      and sraba.etapa = {SECAO_ETAPA_1} 
                                      and sraba.ordem = {SECAO_ORDEM_1}
                                      and not rabar.excluido 
                                )
                                , vw_resposta_retornou_escola_apos_ligacao as (
                                select rabas.registro_acao_busca_ativa_id, 
                                       opr.nome as RetornouAposLigacao,
                                       rabar.resposta_id  as RetornouAposLigacaoId
                                from registro_acao_busca_ativa_secao rabas    
                                join registro_acao_busca_ativa_questao rabaq on rabas.id = rabaq.registro_acao_busca_ativa_secao_id  
                                join questao q on rabaq.questao_id = q.id 
                                join registro_acao_busca_ativa_resposta rabar on rabar.questao_registro_acao_id = rabaq.id 
                                join secao_registro_acao_busca_ativa sraba on sraba.id = rabas.secao_registro_acao_id
                                left join opcao_resposta opr on opr.id = rabar.resposta_id
                                where q.nome_componente = {QUESTAO_APOS_CONTATO_CRIANCA_RETORNOU_ESCOLA_NOME_COMPONENTE}
                                      and sraba.etapa = {SECAO_ETAPA_1} 
                                      and sraba.ordem = {SECAO_ORDEM_1}
                                      and not rabar.excluido 
                                )
                                select ";
            sql.AppendLine(sqlSelect);
            if (contador)
                sql.AppendLine("count(raba.id) ");
            else
            {
                sql.AppendLine(@"raba.id,
                                t.nome as NomeTurma,
                                t.modalidade_codigo as Modalidade,
                                raba.aluno_nome as NomeAluno,
                                raba.aluno_codigo CodigoAluno
                                ,case when length(qdata.DataRegistro) > 0 then to_date(qdata.DataRegistro,'yyyy-mm-dd') else null end DataRegistro
                                ,raba.criado_por as NomeUsuarioCriador
                                ,raba.criado_em as DataCriacao
                                ,qProcedRealizado.ProcedimentoRealizado
                                ,qContatoEfetuadoComResponsavel.ContatoRealizado as ContatoEfetuadoResponsavel
                                ,qRetornouEscolaAposLigacao.RetornouAposLigacao as CriancaRetornouEscolaAposContato
                ");
            }
            sql.AppendLine(@" from registro_acao_busca_ativa raba 
                              inner join turma t on t.id = raba.turma_id 
                              inner join ue u on u.id = t.ue_id 
                              inner join dre d on d.id = u.dre_id 
                              left join vw_resposta_data qdata on qdata.registro_acao_busca_ativa_id = raba.id
                              left join vw_resposta_procedimento_realizado qProcedRealizado on qProcedRealizado.registro_acao_busca_ativa_id = raba.id
                              left join vw_resposta_contato_com_responsavel qContatoEfetuadoComResponsavel on qContatoEfetuadoComResponsavel.registro_acao_busca_ativa_id = raba.id
                              left join vw_resposta_retornou_escola_apos_ligacao qRetornouEscolaAposLigacao on qRetornouEscolaAposLigacao.registro_acao_busca_ativa_id = raba.id");
        }

        private void ObterFiltro(StringBuilder sql, string codigoAluno, long? turmaId)
        {
            sql.AppendLine(@" where not raba.excluido ");
            if (turmaId.HasValue)
                sql.AppendLine(" and raba.turma_id = @turmaId ");
            if (!string.IsNullOrEmpty(codigoAluno))
                sql.AppendLine(" and raba.aluno_codigo = @codigoAluno ");
        }

        private void ObterFiltro(StringBuilder sql, FiltroTurmaRegistrosAcaoDto filtroTurma,
                                 FiltroRespostaRegistrosAcaoDto filtroRespostas)
        {
            sql.AppendLine(@" where not raba.excluido ");
            if (filtroTurma.AnoLetivo > 0)
                sql.AppendLine(" and t.ano_letivo = @anoLetivo ");
            if (filtroTurma.DreId.HasValue && filtroTurma.DreId.Value != FILTRO_TODOS)
                sql.AppendLine(" and d.id = @dreId ");
            if (filtroTurma.UeId.HasValue && filtroTurma.UeId.Value != FILTRO_TODOS)
                sql.AppendLine(" and u.id = @ueId ");
            if (filtroTurma.TurmaId.HasValue && filtroTurma.TurmaId.Value != FILTRO_TODOS)
                sql.AppendLine(" and raba.turma_id = @turmaId ");
            if (filtroTurma.Modalidade.HasValue && filtroTurma.Modalidade.Value != FILTRO_TODOS)
                sql.AppendLine(" and t.modalidade_codigo = @modalidade ");
            if (filtroTurma.Semestre != 0)
                sql.AppendLine(" and t.semestre = @semestre ");
            if (!string.IsNullOrEmpty(filtroRespostas.NomeAluno))
                sql.AppendLine(" and lower(raba.aluno_nome) like @nomeAluno ");
            if (filtroRespostas.DataRegistroInicio.HasValue)
                sql.AppendLine(" and to_date(qdata.DataRegistro,'yyyy-mm-dd') >= @dataRegistroInicio ");
            if (filtroRespostas.DataRegistroFim.HasValue)
                sql.AppendLine(" and to_date(qdata.DataRegistro,'yyyy-mm-dd') <= @dataRegistroFim ");
            if (filtroRespostas.OrdemRespostaQuestaoProcedimentoRealizado.HasValue)
                sql.AppendLine(" and qProcedRealizado.ProcedimentoRealizadoOrdem = @ordemRespostaQuestaoProcedimentoRealizado ");
        }

        public async Task<IEnumerable<string>> ObterCodigoArquivoPorRegistroAcaoId(long id)
        {
            var sql = @"select
                            a.codigo
                        from
                            registro_acao_busca_ativa raba
                        inner join registro_acao_busca_ativa_secao rabas on
                            raba.id = rabas.registro_acao_busca_ativa_id
                        inner join registro_acao_busca_ativa_questao qraba on
                            rabas.id = qraba.registro_acao_busca_ativa_secao_id
                        inner join registro_acao_busca_ativa_resposta rraba on
                            qraba.id = rraba.questao_registro_acao_id
                        inner join arquivo a on
                            rraba.arquivo_id = a.id
                        where
                            raba.id = @id";

            return await database.Conexao.QueryAsync<string>(sql.ToString(), new { id });
        }

        public async Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoComTurmaPorId(long id)
        {
            var query = @" select raba.*, t.*, ue.*, dre.*
                            from registro_acao_busca_ativa raba
                           inner join turma t on t.id = raba.turma_id
                            join ue on ue.id = t.ue_id
                            join dre on dre.id = ue.dre_id  
                           where raba.id = @id";

            return (await database.Conexao.QueryAsync<RegistroAcaoBuscaAtiva, Turma, Ue, Dre, RegistroAcaoBuscaAtiva>(query,
                (registroAcao, turma, ue, dre) =>
                {
                    registroAcao.Turma = turma;
                    registroAcao.Turma.Ue = ue;
                    registroAcao.Turma.Ue.Dre = dre;

                    return registroAcao;
                }, new { id })).FirstOrDefault();
        }

        public async Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoPorId(long id)
        {
            const string query = @" select raba.*, rabas.*, qraba.*, rraba.*, sraba.*, q.*, op.*
                                    from registro_acao_busca_ativa raba
                                    inner join registro_acao_busca_ativa_secao rabas on rabas.registro_acao_busca_ativa_id = raba.id
                                        and not rabas.excluido
                                    inner join secao_registro_acao_busca_ativa sraba on sraba.id = rabas.secao_registro_acao_id
                                        and not sraba.excluido
                                    inner join registro_acao_busca_ativa_questao qraba on qraba.registro_acao_busca_ativa_secao_id = rabas.id
                                        and not qraba.excluido
                                    inner join questao q on q.id = qraba.questao_id
                                        and not q.excluido
                                    inner join registro_acao_busca_ativa_resposta rraba on rraba.questao_registro_acao_id = qraba.id
                                        and not rraba.excluido
                                    left join opcao_resposta op on op.id = rraba.resposta_id
                                        and not op.excluido
                                    where raba.id = @id
                                    and not raba.excluido;";

            var registroAcao = new RegistroAcaoBuscaAtiva();

            await database.Conexao
                .QueryAsync<RegistroAcaoBuscaAtiva, RegistroAcaoBuscaAtivaSecao, QuestaoRegistroAcaoBuscaAtiva,
                    RespostaRegistroAcaoBuscaAtiva, SecaoRegistroAcaoBuscaAtiva, Questao, OpcaoResposta, RegistroAcaoBuscaAtiva>(
                    query, (RegistroAcaoBuscaAtiva, registroAcaoSecao, questaoRegistroAcaoBuscaAtiva, respostaRegistroAcao,
                        secaoRegistroAcao, questao, opcaoResposta) =>
                    {
                        if (registroAcao.Id == 0)
                            registroAcao = RegistroAcaoBuscaAtiva;

                        var secao = registroAcao.Secoes.FirstOrDefault(c => c.Id == registroAcaoSecao.Id);

                        if (secao.EhNulo())
                        {
                            registroAcaoSecao.SecaoRegistroAcaoBuscaAtiva = secaoRegistroAcao;
                            secao = registroAcaoSecao;
                            registroAcao.Secoes.Add(secao);
                        }

                        var questaoRegistroAcao = secao.Questoes.FirstOrDefault(c => c.Id == questaoRegistroAcaoBuscaAtiva.Id);

                        if (questaoRegistroAcao.EhNulo())
                        {
                            questaoRegistroAcao = questaoRegistroAcaoBuscaAtiva;
                            questaoRegistroAcao.Questao = questao;
                            secao.Questoes.Add(questaoRegistroAcao);
                        }

                        var resposta = questaoRegistroAcao.Respostas.FirstOrDefault(c => c.Id == respostaRegistroAcao.Id);

                        if (resposta.EhNulo())
                        {
                            resposta = respostaRegistroAcao;
                            resposta.Resposta = opcaoResposta;
                            questaoRegistroAcao.Respostas.Add(resposta);
                        }

                        return registroAcao;
                    }, new { id });

            return registroAcao;
        }

        public async Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>> ListarPaginado(FiltroTurmaRegistrosAcaoDto filtroTurma,
                                                                                                   FiltroRespostaRegistrosAcaoDto filtroRespostas,
                                                                                                   Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, filtroTurma,
                                            filtroRespostas);
            if (!string.IsNullOrWhiteSpace(filtroRespostas.NomeAluno))
                filtroRespostas.NomeAluno = $"%{filtroRespostas.NomeAluno.ToLower()}%";

            var parametros = new
            {
                filtroTurma.AnoLetivo,
                filtroTurma.DreId,
                filtroTurma.UeId,
                filtroTurma.TurmaId,
                filtroTurma.Modalidade,
                filtroTurma.Semestre,
                filtroRespostas.NomeAluno,
                filtroRespostas.DataRegistroInicio,
                filtroRespostas.DataRegistroFim,
                filtroRespostas.OrdemRespostaQuestaoProcedimentoRealizado
            };

            var retorno = new PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>();
            using (var registroAcao = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                var items = registroAcao.Read<RegistroAcaoBuscaAtivaListagemDto>().ToList();
                items.ForEach(i =>
                {
                    if (string.IsNullOrEmpty(i.ContatoEfetuadoResponsavel))
                        i.ContatoEfetuadoResponsavel = "Não";
                    if (string.IsNullOrEmpty(i.CriancaRetornouEscolaAposContato))
                        i.CriancaRetornouEscolaAposContato = "Não";
                });
                retorno.Items = items;
                retorno.TotalRegistros = registroAcao.ReadFirst<int>();
            }
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }
    }
}
