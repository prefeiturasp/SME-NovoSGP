using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequencia : RepositorioBase<RegistroFrequencia>, IRepositorioFrequencia
    {
        public RepositorioFrequencia(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExcluirFrequenciaAula(long aulaId)
        {
            var command = @"update registro_frequencia_aluno
                                set excluido = true
                            where not excluido
                                and aula_id  in (@aulaId) ";
            await database.ExecuteAsync(command, new { aulaId });

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