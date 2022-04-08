using Npgsql;
using NpgsqlTypes;
using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaDiarioBordo : RepositorioBase<PendenciaDiarioBordo>, IRepositorioPendenciaDiarioBordo
    {
        public RepositorioPendenciaDiarioBordo(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task Excluir(long pendenciaId)
        {
            await database.Conexao.ExecuteScalarAsync("delete from pendencia_diario_bordo where id = @pendenciaId", new { pendenciaId }, commandTimeout: 60);
        }

        public async Task<IEnumerable<Aula>> ListarPendenciasDiario(long dreId, int anoLetivo)
        {
            var listaRetorno = new List<Aula>();
            var sqlQuery = new StringBuilder();

            sqlQuery.AppendLine("select distinct a.id as Id,");
            sqlQuery.AppendLine("                a.disciplina_id DisciplinaId,");
            sqlQuery.AppendLine("                a.turma_id TurmaId,");
            sqlQuery.AppendLine("                a.professor_rf ProfessorRf,");
            sqlQuery.AppendLine("                t.id Id,");
            sqlQuery.AppendLine("                t.modalidade_codigo ModalidadeCodigo");
            sqlQuery.AppendLine("  	from aula a");
            sqlQuery.AppendLine("  		inner join tipo_calendario tc");
            sqlQuery.AppendLine("  			on tc.ano_letivo = @anoLetivo and a.tipo_calendario_id = tc.id");

            sqlQuery.AppendLine("  		inner join turma t");
            sqlQuery.AppendLine("  			on tc.ano_letivo = t.ano_letivo and a.turma_id = t.turma_id and t.modalidade_codigo = @modalidadeCodigo");
            sqlQuery.AppendLine("  		inner join ue");
            sqlQuery.AppendLine("  			on t.ue_id = ue.id");

            sqlQuery.AppendLine($"  	left join diario_bordo db");
            sqlQuery.AppendLine($"  		on db.aula_id = a.id");
            sqlQuery.AppendLine($"  	left join pendencia_diario_bordo pdb");
            sqlQuery.AppendLine($"  		on pdb.aula_id = a.id");

            sqlQuery.AppendLine("  where not a.excluido");
            sqlQuery.AppendLine("	and a.data_aula < @hoje");
            sqlQuery.AppendLine("	and ue.dre_id = @dreId");

            sqlQuery.AppendLine("	and pdb.id is null");
            sqlQuery.AppendLine("	and db.id is null");

            return await database.Conexao.QueryAsync<Aula, Turma, Aula>(sqlQuery.ToString(), (aula, turma) =>
            {
                aula.Turma = turma;
                return aula;
            }, new
            {
                anoLetivo,
                hoje = DateTime.Today.Date,
                dreId,
                modalidadeCodigo = Modalidade.EducacaoInfantil
            }, splitOn: "Id", commandTimeout: 200);
        }
    }
}
