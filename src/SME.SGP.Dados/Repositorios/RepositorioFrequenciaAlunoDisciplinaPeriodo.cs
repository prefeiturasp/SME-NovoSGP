using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaAlunoDisciplinaPeriodo : RepositorioBase<FrequenciaAlunoDisciplinaPeriodo>, IRepositorioFrequenciaAlunoDisciplinaPeriodo
    {
        public RepositorioFrequenciaAlunoDisciplinaPeriodo(ISgpContext database) : base(database)
        {
        }

        public FrequenciaAlunoDisciplinaPeriodo Obter(string codigoAluno, string disciplinaId, DateTime periodoInicio, DateTime periodoFim)
        {
            var query = @"select
	                        *
                        from
	                        frequencia_aluno_disciplina
                        where
	                        codigo_aluno = @codigoAluno
	                        and disciplina_id = @disciplinaId
	                        and periodo_inicio = @periodoInicio
	                        and periodo_fim = @periodoFim";
            return database.QueryFirstOrDefault<FrequenciaAlunoDisciplinaPeriodo>(query, new
            {
                codigoAluno,
                disciplinaId,
                periodoInicio,
                periodoFim
            });
        }
    }
}