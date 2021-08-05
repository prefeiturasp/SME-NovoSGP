using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtribuicaoCJ : RepositorioBase<AtribuicaoCJ>, IRepositorioAtribuicaoCJ
    {
        public RepositorioAtribuicaoCJ(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<int>> ObterAnosDisponiveis(string professorRF, bool consideraHistorico)
        {
            var query = @"select distinct ano_letivo from atribuicao_esporadica ae 
                    where not ae.excluido and ae.professor_rf = @professorRF ";
            if (consideraHistorico)
                query += $" and ae.ano_letivo < {DateTime.Now.Year}";
            
            query += @"union 
                            select t.ano_letivo from atribuicao_cj ac
                            inner join turma t on ac.turma_id = t.turma_id where ac.professor_rf = @professorRF";
            if (consideraHistorico)
                query += $" and t.ano_letivo < {DateTime.Now.Year} ";

            return await database.QueryAsync<int>(query, new { professorRF });
        }

        public IEnumerable<AtribuicaoCJ> ObterAtribuicaoAtiva(string professorRf)
        {
            var query = @"select id, disciplina_id, dre_id, ue_id, professor_rf, turma_id, modalidade, substituir,
                            criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, migrado
                            from atribuicao_cj where professor_rf = @professorRf and substituir = true";

            var parametros = new { professorRf };

            return database.Query<AtribuicaoCJ>(query, parametros);
        }

        public async Task<IEnumerable<AtribuicaoCJ>> ObterAtribuicaoAtivaAsync(string professorRf)
        {
            var query = @"select id, disciplina_id, dre_id, ue_id, professor_rf, turma_id, modalidade, substituir,
                            criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, migrado
                            from atribuicao_cj where professor_rf = @professorRf and substituir = true";
            return await database.QueryAsync<AtribuicaoCJ>(query, new { professorRf });
        }

        public async Task<IEnumerable<AtribuicaoCJ>> ObterPorFiltros(Modalidade? modalidade, string turmaId, string ueId, long componenteCurricularId,
            string usuarioRf, string usuarioNome, bool? substituir, string dreCodigo = "", string[] turmaCodigos = null, int? anoLetivo = 0, bool? historico = null)
        {
            var query = new StringBuilder();

            query.AppendLine("select a.*, t.*");
            query.AppendLine("from");
            query.AppendLine("atribuicao_cj a");
            query.AppendLine("inner join turma t");
            query.AppendLine("on t.turma_id = a.turma_id");
            query.AppendLine("left join usuario u");
            query.AppendLine("on u.rf_codigo = a.professor_rf");
            query.AppendLine("where 1 = 1");

            if (modalidade.HasValue)
                query.AppendLine(" and a.modalidade = @modalidade ");

            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine(" and a.ue_id = @ueId ");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine(" and a.turma_id = @turmaId ");

            if (componenteCurricularId > 0)
                query.AppendLine(" and a.disciplina_id = @componenteCurricularId ");

            if (!string.IsNullOrEmpty(usuarioRf))
                query.AppendLine(" and a.professor_rf = @usuarioRf ");

            if (!string.IsNullOrEmpty(usuarioNome))
            {
                usuarioNome = $"%{usuarioNome.ToUpper()}%";
                query.AppendLine(" and upper(f_unaccent(u.nome)) LIKE upper(f_unaccent(@usuarioNome)) ");
            }

            if (substituir.HasValue)
                query.AppendLine(" and a.substituir = @substituir ");

            if (!string.IsNullOrEmpty(dreCodigo))
                query.AppendLine(" and a.dre_id = @dreCodigo ");

            if (turmaCodigos != null)
                query.AppendLine(" and t.turma_id = ANY(@turmaCodigos) ");

            if (anoLetivo > 0)
                query.AppendLine(" and t.ano_letivo = @anoLetivo ");

            if (historico.HasValue)
                query.AppendLine(" and t.historica = @historico ");

            return (await database.Conexao.QueryAsync<AtribuicaoCJ, Turma, AtribuicaoCJ>(query.ToString(), (atribuicaoCJ, turma) =>
            {
                atribuicaoCJ.Turma = turma;
                return atribuicaoCJ;
            }, new
            {
                modalidade = modalidade.HasValue ? (int)modalidade : 0,
                ueId,
                turmaId,
                componenteCurricularId,
                usuarioRf,
                usuarioNome,
                substituir,
                dreCodigo,
                turmaCodigos,
                anoLetivo,
                historico
            }, splitOn: "id,id"));
        }

        public async Task<bool> PossuiAtribuicaoPorDreUeETurma(string turmaId, string dreCodigo, string ueCodigo, string professorRf)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"select distinct 1 from atribuicao_cj where dre_id = @dreCodigo and ue_id = @ueCodigo and professor_rf = @professorRf and turma_id = @turmaId;");

            return await database.Conexao.QuerySingleOrDefaultAsync<bool>(sql.ToString(), new { turmaId, dreCodigo, ueCodigo, professorRf });
        }

        public async Task<bool> RemoverRegistros(string dreCodigo, string ueCodigo, string turmaCodigo, string professorRf)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"delete from atribuicao_cj where dre_id = @dreCodigo and ue_id = @ueCodigo and professor_rf = @professorRf and turma_id = @turmaCodigo;");

            return await database.Conexao.ExecuteScalarAsync<bool>(sql.ToString(), new { turmaCodigo, dreCodigo, ueCodigo, professorRf });
        }
    }
}