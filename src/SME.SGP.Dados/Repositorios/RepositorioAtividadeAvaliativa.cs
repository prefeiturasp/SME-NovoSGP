using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtividadeAvaliativa : RepositorioBase<AtividadeAvaliativa>, IRepositorioAtividadeAvaliativa
    {
        private readonly string fromCompleto = @"from atividade_avaliativa a
                                                inner join tipo_avaliacao ta on a.tipo_avaliacao_id = ta.id
                                                inner join atividade_avaliativa_disciplina aad on aad.atividade_avaliativa_id = a.id";

        private readonly string fromCompletoRegencia = @"from atividade_avaliativa a
                                                        inner join tipo_avaliacao ta on a.tipo_avaliacao_id = ta.id
                                                        inner join atividade_avaliativa_regencia aar on a.id = aar.atividade_avaliativa_id
                                                        inner join atividade_avaliativa_disciplina aad on aad.atividade_avaliativa_id = a.id";

        public RepositorioAtividadeAvaliativa(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<PaginacaoResultadoDto<AtividadeAvaliativa>> Listar(DateTime? dataAvaliacao, string dreId, string ueId, string nomeAvaliacao, int? tipoAvaliacaoId, string turmaId, Paginacao paginacao)
        {
            if (!string.IsNullOrEmpty(nomeAvaliacao))
                nomeAvaliacao = $"%{nomeAvaliacao.ToLowerInvariant()}%";
            StringBuilder query = new StringBuilder();
            if (paginacao == null)
                paginacao = new Paginacao(1, 10);

            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, dataAvaliacao, dreId, ueId, nomeAvaliacao, tipoAvaliacaoId, turmaId, null);

            if (paginacao.QuantidadeRegistros != 0)
                query.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);

            var retornoPaginado = new PaginacaoResultadoDto<AtividadeAvaliativa>()
            {
                Items = await database.Conexao.QueryAsync<AtividadeAvaliativa, TipoAvaliacao, AtividadeAvaliativa>(query.ToString(), (atividadeAvaliativa, tipoAvaliacao) =>
                  {
                      atividadeAvaliativa.AdicionarTipoAvaliacao(tipoAvaliacao);
                      return atividadeAvaliativa;
                  }, new
                  {
                      dataAvaliacao,
                      dreId,
                      ueId,
                      nomeAvaliacao,
                      tipoAvaliacaoId,
                      turmaId
                  },
            splitOn: "AtividadeAvaliativaId,TipoAvaliacaoId")
            };

            var queryCountCabecalho = "select count(distinct a.id)";
            var queryCount = new StringBuilder(queryCountCabecalho);
            queryCount.AppendLine(fromCompleto);
            MontaWhere(queryCount, dataAvaliacao, dreId, ueId, nomeAvaliacao, tipoAvaliacaoId, turmaId, null);

            retornoPaginado.TotalRegistros = (await database.Conexao.QueryAsync<int>(queryCount.ToString(), new
            {
                dataAvaliacao,
                dreId,
                ueId,
                nomeAvaliacao,
                tipoAvaliacaoId,
                turmaId
            })).Sum();

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return retornoPaginado;
        }

        public IEnumerable<AtividadeAvaliativa> ListarPorIds(IEnumerable<long> ids)
        {
            var sql = new StringBuilder();

            MontaQueryCabecalho(sql, false);
            sql.AppendLine(fromCompleto);
            sql.AppendLine($"where a.id = ANY(@ids)");

            return database.Query<AtividadeAvaliativa>(sql.ToString(), new { ids = ids.ToArray() });
        }

        public async Task<IEnumerable<AtividadeAvaliativa>> ListarPorTurmaDisciplinaPeriodo(string turmaCodigo, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var sql = new StringBuilder();

            MontaQueryCabecalho(sql, false);
            sql.AppendLine(fromCompleto);
            sql.AppendLine("where a.excluido = false");
            sql.AppendLine("and a.turma_id = @turmaCodigo");
            sql.AppendLine("and a.data_avaliacao >= @inicioPeriodo and a.data_avaliacao <= @fimPeriodo");
            sql.AppendLine("and aad.disciplina_id = @disciplinaId");

            return await database.QueryAsync<AtividadeAvaliativa>(sql.ToString(), new { turmaCodigo, inicioPeriodo, fimPeriodo, disciplinaId });
        }

        public async Task<AtividadeAvaliativa> ObterAtividadeAvaliativa(DateTime dataAvaliacao, string disciplinaId, string turmaId, string ueId)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalhoSimples(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, dataAvaliacao, ueId: ueId, turmaId: turmaId, disciplinaId: disciplinaId);

            return (await database.Conexao.QueryFirstOrDefaultAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                dataAvaliacao,
                disciplinaId,
                turmaId,
                ueId
            }));
        }

        public IEnumerable<AtividadeAvaliativa> ObterAtividadesAvaliativasSemNotaParaNenhumAluno(string turmaCodigo, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            var sql = @"select av.*
                        from atividade_avaliativa av
                       inner join atividade_avaliativa_disciplina aad on aad.atividade_avaliativa_id = av.id
                        left join notas_conceito n on n.atividade_avaliativa = av.id
                       where av.turma_id = @turmaCodigo
	                     and aad.disciplina_id = @disciplinaId
                         and n.id is null";

            return database.Query<AtividadeAvaliativa>(sql.ToString(), new { turmaCodigo, disciplinaId, inicioPeriodo, fimPeriodo });
        }

        public async Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesPorDia(string dreId, string ueId, DateTime dataAvaliacao, string professorRf, string turmaId)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, dataAvaliacao, dreId, ueId, null, null, turmaId, professorRf);
            query.AppendLine("group by a.id, ta.id");

            return (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                dreId,
                ueId,
                professorRf,
                dataAvaliacao,
                turmaId
            }));
        }

        public async Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesPorMes(string dreId, string ueId, int mes, int ano, string professorRf, string turmaId)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, null, dreId, ueId, null, null, turmaId, professorRf, null, null, false, null, null, null, mes, ano);

            return (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                dreId,
                ueId,
                professorRf,
                mes,
                ano,
                turmaId
            }));
        }
        public async Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesCalendarioProfessorPorMes(string dreCodigo, string ueCodigo, int mes, int ano, string turmaCodigo)
        {
            var query = @"select aa.id, aa.tipo_avaliacao_id, aa.data_avaliacao, aad.id, aad.disciplina_id from atividade_avaliativa aa
                            inner join atividade_avaliativa_disciplina aad
                            on aad.atividade_avaliativa_id  = aa.id
                        where aa.dre_id  = @dreCodigo
                        and aa.ue_id  = @ueCodigo
                        and extract(month from aa.data_avaliacao) = @mes 
                        and extract(year from aa.data_avaliacao) = @ano
                        and aa.turma_id = @turmaCodigo";


            var lookup = new Dictionary<long, AtividadeAvaliativa>();

            await database.Conexao.QueryAsync<AtividadeAvaliativa, AtividadeAvaliativaDisciplina, AtividadeAvaliativa>(query, (atividadeAvaliativa, atividadeAvaliativaDisciplina) => {

                var retorno = new AtividadeAvaliativa();
                if (!lookup.TryGetValue(atividadeAvaliativa.Id, out retorno))
                {
                    retorno = atividadeAvaliativa;
                    lookup.Add(atividadeAvaliativa.Id, retorno);
                }

                retorno.Adicionar(atividadeAvaliativaDisciplina);

                return retorno;
            }, param: new
            {
                dreCodigo,
                ueCodigo,
                mes,
                ano,
                turmaCodigo
            });

            return lookup.Values;
        }

        public async Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesCalendarioProfessorPorMesDia(string dreCodigo, string ueCodigo, string turmaCodigo, string codigoRf, DateTime dataReferencia)
        {
            var query = @"select aa.id, aa.nome_avaliacao, aa.tipo_avaliacao_id, aa.data_avaliacao, aad.id, aad.disciplina_id from atividade_avaliativa aa
                            inner join atividade_avaliativa_disciplina aad
                            on aad.atividade_avaliativa_id  = aa.id
                        where not aa.excluido
                        and aa.dre_id  = @dreCodigo
                        and aa.ue_id  = @ueCodigo
                        and aa.professor_rf = @codigoRf    
                        and aa.data_avaliacao ::date = @dataReferencia
                        and aa.turma_id = @turmaCodigo    ";
                        


            var lookup = new Dictionary<long, AtividadeAvaliativa>();

            await database.Conexao.QueryAsync<AtividadeAvaliativa, AtividadeAvaliativaDisciplina, AtividadeAvaliativa>(query, (atividadeAvaliativa, atividadeAvaliativaDisciplina) => {

                var retorno = new AtividadeAvaliativa();
                if (!lookup.TryGetValue(atividadeAvaliativa.Id, out retorno))
                {
                    retorno = atividadeAvaliativa;
                    lookup.Add(atividadeAvaliativa.Id, retorno);
                }

                retorno.Adicionar(atividadeAvaliativaDisciplina);

                return retorno;
            }, param: new
            {
                dreCodigo,
                ueCodigo,
                dataReferencia,
                turmaCodigo,
                codigoRf
            });

            return lookup.Values;
        }
        

        public async Task<bool> VerificarSeExisteAvaliacao(DateTime dataAvaliacao, string ueId, string turmaId, string professorRf, string disciplinaId)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, dataAvaliacao, null, ueId, null, null, turmaId, professorRf, null, null, false, disciplinaId);

            var resultado = await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                dataAvaliacao,
                disciplinaId,
                ueId,
                turmaId,
                professorRf
            });

            return resultado.Any();
        }

        public async Task<bool> VerificarSeJaExisteAvaliacaoComMesmoNome(string nomeAvaliacao, string dreId, string ueId, string turmaId, string[] disciplinasId, string professorRf, DateTime periodoInicio, DateTime periodoFim, long? id)
        {
            nomeAvaliacao = nomeAvaliacao.ToLowerInvariant();
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, null, dreId, ueId, nomeAvaliacao, null, turmaId, professorRf, periodoInicio, periodoFim, true, null, disciplinasId, null, null, null, id, id.HasValue);

            var resultado = (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                id,
                dreId,
                ueId,
                nomeAvaliacao,
                turmaId,
                disciplinasId,
                professorRf,
                periodoInicio,
                periodoFim
            }));

            return resultado.Any();
        }

        public async Task<bool> VerificarSeJaExisteAvaliacaoNaoRegencia(DateTime dataAvaliacao, string dreId, string ueId, string turmaId, string[] disciplinasId, string professorRf, long? id)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, dataAvaliacao, dreId, ueId, null, null, turmaId, professorRf, null, null, false, null, disciplinasId, false, null, null, id, id.HasValue); ;

            var resultado = (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                id,
                dataAvaliacao,
                dreId,
                ueId,
                turmaId,
                disciplinasId,
                professorRf
            }));

            return resultado.Any();
        }

        public async Task<bool> VerificarSeJaExisteAvaliacaoRegencia(DateTime dataAvaliacao, string dreId, string ueId, string turmaId, string[] disciplinasId, string[] disciplinasContidaId, string professorRf, long? id)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompletoRegencia);
            MontaWhere(query, dataAvaliacao, dreId, ueId, null, null, turmaId, professorRf, null, null, false, null, disciplinasId, true, null, null, id, id.HasValue);
            MontaWhereRegencia(query);
            var resultado = (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                id,
                dataAvaliacao,
                dreId,
                ueId,
                turmaId,
                professorRf,
                disciplinasId,
                disciplinasContidaId
            }));

            return resultado.Any();
        }

        public async Task<bool> VerificarSeJaExistePorTipoAvaliacao(long tipoAvaliacaoId)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query: query, tipoAvaliacaoId: tipoAvaliacaoId);

            var resultado = (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                tipoAvaliacaoId
            }));

            return resultado.Any();
        }

        private static void MontaQueryCabecalho(StringBuilder query, bool listagem = true)
        {
            query.AppendLine("select");

            if (listagem)
                query.AppendLine("a.id as AtividadeAvaliativaId,");

            query.AppendLine("a.id,");
            query.AppendLine("a.dre_id,");
            query.AppendLine("a.ue_id,");
            query.AppendLine("a.professor_rf,");
            query.AppendLine("a.turma_id,");
            query.AppendLine("a.categoria_id,");
            query.AppendLine("a.tipo_avaliacao_id,");
            query.AppendLine("a.nome_avaliacao,");
            query.AppendLine("a.descricao_avaliacao,");
            query.AppendLine("a.data_avaliacao,");
            query.AppendLine("a.criado_em,");
            query.AppendLine("a.criado_por,");
            query.AppendLine("a.alterado_em,");
            query.AppendLine("a.alterado_por,");
            query.AppendLine("a.criado_rf,");
            query.AppendLine("a.alterado_rf,");
            query.AppendLine("a.excluido,");
            query.AppendLine("a.eh_regencia,");
            query.AppendLine("ta.id as TipoAvaliacaoId,");
            query.AppendLine("ta.nome,");
            query.AppendLine("ta.descricao,");
            query.AppendLine("ta.situacao");

            if (listagem)
            {
                query.AppendLine(",");
                query.AppendLine("ta.id as TipoAvaliacaoId,");
                query.AppendLine("ta.nome,");
                query.AppendLine("ta.descricao,");
                query.AppendLine("ta.situacao");
            }
        }

        private static void MontaQueryCabecalhoSimples(StringBuilder query)
        {
            query.AppendLine("select");
            query.AppendLine("a.id,");
            query.AppendLine("a.dre_id,");
            query.AppendLine("a.ue_id,");
            query.AppendLine("a.professor_rf,");
            query.AppendLine("a.turma_id,");
            query.AppendLine("a.categoria_id,");
            query.AppendLine("a.tipo_avaliacao_id,");
            query.AppendLine("a.nome_avaliacao,");
            query.AppendLine("a.descricao_avaliacao,");
            query.AppendLine("a.data_avaliacao");
        }

        private void MontaWhere(StringBuilder query,
            DateTime? dataAvaliacao = null,
            string dreId = null,
            string ueId = null,
            string nomeAvaliacao = null,
            long? tipoAvaliacaoId = null,
            string turmaId = null,
            string professorRf = null,
            DateTime? perioInicio = null,
            DateTime? periodoFim = null,
            bool nomeExato = false,
            string disciplinaId = null,
            string[] disciplinasId = null,
            bool? ehRegencia = null,
            int? mes = null,
            int? ano = null,
            long? id = null,
            bool ehAlteracao = false)
        {
            query.AppendLine("where");
            query.AppendLine("a.excluido = false");
            query.AppendLine("and ta.situacao = true");
            if (dataAvaliacao.HasValue)
                query.AppendLine("and date(a.data_avaliacao) = @dataAvaliacao");
            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine("and a.dre_id = @dreId");
            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine("and a.ue_id = @ueId");
            if (!string.IsNullOrEmpty(nomeAvaliacao))
                if (nomeExato)
                    query.AppendLine("and  lower(f_unaccent(a.nome_avaliacao)) = f_unaccent(@nomeAvaliacao)");
                else
                    query.AppendLine("and  lower(f_unaccent(a.nome_avaliacao)) LIKE f_unaccent(@nomeAvaliacao)");
            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and a.turma_id = @turmaId");
            if (tipoAvaliacaoId.HasValue)
                query.AppendLine("and ta.id = @tipoAvaliacaoId");
            if (!string.IsNullOrEmpty(professorRf))
                query.AppendLine("and a.professor_rf = @professorRf");
            if (perioInicio.HasValue)
                query.AppendLine("and date(a.data_avaliacao) >= @periodoInicio");
            if (periodoFim.HasValue)
                query.AppendLine("and date(a.data_avaliacao) <= @periodoFim");
            if (disciplinasId != null && disciplinasId.Length > 0)
            {
                query.AppendLine("and aad.disciplina_id =  ANY(@disciplinasId)");
                query.AppendLine("and aad.excluido =  false");
            }
            if (!String.IsNullOrEmpty(disciplinaId))
            {
                query.AppendLine("and aad.disciplina_id =  @disciplinaId");
                query.AppendLine("and aad.excluido =  false");
            }
            if (ehRegencia.HasValue)
            {
                if (ehRegencia.Value)
                    query.AppendLine("and a.eh_regencia = true");
                else
                    query.AppendLine("and a.eh_regencia = false");
            }
            if (mes.HasValue)
                query.AppendLine("AND extract(month from a.data_avaliacao) = @mes");
            if (ano.HasValue)
                query.AppendLine("AND extract(year from a.data_avaliacao) = @ano");
            if (id.HasValue)
                if (ehAlteracao)
                    query.AppendLine("AND a.id <> @id");
                else
                    query.AppendLine("AND a.id = @id");
        }

        private void MontaWhereRegencia(StringBuilder query)
        {
            query.AppendLine("AND aar.disciplina_contida_regencia_id = ANY(@disciplinasContidaId)");
        }
    }
}