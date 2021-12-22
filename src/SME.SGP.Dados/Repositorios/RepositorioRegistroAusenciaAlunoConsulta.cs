using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroAusenciaAlunoConsulta : IRepositorioRegistroAusenciaAlunoConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioRegistroAusenciaAlunoConsulta(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<int> ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string disciplinaId, params string[] turmasId)
        {
            String query = BuildQueryObterTotalAulasPorDisciplinaETurma(disciplinaId);
            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new { dataAula, disciplinaId, turmasId });
        }

        private String BuildQueryObterTotalAulasPorDisciplinaETurma(string disciplinaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select ");
            query.AppendLine("COALESCE(SUM(a.quantidade),0) AS total");
            query.AppendLine("from ");
            query.AppendLine("aula a ");
            query.AppendLine("inner join registro_frequencia rf on ");
            query.AppendLine("rf.aula_id = a.id ");
            query.AppendLine("inner join periodo_escolar p on ");
            query.AppendLine("a.tipo_calendario_id = p.tipo_calendario_id ");
            query.AppendLine("where not a.excluido");
            query.AppendLine("and p.periodo_inicio <= @dataAula ");
            query.AppendLine("and p.periodo_fim >= @dataAula ");
            query.AppendLine("and a.data_aula >= p.periodo_inicio");
            query.AppendLine("and a.data_aula <= p.periodo_fim ");

            if (!string.IsNullOrWhiteSpace(disciplinaId))
                query.AppendLine("and a.disciplina_id = @disciplinaId ");

            query.AppendLine("and a.turma_id = any(@turmasId) ");
            return query.ToString();
        }
    }
}
