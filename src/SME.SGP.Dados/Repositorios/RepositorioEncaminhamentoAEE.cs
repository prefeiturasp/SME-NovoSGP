using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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
    public class RepositorioEncaminhamentoAEE : RepositorioBase<EncaminhamentoAEE>, IRepositorioEncaminhamentoAEE
    {
        public RepositorioEncaminhamentoAEE(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf, int anoLetivo, string[] turmasCodigos, Paginacao paginacao, bool exibirEncerrados)
        {
            var query = MontaQueryCompleta(paginacao, ueId, turmaId, alunoCodigo, situacao, responsavelRf, turmasCodigos, exibirEncerrados);
            var situacoesEncerrado = new int[] { (int)SituacaoAEE.Encerrado, (int)SituacaoAEE.EncerradoAutomaticamente };
            var parametros = new { dreId, ueId, turmaId, alunoCodigo, situacao, responsavelRf, anoLetivo, turmasCodigos, situacoesEncerrado };
            var retorno = new PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<EncaminhamentoAEEAlunoTurmaDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private static string MontaQueryCompleta(Paginacao paginacao, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf, string[] turmasCodigos, bool exibirEncerrados)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, ueId, turmaId, alunoCodigo, situacao, responsavelRf, turmasCodigos, exibirEncerrados);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, ueId, turmaId, alunoCodigo, situacao, responsavelRf, turmasCodigos, exibirEncerrados);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf, string[] turmasCodigos, bool exibirEncerrados)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, ueId, turmaId, alunoCodigo, situacao, responsavelRf, turmasCodigos, exibirEncerrados);

            if (!contador)
                sql.AppendLine(" order by ueOrdem, ea.aluno_nome ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
            if (contador)
                sql.AppendLine(" count(ea.id) ");
            else
            {
                sql.AppendLine(" ea.id ");
                sql.AppendLine(", ea.aluno_codigo as AlunoCodigo ");
                sql.AppendLine(", ea.aluno_nome as AlunoNome ");
                sql.AppendLine(", t.turma_id as TurmaCodigo ");
                sql.AppendLine(", t.nome as TurmaNome ");
                sql.AppendLine(", t.modalidade_codigo as TurmaModalidade ");
                sql.AppendLine(", t.ano_letivo as AnoTurma ");
                sql.AppendLine(", ea.situacao ");
                sql.AppendLine(", u.nome as Responsavel ");
                sql.AppendLine(", ue.nome UeNome ");
                sql.AppendLine(", ue.tipo_escola TipoEscola ");
                sql.AppendLine(", te.descricao  || ' ' || ue.nome  ueOrdem");
            }

            sql.AppendLine(" from encaminhamento_aee ea ");
            sql.AppendLine(" inner join turma t on t.id = ea.turma_id");
            sql.AppendLine(" inner join ue on t.ue_id = ue.id");
            sql.AppendLine(" inner join tipo_escola te on ue.tipo_escola = te.cod_tipo_escola_eol");
            sql.AppendLine("  left join usuario u on u.id = ea.responsavel_id");
        }

        private static void ObtenhaFiltro(StringBuilder sql, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf, string[] turmasCodigos, bool exibirEncerrados)
        {
            sql.AppendLine(" where not ea.excluido ");

            if (ueId > 0)
                sql.AppendLine(" and ue.id = @ueId ");

            if (!string.IsNullOrEmpty(alunoCodigo))
                sql.AppendLine(" and ea.aluno_codigo = @alunoCodigo ");
            if (situacao.HasValue && situacao > 0)
                sql.AppendLine(" and ea.situacao = @situacao ");
            if (!string.IsNullOrEmpty(responsavelRf))
                sql.AppendLine(" and u.rf_codigo = @responsavelRf ");
            if (!exibirEncerrados)
                sql.AppendLine(" and not ea.situacao = ANY(@situacoesEncerrado) ");

            sql.AppendLine(" and ((ue.dre_id = @dreId ");
            sql.AppendLine(" and t.ano_letivo = @anoLetivo ");

            if (turmaId > 0)
                sql.AppendLine(" and t.id = @turmaId ");
            if (turmasCodigos.NaoEhNulo() && turmaId == 0)
                sql.AppendLine(" and t.turma_id = ANY(@turmasCodigos) ");

            sql.AppendLine(" ) ");

            //-> considerar turmas de srm e regular onde o aluno possui matricula
            sql.AppendLine("    or exists(select 1 ");
            sql.AppendLine("              from encaminhamento_aee_turma_aluno eta ");
            sql.AppendLine("              inner join turma t2 on t2.id = eta.turma_id ");
            sql.AppendLine("              inner join ue u2 on t2.ue_id = u2.id");
            sql.AppendLine("              where eta.encaminhamento_aee_id = ea.id ");
            sql.AppendLine("                and u2.dre_id = @dreId ");
            sql.AppendLine("                and t2.ano_letivo = @anoLetivo ");

            if (ueId > 0)
                sql.AppendLine("                and t2.ue_id = @ueId ");
            if (turmaId > 0)
                sql.AppendLine("                and t2.id = @turmaId ");
            if (turmasCodigos.NaoEhNulo() && turmaId == 0)
                sql.AppendLine("                and t2.turma_id = ANY(@turmasCodigos) ");

            sql.AppendLine("              limit 1)) ");
        }

        public async Task<SituacaoAEE> ObterSituacaoEncaminhamentoAEE(long encaminhamentoAEEId)
        {
            var query = "select situacao from encaminhamento_aee ea where id = @encaminhamentoAEEId";

            return await database.Conexao.QueryFirstOrDefaultAsync<SituacaoAEE>(query, new { encaminhamentoAEEId });
        }

        public async Task<EncaminhamentoAEE> ObterEncaminhamentoPorId(long id)
        {
            var query = @"select ea.*, eas.*, qea.*, rea.*, sea.*, q.*, op.*
                        from encaminhamento_aee ea
                        inner join encaminhamento_aee_secao eas on eas.encaminhamento_aee_id = ea.id
                        inner join secao_encaminhamento_aee sea on sea.id = eas.secao_encaminhamento_id 
                        inner join questao_encaminhamento_aee qea on qea.encaminhamento_aee_secao_id = eas.id
                        inner join questao q on q.id = qea.questao_id
                        inner join resposta_encaminhamento_aee rea on rea.questao_encaminhamento_id = qea.id
                         left join opcao_resposta op on op.id = rea.resposta_id
                        where ea.id = @id";

            var encaminhamento = new EncaminhamentoAEE();

            await database.Conexao.QueryAsync<EncaminhamentoAEE, EncaminhamentoAEESecao, QuestaoEncaminhamentoAEE, RespostaEncaminhamentoAEE, SecaoEncaminhamentoAEE, Questao, OpcaoResposta, EncaminhamentoAEE>(query.ToString()
                , (encaminhamentoAEE, encaminhamentoSecao, questaoEncaminhamentoAEE, respostaEncaminhamento, secaoEncaminhamento, questao, opcaoResposta) =>
            {
                if (encaminhamento.Id == 0)
                    encaminhamento = encaminhamentoAEE;

                var secao = encaminhamento.Secoes.FirstOrDefault(c => c.Id == encaminhamentoSecao.Id);
                if (secao.EhNulo())
                {
                    encaminhamentoSecao.SecaoEncaminhamentoAEE = secaoEncaminhamento;
                    secao = encaminhamentoSecao;
                    encaminhamento.Secoes.Add(secao);
                }

                var questaoEncaminhamento = secao.Questoes.FirstOrDefault(c => c.Id == questaoEncaminhamentoAEE.Id);
                if (questaoEncaminhamento.EhNulo())
                {
                    questaoEncaminhamento = questaoEncaminhamentoAEE;
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

        public async Task<EncaminhamentoAEE> ObterEncaminhamentoComTurmaPorId(long encaminhamentoId)
        {
            var query = @" select ea.*, t.*, ue.*, u.*
                            from encaminhamento_aee ea
                           inner join turma t on t.id = ea.turma_id
                            left join usuario u on u.id = ea.responsavel_id
                            join ue on ue.id = t.ue_id
                           where ea.id = @encaminhamentoId";

            return (await database.Conexao.QueryAsync<EncaminhamentoAEE, Turma, Ue, Usuario, EncaminhamentoAEE>(query,
                (encaminhamentoAEE, turma, ue, usuario) =>
                {
                    encaminhamentoAEE.Turma = turma;
                    encaminhamentoAEE.Turma.Ue = ue;
                    encaminhamentoAEE.Responsavel = usuario;
                    return encaminhamentoAEE;
                }, new { encaminhamentoId })).FirstOrDefault();
        }

        public async Task<EncaminhamentoAEEAlunoTurmaDto> ObterEncaminhamentoPorEstudante(string estudanteCodigo, string ueCodigo)
        {
            var sql = @"select ea.id 
                            , ea.aluno_codigo as AlunoCodigo 
                            , ea.aluno_nome as AlunoNome 
                            , t.turma_id as TurmaCodigo 
                            , t.nome as TurmaNome 
                            , t.modalidade_codigo as TurmaModalidade 
                            , t.ano_letivo as AnoTurma 
                            , ea.situacao 
                         from encaminhamento_aee ea 
                         inner join turma t on t.id = ea.turma_id
                         inner join ue on t.ue_id = ue.id 
                     where not ea.excluido 
                       and ea.situacao not in (4, 5)
                       and ea.aluno_codigo = @estudanteCodigo
                       and ue.ue_id = @ueCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<EncaminhamentoAEEAlunoTurmaDto>(sql, new { estudanteCodigo, ueCodigo });
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterResponsaveis(long dreId, long ueId, long turmaId, string alunoCodigo, int anoLetivo, int? situacao, bool exibirEncerrados)
        {
            var sql = new StringBuilder(@"select distinct u.rf_codigo as CodigoRf
                                        , u.nome as NomeServidor
                                      from encaminhamento_aee ea 
                                     inner join turma t on t.id = ea.turma_id
                                     inner join ue on t.ue_id = ue.id
                                     inner join usuario u on u.id = ea.responsavel_id ");

            var situacoesEncerrado = new int[] { (int)SituacaoAEE.Encerrado, (int)SituacaoAEE.EncerradoAutomaticamente };

            ObtenhaFiltro(sql, ueId, turmaId, alunoCodigo, situacao, "", null, exibirEncerrados);

            return await database.Conexao.QueryAsync<UsuarioEolRetornoDto>(sql.ToString(), new { dreId, ueId, turmaId, alunoCodigo, situacao, anoLetivo, situacoesEncerrado });
        }

        public async Task<DashboardAEEEncaminhamentosDto> ObterDashBoardAEEEncaminhamentos(int ano, long dreId, long ueId)
        {
            var sql = new StringBuilder(@"select situacao, count(ea.id) as Quantidade from encaminhamento_aee ea ");
            sql.Append(" inner join turma t on ea.turma_id = t.id ");
            sql.Append(" inner join ue on t.ue_id = ue.id ");

            var where = new StringBuilder(@" where t.ano_letivo = @ano ");

            if (dreId > 0)
                where.Append(" and ue.dre_id = @dreId");
            
            if (ueId > 0)
                where.Append(" and ue.id = @ueId");
            
            sql.Append(where.ToString());

            sql.Append(" group by ea.situacao; ");

            sql.Append(ObterQueryTotalEncaminhamento(where.ToString(), false));
            sql.Append(ObterQueryTotalEncaminhamento(where.ToString(), true));

            var situacaoEmAnalise = new int[] 
            { 
                (int)SituacaoAEE.AtribuicaoPAAI, 
                (int)SituacaoAEE.Encaminhado, 
                (int)SituacaoAEE.Analise 
            };

            var retorno = new DashboardAEEEncaminhamentosDto();

            using (var multi = await database.Conexao.QueryMultipleAsync(sql.ToString(), new { ano, dreId, ueId, situacaoEmAnalise }))
            {
                retorno.SituacoesEncaminhamentoAEE = multi.Read<AEESituacaoEncaminhamentoDto>();
                retorno.QtdeEncaminhamentosSituacao = multi.ReadFirst<long>();
                retorno.TotalEncaminhamentosAnalise = multi.ReadFirst<long>();
            }

            return retorno;
        }

        public async Task<IEnumerable<AEETurmaDto>> ObterQuantidadeDeferidos(int ano, long dreId, long ueId)
        {
            var sql = new StringBuilder(@"select t.modalidade_codigo as Modalidade, count(ea.id) as Quantidade, ");
            sql.Append("t.nome, t.ano as AnoTurma ");

            sql.Append(" from encaminhamento_aee ea ");
            sql.Append(" inner join turma t on ea.turma_id = t.id ");
            sql.Append(" inner join ue on t.ue_id = ue.id ");

            var where = new StringBuilder(@" where t.ano_letivo = @ano and situacao = 7 ");

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

        public async Task<bool> VerificaSeExisteEncaminhamentoPorAluno(string codigoEstudante, long ueId)
        {
            var sql = @"select exists (select 1 
                          from encaminhamento_aee ea 
                         inner join turma t on t.id = ea.turma_id                     
                         where not ea.excluido 
                           and ea.situacao not in (4, 5, 8, 10)
                           and ea.aluno_codigo = @codigoEstudante
                           and t.ue_id = @ueId
                           limit 1) ";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql, new { codigoEstudante, ueId });
        }

        public async Task<IEnumerable<EncaminhamentoAEECodigoArquivoDto>> ObterCodigoArquivoPorEncaminhamentoAEEId(long encaminhamentoId)
        {
            var sql = @"select
                            a.codigo
                        from
                            encaminhamento_aee ea
                        inner join encaminhamento_aee_secao eas on
                            ea.id = eas.encaminhamento_aee_id
                        inner join questao_encaminhamento_aee qea on
                            eas.id = qea.encaminhamento_aee_secao_id
                        inner join resposta_encaminhamento_aee rea on
                            qea.id = rea.questao_encaminhamento_id
                        inner join arquivo a on
                            rea.arquivo_id = a.id
                        where
                            ea.id = @encaminhamentoId";

            return await database.Conexao.QueryAsync<EncaminhamentoAEECodigoArquivoDto>(sql.ToString(), new { encaminhamentoId });
        }

        public async Task<IEnumerable<EncaminhamentoAEEVigenteDto>> ObterEncaminhamentosVigentes(long? anoLetivo = null)
        {
            string sql = @" select ea.id as encaminhamentoid,
                                        ea.aluno_codigo as alunocodigo,
                                        ea.turma_id as turmaid,
                                        t.turma_id as turmacodigo,
                                        t.ano_letivo as anoletivo,
                                        t.ue_id as ueid,
                                        u.ue_id as uecodigo
                                from encaminhamento_aee ea
                                    inner join turma t on (t.id = ea.turma_id)
                                    inner join ue u on (u.id = t.ue_id)
                                where not ea.excluido
                                and ea.situacao not in (5, 8, 10) ";

            if (anoLetivo.HasValue)
                sql += " and t.ano_letivo = @anoLetivo";

            sql += " order by ea.id";

            return await database.Conexao.QueryAsync<EncaminhamentoAEEVigenteDto>(sql, new { anoLetivo });
        }

        private string ObterQueryTotalEncaminhamento(string where, bool emAnalise)
        {
            var sql = new StringBuilder(@"select count(ea.id) as Quantidade from encaminhamento_aee ea ");
            sql.Append(" inner join turma t on ea.turma_id = t.id ");
            sql.Append(" inner join ue on t.ue_id = ue.id ");

            where += " and ea.situacao";

            if (emAnalise)
                where += " = ANY(@situacaoEmAnalise)";
            else
                where += $" <> {(int)SituacaoAEE.EncerradoAutomaticamente}";

            sql.Append(where);
            sql.Append(";");

            return sql.ToString();
        }
    }
}
