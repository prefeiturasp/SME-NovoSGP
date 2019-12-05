using Dapper;
using SME.SGP.Dados.Contexto;
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
        private readonly string fromCompleto = "from atividade_avaliativa a inner join tipo_avaliacao ta on a.tipo_avaliacao_id = ta.id";

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

        public async Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesPorDia(string dreId, string ueId, DateTime dataAvaliacao, string professorRf, string turmaId)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, dataAvaliacao, dreId, ueId, null, null, turmaId, professorRf);

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
            MontaWhere(query, null, dreId, ueId, null, null, turmaId, professorRf, null, null, false, null, null, mes, ano);

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

        public async Task<bool> VerificarSeExisteAvaliacao(DateTime dataAvaliacao, string ueId, string turmaId, string professorRf, string disciplinaId)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, dataAvaliacao, null, ueId, null, null, turmaId, professorRf, null, null, false, disciplinaId);

            var resultado = (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                dataAvaliacao,
                disciplinaId,
                ueId,
                turmaId,
                professorRf
            }));

            return resultado.Any();
        }

        public async Task<bool> VerificarSeJaExisteAvaliacaoComMesmoNome(string nomeAvaliacao, string dreId, string ueId, string turmaId, string professorRf, DateTime periodoInicio, DateTime periodoFim)
        {
            nomeAvaliacao = nomeAvaliacao.ToLowerInvariant();
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, null, dreId, ueId, nomeAvaliacao, null, turmaId, professorRf, periodoInicio, periodoFim, true);

            var resultado = (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                dreId,
                ueId,
                nomeAvaliacao,
                turmaId,
                professorRf,
                periodoInicio,
                periodoFim
            }));

            return resultado.Any();
        }

        public async Task<bool> VerificarSeJaExisteAvaliacaoNaoRegencia(DateTime dataAvaliacao, string dreId, string ueId, string turmaId, string professorRf)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, dataAvaliacao, dreId, ueId, null, null, turmaId, professorRf, null, null, false, null, false); ;

            var resultado = (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                dataAvaliacao,
                dreId,
                ueId,
                turmaId,
                professorRf
            }));

            return resultado.Any();
        }

        public async Task<bool> VerificarSeJaExisteAvaliacaoRegencia(DateTime dataAvaliacao, string dreId, string ueId, string turmaId, string disciplinaId, string professorRf)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryCabecalho(query);
            query.AppendLine(fromCompleto);
            MontaWhere(query, dataAvaliacao, dreId, ueId, null, null, turmaId, professorRf, null, null, false, disciplinaId, true);

            var resultado = (await database.Conexao.QueryAsync<AtividadeAvaliativa>(query.ToString(), new
            {
                dataAvaliacao,
                dreId,
                ueId,
                turmaId,
                professorRf,
                disciplinaId
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

        private static void MontaQueryCabecalho(StringBuilder query)
        {
            query.AppendLine("select");
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
            query.AppendLine("a.disciplina_id,");
            query.AppendLine("a.disciplina_contida_regencia_id,");
            query.AppendLine("ta.id as TipoAvaliacaoId,");
            query.AppendLine("ta.id,");
            query.AppendLine("ta.nome,");
            query.AppendLine("ta.descricao,");
            query.AppendLine("ta.situacao");
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
            bool? ehRegencia = null,
            int? mes = null,
            int? ano = null)
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
            if (!string.IsNullOrEmpty(disciplinaId))
            {
                if (ehRegencia.HasValue && ehRegencia.Value)
                    query.AppendLine("and a.disciplina_contida_regencia_id = @disciplinaId");
                else
                    query.AppendLine("and a.disciplina_id = @disciplinaId");
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
        }
    }
}