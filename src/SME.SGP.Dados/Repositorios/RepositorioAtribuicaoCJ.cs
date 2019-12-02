using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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

        public async Task<IEnumerable<AtribuicaoCJ>> ObterPorFiltros(Modalidade? modalidade, string turmaId, string ueId, string disciplinaId,
            string usuarioRf, string usuarioNome)
        {
            var query = new StringBuilder();

            query.AppendLine("select a.*, t.*");
            query.AppendLine("from");
            query.AppendLine("atribuicao_cj a");
            query.AppendLine("inner join turma t");
            query.AppendLine("on t.turma_id = a.turma_id");
            query.AppendLine("inner join usuario u");
            query.AppendLine("on u.rf_codigo = a.professor_rf");
            query.AppendLine("where 1 = 1");

            if (modalidade.HasValue)
                query.AppendLine("and a.modalidade = @modalidade");

            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine("and a.ue_id = @ueId");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and a.turma_id = @turmaId");

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and a.disciplina_id = @disciplinaId");

            if (!string.IsNullOrEmpty(usuarioRf))
                query.AppendLine("and a.professor_rf = @usuarioRf");

            if (!string.IsNullOrEmpty(usuarioNome))
            {
                usuarioNome = $"%{usuarioNome.ToUpper()}%";
                query.AppendLine("and upper(f_unaccent(u.nome)) LIKE @usuarioNome");
            }

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
                usuarioNome
            }, splitOn: "id,id"));
        }
    }
}