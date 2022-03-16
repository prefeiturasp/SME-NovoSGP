using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequencia : RepositorioBase<RegistroFrequencia>, IRepositorioFrequencia
    {
        public RepositorioFrequencia(ISgpContext database) : base(database)
        {
        }

        public async Task ExcluirFrequenciaAula(long aulaId)
        {
            // Exclui registros de ausencia do aluno
            var command = @"update registro_frequencia_aluno
                                set excluido = true
                            where not excluido
                              and registro_frequencia_id in (
                                select id from registro_frequencia
                                 where not excluido
                                   and aula_id = @aulaId)";
            await database.ExecuteAsync(command, new { aulaId });

            // Exclui registro de frequencia da aula
            command = @"update registro_frequencia
                            set excluido = true
                        where not excluido
                          and aula_id = @aulaId ";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public async Task SalvarConciliacaoTurma(string turmaId, string disciplinaId, DateTime dataReferencia, string alunos)
        {
            var query = @"insert into conciliacao_turma (turma_id, disciplina_id, data_referencia, alunos) 
                          values (@turmaId, @disciplinaId, @dataReferencia, @alunos)";

            await database.Conexao.ExecuteAsync(query, new { turmaId, disciplinaId, dataReferencia, alunos });
        }
    }
}