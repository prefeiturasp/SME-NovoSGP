using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoAEE : RepositorioBase<EncaminhamentoAEE>, IRepositorioEncaminhamentoAEE
    {
        public RepositorioEncaminhamentoAEE(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, TipoOrdenacaoEstudante ordenacao, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, dreId, ueId, turmaId, alunoCodigo, situacao, ordenacao);

            var parametros = new { dreId, ueId, turmaId, alunoCodigo, situacao };
            var retorno = new PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<EncaminhamentoAEEAlunoTurmaDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private static string MontaQueryCompleta(Paginacao paginacao, long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, TipoOrdenacaoEstudante ordenacao)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, ueId, turmaId, alunoCodigo, situacao, ordenacao);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, ueId, turmaId, alunoCodigo, situacao, ordenacao);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long ueId, long turmaId, string alunoCodigo, int? situacao, TipoOrdenacaoEstudante ordenacao)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, ueId, turmaId, alunoCodigo, situacao);

            if (!contador)
                sql.AppendLine("order by "+ ObterOrdenacao(ordenacao));

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY");
        }

        private static string ObterOrdenacao(TipoOrdenacaoEstudante ordenacao)
        {
            switch (ordenacao)
            {
                case TipoOrdenacaoEstudante.NumeroAsc:
                    return "ea.aluno_codigo";
                case TipoOrdenacaoEstudante.NumeroDesc:
                    return "ea.aluno_codigo desc";
                case TipoOrdenacaoEstudante.NomeAsc:
                    return "ea.aluno_nome";
                case TipoOrdenacaoEstudante.NomeDesc:
                    return "ea.aluno_nome desc";
                default:
                    throw new NegocioException("Tipo de ordenação não identificado para a consulta de Encaminhamentos AEE");
            }
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
            if (contador)
                sql.AppendLine("count(ea.id)");
            else
            {
                sql.AppendLine(" ea.id ");
                sql.AppendLine(", ea.aluno_codigo as AlunoCodigo ");
                sql.AppendLine(", t.turma_id as TurmaCodigo");
                sql.AppendLine(", t.nome as TurmaNome");
                sql.AppendLine(", t.modalidade_codigo as TurmaModalidade");
                sql.AppendLine(", t.ano_letivo as TurmaAno");
                sql.AppendLine(", ea.situacao");
            }

            sql.AppendLine("from encaminhamento_aee ea ");
            sql.AppendLine("inner join turma t on t.id = ea.turma_id");
            sql.AppendLine("inner join ue on t.ue_id = ue.id");
        }

        private static void ObtenhaFiltro(StringBuilder sql, long ueId, long turmaId, string alunoCodigo, int? situacao)
        {
            sql.AppendLine("where ue.dre_id = @dreId ");

            if (ueId > 0)
                sql.AppendLine("and ue.id = @ueId");
            if (turmaId > 0)
                sql.AppendLine("and t.id = @turmaId");
            if (!string.IsNullOrEmpty(alunoCodigo))
                sql.AppendLine("and ea.aluno_codigo = @alunoCodigo");
            if (situacao.HasValue && situacao > 0)
                sql.AppendLine("and ea.situacao = @situacao");
        }

        public async Task<SituacaoAEE> ObterSituacaoEncaminhamentoAEE(long encaminhamentoAEEId)
        {
            var query = "select situacao from encaminhamento_aee ea where id = @encaminhamentoAEEId";

            return await database.Conexao.QueryFirstOrDefaultAsync<SituacaoAEE>(query, new { encaminhamentoAEEId });
        }

    }
}
