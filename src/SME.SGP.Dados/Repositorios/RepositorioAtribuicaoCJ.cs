using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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

        public IEnumerable<AtribuicaoCJ> ObterAtribuicaoAtiva(string professorRf)
        {
            var query = @"select id, disciplina_id, dre_id, ue_id, professor_rf, turma_id, modalidade, substituir,
                            criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, migrado
                            from atribuicao_cj where professor_rf = @professorRf and substituir = true";

            var parametros = new { professorRf };

            return database.Query<AtribuicaoCJ>(query, parametros);
        }

        public async Task<IEnumerable<AtribuicaoCJ>> ObterPorFiltros(Modalidade? modalidade, string turmaId, string ueId, long disciplinaId,
            string usuarioRf, string usuarioNome, bool? substituir, string dreCodigo = "", string[] turmaIds = null, int? anoLetivo = null)
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
                query.AppendLine("and a.modalidade = @modalidade");

            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine("and a.ue_id = @ueId");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and a.turma_id = @turmaId");

            if (disciplinaId > 0)
                query.AppendLine("and a.disciplina_id = @disciplinaId");

            if (!string.IsNullOrEmpty(usuarioRf))
                query.AppendLine("and a.professor_rf = @usuarioRf");

            if (!string.IsNullOrEmpty(usuarioNome))
            {
                usuarioNome = $"%{usuarioNome.ToUpper()}%";
                query.AppendLine("and upper(f_unaccent(u.nome)) LIKE @usuarioNome");
            }

            if (substituir.HasValue)
                query.AppendLine("and a.substituir = @substituir");

            if (!string.IsNullOrEmpty(dreCodigo))
                query.AppendLine("and a.dre_id = @dreCodigo");

            if (turmaIds != null)
                query.AppendLine("and t.turma_id = ANY(@turmaIds)");

            if (anoLetivo != null)
                query.AppendLine("and t.ano_letivo = @anoLetivo");

            return (await database.Conexao.QueryAsync<AtribuicaoCJ, Turma, AtribuicaoCJ>(query.ToString(), (atribuicaoCJ, turma) =>
            {
                atribuicaoCJ.Turma = turma;
                return atribuicaoCJ;
            }, new
            {
                modalidade = modalidade.HasValue ? (int)modalidade : 0,
                ueId,
                turmaId,
                disciplinaId,
                usuarioRf,
                usuarioNome,
                substituir,
                dreCodigo,
                turmaIds,
                anoLetivo
            }, splitOn: "id,id"));
        }
    }
}