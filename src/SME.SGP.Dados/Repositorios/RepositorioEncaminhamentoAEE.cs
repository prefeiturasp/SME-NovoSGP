using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoAEE : RepositorioBase<EncaminhamentoAEE>, IRepositorioEncaminhamentoAEE
    {
        public RepositorioEncaminhamentoAEE(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf, int anoLetivo, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, dreId, ueId, turmaId, alunoCodigo, situacao, responsavelRf, anoLetivo);

            var parametros = new { dreId, ueId, turmaId, alunoCodigo, situacao, responsavelRf, anoLetivo };
            var retorno = new PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<EncaminhamentoAEEAlunoTurmaDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private static string MontaQueryCompleta(Paginacao paginacao, long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf, int anoLetivo)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, ueId, turmaId, alunoCodigo, situacao, responsavelRf);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, ueId, turmaId, alunoCodigo, situacao, responsavelRf);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, ueId, turmaId, alunoCodigo, situacao, responsavelRf);

            if (!contador)
                sql.AppendLine(" order by ea.aluno_nome ");

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
                sql.AppendLine(", t.ano_letivo as TurmaAno ");
                sql.AppendLine(", ea.situacao ");
                sql.AppendLine(", u.nome as Responsavel ");
            }

            sql.AppendLine(" from encaminhamento_aee ea ");
            sql.AppendLine(" inner join turma t on t.id = ea.turma_id");
            sql.AppendLine(" inner join ue on t.ue_id = ue.id");
            sql.AppendLine("  left join usuario u on u.id = ea.responsavel_id");
        }

        private static void ObtenhaFiltro(StringBuilder sql, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf)
        {
            sql.AppendLine(" where ue.dre_id = @dreId and not ea.excluido ");
            sql.AppendLine("   and t.ano_letivo = @anoLetivo ");

            if (ueId > 0)
                sql.AppendLine(" and ue.id = @ueId ");
            if (turmaId > 0)
                sql.AppendLine(" and t.id = @turmaId ");
            if (!string.IsNullOrEmpty(alunoCodigo))
                sql.AppendLine(" and ea.aluno_codigo = @alunoCodigo ");
            if (situacao.HasValue && situacao > 0)
                sql.AppendLine(" and ea.situacao = @situacao ");
            if (!string.IsNullOrEmpty(responsavelRf))
                sql.AppendLine(" and u.rf_codigo = @responsavelRf ");

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
                if (secao == null)
                {
                    encaminhamentoSecao.SecaoEncaminhamentoAEE = secaoEncaminhamento;
                    secao = encaminhamentoSecao;
                    encaminhamento.Secoes.Add(secao);
                }

                var questaoEncaminhamento = secao.Questoes.FirstOrDefault(c => c.Id == questaoEncaminhamentoAEE.Id);
                if (questaoEncaminhamento == null)
                {
                    questaoEncaminhamento = questaoEncaminhamentoAEE;
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

        public async Task<EncaminhamentoAEE> ObterEncaminhamentoComTurmaPorId(long encaminhamentoId)
        {
            var query = @" select ea.*, t.*, u.*
                            from encaminhamento_aee ea
                           inner join turma t on t.id = ea.turma_id
                            left join usuario u on u.id = ea.responsavel_id 
                           where ea.id = @encaminhamentoId";

            return (await database.Conexao.QueryAsync<EncaminhamentoAEE, Turma, Usuario, EncaminhamentoAEE>(query,
                (encaminhamentoAEE, turma, usuario) =>
                {
                    encaminhamentoAEE.Turma = turma;
                    encaminhamentoAEE.Responsavel = usuario;
                    return encaminhamentoAEE;
                }, new { encaminhamentoId })).FirstOrDefault();
        }

        public async Task<EncaminhamentoAEEAlunoTurmaDto> ObterEncaminhamentoPorEstudante(string codigoEstudante)
        {
            var sql = @"select ea.id 
                            , ea.aluno_codigo as AlunoCodigo 
                            , ea.aluno_nome as AlunoNome 
                            , t.turma_id as TurmaCodigo 
                            , t.nome as TurmaNome 
                            , t.modalidade_codigo as TurmaModalidade 
                            , t.ano_letivo as TurmaAno 
                            , ea.situacao 
                         from encaminhamento_aee ea 
                         inner join turma t on t.id = ea.turma_id
                         inner join ue on t.ue_id = ue.id 
                     where not ea.excluido 
                       and ea.situacao not in (4, 5, 8)
                       and ea.aluno_codigo = @codigoEstudante ";

            return await database.Conexao.QueryFirstOrDefaultAsync<EncaminhamentoAEEAlunoTurmaDto>(sql, new { codigoEstudante });
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterResponsaveis(long dreId, long ueId, long turmaId, string alunoCodigo, int anoLetivo, int? situacao)
        {
            var sql = new StringBuilder(@"select distinct u.rf_codigo as CodigoRf
	                                    , u.nome as NomeServidor
                                      from encaminhamento_aee ea 
                                     inner join turma t on t.id = ea.turma_id
                                     inner join ue on t.ue_id = ue.id
                                     inner join usuario u on u.id = ea.responsavel_id ");

            ObtenhaFiltro(sql, ueId, turmaId, alunoCodigo, situacao, "");

            return await database.Conexao.QueryAsync<UsuarioEolRetornoDto>(sql.ToString(), new { dreId, ueId, turmaId, alunoCodigo, situacao, anoLetivo });
        }

        public async Task<IEnumerable<AEESituacaoEncaminhamentoDto>> ObterQuantidadeSituacoes(int ano, long dreId, long ueId)
        {
            var sql = new StringBuilder(@"select situacao, count(ea.id) as Quantidade from encaminhamento_aee ea ");
            sql.Append(" inner join turma t on ea.turma_id = t.id ");
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

            sql.Append(" group by ea.situacao ");

            return await database.Conexao.QueryAsync<AEESituacaoEncaminhamentoDto>(sql.ToString(), new { ano, dreId, ueId });
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
    }
}
