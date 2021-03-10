using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEE : RepositorioBase<PlanoAEE>, IRepositorioPlanoAEE
    {
        public RepositorioPlanoAEE(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, dreId, ueId, turmaId, alunoCodigo, situacao);

            var parametros = new { dreId, ueId, turmaId, alunoCodigo, situacao };
            var retorno = new PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<PlanoAEEAlunoTurmaDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private static string MontaQueryCompleta(Paginacao paginacao, long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, ueId, turmaId, alunoCodigo, situacao);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, contador: true, ueId, turmaId, alunoCodigo, situacao);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long ueId, long turmaId, string alunoCodigo, int? situacao)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, ueId, turmaId, alunoCodigo, situacao);

            if (!contador)
                sql.AppendLine(" order by pa.aluno_nome ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine("select ");
            if (contador)
                sql.AppendLine(" count(pa.id) ");
            else
            {
                sql.AppendLine(" pa.id ");
                sql.AppendLine(", pa.aluno_codigo as AlunoCodigo ");
                sql.AppendLine(", pa.aluno_numero as AlunoNumero ");
                sql.AppendLine(", pa.aluno_nome as AlunoNome ");
                sql.AppendLine(", t.turma_id as TurmaCodigo ");
                sql.AppendLine(", t.nome as TurmaNome ");
                sql.AppendLine(", t.modalidade_codigo as TurmaModalidade ");
                sql.AppendLine(", t.ano_letivo as TurmaAno ");
                sql.AppendLine(", CASE ");
                sql.AppendLine("    WHEN ea.id = 0 THEN 0 ");
                sql.AppendLine("    WHEN ea.id > 0  THEN 1 ");
                sql.AppendLine("  END as PossuiEncaminhamentoAEE ");
                sql.AppendLine(", pa.situacao ");
                sql.AppendLine(", pa.criado_em as CriadoEm ");
                sql.AppendLine(", pav.numero as Versao ");
                sql.AppendLine(", pav.criado_em as DataVersao ");
            }

            sql.AppendLine(" from plano_aee pa ");
            sql.AppendLine(" left join encaminhamento_aee ea on ea.aluno_codigo = pa.aluno_codigo and not ea.excluido and ea.situacao not in(4,5,7,8) ");
            sql.AppendLine(" inner join turma t on t.id = pa.turma_id");
            sql.AppendLine(" inner join ue on t.ue_id = ue.id");
            sql.AppendLine(" inner join plano_aee_versao pav on pav.id = (select max(id) from plano_aee_versao where plano_aee_id = pa.id)");
        }

        private static void ObtenhaFiltro(StringBuilder sql, long ueId, long turmaId, string alunoCodigo, int? situacao)
        {
            sql.AppendLine(" where ue.dre_id = @dreId and not pa.excluido ");

            if (ueId > 0)
                sql.AppendLine(" and ue.id = @ueId ");
            if (turmaId > 0)
                sql.AppendLine(" and t.id = @turmaId ");
            if (!string.IsNullOrEmpty(alunoCodigo))
                sql.AppendLine(" and pa.aluno_codigo = @alunoCodigo ");
            if (situacao.HasValue && situacao > 0)
                sql.AppendLine(" and pa.situacao = @situacao ");
        }

        public async Task<PlanoAEEResumoDto> ObterPlanoPorEstudante(string codigoEstudante)
        {
            var query = @"select distinct   pa.Id,
	                                        pa.aluno_numero as numero,
	                                        pa.aluno_nome as nome,
	                                        tu.nome as turma,
	                                        pa.situacao 
                                        from plano_aee pa
                                        inner join turma tu on tu.id = pa.turma_id 
                                        where pa.aluno_codigo = @codigoEstudante 
                                        and pa.situacao = 1
                                        limit 1";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAEEResumoDto>(query, new { codigoEstudante });
        }

        public async Task<PlanoAEE> ObterPlanoComTurmaPorId(long planoId)
        {
            var query = @" select pa.*, t.*
                            from plano_aee pa
                           inner join turma t on t.id = pa.turma_id
                           where pa.id = @planoId";

            return (await database.Conexao.QueryAsync<PlanoAEE, Turma, PlanoAEE>(query,
                (planoAEEDto, turma) =>
                {
                    planoAEEDto.Turma = turma;
                    return planoAEEDto;
                }, new { planoId })).FirstOrDefault();
        }

        public async Task<IEnumerable<PlanoAEE>> ObterPlanosAtivos()
        {
            var query = @"select * from plano_aee where not excluido and situacao not in (2,3)";

            return await database.Conexao.QueryAsync<PlanoAEE>(query);
        }

        public async Task<int> AtualizarSituacaoPlanoPorVersao(long versaoId, int situacao)
        {
            var query = @"update plano_aee
                           set situacao = @situacao
                          where id in (select plano_aee_id from plano_aee_versao where id = @versaoId) ";

            return await database.Conexao.ExecuteAsync(query, new { versaoId, situacao });
        }
    }
}
