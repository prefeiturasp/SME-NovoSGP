using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtividadeAvaliativaDisciplina : RepositorioBase<AtividadeAvaliativaDisciplina>, IRepositorioAtividadeAvaliativaDisciplina
    {
        public RepositorioAtividadeAvaliativaDisciplina(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<AtividadeAvaliativaDisciplina>> ListarPorIdAtividade(long atividadeAvaliativaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT id,");
            query.AppendLine("atividade_avaliativa_id,");
            query.AppendLine("disciplina_id,");
            query.AppendLine("criado_em,");
            query.AppendLine("criado_por,");
            query.AppendLine("alterado_em,");
            query.AppendLine("alterado_por,");
            query.AppendLine("criado_rf,");
            query.AppendLine("alterado_rf,");
            query.AppendLine("excluido");
            query.AppendLine("FROM atividade_avaliativa_disciplina");
            query.AppendLine("WHERE atividade_avaliativa_id = @atividadeAvaliativaId");
            query.AppendLine("AND excluido = false");

            return await database.Conexao.QueryAsync<AtividadeAvaliativaDisciplina>(query.ToString(), new
            {
                atividadeAvaliativaId
            });
        }

        public async Task<IEnumerable<AtividadeAvaliativaDisciplina>> ObterAvaliacoesBimestrais(long tipoCalendarioId, string turmaId, string disciplinaId, int bimestre)
        {
            var query = @"select aad.* 
                        from atividade_avaliativa_disciplina aad 
                       inner join atividade_avaliativa aa on aa.id = aad.atividade_avaliativa_id 
                       inner join tipo_avaliacao t on t.id = aa.tipo_avaliacao_id and t.codigo = 1
                       inner join periodo_escolar p on p.periodo_inicio <= aa.data_avaliacao and p.periodo_fim >= aa.data_avaliacao 
                       where not aa.excluido 
                        and aad.disciplina_id = @disciplinaId
                        and aa.turma_id = @turmaId
                        and p.bimestre = @bimestre
                        and p.tipo_calendario_id = @tipoCalendarioId ";

            return await database.Conexao.QueryAsync<AtividadeAvaliativaDisciplina>(query, new { tipoCalendarioId, disciplinaId, turmaId, bimestre });
        }
        public bool PossuiDisciplinas(long atividadeAvaliativaId, string disciplinaId)
        {
            var query = new StringBuilder();
            query.AppendLine("SELECT 1 FROM atividade_avaliativa_disciplina");
            query.AppendLine("WHERE atividade_avaliativa_id = @atividadeAvaliativaId");
            query.AppendLine("AND disciplina_id = @disciplinaId");
            query.AppendLine("AND excluido = false");

            return database.Query<bool>(query.ToString(), new { atividadeAvaliativaId, disciplinaId }).SingleOrDefault();
        }
    }
}
